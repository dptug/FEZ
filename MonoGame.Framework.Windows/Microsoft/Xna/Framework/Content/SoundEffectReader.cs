// Type: Microsoft.Xna.Framework.Content.SoundEffectReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Audio;
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
        return new SoundEffect(input.AssetName, data);
    }
  }
}
