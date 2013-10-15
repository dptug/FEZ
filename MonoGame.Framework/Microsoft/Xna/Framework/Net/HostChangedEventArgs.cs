// Type: Microsoft.Xna.Framework.Net.HostChangedEventArgs
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Net
{
  public class HostChangedEventArgs : EventArgs
  {
    private NetworkGamer newHost;
    private NetworkGamer oldHost;

    public NetworkGamer NewHost
    {
      get
      {
        return this.newHost;
      }
    }

    public NetworkGamer OldHost
    {
      get
      {
        return this.oldHost;
      }
    }

    public HostChangedEventArgs(NetworkGamer aNewHost, NetworkGamer aOldHost)
    {
      this.newHost = aNewHost;
      this.oldHost = aOldHost;
    }
  }
}
