// Type: FezEngine.Services.Scripting.IOwlService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Static = true)]
  public interface IOwlService : IScriptingBase
  {
    [Description("Number of owls collected up to now")]
    int OwlsCollected { get; }

    event Action OwlCollected;

    event Action OwlLanded;

    void OnOwlCollected();

    void OnOwlLanded();
  }
}
