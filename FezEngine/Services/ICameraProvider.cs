// Type: FezEngine.Services.ICameraProvider
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public interface ICameraProvider
  {
    Matrix Projection { get; }

    Matrix View { get; }

    event Action ViewChanged;

    event Action ProjectionChanged;
  }
}
