// Type: Microsoft.Xna.Framework.GamePlatform
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Diagnostics;

namespace Microsoft.Xna.Framework
{
  internal abstract class GamePlatform : IDisposable
  {
    protected TimeSpan _inactiveSleepTime = TimeSpan.FromMilliseconds(20.0);
    protected bool _needsToResetElapsedTime;
    private bool disposed;
    private bool _isActive;
    private bool _isMouseVisible;

    protected bool IsDisposed
    {
      get
      {
        return this.disposed;
      }
    }

    public abstract GameRunBehavior DefaultRunBehavior { get; }

    public Game Game { get; private set; }

    protected bool DeactivateForBounds { get; set; }

    public bool IsActive
    {
      get
      {
        return this._isActive;
      }
      internal set
      {
        if (this._isActive == value)
          return;
        this._isActive = value;
        this.Raise<EventArgs>(this._isActive ? this.Activated : this.Deactivated, (EventArgs) new ActiveEventArgs(this.DeactivateForBounds));
      }
    }

    public bool IsMouseVisible
    {
      get
      {
        return this._isMouseVisible;
      }
      set
      {
        if (this._isMouseVisible == value)
          return;
        this._isMouseVisible = value;
        this.OnIsMouseVisibleChanged();
      }
    }

    public GameWindow Window { get; protected set; }

    public virtual bool VSyncEnabled
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
      }
    }

    public event EventHandler<EventArgs> AsyncRunLoopEnded;

    public event EventHandler<EventArgs> Activated;

    public event EventHandler<EventArgs> Deactivated;

    protected GamePlatform(Game game)
    {
      if (game == null)
        throw new ArgumentNullException("game");
      this.Game = game;
    }

    ~GamePlatform()
    {
      this.Dispose(false);
    }

    public static GamePlatform Create(Game game)
    {
      return (GamePlatform) new OpenTKGamePlatform(game);
    }

    private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e) where TEventArgs : EventArgs
    {
      if (handler == null)
        return;
      handler((object) this, e);
    }

    protected void RaiseAsyncRunLoopEnded()
    {
      this.Raise<EventArgs>(this.AsyncRunLoopEnded, EventArgs.Empty);
    }

    public virtual void BeforeInitialize()
    {
      this.IsActive = true;
      if (this.Game.GraphicsDevice != null)
        return;
      (this.Game.Services.GetService(typeof (IGraphicsDeviceManager)) as IGraphicsDeviceManager).CreateDevice();
    }

    public virtual bool BeforeRun()
    {
      return true;
    }

    public abstract void Exit();

    public abstract void RunLoop();

    public abstract void StartRunLoop();

    public abstract bool BeforeUpdate(GameTime gameTime);

    public abstract bool BeforeDraw(GameTime gameTime);

    public abstract void EnterFullScreen();

    public abstract void ExitFullScreen();

    public virtual TimeSpan TargetElapsedTimeChanging(TimeSpan value)
    {
      return value;
    }

    public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

    public abstract void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight);

    public virtual void TargetElapsedTimeChanged()
    {
    }

    public virtual void ResetElapsedTime()
    {
    }

    protected virtual void OnIsMouseVisibleChanged()
    {
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
    }

    [Conditional("DEBUG")]
    public virtual void Log(string Message)
    {
    }

    public virtual void Present()
    {
    }
  }
}
