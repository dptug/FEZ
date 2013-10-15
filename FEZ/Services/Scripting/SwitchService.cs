// Type: FezGame.Services.Scripting.SwitchService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using System;

namespace FezGame.Services.Scripting
{
  public class SwitchService : ISwitchService, IScriptingBase
  {
    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    public event Action<int> Explode = new Action<int>(Util.NullAction<int>);

    public event Action<int> Push = new Action<int>(Util.NullAction<int>);

    public event Action<int> Lift = new Action<int>(Util.NullAction<int>);

    public void OnExplode(int id)
    {
      this.Explode(id);
    }

    public void OnPush(int id)
    {
      this.Push(id);
    }

    public void OnLift(int id)
    {
      this.Lift(id);
    }

    public void Activate(int id)
    {
      this.OnExplode(id);
      this.OnPush(id);
    }

    public LongRunningAction ChangeTrile(int id, int newTrileId)
    {
      int[] oldTrileId = new int[this.LevelManager.Groups[id].Triles.Count];
      for (int index = 0; index < oldTrileId.Length; ++index)
      {
        TrileInstance instance = this.LevelManager.Groups[id].Triles[index];
        oldTrileId[index] = instance.Trile.Id;
        this.LevelManager.SwapTrile(instance, this.LevelManager.SafeGetTrile(newTrileId));
      }
      return new LongRunningAction((Action) (() =>
      {
        TrileGroup local_0;
        if (!this.LevelManager.Groups.TryGetValue(id, out local_0))
          return;
        for (int local_1 = 0; local_1 < oldTrileId.Length; ++local_1)
          this.LevelManager.SwapTrile(local_0.Triles[local_1], this.LevelManager.SafeGetTrile(oldTrileId[local_1]));
      }));
    }

    public void ResetEvents()
    {
      this.Explode = new Action<int>(Util.NullAction<int>);
      this.Push = new Action<int>(Util.NullAction<int>);
      this.Lift = new Action<int>(Util.NullAction<int>);
    }
  }
}
