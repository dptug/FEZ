// Type: FezEngine.Components.FpsMeasurer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;

namespace FezEngine.Components
{
  public class FpsMeasurer : DrawableGameComponent
  {
    private double accumulatedTime;
    private int framesCounter;

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    public FpsMeasurer(Game game)
      : base(game)
    {
    }

    public override void Draw(GameTime gameTime)
    {
      this.accumulatedTime += gameTime.ElapsedGameTime.TotalSeconds;
      ++this.framesCounter;
      if (this.accumulatedTime < 1.0)
        return;
      this.EngineState.FramesPerSecond = (float) this.framesCounter / (float) this.accumulatedTime;
      --this.accumulatedTime;
      this.framesCounter = 0;
    }
  }
}
