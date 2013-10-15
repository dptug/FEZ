// Type: Microsoft.Xna.Framework.Media.MediaLibrary
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
