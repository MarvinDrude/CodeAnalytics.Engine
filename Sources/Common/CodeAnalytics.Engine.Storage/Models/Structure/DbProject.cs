namespace CodeAnalytics.Engine.Storage.Models.Structure;

public sealed class DbProject
{
   public DbProjectId Id { get; set; }
   
   public required string Name { get; set; }
   
   public required string RelativeFilePath { get; set; }
   
   public required string AssemblyName { get; set; }
   
   public List<DbSolution> Solutions { get; set; } = [];
   public List<DbFile> Files { get; set; } = [];
   
   public List<DbProject> ReferencedProjects { get; set; } = [];
   public List<DbProject> ReferencedByProjects { get; set; } = [];
}

public readonly record struct DbProjectId(long Value)
{
   public static readonly DbProjectId Empty = new(0);
   
   public static implicit operator long(DbProjectId id) => id.Value;
   public static implicit operator DbProjectId(long value) => new(value);
}