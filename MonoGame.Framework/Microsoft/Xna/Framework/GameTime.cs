// Type: Microsoft.Xna.Framework.GameTime
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework
{
  public class GameTime
  {
    public TimeSpan TotalGameTime { get; set; }

    public TimeSpan ElapsedGameTime { get; set; }

    public bool IsRunningSlowly { get; set; }

    public GameTime()
    {
      this.TotalGameTime = TimeSpan.Zero;
      this.ElapsedGameTime = TimeSpan.Zero;
      this.IsRunningSlowly = false;
    }

    public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
    {
      this.TotalGameTime = totalGameTime;
      this.ElapsedGameTime = elapsedGameTime;
      this.IsRunningSlowly = false;
    }

    public GameTime(TimeSpan totalRealTime, TimeSpan elapsedRealTime, bool isRunningSlowly)
    {
      this.TotalGameTime = totalRealTime;
      this.ElapsedGameTime = elapsedRealTime;
      this.IsRunningSlowly = isRunningSlowly;
    }
  }
}
