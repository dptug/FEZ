// Type: Microsoft.Xna.Framework.Content.SongReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Media;
using System;
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
      string fileName = input.ReadString();
      if (!string.IsNullOrEmpty(fileName))
      {
        char newChar = Path.DirectorySeparatorChar;
        string relativeUri = fileName.Replace('\\', newChar);
        string path2 = new Uri(new Uri("file:///" + input.AssetName.Replace('\\', newChar)), relativeUri).LocalPath.Substring(1);
        fileName = Path.Combine(input.ContentManager.RootDirectoryFullPath, path2);
      }
      int durationMS = input.ReadObject<int>();
      return new Song(fileName, durationMS);
    }
  }
}
