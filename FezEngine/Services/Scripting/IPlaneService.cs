// Type: FezEngine.Services.Scripting.IPlaneService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Components.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (BackgroundPlane))]
  public interface IPlaneService : IScriptingBase
  {
    LongRunningAction FadeIn(int id, float seconds);

    LongRunningAction FadeOut(int id, float seconds);

    LongRunningAction Flicker(int id, float factor);
  }
}
