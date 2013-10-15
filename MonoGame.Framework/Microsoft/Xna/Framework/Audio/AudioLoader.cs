// Type: Microsoft.Xna.Framework.Audio.AudioLoader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK.Audio.OpenAL;
using System;
using System.Globalization;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  internal class AudioLoader
  {
    private AudioLoader()
    {
    }

    private static ALFormat GetSoundFormat(int channels, int bits, bool adpcm)
    {
      switch (channels)
      {
        case 1:
          if (adpcm)
            return ALFormat.MonoIma4Ext;
          return bits != 8 ? ALFormat.Mono16 : ALFormat.Mono8;
        case 2:
          if (adpcm)
            return ALFormat.StereoIma4Ext;
          return bits != 8 ? ALFormat.Stereo16 : ALFormat.Stereo8;
        default:
          throw new NotSupportedException("The specified sound format is not supported.");
      }
    }

    public static byte[] Load(Stream data, out ALFormat format, out int size, out int frequency)
    {
      format = ALFormat.Mono8;
      size = 0;
      frequency = 0;
      using (BinaryReader reader = new BinaryReader(data))
        return AudioLoader.LoadWave(reader, out format, out size, out frequency);
    }

    private static byte[] LoadWave(BinaryReader reader, out ALFormat format, out int size, out int frequency)
    {
      if (new string(reader.ReadChars(4)) != "RIFF")
        throw new NotSupportedException("Specified stream is not a wave file.");
      reader.ReadInt32();
      if (new string(reader.ReadChars(4)) != "WAVE")
        throw new NotSupportedException("Specified stream is not a wave file.");
      for (string str = new string(reader.ReadChars(4)); str != "fmt "; str = new string(reader.ReadChars(4)))
        reader.ReadBytes(reader.ReadInt32());
      int num1 = reader.ReadInt32();
      int num2 = (int) reader.ReadUInt16();
      int channels = (int) reader.ReadUInt16();
      int num3 = (int) reader.ReadUInt32();
      int num4 = (int) reader.ReadUInt32();
      int num5 = (int) reader.ReadUInt16();
      int bits = (int) reader.ReadUInt16();
      if (num1 > 16)
        reader.ReadBytes(num1 - 16);
      string str1;
      for (str1 = new string(reader.ReadChars(4)); str1.ToLower(CultureInfo.InvariantCulture) != "data"; str1 = new string(reader.ReadChars(4)))
        reader.ReadBytes(reader.ReadInt32());
      if (str1 != "data")
        throw new NotSupportedException("Specified wave file is not supported.");
      int num6 = reader.ReadInt32();
      frequency = num3;
      format = AudioLoader.GetSoundFormat(channels, bits, num2 == 2);
      byte[] numArray;
      if (reader.BaseStream.Length <= 11072L)
      {
        numArray = new byte[0];
        size = 0;
      }
      else
      {
        numArray = reader.ReadBytes((int) reader.BaseStream.Length);
        size = num6 / num5 * num5;
      }
      return numArray;
    }
  }
}
