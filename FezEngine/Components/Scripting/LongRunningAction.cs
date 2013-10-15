// Type: FezEngine.Components.Scripting.LongRunningAction
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Components.Scripting
{
  public class LongRunningAction : GameComponent
  {
    public Func<float, float, bool> OnUpdate;
    public Action OnDispose;
    public Action Ended;
    private bool disposed;
    private float sinceStarted;

    [ServiceDependency]
    public IEngineStateManager EngineState { protected get; set; }

    public LongRunningAction()
      : this(new Func<float, float, bool>(Util.NullFunc<float, float, bool>))
    {
    }

    public LongRunningAction(Action onDispose)
      : this(new Func<float, float, bool>(Util.NullFunc<float, float, bool>), onDispose)
    {
    }

    public LongRunningAction(Func<float, float, bool> onUpdate)
      : this(onUpdate, new Action(Util.NullAction))
    {
    }

    public LongRunningAction(Func<float, float, bool> onUpdate, Action onDispose)
      : base(ServiceHelper.Game)
    {
      ServiceHelper.AddComponent((IGameComponent) this);
      this.OnDispose = onDispose;
      this.OnUpdate = onUpdate;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Paused || this.EngineState.InMap || (this.EngineState.Loading || this.disposed))
        return;
      this.sinceStarted += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (!this.OnUpdate((float) gameTime.ElapsedGameTime.TotalSeconds, this.sinceStarted))
        return;
      ServiceHelper.RemoveComponent<LongRunningAction>(this);
    }

    protected override void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
      base.Dispose(disposing);
      if (this.OnDispose != null)
        this.OnDispose();
      if (this.Ended != null)
        this.Ended();
      this.OnUpdate = (Func<float, float, bool>) null;
      this.OnDispose = (Action) null;
      this.Ended = (Action) null;
    }
  }
}
