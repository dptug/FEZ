// Type: Microsoft.Xna.Framework.Net.NetworkGamer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.GamerServices;
using System;
using System.ComponentModel;

namespace Microsoft.Xna.Framework.Net
{
  public class NetworkGamer : Gamer, INotifyPropertyChanged
  {
    private long remoteUniqueIdentifier = -1L;
    private byte id;
    private NetworkSession session;
    private GamerStates gamerState;
    private GamerStates oldGamerState;
    private NetworkMachine _Machine;

    internal long RemoteUniqueIdentifier
    {
      get
      {
        return this.remoteUniqueIdentifier;
      }
      set
      {
        this.remoteUniqueIdentifier = value;
      }
    }

    public bool HasLeftSession
    {
      get
      {
        return false;
      }
    }

    public bool HasVoice
    {
      get
      {
        return (this.gamerState & GamerStates.HasVoice) != (GamerStates) 0;
      }
    }

    public byte Id
    {
      get
      {
        return this.id;
      }
    }

    public bool IsGuest
    {
      get
      {
        return (this.gamerState & GamerStates.Guest) != (GamerStates) 0;
      }
    }

    public bool IsHost
    {
      get
      {
        return (this.gamerState & GamerStates.Host) != (GamerStates) 0;
      }
    }

    public bool IsLocal
    {
      get
      {
        return (this.gamerState & GamerStates.Local) != (GamerStates) 0;
      }
    }

    public bool IsMutedByLocalUser
    {
      get
      {
        return true;
      }
    }

    public bool IsPrivateSlot
    {
      get
      {
        return false;
      }
    }

    public bool IsReady
    {
      get
      {
        return (this.gamerState & GamerStates.Ready) != (GamerStates) 0;
      }
      set
      {
        if ((this.gamerState & GamerStates.Ready) != (GamerStates) 0 == value)
          return;
        if (value)
          this.gamerState |= GamerStates.Ready;
        else
          this.gamerState &= ~GamerStates.Ready;
        this.OnPropertyChanged("Ready");
      }
    }

    public bool IsTalking
    {
      get
      {
        return false;
      }
    }

    public NetworkMachine Machine
    {
      get
      {
        return this._Machine;
      }
      set
      {
        if (this._Machine == value)
          return;
        this._Machine = value;
      }
    }

    public TimeSpan RoundtripTime
    {
      get
      {
        return TimeSpan.MinValue;
      }
    }

    public NetworkSession Session
    {
      get
      {
        return this.session;
      }
    }

    internal GamerStates State
    {
      get
      {
        return this.gamerState;
      }
      set
      {
        this.gamerState = value;
      }
    }

    internal GamerStates OldState
    {
      get
      {
        return this.oldGamerState;
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public NetworkGamer(NetworkSession session, byte id, GamerStates state)
    {
      this.id = id;
      this.session = session;
      this.gamerState = state;
      this.gamerState = state;
      this.oldGamerState = state;
    }

    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler changedEventHandler = this.PropertyChanged;
      if (changedEventHandler == null)
        return;
      changedEventHandler((object) this, new PropertyChangedEventArgs(name));
    }
  }
}
