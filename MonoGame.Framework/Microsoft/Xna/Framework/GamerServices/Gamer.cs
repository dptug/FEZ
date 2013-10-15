// Type: Microsoft.Xna.Framework.GamerServices.Gamer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
        return this.disposed;
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
