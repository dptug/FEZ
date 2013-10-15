// Type: FezEngine.Services.Scripting.IDotService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Static = true)]
  public interface IDotService : IScriptingBase
  {
    [Description("Makes Dot say a custom text line")]
    LongRunningAction Say(string line, bool nearGomez, bool hideAfter);

    [Description("Hides Dot in Gomez's hat")]
    LongRunningAction ComeBackAndHide(bool withCamera);

    [Description("Spiral around the level, yo")]
    LongRunningAction SpiralAround(bool withCamera, bool hideDot);
  }
}
