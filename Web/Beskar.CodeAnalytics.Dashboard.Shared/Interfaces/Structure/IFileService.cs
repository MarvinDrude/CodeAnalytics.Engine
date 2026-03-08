using Beskar.CodeAnalytics.Data.Entities.Structure;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Structure;

public interface IFileService
{
   public SyntaxFile? GetFile(uint fileId);
}