// Type: FezEngine.Services.Scripting.ISwitchService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (TrileGroup), RestrictTo = new ActorType[] {ActorType.PushSwitch, ActorType.ExploSwitch, ActorType.PushSwitchPermanent})]
  public interface ISwitchService : IScriptingBase
  {
    [Description("When a bomb explodes near this switch")]
    event Action<int> Explode;

    [EndTrigger("Lift")]
    [Description("When this switch is pushed completely")]
    event Action<int> Push;

    [Description("When this switch is lifted back up")]
    event Action<int> Lift;

    void OnExplode(int id);

    void OnPush(int id);

    void OnLift(int id);

    [Description("Activates this switch")]
    void Activate(int id);

    [Description("Changes the visual of this switch's triles")]
    LongRunningAction ChangeTrile(int id, int newTrileId);
  }
}
