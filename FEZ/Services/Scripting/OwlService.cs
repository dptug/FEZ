// Type: FezGame.Services.Scripting.OwlService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services.Scripting;
using FezEngine.Tools;
using FezGame.Services;
using System;

namespace FezGame.Services.Scripting
{
  internal class OwlService : IOwlService, IScriptingBase
  {
    public int OwlsCollected
    {
      get
      {
        return this.GameState.SaveData.CollectedOwls;
      }
    }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public event Action OwlCollected;

    public event Action OwlLanded;

    public void ResetEvents()
    {
      this.OwlCollected = (Action) null;
      this.OwlLanded = (Action) null;
    }

    public void OnOwlCollected()
    {
      if (this.OwlCollected == null)
        return;
      this.OwlCollected();
    }

    public void OnOwlLanded()
    {
      if (this.OwlLanded == null)
        return;
      this.OwlLanded();
    }
  }
}
