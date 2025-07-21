using Microsoft.CodeAnalysis.Classification;

namespace CodeAnalytics.Engine.Collector.TextRendering.Themes;

public sealed partial class CodeTheme
{
   public static readonly CodeTheme Default = new()
   {
      Colors = new Dictionary<string, string>()
      {
         [ClassificationTypeNames.Comment] = "#6A9955",
         [ClassificationTypeNames.ExcludedCode] = "#6D6D6D",
         [ClassificationTypeNames.Identifier] = "#D4D4D4",
         [ClassificationTypeNames.Keyword] = "#569CD6",
         [ClassificationTypeNames.ControlKeyword] = "#569CD6",
         [ClassificationTypeNames.NumericLiteral] = "#B5CEA8",
         [ClassificationTypeNames.Operator] = "#D4D4D4",
         [ClassificationTypeNames.OperatorOverloaded] = "#D4D4D4",
         [ClassificationTypeNames.PreprocessorKeyword] = "#569CD6",
         [ClassificationTypeNames.StringLiteral] = "#CE9178",
         [ClassificationTypeNames.WhiteSpace] = "transparent",
         [ClassificationTypeNames.Text] = "#D4D4D4",
         ["reassigned variable"] = "#9CDCFE",
         ["obsolete symbol"] = "#D4D4D4",
         [ClassificationTypeNames.StaticSymbol] = "#4EC9B0",
         [ClassificationTypeNames.PreprocessorText] = "#D4D4D4",
         [ClassificationTypeNames.Punctuation] = "#D4D4D4",
         [ClassificationTypeNames.VerbatimStringLiteral] = "#CE9178",
         [ClassificationTypeNames.StringEscapeCharacter] = "#D16969",
         
         [ClassificationTypeNames.ClassName] = "#4EC9B0",
         [ClassificationTypeNames.RecordClassName] = "#4EC9B0",
         [ClassificationTypeNames.DelegateName] = "#4EC9B0",
         [ClassificationTypeNames.EnumName] = "#4EC9B0",
         [ClassificationTypeNames.InterfaceName] = "#b8d7a3",
         [ClassificationTypeNames.ModuleName] = "#4EC9B0",
         [ClassificationTypeNames.StructName] = "#4EC9B0",
         [ClassificationTypeNames.RecordStructName] = "#4EC9B0",
         [ClassificationTypeNames.TypeParameterName] = "#4EC9B0",
         [ClassificationTypeNames.FieldName] = "#FFFFFF",
         [ClassificationTypeNames.EnumMemberName] = "#B5CEA8",
         [ClassificationTypeNames.ConstantName] = "#FFFFFF",
         [ClassificationTypeNames.LocalName] = "#9CDCFE",
         [ClassificationTypeNames.ParameterName] = "#9CDCFE",
         [ClassificationTypeNames.MethodName] = "#DCDCAA",
         [ClassificationTypeNames.ExtensionMethodName] = "#DCDCAA",
         [ClassificationTypeNames.PropertyName] = "#FFFFFF",
         [ClassificationTypeNames.EventName] = "#9CDCFE",
         [ClassificationTypeNames.NamespaceName] = "#FFFFFF",
         [ClassificationTypeNames.LabelName] = "#D4D4D4",
         
         [ClassificationTypeNames.XmlDocCommentAttributeName] = "#569CD6",
         [ClassificationTypeNames.XmlDocCommentAttributeQuotes] = "#D4D4D4",
         [ClassificationTypeNames.XmlDocCommentAttributeValue] = "#CE9178",
         [ClassificationTypeNames.XmlDocCommentCDataSection] = "#B5CEA8",
         [ClassificationTypeNames.XmlDocCommentComment] = "#6A9955",
         [ClassificationTypeNames.XmlDocCommentDelimiter] = "#D4D4D4",
         [ClassificationTypeNames.XmlDocCommentEntityReference] = "#B5CEA8",
         [ClassificationTypeNames.XmlDocCommentName] = "#569CD6",
         [ClassificationTypeNames.XmlDocCommentProcessingInstruction] = "#D4D4D4",
         [ClassificationTypeNames.XmlDocCommentText] = "#6A9955",
         
         [ClassificationTypeNames.XmlLiteralAttributeName] = "#569CD6",
         [ClassificationTypeNames.XmlLiteralAttributeQuotes] = "#D4D4D4",
         [ClassificationTypeNames.XmlLiteralAttributeValue] = "#CE9178",
         [ClassificationTypeNames.XmlLiteralCDataSection] = "#B5CEA8",
         [ClassificationTypeNames.XmlLiteralComment] = "#6A9955",
         [ClassificationTypeNames.XmlLiteralDelimiter] = "#D4D4D4",
         [ClassificationTypeNames.XmlLiteralEmbeddedExpression] = "#569CD6",
         [ClassificationTypeNames.XmlLiteralEntityReference] = "#B5CEA8",
         [ClassificationTypeNames.XmlLiteralName] = "#569CD6",
         [ClassificationTypeNames.XmlLiteralProcessingInstruction] = "#D4D4D4",
         [ClassificationTypeNames.XmlLiteralText] = "#6A9955",
         
         [ClassificationTypeNames.RegexComment] = "#6A9955",
         [ClassificationTypeNames.RegexCharacterClass] = "#646695",
         [ClassificationTypeNames.RegexAnchor] = "#D4D4D4",
         [ClassificationTypeNames.RegexQuantifier] = "#569CD6",
         [ClassificationTypeNames.RegexGrouping] = "#569CD6",
         [ClassificationTypeNames.RegexAlternation] = "#569CD6",
         [ClassificationTypeNames.RegexText] = "#D4D4D4",
         [ClassificationTypeNames.RegexSelfEscapedCharacter] = "#D4D4D4",
         [ClassificationTypeNames.RegexOtherEscape] = "#D4D4D4",
         
         [DefaultColorKeyName] = "#FFFFFF",
      }
   };
}