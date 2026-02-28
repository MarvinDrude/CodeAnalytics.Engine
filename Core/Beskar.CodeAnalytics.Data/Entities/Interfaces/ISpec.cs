using Beskar.CodeAnalytics.Data.Constants;

namespace Beskar.CodeAnalytics.Data.Entities.Interfaces;

public interface ISpec
{
   public uint Identifier { get; }

   public static abstract FileId FileId { get; }
}