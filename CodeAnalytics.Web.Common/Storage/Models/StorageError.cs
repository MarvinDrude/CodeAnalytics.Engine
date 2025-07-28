using System.Runtime.InteropServices;

namespace CodeAnalytics.Web.Common.Storage.Models;

[StructLayout(LayoutKind.Auto)]
public readonly struct StorageError
{
   public readonly StorageErrorType Type;
   public readonly string? Message;

   public StorageError(StorageErrorType type, string? message)
   {
      Type = type;
      Message = message;
   }
}

public enum StorageErrorType
{
   KeyNotFound,
   StorageError,
   JsonError
}