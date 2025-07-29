


public interface IFoo
{
   void DoStuff();
}

public interface IBar : IBase
{
   void DoStuff();
}

public interface IBase
{
   void BaseStuff();
}

public class MyClass : IFoo, IBar
{
   void IFoo.DoStuff() { Console.WriteLine("Foo"); }
   void IBar.DoStuff() { Console.WriteLine("Bar"); }
   
   public void BaseStuff() { Console.WriteLine("BaseStuff"); }
}
