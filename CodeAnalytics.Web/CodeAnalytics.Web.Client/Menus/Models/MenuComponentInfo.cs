using CodeAnalytics.Web.Client.Menus.Interfaces;

namespace CodeAnalytics.Web.Client.Menus.Models;

public sealed class MenuComponentInfo<TComponent, TResult> : IMenuComponentInfo<TResult>
   where TComponent : IMenuComponent<TResult>
   where TResult : class
{
   public Dictionary<string, object?> Parameters { get; init; } = [];

   public Type ComponentType { get; } = typeof(TComponent);
   
   public TaskCompletionSource<TResult?> Tcs { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
   
   public ulong Id { get; init; }
}