// Type: OpenTK.Platform.Windows.PixelFormatDescriptor
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Platform.Windows
{
  internal struct PixelFormatDescriptor
  {
    internal short Size;
    internal short Version;
    internal PixelFormatDescriptorFlags Flags;
    internal PixelType PixelType;
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
    internal byte LayerType;
    private byte Reserved;
    internal int LayerMask;
    internal int VisibleMask;
    internal int DamageMask;
  }
}
