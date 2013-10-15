// Type: FezEngine.Services.ContentManagerProvider
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FezEngine.Services
{
  public class ContentManagerProvider : GameComponent, IContentManagerProvider
  {
    private readonly ContentManager global;
    private readonly Dictionary<string, SharedContentManager> levelScope;
    private readonly Dictionary<CM, SharedContentManager> temporary;

    public ContentManager Global
    {
      get
      {
        return this.global;
      }
    }

    public ContentManager CurrentLevel
    {
      get
      {
        return this.GetForLevel(this.LevelManager.Name ?? "");
      }
    }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    public ContentManagerProvider(Game game)
      : base(game)
    {
      this.global = (ContentManager) new SharedContentManager("Global");
      this.levelScope = new Dictionary<string, SharedContentManager>();
      this.temporary = new Dictionary<CM, SharedContentManager>();
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanged += new Action(this.CleanAndPrecache);
    }

    private void CleanAndPrecache()
    {
      foreach (string key in Enumerable.ToArray<string>((IEnumerable<string>) this.levelScope.Keys))
      {
        if (key != this.LevelManager.Name)
        {
          this.levelScope[key].Dispose();
          this.levelScope.Remove(key);
        }
      }
    }

    public ContentManager GetForLevel(string levelName)
    {
      SharedContentManager sharedContentManager;
      if (!this.levelScope.TryGetValue(levelName, out sharedContentManager))
      {
        this.levelScope.Add(levelName, sharedContentManager = new SharedContentManager(levelName));
        sharedContentManager.RootDirectory = this.global.RootDirectory;
      }
      return (ContentManager) sharedContentManager;
    }

    public ContentManager Get(CM name)
    {
      SharedContentManager sharedContentManager;
      if (!this.temporary.TryGetValue(name, out sharedContentManager))
      {
        this.temporary.Add(name, sharedContentManager = new SharedContentManager(((object) name).ToString()));
        sharedContentManager.RootDirectory = this.global.RootDirectory;
      }
      return (ContentManager) sharedContentManager;
    }

    public void Dispose(CM name)
    {
      SharedContentManager sharedContentManager;
      if (!this.temporary.TryGetValue(name, out sharedContentManager))
        return;
      sharedContentManager.Dispose();
      this.temporary.Remove(name);
    }

    public IEnumerable<string> GetAllIn(string directory)
    {
      directory = directory.Replace('/', '\\').ToLower(CultureInfo.InvariantCulture);
      return Enumerable.Where<string>(MemoryContentManager.AssetNames, (Func<string, bool>) (x => x.StartsWith(directory)));
    }
  }
}
