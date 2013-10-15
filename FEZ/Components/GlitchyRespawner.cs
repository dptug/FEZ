// Type: FezGame.Components.GlitchyRespawner
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
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  internal class GlitchyRespawner : DrawableGameComponent
  {
    private static readonly object StaticLock = new object();
    private const float StarsDuration = 1f;
    private const float FlashDuration = 1f;
    private const float FadeInDuration = 0.25f;
    private readonly ITargetRenderingManager TargetRenderer;
    private readonly ILightingPostProcess LightingPostProcess;
    private readonly ILevelMaterializer LevelMaterializer;
    private readonly IGameStateManager GameState;
    private readonly IDefaultCameraManager CameraManager;
    private readonly IGameLevelManager LevelManager;
    private readonly IContentManagerProvider CMProvider;
    private readonly TrileInstance Instance;
    private readonly bool EmitOrNot;
    private Mesh SpawnMesh;
    private Texture2D StarsTexture;
    public bool DontCullIn;
    private TimeSpan SinceAlive;
    private static volatile DefaultEffect FullbrightEffect;
    private static volatile CubemappedEffect CubemappedEffect;
    private int sinceColorSwapped;
    private int nextSwapIn;
    private bool redVisible;
    private bool greenVisible;
    private bool blueVisible;

    static GlitchyRespawner()
    {
    }

    public GlitchyRespawner(Game game, TrileInstance instance)
      : this(game, instance, true)
    {
    }

    public GlitchyRespawner(Game game, TrileInstance instance, bool soundEmitter)
      : base(game)
    {
      this.UpdateOrder = -2;
      this.DrawOrder = 10;
      this.Instance = instance;
      this.EmitOrNot = soundEmitter;
      this.TargetRenderer = ServiceHelper.Get<ITargetRenderingManager>();
      this.LightingPostProcess = ServiceHelper.Get<ILightingPostProcess>();
      this.LevelMaterializer = ServiceHelper.Get<ILevelMaterializer>();
      this.GameState = ServiceHelper.Get<IGameStateManager>();
      this.CameraManager = ServiceHelper.Get<IDefaultCameraManager>();
      this.LevelManager = ServiceHelper.Get<IGameLevelManager>();
      this.CMProvider = ServiceHelper.Get<IContentManagerProvider>();
    }

    public override void Initialize()
    {
      base.Initialize();
      lock (GlitchyRespawner.StaticLock)
      {
        if (GlitchyRespawner.FullbrightEffect == null)
        {
          DefaultEffect.Textured local_0 = new DefaultEffect.Textured();
          local_0.Fullbright = true;
          GlitchyRespawner.FullbrightEffect = (DefaultEffect) local_0;
        }
        if (GlitchyRespawner.CubemappedEffect == null)
          GlitchyRespawner.CubemappedEffect = new CubemappedEffect();
      }
      this.SpawnMesh = new Mesh()
      {
        SamplerState = SamplerState.PointClamp,
        DepthWrites = false,
        Effect = (BaseEffect) GlitchyRespawner.CubemappedEffect
      };
      ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry = this.Instance.Trile.Geometry;
      IndexedUserPrimitives<VertexPositionNormalTextureInstance> indexedUserPrimitives = new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(geometry.Vertices, geometry.Indices, geometry.PrimitiveType);
      Group group = this.SpawnMesh.AddGroup();
      group.Geometry = (IIndexedPrimitiveCollection) indexedUserPrimitives;
      group.Texture = (Texture) this.LevelMaterializer.TrilesMesh.Texture;
      if (ActorTypeExtensions.IsPickable(this.Instance.Trile.ActorSettings.Type))
        this.Instance.Phi = (float) FezMath.Round((double) this.Instance.Phi / 1.57079637050629) * 1.570796f;
      group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, this.Instance.Phi);
      if (this.Instance.Trile.ActorSettings.Type == ActorType.CubeShard || this.Instance.Trile.ActorSettings.Type == ActorType.SecretCube || this.Instance.Trile.ActorSettings.Type == ActorType.PieceOfHeart)
        group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Left, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0))) * Quaternion.CreateFromAxisAngle(Vector3.Down, 0.7853982f) * group.Rotation;
      this.Instance.Foreign = true;
      this.StarsTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/black_hole/Stars");
      if (this.EmitOrNot)
      {
        SoundEmitter soundEmitter = SoundEffectExtensions.EmitAt(this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/GlitchyRespawn"), this.Instance.Center);
        soundEmitter.PauseViewTransitions = true;
        soundEmitter.FactorizeVolume = true;
      }
      this.LightingPostProcess.DrawOnTopLights += new Action(this.DrawLights);
      this.LevelManager.LevelChanging += new Action(this.Kill);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.SpawnMesh.Dispose(false);
      this.LightingPostProcess.DrawOnTopLights -= new Action(this.DrawLights);
      this.LevelManager.LevelChanging -= new Action(this.Kill);
    }

    private void Kill()
    {
      ServiceHelper.RemoveComponent<GlitchyRespawner>(this);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || (!this.CameraManager.ActionRunning || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)))
        return;
      this.SinceAlive += gameTime.ElapsedGameTime;
      this.SpawnMesh.Position = this.Instance.Center;
      if (this.sinceColorSwapped++ >= this.nextSwapIn)
      {
        int num1 = RandomHelper.Random.Next(0, 4);
        this.redVisible = num1 == 0;
        this.greenVisible = num1 == 1;
        this.blueVisible = num1 == 2;
        if (num1 == 3)
        {
          int num2 = RandomHelper.Random.Next(0, 3);
          if (num2 == 0)
            this.blueVisible = this.redVisible = true;
          if (num2 == 1)
            this.greenVisible = this.redVisible = true;
          if (num2 == 2)
            this.blueVisible = this.greenVisible = true;
        }
        this.sinceColorSwapped = 0;
        this.nextSwapIn = RandomHelper.Random.Next(1, 6);
      }
      if (this.SinceAlive.TotalSeconds <= 2.25)
        return;
      this.Instance.Hidden = false;
      this.Instance.Enabled = true;
      if (ActorTypeExtensions.IsPickable(this.Instance.Trile.ActorSettings.Type))
      {
        this.Instance.PhysicsState.Respawned = true;
        this.Instance.PhysicsState.Vanished = false;
      }
      this.LevelManager.RestoreTrile(this.Instance);
      this.LevelMaterializer.UpdateInstance(this.Instance);
      ServiceHelper.RemoveComponent<GlitchyRespawner>(this);
    }

    public override void Draw(GameTime gameTime)
    {
      float alpha = FezMath.Saturate(Easing.EaseOut(this.SinceAlive.TotalSeconds / 1.0, EasingType.Quintic));
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.Glitch));
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
      this.SpawnMesh.Draw();
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.Glitch);
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      float m11 = this.CameraManager.Radius / ((float) this.StarsTexture.Width / 16f) / viewScale;
      float m22 = (float) ((double) this.CameraManager.Radius / (double) this.CameraManager.AspectRatio / ((double) this.StarsTexture.Height / 16.0)) / viewScale;
      Matrix textureMatrix = new Matrix(m11, 0.0f, 0.0f, 0.0f, 0.0f, m22, 0.0f, 0.0f, (float) (-(double) m11 / 2.0), (float) (-(double) m22 / 2.0), 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
      this.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
      this.TargetRenderer.DrawFullscreen((Texture) this.StarsTexture, textureMatrix, new Color(1f, 1f, 1f, alpha));
      if (this.SinceAlive.TotalSeconds > 2.0)
        this.TargetRenderer.DrawFullscreen(Color.White);
      else if (this.SinceAlive.TotalSeconds > 1.0)
        this.TargetRenderer.DrawFullscreen(new Color(this.redVisible ? 1f : 0.0f, this.greenVisible ? 1f : 0.0f, this.blueVisible ? 1f : 0.0f, 1f));
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
      GraphicsDeviceExtensions.GetDssCombiner(this.GraphicsDevice).StencilPass = StencilOperation.Zero;
      this.TargetRenderer.DrawFullscreen(Color.Black);
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.None));
      if (this.SinceAlive.TotalSeconds <= 2.0)
        return;
      float num = FezMath.Saturate(Easing.EaseIn((this.SinceAlive.TotalSeconds - 2.0) / 0.25, EasingType.Quadratic));
      this.SpawnMesh.Blending = new BlendingMode?(BlendingMode.Alphablending);
      this.SpawnMesh.Material.Opacity = num;
      this.SpawnMesh.Material.Diffuse = new Vector3(1f);
      this.SpawnMesh.Draw();
    }

    private void DrawLights()
    {
      BaseEffect effect = this.SpawnMesh.Effect;
      Texture texture = this.SpawnMesh.FirstGroup.Texture;
      this.SpawnMesh.FirstGroup.Texture = (Texture) null;
      this.SpawnMesh.Effect = (BaseEffect) GlitchyRespawner.FullbrightEffect;
      this.SpawnMesh.Draw();
      this.SpawnMesh.FirstGroup.Texture = texture;
      this.SpawnMesh.Effect = effect;
      if (this.SinceAlive.TotalSeconds <= 2.0)
        return;
      this.SpawnMesh.Material.Opacity = FezMath.Saturate(Easing.EaseIn((this.SinceAlive.TotalSeconds - 2.0) / 0.25, EasingType.Quadratic));
      (this.SpawnMesh.Effect as CubemappedEffect).Pass = LightingEffectPass.Pre;
      this.SpawnMesh.Draw();
      (this.SpawnMesh.Effect as CubemappedEffect).Pass = LightingEffectPass.Main;
    }
  }
}
