// Type: Microsoft.Xna.Framework.Net.CommandSendData
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Net
{
  internal class CommandSendData : ICommand
  {
    internal int gamerInternalIndex = -1;
    internal byte[] data;
    internal SendDataOptions options;
    internal int offset;
    internal int count;
    internal NetworkGamer gamer;
    internal LocalNetworkGamer sender;

    public CommandEventType Command
    {
      get
      {
        return CommandEventType.SendData;
      }
    }

    public CommandSendData(byte[] data, int offset, int count, SendDataOptions options, NetworkGamer gamer, LocalNetworkGamer sender)
    {
      if (gamer != null)
        this.gamerInternalIndex = (int) gamer.Id;
      this.data = new byte[count];
      Array.Copy((Array) data, offset, (Array) this.data, 0, count);
      this.offset = offset;
      this.count = count;
      this.options = options;
      this.gamer = gamer;
      this.sender = sender;
    }
  }
}
