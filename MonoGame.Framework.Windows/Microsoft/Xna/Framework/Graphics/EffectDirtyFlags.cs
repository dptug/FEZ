// Type: Microsoft.Xna.Framework.Graphics.EffectDirtyFlags
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Graphics
{
  [Flags]
  internal enum EffectDirtyFlags
  {
    WorldViewProj = 1,
    World = 2,
    EyePosition = 4,
    MaterialColor = 8,
    Fog = 16,
    FogEnable = 32,
    AlphaTest = 64,
    ShaderIndex = 128,
    All = -1,
  }
}
