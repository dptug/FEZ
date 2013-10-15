// Type: FezEngine.Services.Scripting.ITimeswitchService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (ArtObjectInstance), RestrictTo = new ActorType[] {ActorType.Timeswitch})]
  public interface ITimeswitchService : IScriptingBase
  {
    [Description("When the screw minimally sticks out from the base (it's been screwed out)")]
    event Action<int> ScrewedOut;

    [Description("When it stop winding back in (hits the base)")]
    event Action<int> HitBase;

    void OnScrewedOut(int id);

    void OnHitBase(int id);
  }
}
