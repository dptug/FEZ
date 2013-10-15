// Type: Microsoft.Xna.Framework.DisplayOrientation
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework
{
  [Flags]
  public enum DisplayOrientation
  {
    Default = 0,
    LandscapeLeft = 1,
    LandscapeRight = 2,
    Portrait = 4,
    PortraitDown = 8,
    Unknown = 16,
  }
}
