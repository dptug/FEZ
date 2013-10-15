// Type: Microsoft.Xna.Framework.DisplayOrientation
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework
{
  [Flags]
  public enum DisplayOrientation
  {
    Default = 1,
    LandscapeLeft = 2,
    LandscapeRight = 4,
    Portrait = 8,
    FaceDown = 16,
    FaceUp = 32,
    PortraitUpsideDown = 64,
    Unknown = 128,
  }
}
