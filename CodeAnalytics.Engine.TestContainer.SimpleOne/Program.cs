
public sealed class ConstructorArchetypeChunk : ConstructorArchetypeChunkBase
{
   private string test;
   
   public ConstructorArchetypeChunk(string a) 
      : base(a, "")
   {
      this.test = a;
   }

   public static void CreateArchetype()
   {
   }
}

public class ConstructorArchetypeChunkBase
{
   public ConstructorArchetypeChunkBase(string a, string b)
   {
      
   }
}