
public interface ITest<T>
   where T : BaseClass
{
   public void Run(string a);
}

public class Test : BaseClass, ITest<BaseClass>, IDisposable
{
   public string? Tests { get; set; }
   public string? Testss;
   
   public void Run(string a)
   {
      var bb = "";

      Action<string> aaaa = (aaaa) =>
      {
         var bb = "aaa";
         bb = "a";
         bb = "a";
         bb = "a";
      };
      
      a = "";
      Tests = "";
   }

   public void Dispose()
   {
      
   }
}

public abstract class BaseClass;