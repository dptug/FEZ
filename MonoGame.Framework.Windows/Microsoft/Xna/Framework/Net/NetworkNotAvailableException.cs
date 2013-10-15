// Type: Microsoft.Xna.Framework.Net.NetworkNotAvailableException
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Net
{
  [Serializable]
  public class NetworkNotAvailableException : NetworkException
  {
    public NetworkNotAvailableException()
    {
    }

    public NetworkNotAvailableException(string message)
      : base(message)
    {
    }

    public NetworkNotAvailableException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
