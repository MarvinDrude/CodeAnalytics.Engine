using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Symbols;
using Beskar.CodeAnalytics.Dashboard.Shared.Models.Symbols;
using Beskar.CodeAnalytics.Data.Entities.Archetypes;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Dashboard.Services.Symbols;

public sealed class SymbolService(IDatabaseProvider provider) : ISymbolService
{
   private readonly IDatabaseProvider _provider = provider;
   private DatabaseDescriptor Descriptor => _provider.GetDescriptor();

   public SymbolType GetSymbolType(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      return db.Symbols.GetReader().GetSpecById(symbolId).Type;
   }
   
   public ArchetypeCardModel<FieldArchetype> GetFieldCard(uint symbolId)
   {
      var db = Descriptor.Symbols;

      var model = new ArchetypeCardModel<FieldArchetype>()
      {
         Archetype = db.GetFieldArchetype(symbolId),
      };
      
      var symbol = model.Archetype.Symbol;
      FillSymbolStrings(model, ref symbol);

      return model;
   }

   public ArchetypeCardModel<MethodArchetype> GetMethodCard(uint symbolId)
   {
      var db = Descriptor.Symbols;

      var model = new ArchetypeCardModel<MethodArchetype>()
      {
         Archetype = db.GetMethodArchetype(symbolId),
      };
      
      var symbol = model.Archetype.Symbol;
      FillSymbolStrings(model, ref symbol);

      return model;
   }

   public ArchetypeCardModel<NamedTypeArchetype> GetNamedTypeCard(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      var model = new ArchetypeCardModel<NamedTypeArchetype>()
      {
         Archetype = db.GetNamedTypeArchetype(symbolId),
      };

      var symbol = model.Archetype.Symbol;
      FillSymbolStrings(model, ref symbol);
      
      return model;
   }
   
   public ArchetypeCardModel<PropertyArchetype> GetPropertyCard(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      var model = new ArchetypeCardModel<PropertyArchetype>()
      {
         Archetype = db.GetPropertyArchetype(symbolId),
      };
      
      var symbol = model.Archetype.Symbol;
      FillSymbolStrings(model, ref symbol);
      
      return model;
   }

   public ArchetypeCardModel<TypeArchetype> GetTypeCard(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      var model = new ArchetypeCardModel<TypeArchetype>()
      {
         Archetype = db.GetTypeArchetype(symbolId),
      };
      
      var symbol = model.Archetype.Symbol;
      FillSymbolStrings(model, ref symbol);
      
      return model;
   }
   
   public ArchetypeCardModel<TypeParameterArchetype> GetTypeParameterCard(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      var model = new ArchetypeCardModel<TypeParameterArchetype>()
      {
         Archetype = db.GetTypeParameterArchetype(symbolId),
      };
      
      var symbol = model.Archetype.Symbol;
      FillSymbolStrings(model, ref symbol);

      return model;
   }

   private void FillSymbolStrings<T>(ArchetypeCardModel<T> model, ref SymbolSpec symbol)
      where T : unmanaged
   {
      var strPool = Descriptor.StringPool;
      
      using var ownerStrs = new SpanOwner<StringFileView>(stackalloc StringFileView[3]);
      var span = ownerStrs.Span;

      span[0] = symbol.Name;
      span[1] = symbol.MetadataName;
      span[2] = symbol.FullPathName;

      span.Sort(static (x, y) => x.Offset.CompareTo(y.Offset));
      var strings = strPool.Reader.GetStrings(span);

      foreach (var off in span)
      {
         model.Strings[off] = strings[off];
      }
   }
}