// Type: Microsoft.Xna.Framework.Net.NetworkSessionJoinException
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
