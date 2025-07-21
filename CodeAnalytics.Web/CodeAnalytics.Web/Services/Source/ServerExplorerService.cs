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
   
   public ServerExplorerService(IOptionsMonitor<CodeOptions> optionsMonitor)
   {
      _optionsMonitor = optionsMonitor;
      _optionsMonitor.OnChange(_ => _initialized = false);
   }
   
   public async Task<List<ExplorerTreeItem>> GetExplorerTreeItems()
   {
      await EnsureInitialized();
      return _treeItems;
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
      
      _initialized = true;
      return Task.CompletedTask;
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
            Name = Path.GetDirectoryName(directory) ?? "Unknown",
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