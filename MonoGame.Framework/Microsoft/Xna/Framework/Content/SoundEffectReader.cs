// Type: Microsoft.Xna.Framework.Content.SoundEffectReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;

namespace Microsoft.Xna.Framework.Content
{
  internal class SoundEffectReader : ContentTypeReader<SoundEffect>
  {
    private static string[] supportedExtensions = new string[4]
    {
      ".wav",
      ".aiff",
      ".ac3",
      ".mp3"
    };

    static SoundEffectReader()
    {
    }

    internal static string Normalize(string fileName)
    {
      return ContentTypeReader.Normalize(fileName, SoundEffectReader.supportedExtensions);
    }

    protected internal override SoundEffect Read(ContentReader input, SoundEffect existingInstance)
    {
      byte[] buffer1 = input.ReadBytes(input.ReadInt32());
      byte[] buffer2 = input.ReadBytes(input.ReadInt32());
      input.ReadInt32();
      input.ReadInt32();
      input.ReadInt32();
      int num = (int) BitConverter.ToUInt16(buffer1, 0);
      byte[] data = (byte[]) null;
      MemoryStream memoryStream = new MemoryStream(20 + buffer1.Length + 8 + buffer2.Length);
      using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
      {
        binaryWriter.Write("RIFF".ToCharArray());
        binaryWriter.Write(20 + buffer1.Length + buffer2.Length);
        binaryWriter.Write("WAVE".ToCharArray());
        binaryWriter.Write("fmt ".ToCharArray());
        binaryWriter.Write(buffer1.Length);
        binaryWriter.Write(buffer1);
        binaryWriter.Write("data".ToCharArray());
        binaryWriter.Write(buffer2.Length);
        binaryWriter.Write(buffer2);
        data = memoryStream.ToArray();
      }
      if (data == null)
        throw new ContentLoadException("Failed to load SoundEffect");
      else
        return new SoundEffect(input.AssetName, num == 1, data);
    }
  }
}
