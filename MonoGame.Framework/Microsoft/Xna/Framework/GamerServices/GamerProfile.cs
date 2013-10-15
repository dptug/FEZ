// Type: Microsoft.Xna.Framework.GamerServices.GamerProfile
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
