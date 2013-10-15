// Type: Microsoft.Xna.Framework.Net.HostChangedEventArgs
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
