// Type: Microsoft.Xna.Framework.GamerServices.GamerProfile
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;

namespace Microsoft.Xna.Framework.GamerServices
{
  public sealed class GamerProfile : IDisposable
  {
    public Texture2D GamerPicture
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int GamerScore
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public GamerZone GamerZone
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool IsDisposed
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public string Motto
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public RegionInfo Region
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public float Reputation
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int TitlesPlayed
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int TotalAchievements
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    ~GamerProfile()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected void Dispose(bool disposing)
    {
    }
  }
}
