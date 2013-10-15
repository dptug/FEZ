// Type: Microsoft.Xna.Framework.Media.MediaSource
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
