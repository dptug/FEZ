// Type: FezEngine.Services.Scripting.IPivotService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (ArtObjectInstance), RestrictTo = new ActorType[] {ActorType.PivotHandle})]
  public interface IPivotService : IScriptingBase
  {
    [Description("When it's been rotated right")]
    event Action<int> RotatedRight;

    [Description("When it's been rotated left")]
    event Action<int> RotatedLeft;

    void OnRotateRight(int id);

    void OnRotateLeft(int id);

    [Description("Gets the number of turns it's relative to the original state")]
    int get_Turns(int id);

    [Description("Enables or disables a pivot handle's rotatability")]
    void SetEnabled(int id, bool enabled);

    [Description("Enables or disables a pivot handle's rotatability")]
    void RotateTo(int id, int turns);
  }
}
