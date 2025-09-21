using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Models.Structure;

public sealed class DbSolution
{
   public DbSolutionId Id { get; set; }
   
   public required string Name { get; set; }
   
   public required string RelativeFilePath { get; set; }

   public List<DbProject> Projects { get; set; } = [];
}

public readonly record struct DbSolutionId(long Value)
{
   public static readonly DbSolutionId Empty = new(0);
   
   public static implicit operator long(DbSolutionId id) => id.Value;
   public static implicit operator DbSolutionId(long value) => new(value);
}