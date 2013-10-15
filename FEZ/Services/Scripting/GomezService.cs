// Type: FezGame.Services.Scripting.GomezService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Services.Scripting;
using FezEngine.Tools;
using FezGame.Components.Scripting;
using FezGame.Services;
using FezGame.Structure;
using System;

namespace FezGame.Services.Scripting
{
  public class GomezService : IGomezService, IScriptingBase
  {
    public int CollectedCubes
    {
      get
      {
        return this.GameState.SaveData.CubeShards;
      }
    }

    public int CollectedSplits
    {
      get
      {
        return this.GameState.SaveData.CollectedParts;
      }
    }

    public bool Grounded
    {
      get
      {
        return this.PlayerManager.Grounded;
      }
    }

    public bool IsOnLadder
    {
      get
      {
        return ActionTypeExtensions.IsClimbingLadder(this.PlayerManager.Action);
      }
    }

    public bool CanControl
    {
      get
      {
        return this.PlayerManager.CanControl;
      }
    }

    public bool Visible
    {
      get
      {
        return !this.PlayerManager.Hidden;
      }
    }

    public bool Alive
    {
      get
      {
        if (this.PlayerManager.Action != ActionType.Dying && this.PlayerManager.Action != ActionType.FreeFalling && this.PlayerManager.Action != ActionType.SuckedIn)
          return this.PlayerManager.Action != ActionType.Sinking;
        else
          return false;
      }
    }

    [ServiceDependency]
    internal IScriptingManager ScriptingManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    public event Action EnteredDoor = new Action(Util.NullAction);

    public event Action Jumped = new Action(Util.NullAction);

    public event Action ClimbedLadder = new Action(Util.NullAction);

    public event Action ClimbedVine = new Action(Util.NullAction);

    public event Action LookedAround = new Action(Util.NullAction);

    public event Action LiftedObject = new Action(Util.NullAction);

    public event Action ThrewObject = new Action(Util.NullAction);

    public event Action OpenedMenuCube = new Action(Util.NullAction);

    public event Action ReadSign = new Action(Util.NullAction);

    public event Action GrabbedLedge = new Action(Util.NullAction);

    public event Action DropObject = new Action(Util.NullAction);

    public event Action DroppedLedge = new Action(Util.NullAction);

    public event Action Hoisted = new Action(Util.NullAction);

    public event Action ClimbedOverLadder = new Action(Util.NullAction);

    public event Action DroppedFromLadder = new Action(Util.NullAction);

    public event Action ReadMail = new Action(Util.NullAction);

    public event Action CollectedSplitUpCube = new Action(Util.NullAction);

    public event Action CollectedShard = new Action(Util.NullAction);

    public event Action OpenedTreasure = new Action(Util.NullAction);

    public event Action CollectedAnti = new Action(Util.NullAction);

    public event Action CollectedPieceOfHeart = new Action(Util.NullAction);

    public event Action Landed = new Action(Util.NullAction);

    public void OnEnterDoor()
    {
      this.EnteredDoor();
    }

    public void OnJump()
    {
      this.Jumped();
    }

    public void OnClimbLadder()
    {
      this.ClimbedLadder();
    }

    public void OnClimbVine()
    {
      this.ClimbedVine();
    }

    public void OnLookAround()
    {
      this.LookedAround();
    }

    public void OnLiftObject()
    {
      this.LiftedObject();
    }

    public void OnThrowObject()
    {
      this.ThrewObject();
    }

    public void OnDropObject()
    {
      this.DropObject();
    }

    public void OnOpenMenuCube()
    {
      this.OpenedMenuCube();
    }

    public void OnReadSign()
    {
      this.ReadSign();
    }

    public void OnGrabLedge()
    {
      this.GrabbedLedge();
    }

    public void OnHoist()
    {
      this.Hoisted();
    }

    public void OnDropLedge()
    {
      this.DroppedLedge();
    }

    public void OnClimbOverLadder()
    {
      this.ClimbedOverLadder();
    }

    public void OnDropFromLadder()
    {
      this.DroppedFromLadder();
    }

    public void OnReadMail()
    {
      this.ReadMail();
    }

    public void OnCollectedSplitUpCube()
    {
      this.CollectedSplitUpCube();
    }

    public void OnCollectedShard()
    {
      this.CollectedShard();
    }

    public void OnOpenTreasure()
    {
      this.OpenedTreasure();
    }

    public void OnCollectedPieceOfHeart()
    {
      this.CollectedPieceOfHeart();
    }

    public void OnCollectedAnti()
    {
      this.CollectedAnti();
    }

    public void OnLand()
    {
      this.Landed();
    }

    public void SetCanControl(bool controllable)
    {
      this.PlayerManager.CanControl = controllable;
    }

    public void SetAction(string actionName)
    {
      this.PlayerManager.Action = (ActionType) Enum.Parse(typeof (ActionType), actionName, false);
    }

    public void SetFezVisible(bool visible)
    {
      this.PlayerManager.HideFez = !visible;
    }

    public void SetGomezVisible(bool visible)
    {
      this.PlayerManager.Hidden = !visible;
    }

    public LongRunningAction AllowEnterTunnel()
    {
      this.PlayerManager.TunnelVolume = this.ScriptingManager.EvaluatedScript.InitiatingTrigger.Object.Identifier;
      return new LongRunningAction((Action) (() => this.PlayerManager.TunnelVolume = new int?()));
    }

    public void ResetEvents()
    {
      this.EnteredDoor = new Action(Util.NullAction);
      this.Jumped = new Action(Util.NullAction);
      this.ClimbedLadder = new Action(Util.NullAction);
      this.ClimbedVine = new Action(Util.NullAction);
      this.LookedAround = new Action(Util.NullAction);
      this.LiftedObject = new Action(Util.NullAction);
      this.ThrewObject = new Action(Util.NullAction);
      this.OpenedMenuCube = new Action(Util.NullAction);
      this.ReadSign = new Action(Util.NullAction);
      this.GrabbedLedge = new Action(Util.NullAction);
      this.DropObject = new Action(Util.NullAction);
      this.DroppedLedge = new Action(Util.NullAction);
      this.Hoisted = new Action(Util.NullAction);
      this.ClimbedOverLadder = new Action(Util.NullAction);
      this.DroppedFromLadder = new Action(Util.NullAction);
      this.ReadMail = new Action(Util.NullAction);
      this.CollectedSplitUpCube = new Action(Util.NullAction);
      this.CollectedShard = new Action(Util.NullAction);
      this.OpenedTreasure = new Action(Util.NullAction);
      this.CollectedAnti = new Action(Util.NullAction);
      this.CollectedPieceOfHeart = new Action(Util.NullAction);
      this.Landed = new Action(Util.NullAction);
    }
  }
}
