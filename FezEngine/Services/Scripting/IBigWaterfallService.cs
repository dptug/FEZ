// Type: FezEngine.Services.Scripting.IBigWaterfallService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Components.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (BackgroundPlane), RestrictTo = new ActorType[] {ActorType.BigWaterfall})]
  public interface IBigWaterfallService : IScriptingBase
  {
    LongRunningAction Open(int id);
  }
}
