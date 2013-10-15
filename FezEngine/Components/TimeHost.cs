// Type: FezEngine.Components.TimeHost
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Components
{
  public class TimeHost : GameComponent
  {
    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    public TimeHost(Game game)
      : base(game)
    {
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.TimePaused || this.EngineState.Loading)
        return;
      this.TimeManager.CurrentTime += TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds * (double) this.TimeManager.TimeFactor);
      this.TimeManager.OnTick();
    }
  }
}
