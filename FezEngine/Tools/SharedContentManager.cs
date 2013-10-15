// Type: FezEngine.Tools.SharedContentManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Effects.Structures;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace FezEngine.Tools
{
  public class SharedContentManager : ContentManager
  {
    private static readonly SharedContentManager.CommonContentManager Common = new SharedContentManager.CommonContentManager((IServiceProvider) ServiceHelper.Game.Services, ServiceHelper.Game.Content.RootDirectory);
    private readonly string Name;
    private List<string> loadedAssets;

    static SharedContentManager()
    {
      SharedContentManager.Common.LoadEssentials();
    }

    public SharedContentManager(string name)
      : base((IServiceProvider) ServiceHelper.Game.Services, ServiceHelper.Game.Content.RootDirectory)
    {
      this.Name = name;
      this.loadedAssets = new List<string>();
    }

    public static string GetCleanPath(string path)
    {
      path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
      for (int startIndex1 = 1; startIndex1 < path.Length; {
        int startIndex2;
        startIndex1 = Math.Max(startIndex2 - 1, 1);
      }
      )
      {
        int num = path.IndexOf("\\..\\", startIndex1);
        if (num < 0)
          return path;
        startIndex2 = path.LastIndexOf(Path.DirectorySeparatorChar, num - 1) + 1;
        path = path.Remove(startIndex2, num - startIndex2 + "\\..\\".Length);
      }
      return path;
    }

    public override T Load<T>(string assetName)
    {
      assetName = SharedContentManager.GetCleanPath(assetName);
      this.loadedAssets.Add(assetName);
      return SharedContentManager.Common.Load<T>(this.Name, assetName);
    }

    public override void Unload()
    {
      if (this.loadedAssets == null)
        throw new ObjectDisposedException(typeof (SharedContentManager).Name);
      SharedContentManager.Common.Unload(this);
      this.loadedAssets = (List<string>) null;
      base.Unload();
    }

    public static void Preload()
    {
      SharedContentManager.Common.Preload();
    }

    private class CommonContentManager : MemoryContentManager
    {
      private readonly Dictionary<string, SharedContentManager.CommonContentManager.ReferencedAsset> references = new Dictionary<string, SharedContentManager.CommonContentManager.ReferencedAsset>();

      public CommonContentManager(IServiceProvider serviceProvider, string rootDirectory)
        : base(serviceProvider, rootDirectory)
      {
      }

      public T Load<T>(string name, string assetName)
      {
        lock (this)
        {
          assetName = SharedContentManager.GetCleanPath(assetName);
          SharedContentManager.CommonContentManager.ReferencedAsset local_0;
          if (!this.references.TryGetValue(assetName, out local_0))
          {
            if (TraceFlags.TraceContentLoad)
              Logger.Log("Content", "[" + name + "] Loading " + typeof (T).Name + " " + assetName);
            this.references.Add(assetName, local_0 = new SharedContentManager.CommonContentManager.ReferencedAsset()
            {
              Asset = (object) this.ReadAsset<T>(assetName)
            });
          }
          ++local_0.References;
          if (local_0.Asset is SoundEffect)
            (local_0.Asset as SoundEffect).Name = assetName.Substring("Sounds/".Length);
          return (T) local_0.Asset;
        }
      }

      private T ReadAsset<T>(string assetName)
      {
        return base.ReadAsset<T>(assetName, new Action<IDisposable>(Util.NullAction<IDisposable>));
      }

      public void Unload(SharedContentManager container)
      {
        lock (this)
        {
          foreach (string item_0 in container.loadedAssets)
          {
            SharedContentManager.CommonContentManager.ReferencedAsset local_1;
            if (!this.references.TryGetValue(item_0, out local_1))
            {
              Logger.Log("Content", Common.LogSeverity.Warning, "Couldn't find asset in references : " + item_0);
            }
            else
            {
              --local_1.References;
              if (local_1.References == 0)
              {
                if (local_1.Asset is Texture)
                  TextureExtensions.Unhook(local_1.Asset as Texture);
                if (local_1.Asset is IDisposable)
                  (local_1.Asset as IDisposable).Dispose();
                this.references.Remove(item_0);
                local_1.Asset = (object) null;
              }
            }
          }
        }
      }

      private class ReferencedAsset
      {
        public object Asset;
        public int References;
      }
    }
  }
}
