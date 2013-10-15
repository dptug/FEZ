// Type: FezEngine.Components.Waiter
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
  internal class Waiter : IGameComponent, IUpdateable, IWaiter
  {
    private readonly IEngineStateManager EngineState;
    private readonly IDefaultCameraManager Camera;
    private Func<bool> condition;
    private Action<TimeSpan> whileWaiting;
    private Action onValid;
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

    internal Waiter(Func<bool> condition, Action onValid)
      : this(condition, new Action<TimeSpan>(Util.NullAction<TimeSpan>), onValid)
    {
    }

    internal Waiter(Func<bool> condition, Action<TimeSpan> whileWaiting)
      : this(condition, whileWaiting, new Action(Util.NullAction))
    {
    }

    internal Waiter(Func<bool> condition, Action<TimeSpan> whileWaiting, Action onValid)
    {
      this.condition = condition;
      this.whileWaiting = whileWaiting;
      this.onValid = onValid;
      this.Alive = true;
      this.EngineState = ServiceHelper.Get<IEngineStateManager>();
      this.Camera = ServiceHelper.Get<IDefaultCameraManager>();
    }

    public void Update(GameTime gameTime)
    {
      if (this.AutoPause && (this.EngineState.Paused || !this.Camera.ActionRunning || !FezMath.IsOrthographic(this.Camera.Viewpoint) || this.CustomPause != null && this.CustomPause()) || !this.Alive)
        return;
      if (this.condition())
      {
        this.onValid();
        this.Kill();
      }
      else
        this.whileWaiting(gameTime.ElapsedGameTime);
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
      this.condition = (Func<bool>) null;
      this.onValid = (Action) null;
      this.whileWaiting = (Action<TimeSpan>) null;
      ServiceHelper.RemoveComponent<Waiter>(this);
    }

    public void Initialize()
    {
    }
  }
}
