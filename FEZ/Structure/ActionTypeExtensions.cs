// Type: FezGame.Structure.ActionTypeExtensions
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Microsoft.Xna.Framework;
using System;

namespace FezGame.Structure
{
  public static class ActionTypeExtensions
  {
    public static bool IsAnimationLooping(this ActionType action)
    {
      switch (action)
      {
        case ActionType.DrumsIdle:
        case ActionType.StandWinking:
        case ActionType.PivotTombstone:
        case ActionType.LesserWarp:
        case ActionType.GateWarp:
        case ActionType.Idle:
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.FrontClimbingLadder:
        case ActionType.BackClimbingLadder:
        case ActionType.SideClimbingLadder:
        case ActionType.CarryIdle:
        case ActionType.CarryWalk:
        case ActionType.CarrySlide:
        case ActionType.CarryHeavyIdle:
        case ActionType.CarryHeavyWalk:
        case ActionType.CarryHeavySlide:
        case ActionType.Suffering:
        case ActionType.Sliding:
        case ActionType.ReadingSign:
        case ActionType.FreeFalling:
        case ActionType.Pushing:
        case ActionType.FrontClimbingVine:
        case ActionType.FrontClimbingVineSideways:
        case ActionType.SideClimbingVine:
        case ActionType.BackClimbingVine:
        case ActionType.BackClimbingVineSideways:
        case ActionType.Treading:
        case ActionType.Swimming:
        case ActionType.Sinking:
        case ActionType.Teetering:
        case ActionType.HurtSwim:
        case ActionType.EnteringTunnel:
        case ActionType.ShimmyFront:
        case ActionType.ShimmyBack:
          return true;
        default:
          return false;
      }
    }

    public static int GetStartFrame(this ActionType action)
    {
      switch (action)
      {
        case ActionType.Falling:
        case ActionType.Dropping:
          return 4;
        case ActionType.Bouncing:
        case ActionType.Flying:
          return 1;
        case ActionType.Landing:
          return 6;
        default:
          return 0;
      }
    }

    public static int GetEndFrame(this ActionType action)
    {
      switch (action)
      {
        case ActionType.Jumping:
        case ActionType.Flying:
        case ActionType.Floating:
          return 4;
        case ActionType.Falling:
        case ActionType.Dropping:
          return 5;
        default:
          return -1;
      }
    }

