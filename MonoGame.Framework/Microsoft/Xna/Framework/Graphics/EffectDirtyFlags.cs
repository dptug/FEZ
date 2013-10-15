// Type: Microsoft.Xna.Framework.Graphics.EffectDirtyFlags
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
