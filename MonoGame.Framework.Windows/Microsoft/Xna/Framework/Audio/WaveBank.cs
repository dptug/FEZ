// Type: Microsoft.Xna.Framework.Audio.WaveBank
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.IO;
using System.Text;

namespace Microsoft.Xna.Framework.Audio
{
  public class WaveBank : IDisposable
  {
    private const int Flag_EntryNames = 65536;
    private const int Flag_Compact = 131072;
    private const int Flag_SyncDisabled = 262144;
    private const int Flag_SeekTables = 524288;
    private const int Flag_Mask = 983040;
    private const int MiniFormatTag_PCM = 0;
    private const int MiniFormatTag_XMA = 1;
    private const int MiniFormatTag_ADPCM = 2;
    private const int MiniForamtTag_WMA = 3;
    internal SoundEffectInstance[] sounds;
    internal string BankName;

    public WaveBank(AudioEngine audioEngine, string nonStreamingWaveBankFilename)
    {
      WaveBank.WaveBankData waveBankData;
      waveBankData.EntryNameElementSize = 0;
      waveBankData.CompactFormat = 0;
      waveBankData.Alignment = 0;
      WaveBank.WaveBankEntry waveBankEntry;
      waveBankEntry.Format = 0;
      waveBankEntry.PlayRegion.Length = 0;
      waveBankEntry.PlayRegion.Offset = 0;
      int num1 = 0;
      nonStreamingWaveBankFilename = nonStreamingWaveBankFilename.Replace('\\', Path.DirectorySeparatorChar);
      BinaryReader binaryReader = new BinaryReader((Stream) new FileStream(nonStreamingWaveBankFilename, FileMode.Open));
      binaryReader.ReadBytes(4);
      WaveBank.WaveBankHeader waveBankHeader;
      waveBankHeader.Version = binaryReader.ReadInt32();
      int index1 = 4;
      if (waveBankHeader.Version <= 3)
        index1 = 3;
      if (waveBankHeader.Version >= 42)
        binaryReader.ReadInt32();
      waveBankHeader.Segments = new WaveBank.Segment[5];
      for (int index2 = 0; index2 <= index1; ++index2)
      {
        waveBankHeader.Segments[index2].Offset = binaryReader.ReadInt32();
        waveBankHeader.Segments[index2].Length = binaryReader.ReadInt32();
      }
      binaryReader.BaseStream.Seek((long) waveBankHeader.Segments[0].Offset, SeekOrigin.Begin);
      waveBankData.Flags = binaryReader.ReadInt32();
      waveBankData.EntryCount = binaryReader.ReadInt32();
      int num2 = waveBankHeader.Version == 2 ? 0 : (waveBankHeader.Version != 3 ? 1 : 0);
      waveBankData.BankName = num2 != 0 ? Encoding.UTF8.GetString(binaryReader.ReadBytes(64)).Replace("\0", "") : Encoding.UTF8.GetString(binaryReader.ReadBytes(16)).Replace("\0", "");
      this.BankName = waveBankData.BankName;
      if (waveBankHeader.Version == 1)
      {
        waveBankData.EntryMetaDataElementSize = 20;
      }
      else
      {
        waveBankData.EntryMetaDataElementSize = binaryReader.ReadInt32();
        waveBankData.EntryNameElementSize = binaryReader.ReadInt32();
        waveBankData.Alignment = binaryReader.ReadInt32();
        num1 = waveBankHeader.Segments[1].Offset;
      }
      if ((waveBankData.Flags & 131072) != 0)
        binaryReader.ReadInt32();
      int num3 = waveBankHeader.Segments[index1].Offset;
      if (num3 == 0)
        num3 = num1 + waveBankData.EntryCount * waveBankData.EntryMetaDataElementSize;
      int index3 = 2;
      if (waveBankHeader.Version >= 42)
        index3 = 3;
      int num4 = waveBankHeader.Segments[index3].Offset;
      if (waveBankHeader.Segments[index3].Offset != 0 && waveBankHeader.Segments[index3].Length != 0)
      {
        if (waveBankData.EntryNameElementSize == -1)
          waveBankData.EntryNameElementSize = 0;
        new byte[waveBankData.EntryNameElementSize + 1][waveBankData.EntryNameElementSize] = (byte) 0;
      }
      this.sounds = new SoundEffectInstance[waveBankData.EntryCount];
      for (int index2 = 0; index2 < waveBankData.EntryCount; ++index2)
      {
        binaryReader.BaseStream.Seek((long) num1, SeekOrigin.Begin);
        if ((waveBankData.Flags & 131072) != 0)
        {
          int num5 = binaryReader.ReadInt32();
          waveBankEntry.Format = waveBankData.CompactFormat;
          waveBankEntry.PlayRegion.Offset = (num5 & 2097151) * waveBankData.Alignment;
          waveBankEntry.PlayRegion.Length = num5 >> 21 & 2047;
          binaryReader.BaseStream.Seek((long) (num1 + waveBankData.EntryMetaDataElementSize), SeekOrigin.Begin);
          int num6 = index2 != waveBankData.EntryCount - 1 ? (binaryReader.ReadInt32() & 2097151) * waveBankData.Alignment : waveBankHeader.Segments[index1].Length;
          waveBankEntry.PlayRegion.Length = num6 - waveBankEntry.PlayRegion.Offset;
        }
        else
        {
          if (waveBankHeader.Version == 1)
          {
            waveBankEntry.Format = binaryReader.ReadInt32();
            waveBankEntry.PlayRegion.Offset = binaryReader.ReadInt32();
            waveBankEntry.PlayRegion.Length = binaryReader.ReadInt32();
            waveBankEntry.LoopRegion.Offset = binaryReader.ReadInt32();
            waveBankEntry.LoopRegion.Length = binaryReader.ReadInt32();
          }
          else
          {
            if (waveBankData.EntryMetaDataElementSize >= 4)
              waveBankEntry.FlagsAndDuration = binaryReader.ReadInt32();
            if (waveBankData.EntryMetaDataElementSize >= 8)
              waveBankEntry.Format = binaryReader.ReadInt32();
            if (waveBankData.EntryMetaDataElementSize >= 12)
              waveBankEntry.PlayRegion.Offset = binaryReader.ReadInt32();
            if (waveBankData.EntryMetaDataElementSize >= 16)
              waveBankEntry.PlayRegion.Length = binaryReader.ReadInt32();
            if (waveBankData.EntryMetaDataElementSize >= 20)
              waveBankEntry.LoopRegion.Offset = binaryReader.ReadInt32();
            if (waveBankData.EntryMetaDataElementSize >= 24)
              waveBankEntry.LoopRegion.Length = binaryReader.ReadInt32();
          }
          if (waveBankData.EntryMetaDataElementSize < 24 && waveBankEntry.PlayRegion.Length != 0)
            waveBankEntry.PlayRegion.Length = waveBankHeader.Segments[index1].Length;
        }
        num1 += waveBankData.EntryMetaDataElementSize;
        waveBankEntry.PlayRegion.Offset += num3;
        int num7;
        int num8;
        int num9;
        int num10;
        int num11;
        if (waveBankHeader.Version == 1)
        {
          num7 = waveBankEntry.Format & 1;
          num8 = waveBankEntry.Format >> 1 & 7;
          num9 = waveBankEntry.Format >> 5 & 262143;
          num10 = waveBankEntry.Format >> 23 & (int) byte.MaxValue;
          num11 = waveBankEntry.Format >> 31 & 1;
        }
        else
        {
          num7 = waveBankEntry.Format & 3;
          num8 = waveBankEntry.Format >> 2 & 7;
          num9 = waveBankEntry.Format >> 5 & 262143;
          num10 = waveBankEntry.Format >> 23 & (int) byte.MaxValue;
          num11 = waveBankEntry.Format >> 31 & 1;
        }
        binaryReader.BaseStream.Seek((long) waveBankEntry.PlayRegion.Offset, SeekOrigin.Begin);
        byte[] buffer = binaryReader.ReadBytes(waveBankEntry.PlayRegion.Length);
        if (num7 == 0)
        {
          MemoryStream memoryStream = new MemoryStream(44 + buffer.Length);
          BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream);
          binaryWriter.Write("RIFF".ToCharArray());
          binaryWriter.Write(36 + buffer.Length);
          binaryWriter.Write("WAVE".ToCharArray());
          binaryWriter.Write("fmt ".ToCharArray());
          binaryWriter.Write(16);
          binaryWriter.Write((short) 1);
          binaryWriter.Write((short) num8);
          binaryWriter.Write(num9);
          binaryWriter.Write(num9 * num10);
          binaryWriter.Write((short) num10);
          if (num11 == 1)
            binaryWriter.Write((short) 16);
          else
            binaryWriter.Write((short) 8);
          binaryWriter.Write("data".ToCharArray());
          binaryWriter.Write(buffer.Length);
          binaryWriter.Write(buffer);
          binaryWriter.Close();
          memoryStream.Close();
          this.sounds[index2] = new SoundEffect((string) null, memoryStream.ToArray()).CreateInstance(false);
        }
        else
        {
          if (num7 != 3)
            throw new NotImplementedException();
          byte[] numArray1 = new byte[16]
          {
            (byte) 48,
            (byte) 38,
            (byte) 178,
            (byte) 117,
            (byte) 142,
            (byte) 102,
            (byte) 207,
            (byte) 17,
            (byte) 166,
            (byte) 217,
            (byte) 0,
            (byte) 170,
            (byte) 0,
            (byte) 98,
            (byte) 206,
            (byte) 108
          };
          bool flag1 = true;
          for (int index4 = 0; index4 < numArray1.Length; ++index4)
          {
            if ((int) numArray1[index4] != (int) buffer[index4])
            {
              flag1 = false;
              break;
            }
          }
          byte[][] numArray2 = new byte[2][]
          {
            new byte[16]
            {
              (byte) 0,
              (byte) 0,
              (byte) 0,
              (byte) 24,
              (byte) 102,
              (byte) 116,
              (byte) 121,
              (byte) 112,
              (byte) 77,
              (byte) 52,
              (byte) 65,
              (byte) 32,
              (byte) 0,
              (byte) 0,
              (byte) 2,
              (byte) 0
            },
            new byte[16]
            {
              (byte) 0,
              (byte) 0,
              (byte) 0,
              (byte) 32,
              (byte) 102,
              (byte) 116,
              (byte) 121,
              (byte) 112,
              (byte) 77,
              (byte) 52,
              (byte) 65,
              (byte) 32,
              (byte) 0,
              (byte) 0,
              (byte) 0,
              (byte) 0
            }
          };
          bool flag2 = false;
          for (int index4 = 0; index4 < numArray2.Length; ++index4)
          {
            byte[] numArray3 = numArray2[index4];
            bool flag3 = true;
            for (int index5 = 0; index5 < numArray3.Length; ++index5)
            {
              if ((int) numArray3[index5] != (int) buffer[index5])
              {
                flag3 = false;
                break;
              }
            }
            if (flag3)
            {
              flag2 = true;
              break;
            }
          }
          if (!flag1 && !flag2)
            throw new NotImplementedException();
          string str = Path.GetTempFileName();
          if (flag1)
            str = str.Replace(".tmp", ".wma");
          else if (flag2)
            str = str.Replace(".tmp", ".m4a");
          using (FileStream fileStream = File.Create(str))
            fileStream.Write(buffer, 0, buffer.Length);
          this.sounds[index2] = new SoundEffect(str).CreateInstance(false);
        }
      }
      audioEngine.Wavebanks[this.BankName] = this;
    }

    public WaveBank(AudioEngine audioEngine, string streamingWaveBankFilename, int offset, short packetsize)
      : this(audioEngine, streamingWaveBankFilename)
    {
      if (offset != 0)
        throw new NotImplementedException();
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    private struct Segment
    {
      public int Offset;
      public int Length;
    }

    private struct WaveBankEntry
    {
      public int Format;
      public WaveBank.Segment PlayRegion;
      public WaveBank.Segment LoopRegion;
      public int FlagsAndDuration;
    }

    private struct WaveBankHeader
    {
      public int Version;
      public WaveBank.Segment[] Segments;
    }

    private struct WaveBankData
    {
      public int Flags;
      public int EntryCount;
      public string BankName;
      public int EntryMetaDataElementSize;
      public int EntryNameElementSize;
      public int Alignment;
      public int CompactFormat;
      public int BuildTime;
    }
  }
}
