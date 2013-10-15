// Type: MonoGame.Framework.GamerServices.MonoLiveClient
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.GamerServices;
using MonoGame.Framework.MonoLive;
using System;

namespace MonoGame.Framework.GamerServices
{
  internal class MonoLiveClient
  {
    private static MonoLiveClient instance = (MonoLiveClient) null;

    public static MonoLiveClient Instance
    {
      get
      {
        if (MonoLiveClient.instance == null)
          MonoLiveClient.instance = new MonoLiveClient();
        return MonoLiveClient.instance;
      }
    }

    public event MonoLiveClient.SignInCompletedEventHandler SignInCompleted;

    static MonoLiveClient()
    {
    }

    internal MonoLiveClient()
    {
    }

    public void SignInAsync(string username, string password)
    {
      MonoLive monoLive = new MonoLive();
      monoLive.SignInCompleted += new MonoGame.Framework.MonoLive.SignInCompletedEventHandler(this.client_SignInCompleted);
      monoLive.SignInAsync(username, password, (object) monoLive);
    }

    private void client_SignInCompleted(object sender, MonoGame.Framework.MonoLive.SignInCompletedEventArgs e)
    {
      if (this.SignInCompleted != null && e.Error != null)
      {
        ((IDisposable) e.UserState).Dispose();
        MonoLiveClient.SignInCompletedEventHandler completedEventHandler = this.SignInCompleted;
        MonoLiveClient monoLiveClient = this;
        SignedInGamer signedInGamer = new SignedInGamer();
        signedInGamer.Gamertag = e.Result.Gamer.GamerTag;
        signedInGamer.DisplayName = e.Result.Gamer.GamerTag;
        SignInCompletedEventArgs e1 = new SignInCompletedEventArgs((Microsoft.Xna.Framework.GamerServices.Gamer) signedInGamer);
        completedEventHandler((object) monoLiveClient, e1);
      }
      else
        this.SignInCompleted((object) this, (SignInCompletedEventArgs) null);
    }

    public Microsoft.Xna.Framework.GamerServices.Gamer SignIn(string username, string password)
    {
      using (MonoLive monoLive = new MonoLive())
      {
        Result result = monoLive.SignIn(username, password);
        if (result.ok)
        {
          SignedInGamer signedInGamer = new SignedInGamer();
          signedInGamer.Gamertag = result.Gamer.GamerTag;
          signedInGamer.DisplayName = result.Gamer.GamerTag;
          return (Microsoft.Xna.Framework.GamerServices.Gamer) signedInGamer;
        }
      }
      return (Microsoft.Xna.Framework.GamerServices.Gamer) null;
    }

    public delegate void SignInCompletedEventHandler(object sender, SignInCompletedEventArgs e);
  }
}
