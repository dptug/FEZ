// Type: FezGame.Components.PointsOfInterestHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class PointsOfInterestHost : GameComponent
  {
    private Volume[] PointsList;
    private bool InGroup;
    private SoundEffect sDotTalk;
    private SoundEmitter eDotTalk;
    private IWaiter talkWaiter;

    [ServiceDependency]
    public IDotManager Dot { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public ISpeechBubbleManager SpeechBubble { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public PointsOfInterestHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.sDotTalk = this.CMProvider.Global.Load<SoundEffect>("Sounds/Dot/Talk");
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.Dot.Reset();
      this.PointsList = Enumerable.ToArray<Volume>(Enumerable.Where<Volume>((IEnumerable<Volume>) this.LevelManager.Volumes.Values, (Func<Volume, bool>) (x =>
      {
        if (x.ActorSettings != null && x.Enabled)
          return x.ActorSettings.IsPointOfInterest;
        else
          return false;
      })));
      this.SyncTutorials();
    }

    private void SyncTutorials()
    {
      foreach (Volume volume in this.PointsList)
      {
        foreach (DotDialogueLine dotDialogueLine in volume.ActorSettings.DotDialogue)
        {
          bool flag;
          if (this.GameState.SaveData.OneTimeTutorials.TryGetValue(dotDialogueLine.ResourceText, out flag) && flag)
          {
            volume.ActorSettings.PreventHey = true;
            break;
          }
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.InMap || (this.PointsList.Length == 0 || this.Dot.PreventPoI) || (this.GameState.Paused || this.GameState.InMenuCube || this.Dot.Owner != null && this.Dot.Owner != this) || (this.GameState.FarawaySettings.InTransition || ActionTypeExtensions.IsEnteringDoor(this.PlayerManager.Action)))
        return;
      Vector3 vector3 = FezMath.DepthMask(this.CameraManager.Viewpoint);
      BoundingBox boundingBox1 = new BoundingBox(this.PlayerManager.Center - new Vector3(6f), this.PlayerManager.Center + new Vector3(6f));
      Volume volume1 = (Volume) null;
      foreach (Volume volume2 in this.PointsList)
      {
        if (volume2.Enabled)
        {
          BoundingBox boundingBox2 = volume2.BoundingBox;
          boundingBox2.Min -= vector3 * 1000f;
          boundingBox2.Max += vector3 * 1000f;
          if (boundingBox1.Contains(boundingBox2) != ContainmentType.Disjoint)
          {
            this.Dot.ComeOut();
            this.Dot.Behaviour = DotHost.BehaviourType.RoamInVolume;
            this.Dot.RoamingVolume = volume1 = volume2;
          }
          if (this.SpeechBubble.Hidden)
          {
            if (this.talkWaiter != null && this.talkWaiter.Alive)
              this.talkWaiter.Cancel();
            if (this.eDotTalk != null && !this.eDotTalk.Dead)
              this.eDotTalk.FadeOutAndPause(0.1f);
          }
          if (!this.SpeechBubble.Hidden && this.PlayerManager.CurrentVolumes.Contains(volume2) && volume2.ActorSettings.DotDialogue.Count > 0 && (this.PlayerManager.Action == ActionType.Suffering || this.PlayerManager.Action == ActionType.SuckedIn || (this.PlayerManager.Action == ActionType.LesserWarp || this.PlayerManager.Action == ActionType.GateWarp)))
          {
            this.SpeechBubble.Hide();
            this.Dot.Behaviour = DotHost.BehaviourType.ReadyToTalk;
            this.InGroup = false;
          }
          if (!this.GameState.InFpsMode && this.PlayerManager.CurrentVolumes.Contains(volume2) && volume2.ActorSettings.DotDialogue.Count > 0)
          {
            if (this.SpeechBubble.Hidden && (this.InputManager.CancelTalk == FezButtonState.Pressed || this.InGroup))
            {
              switch (this.PlayerManager.Action)
              {
                case ActionType.Sliding:
                case ActionType.Landing:
                case ActionType.IdlePlay:
                case ActionType.IdleSleep:
                case ActionType.IdleLookAround:
                case ActionType.IdleYawn:
                case ActionType.Idle:
                case ActionType.Walking:
                case ActionType.Running:
                  volume2.ActorSettings.NextLine = (volume2.ActorSettings.NextLine + 1) % volume2.ActorSettings.DotDialogue.Count;
                  int index1 = (volume2.ActorSettings.NextLine + 1) % volume2.ActorSettings.DotDialogue.Count;
                  this.InGroup = volume2.ActorSettings.DotDialogue[volume2.ActorSettings.NextLine].Grouped && volume2.ActorSettings.DotDialogue[index1].Grouped && index1 != 0;
                  volume2.ActorSettings.PreventHey = true;
                  string index2 = volume2.ActorSettings.DotDialogue[volume2.ActorSettings.NextLine].ResourceText;
                  bool flag;
                  if (this.GameState.SaveData.OneTimeTutorials.TryGetValue(index2, out flag) && !flag)
                  {
                    this.GameState.SaveData.OneTimeTutorials[index2] = true;
                    this.SyncTutorials();
                  }
                  if (this.talkWaiter != null && this.talkWaiter.Alive)
                    this.talkWaiter.Cancel();
                  string @string = GameText.GetString(index2);
                  this.SpeechBubble.ChangeText(@string);
                  this.PlayerManager.Action = ActionType.ReadingSign;
                  if (this.eDotTalk == null || this.eDotTalk.Dead)
                  {
                    this.eDotTalk = SoundEffectExtensions.EmitAt(this.sDotTalk, this.Dot.Position, true);
                  }
                  else
                  {
                    this.eDotTalk.Position = this.Dot.Position;
                    Waiters.Wait(0.100000001490116, (Action) (() =>
                    {
                      this.eDotTalk.Cue.Resume();
                      this.eDotTalk.VolumeFactor = 1f;
                    })).AutoPause = true;
                  }
                  this.talkWaiter = Waiters.Wait(0.100000001490116 + 0.0750000029802322 * (double) Util.StripPunctuation(@string).Length * (Culture.IsCJK ? 2.0 : 1.0), (Action) (() =>
                  {
                    if (this.eDotTalk == null)
                      return;
                    this.eDotTalk.FadeOutAndPause(0.1f);
                  }));
                  this.talkWaiter.AutoPause = true;
                  break;
              }
            }
            if (this.SpeechBubble.Hidden && !volume2.ActorSettings.PreventHey)
            {
              if (this.PlayerManager.Grounded)
              {
                switch (this.PlayerManager.Action)
                {
                  case ActionType.Sliding:
                  case ActionType.Landing:
                  case ActionType.IdlePlay:
                  case ActionType.IdleSleep:
                  case ActionType.IdleLookAround:
                  case ActionType.IdleYawn:
                  case ActionType.Idle:
                  case ActionType.Walking:
                  case ActionType.Running:
                    this.Dot.Behaviour = DotHost.BehaviourType.ThoughtBubble;
                    this.Dot.FaceButton = DotFaceButton.B;
                    if (this.Dot.Owner != this)
                      this.Dot.Hey();
                    this.Dot.Owner = (object) this;
                    break;
                  default:
                    this.Dot.Behaviour = DotHost.BehaviourType.ReadyToTalk;
                    break;
                }
              }
            }
            else
              this.Dot.Behaviour = DotHost.BehaviourType.ReadyToTalk;
            if (!this.SpeechBubble.Hidden)
              this.SpeechBubble.Origin = this.Dot.Position;
            if (this.Dot.Behaviour == DotHost.BehaviourType.ReadyToTalk || this.Dot.Behaviour == DotHost.BehaviourType.ThoughtBubble)
              break;
          }
        }
      }
      if (this.Dot.Behaviour != DotHost.BehaviourType.ThoughtBubble && this.Dot.Owner == this && this.SpeechBubble.Hidden)
        this.Dot.Owner = (object) null;
      if (volume1 != null || this.Dot.RoamingVolume == null)
        return;
      this.Dot.Burrow();
    }
  }
}
