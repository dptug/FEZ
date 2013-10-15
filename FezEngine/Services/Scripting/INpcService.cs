// Type: FezEngine.Services.Scripting.INpcService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (NpcInstance))]
  public interface INpcService : IScriptingBase
  {
    [Description("Makes the NPC say a custom text line")]
    LongRunningAction Say(int id, string line, string customSound, string customAnimation);

    [Description("CarryGeezerLetter")]
    void CarryGeezerLetter(int id);
  }
}