    public static Vector2 GetOffset(this ActionType action)
    {
      switch (action)
      {
        case ActionType.FrontClimbingLadder:
        case ActionType.BackClimbingLadder:
        case ActionType.SideClimbingLadder:
        case ActionType.FrontClimbingVine:
        case ActionType.BackClimbingVine:
          return new Vector2(0.0f, -2f);
        case ActionType.CarryIdle:
        case ActionType.CarryWalk:
        case ActionType.CarrySlide:
        case ActionType.EnterDoorSpinCarry:
        case ActionType.EnterTunnelCarry:
        case ActionType.ExitDoorCarry:
          return new Vector2(1f, -4f);
        case ActionType.CarryJump:
          return new Vector2(1f, -4f);
        case ActionType.CarryEnter:
          return new Vector2(0.0f, -4f);
        case ActionType.CarryHeavyIdle:
        case ActionType.CarryHeavyWalk:
        case ActionType.CarryHeavySlide:
        case ActionType.EnterDoorSpinCarryHeavy:
        case ActionType.EnterTunnelCarryHeavy:
          return new Vector2(0.0f, -7f);
        case ActionType.CarryHeavyJump:
          return new Vector2(-1f, -7f);
        case ActionType.CarryHeavyEnter:
          return new Vector2(1f, -7f);
        case ActionType.DropTrile:
        case ActionType.Lifting:
          return new Vector2(1f, 0.0f);
        case ActionType.DropHeavyTrile:
        case ActionType.LiftingHeavy:
          return new Vector2(0.0f, -3f);
        case ActionType.Throwing:
          return new Vector2(2f, 0.0f);
        case ActionType.ThrowingHeavy:
          return new Vector2(3f, -3f);
        case ActionType.Dying:
        case ActionType.WakingUp:
          return new Vector2(3f, 0.0f);
        case ActionType.Victory:
        case ActionType.VictoryForever:
          return new Vector2(1f, 0.0f);
        case ActionType.Pushing:
          return new Vector2(2f, 0.0f);
        case ActionType.SideClimbingVine:
          return new Vector2(3f, -2f);
        case ActionType.OpeningDoor:
        case ActionType.ExitDoor:
          return new Vector2(1f, 0.0f);
        case ActionType.Teetering:
          return new Vector2(-1f, -1f);
        case ActionType.IdlePlay:
          return new Vector2(-0.5f, 0.0f);
        case ActionType.IdleSleep:
        case ActionType.SleepWake:
          return new Vector2(1f, 0.0f);
        case ActionType.IdleLookAround:
          return new Vector2(-1f, 0.0f);
        case ActionType.PullUpCornerLedge:
          return new Vector2(2.5f, -1.5f);
        case ActionType.LowerToCornerLedge:
          return new Vector2(-2.5f, -1.5f);
        case ActionType.GrabCornerLedge:
          return new Vector2(-1f, -1.5f);
        case ActionType.GrabLedgeFront:
        case ActionType.GrabLedgeBack:
          return new Vector2(1f, -2.5f);
        case ActionType.PullUpFront:
        case ActionType.PullUpBack:
          return new Vector2(1f, -1.5f);
        case ActionType.LowerToLedge:
          return new Vector2(1f, -2.5f);
        case ActionType.ShimmyFront:
        case ActionType.ShimmyBack:
          return new Vector2(1f, -1.5f);
        case ActionType.ToCornerFront:
        case ActionType.ToCornerBack:
          return new Vector2(-2f, -1.5f);
        case ActionType.FromCornerBack:
          return new Vector2(-4f, -1.5f);
        case ActionType.ClimbOverLadder:
          return new Vector2(6f, 0.0f);
        case ActionType.ExitDoorCarryHeavy:
          return new Vector2(0.0f, -7f);
        case ActionType.CrushHorizontal:
          return new Vector2(-0.5f, 4f);
        case ActionType.DrumsIdle:
        case ActionType.DrumsCrash:
        case ActionType.DrumsTom:
        case ActionType.DrumsTom2:
        case ActionType.DrumsToss:
        case ActionType.DrumsTwirl:
        case ActionType.DrumsHiHat:
          return new Vector2(8f, 0.5f);
        default:
          return Vector2.Zero;
      }
    }

