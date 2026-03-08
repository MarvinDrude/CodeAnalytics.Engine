using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Structure;
using Beskar.CodeAnalytics.Data.Entities.Structure;

namespace Beskar.CodeAnalytics.Dashboard.Services.Structure;

public sealed class FileService(IDatabaseProvider dbProvider) : IFileService
{
   private readonly IDatabaseProvider _databaseProvider = dbProvider;
   
   public SyntaxFile? GetFile(uint fileId)
   {
      var db = _databaseProvider.GetDescriptor();

      return db.Structure.SyntaxFiles.Reader.GetById(fileId);
   }
}