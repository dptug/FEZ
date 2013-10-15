// Type: CommunityExpressNS.Lobby
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Lobby : ICollection<Friend>, IEnumerable<Friend>, IEnumerable
  {
    private IntPtr _matchmaking;
    private Lobbies _lobbies;
    private SteamID _id;
    private bool _isLocked;
    private uint _chatPermissions;

    public SteamID SteamID
    {
      get
      {
        return this._id;
      }
    }

    public bool IsLocked
    {
      get
      {
        return this._isLocked;
      }
      internal set
      {
        this._isLocked = value;
      }
    }

    public uint ChatPermissions
    {
      get
      {
        return this._chatPermissions;
      }
      internal set
      {
        this._chatPermissions = value;
      }
    }

    public int Count
    {
      get
      {
        return Lobby.SteamUnityAPI_SteamMatchmaking_GetNumLobbyMembers(this._matchmaking, this._id.ToUInt64());
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    internal Lobby(Lobbies lobbies, SteamID id)
    {
      this._matchmaking = Lobby.SteamUnityAPI_SteamMatchmaking();
      this._lobbies = lobbies;
      this._id = id;
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamMatchmaking();

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamMatchmaking_GetNumLobbyMembers(IntPtr matchmaking, ulong steamIDLobby);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamMatchmaking_GetLobbyMemberByIndex(IntPtr matchmaking, ulong steamIDLobby, int iLobbyMember);

    private SteamID GetLobbyMemberByIndex(int iLobbyMember)
    {
      return new SteamID(Lobby.SteamUnityAPI_SteamMatchmaking_GetLobbyMemberByIndex(this._matchmaking, this._id.ToUInt64(), iLobbyMember));
    }

    public void Add(Friend item)
    {
      throw new NotSupportedException();
    }

    public void Clear()
    {
      throw new NotSupportedException();
    }

    public bool Contains(Friend item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Friend[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(Friend item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<Friend> GetEnumerator()
    {
      return (IEnumerator<Friend>) new Lobby.FriendEnumator(this, CommunityExpress.Instance.Friends);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    private class FriendEnumator : IEnumerator<Friend>, IDisposable, IEnumerator
    {
      private int _index;
      private Lobby _lobby;
      private Friends _friends;

      public Friend Current
      {
        get
        {
          return new Friend(this._friends, this._lobby.GetLobbyMemberByIndex(this._index));
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.Current;
        }
      }

      public FriendEnumator(Lobby lobby, Friends friends)
      {
        this._lobby = lobby;
        this._friends = friends;
        this._index = -1;
      }

      public bool MoveNext()
      {
        ++this._index;
        return this._index < this._lobby.Count;
      }

      public void Reset()
      {
        this._index = -1;
      }

      public void Dispose()
      {
      }
    }
  }
}
