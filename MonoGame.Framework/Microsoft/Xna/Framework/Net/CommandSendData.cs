// Type: Microsoft.Xna.Framework.Net.CommandSendData
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
