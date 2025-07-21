using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.TextRendering;

namespace CodeAnalytics.Web.Common.Services.Source;

public interface ISourceTextService
{
   public Task<Result<SyntaxSpan[], Error<string>>> GetSyntaxSpansByStringId(StringId stringId);
   public Task<Result<SyntaxSpan[], Error<string>>> GetSyntaxSpansByPath(string path);
}