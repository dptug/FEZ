// Type: FezGame.Components.Actions.Crush
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  public class Crush : PlayerAction
  {
    private const float Duration = 1.75f;
    private float AccumlatedTime;
    private Vector3 crushPosition;

    public Crush(Game game)
      : base(game)
    {
    }

    protected override void Begin()
    {
      this.PlayerManager.Velocity = Vector3.Zero;
      this.crushPosition = this.PlayerManager.Position;
      this.AccumlatedTime = 0.0f;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      this.AccumlatedTime += (float) elapsed.TotalSeconds;
      this.PlayerManager.Position = this.crushPosition;
      this.PlayerManager.Animation.Timing.Update(elapsed, 2f);
      if ((double) this.AccumlatedTime > 1.75 * (this.PlayerManager.Action == ActionType.CrushHorizontal ? 1.20000004768372 : 1.0))
        this.PlayerManager.Respawn();
      return false;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.CrushVertical)
        return type == ActionType.CrushHorizontal;
      else
        return true;
    }
  }
}
