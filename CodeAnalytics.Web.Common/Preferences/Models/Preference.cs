namespace CodeAnalytics.Web.Common.Preferences.Models;

public class Preference<TData> : IPreference
   where TData : class
{
   public required TData Data { get; set; }
   
   public required int Version { get; set; }
}

public interface IPreference
{
   public int Version { get; set; }
}