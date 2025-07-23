namespace CodeAnalytics.Web.Client.Menus.Interfaces;

public interface IMenuComponentInfo
{
   public Dictionary<string, object?> Parameters { get; }
   
   public Type ComponentType { get; }
   
   public ulong Id { get; }
}

public interface IMenuComponentInfo<TResult> : IMenuComponentInfo
{
   public TaskCompletionSource<TResult?> Tcs { get; }
}