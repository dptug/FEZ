// Type: FezGame.Components.PyramidHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FezGame.Components
{
  internal class PyramidHost : DrawableGameComponent
  {
    private static readonly Vector2 DoorCenter = new Vector2(25f, 168f);
    private ArtObjectInstance MotherCubeAo;
    private Vector3 OriginalPosition;
    private float TimeAccumulator;
    private bool DoCapture;
    private Vector3 OriginalCenter;
    private SoundEffect sRotationDrone;
    private SoundEffect sWhiteOut;
    private SoundEmitter eRotationDrone;
    private Mesh RaysMesh;
    private Mesh FlareMesh;

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardManager { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    static PyramidHost()
    {
    }

    public PyramidHost(Game game)
      : base(game)
    {
      this.DrawOrder = 500;
      this.Enabled = this.Visible = false;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.KeyboardManager.RegisterKey(Keys.R);
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.Visible = this.Enabled = this.LevelManager.Name == "PYRAMID";
      this.Clear();
      if (!this.Enabled)
        return;
      this.MotherCubeAo = this.LevelManager.ArtObjects[217];
      this.OriginalPosition = this.MotherCubeAo.Position;
      this.RaysMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/smooth_ray")),
        Blending = new BlendingMode?(BlendingMode.Additive),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      this.FlareMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/flare_alpha")),
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      this.FlareMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, true);
      this.sRotationDrone = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Ending/Pyramid/MothercubeRotateDrone");
      this.sWhiteOut = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ending/Pyramid/WhiteOut");
      this.eRotationDrone = SoundEffectExtensions.EmitAt(this.sRotationDrone, this.OriginalPosition, true);
    }

    private void Clear()
    {
      this.MotherCubeAo = (ArtObjectInstance) null;
      this.DoCapture = false;
      this.TimeAccumulator = 0.0f;
      if (this.RaysMesh != null)
        this.RaysMesh.Dispose();
      if (this.FlareMesh != null)
        this.FlareMesh.Dispose();
      this.FlareMesh = this.RaysMesh = (Mesh) null;
      this.sRotationDrone = this.sWhiteOut = (SoundEffect) null;
      if (this.eRotationDrone == null || this.eRotationDrone.Dead)
        return;
      this.eRotationDrone.FadeOutAndDie(0.0f);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || (!this.CameraManager.ActionRunning || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)) || this.GameState.Loading)
        return;
      if (this.DoCapture)
      {
        this.TimeAccumulator += (float) (gameTime.ElapsedGameTime.TotalSeconds * 1.5);
        this.MotherCubeAo.Position = Vector3.Lerp(this.MotherCubeAo.Position, this.OriginalPosition, 0.025f);
        this.MotherCubeAo.Rotation = Quaternion.Slerp(this.MotherCubeAo.Rotation, Quaternion.CreateFromYawPitchRoll(FezMath.ToPhi(FezMath.VisibleOrientation(this.CameraManager.Viewpoint)), 0.0f, 0.0f), 0.025f);
        this.PlayerManager.Position = Vector3.Lerp(this.PlayerManager.Position, this.PlayerManager.Position * FezMath.DepthMask(this.CameraManager.Viewpoint) + PyramidHost.DoorCenter.X * FezMath.SideMask(this.CameraManager.Viewpoint) + PyramidHost.DoorCenter.Y * Vector3.UnitY - Vector3.UnitY * 0.125f, 0.025f);
        this.GameState.SkipRendering = true;
        this.CameraManager.Center = Vector3.Lerp(this.OriginalCenter, this.PlayerManager.Position, 0.025f);
        this.GameState.SkipRendering = false;
        this.UpdateRays((float) gameTime.ElapsedGameTime.TotalSeconds);
        if ((double) this.TimeAccumulator <= 6.0)
          return;
        this.GameState.SkipLoadScreen = true;
        this.LevelManager.ChangeLevel("HEX_REBUILD");
        Waiters.Wait((Func<bool>) (() => !this.GameState.Loading), (Action) (() =>
        {
          this.GameState.SkipLoadScreen = false;
          this.Clear();
          this.Visible = false;
        }));
        this.Enabled = false;
      }
      else
      {
        this.TimeAccumulator += (float) (gameTime.ElapsedGameTime.TotalSeconds / 2.0);
        this.TimeAccumulator = FezMath.WrapAngle(this.TimeAccumulator);
        this.MotherCubeAo.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) (-gameTime.ElapsedGameTime.TotalSeconds * 0.375));
        Vector3 vector3 = new Vector3(0.0f, (float) Math.Sin((double) this.TimeAccumulator), 0.0f) / 2f;
        this.MotherCubeAo.Position = this.OriginalPosition + vector3;
        Vector2 vector2 = new Vector2(FezMath.Dot(this.PlayerManager.Center, FezMath.SideMask(this.CameraManager.Viewpoint)), this.PlayerManager.Center.Y);
        if ((double) Math.Abs(vector2.X - PyramidHost.DoorCenter.X) >= 1.0 || (double) Math.Abs(vector2.Y - (PyramidHost.DoorCenter.Y + vector3.Y)) >= 1.0 || (double) FezMath.AngleBetween(Vector3.Transform(-Vector3.UnitZ, this.MotherCubeAo.Rotation), FezMath.ForwardVector(this.CameraManager.Viewpoint)) >= 0.25)
          return;
        this.DoCapture = true;
        this.TimeAccumulator = 0.0f;
        this.PlayerManager.CanControl = false;
        this.PlayerManager.Action = ActionType.Floating;
        this.PlayerManager.Velocity = Vector3.Zero;
        this.OriginalCenter = this.CameraManager.Center;
        this.eRotationDrone.FadeOutAndDie(1.5f);
        this.SoundManager.PlayNewSong(5f);
        SoundEffectExtensions.Emit(this.sWhiteOut).Persistent = true;
      }
    }

    private void UpdateRays(float elapsedSeconds)
    {
      if (this.RaysMesh.Groups.Count < 50 && RandomHelper.Probability(0.25))
      {
        float x = 6f + RandomHelper.Centered(4.0);
        float num = RandomHelper.Between(0.5, (double) x / 2.5);
        Group group = this.RaysMesh.AddGroup();
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
        group.CustomData = (object) new DotHost.RayState();
        group.Material = new Material()
        {
          Diffuse = new Vector3(0.0f)
        };
        group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Forward, RandomHelper.Between(0.0, 6.28318548202515));
      }
      for (int i = this.RaysMesh.Groups.Count - 1; i >= 0; --i)
      {
        Group group = this.RaysMesh.Groups[i];
        DotHost.RayState rayState = group.CustomData as DotHost.RayState;
        rayState.Age += elapsedSeconds * 0.15f;
        float num1 = Easing.EaseOut((double) Easing.EaseOut(Math.Sin((double) rayState.Age * 6.28318548202515 - 1.57079637050629) * 0.5 + 0.5, EasingType.Quintic), EasingType.Quintic);
        group.Material.Diffuse = Vector3.Lerp(Vector3.One, rayState.Tint.ToVector3(), 0.05f) * 0.15f * num1;
        float num2 = rayState.Speed;
        group.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Forward, (float) ((double) elapsedSeconds * (double) num2 * (0.100000001490116 + (double) Easing.EaseIn((double) this.TimeAccumulator / 3.0, EasingType.Quadratic) * 0.200000002980232)));
        group.Scale = new Vector3((float) ((double) num1 * 0.75 + 0.25), (float) ((double) num1 * 0.5 + 0.5), 1f);
        if ((double) rayState.Age > 1.0)
          this.RaysMesh.RemoveGroupAt(i);
      }
      this.FlareMesh.Position = this.RaysMesh.Position = this.PlayerManager.Center;
      this.FlareMesh.Rotation = this.RaysMesh.Rotation = this.CameraManager.Rotation;
      this.RaysMesh.Scale = new Vector3(Easing.EaseIn((double) this.TimeAccumulator / 2.0, EasingType.Quadratic) + 1f);
      this.FlareMesh.Material.Opacity = (float) (0.125 + (double) Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.TimeAccumulator - 2.0) / 3.0)), EasingType.Cubic) * 0.875);
      this.FlareMesh.Scale = Vector3.One + this.RaysMesh.Scale * Easing.EaseIn((double) Math.Max(this.TimeAccumulator - 2.5f, 0.0f) / 1.5, EasingType.Cubic) * 4f;
      if (this.KeyboardManager.GetKeyState(Keys.R) != FezButtonState.Pressed)
        return;
      this.TimeAccumulator = 0.0f;
      this.RaysMesh.ClearGroups();
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || this.GameState.Loading)
        return;
      base.Draw(gameTime);
      this.RaysMesh.Draw();
      this.FlareMesh.Draw();
      if (!this.DoCapture)
        return;
      this.TargetRenderer.DrawFullscreen(new Color(1f, 1f, 1f, FezMath.Saturate(Easing.EaseIn(((double) this.TimeAccumulator - 6.0) / 1.0, EasingType.Quintic))));
    }
  }
}
