// Type: FezEngine.Services.LightEventArgs
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Services
{
  public class LightEventArgs : EventArgs
  {
    private readonly int lightNumber;

    public int LightNumber
    {
      get
      {
        return this.lightNumber;
      }
    }

    public LightEventArgs(int lightNumber)
    {
      this.lightNumber = lightNumber;
    }
  }
}
