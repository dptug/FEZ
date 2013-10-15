// Type: FezGame.Components.MailboxesHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  internal class MailboxesHost : GameComponent
  {
    private readonly List<MailboxesHost.MailboxState> Mailboxes = new List<MailboxesHost.MailboxState>();

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public MailboxesHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.Mailboxes.Clear();
      foreach (ArtObjectInstance aoInstance in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
      {
        if (aoInstance.ArtObject.ActorType == ActorType.Mailbox && !this.GameState.SaveData.ThisLevel.InactiveArtObjects.Contains(aoInstance.Id) && aoInstance.ActorSettings.TreasureMapName != null)
          this.Mailboxes.Add(new MailboxesHost.MailboxState(aoInstance));
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InCutscene || this.GameState.InMap) || (this.GameState.InMenuCube || this.GameState.InFpsMode))
        return;
      for (int index = this.Mailboxes.Count - 1; index >= 0; --index)
      {
        this.Mailboxes[index].Update();
        if (this.Mailboxes[index].Empty)
          this.Mailboxes.RemoveAt(index);
      }
    }

    private class MailboxState
    {
      public readonly ArtObjectInstance MailboxAo;
      public BackgroundPlane CapsulePlane;

      public bool Empty { get; private set; }

      [ServiceDependency]
      public IGomezService GomezService { private get; set; }

      [ServiceDependency]
      public IGameStateManager GameState { private get; set; }

      [ServiceDependency]
      public IInputManager InputManager { private get; set; }

      [ServiceDependency]
      public IPlayerManager PlayerManager { private get; set; }

      [ServiceDependency]
      public IGameCameraManager CameraManager { private get; set; }

      public MailboxState(ArtObjectInstance aoInstance)
      {
        ServiceHelper.InjectServices((object) this);
        this.MailboxAo = aoInstance;
      }

      public void Update()
      {
        Vector3 position = this.MailboxAo.Position;
        Vector3 center = this.PlayerManager.Center;
        Vector3 b1 = FezMath.SideMask(this.CameraManager.Viewpoint);
        Vector3 b2 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
        Vector3 vector3 = new Vector3(FezMath.Dot(position, b1), position.Y, 0.0f);
        BoundingBox boundingBox = new BoundingBox(vector3 - new Vector3(0.5f, 0.75f, 0.5f), vector3 + new Vector3(0.5f, 0.75f, 0.5f));
        Vector3 point = new Vector3(FezMath.Dot(center, b1), center.Y, 0.0f);
        if ((double) FezMath.Dot(center, b2) >= (double) FezMath.Dot(this.MailboxAo.Position, b2) || boundingBox.Contains(point) == ContainmentType.Disjoint || this.InputManager.GrabThrow != FezButtonState.Pressed)
          return;
        this.GameState.SaveData.ThisLevel.InactiveArtObjects.Add(this.MailboxAo.Id);
        ServiceHelper.AddComponent((IGameComponent) new LetterViewer(ServiceHelper.Game, this.MailboxAo.ActorSettings.TreasureMapName));
        this.GomezService.OnReadMail();
        this.Empty = true;
      }
    }
  }
}
