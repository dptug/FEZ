// Type: Microsoft.Xna.Framework.SoundEffectReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
