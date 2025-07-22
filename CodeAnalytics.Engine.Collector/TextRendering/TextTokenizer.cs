using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.TextRendering.Themes;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Engine.Extensions.Symbols;
using CodeAnalytics.Engine.Extensions.System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeAnalytics.Engine.Collector.TextRendering;

public sealed class TextTokenizer
{
   private readonly CollectContext _context;
   private readonly CodeTheme _theme;

   public TextTokenizer(
      CollectContext context,
      CodeTheme theme)
   {
      _context = context;
      _theme = theme;
   }

   public async Task<PooledList<SyntaxSpan>> Tokenize(CancellationToken ct = default)
   {
      var lineCount = _context.SourceText.Lines.Count;
      var list = new PooledList<SyntaxSpan>(64);

      for (var e = 0; e < lineCount; e++)
      {
         var line = _context.SourceText.Lines[e];
         var lineSpan = line.Span;
         var classified = (await Classifier.GetClassifiedSpansAsync(_context.Document, lineSpan, ct))
            .GroupBy(x => x.TextSpan)
            .Select(g => g.OrderByDescending(c => c.ClassificationType == ClassificationTypeNames.Keyword).First())
            .OrderBy(c => c.TextSpan.Start)
            .ToList();
         
         TokenizeLine(ref list, lineSpan, classified, e + 1);
         list.Add(new SyntaxSpan(string.Empty, string.Empty, isLineBreak: true));
      }

      return list;
   }

   private void TokenizeLine(
      ref PooledList<SyntaxSpan> list, 
      TextSpan lineSpan, 
      List<ClassifiedSpan> classifiedSpans,
      int lineNumber)
   {
      var start = lineSpan.Start;
      List<SyntaxSpan> lineSpans = [];
      
      if (classifiedSpans.Count == 0)
      {
         var empty = new SyntaxSpan(GetText(lineSpan), GetColor());
         
         list.Add(empty);
         lineSpans.Add(empty);
         
         return;
      }

      foreach (var classifiedSpan in classifiedSpans)
      {
         if (classifiedSpan.TextSpan.Intersection(lineSpan) 
             is not { } classified)
         {
            continue;
         }
         
         if (classified.Start > start)
         {
            var unclassified = new TextSpan(start, classified.Start - start);
            start = classified.Start;

            var empty = new SyntaxSpan(GetText(unclassified), GetColor());
            
            list.Add(empty);
            lineSpans.Add(empty);
         }
         
         var type = classifiedSpan.ClassificationType;
         var syntaxSpan = new SyntaxSpan(GetText(classifiedSpan.TextSpan), GetColor(type));
         
         ApplyContext(
            ref syntaxSpan, 
            classifiedSpan, 
            lineSpans, 
            ref list,
            lineNumber);
         
         list.Add(syntaxSpan);
         lineSpans.Add(syntaxSpan);
         start = classified.End;
      }

      if (start >= lineSpan.End) return;
      
      var ending = new TextSpan(start, lineSpan.End - start);
      var endingSpan = new SyntaxSpan(GetText(ending), GetColor());
      
      list.Add(endingSpan);
      lineSpans.Add(endingSpan);
   }
   
   private string GetText(TextSpan span)
   {
      return _context.SourceText.ToString(span);
   }

   private string GetColor(string? type = null)
   {
      return _theme.Colors.GetValueOrDefault(type ?? string.Empty)
         ?? _theme.Colors[CodeTheme.DefaultColorKeyName];
   }

   private void ApplyContext(
      ref SyntaxSpan span, ClassifiedSpan classified, 
      List<SyntaxSpan> lineSpans, ref PooledList<SyntaxSpan> list,
      int lineNumber)
   {
      switch (classified.ClassificationType)
      {
         case ClassificationTypeNames.NamespaceName:
            break;
         
         case ClassificationTypeNames.ClassName:
         case ClassificationTypeNames.RecordClassName:
         case ClassificationTypeNames.StructName:
         case ClassificationTypeNames.RecordStructName:
         case ClassificationTypeNames.InterfaceName:
         case ClassificationTypeNames.EnumName:
         case ClassificationTypeNames.MethodName:
         case ClassificationTypeNames.ExtensionMethodName:
         case ClassificationTypeNames.FieldName:
         case ClassificationTypeNames.PropertyName:
            ApplySymbolContext(ref span, classified, lineSpans, ref list, lineNumber);
            break;
         
         case ClassificationTypeNames.ParameterName:
            ApplyParameterSymbolContext(ref span, classified);
            break;
         
         case ClassificationTypeNames.LocalName:
            ApplyLocalNameContext(ref span, classified);
            break;
      }
   }

   private void ApplySymbolContext(
      ref SyntaxSpan span, ClassifiedSpan classified, 
      List<SyntaxSpan> lineSpans, ref PooledList<SyntaxSpan> list,
      int lineNumber)
   {
      var (node, symbol) = GetSymbolFromContext(classified);
      if (symbol is null) return;
      
      span.Reference = _context.Store.NodeIdStore.GetOrAdd(symbol.OriginalDefinition);
      span.IsDeclaration = node is ClassDeclarationSyntax
         or InterfaceDeclarationSyntax
         or StructDeclarationSyntax
         or EnumDeclarationSyntax
         or MethodDeclarationSyntax
         or FieldDeclarationSyntax
         or PropertyDeclarationSyntax;

      _context.Store.Occurrences.AddOccurrence(
         ref span, lineSpans, _context.ProjectId, 
         _context.FileId, list.Count, lineNumber);
   }
   
   private void ApplyParameterSymbolContext(ref SyntaxSpan span, ClassifiedSpan classified)
   {
      var (node, symbol) = GetSymbolFromContext(classified);

      if (symbol is not IParameterSymbol parameter 
          || symbol?.ContainingSymbol is not IMethodSymbol { OriginalDefinition: { } method })
      {
         return;
      }

      var stringId = parameter.GenerateParameterId(method);
      span.Reference = _context.Store.NodeIdStore.GetOrAdd(stringId);
      //span.StringReference = stringId;
   }

   private void ApplyLocalNameContext(ref SyntaxSpan span, ClassifiedSpan classified)
   {
      var (node, symbol) = GetSymbolFromContext(classified);

      if (symbol?.Locations is [var reference])
      {
         span.StringReference = reference.ToString().GenerateId();
      }
   }

   private (SyntaxNode node, ISymbol? symbol) GetSymbolFromContext(ClassifiedSpan classified)
   {
      var node = _context.SyntaxNode.FindNode(classified.TextSpan);

      node = node switch
      {
         SimpleBaseTypeSyntax baseTypeSyntax => baseTypeSyntax.Type,
         TypeConstraintSyntax typeConstraintSyntax => typeConstraintSyntax.Type,
         _ => node
      };
      
      if (_context.SemanticModel.GetSymbolInfo(node) is not { Symbol: { } symbol })
      {
         if (_context.SemanticModel.GetDeclaredSymbol(node) is not { OriginalDefinition: { } declaration })
         {
            return (node, null);
         }
         
         symbol = declaration;
      }

      return (node, symbol);
   }
}