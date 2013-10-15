// Type: FezGame.Services.Scripting.ValveService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Tools;
using System;

namespace FezGame.Services.Scripting
{
  internal class ValveService : IValveService, IScriptingBase
  {
    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    public event Action<int> Screwed = new Action<int>(Util.NullAction<int>);

    public event Action<int> Unscrewed = new Action<int>(Util.NullAction<int>);

    public void ResetEvents()
    {
      this.Screwed = new Action<int>(Util.NullAction<int>);
      this.Unscrewed = new Action<int>(Util.NullAction<int>);
    }

    public void OnScrew(int id)
    {
      this.Screwed(id);
    }

    public void OnUnscrew(int id)
    {
      this.Unscrewed(id);
    }

    public void SetEnabled(int id, bool enabled)
    {
      this.LevelManager.ArtObjects[id].Enabled = enabled;
    }
  }
}
