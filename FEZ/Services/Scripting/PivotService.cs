// Type: FezGame.Services.Scripting.PivotService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Tools;
using FezGame.Services;
using System;

namespace FezGame.Services.Scripting
{
  internal class PivotService : IPivotService, IScriptingBase
  {
    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public event Action<int> RotatedRight = new Action<int>(Util.NullAction<int>);

    public event Action<int> RotatedLeft = new Action<int>(Util.NullAction<int>);

    public void ResetEvents()
    {
      this.RotatedRight = new Action<int>(Util.NullAction<int>);
      this.RotatedLeft = new Action<int>(Util.NullAction<int>);
    }

    public void OnRotateRight(int id)
    {
      this.RotatedRight(id);
    }

    public void OnRotateLeft(int id)
    {
      this.RotatedLeft(id);
    }

    public int get_Turns(int id)
    {
      int num;
      if (!this.GameState.SaveData.ThisLevel.PivotRotations.TryGetValue(id, out num))
        return 0;
      else
        return num;
    }

    public void SetEnabled(int id, bool enabled)
    {
      this.LevelManager.ArtObjects[id].Enabled = enabled;
    }

    public void RotateTo(int id, int turns)
    {
      this.GameState.SaveData.ThisLevel.PivotRotations[id] = turns;
    }
  }
}
