using Beskar.CodeAnalytics.Collector.Options;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Storage.Discovery.Sorting;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
using Me.Memory.Extensions;
using Me.Memory.Threading;

namespace Beskar.CodeAnalytics.Collector.Sorting;

public sealed class DiscoverySorter(
   WorkPool workPool, CollectorOptions collectOptions,
   DiscoveryResult result)
{
   private readonly WorkPool _workPool = workPool;
   private readonly CollectorOptions _collectOptions = collectOptions;
   
   private readonly DiscoveryResult _result = result;

   public async Task Run(CancellationToken token)
   {
      var sortTasks = new Task<bool>[9];

      sortTasks[0] = _workPool.Enqueue(_ => RunSorter(_result.SymbolFilePath, SymbolComparer), token);
      sortTasks[1] = _workPool.Enqueue(_ => RunSorter(_result.TypeSymbolFilePath, TypeSymbolComparer), token);
      sortTasks[2] = _workPool.Enqueue(_ => RunSorter(_result.NamedTypeSymbolFilePath, NamedTypeSymbolComparer), token);
      sortTasks[3] = _workPool.Enqueue(_ => RunSorter(_result.ParameterSymbolFilePath, ParameterSymbolComparer), token);
      sortTasks[4] = _workPool.Enqueue(_ => RunSorter(_result.TypeParameterSymbolFilePath, TypeParameterSymbolComparer), token);
      sortTasks[5] = _workPool.Enqueue(_ => RunSorter(_result.MethodSymbolFilePath, MethodSymbolComparer), token);
      sortTasks[6] = _workPool.Enqueue(_ => RunSorter(_result.FieldSymbolFilePath, FieldSymbolComparer), token);
      sortTasks[7] = _workPool.Enqueue(_ => RunSorter(_result.PropertySymbolFilePath, PropertySymbolComparer), token);
      sortTasks[8] = _workPool.Enqueue(_ => RunSorter(_result.EdgeFilePath, EdgeDiscoveryComparer.Instance), token);

      await Task.WhenAll(sortTasks)
         .WithAggregateException();
   }

   private Task<bool> RunSorter<T>(string filePath, IComparer<T> comparer)
      where T : unmanaged
   {
      var targetPath = filePath.Replace(".discovery.", ".sorted.");
      File.Delete(targetPath);
      
      var sorter = new FileSorter<T>(comparer);
      sorter.Sort(filePath, targetPath, _collectOptions.SortMaxItemBuffer);

      if (_collectOptions.DeleteIntermediateFiles)
      {
         File.Delete(filePath);
      }
      return Task.FromResult(true);
   }
   
   private static readonly IComparer<SymbolDefinition> SymbolComparer 
      = Comparer<SymbolDefinition>.Create((x, y) => x.Id.CompareTo(y.Id));
   
   private static readonly IComparer<TypeSymbolDefinition> TypeSymbolComparer 
      = Comparer<TypeSymbolDefinition>.Create((x, y) => x.SymbolId.CompareTo(y.SymbolId));
   
   private static readonly IComparer<NamedTypeSymbolDefinition> NamedTypeSymbolComparer 
      = Comparer<NamedTypeSymbolDefinition>.Create((x, y) => x.SymbolId.CompareTo(y.SymbolId));
   
   private static readonly IComparer<ParameterSymbolDefinition> ParameterSymbolComparer 
      = Comparer<ParameterSymbolDefinition>.Create((x, y) => x.SymbolId.CompareTo(y.SymbolId));
   
   private static readonly IComparer<TypeParameterSymbolDefinition> TypeParameterSymbolComparer 
      = Comparer<TypeParameterSymbolDefinition>.Create((x, y) => x.SymbolId.CompareTo(y.SymbolId));
   
   private static readonly IComparer<MethodSymbolDefinition> MethodSymbolComparer 
      = Comparer<MethodSymbolDefinition>.Create((x, y) => x.SymbolId.CompareTo(y.SymbolId));
   
   private static readonly IComparer<FieldSymbolDefinition> FieldSymbolComparer 
      = Comparer<FieldSymbolDefinition>.Create((x, y) => x.SymbolId.CompareTo(y.SymbolId));
   
   private static readonly IComparer<PropertySymbolDefinition> PropertySymbolComparer 
      = Comparer<PropertySymbolDefinition>.Create((x, y) => x.SymbolId.CompareTo(y.SymbolId));
}