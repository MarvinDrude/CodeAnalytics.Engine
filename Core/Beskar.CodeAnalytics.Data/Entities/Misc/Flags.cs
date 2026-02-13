using System.Runtime.CompilerServices;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Data.Entities.Misc;

[InlineArray(1)]
public struct Flags8
{
   private PackedBools _element;
}

[InlineArray(2)]
public struct Flags16
{
   private PackedBools _element;
}

[InlineArray(3)]
public struct Flags24
{
   private PackedBools _element;
}

[InlineArray(4)]
public struct Flags32
{
   private PackedBools _element;
}