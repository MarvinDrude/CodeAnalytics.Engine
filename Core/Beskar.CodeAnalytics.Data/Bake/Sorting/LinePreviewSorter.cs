using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Files;

namespace Beskar.CodeAnalytics.Data.Bake.Sorting;

/// <summary>
/// Sort previews by symbol to get better cache locality in the file
/// </summary>
public sealed class LinePreviewSorter(BakeContext context)
{
   private readonly BakeContext _context = context;
   private readonly string _filePath = Path.Combine(context.OutputDirectoryPath, FileNames.LinePreviews);

   public void Sort()
   {
      var specFilePath = Path.Combine(_context.OutputDirectoryPath, _context.FileNames[FileIds.FileLocation]);
      
      using var specHandle = new MmfHandle(specFilePath, writable: true);
      using var buffer = specHandle.GetBuffer();

      var specSpan = buffer.GetSpanByByteCount<SymbolLocationSpec>(0, specHandle.Length);
      var targetFilePath = _filePath + ".sorted";

      using (var fs = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
      using (var unorderedHandle = new MmfHandle(_filePath, writable: false))
      using (var unorderedBuffer = unorderedHandle.GetBuffer())
      {
         foreach (ref var spec in specSpan)
         {
            var positionBefore = fs.Position;
            var bytes = unorderedBuffer.GetSpan<byte>((long)spec.LinePreview.Offset, spec.LinePreview.Length);
            fs.Write(bytes);
         
            spec.LinePreview.Offset = (ulong)positionBefore;
         }
      }
      
      File.Delete(_filePath);
      File.Move(targetFilePath, _filePath, true);
   }
}