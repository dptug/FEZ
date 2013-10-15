// Type: FezGame.Components.Actions.WakingUp
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Effects;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components.Actions
{
  internal class WakingUp : PlayerAction
  {
    private static readonly TimeSpan FadeTime = TimeSpan.FromSeconds(1.0);
    private readonly Mesh fadePlane;
    private TimeSpan sinceStarted;
    private bool respawned;
    private bool diedByLava;

    static WakingUp()
    {
    }

    public WakingUp(Game game)
      : base(game)
    {
      WakingUp wakingUp = this;
      Mesh mesh1 = new Mesh();
      mesh1.AlwaysOnTop = true;
      mesh1.DepthWrites = false;
      Mesh mesh2 = mesh1;
      DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
      vertexColored1.ForcedViewMatrix = new Matrix?(Matrix.Identity);
      vertexColored1.ForcedProjectionMatrix = new Matrix?(Matrix.Identity);
      DefaultEffect.VertexColored vertexColored2 = vertexColored1;
      mesh2.Effect = (BaseEffect) vertexColored2;
      Mesh mesh3 = mesh1;
      wakingUp.fadePlane = mesh3;
      this.fadePlane.AddFace(Vector3.One * 2f, Vector3.Zero, FaceOrientation.Front, Color.Black, true);
      this.Visible = false;
      this.DrawOrder = 101;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.Reset);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      if (this.PlayerManager.Animation.Timing.Ended)
      {
        this.Reset();
        this.PlayerManager.Action = ActionType.Idle;
      }
      return true;
    }

    private void Reset()
    {
      if (ActionTypeExtensions.IsIdle(this.PlayerManager.Action) && this.diedByLava)
        return;
      this.diedByLava = false;
      this.Visible = false;
      this.respawned = false;
      this.sinceStarted = TimeSpan.FromSeconds(-1.0);
      this.GameState.SkipFadeOut = false;
    }

    public override void Draw(GameTime gameTime)
    {
      TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
      if (this.CameraManager.ActionRunning)
        this.sinceStarted += elapsedGameTime;
      if (this.sinceStarted >= WakingUp.FadeTime && !this.respawned)
      {
        this.PlayerManager.RespawnAtCheckpoint();
        if (!this.GameState.SkipFadeOut)
          this.CameraManager.Constrained = false;
        this.respawned = true;
      }
      if (this.GameState.SkipFadeOut)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.Always, StencilMask.None);
      this.fadePlane.Material.Opacity = 1f - Math.Abs((float) (1.0 - (double) this.sinceStarted.Ticks / (double) WakingUp.FadeTime.Ticks));
      this.fadePlane.Draw();
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return type == ActionType.WakingUp;
    }
  }
}
