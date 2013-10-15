// Type: FezEngine.Services.Scripting.IRotatingGroupService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (TrileGroup), RestrictTo = new ActorType[] {ActorType.RotatingGroup})]
  public interface IRotatingGroupService : IScriptingBase
  {
    void Rotate(int id, bool clockwise, int turns);

    void SetEnabled(int id, bool enabled);
  }
}
