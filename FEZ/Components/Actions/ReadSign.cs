// Type: FezGame.Components.Actions.ReadSign
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  public class ReadSign : PlayerAction
  {
    private string signText;
    private SoundEffect sTextNext;

    [ServiceDependency]
    public ISpeechBubbleManager SpeechBubble { private get; set; }

    public ReadSign(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.sTextNext = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/TextNext");
    }

    protected override void TestConditions()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.Teetering:
        case ActionType.IdlePlay:
        case ActionType.IdleSleep:
        case ActionType.IdleLookAround:
        case ActionType.IdleYawn:
        case ActionType.Idle:
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Sliding:
        case ActionType.Landing:
          if (!this.IsOnSign() || this.InputManager.CancelTalk != FezButtonState.Pressed)
            break;
          this.SpeechBubble.Origin = this.PlayerManager.Position;
          this.SpeechBubble.ChangeText(GameText.GetString(this.signText));
          this.PlayerManager.Action = ActionType.ReadingSign;
          this.InputManager.PressedToDown();
          break;
      }
    }

    protected override void Begin()
    {
      base.Begin();
      this.GomezService.OnReadSign();
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity * Vector3.UnitY;
      playerManager.Velocity = vector3;
    }

    private bool IsOnSign()
    {
      if (!this.TestSignCollision(VerticalDirection.Up))
        return this.TestSignCollision(VerticalDirection.Down);
      else
        return true;
    }

    private bool TestSignCollision(VerticalDirection direction)
    {
      TrileInstance trileInstance = this.PlayerManager.AxisCollision[direction].Surface;
      if (trileInstance == null)
        return false;
      Trile trile = trileInstance.Trile;
      FaceOrientation faceOrientation = FezMath.OrientationFromPhi(FezMath.ToPhi(trile.ActorSettings.Face) + trileInstance.Phi);
      bool flag = trile.ActorSettings.Type == ActorType.Sign && faceOrientation == this.CameraManager.VisibleOrientation && trileInstance.ActorSettings != (InstanceActorSettings) null && !string.IsNullOrEmpty(trileInstance.ActorSettings.SignText);
      if (flag)
        this.signText = trileInstance.ActorSettings.SignText;
      return flag;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.InputManager.CancelTalk == FezButtonState.Pressed)
      {
        SoundEffectExtensions.Emit(this.sTextNext);
        this.SpeechBubble.Hide();
        this.PlayerManager.Action = ActionType.Idle;
        this.InputManager.PressedToDown();
      }
      return true;
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.ReadingSign;
    }
  }
}
