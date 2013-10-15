// Type: FezGame.Services.Scripting.ScriptService
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
  public class ScriptService : IScriptService, IScriptingBase
  {
    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    public event Action<int> Complete = new Action<int>(Util.NullAction<int>);

    public void OnComplete(int id)
    {
      this.Complete(id);
    }

    public void SetEnabled(int id, bool enabled)
    {
      this.LevelManager.Scripts[id].Disabled = !enabled;
    }

    public void Evaluate(int id)
    {
      this.LevelManager.Scripts[id].ScheduleEvalulation = true;
    }

    public void ResetEvents()
    {
      this.Complete = new Action<int>(Util.NullAction<int>);
    }
  }
}
