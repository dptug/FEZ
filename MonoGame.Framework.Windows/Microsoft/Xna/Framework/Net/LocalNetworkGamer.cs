// Type: Microsoft.Xna.Framework.Net.LocalNetworkGamer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Net
{
  public sealed class LocalNetworkGamer : NetworkGamer
  {
    private SignedInGamer sig;
    internal Queue<CommandReceiveData> receivedData;

    public bool IsDataAvailable
    {
      get
      {
        lock (this.receivedData)
          return this.receivedData.Count > 0;
      }
    }

    public SignedInGamer SignedInGamer
    {
      get
      {
        return this.sig;
      }
      internal set
      {
        this.sig = value;
        this.DisplayName = this.sig.DisplayName;
        this.Gamertag = this.sig.Gamertag;
      }
    }

    public LocalNetworkGamer()
      : base((NetworkSession) null, (byte) 0, (GamerStates) 0)
    {
      this.sig = new SignedInGamer();
      this.receivedData = new Queue<CommandReceiveData>();
    }

    public LocalNetworkGamer(NetworkSession session, byte id, GamerStates state)
      : base(session, id, state | GamerStates.Local)
    {
      this.sig = new SignedInGamer();
      this.receivedData = new Queue<CommandReceiveData>();
    }

    public void EnableSendVoice(NetworkGamer remoteGamer, bool enable)
    {
      throw new NotImplementedException();
    }

    public int ReceiveData(byte[] data, int offset, out NetworkGamer sender)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      if (this.receivedData.Count <= 0)
      {
        sender = (NetworkGamer) null;
        return 0;
      }
      else
      {
        lock (this.receivedData)
        {
          CommandReceiveData local_0 = this.receivedData.Peek();
          if (offset + local_0.data.Length > data.Length)
            throw new ArgumentOutOfRangeException("data", "The length + offset is greater than parameter can hold.");
          this.receivedData.Dequeue();
          Array.Copy((Array) local_0.data, offset, (Array) data, 0, data.Length);
          sender = local_0.gamer;
          return data.Length;
        }
      }
    }

    public int ReceiveData(byte[] data, out NetworkGamer sender)
    {
      return this.ReceiveData(data, 0, out sender);
    }

    public int ReceiveData(PacketReader data, out NetworkGamer sender)
    {
      lock (this.receivedData)
      {
        if (this.receivedData.Count >= 0)
        {
          data.Reset(0);
          CommandReceiveData local_0 = this.receivedData.Dequeue();
          if (data.Length < local_0.data.Length)
            data.Reset(local_0.data.Length);
          Array.Copy((Array) local_0.data, (Array) data.Data, data.Length);
          sender = local_0.gamer;
          return data.Length;
        }
        else
        {
          sender = (NetworkGamer) null;
          return 0;
        }
      }
    }

    public void SendData(byte[] data, int offset, int count, SendDataOptions options)
    {
      this.Session.commandQueue.Enqueue(new CommandEvent((ICommand) new CommandSendData(data, offset, count, options, (NetworkGamer) null, this)));
    }

    public void SendData(byte[] data, int offset, int count, SendDataOptions options, NetworkGamer recipient)
    {
      this.Session.commandQueue.Enqueue(new CommandEvent((ICommand) new CommandSendData(data, offset, count, options, recipient, this)));
    }

    public void SendData(byte[] data, SendDataOptions options)
    {
      this.Session.commandQueue.Enqueue(new CommandEvent((ICommand) new CommandSendData(data, 0, data.Length, options, (NetworkGamer) null, this)));
    }

    public void SendData(byte[] data, SendDataOptions options, NetworkGamer recipient)
    {
      this.Session.commandQueue.Enqueue(new CommandEvent((ICommand) new CommandSendData(data, 0, data.Length, options, recipient, this)));
    }

    public void SendData(PacketWriter data, SendDataOptions options)
    {
      this.SendData(data.Data, 0, data.Length, options, (NetworkGamer) null);
      data.Reset();
    }

    public void SendData(PacketWriter data, SendDataOptions options, NetworkGamer recipient)
    {
      this.SendData(data.Data, 0, data.Length, options, recipient);
      data.Reset();
    }
  }
}
