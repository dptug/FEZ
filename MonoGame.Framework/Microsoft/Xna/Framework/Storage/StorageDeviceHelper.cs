// Type: Microsoft.Xna.Framework.Storage.StorageDeviceHelper
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Storage
{
  internal class StorageDeviceHelper
  {
    private static string path = string.Empty;

    internal static string Path
    {
      get
      {
        return StorageDeviceHelper.path;
      }
      set
      {
        if (!(StorageDeviceHelper.path != value))
          return;
        StorageDeviceHelper.path = value;
      }
    }

    internal static long FreeSpace
    {
      get
      {
        return 0L;
      }
    }

    internal static long TotalSpace
    {
      get
      {
        return 0L;
      }
    }

    static StorageDeviceHelper()
    {
    }
  }
}
