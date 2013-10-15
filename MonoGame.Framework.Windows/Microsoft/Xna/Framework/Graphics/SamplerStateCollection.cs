// Type: Microsoft.Xna.Framework.Graphics.SamplerStateCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Graphics.OpenGL;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class SamplerStateCollection
  {
    private SamplerState[] _samplers;

    public SamplerState this[int index]
    {
      get
      {
        return this._samplers[index];
      }
      set
      {
        if (this._samplers[index] == value)
          return;
        this._samplers[index] = value;
      }
    }

    internal SamplerStateCollection(int maxSamplers)
    {
      this._samplers = new SamplerState[maxSamplers];
      for (int index = 0; index < maxSamplers; ++index)
        this._samplers[index] = SamplerState.LinearWrap;
    }

    internal void Clear()
    {
      for (int index = 0; index < this._samplers.Length; ++index)
        this._samplers[index] = (SamplerState) null;
    }

    internal void SetSamplers(GraphicsDevice device)
    {
      for (int index = 0; index < this._samplers.Length; ++index)
      {
        SamplerState samplerState = this._samplers[index];
        Texture texture = device.Textures[index];
        if (samplerState != null && texture != null && samplerState != texture.glLastSamplerState)
        {
          GL.ActiveTexture((TextureUnit) (33984 + index));
          samplerState.Activate(texture.glTarget, texture.LevelCount > 1);
          texture.glLastSamplerState = samplerState;
        }
      }
    }
  }
}
