// Type: SharpDX.Multimedia.WaveFormatAdpcm
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;

namespace SharpDX.Multimedia
{
  public class WaveFormatAdpcm : WaveFormat
  {
    public ushort SamplesPerBlock { get; private set; }

    public short[] Coefficients1 { get; set; }

    public short[] Coefficients2 { get; set; }

    internal WaveFormatAdpcm()
    {
    }

    public WaveFormatAdpcm(int rate, int channels, int blockAlign = 0)
      : base(rate, 4, channels)
    {
      this.waveFormatTag = WaveFormatEncoding.Adpcm;
      this.blockAlign = (short) blockAlign;
      if (blockAlign == 0)
        blockAlign = rate > 11025 ? (rate > 22050 ? 1024 : 512) : 256;
      this.SamplesPerBlock = (ushort) (blockAlign * 2 / channels - 12);
      this.averageBytesPerSecond = this.SampleRate * blockAlign / (int) this.SamplesPerBlock;
      this.Coefficients1 = new short[7]
      {
        (short) 256,
        (short) 512,
        (short) 0,
        (short) 192,
        (short) 240,
        (short) 460,
        (short) 392
      };
      this.Coefficients2 = new short[7]
      {
        (short) 0,
        (short) -256,
        (short) 0,
        (short) 64,
        (short) 0,
        (short) -208,
        (short) -232
      };
      this.extraSize = (short) 32;
    }

    protected override unsafe IntPtr MarshalToPtr()
    {
      IntPtr num = Marshal.AllocHGlobal(Utilities.SizeOf<WaveFormat.__Native>() + 4 + 4 * this.Coefficients1.Length);
      this.__MarshalTo(ref *(WaveFormatAdpcm.__Native*) (void*) num);
      return num;
    }

    internal unsafe void __MarshalFrom(ref WaveFormatAdpcm.__Native @ref)
    {
      base.__MarshalFrom(ref @ref.waveFormat);
      this.SamplesPerBlock = @ref.samplesPerBlock;
      this.Coefficients1 = new short[(int) @ref.numberOfCoefficients];
      this.Coefficients2 = new short[(int) @ref.numberOfCoefficients];
      if ((int) @ref.numberOfCoefficients > 7)
        throw new InvalidOperationException("Unable to read Adpcm format. Too may coefficients (max 7)");
      fixed (short* numPtr = &@ref.coefficients)
      {
        for (int index = 0; index < (int) @ref.numberOfCoefficients; ++index)
        {
          this.Coefficients1[index] = numPtr[index * 2];
          this.Coefficients2[index] = numPtr[index * 2 + 1];
        }
      }
      this.extraSize = (short) (4 + 4 * (int) @ref.numberOfCoefficients);
    }

    private unsafe void __MarshalTo(ref WaveFormatAdpcm.__Native @ref)
    {
      if (this.Coefficients1.Length > 7)
        throw new InvalidOperationException("Unable to encode Adpcm format. Too may coefficients (max 7)");
      this.extraSize = (short) (4 + 4 * this.Coefficients1.Length);
      base.__MarshalTo(ref @ref.waveFormat);
      @ref.samplesPerBlock = this.SamplesPerBlock;
      @ref.numberOfCoefficients = (ushort) this.Coefficients1.Length;
      fixed (short* numPtr = &@ref.coefficients)
      {
        for (int index = 0; index < (int) @ref.numberOfCoefficients; ++index)
        {
          numPtr[index * 2] = this.Coefficients1[index];
          numPtr[index * 2 + 1] = this.Coefficients2[index];
        }
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct __Native
    {
      public WaveFormat.__Native waveFormat;
      public ushort samplesPerBlock;
      public ushort numberOfCoefficients;
      public short coefficients;

      internal void __MarshalFree()
      {
        this.waveFormat.__MarshalFree();
      }
    }
  }
}
