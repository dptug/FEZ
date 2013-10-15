// Type: FezGame.Components.NowLoadingHexahedron
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Components.Actions;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  internal class NowLoadingHexahedron : DrawableGameComponent
  {
    private static NowLoadingHexahedron.Phases Phase = NowLoadingHexahedron.Phases.VectorOutline;
    private int NextOutline = 1;
    private static TimeSpan SincePhaseStarted;
    private static TimeSpan SinceWavesStarted;
    private static TimeSpan SinceTurning;
    private readonly string ToLevel;
    private readonly Vector3 Center;
    private Mesh Outline;
    private Mesh WireCube;
    private Mesh SolidCube;
    private Mesh Flare;
    private Mesh Rays;
    private float OutlineIn;
    private float WhiteFillStep;
    private float Phi;
    private SoundEffect WarpSound;
    private NowLoadingHexahedron.Darkener TheDarkening;

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public ISpeechBubbleManager Speech { get; set; }

    [ServiceDependency]
    public IDotManager Dot { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { get; set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency(Optional = true)]
    public IWalkToService WalkTo { protected get; set; }

    static NowLoadingHexahedron()
    {
    }

    public NowLoadingHexahedron(Game game, Vector3 center, string toLevel)
      : base(game)
    {
      this.ToLevel = toLevel;
      this.Center = center;
      this.UpdateOrder = 10;
      this.DrawOrder = 901;
    }

    public override void Initialize()
    {
      base.Initialize();
      NowLoadingHexahedron.Phase = NowLoadingHexahedron.Phases.VectorOutline;
      TimeSpan timeSpan;
      NowLoadingHexahedron.SinceTurning = timeSpan = TimeSpan.Zero;
      NowLoadingHexahedron.SinceWavesStarted = timeSpan;
      NowLoadingHexahedron.SincePhaseStarted = timeSpan;
      this.PlayerManager.CanControl = false;
      this.WarpSound = this.CMProvider.GetForLevel(this.GameState.IsTrialMode ? "trial/ELDERS" : "ELDERS").Load<SoundEffect>("Sounds/Zu/HexaWarpIn");
      this.Dot.Hidden = false;
      this.Dot.Behaviour = DotHost.BehaviourType.ClampToTarget;
      this.Dot.Target = this.Center;
      this.Dot.ScalePulsing = 0.0f;
      this.Dot.Opacity = 0.0f;
      Waiters.Wait((Func<bool>) (() => this.PlayerManager.Grounded), (Action) (() =>
      {
        this.WalkTo.Destination = (Func<Vector3>) (() => this.PlayerManager.Position * Vector3.UnitY + this.Center * FezMath.XZMask);
        this.WalkTo.NextAction = ActionType.Idle;
        this.PlayerManager.Action = ActionType.WalkingTo;
      }));
      this.TimeManager.TimeFactor = this.TimeManager.DefaultTimeFactor;
      NowLoadingHexahedron loadingHexahedron1 = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
      vertexColored1.Fullbright = true;
      vertexColored1.AlphaIsEmissive = false;
      DefaultEffect.VertexColored vertexColored2 = vertexColored1;
      mesh2.Effect = (BaseEffect) vertexColored2;
      mesh1.DepthWrites = false;
      mesh1.AlwaysOnTop = true;
      Mesh mesh3 = mesh1;
      loadingHexahedron1.Outline = mesh3;
      this.Outline.AddWireframePolygon(Color.White, new Vector3(0.0f, 0.8660254f, 0.0f), new Vector3(0.7071068f, 0.2886752f, 0.0f), new Vector3(0.7071068f, -0.2886752f, 0.0f), new Vector3(0.0f, -0.8660254f, 0.0f), new Vector3(-0.7071068f, -0.2886752f, 0.0f), new Vector3(-0.7071068f, 0.2886752f, 0.0f), new Vector3(0.0f, 0.8660254f, 0.0f));
      this.Outline.Scale = new Vector3(4f);
      this.Outline.BakeTransform<FezVertexPositionColor>();
      Group firstGroup = this.Outline.FirstGroup;
      firstGroup.Material = new Material();
      firstGroup.Enabled = false;
      for (int index = 0; index < 1024; ++index)
        this.Outline.CloneGroup(firstGroup);
      firstGroup.Enabled = true;
      NowLoadingHexahedron loadingHexahedron2 = this;
      Mesh mesh4 = new Mesh();
      Mesh mesh5 = mesh4;
      DefaultEffect.VertexColored vertexColored3 = new DefaultEffect.VertexColored();
      vertexColored3.Fullbright = true;
      vertexColored3.AlphaIsEmissive = false;
      DefaultEffect.VertexColored vertexColored4 = vertexColored3;
      mesh5.Effect = (BaseEffect) vertexColored4;
      mesh4.DepthWrites = false;
      mesh4.AlwaysOnTop = true;
      mesh4.Material.Opacity = 0.0f;
      Mesh mesh6 = mesh4;
      loadingHexahedron2.WireCube = mesh6;
      this.WireCube.AddWireframeBox(Vector3.One * 4f, Vector3.Zero, Color.White, true);
      this.WireCube.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0))) * Quaternion.CreateFromAxisAngle(Vector3.Up, 0.7853982f);
      this.WireCube.BakeTransform<FezVertexPositionColor>();
      NowLoadingHexahedron loadingHexahedron3 = this;
      Mesh mesh7 = new Mesh();
      Mesh mesh8 = mesh7;
      DefaultEffect.LitVertexColored litVertexColored1 = new DefaultEffect.LitVertexColored();
      litVertexColored1.Fullbright = false;
      litVertexColored1.AlphaIsEmissive = false;
      DefaultEffect.LitVertexColored litVertexColored2 = litVertexColored1;
      mesh8.Effect = (BaseEffect) litVertexColored2;
      mesh7.AlwaysOnTop = true;
      mesh7.Material.Opacity = 0.0f;
      Mesh mesh9 = mesh7;
      loadingHexahedron3.SolidCube = mesh9;
      this.SolidCube.AddFlatShadedBox(Vector3.One * 4f, Vector3.Zero, Color.White, true);
      this.SolidCube.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0))) * Quaternion.CreateFromAxisAngle(Vector3.Up, 0.7853982f);
      this.SolidCube.BakeTransform<VertexPositionNormalColor>();
      NowLoadingHexahedron loadingHexahedron4 = this;
      Mesh mesh10 = new Mesh();
      Mesh mesh11 = mesh10;
      DefaultEffect.Textured textured1 = new DefaultEffect.Textured();
      textured1.Fullbright = true;
      textured1.AlphaIsEmissive = false;
      DefaultEffect.Textured textured2 = textured1;
      mesh11.Effect = (BaseEffect) textured2;
      mesh10.DepthWrites = false;
      mesh10.Material.Opacity = 0.0f;
      mesh10.Blending = new BlendingMode?(BlendingMode.Alphablending);
      mesh10.SamplerState = SamplerState.LinearClamp;
      Mesh mesh12 = mesh10;
      loadingHexahedron4.Flare = mesh12;
      this.Flare.AddFace(Vector3.One * 4f, Vector3.Zero, FaceOrientation.Front, true);
      this.Flare.Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/flare_alpha"));
      this.Rays = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/smooth_ray")),
        Blending = new BlendingMode?(BlendingMode.Additive),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      for (int index = 0; index < 128; ++index)
      {
        float x = 0.75f;
        float num = 0.0075f;
        Group group = this.Rays.AddGroup();
        group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionTexture>(new FezVertexPositionTexture[6]
        {
          new FezVertexPositionTexture(new Vector3(0.0f, (float) ((double) num / 2.0 * 0.100000001490116), 0.0f), new Vector2(0.0f, 0.0f)),
          new FezVertexPositionTexture(new Vector3(x, num / 2f, 0.0f), new Vector2(1f, 0.0f)),
          new FezVertexPositionTexture(new Vector3(x, (float) ((double) num / 2.0 * 0.100000001490116), 0.0f), new Vector2(1f, 0.45f)),
          new FezVertexPositionTexture(new Vector3(x, (float) (-(double) num / 2.0 * 0.100000001490116), 0.0f), new Vector2(1f, 0.55f)),
          new FezVertexPositionTexture(new Vector3(x, (float) (-(double) num / 2.0), 0.0f), new Vector2(1f, 1f)),
          new FezVertexPositionTexture(new Vector3(0.0f, (float) (-(double) num / 2.0 * 0.100000001490116), 0.0f), new Vector2(0.0f, 1f))
        }, new int[12]
        {
          0,
          1,
          2,
          0,
          2,
          5,
          5,
          2,
          3,
          5,
          3,
          4
        }, PrimitiveType.TriangleList);
        group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Forward, RandomHelper.Between(0.0, 6.28318548202515));
        group.Material = new Material()
        {
          Diffuse = new Vector3(0.0f)
        };
      }
      ServiceHelper.AddComponent((IGameComponent) (this.TheDarkening = new NowLoadingHexahedron.Darkener(this.Game)));
      this.LevelManager.LevelChanged += new Action(this.Kill);
      SoundEffectExtensions.Emit(this.WarpSound).Persistent = true;
    }

    private void Kill()
    {
      if (NowLoadingHexahedron.Phase == NowLoadingHexahedron.Phases.Load)
        return;
      ServiceHelper.RemoveComponent<NowLoadingHexahedron>(this);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.Outline.Dispose();
      this.WireCube.Dispose();
      this.SolidCube.Dispose();
      this.Rays.Dispose();
      this.Flare.Dispose();
      ServiceHelper.RemoveComponent<NowLoadingHexahedron.Darkener>(this.TheDarkening);
      this.LevelManager.LevelChanged -= new Action(this.Kill);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      TimeSpan timeSpan = TimeSpan.Zero;
      if (!this.GameState.Paused && this.CameraManager.ActionRunning && FezMath.IsOrthographic(this.CameraManager.Viewpoint))
        timeSpan = gameTime.ElapsedGameTime;
      float num1 = (float) NowLoadingHexahedron.SincePhaseStarted.TotalSeconds;
      float num2 = (float) timeSpan.TotalSeconds;
      this.Rays.Position = this.Flare.Position = this.SolidCube.Position = this.WireCube.Position = this.Outline.Position = this.Center;
      this.Flare.Rotation = this.Outline.Rotation = this.CameraManager.Rotation;
      this.SolidCube.Rotation = this.WireCube.Rotation = this.CameraManager.Rotation * Quaternion.CreateFromAxisAngle(Vector3.Up, this.Phi);
      NowLoadingHexahedron.SincePhaseStarted += timeSpan;
      if (NowLoadingHexahedron.Phase < NowLoadingHexahedron.Phases.TurnTurnTurn)
      {
        NowLoadingHexahedron.SinceWavesStarted += timeSpan;
        float num3 = (float) (1.0 + Math.Pow(Math.Max(NowLoadingHexahedron.SinceWavesStarted.TotalSeconds - 2.0, 0.0) / 5.0, 4.0));
        if (NowLoadingHexahedron.SinceWavesStarted.TotalSeconds > 2.0 && NowLoadingHexahedron.SinceWavesStarted.TotalSeconds < 12.0)
        {
          this.OutlineIn -= num2;
          if ((double) this.OutlineIn <= 0.0)
          {
            this.Outline.Groups[this.NextOutline].Enabled = true;
            this.Outline.Groups[this.NextOutline].Scale = new Vector3(5f);
            if (++this.NextOutline >= this.Outline.Groups.Count)
              this.NextOutline = 1;
            if ((double) num3 > 15.0)
              this.OutlineIn = 0.0f;
            else
              this.OutlineIn += 2f / (float) Math.Pow((double) num3 * 1.25, 2.0);
          }
        }
        foreach (Group group in this.Outline.Groups)
        {
          if (group.Enabled && group.Id != 0)
          {
            group.Scale -= new Vector3(num3 * num2);
            group.Material.Opacity = Easing.EaseOut((double) FezMath.Saturate((float) (1.0 - ((double) group.Scale.X - 1.0) / 4.0)), EasingType.Sine);
            if ((double) group.Scale.X <= 1.0)
              group.Enabled = false;
          }
        }
      }
      if (NowLoadingHexahedron.Phase >= NowLoadingHexahedron.Phases.TurnTurnTurn)
      {
        NowLoadingHexahedron.SinceTurning += timeSpan;
        float num3 = MathHelper.Lerp(0.0f, 13f, Easing.EaseIn((double) Easing.EaseOut(NowLoadingHexahedron.SinceTurning.TotalSeconds / 9.75, EasingType.Sine), EasingType.Quintic));
        this.Dot.RotationSpeed = -num3;
        this.Phi += num2 * (float) Math.Pow((double) num3, 1.125);
      }
      switch (NowLoadingHexahedron.Phase)
      {
        case NowLoadingHexahedron.Phases.VectorOutline:
          float num4 = FezMath.Saturate(Easing.Ease((double) num1 / 3.0, -0.75f, EasingType.Sine));
          this.Outline.FirstGroup.Material.Opacity = num4;
          this.Outline.FirstGroup.Scale = new Vector3((float) (4.0 - (double) num4 * 3.0));
          if ((double) num1 < 3.0)
            break;
          NowLoadingHexahedron.Phase = NowLoadingHexahedron.Phases.WireframeWaves;
          this.PlayerManager.Action = ActionType.LookingUp;
          NowLoadingHexahedron.SincePhaseStarted = TimeSpan.Zero;
          ServiceHelper.AddComponent((IGameComponent) new CamShake(ServiceHelper.Game)
          {
            Duration = TimeSpan.FromSeconds(10.0),
            Distance = 0.25f
          });
          break;
        case NowLoadingHexahedron.Phases.WireframeWaves:
          this.Speech.Hide();
          float num5 = Easing.EaseIn((double) FezMath.Saturate(num1 / 5f), EasingType.Quadratic);
          this.WireCube.Material.Opacity = num5;
          this.Outline.FirstGroup.Material.Opacity = 1f - num5;
          if ((double) num5 != 1.0)
            break;
          NowLoadingHexahedron.Phase = NowLoadingHexahedron.Phases.FillCube;
          NowLoadingHexahedron.SincePhaseStarted = TimeSpan.Zero;
          break;
        case NowLoadingHexahedron.Phases.FillCube:
          float num6 = FezMath.Saturate(num1 / 4f);
          this.WireCube.Material.Opacity = 1f - num6;
          this.SolidCube.Material.Opacity = num6;
          (this.SolidCube.Effect as DefaultEffect).Emissive = num6 / 2f;
          this.Flare.Material.Opacity = num6 / 2f;
          this.Flare.Scale = new Vector3(4f * num6);
          if ((double) num6 != 1.0)
            break;
          NowLoadingHexahedron.Phase = NowLoadingHexahedron.Phases.FillDot;
          NowLoadingHexahedron.SincePhaseStarted = TimeSpan.Zero;
          break;
        case NowLoadingHexahedron.Phases.FillDot:
          float num7 = FezMath.Saturate(num1 / 2.75f);
          this.Dot.Hidden = false;
          this.Dot.ScaleFactor = 50f * Easing.EaseOut((double) num7, EasingType.Sine);
          this.Dot.InnerScale = 1f;
          this.Dot.Opacity = (float) (0.5 + 0.25 * (double) num7);
          this.Dot.RotationSpeed = 0.0f;
          this.GameState.SkyOpacity = 1f - num7;
          if ((double) num7 != 1.0)
            break;
          NowLoadingHexahedron.Phase = NowLoadingHexahedron.Phases.TurnTurnTurn;
          NowLoadingHexahedron.SincePhaseStarted = TimeSpan.Zero;
          break;
        case NowLoadingHexahedron.Phases.TurnTurnTurn:
          if ((double) FezMath.Saturate(num1 / 7.5f) != 1.0)
            break;
          NowLoadingHexahedron.Phase = NowLoadingHexahedron.Phases.Rays;
          NowLoadingHexahedron.SincePhaseStarted = TimeSpan.Zero;
          break;
        case NowLoadingHexahedron.Phases.Rays:
          float num8 = Easing.EaseIn((double) FezMath.Saturate(num1 / 1.75f), EasingType.Quadratic);
          (this.SolidCube.Effect as DefaultEffect).Emissive = (float) (0.5 + (double) num8 / 2.0);
          this.Flare.Material.Opacity = (float) (0.75 + (double) num8 * 0.25);
          this.Flare.Scale = new Vector3((float) (4.0 + 5.0 * (double) num8));
          float num9 = Easing.EaseIn((double) FezMath.Saturate(num1 / 1.75f), EasingType.Cubic);
          foreach (Group group in this.Rays.Groups)
          {
            float num3 = (float) group.Id / (float) this.Rays.Groups.Count;
            group.Material.Diffuse = new Vector3(FezMath.Saturate((float) ((double) num9 / (double) num3 * 4.0)) * 0.25f);
            float val1 = (float) Math.Pow((double) num9 / (double) num3 * 4.0, 2.0);
            group.Scale = new Vector3(Math.Min(val1, 50f), Math.Min(val1, 100f), 1f);
          }
          if ((double) num9 != 1.0)
            break;
          NowLoadingHexahedron.Phase = NowLoadingHexahedron.Phases.FadeToWhite;
          NowLoadingHexahedron.SincePhaseStarted = TimeSpan.Zero;
          break;
        case NowLoadingHexahedron.Phases.FadeToWhite:
          float num10 = FezMath.Saturate(num1 / 1f);
          this.WhiteFillStep = num10;
          if ((double) num10 != 1.0)
            break;
          NowLoadingHexahedron.Phase = NowLoadingHexahedron.Phases.Load;
          NowLoadingHexahedron.SincePhaseStarted = TimeSpan.Zero;
          break;
        case NowLoadingHexahedron.Phases.Load:
          this.GameState.SkyOpacity = 1f;
          this.GameState.SkipLoadScreen = true;
          this.GameState.Loading = true;
          Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoLoad));
          worker.Finished += (Action) (() => this.ThreadPool.Return<bool>(worker));
          worker.Start(false);
          break;
        case NowLoadingHexahedron.Phases.FadeOut:
          this.WhiteFillStep = 1f - Easing.EaseOut((double) FezMath.Saturate(num1 / 0.75f), EasingType.Quintic);
          if ((double) num1 <= 0.75)
            break;
          ServiceHelper.RemoveComponent<NowLoadingHexahedron>(this);
          break;
      }
    }

    private void DoLoad(bool dummy)
    {
      HorizontalDirection lookingDirection = this.PlayerManager.LookingDirection;
      this.Dot.Reset();
      this.LevelManager.ChangeLevel(this.ToLevel);
      this.PlayerManager.LookingDirection = lookingDirection;
      NowLoadingHexahedron.Phase = NowLoadingHexahedron.Phases.FadeOut;
      this.PlayerManager.CheckpointGround = (TrileInstance) null;
      this.PlayerManager.RespawnAtCheckpoint();
      this.CameraManager.Center = this.PlayerManager.Position + Vector3.Up * this.PlayerManager.Size.Y / 2f + Vector3.UnitY;
      this.CameraManager.SnapInterpolation();
      this.LevelMaterializer.CullInstances();
      this.GameState.ScheduleLoadEnd = true;
      this.GameState.SkipLoadScreen = false;
      this.TimeManager.TimeFactor = this.TimeManager.DefaultTimeFactor;
    }

    public override void Draw(GameTime gameTime)
    {
      if (NowLoadingHexahedron.Phase != NowLoadingHexahedron.Phases.FadeOut)
      {
        this.Outline.Draw();
        this.WireCube.Draw();
        this.Rays.Draw();
        this.SolidCube.Draw();
        this.Flare.Draw();
      }
      if ((double) this.WhiteFillStep <= 0.0)
        return;
      this.TargetRenderer.DrawFullscreen(new Color(1f, 1f, 1f, this.WhiteFillStep));
    }

    private enum Phases
    {
      VectorOutline,
      WireframeWaves,
      FillCube,
      FillDot,
      TurnTurnTurn,
      Rays,
      FadeToWhite,
      Load,
      FadeOut,
    }

    public class Darkener : DrawableGameComponent
    {
      [ServiceDependency]
      public ITargetRenderingManager TargetRenderer { get; set; }

      [ServiceDependency]
      public IGameStateManager GameState { get; set; }

      public Darkener(Game game)
        : base(game)
      {
        this.DrawOrder = 899;
      }

      public override void Draw(GameTime gameTime)
      {
        if (NowLoadingHexahedron.Phase == NowLoadingHexahedron.Phases.FadeOut)
          return;
        this.TargetRenderer.DrawFullscreen(new Color(0.0f, 0.0f, 0.0f, FezMath.Saturate(Easing.EaseIn(NowLoadingHexahedron.SinceWavesStarted.TotalSeconds / 12.0, EasingType.Cubic)) * 0.375f * this.GameState.SkyOpacity));
      }
    }
  }
}
