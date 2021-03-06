﻿// Type: FezGame.Services.Scripting.SpinBlockService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Tools;

namespace FezGame.Services.Scripting
{
  public class SpinBlockService : ISpinBlockService, IScriptingBase
  {
    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    public void ResetEvents()
    {
    }

    public void SetEnabled(int id, bool enabled)
    {
      this.LevelManager.ArtObjects[id].ActorSettings.Inactive = !enabled;
    }
  }
}
