// Type: Microsoft.Xna.Framework.OpenTKGamePlatform
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
using OpenTK;
using System;

namespace Microsoft.Xna.Framework
{
  internal class OpenTKGamePlatform : GamePlatform
  {
    private OpenALSoundController soundControllerInstance = (OpenALSoundController) null;
    private bool isCurrentlyFullScreen = false;
    private OpenTKGameWindow _view;

    public override bool VSyncEnabled
    {
      get
      {
        return this._view.Window.VSync == VSyncMode.On;
      }
      set
      {
        this._view.Window.VSync = value ? VSyncMode.On : VSyncMode.Off;
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
      this.IsMouseVisible = true;
      this.soundControllerInstance = OpenALSoundController.Instance;
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
      bool isActive = this.IsActive;
      this.IsActive = false;
      GraphicsDeviceManager graphicsDeviceManager = (GraphicsDeviceManager) this.Game.Services.GetService(typeof (IGraphicsDeviceManager));
      this.VSyncEnabled = graphicsDeviceManager.SynchronizeWithVerticalRetrace;
      if (graphicsDeviceManager.IsFullScreen)
      {
        clientBounds = new Rectangle(0, 0, graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight);
        if (DisplayDevice.Default.Width != graphicsDeviceManager.PreferredBackBufferWidth || DisplayDevice.Default.Height != graphicsDeviceManager.PreferredBackBufferHeight)
          DisplayDevice.Default.ChangeResolution(graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight, DisplayDevice.Default.BitsPerPixel, DisplayDevice.Default.RefreshRate);
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
      this.isCurrentlyFullScreen = graphicsDeviceManager.IsFullScreen;
      this.IsActive = isActive;
    }

    public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
    {
    }

    public override void BeginScreenDeviceChange(bool willBeFullScreen)
    {
    }

    protected override void OnIsMouseVisibleChanged()
    {
      Microsoft.Xna.Framework.Input.MouseState state = Microsoft.Xna.Framework.Input.Mouse.GetState();
      this._view.Window.CursorVisible = this.IsMouseVisible;
      System.Drawing.Point point = this._view.Window.PointToScreen(new System.Drawing.Point(state.X, state.Y));
      OpenTK.Input.Mouse.SetPosition((double) point.X, (double) point.Y);
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
      this.soundControllerInstance.Dispose();
      base.Dispose(disposing);
    }
  }
}
