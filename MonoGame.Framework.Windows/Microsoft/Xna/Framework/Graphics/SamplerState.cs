// Type: Microsoft.Xna.Framework.Graphics.SamplerState
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Graphics.OpenGL;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class SamplerState : GraphicsResource
  {
    public static readonly SamplerState AnisotropicClamp = new SamplerState()
    {
      Filter = TextureFilter.Anisotropic,
      AddressU = TextureAddressMode.Clamp,
      AddressV = TextureAddressMode.Clamp,
      AddressW = TextureAddressMode.Clamp
    };
    public static readonly SamplerState AnisotropicWrap = new SamplerState()
    {
      Filter = TextureFilter.Anisotropic,
      AddressU = TextureAddressMode.Wrap,
      AddressV = TextureAddressMode.Wrap,
      AddressW = TextureAddressMode.Wrap
    };
    public static readonly SamplerState LinearClamp = new SamplerState()
    {
      Filter = TextureFilter.Linear,
      AddressU = TextureAddressMode.Clamp,
      AddressV = TextureAddressMode.Clamp,
      AddressW = TextureAddressMode.Clamp
    };
    public static readonly SamplerState LinearWrap = new SamplerState()
    {
      Filter = TextureFilter.Linear,
      AddressU = TextureAddressMode.Wrap,
      AddressV = TextureAddressMode.Wrap,
      AddressW = TextureAddressMode.Wrap
    };
    public static readonly SamplerState PointClamp = new SamplerState()
    {
      Filter = TextureFilter.Point,
      AddressU = TextureAddressMode.Clamp,
      AddressV = TextureAddressMode.Clamp,
      AddressW = TextureAddressMode.Clamp
    };
    public static readonly SamplerState PointWrap = new SamplerState()
    {
      Filter = TextureFilter.Point,
      AddressU = TextureAddressMode.Wrap,
      AddressV = TextureAddressMode.Wrap,
      AddressW = TextureAddressMode.Wrap
    };

    public TextureAddressMode AddressU { get; set; }

    public TextureAddressMode AddressV { get; set; }

    public TextureAddressMode AddressW { get; set; }

    public TextureFilter Filter { get; set; }

    public int MaxAnisotropy { get; set; }

    public int MaxMipLevel { get; set; }

    public float MipMapLevelOfDetailBias { get; set; }

    static SamplerState()
    {
    }

    internal void Activate(TextureTarget target, bool useMipmaps = false)
    {
      switch (this.Filter)
      {
        case TextureFilter.Linear:
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9987 : 9729);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9729);
          break;
        case TextureFilter.Point:
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9984 : 9728);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9728);
          break;
        case TextureFilter.Anisotropic:
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9987 : 9729);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9729);
          break;
        case TextureFilter.MinLinearMagPointMipLinear:
          GL.TexParameter(target, TextureParameterName.TextureMinFilter, useMipmaps ? 9987 : 9729);
          GL.TexParameter(target, TextureParameterName.TextureMagFilter, 9728);
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
        case TextureAddressMode.Clamp:
          return 33071;
        case TextureAddressMode.Mirror:
          return 33648;
        case TextureAddressMode.Wrap:
          return 10497;
        default:
          throw new NotImplementedException("No support for " + (object) textureAddressMode);
      }
    }
  }
}
