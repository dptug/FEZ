// Type: Microsoft.Xna.Framework.Content.SongReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Microsoft.Xna.Framework.Content
{
  internal class SongReader : ContentTypeReader<Song>
  {
    private static string[] supportedExtensions = new string[1]
    {
      ".mp3"
    };

    static SongReader()
    {
    }

    internal static string Normalize(string fileName)
    {
      return ContentTypeReader.Normalize(fileName, SongReader.supportedExtensions);
    }

    protected internal override Song Read(ContentReader input, Song existingInstance)
    {
      string path3 = input.ReadString();
      string filename = TitleContainer.GetFilename(Path.Combine(input.ContentManager.RootDirectory, input.ContentManager.CurrentAssetDirectory, path3));
      input.ReadObject<int>();
      return new Song(filename);
    }
  }
}
