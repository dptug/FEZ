// Type: Microsoft.Xna.Framework.Graphics.SamplerState
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class SamplerState : GraphicsResource
  {
    private static float MaxTextureMaxAnisotropy = 4f;
    private const GetPName GetPNameMaxTextureMaxAnisotropy = (GetPName) 34047;
    private const TextureParameterName TextureParameterNameTextureMaxAnisotropy = (TextureParameterName) 34046;
    public static readonly SamplerState AnisotropicClamp;
    public static readonly SamplerState AnisotropicWrap;
    public static readonly SamplerState LinearClamp;
    public static readonly SamplerState LinearWrap;
    public static readonly SamplerState PointClamp;
    public static readonly SamplerState PointWrap;

    public TextureAddressMode AddressU { get; set; }

    public TextureAddressMode AddressV { get; set; }

    public TextureAddressMode AddressW { get; set; }

    public TextureFilter Filter { get; set; }

    public int MaxAnisotropy { get; set; }

    public int MaxMipLevel { get; set; }

    public float MipMapLevelOfDetailBias { get; set; }

    static SamplerState()
    {
      GL.GetFloat((GetPName) 34047, out SamplerState.MaxTextureMaxAnisotropy);
      SamplerState.AnisotropicClamp = new SamplerState()
      {
        Filter = TextureFilter.Anisotropic,
        AddressU = TextureAddressMode.Clamp,
        AddressV = TextureAddressMode.Clamp,
        AddressW = TextureAddressMode.Clamp
      };
      SamplerState.AnisotropicWrap = new SamplerState()
      {
        Filter = TextureFilter.Anisotropic,
        AddressU = TextureAddressMode.Wrap,
        AddressV = TextureAddressMode.Wrap,
        AddressW = TextureAddressMode.Wrap
      };
      SamplerState.LinearClamp = new SamplerState()
      {
        Filter = TextureFilter.Linear,
        AddressU = TextureAddressMode.Clamp,
        AddressV = TextureAddressMode.Clamp,
        AddressW = TextureAddressMode.Clamp
      };
      SamplerState.LinearWrap = new SamplerState()
      {
        Filter = TextureFilter.Linear,
        AddressU = TextureAddressMode.Wrap,
        AddressV = TextureAddressMode.Wrap,
        AddressW = TextureAddressMode.Wrap
      };
      SamplerState.PointClamp = new SamplerState()
      {
        Filter = TextureFilter.Point,
        AddressU = TextureAddressMode.Clamp,
        AddressV = TextureAddressMode.Clamp,
        AddressW = TextureAddressMode.Clamp
      };
      SamplerState.PointWrap = new SamplerState()
      {
        Filter = TextureFilter.Point,
        AddressU = TextureAddressMode.Wrap,
        AddressV = TextureAddressMode.Wrap,
        AddressW = TextureAddressMode.Wrap
      };
    }

    public SamplerState()
    {
      this.Filter = TextureFilter.Linear;
      this.AddressU = TextureAddressMode.Wrap;
      this.AddressV = TextureAddressMode.Wrap;
      this.AddressW = TextureAddressMode.Wrap;
      this.MaxAnisotropy = 4;
      this.MaxMipLevel = 0;
      this.MipMapLevelOfDetailBias = 0.0f;
    }

    internal void Activate(TextureTarget target, bool useMipmaps = false)
    {
      switch (this.Filter)
      {
        case TextureFilter.Linear:
          if (GraphicsCapabilities.TextureFilterAnisotric)
            GL.TexParameter(target, (TextureParameterName) 34046, 1f);
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9987 : 9729);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9729);
          break;
        case TextureFilter.Point:
          if (GraphicsCapabilities.TextureFilterAnisotric)
            GL.TexParameter(target, (TextureParameterName) 34046, 1f);
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9984 : 9728);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9728);
          break;
        case TextureFilter.Anisotropic:
          if (GraphicsCapabilities.TextureFilterAnisotric)
            GL.TexParameter(target, (TextureParameterName) 34046, MathHelper.Clamp((float) this.MaxAnisotropy, 1f, SamplerState.MaxTextureMaxAnisotropy));
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9987 : 9729);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9729);
          break;
        case TextureFilter.LinearMipPoint:
          if (GraphicsCapabilities.TextureFilterAnisotric)
            GL.TexParameter(target, (TextureParameterName) 34046, 1f);
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9985 : 9729);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9729);
          break;
        case TextureFilter.PointMipLinear:
          if (GraphicsCapabilities.TextureFilterAnisotric)
            GL.TexParameter(target, (TextureParameterName) 34046, 1f);
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9986 : 9728);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9728);
          break;
        case TextureFilter.MinLinearMagPointMipLinear:
          if (GraphicsCapabilities.TextureFilterAnisotric)
            GL.TexParameter(target, (TextureParameterName) 34046, 1f);
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9987 : 9729);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9728);
          break;
        case TextureFilter.MinLinearMagPointMipPoint:
          if (GraphicsCapabilities.TextureFilterAnisotric)
            GL.TexParameter(target, (TextureParameterName) 34046, 1f);
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9985 : 9729);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9728);
          break;
        case TextureFilter.MinPointMagLinearMipLinear:
          if (GraphicsCapabilities.TextureFilterAnisotric)
            GL.TexParameter(target, (TextureParameterName) 34046, 1f);
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9986 : 9728);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9729);
          break;
        case TextureFilter.MinPointMagLinearMipPoint:
          if (GraphicsCapabilities.TextureFilterAnisotric)
            GL.TexParameter(target, (TextureParameterName) 34046, 1f);
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9984 : 9728);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9729);
          break;
        default:
          throw new NotImplementedException();
      }
      GL.TexParameter(target, TextureParameterName.TextureWrapS, this.GetWrapMode(this.AddressU));
      GL.TexParameter(target, TextureParameterName.TextureWrapT, this.GetWrapMode(this.AddressV));
    }

    private int GetWrapMode(TextureAddressMode textureAddressMode)
    {
      switch (textureAddressMode)
      {
        case TextureAddressMode.Wrap:
          return 10497;
        case TextureAddressMode.Clamp:
          return 33071;
        case TextureAddressMode.Mirror:
          return 33648;
        default:
          throw new NotImplementedException("No support for " + (object) textureAddressMode);
      }
    }
  }
}
