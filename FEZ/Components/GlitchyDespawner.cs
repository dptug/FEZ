// Type: FezGame.Components.GlitchyDespawner
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
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
using System.Linq;

namespace FezGame.Components
{
  internal class GlitchyDespawner : DrawableGameComponent
  {
    public ActorType ActorToSpawn = ActorType.SecretCube;
    private const float StarsDuration = 1f;
    private const float FlashDuration = 1f;
    private const float FadeInDuration = 0.5f;
    private ArtObjectInstance AoInstance;
    private TrileInstance TrileInstance;
    private readonly Vector3 TreasureCenter;
    private readonly bool CreateTreasure;
    private readonly bool IsArtObject;
    public bool FlashOnSpawn;
    private Mesh SpawnMesh;
    private Texture2D StarsTexture;
    private DefaultEffect FullbrightEffect;
    private TimeSpan SinceAlive;
    private int sinceColorSwapped;
    private int nextSwapIn;
    private bool redVisible;
    private bool greenVisible;
    private bool blueVisible;
    private bool hasCreatedTreasure;

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    private GlitchyDespawner(Game game)
      : base(game)
    {
      this.UpdateOrder = -2;
      this.DrawOrder = 10;
    }

    public GlitchyDespawner(Game game, ArtObjectInstance instance)
      : this(game)
    {
      this.AoInstance = instance;
      this.CreateTreasure = false;
      instance.Hidden = true;
      this.IsArtObject = true;
    }

    public GlitchyDespawner(Game game, ArtObjectInstance instance, Vector3 treasureCenter)
      : this(game)
    {
      this.AoInstance = instance;
      this.TreasureCenter = treasureCenter;
      this.CreateTreasure = true;
      instance.Hidden = true;
      this.IsArtObject = true;
    }

    public GlitchyDespawner(Game game, TrileInstance instance)
      : this(game)
    {
      this.UpdateOrder = -2;
      this.DrawOrder = 10;
      this.TrileInstance = instance;
      this.CreateTreasure = false;
      instance.Hidden = true;
      this.IsArtObject = false;
    }

    public GlitchyDespawner(Game game, TrileInstance instance, Vector3 treasureCenter)
      : this(game)
    {
      this.UpdateOrder = -2;
      this.DrawOrder = 10;
      this.TrileInstance = instance;
      this.TreasureCenter = treasureCenter;
      this.CreateTreasure = true;
      instance.Hidden = true;
      this.IsArtObject = false;
    }

