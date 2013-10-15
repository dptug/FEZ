// Type: Microsoft.Xna.Framework.Graphics.TextureCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class TextureCollection
  {
    private readonly Texture[] _textures;
    private readonly TextureTarget[] _targets;
    private int _dirty;

    public Texture this[int index]
    {
      get
      {
        return this._textures[index];
      }
      set
      {
        if (this._textures[index] == value)
          return;
        this._textures[index] = value;
        this._dirty |= 1 << index;
      }
    }

    internal TextureCollection(int maxTextures)
    {
      this._textures = new Texture[maxTextures];
      this._dirty = int.MaxValue;
      this._targets = new TextureTarget[maxTextures];
    }

    internal void Clear()
    {
      for (int index = 0; index < this._textures.Length; ++index)
      {
        this._textures[index] = (Texture) null;
        this._targets[index] = (TextureTarget) 0;
      }
      this._dirty = int.MaxValue;
    }

    internal void Dirty()
    {
      this._dirty = int.MaxValue;
    }

    internal void SetTextures(GraphicsDevice device)
    {
      Threading.EnsureUIThread();
      if (this._dirty == 0)
        return;
      for (int index = 0; index < this._textures.Length; ++index)
      {
        int num = 1 << index;
        if ((this._dirty & num) != 0)
        {
          Texture texture = this._textures[index];
          GL.ActiveTexture((TextureUnit) (33984 + index));
          if (this._targets[index] != (TextureTarget) 0 && (texture == null || this._targets[index] != texture.glTarget))
            GL.BindTexture(this._targets[index], 0);
          if (texture != null)
          {
            this._targets[index] = texture.glTarget;
            GL.BindTexture(texture.glTarget, texture.glTexture);
          }
          this._dirty &= ~num;
          if (this._dirty == 0)
            break;
        }
      }
      this._dirty = 0;
    }
  }
}
