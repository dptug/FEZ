// Type: Microsoft.Xna.Framework.Media.MediaSource
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
  public sealed class MediaSource
  {
    private MediaSourceType _type;
    private string _name;

    public MediaSourceType MediaSourceType
    {
      get
      {
        return this._type;
      }
    }

    public string Name
    {
      get
      {
        return this._name;
      }
    }

    internal MediaSource(string name, MediaSourceType type)
    {
      this._name = name;
      this._type = type;
    }

    public static IList<MediaSource> GetAvailableMediaSources()
    {
      return (IList<MediaSource>) new MediaSource[1]
      {
        new MediaSource("DummpMediaSource", MediaSourceType.LocalDevice)
      };
    }
  }
}