    public static string GetAnimationPath(this ActionType action)
    {
      switch (action)
      {
        case ActionType.Idle:
        case ActionType.ReadingSign:
        case ActionType.EnteringPipe:
        case ActionType.Standing:
        case ActionType.StandWinking:
          return "IdleWink";
        case ActionType.LookingLeft:
          return "LookLeft";
        case ActionType.LookingRight:
          return "LookRight";
        case ActionType.LookingUp:
          return "LookUp";
        case ActionType.LookingDown:
          return "LookDown";
        case ActionType.Walking:
        case ActionType.Sliding:
        case ActionType.CollectingFez:
        case ActionType.WalkingTo:
        case ActionType.EnteringTunnel:
        case ActionType.EnterDoorSpin:
          return "Walk";
        case ActionType.Running:
          return "Run";
        case ActionType.Jumping:
        case ActionType.Falling:
        case ActionType.Bouncing:
        case ActionType.Flying:
        case ActionType.Dropping:
        case ActionType.Landing:
        case ActionType.Floating:
          return "Jump";
        case ActionType.FrontClimbingLadder:
        case ActionType.FrontClimbingVine:
          return "ClimbFront";
        case ActionType.BackClimbingLadder:
        case ActionType.BackClimbingVine:
          return "ClimbRear";
        case ActionType.SideClimbingLadder:
        case ActionType.SideClimbingVine:
          return "ClimbSide";
        case ActionType.CarryIdle:
          return "CarryStand";
        case ActionType.CarryWalk:
        case ActionType.CarrySlide:
        case ActionType.EnterDoorSpinCarry:
        case ActionType.EnterTunnelCarry:
          return "CarryWalk";
        case ActionType.CarryJump:
          return "CarryJump";
        case ActionType.CarryEnter:
          return "CarryEnter";
        case ActionType.CarryHeavyIdle:
          return "CarryHeavyStand";
        case ActionType.CarryHeavyWalk:
        case ActionType.CarryHeavySlide:
        case ActionType.EnterDoorSpinCarryHeavy:
        case ActionType.EnterTunnelCarryHeavy:
          return "CarryHeavyWalk";
        case ActionType.CarryHeavyJump:
          return "CarryHeavyJump";
        case ActionType.CarryHeavyEnter:
          return "CarryHeavyEnter";
        case ActionType.DropTrile:
          return "DropTrile";
        case ActionType.DropHeavyTrile:
          return "DropHeavyTrile";
        case ActionType.Throwing:
          return "Throw";
        case ActionType.ThrowingHeavy:
          return "ThrowHeavy";
        case ActionType.Lifting:
          return "Lift";
        case ActionType.LiftingHeavy:
          return "LiftHeavy";
        case ActionType.Dying:
          return "Die";
        case ActionType.Suffering:
          return "Hurt";
        case ActionType.FreeFalling:
          return "AirPanic";
        case ActionType.Victory:
        case ActionType.FindingTreasure:
        case ActionType.VictoryForever:
          return "Victory";
        case ActionType.EnteringDoor:
          return "EnterDoor";
        case ActionType.Grabbing:
          return "Grab";
        case ActionType.Pushing:
          return "Push";
        case ActionType.SuckedIn:
          return "SuckedIn";
        case ActionType.FrontClimbingVineSideways:
          return "ClimbFrontSideways";
        case ActionType.BackClimbingVineSideways:
          return "ClimbBackSideways";
        case ActionType.WakingUp:
          return "WakeUp";
        case ActionType.OpeningTreasure:
          return "opentreasure";
        case ActionType.OpeningDoor:
          return "UnlockDoor";
        case ActionType.Treading:
          return "Tread";
        case ActionType.Swimming:
          return "Swim";
        case ActionType.Sinking:
        case ActionType.HurtSwim:
          return "Flail";
        case ActionType.Teetering:
          return "Teeter";
        case ActionType.PushingPivot:
          return "PushSpinFall";
        case ActionType.RunTurnAround:
          return "runswitch";
        case ActionType.IdlePlay:
          return "IdlePlay";
        case ActionType.IdleSleep:
        case ActionType.SleepWake:
          return "IdleSleep";
        case ActionType.IdleLookAround:
          return "IdleLookAround";
        case ActionType.PullUpCornerLedge:
          return "CornerLedgePullUp";
        case ActionType.LowerToCornerLedge:
          return "CornerLedgeLower";
        case ActionType.GrabCornerLedge:
          return "CornerLedgeGrab";
        case ActionType.GrabLedgeFront:
          return "LedgeGrabFront";
        case ActionType.GrabLedgeBack:
          return "LedgeGrabBack";
        case ActionType.PullUpFront:
          return "LedgePullUpFront";
        case ActionType.PullUpBack:
          return "LedgePullUpBack";
        case ActionType.LowerToLedge:
          return "LowerToLedge";
        case ActionType.ShimmyFront:
          return "LedgeShimmyFront";
        case ActionType.ShimmyBack:
          return "LedgeShimmyBack";
        case ActionType.ToCornerFront:
          return "FrontToCornerGrab";
        case ActionType.ToCornerBack:
          return "BackToCornerGrab";
        case ActionType.FromCornerBack:
          return "CornerToBackGrab";
        case ActionType.IdleToClimb:
          return "IdleToBackClimb";
        case ActionType.IdleToFrontClimb:
          return "IdleToFrontClimb";
        case ActionType.IdleToSideClimb:
          return "IdleToSideClimb";
        case ActionType.JumpToClimb:
          return "JumpToBackClimb";
        case ActionType.JumpToSideClimb:
          return "JumpToSideClimb";
        case ActionType.ClimbOverLadder:
          return "ClimbOverLadder";
        case ActionType.GrabTombstone:
        case ActionType.ReadTurnAround:
        case ActionType.TurnToBell:
          return "GrabTombstone";
        case ActionType.PivotTombstone:
          return "PivotTombstone";
        case ActionType.LetGoOfTombstone:
        case ActionType.EndReadTurnAround:
        case ActionType.TurnAwayFromBell:
          return "LetGoTombstone";
        case ActionType.ExitDoor:
          return "ExitDoor";
        case ActionType.ExitDoorCarry:
          return "CarryExit";
        case ActionType.ExitDoorCarryHeavy:
          return "CarryHeavyExit";
        case ActionType.LesserWarp:
        case ActionType.GateWarp:
          return "Warp";
        case ActionType.HitBell:
          return "HitBell";
        case ActionType.CrushHorizontal:
          return "CrushHorizontal";
        case ActionType.CrushVertical:
          return "CrushVertical";
        case ActionType.DrumsIdle:
          return "Drums/DrumsIdle";
        case ActionType.DrumsCrash:
          return "Drums/DrumsCrash";
        case ActionType.DrumsTom:
          return "Drums/DrumsTom";
        case ActionType.DrumsTom2:
          return "Drums/DrumsTom2";
        case ActionType.DrumsToss:
          return "Drums/DrumsToss";
        case ActionType.DrumsTwirl:
          return "Drums/DrumsTwirl";
        case ActionType.DrumsHiHat:
          return "Drums/DrumsHiHat";
        case ActionType.IdleYawn:
          return "IdleYawn";
        default:
          throw new InvalidOperationException("Action '" + (object) action + "' does not map to an animation.");
      }
    }

