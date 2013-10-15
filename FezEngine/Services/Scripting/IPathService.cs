// Type: FezEngine.Services.Scripting.IPathService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (MovementPath))]
  public interface IPathService : IScriptingBase
  {
    [Description("Applies the whole path to the camera")]
    LongRunningAction Start(int id, bool inTransition, bool outTransition);
  }
}
