// Type: CommunityExpressNS.Group
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Group : ICollection<Friend>, IEnumerable<Friend>, IEnumerable
  {
    private IntPtr _friends;
    private Friends _friendsRef;
    private Groups _groups;
    private SteamID _id;

    public string GroupName
    {
      get
      {
        return this._groups.GetGroupName(this._id);
      }
    }

    public string ClanTag
    {
      get
      {
        return this._groups.GetClanTag(this._id);
      }
    }

    public SteamID SteamID
    {
      get
      {
        return this._id;
      }
    }

    public int Count
    {
      get
      {
        return Group.SteamUnityAPI_SteamFriends_GetFriendCountFromSource(this._friends, this._id.ToUInt64());
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    internal Group(Groups groups, Friends friendsRef, SteamID id)
    {
      this._friends = Group.SteamUnityAPI_SteamFriends();
      this._groups = groups;
      this._friendsRef = friendsRef;
      this._id = id;
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamFriends();

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamFriends_GetFriendCountFromSource(IntPtr friends, ulong steamIDSource);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamFriends_GetFriendFromSourceByIndex(IntPtr friends, ulong steamIDSource, int iFriend);

    private SteamID GetGroupMemberByIndex(int iGroupMember)
    {
      return new SteamID(Group.SteamUnityAPI_SteamFriends_GetFriendFromSourceByIndex(this._friends, this._id.ToUInt64(), iGroupMember));
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
      return (IEnumerator<Friend>) new Group.FriendEnumator(this, this._friendsRef);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    private class FriendEnumator : IEnumerator<Friend>, IDisposable, IEnumerator
    {
      private int _index;
      private Group _group;
      private Friends _friends;

      public Friend Current
      {
        get
        {
          return new Friend(this._friends, this._group.GetGroupMemberByIndex(this._index));
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.Current;
        }
      }

      public FriendEnumator(Group group, Friends friends)
      {
        this._group = group;
        this._friends = friends;
        this._index = -1;
      }

      public bool MoveNext()
      {
        ++this._index;
        return this._index < this._group.Count;
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
