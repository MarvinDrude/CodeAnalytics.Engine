using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Enums.Syntax;

namespace Beskar.CodeAnalytics.Data.Syntax;

public sealed class DefaultSyntaxColorTheme : SyntaxColorTheme
{
   public override string GetTokenColor(SyntaxTokenSpec token)
   {
      return token.Color switch
      {
         SyntaxColor.Comment => "#6A9955",
         SyntaxColor.ExcludedCode => "#6D6D6D",
         SyntaxColor.Identifier => "#D4D4D4",
         SyntaxColor.Keyword => "#569CD6",
         SyntaxColor.ControlKeyword => "#569CD6",
         SyntaxColor.NumericLiteral => "#B5CEA8",
         SyntaxColor.Operator => "#D4D4D4",
         SyntaxColor.OperatorOverloaded => "#D4D4D4",
         SyntaxColor.PreprocessorKeyword => "#569CD6",
         SyntaxColor.StringLiteral => "#CE9178",
         SyntaxColor.WhiteSpace => "transparent",
         SyntaxColor.Text => "#D4D4D4",
         SyntaxColor.ReassignedVariable => "#9CDCFE",
         SyntaxColor.ObsoleteSymbol => "#D4D4D4",
         SyntaxColor.StaticSymbol => "#4EC9B0",
         SyntaxColor.PreprocessorText => "#D4D4D4",
         SyntaxColor.Punctuation => "#D4D4D4",
         SyntaxColor.VerbatimStringLiteral => "#CE9178",
         SyntaxColor.StringEscapeCharacter => "#D16969",

         SyntaxColor.ClassName => "#4EC9B0",
         SyntaxColor.RecordClassName => "#4EC9B0",
         SyntaxColor.DelegateName => "#4EC9B0",
         SyntaxColor.EnumName => "#4EC9B0",
         SyntaxColor.InterfaceName => "#b8d7a3",
         SyntaxColor.ModuleName => "#4EC9B0",
         SyntaxColor.StructName => "#4EC9B0",
         SyntaxColor.RecordStructName => "#4EC9B0",
         SyntaxColor.TypeParameterName => "#4EC9B0",
         SyntaxColor.FieldName => "#FFFFFF",
         SyntaxColor.EnumMemberName => "#B5CEA8",
         SyntaxColor.ConstantName => "#FFFFFF",
         SyntaxColor.LocalName => "#9CDCFE",
         SyntaxColor.ParameterName => "#9CDCFE",
         SyntaxColor.MethodName => "#DCDCAA",
         SyntaxColor.ExtensionMethodName => "#DCDCAA",
         SyntaxColor.PropertyName => "#FFFFFF",
         SyntaxColor.EventName => "#9CDCFE",
         SyntaxColor.NamespaceName => "#FFFFFF",
         SyntaxColor.LabelName => "#D4D4D4",

         SyntaxColor.XmlDocCommentAttributeName => "#569CD6",
         SyntaxColor.XmlDocCommentAttributeQuotes => "#D4D4D4",
         SyntaxColor.XmlDocCommentAttributeValue => "#CE9178",
         SyntaxColor.XmlDocCommentCDataSection => "#B5CEA8",
         SyntaxColor.XmlDocCommentComment => "#6A9955",
         SyntaxColor.XmlDocCommentDelimiter => "#D4D4D4",
         SyntaxColor.XmlDocCommentEntityReference => "#B5CEA8",
         SyntaxColor.XmlDocCommentName => "#569CD6",
         SyntaxColor.XmlDocCommentProcessingInstruction => "#D4D4D4",
         SyntaxColor.XmlDocCommentText => "#6A9955",

         SyntaxColor.XmlLiteralAttributeName => "#569CD6",
         SyntaxColor.XmlLiteralAttributeQuotes => "#D4D4D4",
         SyntaxColor.XmlLiteralAttributeValue => "#CE9178",
         SyntaxColor.XmlLiteralCDataSection => "#B5CEA8",
         SyntaxColor.XmlLiteralComment => "#6A9955",
         SyntaxColor.XmlLiteralDelimiter => "#D4D4D4",
         SyntaxColor.XmlLiteralEmbeddedExpression => "#569CD6",
         SyntaxColor.XmlLiteralEntityReference => "#B5CEA8",
         SyntaxColor.XmlLiteralName => "#569CD6",
         SyntaxColor.XmlLiteralProcessingInstruction => "#D4D4D4",
         SyntaxColor.XmlLiteralText => "#6A9955",

         SyntaxColor.RegexComment => "#6A9955",
         SyntaxColor.RegexCharacterClass => "#646695",
         SyntaxColor.RegexAnchor => "#D4D4D4",
         SyntaxColor.RegexQuantifier => "#569CD6",
         SyntaxColor.RegexGrouping => "#569CD6",
         SyntaxColor.RegexAlternation => "#569CD6",
         SyntaxColor.RegexText => "#D4D4D4",
         SyntaxColor.RegexSelfEscapedCharacter => "#D4D4D4",
         SyntaxColor.RegexOtherEscape => "#D4D4D4",
         _ => "#FFFFFF"
      };
   }
}