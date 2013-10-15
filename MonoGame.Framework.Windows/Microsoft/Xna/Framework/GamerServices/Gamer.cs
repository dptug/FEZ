// Type: Microsoft.Xna.Framework.GamerServices.Gamer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.GamerServices
{
  public abstract class Gamer
  {
    private static SignedInGamerCollection _signedInGamers = new SignedInGamerCollection();
    private string _gamer = "MonoGame";
    private object _tag;
    private bool disposed;

    public string DisplayName { get; internal set; }

    public string Gamertag
    {
      get
      {
        return this._gamer;
      }
      internal set
      {
        this._gamer = value;
      }
    }

    public bool IsDisposed
    {
      get
      {
        return this.IsDisposed;
      }
    }

    public object Tag
    {
      get
      {
        return this._tag;
      }
      set
      {
        if (this._tag == value)
          return;
        this._tag = value;
      }
    }

    public static SignedInGamerCollection SignedInGamers
    {
      get
      {
        return Gamer._signedInGamers;
      }
    }

    static Gamer()
    {
    }

    public IAsyncResult BeginGetProfile(AsyncCallback callback, object asyncState)
    {
      throw new NotImplementedException();
    }

    public GamerProfile EndGetProfile(IAsyncResult result)
    {
      throw new NotImplementedException();
    }

    public GamerProfile GetProfile()
    {
      throw new NotImplementedException();
    }

    public override string ToString()
    {
      return this._gamer;
    }

    internal void Dispose()
    {
      this.disposed = true;
    }
  }
}
