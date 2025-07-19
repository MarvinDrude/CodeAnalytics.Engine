using System.Buffers;

namespace CodeAnalytics.Engine.Common.Buffers;

public class MemoryAllocator<T>
{
   private static readonly ArrayPool<T> _pool = ArrayPool<T>.Shared;
   
   public static MemoryOwner<T> CreatePooled(int length)
   {
      return new MemoryOwner<T>(_pool, length);
   }

   public static MemoryOwner<T> Create(int length)
   {
      return new MemoryOwner<T>(new T[length], length);
   }
}