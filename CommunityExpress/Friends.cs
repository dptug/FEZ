// Type: CommunityExpressNS.Friends
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Friends : ICollection<Friend>, IEnumerable<Friend>, IEnumerable
  {
    private IntPtr _friends;
    private EFriendFlags _friendFlags;
    private OnLargeAvatarReceivedFromSteam _internalOnLargeAvatarReceived;
    private OnLargeAvatarReceived _largeAvatarReceivedCallback;

    public int Count
    {
      get
      {
        return Friends.SteamUnityAPI_SteamFriends_GetFriendCount(this._friends, (int) this._friendFlags);
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    internal Friends()
    {
      this._friends = Friends.SteamUnityAPI_SteamFriends();
      this._friendFlags = EFriendFlags.k_EFriendFlagImmediate;
    }

    internal Friends(EFriendFlags friendFlags)
    {
      this._friends = Friends.SteamUnityAPI_SteamFriends();
      this._friendFlags = friendFlags;
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamFriends();

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamFriends_GetFriendCount(IntPtr friends, int friendFlags);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamFriends_GetFriendByIndex(IntPtr friends, int iFriend, int friendFlags);

    [DllImport("CommunityExpressSW")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static string SteamUnityAPI_SteamFriends_GetFriendPersonaName(IntPtr friends, ulong steamIDFriend);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamFriends_GetFriendPersonaState(IntPtr friends, ulong steamIDFriend);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamFriends_GetSmallFriendAvatar(IntPtr friends, ulong steamIDFriend);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamFriends_GetMediumFriendAvatar(IntPtr friends, ulong steamIDFriend);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamFriends_GetLargeFriendAvatar(IntPtr friends, ulong steamIDFriend, IntPtr OnAvatarReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamFriends_ActivateGameOverlay([MarshalAs(UnmanagedType.LPStr)] string dialog);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamFriends_ActivateGameOverlayToUser([MarshalAs(UnmanagedType.LPStr)] string dialog, ulong steamIDUser);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamFriends_ActivateGameOverlayToWebPage([MarshalAs(UnmanagedType.LPStr)] string url);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamFriends_ActivateGameOverlayToStore(uint appID, EOverlayToStoreFlag flag);

    private SteamID GetFriendByIndex(int iFriend)
    {
      return new SteamID(Friends.SteamUnityAPI_SteamFriends_GetFriendByIndex(this._friends, iFriend, (int) this._friendFlags));
    }

    internal string GetFriendPersonaName(SteamID steamIDFriend)
    {
      return Friends.SteamUnityAPI_SteamFriends_GetFriendPersonaName(this._friends, steamIDFriend.ToUInt64());
    }

    internal EPersonaState GetFriendPersonaState(SteamID steamIDFriend)
    {
      return (EPersonaState) Friends.SteamUnityAPI_SteamFriends_GetFriendPersonaState(this._friends, steamIDFriend.ToUInt64());
    }

    internal Image GetSmallFriendAvatar(SteamID steamIDFriend)
    {
      int smallFriendAvatar = Friends.SteamUnityAPI_SteamFriends_GetSmallFriendAvatar(this._friends, steamIDFriend.ToUInt64());
      if (smallFriendAvatar != -1)
        return new Image(smallFriendAvatar);
      else
        return (Image) null;
    }

    internal Image GetMediumFriendAvatar(SteamID steamIDFriend)
    {
      int mediumFriendAvatar = Friends.SteamUnityAPI_SteamFriends_GetMediumFriendAvatar(this._friends, steamIDFriend.ToUInt64());
      if (mediumFriendAvatar != -1)
        return new Image(mediumFriendAvatar);
      else
        return (Image) null;
    }

    internal Image GetLargeFriendAvatar(SteamID steamIDFriend, OnLargeAvatarReceived largeAvatarReceivedCallback)
    {
      this._largeAvatarReceivedCallback = largeAvatarReceivedCallback;
      if (this._internalOnLargeAvatarReceived == null)
        this._internalOnLargeAvatarReceived = new OnLargeAvatarReceivedFromSteam(this.InternalOnLargeAvatarReceived);
      int largeFriendAvatar = Friends.SteamUnityAPI_SteamFriends_GetLargeFriendAvatar(this._friends, steamIDFriend.ToUInt64(), Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnLargeAvatarReceived));
      if (largeFriendAvatar == -1)
        return (Image) null;
      Image avatar = new Image(largeFriendAvatar);
      if (this._largeAvatarReceivedCallback != null)
        this._largeAvatarReceivedCallback(steamIDFriend, avatar);
      return avatar;
    }

    private void InternalOnLargeAvatarReceived(ref AvatarImageLoaded_t CallbackData)
    {
      this._largeAvatarReceivedCallback(new SteamID(CallbackData.m_steamID), new Image(CallbackData.m_iImage));
    }

    public void ActivateGameOverlay(string dialog)
    {
      Friends.SteamUnityAPI_SteamFriends_ActivateGameOverlay(dialog);
    }

    public void ActivateGameOverlayToUser(string dialog, SteamID user)
    {
      Friends.SteamUnityAPI_SteamFriends_ActivateGameOverlayToUser(dialog, user.ToUInt64());
    }

    public void ActivateGameOverlayToWebPage(string url)
    {
      Friends.SteamUnityAPI_SteamFriends_ActivateGameOverlayToWebPage(url);
    }

    public void ActivateGameOverlayToStore(uint appID, EOverlayToStoreFlag flag)
    {
      Friends.SteamUnityAPI_SteamFriends_ActivateGameOverlayToStore(appID, flag);
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
      return (IEnumerator<Friend>) new Friends.FriendEnumator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    private class FriendEnumator : IEnumerator<Friend>, IDisposable, IEnumerator
    {
      private int _index;
      private Friends _friends;

      public Friend Current
      {
        get
        {
          return new Friend(this._friends, this._friends.GetFriendByIndex(this._index));
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.Current;
        }
      }

      public FriendEnumator(Friends friends)
      {
        this._friends = friends;
        this._index = -1;
      }

      public bool MoveNext()
      {
        ++this._index;
        return this._index < this._friends.Count;
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
