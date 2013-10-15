// Type: FezEngine.Components.Waiter`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Components
{
  internal class Waiter<T> : IGameComponent, IUpdateable, IWaiter where T : class, new()
  {
    private readonly IEngineStateManager EngineState;
    private readonly IDefaultCameraManager Camera;
    private Func<T, bool> condition;
    private Action<TimeSpan, T> whileWaiting;
    private Action onValid;
    private readonly T state;
    private int updateOrder;

    public bool Alive { get; private set; }

    public object Tag { get; set; }

    public bool AutoPause { get; set; }

    public Func<bool> CustomPause { get; set; }

    public bool Enabled
    {
      get
      {
        return true;
      }
    }

    public int UpdateOrder
    {
      get
      {
        return this.updateOrder;
      }
      set
      {
        this.updateOrder = value;
        if (this.UpdateOrderChanged == null)
          return;
        this.UpdateOrderChanged((object) this, EventArgs.Empty);
      }
    }

    public event EventHandler<EventArgs> EnabledChanged;

    public event EventHandler<EventArgs> UpdateOrderChanged;

    internal Waiter(Func<T, bool> condition, Action onValid)
      : this(condition, new Action<TimeSpan, T>(Util.NullAction<TimeSpan, T>), onValid, Activator.CreateInstance<T>())
    {
    }

    internal Waiter(Func<T, bool> condition, Action<TimeSpan, T> whileWaiting)
      : this(condition, whileWaiting, new Action(Util.NullAction), Activator.CreateInstance<T>())
    {
    }

    internal Waiter(Func<T, bool> condition, Action<TimeSpan, T> whileWaiting, Action onValid)
      : this(condition, whileWaiting, onValid, Activator.CreateInstance<T>())
    {
    }

    internal Waiter(Func<T, bool> condition, Action<TimeSpan, T> whileWaiting, T state)
      : this(condition, whileWaiting, new Action(Util.NullAction), state)
    {
    }

    internal Waiter(Func<T, bool> condition, Action<TimeSpan, T> whileWaiting, Action onValid, T state)
    {
      this.condition = condition;
      this.whileWaiting = whileWaiting;
      this.onValid = onValid;
      this.state = state;
      this.Alive = true;
      this.EngineState = ServiceHelper.Get<IEngineStateManager>();
      this.Camera = ServiceHelper.Get<IDefaultCameraManager>();
    }

    public void Update(GameTime gameTime)
    {
      if (this.AutoPause && (this.EngineState.Paused || !this.Camera.ActionRunning || !FezMath.IsOrthographic(this.Camera.Viewpoint) || this.CustomPause != null && this.CustomPause()) || !this.Alive)
        return;
      this.whileWaiting(gameTime.ElapsedGameTime, this.state);
      if (!this.condition(this.state))
        return;
      this.onValid();
      this.Kill();
    }

    public void Cancel()
    {
      if (!this.Alive)
        return;
      this.Kill();
    }

    private void Kill()
    {
      this.Alive = false;
      this.whileWaiting = (Action<TimeSpan, T>) null;
      this.onValid = (Action) null;
      this.condition = (Func<T, bool>) null;
      ServiceHelper.RemoveComponent<Waiter<T>>(this);
    }

    public void Initialize()
    {
    }
  }
}
