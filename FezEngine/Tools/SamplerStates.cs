// Type: FezEngine.Tools.SamplerStates
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Tools
{
  public static class SamplerStates
  {
    public static readonly SamplerState PointMipWrap = new SamplerState()
    {
      AddressU = TextureAddressMode.Wrap,
      AddressV = TextureAddressMode.Wrap,
      Filter = TextureFilter.MinLinearMagPointMipLinear
    };
    public static readonly SamplerState PointMipClamp = new SamplerState()
    {
      AddressU = TextureAddressMode.Clamp,
      AddressV = TextureAddressMode.Clamp,
      Filter = TextureFilter.MinLinearMagPointMipLinear
    };
    public static readonly SamplerState LinearUWrapVClamp = new SamplerState()
    {
      AddressU = TextureAddressMode.Wrap,
      AddressV = TextureAddressMode.Clamp,
      Filter = TextureFilter.Linear
    };
    public static readonly SamplerState PointUWrapVClamp = new SamplerState()
    {
      AddressU = TextureAddressMode.Wrap,
      AddressV = TextureAddressMode.Clamp,
      Filter = TextureFilter.Point
    };

    static SamplerStates()
    {
    }
  }
}