    public static bool HandlesZClamping(this ActionType type)
    {
      switch (type)
      {
        case ActionType.PullUpCornerLedge:
        case ActionType.GrabCornerLedge:
        case ActionType.GrabLedgeFront:
        case ActionType.GrabLedgeBack:
        case ActionType.ShimmyFront:
        case ActionType.EnteringPipe:
        case ActionType.EnteringDoor:
        case ActionType.EnteringTunnel:
        case ActionType.EnterDoorSpin:
        case ActionType.EnterDoorSpinCarry:
        case ActionType.EnterDoorSpinCarryHeavy:
        case ActionType.SideClimbingLadder:
        case ActionType.CarryEnter:
        case ActionType.CarryHeavyEnter:
        case ActionType.DropTrile:
        case ActionType.DropHeavyTrile:
        case ActionType.Lifting:
        case ActionType.LiftingHeavy:
          return true;
        default:
          return false;
      }
    }

    public static bool PreventsRotation(this ActionType type)
    {
      switch (type)
      {
        case ActionType.ShimmyFront:
        case ActionType.ShimmyBack:
        case ActionType.FreeFalling:
          return true;
        case ActionType.GrabTombstone:
        case ActionType.SideClimbingVine:
        case ActionType.GrabCornerLedge:
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
        case ActionType.SideClimbingLadder:
          return false;
        default:
          return !ActionTypeExtensions.AllowsLookingDirectionChange(type);
      }
    }

    public static bool NeedsAlwaysOnTop(this ActionType type)
    {
      switch (type)
      {
        case ActionType.Victory:
        case ActionType.SuckedIn:
        case ActionType.CrushHorizontal:
        case ActionType.CrushVertical:
          return true;
        default:
          return false;
      }
    }

    public static bool SkipSilhouette(this ActionType type)
    {
      switch (type)
      {
        case ActionType.OpeningTreasure:
        case ActionType.EnteringTunnel:
        case ActionType.EnterDoorSpin:
        case ActionType.EnterDoorSpinCarry:
        case ActionType.EnterDoorSpinCarryHeavy:
        case ActionType.EnteringPipe:
        case ActionType.LesserWarp:
        case ActionType.GateWarp:
        case ActionType.CrushHorizontal:
        case ActionType.CrushVertical:
        case ActionType.DrumsIdle:
        case ActionType.DrumsCrash:
        case ActionType.DrumsTom:
        case ActionType.DrumsTom2:
        case ActionType.DrumsToss:
        case ActionType.DrumsTwirl:
        case ActionType.DrumsHiHat:
        case ActionType.VictoryForever:
        case ActionType.CarryEnter:
        case ActionType.CarryHeavyEnter:
        case ActionType.EnteringDoor:
          return true;
        default:
          return false;
      }
    }

