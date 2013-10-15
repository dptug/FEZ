// Type: Microsoft.Xna.Framework.Media.MediaLibrary
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Media
{
  public class MediaLibrary : IDisposable
  {
    private PlaylistCollection _playLists;

    public SongCollection Songs
    {
      get
      {
        return new SongCollection();
      }
    }

    public MediaLibrary()
    {
    }

    public MediaLibrary(MediaSource mediaSource)
    {
    }

    public void Dispose()
    {
    }

    public void SavePicture(string name, byte[] imageBuffer)
    {
      throw new NotSupportedException();
    }

    public void SavePicture(string name, Stream source)
    {
      throw new NotSupportedException();
    }
  }
}
