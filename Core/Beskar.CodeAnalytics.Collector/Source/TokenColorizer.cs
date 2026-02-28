using Beskar.CodeAnalytics.Data.Enums.Syntax;
using Microsoft.CodeAnalysis.Classification;

namespace Beskar.CodeAnalytics.Collector.Source;

public static class TokenColorizer
{
   public static SyntaxColor Determine(ClassifiedSpan span)
   {
      return span.ClassificationType switch
      {
         ClassificationTypeNames.Comment => SyntaxColor.Comment,
         ClassificationTypeNames.ExcludedCode => SyntaxColor.ExcludedCode,
         ClassificationTypeNames.Identifier => SyntaxColor.Identifier,
         ClassificationTypeNames.Keyword => SyntaxColor.Keyword,
         ClassificationTypeNames.ControlKeyword => SyntaxColor.ControlKeyword,
         ClassificationTypeNames.NumericLiteral => SyntaxColor.NumericLiteral,
         ClassificationTypeNames.Operator => SyntaxColor.Operator,
         ClassificationTypeNames.OperatorOverloaded => SyntaxColor.OperatorOverloaded,
         ClassificationTypeNames.PreprocessorKeyword => SyntaxColor.PreprocessorKeyword,
         ClassificationTypeNames.StringLiteral => SyntaxColor.StringLiteral,
         ClassificationTypeNames.WhiteSpace => SyntaxColor.WhiteSpace,
         ClassificationTypeNames.Text => SyntaxColor.Text,
         "reassigned variable" => SyntaxColor.ReassignedVariable,
         "obsolete symbol" => SyntaxColor.ObsoleteSymbol,
         ClassificationTypeNames.StaticSymbol => SyntaxColor.StaticSymbol,
         ClassificationTypeNames.PreprocessorText => SyntaxColor.PreprocessorText,
         ClassificationTypeNames.Punctuation => SyntaxColor.Punctuation,
         ClassificationTypeNames.VerbatimStringLiteral => SyntaxColor.VerbatimStringLiteral,
         ClassificationTypeNames.StringEscapeCharacter => SyntaxColor.StringEscapeCharacter,

         ClassificationTypeNames.ClassName => SyntaxColor.ClassName,
         ClassificationTypeNames.RecordClassName => SyntaxColor.RecordClassName,
         ClassificationTypeNames.DelegateName => SyntaxColor.DelegateName,
         ClassificationTypeNames.EnumName => SyntaxColor.EnumName,
         ClassificationTypeNames.InterfaceName => SyntaxColor.InterfaceName,
         ClassificationTypeNames.ModuleName => SyntaxColor.ModuleName,
         ClassificationTypeNames.StructName => SyntaxColor.StructName,
         ClassificationTypeNames.RecordStructName => SyntaxColor.RecordStructName,
         ClassificationTypeNames.TypeParameterName => SyntaxColor.TypeParameterName,
         ClassificationTypeNames.FieldName => SyntaxColor.FieldName,
         ClassificationTypeNames.EnumMemberName => SyntaxColor.EnumMemberName,
         ClassificationTypeNames.ConstantName => SyntaxColor.ConstantName,
         ClassificationTypeNames.LocalName => SyntaxColor.LocalName,
         ClassificationTypeNames.ParameterName => SyntaxColor.ParameterName,
         ClassificationTypeNames.MethodName => SyntaxColor.MethodName,
         ClassificationTypeNames.ExtensionMethodName => SyntaxColor.ExtensionMethodName,
         ClassificationTypeNames.PropertyName => SyntaxColor.PropertyName,
         ClassificationTypeNames.EventName => SyntaxColor.EventName,
         ClassificationTypeNames.NamespaceName => SyntaxColor.NamespaceName,
         ClassificationTypeNames.LabelName => SyntaxColor.LabelName,

         ClassificationTypeNames.XmlDocCommentAttributeName => SyntaxColor.XmlDocCommentAttributeName,
         ClassificationTypeNames.XmlDocCommentAttributeQuotes => SyntaxColor.XmlDocCommentAttributeQuotes,
         ClassificationTypeNames.XmlDocCommentAttributeValue => SyntaxColor.XmlDocCommentAttributeValue,
         ClassificationTypeNames.XmlDocCommentCDataSection => SyntaxColor.XmlDocCommentCDataSection,
         ClassificationTypeNames.XmlDocCommentComment => SyntaxColor.XmlDocCommentComment,
         ClassificationTypeNames.XmlDocCommentDelimiter => SyntaxColor.XmlDocCommentDelimiter,
         ClassificationTypeNames.XmlDocCommentEntityReference => SyntaxColor.XmlDocCommentEntityReference,
         ClassificationTypeNames.XmlDocCommentName => SyntaxColor.XmlDocCommentName,
         ClassificationTypeNames.XmlDocCommentProcessingInstruction => SyntaxColor.XmlDocCommentProcessingInstruction,
         ClassificationTypeNames.XmlDocCommentText => SyntaxColor.XmlDocCommentText,

         ClassificationTypeNames.XmlLiteralAttributeName => SyntaxColor.XmlLiteralAttributeName,
         ClassificationTypeNames.XmlLiteralAttributeQuotes => SyntaxColor.XmlLiteralAttributeQuotes,
         ClassificationTypeNames.XmlLiteralAttributeValue => SyntaxColor.XmlLiteralAttributeValue,
         ClassificationTypeNames.XmlLiteralCDataSection => SyntaxColor.XmlLiteralCDataSection,
         ClassificationTypeNames.XmlLiteralComment => SyntaxColor.XmlLiteralComment,
         ClassificationTypeNames.XmlLiteralDelimiter => SyntaxColor.XmlLiteralDelimiter,
         ClassificationTypeNames.XmlLiteralEmbeddedExpression => SyntaxColor.XmlLiteralEmbeddedExpression,
         ClassificationTypeNames.XmlLiteralEntityReference => SyntaxColor.XmlLiteralEntityReference,
         ClassificationTypeNames.XmlLiteralName => SyntaxColor.XmlLiteralName,
         ClassificationTypeNames.XmlLiteralProcessingInstruction => SyntaxColor.XmlLiteralProcessingInstruction,
         ClassificationTypeNames.XmlLiteralText => SyntaxColor.XmlLiteralText,

         ClassificationTypeNames.RegexComment => SyntaxColor.RegexComment,
         ClassificationTypeNames.RegexCharacterClass => SyntaxColor.RegexCharacterClass,
         ClassificationTypeNames.RegexAnchor => SyntaxColor.RegexAnchor,
         ClassificationTypeNames.RegexQuantifier => SyntaxColor.RegexQuantifier,
         ClassificationTypeNames.RegexGrouping => SyntaxColor.RegexGrouping,
         ClassificationTypeNames.RegexAlternation => SyntaxColor.RegexAlternation,
         ClassificationTypeNames.RegexText => SyntaxColor.RegexText,
         ClassificationTypeNames.RegexSelfEscapedCharacter => SyntaxColor.RegexSelfEscapedCharacter,
         ClassificationTypeNames.RegexOtherEscape => SyntaxColor.RegexOtherEscape,

         _ => SyntaxColor.Default
      };
   }
}