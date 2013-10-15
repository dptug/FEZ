// Type: FezEngine.Services.Scripting.IGomezService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Static = true)]
  public interface IGomezService : IScriptingBase
  {
    [Description("The number of small golden cubes the player's collected")]
    int CollectedCubes { get; }

    [Description("Is he standing on solid ground?")]
    bool Grounded { get; }

    [Description("Is Gomez controllable by the player?")]
    bool CanControl { get; }

    [Description("Is Gomez visible?")]
    bool Visible { get; }

    bool IsOnLadder { get; }

    bool Alive { get; }

    int CollectedSplits { get; }

    event Action EnteredDoor;

    event Action Jumped;

    event Action ClimbedLadder;

    event Action ClimbedVine;

    event Action LookedAround;

    event Action LiftedObject;

    event Action ThrewObject;

    event Action OpenedMenuCube;

    event Action ReadSign;

    event Action GrabbedLedge;

    event Action DropObject;

    event Action DroppedLedge;

    event Action Hoisted;

    event Action ClimbedOverLadder;

    event Action DroppedFromLadder;

    event Action ReadMail;

    event Action CollectedSplitUpCube;

    event Action CollectedShard;

    event Action CollectedAnti;

    event Action CollectedPieceOfHeart;

    event Action OpenedTreasure;

    event Action Landed;

    void OnEnterDoor();

    void OnJump();

    void OnClimbLadder();

    void OnClimbVine();

    void OnLookAround();

    void OnLiftObject();

    void OnThrowObject();

    void OnOpenMenuCube();

    void OnReadSign();

    void OnGrabLedge();

    void OnDropObject();

    void OnHoist();

    void OnDropLedge();

    void OnClimbOverLadder();

    void OnDropFromLadder();

    void OnReadMail();

    void OnCollectedSplitUpCube();

    void OnCollectedShard();

    void OnCollectedAnti();

    void OnCollectedPieceOfHeart();

    void OnOpenTreasure();

    void OnLand();

    [Description("Sets whether Gomez can be controlled by the player")]
    void SetCanControl(bool controllable);

    [Description("Sets the current action (animation) for Gomez")]
    void SetAction(string actionName);

    [Description("Allows Gomez to enter that tunnel/passageway by pressing up")]
    LongRunningAction AllowEnterTunnel();

    [Description("Shows/Hides gomez's fez")]
    void SetFezVisible(bool visible);

    [Description("Shows/Hides gomez")]
    void SetGomezVisible(bool visible);
  }
}
