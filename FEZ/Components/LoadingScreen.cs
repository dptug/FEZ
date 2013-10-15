// Type: FezGame.Components.LoadingScreen
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

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
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  public class LoadingScreen : DrawableGameComponent
  {
    private static readonly object EffectRefreshMutex = new object();
    private const float FadeTime = 0.5f;
    private Mesh mesh;
    private float sinceBgShown;
    private float sinceCubeShown;
    private float bgOpacity;
    private float cubeOpacity;
    private Texture2D starBack;
    private FakeDot fakeDot;
    private SoundEffect sDrone;
    private SoundEffectInstance iDrone;

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IDotManager DotManager { get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    static LoadingScreen()
    {
    }

    public LoadingScreen(Game game)
      : base(game)
    {
      this.DrawOrder = 2100;
    }

    protected override void LoadContent()
    {
      TrileSet trileSet = this.CMProvider.Global.Load<TrileSet>("Trile Sets/LOADING");
      float viewAspect = 1.777778f;
      LoadingScreen loadingScreen = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.LitTextured litTextured1 = new DefaultEffect.LitTextured();
      litTextured1.Specular = true;
      litTextured1.AlphaIsEmissive = true;
      litTextured1.Emissive = 0.5f;
      litTextured1.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(14f * viewAspect, 14f, 0.1f, 100f));
      litTextured1.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10f), Vector3.Zero, Vector3.Up));
      DefaultEffect.LitTextured litTextured2 = litTextured1;
      mesh2.Effect = (BaseEffect) litTextured2;
      mesh1.Blending = new BlendingMode?(BlendingMode.Alphablending);
      mesh1.SamplerState = SamplerState.PointClamp;
      mesh1.AlwaysOnTop = false;
      mesh1.DepthWrites = true;
      mesh1.Texture = (Dirtyable<Texture>) ((Texture) trileSet.TextureAtlas);
      mesh1.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0))) * Quaternion.CreateFromAxisAngle(Vector3.Up, 0.7853982f);
      mesh1.Position = new Vector3(5.5f * viewAspect, -4.5f, 0.0f);
      Mesh mesh3 = mesh1;
      loadingScreen.mesh = mesh3;
      Group group = this.mesh.AddGroup();
      ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry = trileSet.Triles[0].Geometry;
      group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(Enumerable.ToArray<VertexPositionNormalTextureInstance>((IEnumerable<VertexPositionNormalTextureInstance>) geometry.Vertices), geometry.Indices, geometry.PrimitiveType);
      group.Scale = new Vector3(0.95f);
      this.starBack = this.CMProvider.Global.Load<Texture2D>("Other Textures/hud/starback");
      this.sDrone = this.CMProvider.Global.Load<SoundEffect>("Sounds/Intro/FezLogoDrone");
      ServiceHelper.AddComponent((IGameComponent) (this.fakeDot = new FakeDot(ServiceHelper.Game)));
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        lock (LoadingScreen.EffectRefreshMutex)
        {
          if (this.mesh.Effect != null)
            this.mesh.Effect.Dispose();
          if (this.LevelManager.WaterType == LiquidType.Sewer || this.LevelManager.WaterType == LiquidType.Lava || this.LevelManager.BlinkingAlpha)
          {
            Mesh temp_131 = this.mesh;
            DefaultEffect.Textured temp_135 = new DefaultEffect.Textured()
            {
              AlphaIsEmissive = true
            };
            temp_131.Effect = (BaseEffect) temp_135;
          }
          else
          {
            Mesh temp_179 = this.mesh;
            DefaultEffect.LitTextured temp_187 = new DefaultEffect.LitTextured()
            {
              Specular = true,
              Emissive = 0.5f,
              AlphaIsEmissive = true
            };
            temp_179.Effect = (BaseEffect) temp_187;
          }
          this.mesh.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(14f * viewAspect, 14f, 0.1f, 100f));
          this.mesh.Effect.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10f), Vector3.Zero, Vector3.Up));
          this.mesh.TextureMatrix.Dirty = true;
        }
      });
    }

    private void CreateDrone()
    {
      this.iDrone = this.sDrone.CreateInstance(true);
      this.iDrone.IsLooped = true;
      this.iDrone.Volume = 0.0f;
      this.iDrone.Play();
    }

    private void KillDrone()
    {
      Waiters.Interpolate(1.0, (Action<float>) (s => this.iDrone.Volume = FezMath.Saturate(1f - s)), (Action) (() =>
      {
        this.iDrone.Stop(false);
        this.iDrone.Dispose();
        this.iDrone = (SoundEffectInstance) null;
      }));
      foreach (SoundEmitter soundEmitter in this.SoundManager.Emitters)
        soundEmitter.VolumeMaster = 1f;
      this.SoundManager.MusicVolumeFactor = 1f;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.SkipLoadScreen)
        return;
      if (this.GameState.SkipLoadBackground)
        this.sinceBgShown = 0.0f;
      float num1 = (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (this.GameState.Loading && (!this.GameState.ScheduleLoadEnd || this.GameState.DotLoading) || this.GameState.ForceLoadIcon)
      {
        if (!this.GameState.SkipLoadBackground && (double) this.bgOpacity < 1.0)
          this.sinceBgShown += num1;
        if ((double) this.cubeOpacity < 1.0 && !this.GameState.DotLoading)
          this.sinceCubeShown += num1;
      }
      else
      {
        if (!this.GameState.SkipLoadBackground && (double) this.bgOpacity > 0.0)
          this.sinceBgShown -= num1 * 1.25f;
        if ((double) this.cubeOpacity > 0.0)
          this.sinceCubeShown -= num1 * 1.25f;
      }
      float num2 = this.bgOpacity;
      this.bgOpacity = FezMath.Saturate(this.sinceBgShown / 0.5f);
      if ((double) num2 == 1.0 && (double) this.bgOpacity < (double) num2 && this.iDrone != null)
        this.KillDrone();
      this.cubeOpacity = FezMath.Saturate(this.sinceCubeShown / 0.5f);
    }

    public override void Draw(GameTime gameTime)
    {
      this.GameState.LoadingVisible = false;
      if (!this.GameState.DotLoading && (double) this.cubeOpacity == 0.0 || (this.GameState.SkipLoadScreen || this.GameState.FarawaySettings.InTransition))
        return;
      this.GameState.LoadingVisible = true;
      if (!this.GameState.SkipLoadBackground)
      {
        if (this.GameState.DotLoading)
          this.TargetRenderer.DrawFullscreen((Texture) this.starBack, new Color(1f, 1f, 1f, this.bgOpacity));
        else
          this.TargetRenderer.DrawFullscreen(new Color(0.0f, 0.0f, 0.0f, this.bgOpacity));
      }
      if (!this.GameState.DotLoading)
      {
        this.GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1f, 0);
        this.mesh.Material.Opacity = this.cubeOpacity;
        this.mesh.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) (gameTime.ElapsedGameTime.TotalSeconds * 3.0)) * this.mesh.Rotation;
        this.mesh.FirstGroup.Position = new Vector3(0.0f, (float) (Math.Sin(gameTime.TotalGameTime.TotalSeconds * 3.14159274101257) * 0.200000002980232), 0.0f);
        lock (LoadingScreen.EffectRefreshMutex)
          this.mesh.Draw();
      }
      else
      {
        if (this.iDrone == null)
          this.CreateDrone();
        this.fakeDot.Opacity = this.bgOpacity;
        this.fakeDot.Update(gameTime);
        this.fakeDot.Draw(gameTime);
        this.iDrone.Volume = this.bgOpacity;
        this.SoundManager.MusicVolumeFactor = !this.GameState.ScheduleLoadEnd ? Math.Min(this.SoundManager.MusicVolumeFactor, 1f - this.bgOpacity) : 1f - this.bgOpacity;
        if (!this.GameState.ScheduleLoadEnd)
          return;
        foreach (SoundEmitter soundEmitter in this.SoundManager.Emitters)
        {
          if (!soundEmitter.Dead)
          {
            soundEmitter.VolumeMaster = 1f - this.bgOpacity;
            soundEmitter.Update();
          }
        }
      }
    }
  }
}
