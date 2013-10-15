// Type: Microsoft.Xna.Framework.GameTime
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
