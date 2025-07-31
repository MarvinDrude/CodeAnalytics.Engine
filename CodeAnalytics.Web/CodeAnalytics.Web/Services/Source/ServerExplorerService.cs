using CodeAnalytics.Web.Common.Enums.Explorer;
using CodeAnalytics.Web.Common.Models.Explorer;
using CodeAnalytics.Web.Common.Services.Source;
using CodeAnalytics.Web.Options;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Web.Services.Source;

public sealed class ServerExplorerService : IExplorerService
{
   private readonly IOptionsMonitor<CodeOptions> _optionsMonitor;
   private CodeOptions Options => _optionsMonitor.CurrentValue;

   private bool _initialized = false;
   private List<ExplorerTreeItem> _treeItems = [];
   private ExplorerFlatTreeItem[] _flatTreeItems = [];
   
   public ServerExplorerService(IOptionsMonitor<CodeOptions> optionsMonitor)
   {
      _optionsMonitor = optionsMonitor;
      _optionsMonitor.OnChange(_ => _initialized = false);
   }
   
   public async Task<List<ExplorerTreeItem>> GetExplorerTreeItems()
   {
      await EnsureInitialized();
      return ExplorerTreeItem.Clone(_treeItems);
   }

   public async Task<ExplorerFlatTreeItem[]> GetFlatTreeItems()
   {
      await EnsureInitialized();
      return _flatTreeItems;
   }

   private ValueTask EnsureInitialized()
   {
      return _initialized ? 
         ValueTask.CompletedTask
         : new ValueTask(Reload());
   }

   private Task Reload()
   {
      _initialized = false;
      _treeItems = Traverse(Options.DataFolderPath);
      _flatTreeItems = FlattenItems(_treeItems);
      
      _initialized = true;
      return Task.CompletedTask;
   }

   private ExplorerFlatTreeItem[] FlattenItems(List<ExplorerTreeItem> items)
   {
      var result = new List<ExplorerFlatTreeItem>(items.Count);
      Stack<string> stack = [];

      foreach (var item in items)
      {
         TraverseLocal(item);
      }

      return [.. result];

      void TraverseLocal(ExplorerTreeItem item)
      {
         result.Add(new ExplorerFlatTreeItem(
            item.Type,
            item.Name,
            stack.Reverse().ToArray()));
         
         stack.Push(item.Name);
         
         foreach (var child in item.Children)
         {
            TraverseLocal(child);
         }

         stack.Pop();
      }
   }
   
   private List<ExplorerTreeItem> Traverse(string directory)
   {
      List<ExplorerTreeItem> result = [];
      
      foreach (var filePath in Directory.GetFiles(directory))
      {
         if (CreateFileItem(filePath) is not { } file)
         {
            continue;
         }

         result.Add(file);
      }

      foreach (var subDirectory in Directory.GetDirectories(directory))
      {
         var folder = new ExplorerTreeItem
         {
            Name = Path.GetFileName(subDirectory) ?? "Unknown",
            Path = Path.GetRelativePath(Options.DataFolderPath, subDirectory),
            Type = ExplorerTreeItemType.Folder,
            Children = Traverse(subDirectory)
         };

         result.Add(folder);
      }

      return result
         .OrderByDescending(item => item.Type)
         .ThenBy(item => item.Name)
         .ToList();
   }

   private ExplorerTreeItem? CreateFileItem(string filePath)
   {
      return filePath switch
      {
         _ when filePath.EndsWith(".csspan") => CreateCsFileItem(filePath),
         _ => null
      };
   }

   private ExplorerTreeItem CreateCsFileItem(string filePath)
   {
      filePath = Path.ChangeExtension(filePath, ".cs");

      return new ExplorerTreeItem()
      {
         Name = Path.GetFileName(filePath),
         Path = Path.GetRelativePath(Options.DataFolderPath, filePath),
         Type = ExplorerTreeItemType.CsFile
      };
   }
}