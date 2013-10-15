// Type: FezEngine.Tools.MemoryContentManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FezEngine.Tools
{
  public class MemoryContentManager : ContentManager
  {
    private static readonly object ReadLock = new object();
    private static Dictionary<string, byte[]> cachedAssets;

    private string TitleUpdateRoot
    {
      get
      {
        return this.RootDirectory;
      }
    }

    public static IEnumerable<string> AssetNames
    {
      get
      {
        return (IEnumerable<string>) MemoryContentManager.cachedAssets.Keys;
      }
    }

    static MemoryContentManager()
    {
    }

    public MemoryContentManager(IServiceProvider serviceProvider, string rootDirectory)
      : base(serviceProvider, rootDirectory)
    {
    }

    public void LoadEssentials()
    {
      MemoryContentManager.cachedAssets = new Dictionary<string, byte[]>(3011);
      using (FileStream fileStream = File.OpenRead(Path.Combine(this.RootDirectory, "Essentials.pak")))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          int num = binaryReader.ReadInt32();
          for (int index = 0; index < num; ++index)
          {
            string key = binaryReader.ReadString();
            int count = binaryReader.ReadInt32();
            if (!MemoryContentManager.cachedAssets.ContainsKey(key))
              MemoryContentManager.cachedAssets.Add(key, binaryReader.ReadBytes(count));
            else
              binaryReader.BaseStream.Seek((long) count, SeekOrigin.Current);
          }
        }
      }
    }

    public void Preload()
    {
      using (FileStream fileStream = File.OpenRead(Path.Combine(this.RootDirectory, "Other.pak")))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          int num = binaryReader.ReadInt32();
          for (int index = 0; index < num; ++index)
          {
            string key = binaryReader.ReadString();
            int count = binaryReader.ReadInt32();
            bool flag;
            lock (MemoryContentManager.ReadLock)
              flag = MemoryContentManager.cachedAssets.ContainsKey(key);
            if (!flag)
            {
              byte[] numArray = binaryReader.ReadBytes(count);
              lock (MemoryContentManager.ReadLock)
                MemoryContentManager.cachedAssets.Add(key, numArray);
            }
            else
              binaryReader.BaseStream.Seek((long) count, SeekOrigin.Current);
          }
        }
      }
    }

    protected override Stream OpenStream(string assetName)
    {
      lock (MemoryContentManager.ReadLock)
      {
        byte[] local_0;
        if (!MemoryContentManager.cachedAssets.TryGetValue(assetName.ToLower(CultureInfo.InvariantCulture).Replace('/', '\\'), out local_0))
          throw new ContentLoadException("Can't find asset named : " + assetName);
        else
          return (Stream) new MemoryStream(local_0);
      }
    }

    public static bool AssetExists(string name)
    {
      return MemoryContentManager.cachedAssets.ContainsKey(name.Replace('/', '\\').ToLower(CultureInfo.InvariantCulture));
    }
  }
}
