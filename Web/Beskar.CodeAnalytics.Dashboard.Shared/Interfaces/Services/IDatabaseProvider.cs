using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;

public interface IDatabaseProvider
{
   public DatabaseDescriptor GetDescriptor();
}