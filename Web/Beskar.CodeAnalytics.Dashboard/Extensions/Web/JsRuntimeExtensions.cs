using Microsoft.JSInterop;

namespace Beskar.CodeAnalytics.Dashboard.Extensions.Web;

public static class JsRuntimeExtensions
{
   extension(IJSRuntime runtime)
   {
      public async Task<IJSObjectReference> GetModule(string path)
      {
         return await runtime.InvokeAsync<IJSObjectReference>("import", $"./Components/{path}");
      }
   }
}