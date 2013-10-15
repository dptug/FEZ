// Type: FezEngine.Services.IFogManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public interface IFogManager
  {
    FogType Type { get; set; }

    Color Color { get; set; }

    float Density { get; set; }

    float Start { get; set; }

    float End { get; set; }

    event Action FogSettingsChanged;
  }
}
