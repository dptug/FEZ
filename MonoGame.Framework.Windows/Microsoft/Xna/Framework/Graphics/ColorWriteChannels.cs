// Type: Microsoft.Xna.Framework.Graphics.ColorWriteChannels
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Graphics
{
  [Flags]
  public enum ColorWriteChannels
  {
    None = 0,
    Red = 1,
    Green = 2,
    Blue = 4,
    Alpha = 8,
    All = Alpha | Blue | Green | Red,
  }
}
