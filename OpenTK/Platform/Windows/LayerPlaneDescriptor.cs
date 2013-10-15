// Type: OpenTK.Platform.Windows.LayerPlaneDescriptor
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Runtime.InteropServices;

namespace OpenTK.Platform.Windows
{
  internal struct LayerPlaneDescriptor
  {
    internal static readonly short Size = (short) Marshal.SizeOf(typeof (LayerPlaneDescriptor));
    internal short Version;
    internal int Flags;
    internal byte PixelType;
    internal byte ColorBits;
    internal byte RedBits;
    internal byte RedShift;
    internal byte GreenBits;
    internal byte GreenShift;
    internal byte BlueBits;
    internal byte BlueShift;
    internal byte AlphaBits;
    internal byte AlphaShift;
    internal byte AccumBits;
    internal byte AccumRedBits;
    internal byte AccumGreenBits;
    internal byte AccumBlueBits;
    internal byte AccumAlphaBits;
    internal byte DepthBits;
    internal byte StencilBits;
    internal byte AuxBuffers;
    internal byte LayerPlane;
    private byte Reserved;
    internal int crTransparent;

    static LayerPlaneDescriptor()
    {
    }
  }
}
