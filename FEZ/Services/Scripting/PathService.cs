// Type: FezGame.Services.Scripting.PathService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Tools;
using System;

namespace FezGame.Services.Scripting
{
  internal class PathService : IPathService, IScriptingBase
  {
    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    public LongRunningAction Start(int id, bool inTransition, bool outTransition)
    {
      this.LevelManager.Paths[id].NeedsTrigger = false;
      this.LevelManager.Paths[id].RunOnce = true;
      this.LevelManager.Paths[id].InTransition = inTransition;
      this.LevelManager.Paths[id].OutTransition = outTransition;
      return new LongRunningAction((Func<float, float, bool>) ((elapsed, sinceStarted) => this.LevelManager.Paths[id].NeedsTrigger));
    }

    public void ResetEvents()
    {
    }
  }
}
