// Type: Microsoft.Xna.Framework.Storage.StorageDeviceHelper
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