    public static bool PreventsFall(this ActionType type)
    {
      switch (type)
      {
        case ActionType.FreeFalling:
        case ActionType.WakingUp:
        case ActionType.Treading:
        case ActionType.Swimming:
        case ActionType.Sinking:
        case ActionType.HurtSwim:
        case ActionType.EnterDoorSpin:
        case ActionType.EnterDoorSpinCarry:
        case ActionType.EnterDoorSpinCarryHeavy:
        case ActionType.PivotTombstone:
        case ActionType.EnteringPipe:
        case ActionType.LesserWarp:
        case ActionType.GateWarp:
        case ActionType.CrushHorizontal:
        case ActionType.CrushVertical:
        case ActionType.DrumsIdle:
        case ActionType.DrumsCrash:
        case ActionType.DrumsTom:
        case ActionType.DrumsTom2:
        case ActionType.DrumsToss:
        case ActionType.DrumsTwirl:
        case ActionType.DrumsHiHat:
        case ActionType.Floating:
        case ActionType.Standing:
        case ActionType.StandWinking:
        case ActionType.Jumping:
        case ActionType.CarryHeavyEnter:
        case ActionType.Throwing:
        case ActionType.ThrowingHeavy:
        case ActionType.Dying:
        case ActionType.Suffering:
          return true;
        default:
          return false;
      }
    }

    public static bool AllowsLookingDirectionChange(this ActionType type)
    {
      switch (type)
      {
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
        case ActionType.SideClimbingLadder:
        case ActionType.CarryEnter:
        case ActionType.CarryHeavyEnter:
        case ActionType.DropTrile:
        case ActionType.DropHeavyTrile:
        case ActionType.Throwing:
        case ActionType.ThrowingHeavy:
        case ActionType.Lifting:
        case ActionType.LiftingHeavy:
        case ActionType.Dying:
        case ActionType.Suffering:
        case ActionType.ReadingSign:
        case ActionType.Victory:
        case ActionType.EnteringDoor:
        case ActionType.Grabbing:
        case ActionType.Pushing:
        case ActionType.SuckedIn:
        case ActionType.SideClimbingVine:
        case ActionType.WakingUp:
        case ActionType.OpeningTreasure:
        case ActionType.OpeningDoor:
        case ActionType.WalkingTo:
        case ActionType.Sinking:
        case ActionType.EnteringTunnel:
        case ActionType.PushingPivot:
        case ActionType.EnterDoorSpin:
        case ActionType.EnterDoorSpinCarry:
        case ActionType.EnterDoorSpinCarryHeavy:
        case ActionType.RunTurnAround:
        case ActionType.FindingTreasure:
        case ActionType.PullUpCornerLedge:
        case ActionType.LowerToCornerLedge:
        case ActionType.GrabCornerLedge:
        case ActionType.PullUpFront:
        case ActionType.PullUpBack:
        case ActionType.LowerToLedge:
        case ActionType.ToCornerFront:
        case ActionType.ToCornerBack:
        case ActionType.FromCornerBack:
        case ActionType.IdleToClimb:
        case ActionType.IdleToFrontClimb:
        case ActionType.IdleToSideClimb:
        case ActionType.JumpToClimb:
        case ActionType.JumpToSideClimb:
        case ActionType.ClimbOverLadder:
        case ActionType.GrabTombstone:
        case ActionType.PivotTombstone:
        case ActionType.LetGoOfTombstone:
        case ActionType.EnteringPipe:
        case ActionType.ExitDoor:
        case ActionType.ExitDoorCarry:
        case ActionType.ExitDoorCarryHeavy:
        case ActionType.LesserWarp:
        case ActionType.GateWarp:
        case ActionType.SleepWake:
        case ActionType.ReadTurnAround:
        case ActionType.EndReadTurnAround:
        case ActionType.TurnToBell:
        case ActionType.HitBell:
        case ActionType.TurnAwayFromBell:
        case ActionType.DrumsIdle:
        case ActionType.DrumsCrash:
        case ActionType.DrumsTom:
        case ActionType.DrumsTom2:
        case ActionType.DrumsToss:
        case ActionType.DrumsTwirl:
        case ActionType.DrumsHiHat:
        case ActionType.VictoryForever:
        case ActionType.Floating:
        case ActionType.Standing:
        case ActionType.StandWinking:
          return false;
        default:
          return true;
      }
    }

