// Type: Microsoft.Xna.Framework.SoundEffectReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Content;

namespace Microsoft.Xna.Framework
{
  internal class SoundEffectReader
  {
    private static string[] _extensions = new string[4]
    {
      ".aiff",
      ".wav",
      ".ac3",
      ".mp3"
    };

    static SoundEffectReader()
    {
    }

    public static string Normalize(string FileName)
    {
      return ContentTypeReader.Normalize(FileName, SoundEffectReader._extensions);
    }
  }
}
