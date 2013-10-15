// Type: Microsoft.Xna.Framework.GamerServices.SignedInGamer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.GamerServices
{
  public class SignedInGamer : Gamer
  {
    private GamerPrivileges _privileges = new GamerPrivileges();
    private AchievementCollection gamerAchievements;
    private FriendCollection friendCollection;

    public GameDefaults GameDefaults
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    public bool IsGuest
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    public bool IsSignedInToLive
    {
      get
      {
        return false;
      }
    }

    public int PartySize
    {
      get
      {
        throw new NotSupportedException();
      }
      set
      {
        throw new NotSupportedException();
      }
    }

    public PlayerIndex PlayerIndex
    {
      get
      {
        return PlayerIndex.One;
      }
    }

    public GamerPresence Presence
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    public GamerPrivileges Privileges
    {
      get
      {
        return this._privileges;
      }
    }

    public static event EventHandler<SignedInEventArgs> SignedIn;

    public static event EventHandler<SignedOutEventArgs> SignedOut;

    public SignedInGamer()
    {
      this.EndAuthentication(this.BeginAuthentication((AsyncCallback) null, (object) null));
    }

    public IAsyncResult BeginAuthentication(AsyncCallback callback, object asyncState)
    {
      SignedInGamer.AuthenticationDelegate authenticationDelegate = new SignedInGamer.AuthenticationDelegate(this.DoAuthentication);
      return authenticationDelegate.BeginInvoke(callback, (object) authenticationDelegate);
    }

    public void EndAuthentication(IAsyncResult result)
    {
      ((SignedInGamer.AuthenticationDelegate) result.AsyncState).EndInvoke(result);
    }

    private void DoAuthentication()
    {
    }

    private void AuthenticationCompletedCallback(IAsyncResult result)
    {
      this.EndAuthentication(result);
    }

    public FriendCollection GetFriends()
    {
      if (this.IsSignedInToLive && this.friendCollection == null)
        this.friendCollection = new FriendCollection();
      return this.friendCollection;
    }

    public bool IsFriend(Gamer gamer)
    {
      if (gamer == null)
        throw new ArgumentNullException();
      if (gamer.IsDisposed)
        throw new ObjectDisposedException(gamer.ToString());
      bool flag = false;
      foreach (Gamer gamer1 in this.friendCollection)
      {
        if (gamer1.Gamertag == gamer.Gamertag)
          flag = true;
      }
      return flag;
    }

    public IAsyncResult BeginGetAchievements(AsyncCallback callback, object asyncState)
    {
      SignedInGamer.GetAchievementsDelegate achievementsDelegate = new SignedInGamer.GetAchievementsDelegate(this.GetAchievements);
      return achievementsDelegate.BeginInvoke(callback, (object) achievementsDelegate);
    }

    private void GetAchievementCompletedCallback(IAsyncResult result)
    {
      this.gamerAchievements = ((SignedInGamer.GetAchievementsDelegate) result.AsyncState).EndInvoke(result);
    }

    public AchievementCollection EndGetAchievements(IAsyncResult result)
    {
      this.gamerAchievements = ((SignedInGamer.GetAchievementsDelegate) result.AsyncState).EndInvoke(result);
      return this.gamerAchievements;
    }

    public AchievementCollection GetAchievements()
    {
      if (this.IsSignedInToLive && this.gamerAchievements == null)
        this.gamerAchievements = new AchievementCollection();
      return this.gamerAchievements;
    }

    public IAsyncResult BeginAwardAchievement(string achievementId, AsyncCallback callback, object state)
    {
      return this.BeginAwardAchievement(achievementId, 100.0, callback, state);
    }

    public IAsyncResult BeginAwardAchievement(string achievementId, double percentageComplete, AsyncCallback callback, object state)
    {
      SignedInGamer.AwardAchievementDelegate achievementDelegate = new SignedInGamer.AwardAchievementDelegate(this.DoAwardAchievement);
      return achievementDelegate.BeginInvoke(achievementId, percentageComplete, callback, (object) achievementDelegate);
    }

    public void EndAwardAchievement(IAsyncResult result)
    {
      ((SignedInGamer.AwardAchievementDelegate) result.AsyncState).EndInvoke(result);
    }

    private void AwardAchievementCompletedCallback(IAsyncResult result)
    {
      this.EndAwardAchievement(result);
    }

    public void AwardAchievement(string achievementId)
    {
      this.AwardAchievement(achievementId, 100.0);
    }

    public void DoAwardAchievement(string achievementId, double percentageComplete)
    {
    }

    public void AwardAchievement(string achievementId, double percentageComplete)
    {
      if (!this.IsSignedInToLive)
        return;
      this.BeginAwardAchievement(achievementId, percentageComplete, new AsyncCallback(this.AwardAchievementCompletedCallback), (object) null);
    }

    public void UpdateScore(string aCategory, long aScore)
    {
      if (!this.IsSignedInToLive)
        ;
    }

    public void ResetAchievements()
    {
      if (!this.IsSignedInToLive)
        ;
    }

    protected virtual void OnSignedIn(SignedInEventArgs e)
    {
      if (SignedInGamer.SignedIn == null)
        return;
      SignedInGamer.SignedIn((object) this, e);
    }

    protected virtual void OnSignedOut(SignedOutEventArgs e)
    {
      if (SignedInGamer.SignedOut == null)
        return;
      SignedInGamer.SignedOut((object) this, e);
    }

    private delegate void AuthenticationDelegate();

    private delegate AchievementCollection GetAchievementsDelegate();

    private delegate void AwardAchievementDelegate(string achievementId, double percentageComplete);
  }
}
