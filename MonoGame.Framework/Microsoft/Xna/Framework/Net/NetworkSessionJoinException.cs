// Type: Microsoft.Xna.Framework.Net.NetworkSessionJoinException
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Net
{
  [Serializable]
  public class NetworkSessionJoinException : NetworkException
  {
    private NetworkSessionJoinError _JoinError;

    public NetworkSessionJoinError JoinError
    {
      get
      {
        return this._JoinError;
      }
      set
      {
        if (this._JoinError == value)
          return;
        this.JoinError = value;
      }
    }

    public NetworkSessionJoinException()
    {
    }

    public NetworkSessionJoinException(string message)
      : base(message)
    {
    }

    public NetworkSessionJoinException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public NetworkSessionJoinException(string message, NetworkSessionJoinError joinError)
      : base(message)
    {
      this._JoinError = this.JoinError;
    }
  }
}
