// Type: FezEngine.Services.GamepadsManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using FezEngine.Structure.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezEngine.Services
{
  public class GamepadsManager : GameComponent, IGamepadsManager
  {
    private readonly Dictionary<PlayerIndex, GamepadState> gamepadStates = new Dictionary<PlayerIndex, GamepadState>((IEqualityComparer<PlayerIndex>) PlayerIndexComparer.Default);

    public GamepadState this[PlayerIndex index]
    {
      get
      {
        return this.gamepadStates[index];
      }
    }

    public GamepadsManager(Game game, bool enabled = true)
      : base(game)
    {
      this.Enabled = enabled;
      if (!this.Enabled)
        return;
      this.gamepadStates.Add(PlayerIndex.One, new GamepadState(PlayerIndex.One));
      this.gamepadStates.Add(PlayerIndex.Two, new GamepadState(PlayerIndex.Two));
      this.gamepadStates.Add(PlayerIndex.Three, new GamepadState(PlayerIndex.Three));
      this.gamepadStates.Add(PlayerIndex.Four, new GamepadState(PlayerIndex.Four));
    }

    public override void Update(GameTime gameTime)
    {
      TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
      bool flag1 = false;
      this.gamepadStates[PlayerIndex.One].Update(elapsedGameTime);
      bool flag2 = flag1 | this.gamepadStates[PlayerIndex.One].XInputConnected;
      this.gamepadStates[PlayerIndex.Two].Update(elapsedGameTime);
      bool flag3 = flag2 | this.gamepadStates[PlayerIndex.Two].XInputConnected;
      this.gamepadStates[PlayerIndex.Three].Update(elapsedGameTime);
      bool flag4 = flag3 | this.gamepadStates[PlayerIndex.Three].XInputConnected;
      this.gamepadStates[PlayerIndex.Four].Update(elapsedGameTime);
      GamepadState.AnyXInputConnected = flag4 | this.gamepadStates[PlayerIndex.Four].XInputConnected;
      GamepadState.AnyConnected = this.gamepadStates[PlayerIndex.One].Connected || this.gamepadStates[PlayerIndex.Two].Connected || this.gamepadStates[PlayerIndex.Three].Connected || this.gamepadStates[PlayerIndex.Four].Connected;
    }
  }
}
