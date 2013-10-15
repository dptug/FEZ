// Type: Microsoft.Xna.Framework.Audio.AudioLoader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Audio.OpenAL;
using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  internal class AudioLoader
  {
    private AudioLoader()
    {
    }

    private static ALFormat GetSoundFormat(int channels, int bits)
    {
      switch (channels)
      {
        case 1:
          return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
        case 2:
          return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
        default:
          throw new NotSupportedException("The specified sound format is not supported.");
      }
    }

    public static byte[] Load(Stream data, out ALFormat format, out int size, out int frequency)
    {
      byte[] numArray = (byte[]) null;
      format = ALFormat.Mono8;
      size = 0;
      frequency = 0;
      using (BinaryReader reader = new BinaryReader(data))
        numArray = AudioLoader.LoadWave(reader, out format, out size, out frequency);
      return numArray;
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
      int num2 = (int) reader.ReadInt16();
      int channels = (int) reader.ReadInt16();
      int num3 = reader.ReadInt32();
      reader.ReadInt32();
      int num4 = (int) reader.ReadInt16();
      int bits = (int) reader.ReadInt16();
      if (num1 > 16)
        reader.ReadBytes(num1 - 16);
      string str1;
      for (str1 = new string(reader.ReadChars(4)); str1.ToLower() != "data"; str1 = new string(reader.ReadChars(4)))
        reader.ReadBytes(reader.ReadInt32());
      if (str1 != "data")
        throw new NotSupportedException("Specified wave file is not supported.");
      int num5 = reader.ReadInt32();
      frequency = num3;
      format = AudioLoader.GetSoundFormat(channels, bits);
      byte[] numArray = reader.ReadBytes((int) reader.BaseStream.Length);
      size = num5;
      if (num2 != 1)
      {
        Console.WriteLine("Wave compression is not supported.");
        size = 0;
      }
      return numArray;
    }
  }
}
