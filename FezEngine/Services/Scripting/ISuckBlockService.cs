// Type: FezEngine.Services.Scripting.ISuckBlockService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (TrileGroup), RestrictTo = new ActorType[] {ActorType.SuckBlock})]
  public interface ISuckBlockService : IScriptingBase
  {
    [Description("When it's completely inside its host volume")]
    event Action<int> Sucked;

    void OnSuck(int id);

    bool get_IsSucked(int id);
  }
}
