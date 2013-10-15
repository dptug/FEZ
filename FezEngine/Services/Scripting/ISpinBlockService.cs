// Type: FezEngine.Services.Scripting.ISpinBlockService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (ArtObjectInstance), RestrictTo = new ActorType[] {ActorType.SpinBlock})]
  public interface ISpinBlockService : IScriptingBase
  {
    [Description("Enables or disables a spinblock (which ceases or resumes its spinning)")]
    void SetEnabled(int id, bool enabled);
  }
}
