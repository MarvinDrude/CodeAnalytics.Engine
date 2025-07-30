namespace CodeAnalytics.Engine.Analyze.Interfaces;

public interface IAnalyzeStoreProvider
{
   public ValueTask<AnalyzeStore> GetStore();
}