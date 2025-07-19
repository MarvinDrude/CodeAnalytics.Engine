// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;

Console.WriteLine("Hello, World!");

public class Test
{
   public int TestTest
   {
      get
      {
         Console.WriteLine("Hello, World!");
         var two = RandomNumberGenerator.GetInt32(0, 7);

         if (two == 3)
         {
            Console.WriteLine("Hello, World!");
         }

         return 0;
      }
      set
      {
         Console.WriteLine("Hello, World!");
         var two = RandomNumberGenerator.GetInt32(0, 7);

         if (two == 3)
         {
            Console.WriteLine("Hello, World!");
         }

         value = 1;
      }
   }
   
   public Test()
   {
      Console.WriteLine("Hello, World!");
      var two = RandomNumberGenerator.GetInt32(0, 7);

      if (two == 3)
      {
         Console.WriteLine("Hello, World!");
      }
   }

   public void Hello()
   {
      Console.WriteLine("Hello, World!");
      var two = RandomNumberGenerator.GetInt32(0, 7);

      if (two == 3)
      {
         Console.WriteLine("Hello, World!");
      }
   }
}