// Type: SharpDX.Multimedia.WaveFormatExtensible
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX.Multimedia
{
  public class WaveFormatExtensible : WaveFormat
  {
    private short wValidBitsPerSample;
    public Guid GuidSubFormat;
    public Speakers ChannelMask;

    internal WaveFormatExtensible()
    {
    }

    public WaveFormatExtensible(int rate, int bits, int channels)
      : base(rate, bits, channels)
    {
      this.waveFormatTag = WaveFormatEncoding.Extensible;
      this.extraSize = (short) 22;
      this.wValidBitsPerSample = (short) bits;
      int num = 0;
      for (int index = 0; index < channels; ++index)
        num |= 1 << index;
      this.ChannelMask = (Speakers) num;
      this.GuidSubFormat = bits == 32 ? new Guid("00000003-0000-0010-8000-00aa00389b71") : new Guid("00000001-0000-0010-8000-00aa00389b71");
    }

    protected override unsafe IntPtr MarshalToPtr()
    {
      IntPtr num = Marshal.AllocHGlobal(Utilities.SizeOf<WaveFormatExtensible.__Native>());
      this.__MarshalTo(ref *(WaveFormatExtensible.__Native*) (void*) num);
      return num;
    }

    internal void __MarshalFrom(ref WaveFormatExtensible.__Native @ref)
    {
      base.__MarshalFrom(ref @ref.waveFormat);
      this.wValidBitsPerSample = @ref.wValidBitsPerSample;
      this.ChannelMask = @ref.dwChannelMask;
      this.GuidSubFormat = @ref.subFormat;
    }

    internal void __MarshalTo(ref WaveFormatExtensible.__Native @ref)
    {
      base.__MarshalTo(ref @ref.waveFormat);
      @ref.wValidBitsPerSample = this.wValidBitsPerSample;
      @ref.dwChannelMask = this.ChannelMask;
      @ref.subFormat = this.GuidSubFormat;
    }

    internal static WaveFormatExtensible.__Native __NewNative()
    {
      return new WaveFormatExtensible.__Native()
      {
        waveFormat = {
          extraSize = (short) 22
        }
      };
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} wBitsPerSample:{1} ChannelMask:{2} SubFormat:{3} extraSize:{4}", (object) base.ToString(), (object) this.wValidBitsPerSample, (object) this.ChannelMask, (object) this.GuidSubFormat, (object) this.extraSize);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct __Native
    {
      public WaveFormat.__Native waveFormat;
      public short wValidBitsPerSample;
      public Speakers dwChannelMask;
      public Guid subFormat;

      internal void __MarshalFree()
      {
        this.waveFormat.__MarshalFree();
      }
    }
  }
}
