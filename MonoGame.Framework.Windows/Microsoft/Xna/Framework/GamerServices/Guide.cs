// Type: Microsoft.Xna.Framework.GamerServices.Guide
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.GamerServices
{
  public static class Guide
  {
    private static bool isScreenSaverEnabled;
    private static bool isTrialMode;
    private static bool isVisible;
    private static bool simulateTrialMode;

    public static bool IsScreenSaverEnabled
    {
      get
      {
        return Guide.isScreenSaverEnabled;
      }
      set
      {
        Guide.isScreenSaverEnabled = value;
      }
    }

    public static bool IsTrialMode
    {
      get
      {
        return Guide.simulateTrialMode || Guide.isTrialMode;
      }
      set
      {
        Guide.isTrialMode = value;
      }
    }

    public static bool IsVisible
    {
      get
      {
        return Guide.isVisible;
      }
      set
      {
        Guide.isVisible = value;
      }
    }

    public static bool SimulateTrialMode
    {
      get
      {
        return Guide.simulateTrialMode;
      }
      set
      {
        Guide.simulateTrialMode = value;
      }
    }

    public static GameWindow Window { get; set; }

    static Guide()
    {
    }

    public static string ShowKeyboardInput(PlayerIndex player, string title, string description, string defaultText, bool usePasswordMode)
    {
      throw new NotImplementedException();
    }

    public static IAsyncResult BeginShowKeyboardInput(PlayerIndex player, string title, string description, string defaultText, AsyncCallback callback, object state)
    {
      return Guide.BeginShowKeyboardInput(player, title, description, defaultText, callback, state, false);
    }

    public static IAsyncResult BeginShowKeyboardInput(PlayerIndex player, string title, string description, string defaultText, AsyncCallback callback, object state, bool usePasswordMode)
    {
      Guide.isVisible = true;
      Guide.ShowKeyboardInputDelegate keyboardInputDelegate = new Guide.ShowKeyboardInputDelegate(Guide.ShowKeyboardInput);
      return keyboardInputDelegate.BeginInvoke(player, title, description, defaultText, usePasswordMode, callback, (object) keyboardInputDelegate);
    }

    public static string EndShowKeyboardInput(IAsyncResult result)
    {
      try
      {
        return ((Guide.ShowKeyboardInputDelegate) result.AsyncState).EndInvoke(result);
      }
      finally
      {
        Guide.isVisible = false;
      }
    }

    public static int? ShowMessageBox(string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon)
    {
      return new int?();
    }

    public static IAsyncResult BeginShowMessageBox(PlayerIndex player, string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon, AsyncCallback callback, object state)
    {
      Guide.isVisible = true;
      Guide.ShowMessageBoxDelegate messageBoxDelegate = new Guide.ShowMessageBoxDelegate(Guide.ShowMessageBox);
      return messageBoxDelegate.BeginInvoke(title, text, buttons, focusButton, icon, callback, (object) messageBoxDelegate);
    }

    public static IAsyncResult BeginShowMessageBox(string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon, AsyncCallback callback, object state)
    {
      return Guide.BeginShowMessageBox(PlayerIndex.One, title, text, buttons, focusButton, icon, callback, state);
    }

    public static int? EndShowMessageBox(IAsyncResult result)
    {
      try
      {
        return ((Guide.ShowMessageBoxDelegate) result.AsyncState).EndInvoke(result);
      }
      finally
      {
        Guide.isVisible = false;
      }
    }

    public static void ShowMarketplace(PlayerIndex player)
    {
    }

    public static void Show()
    {
      Guide.ShowSignIn(1, false);
    }

    public static void ShowSignIn(int paneCount, bool onlineOnly)
    {
      if (paneCount != 1 && paneCount != 2 && paneCount != 4)
      {
        ArgumentException argumentException = new ArgumentException("paneCount Can only be 1, 2 or 4 on Windows");
      }
      else
      {
        MonoGameGamerServicesHelper.ShowSigninSheet();
        if (GamerServicesComponent.LocalNetworkGamer == null)
          GamerServicesComponent.LocalNetworkGamer = new LocalNetworkGamer();
        else
          GamerServicesComponent.LocalNetworkGamer.SignedInGamer.BeginAuthentication((AsyncCallback) null, (object) null);
      }
    }

    public static void ShowLeaderboard()
    {
    }

    public static void ShowAchievements()
    {
    }

    public static IAsyncResult BeginShowStorageDeviceSelector(AsyncCallback callback, object state)
    {
      return (IAsyncResult) null;
    }

    public static StorageDevice EndShowStorageDeviceSelector(IAsyncResult result)
    {
      return (StorageDevice) null;
    }

    internal static void Initialise(Game game)
    {
      MonoGameGamerServicesHelper.Initialise(game);
    }

    private delegate string ShowKeyboardInputDelegate(PlayerIndex player, string title, string description, string defaultText, bool usePasswordMode);

    private delegate int? ShowMessageBoxDelegate(string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon);
  }
}
