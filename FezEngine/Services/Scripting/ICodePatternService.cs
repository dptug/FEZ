// Type: FezEngine.Services.Scripting.ICodePatternService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (ArtObjectInstance), RestrictTo = new ActorType[] {ActorType.Rumbler, ActorType.CodeMachine, ActorType.QrCode})]
  public interface ICodePatternService : IScriptingBase
  {
    [Description("When the right pattern is input")]
    event Action<int> Activated;

    void OnActivate(int id);
  }
}