    public override void Initialize()
    {
      base.Initialize();
      if (!this.IsArtObject)
        this.LevelMaterializer.CullInstanceOut(this.TrileInstance);
      this.SpawnMesh = new Mesh()
      {
        SamplerState = SamplerState.PointClamp,
        DepthWrites = false,
        Effect = (BaseEffect) new CubemappedEffect()
      };
      IIndexedPrimitiveCollection primitiveCollection;
      if (this.IsArtObject)
      {
        ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix> geometry = this.AoInstance.ArtObject.Geometry;
        primitiveCollection = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(geometry.Vertices, geometry.Indices, geometry.PrimitiveType);
        this.AoInstance.Material = new Material();
      }
      else
      {
        ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry = this.TrileInstance.Trile.Geometry;
        primitiveCollection = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(geometry.Vertices, geometry.Indices, geometry.PrimitiveType);
      }
      Group group = this.SpawnMesh.AddGroup();
      group.Geometry = primitiveCollection;
      group.Rotation = this.IsArtObject ? this.AoInstance.Rotation : Quaternion.CreateFromAxisAngle(Vector3.UnitY, this.TrileInstance.Phi);
      if (!this.IsArtObject)
      {
        group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, this.TrileInstance.Phi);
        if (this.TrileInstance.Trile.ActorSettings.Type == ActorType.CubeShard || this.TrileInstance.Trile.ActorSettings.Type == ActorType.SecretCube || this.TrileInstance.Trile.ActorSettings.Type == ActorType.PieceOfHeart)
          group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Left, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0))) * Quaternion.CreateFromAxisAngle(Vector3.Down, 0.7853982f) * group.Rotation;
      }
      this.SpawnMesh.Position = this.IsArtObject ? this.AoInstance.Position : this.TrileInstance.Center;
      this.StarsTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/black_hole/Stars");
      GlitchyDespawner glitchyDespawner = this;
      DefaultEffect.Textured textured1 = new DefaultEffect.Textured();
      textured1.Fullbright = true;
      DefaultEffect.Textured textured2 = textured1;
      glitchyDespawner.FullbrightEffect = (DefaultEffect) textured2;
      SoundEffectExtensions.EmitAt(this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/GlitchyRespawn"), this.SpawnMesh.Position);
      this.LightingPostProcess.DrawOnTopLights += new Action(this.DrawLights);
      this.LevelManager.LevelChanging += new Action(this.Kill);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.SpawnMesh.Dispose();
      this.FullbrightEffect.Dispose();
      this.LightingPostProcess.DrawOnTopLights -= new Action(this.DrawLights);
      this.LevelManager.LevelChanging -= new Action(this.Kill);
    }

    private void Kill()
    {
      ServiceHelper.RemoveComponent<GlitchyDespawner>(this);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || (!this.CameraManager.ActionRunning || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)))
        return;
      this.SinceAlive += gameTime.ElapsedGameTime;
      this.SpawnMesh.Position = this.IsArtObject ? this.AoInstance.Position : this.TrileInstance.Center;
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
      if (this.SinceAlive.TotalSeconds > 2.0 && !this.hasCreatedTreasure && this.CreateTreasure)
      {
        if (!this.IsArtObject)
        {
          this.TrileInstance.PhysicsState = (InstancePhysicsState) null;
          this.LevelManager.ClearTrile(this.TrileInstance);
        }
        if (this.FlashOnSpawn)
          ServiceHelper.AddComponent((IGameComponent) new ScreenFade(this.Game)
          {
            FromColor = Color.White,
            ToColor = ColorEx.TransparentWhite,
            Duration = 0.4f
          });
        Trile trile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(this.ActorToSpawn));
        if (trile != null)
        {
          Vector3 position = this.TreasureCenter - Vector3.One / 2f;
          this.LevelManager.ClearTrile(new TrileEmplacement(position));
          TrileInstance toAdd;
          this.LevelManager.RestoreTrile(toAdd = new TrileInstance(position, trile.Id)
          {
            OriginalEmplacement = new TrileEmplacement(position)
          });
          toAdd.Foreign = true;
          if (toAdd.InstanceId == -1)
            this.LevelMaterializer.CullInstanceIn(toAdd);
        }
        else
          Logger.Log("Glitchy Despawner", Common.LogSeverity.Warning, "No secret cube trile in trileset!");
        this.hasCreatedTreasure = true;
      }
      if (this.SinceAlive.TotalSeconds <= 2.5)
        return;
      if (this.IsArtObject)
      {
        this.LevelManager.RemoveArtObject(this.AoInstance);
      }
      else
      {
        this.TrileInstance.PhysicsState = (InstancePhysicsState) null;
        this.LevelManager.ClearTrile(this.TrileInstance);
      }
      ServiceHelper.RemoveComponent<GlitchyDespawner>(this);
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
      if (this.SinceAlive.TotalSeconds > 2.0)
        this.TargetRenderer.DrawFullscreen(new Color(1f, 1f, 1f, 1f - this.SpawnMesh.Material.Opacity));
      else if (this.SinceAlive.TotalSeconds < 1.0)
      {
        this.TargetRenderer.DrawFullscreen(Color.White);
        this.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        this.TargetRenderer.DrawFullscreen((Texture) this.StarsTexture, textureMatrix, new Color(1f, 1f, 1f, alpha));
      }
      else if (this.SinceAlive.TotalSeconds > 1.0)
        this.TargetRenderer.DrawFullscreen(new Color(this.redVisible ? 1f : 0.0f, this.greenVisible ? 1f : 0.0f, this.blueVisible ? 1f : 0.0f, 1f));
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.None));
    }

    private void DrawLights()
    {
      BaseEffect effect = this.SpawnMesh.Effect;
      Texture texture = this.SpawnMesh.FirstGroup.Texture;
      this.SpawnMesh.FirstGroup.Texture = (Texture) null;
      this.SpawnMesh.Effect = (BaseEffect) this.FullbrightEffect;
      this.SpawnMesh.Draw();
      this.SpawnMesh.FirstGroup.Texture = texture;
      this.SpawnMesh.Effect = effect;
      if (this.SinceAlive.TotalSeconds <= 2.0)
        return;
      this.SpawnMesh.Material.Opacity = FezMath.Saturate((float) ((this.SinceAlive.TotalSeconds - 2.0) / 0.5));
      if (this.IsArtObject)
      {
        this.AoInstance.Material.Opacity = this.SpawnMesh.Material.Opacity;
        this.AoInstance.MarkDirty();
      }
      (this.SpawnMesh.Effect as CubemappedEffect).Pass = LightingEffectPass.Pre;
      this.SpawnMesh.Draw();
      (this.SpawnMesh.Effect as CubemappedEffect).Pass = LightingEffectPass.Main;
    }
  }
}
