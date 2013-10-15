// Type: FezEngine.Services.Scripting.IValveService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (ArtObjectInstance), RestrictTo = new ActorType[] {ActorType.Valve, ActorType.BoltHandle})]
  public interface IValveService : IScriptingBase
  {
    [Description("When it's unscrewed")]
    event Action<int> Screwed;

    [Description("When it's screwed in")]
    event Action<int> Unscrewed;

    void OnScrew(int id);

    void OnUnscrew(int id);

    [Description("Enables or disables a valve's rotatability")]
    void SetEnabled(int id, bool enabled);
  }
}
