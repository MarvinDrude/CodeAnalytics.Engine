namespace CodeAnalytics.Web.Client.Menus.Interfaces;

public interface IMenuComponent<TResult> : IMenuComponent
{
   
}

public interface IMenuComponent
{
   public int MenuIndex { get; set; }
   
   public ulong MenuId { get; set; }
}