    public static bool DisallowsRespawn(this ActionType type)
    {
      switch (type)
      {
        case ActionType.Jumping:
        case ActionType.DropTrile:
        case ActionType.Throwing:
        case ActionType.ThrowingHeavy:
        case ActionType.Lifting:
        case ActionType.Dying:
        case ActionType.Suffering:
        case ActionType.Falling:
        case ActionType.Bouncing:
        case ActionType.FreeFalling:
        case ActionType.EnteringDoor:
        case ActionType.Grabbing:
        case ActionType.Pushing:
        case ActionType.SuckedIn:
        case ActionType.WakingUp:
        case ActionType.OpeningTreasure:
        case ActionType.WalkingTo:
        case ActionType.Treading:
        case ActionType.Swimming:
        case ActionType.Sinking:
        case ActionType.Teetering:
        case ActionType.HurtSwim:
        case ActionType.EnteringTunnel:
        case ActionType.PushingPivot:
        case ActionType.EnterDoorSpin:
        case ActionType.EnterDoorSpinCarry:
        case ActionType.EnterDoorSpinCarryHeavy:
        case ActionType.RunTurnAround:
        case ActionType.FindingTreasure:
        case ActionType.IdleToClimb:
        case ActionType.IdleToFrontClimb:
        case ActionType.IdleToSideClimb:
        case ActionType.JumpToClimb:
        case ActionType.JumpToSideClimb:
        case ActionType.EnteringPipe:
        case ActionType.ExitDoor:
        case ActionType.ExitDoorCarry:
        case ActionType.ExitDoorCarryHeavy:
        case ActionType.LesserWarp:
        case ActionType.GateWarp:
        case ActionType.SleepWake:
        case ActionType.ReadTurnAround:
        case ActionType.EndReadTurnAround:
        case ActionType.CrushHorizontal:
        case ActionType.CrushVertical:
        case ActionType.DrumsIdle:
        case ActionType.DrumsCrash:
        case ActionType.DrumsTom:
        case ActionType.DrumsTom2:
        case ActionType.DrumsToss:
        case ActionType.DrumsTwirl:
        case ActionType.DrumsHiHat:
        case ActionType.Floating:
        case ActionType.Standing:
        case ActionType.StandWinking:
          return true;
        default:
          return false;
      }
    }

    public static bool DefiesGravity(this ActionType type)
    {
      switch (type)
      {
        case ActionType.CarryHeavyEnter:
        case ActionType.CollectingFez:
        case ActionType.Victory:
        case ActionType.EnteringDoor:
        case ActionType.SuckedIn:
        case ActionType.FrontClimbingVine:
        case ActionType.FrontClimbingVineSideways:
        case ActionType.SideClimbingVine:
        case ActionType.BackClimbingVine:
        case ActionType.BackClimbingVineSideways:
        case ActionType.Sinking:
        case ActionType.FindingTreasure:
        case ActionType.PullUpCornerLedge:
        case ActionType.LowerToCornerLedge:
        case ActionType.GrabCornerLedge:
        case ActionType.GrabLedgeFront:
        case ActionType.GrabLedgeBack:
        case ActionType.PullUpFront:
        case ActionType.PullUpBack:
        case ActionType.LowerToLedge:
        case ActionType.ShimmyFront:
        case ActionType.ShimmyBack:
        case ActionType.ToCornerFront:
        case ActionType.ToCornerBack:
        case ActionType.FromCornerBack:
        case ActionType.IdleToClimb:
        case ActionType.IdleToFrontClimb:
        case ActionType.IdleToSideClimb:
        case ActionType.JumpToClimb:
        case ActionType.JumpToSideClimb:
        case ActionType.ClimbOverLadder:
        case ActionType.PivotTombstone:
        case ActionType.EnteringPipe:
        case ActionType.LesserWarp:
        case ActionType.GateWarp:
        case ActionType.CrushHorizontal:
        case ActionType.CrushVertical:
        case ActionType.DrumsIdle:
        case ActionType.DrumsCrash:
        case ActionType.DrumsTom:
        case ActionType.DrumsTom2:
        case ActionType.DrumsToss:
        case ActionType.DrumsTwirl:
        case ActionType.DrumsHiHat:
        case ActionType.VictoryForever:
        case ActionType.Floating:
        case ActionType.Standing:
        case ActionType.StandWinking:
        case ActionType.FrontClimbingLadder:
        case ActionType.BackClimbingLadder:
        case ActionType.SideClimbingLadder:
        case ActionType.CarryEnter:
          return true;
        default:
          return false;
      }
    }

    public static bool NoBackgroundDarkening(this ActionType type)
    {
      switch (type)
      {
        case ActionType.PullUpCornerLedge:
        case ActionType.LowerToCornerLedge:
        case ActionType.GrabCornerLedge:
        case ActionType.GrabLedgeFront:
        case ActionType.GrabLedgeBack:
        case ActionType.PullUpFront:
        case ActionType.PullUpBack:
        case ActionType.LowerToLedge:
        case ActionType.ShimmyFront:
        case ActionType.ShimmyBack:
          return true;
        default:
          return false;
      }
    }

