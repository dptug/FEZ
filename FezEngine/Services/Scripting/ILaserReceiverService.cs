// Type: FezEngine.Services.Scripting.ILaserReceiverService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (ArtObjectInstance), RestrictTo = new ActorType[] {ActorType.LaserReceiver})]
  public interface ILaserReceiverService : IScriptingBase
  {
    [Description("When a receiver receives a laser")]
    event Action<int> Activate;

    void OnActivated(int id);
  }
}
