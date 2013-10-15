// Type: FezEngine.Services.Scripting.IScriptService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (Script))]
  public interface IScriptService : IScriptingBase
  {
    [Description("When the script timeouts or terminates")]
    event Action<int> Complete;

    void OnComplete(int id);

    [Description("Enables or disables a script")]
    void SetEnabled(int id, bool enabled);

    [Description("Evaluates a script")]
    void Evaluate(int id);
  }
}
