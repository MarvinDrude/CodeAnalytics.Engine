using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Syntax;
using Beskar.CodeAnalytics.Dashboard.Shared.Models.Syntax;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Dashboard.Services.Syntax;

public sealed class LocationService(IDatabaseProvider provider) : ILocationService
{
   private readonly IDatabaseProvider _provider = provider;

   public TokenLocationModel[] GetLocations(uint symbolId)
   {
      var db = _provider.GetDescriptor();
      var reader = db.Structure.SymbolLocations.GetReader();
      var previewReader = db.LinePreviews.Reader;
      
      using var locations = reader.LeaseById(symbolId);
      
      using var owner = new MemoryOwner<LinePreviewView>(locations.Span.Length);
      using var fileIdOwner = new MemoryOwner<uint>(locations.Span.Length);
      
      for (var index = 0; index < locations.Span.Length; index++)
      {
         var location = locations.Span[index];
         owner.Span[index] = location.LinePreview;
         fileIdOwner.Span[index] = location.SourceFileId;
      }

      //owner.Span.Sort(static (x, y) => x.Offset.CompareTo(y.Offset));
      fileIdOwner.Span.Sort(static (x, y) => x.CompareTo(y));
      
      var result = new TokenLocationModel[locations.Span.Length];
      var previews = previewReader.GetStrings(owner.Span);
      
      var fileReader = db.Structure.Files.GetReader();
      var files = fileReader.GetLookupBySortedIds(fileIdOwner.Span);
      // optimize in future
      var filePaths = db.StringPool.Reader.GetStrings(files.Values.ToArray(), (f) => f.FullPath.Offset);
      
      for (var index = 0; index < locations.Span.Length; index++)
      {
         var location = locations.Span[index];
         var file = files[location.SourceFileId];
         
         result[index] = new TokenLocationModel()
         {
            PreviewLine = previews[index],
            Location = locations.Span[index],
            File = file,
            FilePath = filePaths[file.FullPath]
         };
      }
      
      return result;
   }
}