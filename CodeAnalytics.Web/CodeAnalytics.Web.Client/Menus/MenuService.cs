using System.Collections.Concurrent;
using CodeAnalytics.Web.Client.Menus.Interfaces;
using CodeAnalytics.Web.Client.Menus.Models;
using Microsoft.AspNetCore.Components;

namespace CodeAnalytics.Web.Client.Menus;

public sealed class MenuService
{
   public event Action? OnChange;
   
   private readonly ConcurrentStack<IMenuComponentInfo> _openMenus = [];
   public IEnumerable<IMenuComponentInfo> OpenMenus => _openMenus.AsEnumerable();

   private ulong _idStep;
   
   public Task<TResult?> Open<TComponent, TResult>(MenuCreateOptions? options = null)
      where TComponent : ComponentBase, IMenuComponent<TResult>
      where TResult : class
   {
      options ??= new MenuCreateOptions();
      var info = new MenuComponentInfo<TComponent, TResult>()
      {
         Parameters = options.Parameters,
         Id = Interlocked.Increment(ref _idStep),
      };

      _openMenus.Push(info);
      OnChange?.Invoke();
      
      return info.Tcs.Task;
   }

   public bool SetResult<TResult>(ulong menuId, TResult? result)
      where TResult : class
   {
      // not atomic, but since we dont have real multi thread most of the time,
      // this should be fine for now
      
      List<IMenuComponentInfo> reAdd = [];
      IMenuComponentInfo? item = null;

      while (_openMenus.TryPop(out var info))
      {
         if (info.Id == menuId)
         {
            item = info;
            break;
         }

         reAdd.Add(info);
      }

      for (var e = reAdd.Count - 1; e >= 0; e--)
      {
         _openMenus.Push(reAdd[e]);
      }
      
      if (item is IMenuComponentInfo<TResult> casted)
      {
         casted.Tcs.SetResult(result);
      }
      
      OnChange?.Invoke();
      return item is not null;
   }
}