    public static bool IsEnteringDoor(this ActionType type)
    {
      switch (type)
      {
        case ActionType.EnteringDoor:
        case ActionType.EnterDoorSpin:
        case ActionType.EnterDoorSpinCarry:
        case ActionType.EnterDoorSpinCarryHeavy:
        case ActionType.CarryEnter:
        case ActionType.CarryHeavyEnter:
          return true;
        default:
          return false;
      }
    }

    public static bool IsClimbingLadder(this ActionType type)
    {
      switch (type)
      {
        case ActionType.FrontClimbingLadder:
        case ActionType.BackClimbingLadder:
        case ActionType.SideClimbingLadder:
          return true;
        default:
          return false;
      }
    }

    public static bool IsClimbingVine(this ActionType type)
    {
      switch (type)
      {
        case ActionType.FrontClimbingVine:
        case ActionType.FrontClimbingVineSideways:
        case ActionType.SideClimbingVine:
        case ActionType.BackClimbingVine:
        case ActionType.BackClimbingVineSideways:
          return true;
        default:
          return false;
      }
    }

    public static bool IsSwimming(this ActionType type)
    {
      switch (type)
      {
        case ActionType.Treading:
        case ActionType.Swimming:
        case ActionType.HurtSwim:
          return true;
        default:
          return false;
      }
    }

    public static bool IsCarry(this ActionType type)
    {
      switch (type)
      {
        case ActionType.CarryIdle:
        case ActionType.CarryWalk:
        case ActionType.CarryJump:
        case ActionType.CarrySlide:
        case ActionType.CarryEnter:
        case ActionType.CarryHeavyIdle:
        case ActionType.CarryHeavyWalk:
        case ActionType.CarryHeavyJump:
        case ActionType.CarryHeavySlide:
        case ActionType.CarryHeavyEnter:
          return true;
        default:
          return false;
      }
    }

    public static bool IsIdle(this ActionType type)
    {
      switch (type)
      {
        case ActionType.IdlePlay:
        case ActionType.IdleSleep:
        case ActionType.IdleLookAround:
        case ActionType.IdleYawn:
        case ActionType.Idle:
        case ActionType.Teetering:
          return true;
        default:
          return false;
      }
    }

    public static bool IsOnLedge(this ActionType type)
    {
      switch (type)
      {
        case ActionType.PullUpCornerLedge:
        case ActionType.LowerToCornerLedge:
        case ActionType.GrabCornerLedge:
        case ActionType.GrabLedgeFront:
        case ActionType.GrabLedgeBack:
        case ActionType.PullUpFront:
        case ActionType.PullUpBack:
        case ActionType.LowerToLedge:
        case ActionType.ShimmyFront:
        case ActionType.ShimmyBack:
        case ActionType.ToCornerFront:
        case ActionType.ToCornerBack:
        case ActionType.FromCornerBack:
          return true;
        default:
          return false;
      }
    }

    public static bool FacesBack(this ActionType type)
    {
      switch (type)
      {
        case ActionType.BackClimbingLadder:
        case ActionType.BackClimbingVine:
        case ActionType.BackClimbingVineSideways:
        case ActionType.GrabLedgeBack:
        case ActionType.PullUpBack:
        case ActionType.LowerToLedge:
        case ActionType.ShimmyBack:
          return true;
        default:
          return false;
      }
    }

    public static bool IsLookingAround(this ActionType type)
    {
      switch (type)
      {
        case ActionType.LookingLeft:
        case ActionType.LookingRight:
        case ActionType.LookingUp:
        case ActionType.LookingDown:
          return true;
        default:
          return false;
      }
    }

    public static bool IsPlayingDrums(this ActionType type)
    {
      switch (type)
      {
        case ActionType.DrumsIdle:
        case ActionType.DrumsCrash:
        case ActionType.DrumsTom:
        case ActionType.DrumsTom2:
        case ActionType.DrumsToss:
        case ActionType.DrumsTwirl:
        case ActionType.DrumsHiHat:
          return true;
        default:
          return false;
      }
    }
  }
}
