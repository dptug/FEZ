// Type: SharpDX.Multimedia.WaveFormat
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using SharpDX.Serialization;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.Multimedia
{
  public class WaveFormat : IDataSerializable
  {
    protected WaveFormatEncoding waveFormatTag;
    protected short channels;
    protected int sampleRate;
    protected int averageBytesPerSecond;
    protected short blockAlign;
    protected short bitsPerSample;
    protected short extraSize;

    public WaveFormatEncoding Encoding
    {
      get
      {
        return this.waveFormatTag;
      }
    }

    public int Channels
    {
      get
      {
        return (int) this.channels;
      }
    }

    public int SampleRate
    {
      get
      {
        return this.sampleRate;
      }
    }

    public int AverageBytesPerSecond
    {
      get
      {
        return this.averageBytesPerSecond;
      }
    }

    public int BlockAlign
    {
      get
      {
        return (int) this.blockAlign;
      }
    }

    public int BitsPerSample
    {
      get
      {
        return (int) this.bitsPerSample;
      }
    }

    public int ExtraSize
    {
      get
      {
        return (int) this.extraSize;
      }
    }

    public WaveFormat()
      : this(44100, 16, 2)
    {
    }

    public WaveFormat(int sampleRate, int channels)
      : this(sampleRate, 16, channels)
    {
    }

    public WaveFormat(int rate, int bits, int channels)
    {
      if (channels < 1)
        throw new ArgumentOutOfRangeException("Channels must be 1 or greater", "channels");
      this.waveFormatTag = bits < 32 ? WaveFormatEncoding.Pcm : WaveFormatEncoding.IeeeFloat;
      this.channels = (short) channels;
      this.sampleRate = rate;
      this.bitsPerSample = (short) bits;
      this.extraSize = (short) 0;
      this.blockAlign = (short) (channels * (bits / 8));
      this.averageBytesPerSecond = this.sampleRate * (int) this.blockAlign;
    }

    public WaveFormat(BinaryReader br)
    {
      int num = br.ReadInt32();
      if (num < 16)
        throw new SharpDXException("Invalid WaveFormat Structure", new object[0]);
      this.waveFormatTag = (WaveFormatEncoding) br.ReadUInt16();
      this.channels = br.ReadInt16();
      this.sampleRate = br.ReadInt32();
      this.averageBytesPerSecond = br.ReadInt32();
      this.blockAlign = br.ReadInt16();
      this.bitsPerSample = br.ReadInt16();
      this.extraSize = (short) 0;
      if (num <= 16)
        return;
      this.extraSize = br.ReadInt16();
      if ((int) this.extraSize <= num - 18)
        return;
      Console.WriteLine("Format chunk mismatch");
      this.extraSize = (short) (num - 18);
    }

    internal void __MarshalFree(ref WaveFormat.__Native @ref)
    {
      @ref.__MarshalFree();
    }

    internal void __MarshalFrom(ref WaveFormat.__Native @ref)
    {
      this.waveFormatTag = @ref.pcmWaveFormat.waveFormatTag;
      this.channels = @ref.pcmWaveFormat.channels;
      this.sampleRate = @ref.pcmWaveFormat.sampleRate;
      this.averageBytesPerSecond = @ref.pcmWaveFormat.averageBytesPerSecond;
      this.blockAlign = @ref.pcmWaveFormat.blockAlign;
      this.bitsPerSample = @ref.pcmWaveFormat.bitsPerSample;
      this.extraSize = @ref.extraSize;
    }

    internal void __MarshalTo(ref WaveFormat.__Native @ref)
    {
      @ref.pcmWaveFormat.waveFormatTag = this.waveFormatTag;
      @ref.pcmWaveFormat.channels = this.channels;
      @ref.pcmWaveFormat.sampleRate = this.sampleRate;
      @ref.pcmWaveFormat.averageBytesPerSecond = this.averageBytesPerSecond;
      @ref.pcmWaveFormat.blockAlign = this.blockAlign;
      @ref.pcmWaveFormat.bitsPerSample = this.bitsPerSample;
      @ref.extraSize = this.extraSize;
    }

    internal void __MarshalFree(ref WaveFormat.__PcmNative @ref)
    {
      @ref.__MarshalFree();
    }

    internal void __MarshalFrom(ref WaveFormat.__PcmNative @ref)
    {
      this.waveFormatTag = @ref.waveFormatTag;
      this.channels = @ref.channels;
      this.sampleRate = @ref.sampleRate;
      this.averageBytesPerSecond = @ref.averageBytesPerSecond;
      this.blockAlign = @ref.blockAlign;
      this.bitsPerSample = @ref.bitsPerSample;
      this.extraSize = (short) 0;
    }

    internal void __MarshalTo(ref WaveFormat.__PcmNative @ref)
    {
      @ref.waveFormatTag = this.waveFormatTag;
      @ref.channels = this.channels;
      @ref.sampleRate = this.sampleRate;
      @ref.averageBytesPerSecond = this.averageBytesPerSecond;
      @ref.blockAlign = this.blockAlign;
      @ref.bitsPerSample = this.bitsPerSample;
    }

    public int ConvertLatencyToByteSize(int milliseconds)
    {
      int num = (int) ((double) this.AverageBytesPerSecond / 1000.0 * (double) milliseconds);
      if (num % this.BlockAlign != 0)
        num = num + this.BlockAlign - num % this.BlockAlign;
      return num;
    }

    public static WaveFormat CreateCustomFormat(WaveFormatEncoding tag, int sampleRate, int channels, int averageBytesPerSecond, int blockAlign, int bitsPerSample)
    {
      return new WaveFormat()
      {
        waveFormatTag = tag,
        channels = (short) channels,
        sampleRate = sampleRate,
        averageBytesPerSecond = averageBytesPerSecond,
        blockAlign = (short) blockAlign,
        bitsPerSample = (short) bitsPerSample,
        extraSize = (short) 0
      };
    }

    public static WaveFormat CreateALawFormat(int sampleRate, int channels)
    {
      return WaveFormat.CreateCustomFormat(WaveFormatEncoding.Alaw, sampleRate, channels, sampleRate * channels, 1, 8);
    }

    public static WaveFormat CreateMuLawFormat(int sampleRate, int channels)
    {
      return WaveFormat.CreateCustomFormat(WaveFormatEncoding.Mulaw, sampleRate, channels, sampleRate * channels, 1, 8);
    }

    public static WaveFormat CreateIeeeFloatWaveFormat(int sampleRate, int channels)
    {
      WaveFormat waveFormat = new WaveFormat()
      {
        waveFormatTag = WaveFormatEncoding.IeeeFloat,
        channels = (short) channels,
        bitsPerSample = (short) 32,
        sampleRate = sampleRate,
        blockAlign = (short) (4 * channels)
      };
      waveFormat.averageBytesPerSecond = sampleRate * (int) waveFormat.blockAlign;
      waveFormat.extraSize = (short) 0;
      return waveFormat;
    }

    public static unsafe WaveFormat MarshalFrom(byte[] rawdata)
    {
      fixed (byte* numPtr = rawdata)
        return WaveFormat.MarshalFrom((IntPtr) ((void*) numPtr));
    }

    public static unsafe WaveFormat MarshalFrom(IntPtr pointer)
    {
      if (pointer == IntPtr.Zero)
        return (WaveFormat) null;
      WaveFormat.__PcmNative @ref = *(WaveFormat.__PcmNative*) (void*) pointer;
      WaveFormatEncoding waveFormatEncoding = @ref.waveFormatTag;
      if ((int) @ref.channels <= 2 && (waveFormatEncoding == WaveFormatEncoding.Pcm || waveFormatEncoding == WaveFormatEncoding.IeeeFloat || (waveFormatEncoding == WaveFormatEncoding.Wmaudio2 || waveFormatEncoding == WaveFormatEncoding.Wmaudio3)))
      {
        WaveFormat waveFormat = new WaveFormat();
        waveFormat.__MarshalFrom(ref @ref);
        return waveFormat;
      }
      else if (waveFormatEncoding == WaveFormatEncoding.Extensible)
      {
        WaveFormatExtensible formatExtensible = new WaveFormatExtensible();
        formatExtensible.__MarshalFrom(ref *(WaveFormatExtensible.__Native*) (void*) pointer);
        return (WaveFormat) formatExtensible;
      }
      else
      {
        if (waveFormatEncoding != WaveFormatEncoding.Adpcm)
          throw new InvalidOperationException(string.Format("Unsupported WaveFormat [{0}]", (object) waveFormatEncoding));
        WaveFormatAdpcm waveFormatAdpcm = new WaveFormatAdpcm();
        waveFormatAdpcm.__MarshalFrom(ref *(WaveFormatAdpcm.__Native*) (void*) pointer);
        return (WaveFormat) waveFormatAdpcm;
      }
    }

    protected virtual unsafe IntPtr MarshalToPtr()
    {
      IntPtr num = Marshal.AllocHGlobal(Utilities.SizeOf<WaveFormat.__Native>());
      this.__MarshalTo(ref *(WaveFormat.__Native*) (void*) num);
      return num;
    }

    public static IntPtr MarshalToPtr(WaveFormat format)
    {
      if (format == null)
        return IntPtr.Zero;
      else
        return format.MarshalToPtr();
    }

    public override string ToString()
    {
      switch (this.waveFormatTag)
      {
        case WaveFormatEncoding.Extensible:
        case WaveFormatEncoding.Pcm:
          return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} bit PCM: {1}kHz {2} channels", (object) this.bitsPerSample, (object) (this.sampleRate / 1000), (object) this.channels);
        default:
          return ((object) this.waveFormatTag).ToString();
      }
    }

    public override bool Equals(object obj)
    {
      if (!(obj is WaveFormat))
        return false;
      WaveFormat waveFormat = (WaveFormat) obj;
      if (this.waveFormatTag == waveFormat.waveFormatTag && (int) this.channels == (int) waveFormat.channels && (this.sampleRate == waveFormat.sampleRate && this.averageBytesPerSecond == waveFormat.averageBytesPerSecond) && (int) this.blockAlign == (int) waveFormat.blockAlign)
        return (int) this.bitsPerSample == (int) waveFormat.bitsPerSample;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return (int) (this.waveFormatTag ^ (WaveFormatEncoding) this.channels ^ (WaveFormatEncoding) this.sampleRate ^ (WaveFormatEncoding) this.averageBytesPerSecond ^ (WaveFormatEncoding) this.blockAlign ^ (WaveFormatEncoding) this.bitsPerSample);
    }

    public virtual void Serialize(BinarySerializer serializer)
    {
      WaveFormat.__Native @ref = new WaveFormat.__Native();
      if (serializer.Mode == SerializerMode.Read)
      {
        serializer.Serialize<WaveFormat.__Native>(ref @ref, SerializeFlags.Normal);
        this.__MarshalFrom(ref @ref);
      }
      else
      {
        this.__MarshalTo(ref @ref);
        serializer.Serialize<WaveFormat.__Native>(ref @ref, SerializeFlags.Normal);
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct __Native : IDataSerializable
    {
      public WaveFormat.__PcmNative pcmWaveFormat;
      public short extraSize;

      internal void __MarshalFree()
      {
      }

      public void Serialize(BinarySerializer serializer)
      {
        serializer.Serialize<WaveFormat.__PcmNative>(ref this.pcmWaveFormat, SerializeFlags.Normal);
        serializer.Serialize(ref this.extraSize);
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct __PcmNative : IDataSerializable
    {
      public WaveFormatEncoding waveFormatTag;
      public short channels;
      public int sampleRate;
      public int averageBytesPerSecond;
      public short blockAlign;
      public short bitsPerSample;

      internal void __MarshalFree()
      {
      }

      public void Serialize(BinarySerializer serializer)
      {
        serializer.SerializeEnum<WaveFormatEncoding>(ref this.waveFormatTag);
        serializer.Serialize(ref this.channels);
        serializer.Serialize(ref this.sampleRate);
        serializer.Serialize(ref this.averageBytesPerSecond);
        serializer.Serialize(ref this.blockAlign);
        serializer.Serialize(ref this.bitsPerSample);
      }
    }
  }
}
