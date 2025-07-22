
var test = CreateTest();

Test.Hello();


Test.Hello();

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();

Test.Hello();

static Test CreateTest()
{
   return new Test();
}

public class Test
{
   public static void Hello()
   {
      
   }
}