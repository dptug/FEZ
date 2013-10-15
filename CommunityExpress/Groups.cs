// Type: CommunityExpressNS.Groups
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Groups : ICollection<Group>, IEnumerable<Group>, IEnumerable
  {
    private IntPtr _friends;
    private Friends _friendsRef;

    public int Count
    {
      get
      {
        return Groups.SteamUnityAPI_SteamFriends_GetClanCount(this._friends);
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    internal Groups(Friends friendsRef)
    {
      this._friends = Groups.SteamUnityAPI_SteamFriends();
      this._friendsRef = friendsRef;
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamFriends();

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamFriends_GetClanCount(IntPtr friends);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamFriends_GetClanByIndex(IntPtr friends, int iClan);

    [DllImport("CommunityExpressSW")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static string SteamUnityAPI_SteamFriends_GetClanName(IntPtr friends, ulong steamIDClan);

    [DllImport("CommunityExpressSW")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static string SteamUnityAPI_SteamFriends_GetClanTag(IntPtr friends, ulong steamIDClan);

    private SteamID GetGroupByIndex(int iGroup)
    {
      return new SteamID(Groups.SteamUnityAPI_SteamFriends_GetClanByIndex(this._friends, iGroup));
    }

    internal string GetGroupName(SteamID steamIDGroup)
    {
      return Groups.SteamUnityAPI_SteamFriends_GetClanName(this._friends, steamIDGroup.ToUInt64());
    }

    internal string GetClanTag(SteamID steamIDClan)
    {
      return Groups.SteamUnityAPI_SteamFriends_GetClanTag(this._friends, steamIDClan.ToUInt64());
    }

    public void Add(Group item)
    {
      throw new NotSupportedException();
    }

    public void Clear()
    {
      throw new NotSupportedException();
    }

    public bool Contains(Group item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Group[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(Group item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<Group> GetEnumerator()
    {
      return (IEnumerator<Group>) new Groups.GroupEnumator(this, this._friendsRef);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    private class GroupEnumator : IEnumerator<Group>, IDisposable, IEnumerator
    {
      private int _index;
      private Groups _groups;
      private Friends _friendsRef;

      public Group Current
      {
        get
        {
          return new Group(this._groups, this._friendsRef, this._groups.GetGroupByIndex(this._index));
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.Current;
        }
      }

      public GroupEnumator(Groups groups, Friends friendsRef)
      {
        this._groups = groups;
        this._friendsRef = friendsRef;
        this._index = -1;
      }

      public bool MoveNext()
      {
        ++this._index;
        return this._index < this._groups.Count;
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
