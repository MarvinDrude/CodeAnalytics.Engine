using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Bake.Sorting;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Extensions;
using Me.Memory.Extensions;
using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Data.Bake.Steps;

/// <summary>
/// Just sort the files so all base files are ASC ordered by
/// the symbol id.
/// </summary>
public sealed class SymbolSortBakeStep : IBakeStep
{
   public string Name => "Symbol Sorting";

   private readonly ILogger<SymbolSortBakeStep> _logger;
   
   public SymbolSortBakeStep(ILoggerFactory factory)
   {
      _logger = factory.CreateLogger<SymbolSortBakeStep>();
   }
   
   public async ValueTask Execute(BakeContext context, CancellationToken cancellationToken = default)
   {
      Task<(FileId FileId, string FileName)>[] tasks = [
         context.WorkPool.Enqueue((_) => RunSort(context, _symbolComparer, FileIds.Symbol), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _typeSymbolComparer, FileIds.TypeSymbol), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _namedTypeSymbolComparer, FileIds.NamedTypeSymbol), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _parameterSymbolComparer, FileIds.ParameterSymbol), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _typeParameterSymbolComparer, FileIds.TypeParameterSymbol), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _methodSymbolComparer, FileIds.MethodSymbol), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _fieldSymbolComparer, FileIds.FieldSymbol), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _propertySymbolComparer, FileIds.PropertySymbol), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _edgeSymbolComparer, FileIds.EdgeSymbol), cancellationToken),
         
         context.WorkPool.Enqueue((_) => RunSort(context, SymbolLocationSpec.Comparer, FileIds.FileLocation), cancellationToken),
         
         context.WorkPool.Enqueue((_) => RunSort(context, _solutionComparer, FileIds.Solution), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _projectComparer, FileIds.Project), cancellationToken),
         context.WorkPool.Enqueue((_) => RunSort(context, _fileComparer, FileIds.File), cancellationToken),
      ];
      
      await Task.WhenAll(tasks)
         .WithAggregateException();
      
      new LinePreviewSorter(context).Sort();
      
      foreach (var (_, fileName) in context.FileNames)
      {
         if (context.DeleteIntermediateFiles)
         {
            File.Delete(Path.Combine(context.OutputDirectoryPath, fileName));
         }
      }
      context.FileNames.Clear();

      foreach (var task in tasks)
      {
         var (fileId, fileName) = await task;
         context.FileNames[fileId] = fileName;
      }

      context.CompleteStringWriter();
   }

   private static Task<(FileId FileId, string FileName)> RunSort<TSymbol>(
      BakeContext context, IComparer<TSymbol> comparer, FileId id)
      where TSymbol : unmanaged
   {
      var sourceName = context.FileNames[id];
      var targetName = $"{sourceName.GetBaseFileName()}.sorted.{FileNames.Suffix}";
      
      var sourceFullPath = Path.Combine(context.OutputDirectoryPath, sourceName);
      var targetFullPath = Path.Combine(context.OutputDirectoryPath, targetName);
      
      using var sorter = new FileSorter<TSymbol>(comparer);
      sorter.Sort(sourceFullPath, targetFullPath);

      return Task.FromResult((id, targetName));
   }
   
   private static readonly IComparer<SymbolSpec> _symbolComparer = Comparer<SymbolSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));
   private static readonly IComparer<TypeSymbolSpec> _typeSymbolComparer = Comparer<TypeSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
   private static readonly IComparer<NamedTypeSymbolSpec> _namedTypeSymbolComparer = Comparer<NamedTypeSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
   private static readonly IComparer<ParameterSymbolSpec> _parameterSymbolComparer = Comparer<ParameterSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
   private static readonly IComparer<TypeParameterSymbolSpec> _typeParameterSymbolComparer = Comparer<TypeParameterSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
   private static readonly IComparer<MethodSymbolSpec> _methodSymbolComparer = Comparer<MethodSymbolSpec>.Create
      (static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
   private static readonly IComparer<FieldSymbolSpec> _fieldSymbolComparer = Comparer<FieldSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
   private static readonly IComparer<PropertySymbolSpec> _propertySymbolComparer = Comparer<PropertySymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
   
   private static readonly IComparer<SolutionSpec> _solutionComparer = Comparer<SolutionSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));
   private static readonly IComparer<ProjectSpec> _projectComparer = Comparer<ProjectSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));
   private static readonly IComparer<FileSpec> _fileComparer = Comparer<FileSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));
   
   private static readonly IComparer<SymbolEdgeSpec> _edgeSymbolComparer = Comparer<SymbolEdgeSpec>.Create(
      static (x, y) =>
      {
         var compareSource = x.SourceSymbolId.CompareTo(y.SourceSymbolId);
         if (compareSource != 0) return compareSource;
         
         var compareType = x.Type.CompareTo(y.Type);
         if (compareType != 0) return compareType;
         
         return x.TargetSymbolId.CompareTo(y.TargetSymbolId);
      });
}