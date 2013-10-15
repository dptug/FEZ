// Type: FezGame.Components.EndCutscene32.AxisDna
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components.EndCutscene32
{
  internal class AxisDna : DrawableGameComponent
  {
    private const float AxisZoomDuration = 10f;
    private const float StrandZoomDuration = 7.42f;
    private const int StrandCount = 750;
    private const int PointCount = 100000;
    private readonly EndCutscene32Host Host;
    private float Time;
    private AxisDna.State ActiveState;
    private Mesh FatAxisMesh;
    private Mesh HelixMesh;
    private Mesh NoiseMesh;
    private Texture2D PurpleGradientTexture;
    private ShimmeringPointsEffect ShimmeringEffect;

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public AxisDna(Game game, EndCutscene32Host host)
      : base(game)
    {
      this.Host = host;
      this.DrawOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.Reset();
    }

    private void Reset()
    {
      this.FatAxisMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.VertexColored()
      };
      this.FatAxisMesh.AddColoredBox(new Vector3(1f, 1f, 10000f) / 200f, new Vector3(-0.5f, -0.5f, 0.0f) / 200f, Color.Blue, false);
      this.FatAxisMesh.Rotation = new Quaternion(-0.7408407f, -0.4897192f, 0.4504161f, 0.09191696f);
      this.FatAxisMesh.Scale = new Vector3(3f);
      this.FatAxisMesh.Position = new Vector3(7.574002f, 3.049632f, 5.773395f);
      if (this.HelixMesh != null && this.HelixMesh.Groups.Count == 1)
        (this.HelixMesh.FirstGroup.Geometry as BufferedIndexedPrimitives<FezVertexPositionColor>).Dispose();
      this.HelixMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.VertexColored()
      };
      for (int index = 0; index < 750; ++index)
      {
        float angle = (float) index / 1f;
        Group group = this.HelixMesh.AddColoredBox(new Vector3(0.2f, 70f, 0.2f) / 10000f, Vector3.Zero, EndCutscene32Host.PurpleBlack, true);
        group.Position = new Vector3(0.0f, 0.0f, (float) (((double) index - 375.0) / 3000.0));
        group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, angle);
      }
      this.HelixMesh.CollapseToBuffer<FezVertexPositionColor>();
      this.HelixMesh.Rotation = this.FatAxisMesh.Rotation;
      this.HelixMesh.Scale = this.FatAxisMesh.Scale;
      this.HelixMesh.Culling = CullMode.None;
      Random random = RandomHelper.Random;
      FezVertexPositionColor[] vertices = new FezVertexPositionColor[200000];
      int[] indices = new int[200000];
      for (int index = 0; index < 100000; ++index)
      {
        vertices[index * 2] = new FezVertexPositionColor(new Vector3((float) random.NextDouble() - 0.5f, (float) random.NextDouble() - 0.5f, 0.0f), new Color((int) (byte) random.Next(0, 256), (int) (byte) random.Next(0, 256), (int) (byte) random.Next(0, 256), 0));
        vertices[index * 2 + 1] = new FezVertexPositionColor(vertices[index * 2].Position, new Color((int) vertices[index * 2].Color.R, (int) vertices[index * 2].Color.G, (int) vertices[index * 2].Color.B, (int) byte.MaxValue));
        indices[index * 2] = index * 2;
        indices[index * 2 + 1] = index * 2 + 1;
      }
      this.NoiseMesh = new Mesh()
      {
        Effect = (BaseEffect) (this.ShimmeringEffect = new ShimmeringPointsEffect())
      };
      Group group1 = this.NoiseMesh.AddGroup();
      BufferedIndexedPrimitives<FezVertexPositionColor> indexedPrimitives = new BufferedIndexedPrimitives<FezVertexPositionColor>(vertices, indices, PrimitiveType.LineList);
      indexedPrimitives.UpdateBuffers();
      indexedPrimitives.CleanUp();
      group1.Geometry = (IIndexedPrimitiveCollection) indexedPrimitives;
      this.NoiseMesh.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(1f, 1f, 0.1f, 100f));
      this.NoiseMesh.Effect.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(-Vector3.UnitZ, Vector3.Zero, Vector3.Up));
      this.PurpleGradientTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/end_cutscene/purple_gradient");
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.HelixMesh != null)
        this.HelixMesh.Dispose();
      if (this.FatAxisMesh != null)
        this.FatAxisMesh.Dispose();
      this.HelixMesh = this.FatAxisMesh = this.NoiseMesh = (Mesh) null;
      this.ShimmeringEffect = (ShimmeringPointsEffect) null;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      this.Time += (float) gameTime.ElapsedGameTime.TotalSeconds;
      switch (this.ActiveState)
      {
        case AxisDna.State.AxisZoom:
          if ((double) this.Time == 0.0)
          {
            this.CameraManager.Center = Vector3.Zero;
            this.CameraManager.Direction = Vector3.UnitZ;
            this.CameraManager.Radius = 10f;
            this.CameraManager.SnapInterpolation();
            this.LevelManager.ActualAmbient = new Color(0.25f, 0.25f, 0.25f);
            this.LevelManager.ActualDiffuse = Color.White;
            this.NoiseMesh.Scale = Vector3.One;
          }
          float num = FezMath.Saturate(this.Time / 10f);
          this.CameraManager.Direction = FezMath.Slerp(Vector3.UnitZ, -Vector3.Transform(Vector3.UnitZ, this.HelixMesh.Rotation), num * 0.5f);
          this.FatAxisMesh.Material.Opacity = 1f - Easing.EaseIn((double) FezMath.Saturate(num * 2f), EasingType.Quadratic);
          if ((double) this.Time != 0.0)
            this.FatAxisMesh.Scale *= MathHelper.Lerp(1.015f, 1.01f, Easing.EaseIn((double) num, EasingType.Quadratic));
          this.HelixMesh.Scale = this.FatAxisMesh.Scale;
          this.CameraManager.Center = new Vector3(0.0f, this.FatAxisMesh.Scale.X / 1000f, 0.0f);
          this.CameraManager.SnapInterpolation();
          this.HelixMesh.Rotation = this.HelixMesh.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, Easing.EaseOut(0.0149999996647239 * (1.0 - (double) num), EasingType.Cubic));
          this.NoiseMesh.Material.Opacity = Easing.EaseIn((double) num, EasingType.Cubic);
          this.NoiseMesh.Scale *= 1.0001f;
          if ((double) num != 1.0)
            break;
          this.ChangeState();
          break;
        case AxisDna.State.StrandZoom:
          float amount = FezMath.Saturate(this.Time / 7.42f);
          if ((double) this.Time != 0.0 && (double) amount != 1.0)
            this.HelixMesh.Scale *= 1.01f;
          this.HelixMesh.Position = (float) (((double) this.HelixMesh.Scale.X - 7400.0) / 1000.0) * this.CameraManager.Direction;
          this.CameraManager.Center = Vector3.Lerp(new Vector3(0.0f, this.HelixMesh.Scale.X / 1000f, 0.0f), new Vector3((float) (-(double) this.HelixMesh.Scale.X / 90000.0), this.HelixMesh.Scale.X / 1100f, 0.0f), amount);
          this.CameraManager.SnapInterpolation();
          this.NoiseMesh.Scale *= MathHelper.Lerp(1.0001f, 1.0025f, amount);
          this.ShimmeringEffect.Saturation = (float) (1.0 - (double) amount * 0.5);
          if ((double) amount != 1.0)
            break;
          this.ChangeState();
          break;
      }
    }

    private void ChangeState()
    {
      if (this.ActiveState == AxisDna.State.StrandZoom)
      {
        foreach (DrawableGameComponent drawableGameComponent in this.Host.Scenes)
        {
          if (drawableGameComponent is TetraordialOoze)
            (drawableGameComponent as TetraordialOoze).NoiseMesh = this.NoiseMesh;
        }
        this.Host.Cycle();
      }
      else
      {
        this.Time = 0.0f;
        ++this.ActiveState;
        this.Update(new GameTime());
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      this.GraphicsDevice.Clear(EndCutscene32Host.PurpleBlack);
      switch (this.ActiveState)
      {
        case AxisDna.State.AxisZoom:
          GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.CutsceneWipe));
          this.HelixMesh.Draw();
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.CutsceneWipe);
          this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
          this.TargetRenderer.DrawFullscreen((Texture) this.PurpleGradientTexture);
          this.NoiseMesh.Draw();
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
          this.FatAxisMesh.Draw();
          break;
        case AxisDna.State.StrandZoom:
          GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.CutsceneWipe));
          this.HelixMesh.Draw();
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.CutsceneWipe);
          this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
          this.TargetRenderer.DrawFullscreen((Texture) this.PurpleGradientTexture, new Color(1f, 1f, 1f, 1f - FezMath.Saturate(this.Time / 7.42f)));
          this.NoiseMesh.Draw();
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
          break;
      }
    }

    private enum State
    {
      AxisZoom,
      StrandZoom,
    }
  }
}
