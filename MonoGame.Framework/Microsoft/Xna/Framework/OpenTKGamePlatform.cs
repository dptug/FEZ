// Type: Microsoft.Xna.Framework.OpenTKGamePlatform
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
  internal class OpenTKGamePlatform : GamePlatform
  {
    private OpenTKGameWindow _view;
    private OpenALSoundController soundControllerInstance;
    private bool isCurrentlyFullScreen;

    public override bool VSyncEnabled
    {
      get
      {
        return this._view.Window.VSync == VSyncMode.On;
      }
      set
      {
        this._view.Window.VSync = value ? VSyncMode.On : VSyncMode.Off;
        this._view.Window.Context.SwapInterval = value ? 1 : 0;
      }
    }

    public override GameRunBehavior DefaultRunBehavior
    {
      get
      {
        return GameRunBehavior.Synchronous;
      }
    }

    public OpenTKGamePlatform(Game game)
      : base(game)
    {
      this._view = new OpenTKGameWindow();
      this._view.Game = game;
      this.Window = (GameWindow) this._view;
      this.soundControllerInstance = OpenALSoundController.Instance;
      WindowsHelperAccessibilityKeys.AllowAccessibilityShortcutKeys(false);
    }

    protected override void OnIsMouseVisibleChanged()
    {
      this._view.MouseVisibleToggled();
    }

    public override void RunLoop()
    {
      this.ResetWindowBounds(false);
      this._view.Window.Run(0.0);
    }

    public override void StartRunLoop()
    {
      throw new NotImplementedException();
    }

    public override void Exit()
    {
      if (!this._view.Window.IsExiting)
      {
        NetworkSession.Exit();
        this._view.Window.Exit();
      }
      DisplayDevice.Default.RestoreResolution();
    }

    public override bool BeforeUpdate(GameTime gameTime)
    {
      this.IsActive = this._view.Window.Focused;
      this.soundControllerInstance.Update(gameTime);
      return true;
    }

    public override bool BeforeDraw(GameTime gameTime)
    {
      return true;
    }

    public override void EnterFullScreen()
    {
      this.ResetWindowBounds(false);
    }

    public override void ExitFullScreen()
    {
      this.ResetWindowBounds(false);
    }

    internal void ResetWindowBounds(bool toggleFullScreen)
    {
      Rectangle clientBounds = this.Window.ClientBounds;
      this.DeactivateForBounds = true;
      bool isActive = this.IsActive;
      this.IsActive = false;
      GraphicsDeviceManager graphicsDeviceManager = (GraphicsDeviceManager) this.Game.Services.GetService(typeof (IGraphicsDeviceManager));
      if (graphicsDeviceManager.IsFullScreen)
      {
        clientBounds = new Rectangle(0, 0, graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight);
        if (DisplayDevice.Default.Width != graphicsDeviceManager.PreferredBackBufferWidth || DisplayDevice.Default.Height != graphicsDeviceManager.PreferredBackBufferHeight)
        {
          DisplayResolution resolution = (DisplayResolution) null;
          foreach (DisplayResolution displayResolution in (IEnumerable<DisplayResolution>) DisplayDevice.Default.AvailableResolutions)
          {
            if (displayResolution.Width == graphicsDeviceManager.PreferredBackBufferWidth && displayResolution.Height == graphicsDeviceManager.PreferredBackBufferHeight && displayResolution.BitsPerPixel == 32)
            {
              if (resolution == (DisplayResolution) null)
                resolution = displayResolution;
              if ((double) displayResolution.RefreshRate > (double) resolution.RefreshRate)
                resolution = displayResolution;
            }
          }
          DisplayDevice.Default.ChangeResolution(resolution);
        }
      }
      else
      {
        DisplayDevice.Default.RestoreResolution();
        clientBounds.Width = graphicsDeviceManager.PreferredBackBufferWidth;
        clientBounds.Height = graphicsDeviceManager.PreferredBackBufferHeight;
      }
      GraphicsDevice graphicsDevice = graphicsDeviceManager.GraphicsDevice;
      if (graphicsDevice != null)
      {
        PresentationParameters presentationParameters = graphicsDevice.PresentationParameters;
        presentationParameters.BackBufferHeight = clientBounds.Height;
        presentationParameters.BackBufferWidth = clientBounds.Width;
      }
      if (graphicsDeviceManager.IsFullScreen != this.isCurrentlyFullScreen)
        this._view.ToggleFullScreen();
      this._view.ChangeClientBounds(clientBounds);
      this.VSyncEnabled = graphicsDeviceManager.SynchronizeWithVerticalRetrace;
      this.isCurrentlyFullScreen = graphicsDeviceManager.IsFullScreen;
      this.IsActive = isActive;
      this.DeactivateForBounds = false;
    }

    public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
    {
    }

    public override void BeginScreenDeviceChange(bool willBeFullScreen)
    {
    }

    public override void Log(string Message)
    {
      Console.WriteLine(Message);
    }

    public override void Present()
    {
      base.Present();
      GraphicsDevice graphicsDevice = this.Game.GraphicsDevice;
      if (graphicsDevice != null)
        graphicsDevice.Present();
      if (this._view == null)
        return;
      this._view.Window.SwapBuffers();
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed && this._view != null)
      {
        this._view.Dispose();
        this._view = (OpenTKGameWindow) null;
      }
      if (this.soundControllerInstance != null)
        this.soundControllerInstance.Dispose();
      SdlGamePad.Cleanup();
      WindowsHelperAccessibilityKeys.AllowAccessibilityShortcutKeys(true);
      base.Dispose(disposing);
    }
  }
}
