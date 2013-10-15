// Type: CommunityExpressNS.Friend
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

namespace CommunityExpressNS
{
  public class Friend
  {
    private Friends _friends;
    private SteamID _id;

    public string PersonaName
    {
      get
      {
        return this._friends.GetFriendPersonaName(this._id);
      }
    }

    public EPersonaState PersonaState
    {
      get
      {
        return this._friends.GetFriendPersonaState(this._id);
      }
    }

    public SteamID SteamID
    {
      get
      {
        return this._id;
      }
    }

    public Image SmallAvatar
    {
      get
      {
        return this._friends.GetSmallFriendAvatar(this._id);
      }
    }

    public Image MediumAvatar
    {
      get
      {
        return this._friends.GetMediumFriendAvatar(this._id);
      }
    }

    public Image LargeAvatar
    {
      get
      {
        return this._friends.GetLargeFriendAvatar(this._id, (OnLargeAvatarReceived) null);
      }
    }

    internal Friend(Friends friends, SteamID id)
    {
      this._friends = friends;
      this._id = id;
    }

    public void GetLargeAvatar(OnLargeAvatarReceived largeAvatarReceivedCallback)
    {
      this._friends.GetLargeFriendAvatar(this._id, largeAvatarReceivedCallback);
    }
  }
}
