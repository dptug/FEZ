// Type: FezEngine.Components.IWaiter
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Components
{
  public interface IWaiter
  {
    bool Alive { get; }

    object Tag { get; set; }

    bool AutoPause { get; set; }

    int UpdateOrder { get; set; }

    Func<bool> CustomPause { get; set; }

    void Cancel();
  }
}
