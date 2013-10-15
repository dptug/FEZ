// Type: FezGame.Components.StargateHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  internal class StargateHost : DrawableGameComponent
  {
    private readonly List<bool> AoVisibility = new List<bool>();
    private readonly Texture2D[] OriginalTextures = new Texture2D[4];
    private float SinceStarted;
    private float SpinSpeed;
    private ArtObjectInstance[] Rings;
    private Texture2D WhiteTex;
    private Mesh TrialRaysMesh;
    private Mesh TrialFlareMesh;
    private float TrialTimeAccumulator;
    private StargateHost.MaskRenderer maskRenderer;

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public StargateHost(Game game)
      : base(game)
    {
      this.DrawOrder = 200;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.WhiteTex = this.CMProvider.Global.Load<Texture2D>("Other Textures/FullWhite");
      this.Enabled = this.Visible = false;
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
    }

    private void TryInitialize()
    {
      this.Rings = (ArtObjectInstance[]) null;
      if (this.TrialRaysMesh != null)
        this.TrialRaysMesh.Dispose();
      if (this.TrialFlareMesh != null)
        this.TrialFlareMesh.Dispose();
      this.TrialRaysMesh = this.TrialFlareMesh = (Mesh) null;
      this.TrialTimeAccumulator = this.SinceStarted = 0.0f;
      this.SpinSpeed = 0.0f;
      if (this.maskRenderer != null)
      {
        ServiceHelper.RemoveComponent<StargateHost.MaskRenderer>(this.maskRenderer);
        this.maskRenderer = (StargateHost.MaskRenderer) null;
      }
      this.Enabled = this.Visible = this.LevelManager.Name == "STARGATE";
      if (!this.Enabled)
        return;
      this.TrialRaysMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/smooth_ray")),
        Blending = new BlendingMode?(BlendingMode.Additive),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      this.TrialFlareMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/flare_alpha")),
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SamplerState = SamplerState.AnisotropicClamp,
        DepthWrites = false,
        AlwaysOnTop = true
      };
      this.TrialFlareMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Right, true);
      this.Rings = new ArtObjectInstance[4]
      {
        this.LevelManager.ArtObjects[5],
        this.LevelManager.ArtObjects[6],
        this.LevelManager.ArtObjects[7],
        this.LevelManager.ArtObjects[8]
      };
      foreach (ArtObjectInstance artObjectInstance in this.Rings)
        artObjectInstance.Material = new Material();
      ServiceHelper.AddComponent((IGameComponent) (this.maskRenderer = new StargateHost.MaskRenderer(this.Game)));
      this.maskRenderer.Center = this.Rings[0].Position;
      if (this.GameState.SaveData.ThisLevel.InactiveArtObjects.Contains(0))
      {
        this.Enabled = false;
        this.LevelManager.Scripts[4].Disabled = false;
        this.LevelManager.Scripts[5].Disabled = false;
        foreach (ArtObjectInstance aoInstance in this.Rings)
          this.LevelManager.RemoveArtObject(aoInstance);
      }
      else
      {
        this.LevelManager.Scripts[4].Disabled = true;
        this.LevelManager.Scripts[5].Disabled = true;
        this.maskRenderer.Visible = false;
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || this.GameState.InMenuCube))
        return;
      this.SinceStarted += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.SinceStarted > 8.0 && (double) this.SinceStarted < 19.0)
        this.SpinSpeed = Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.SinceStarted - 8.0) / 5.0)), EasingType.Sine) * 0.005f;
      else if ((double) this.SinceStarted > 19.0)
        this.SpinSpeed = (float) (0.00499999988824129 + (double) Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.SinceStarted - 19.0) / 20.0)), EasingType.Quadratic) * 0.5);
      if ((double) this.SinceStarted > 33.0 && this.Rings != null)
      {
        this.TrialTimeAccumulator += (float) gameTime.ElapsedGameTime.TotalSeconds;
        this.UpdateRays((float) gameTime.ElapsedGameTime.TotalSeconds);
      }
      if (this.Rings == null)
        return;
      this.Rings[0].Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, this.SpinSpeed) * this.Rings[0].Rotation;
      this.Rings[1].Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, this.SpinSpeed) * this.Rings[1].Rotation;
      this.Rings[2].Rotation = Quaternion.CreateFromAxisAngle(Vector3.Left, this.SpinSpeed) * this.Rings[2].Rotation;
      this.Rings[3].Rotation = Quaternion.CreateFromAxisAngle(Vector3.Down, this.SpinSpeed) * this.Rings[3].Rotation;
    }

    private void UpdateRays(float elapsedSeconds)
    {
      if (this.TrialRaysMesh.Groups.Count < 50 && RandomHelper.Probability(0.2))
      {
        float x = 6f + RandomHelper.Centered(4.0);
        float num = RandomHelper.Between(0.5, (double) x / 2.5);
        Group group = this.TrialRaysMesh.AddGroup();
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
      for (int i = this.TrialRaysMesh.Groups.Count - 1; i >= 0; --i)
      {
        Group group = this.TrialRaysMesh.Groups[i];
        DotHost.RayState rayState = group.CustomData as DotHost.RayState;
        rayState.Age += elapsedSeconds * 0.15f;
        float num1 = Easing.EaseOut((double) Easing.EaseOut(Math.Sin((double) rayState.Age * 6.28318548202515 - 1.57079637050629) * 0.5 + 0.5, EasingType.Quintic), EasingType.Quintic);
        group.Material.Diffuse = Vector3.Lerp(Vector3.One, rayState.Tint.ToVector3(), 0.05f) * 0.15f * num1;
        float num2 = rayState.Speed;
        group.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Forward, (float) ((double) elapsedSeconds * (double) num2 * (0.100000001490116 + (double) Easing.EaseIn((double) this.TrialTimeAccumulator / 3.0, EasingType.Quadratic) * 0.200000002980232)));
        group.Scale = new Vector3((float) ((double) num1 * 0.75 + 0.25), (float) ((double) num1 * 0.5 + 0.5), 1f);
        if ((double) rayState.Age > 1.0)
          this.TrialRaysMesh.RemoveGroupAt(i);
      }
      this.TrialFlareMesh.Position = this.TrialRaysMesh.Position = this.Rings[0].Position;
      this.TrialFlareMesh.Rotation = this.TrialRaysMesh.Rotation = this.CameraManager.Rotation;
      this.TrialRaysMesh.Scale = new Vector3(Easing.EaseIn((double) this.TrialTimeAccumulator / 2.0, EasingType.Quadratic) + 1f);
      this.TrialFlareMesh.Material.Opacity = (float) (0.125 + (double) Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.TrialTimeAccumulator - 2.0) / 3.0)), EasingType.Cubic) * 0.875);
      this.TrialFlareMesh.Scale = Vector3.One + this.TrialRaysMesh.Scale * Easing.EaseIn((double) Math.Max(this.TrialTimeAccumulator - 2.5f, 0.0f) / 1.5, EasingType.Cubic) * 4f;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      if ((double) this.SinceStarted > 19.0 && this.Rings != null)
      {
        this.AoVisibility.Clear();
        foreach (ArtObjectInstance artObjectInstance in this.LevelMaterializer.LevelArtObjects)
        {
          this.AoVisibility.Add(artObjectInstance.Visible);
          artObjectInstance.Visible = false;
          artObjectInstance.ArtObject.Group.Enabled = false;
        }
        for (int index = 0; index < 4; ++index)
        {
          ArtObjectInstance artObjectInstance = this.Rings[index];
          this.OriginalTextures[index] = artObjectInstance.ArtObject.Group.TextureMap;
          artObjectInstance.Visible = true;
          artObjectInstance.ArtObject.Group.Enabled = true;
          artObjectInstance.ArtObject.Group.Texture = (Texture) this.WhiteTex;
          artObjectInstance.Material.Opacity = Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.SinceStarted - 19.0) / 18.0)), EasingType.Cubic);
          artObjectInstance.Update();
        }
        this.LevelMaterializer.ArtObjectsMesh.Draw();
        for (int index = 0; index < 4; ++index)
        {
          this.Rings[index].ArtObject.Group.Texture = (Texture) this.OriginalTextures[index];
          this.OriginalTextures[index] = (Texture2D) null;
          this.Rings[index].Material.Opacity = 1f;
        }
        int num = 0;
        foreach (ArtObjectInstance artObjectInstance in this.LevelMaterializer.LevelArtObjects)
        {
          artObjectInstance.Visible = this.AoVisibility[num++];
          if (artObjectInstance.Visible)
            artObjectInstance.ArtObject.Group.Enabled = true;
        }
      }
      if ((double) this.SinceStarted > 36.75)
      {
        if (this.Rings != null)
        {
          foreach (ArtObjectInstance aoInstance in this.Rings)
            this.LevelManager.RemoveArtObject(aoInstance);
          this.maskRenderer.Visible = true;
          this.LevelManager.Scripts[4].Disabled = false;
          this.LevelManager.Scripts[5].Disabled = false;
          this.GameState.SaveData.ThisLevel.InactiveArtObjects.Add(0);
        }
        this.Rings = (ArtObjectInstance[]) null;
        float num = Easing.EaseIn(1.0 - (double) FezMath.Saturate((float) (((double) this.SinceStarted - 36.75) / 6.0)), EasingType.Sine);
        if (FezMath.AlmostEqual(num, 0.0f))
          return;
        this.TargetRenderer.DrawFullscreen(new Color(1f, 1f, 1f, num));
      }
      else
      {
        if ((double) this.SinceStarted <= 33.0)
          return;
        this.TargetRenderer.DrawFullscreen(new Color(1f, 1f, 1f, FezMath.Saturate(Easing.EaseIn(((double) this.TrialTimeAccumulator - 3.0) / 0.75, EasingType.Quintic))));
        this.TrialRaysMesh.Draw();
        this.TrialFlareMesh.Draw();
      }
    }

    private class MaskRenderer : DrawableGameComponent
    {
      private Texture2D PyramidWarp;
      private Mesh PyramidMask;
      private Mesh WarpCube;
      public Vector3 Center;

      [ServiceDependency]
      public IGameStateManager GameState { private get; set; }

      [ServiceDependency]
      public IContentManagerProvider CMProvider { private get; set; }

      [ServiceDependency]
      public IGameCameraManager CameraManager { private get; set; }

      public MaskRenderer(Game game)
        : base(game)
      {
        this.DrawOrder = 6;
      }

      protected override void LoadContent()
      {
        base.LoadContent();
        this.PyramidWarp = this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/warp/pyramid");
        this.PyramidMask = new Mesh()
        {
          Effect = (BaseEffect) new DefaultEffect.Textured(),
          DepthWrites = false
        };
        this.PyramidMask.AddFace(new Vector3(8f), Vector3.Zero, FaceOrientation.Front, true, true);
        this.WarpCube = new Mesh()
        {
          Effect = (BaseEffect) new DefaultEffect.Textured(),
          DepthWrites = false,
          Texture = (Dirtyable<Texture>) ((Texture) this.PyramidWarp)
        };
        this.WarpCube.AddFace(new Vector3(16f), 8f * Vector3.UnitZ, FaceOrientation.Front, true, false);
        this.WarpCube.AddFace(new Vector3(16f), 8f * Vector3.Right, FaceOrientation.Right, true, false);
        this.WarpCube.AddFace(new Vector3(16f), 8f * Vector3.Left, FaceOrientation.Left, true, false);
        this.WarpCube.AddFace(new Vector3(16f), 8f * -Vector3.UnitZ, FaceOrientation.Back, true, false);
      }

      protected override void Dispose(bool disposing)
      {
        base.Dispose(disposing);
        if (this.PyramidMask != null)
          this.PyramidMask.Dispose();
        this.PyramidMask = (Mesh) null;
        if (this.WarpCube != null)
          this.WarpCube.Dispose();
        this.WarpCube = (Mesh) null;
        this.PyramidWarp = (Texture2D) null;
      }

      public override void Draw(GameTime gameTime)
      {
        if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || this.GameState.InMenuCube))
          return;
        Vector3 a = this.Center - this.CameraManager.InterpolatedCenter;
        Vector2 vector2 = FezMath.Dot(a, this.CameraManager.View.Right) * Vector2.UnitX + a.Y * Vector2.UnitY;
        this.WarpCube.Position = this.PyramidMask.Position = this.Center;
        this.WarpCube.TextureMatrix = (Dirtyable<Matrix>) new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, vector2.X / 48f, (float) (-(double) vector2.Y / 48.0 + 0.100000001490116), 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.WarpGate));
        this.PyramidMask.Draw();
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.WarpGate);
        this.WarpCube.Draw();
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      }
    }
  }
}
