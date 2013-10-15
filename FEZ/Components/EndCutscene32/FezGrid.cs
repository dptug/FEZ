// Type: FezGame.Components.EndCutscene32.FezGrid
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components.EndCutscene32
{
  internal class FezGrid : DrawableGameComponent
  {
    private const float SeparateDuration = 2f;
    private const float RotateZoomDuration = 12f;
    private const float GridFadeAlignDuration = 12f;
    private const float CubeRiseDuration = 12f;
    private readonly EndCutscene32Host Host;
    private Mesh GoMesh;
    private Group GomezGroup;
    private Group FezGroup;
    private Mesh TetraMesh;
    private Mesh CubeMesh;
    private Mesh StencilMesh;
    private float Time;
    private FezGrid.State ActiveState;

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public FezGrid(Game game, EndCutscene32Host host)
      : base(game)
    {
      this.Host = host;
      this.DrawOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      FezGrid fezGrid1 = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
      vertexColored1.Fullbright = true;
      DefaultEffect.VertexColored vertexColored2 = vertexColored1;
      mesh2.Effect = (BaseEffect) vertexColored2;
      Mesh mesh3 = mesh1;
      fezGrid1.GoMesh = mesh3;
      this.GomezGroup = this.GoMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, Color.White, true, false, false);
      this.FezGroup = this.GoMesh.AddFace(Vector3.One / 2f, Vector3.Zero, FaceOrientation.Front, Color.Red, true, false, false);
      FezGrid fezGrid2 = this;
      Mesh mesh4 = new Mesh();
      Mesh mesh5 = mesh4;
      DefaultEffect.VertexColored vertexColored3 = new DefaultEffect.VertexColored();
      vertexColored3.Fullbright = true;
      DefaultEffect.VertexColored vertexColored4 = vertexColored3;
      mesh5.Effect = (BaseEffect) vertexColored4;
      Mesh mesh6 = mesh4;
      fezGrid2.TetraMesh = mesh6;
      Vector3[] pointPairs = new Vector3[2560];
      for (int index = 0; index < 1280; ++index)
      {
        pointPairs[index * 2] = new Vector3(-640f, 0.0f, (float) (index - 640));
        pointPairs[index * 2 + 1] = new Vector3(640f, 0.0f, (float) (index - 640));
      }
      Color[] pointColors = Enumerable.ToArray<Color>(Enumerable.Repeat<Color>(Color.Red, 2560));
      this.TetraMesh.AddLines(pointColors, pointPairs, true);
      this.TetraMesh.AddLines(pointColors, Enumerable.ToArray<Vector3>(Enumerable.Select<Vector3, Vector3>((IEnumerable<Vector3>) pointPairs, (Func<Vector3, Vector3>) (v => new Vector3(v.Z, 0.0f, v.X)))), true);
      this.CubeMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.LitVertexColored()
      };
      this.CubeMesh.AddFlatShadedBox(Vector3.One, Vector3.Zero, Color.White, true);
      this.StencilMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        AlwaysOnTop = true,
        DepthWrites = false
      };
      this.StencilMesh.AddFace(FezMath.XZMask * 1280f, Vector3.Zero, FaceOrientation.Top, true);
      this.LevelManager.ActualAmbient = new Color(0.25f, 0.25f, 0.25f);
      this.LevelManager.ActualDiffuse = Color.White;
      this.Reset();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.TetraMesh != null)
        this.TetraMesh.Dispose();
      if (this.GoMesh != null)
        this.GoMesh.Dispose();
      if (this.CubeMesh != null)
        this.CubeMesh.Dispose();
      if (this.StencilMesh != null)
        this.StencilMesh.Dispose();
      this.TetraMesh = this.GoMesh = this.CubeMesh = this.StencilMesh = (Mesh) null;
    }

    private void Reset()
    {
      this.GomezGroup.Position = Vector3.Zero;
      this.FezGroup.Position = new Vector3(-0.25f, 0.75f, 0.0f);
      this.GoMesh.Scale = Vector3.One;
      this.FezGroup.Rotation = this.GomezGroup.Rotation = Quaternion.Identity;
      this.CameraManager.Center = Vector3.Zero;
      this.CameraManager.Direction = Vector3.UnitZ;
      this.CameraManager.Radius = 10f * SettingsManager.GetViewScale(this.GraphicsDevice);
      this.CameraManager.SnapInterpolation();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      this.Time += (float) gameTime.ElapsedGameTime.TotalSeconds;
      switch (this.ActiveState)
      {
        case FezGrid.State.Wait:
          this.Reset();
          if ((double) this.Time <= 2.0)
            break;
          this.ChangeState();
          break;
        case FezGrid.State.RotateZoom:
          float num1 = FezMath.Saturate(this.Time / 12f);
          float amount1 = num1;
          this.GomezGroup.Rotation = Quaternion.Slerp(Quaternion.Identity, Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 0.7853982f), amount1);
          this.FezGroup.Rotation = Quaternion.Slerp(Quaternion.Identity, Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 1.570796f), amount1);
          float amount2 = Easing.EaseOut((double) num1, EasingType.Cubic);
          this.GomezGroup.Position = Vector3.Lerp(Vector3.Zero, new Vector3(0.5f, -1f, 0.0f) / 2f, amount2);
          this.FezGroup.Position = Vector3.Lerp(new Vector3(-0.25f, 0.75f, 0.0f), new Vector3(-1.5f, 1.5f, 0.0f) / 2f, amount2);
          this.GoMesh.Scale = Vector3.Lerp(Vector3.One, new Vector3(40f), Easing.EaseIn((double) num1, EasingType.Quartic));
          this.CameraManager.Center = Vector3.Lerp(Vector3.Zero, Vector3.Transform(this.FezGroup.Position, this.GoMesh.WorldMatrix), Easing.EaseInOut((double) FezMath.Saturate(num1 * 2f), EasingType.Sine));
          this.CameraManager.SnapInterpolation();
          if ((double) this.Time <= 12.0)
            break;
          this.ChangeState();
          break;
        case FezGrid.State.GridFadeAlign:
          float num2 = FezMath.Saturate(this.Time / 12f);
          this.TetraMesh.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreatePerspectiveFieldOfView(MathHelper.Lerp(1.570796f, 2.356194f, FezMath.Saturate(FezMath.Saturate(num2 - 0.3f) / 0.6f)), this.CameraManager.AspectRatio, 0.1f, 2000f));
          float amount3 = Easing.EaseOut((double) num2, EasingType.Quintic);
          float amount4 = Easing.EaseInOut((double) FezMath.Saturate(FezMath.Saturate(num2 - 0.5f) / 0.5f), EasingType.Sine, EasingType.Sine);
          float step = Easing.EaseInOut((double) FezMath.Saturate(FezMath.Saturate(num2 - 0.4f) / 0.6f), EasingType.Sine, EasingType.Sine);
          Vector3 cameraPosition1 = Vector3.Lerp(Vector3.UnitY * 360f, Vector3.UnitY * 2f, amount3) + Vector3.UnitX * num2 * 100f;
          this.TetraMesh.Effect.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(cameraPosition1, Vector3.Lerp(cameraPosition1 - Vector3.UnitY, cameraPosition1 + Vector3.UnitX - Vector3.UnitY, amount4), FezMath.Slerp(new Vector3((float) Math.Sin((double) num2 * 3.14159274101257 * 0.699999988079071), 0.0f, (float) Math.Cos((double) num2 * 3.14159274101257 * 0.699999988079071)), Vector3.UnitY, step)));
          if ((double) this.Time <= 12.0)
            break;
          this.ChangeState();
          break;
        case FezGrid.State.CubeRise:
          float num3 = FezMath.Saturate(this.Time / 12f);
          this.CubeMesh.Position = this.CameraManager.Center;
          this.CubeMesh.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAt(Vector3.One, Vector3.Zero, Vector3.Up));
          this.CubeMesh.Scale = Vector3.Lerp(Vector3.One, Vector3.One * 2f, Easing.EaseIn((double) FezMath.Saturate(num3 - 0.6f) / 0.400000005960464, EasingType.Quadratic));
          this.TetraMesh.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.CreatePerspectiveFieldOfView(2.356194f, this.CameraManager.AspectRatio, 0.1f, 2000f));
          float amount5 = Easing.EaseIn((double) num3, EasingType.Quadratic);
          Vector3 cameraPosition2 = Vector3.UnitY * 2f + Vector3.UnitX * num3 * 100f;
          this.TetraMesh.Effect.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(cameraPosition2, cameraPosition2 + Vector3.UnitX + Vector3.UnitY * MathHelper.Lerp(-1f, 3f, amount5), Vector3.UnitY));
          this.StencilMesh.Effect.ForcedProjectionMatrix = this.TetraMesh.Effect.ForcedProjectionMatrix;
          this.StencilMesh.Effect.ForcedViewMatrix = this.TetraMesh.Effect.ForcedViewMatrix;
          if ((double) this.Time <= 12.0)
            break;
          this.ChangeState();
          break;
      }
    }

    private void ChangeState()
    {
      if (this.ActiveState == FezGrid.State.CubeRise)
      {
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
        case FezGrid.State.Wait:
        case FezGrid.State.RotateZoom:
          this.GoMesh.Draw();
          break;
        case FezGrid.State.GridFadeAlign:
          this.TetraMesh.Draw();
          break;
        case FezGrid.State.CubeRise:
          this.TetraMesh.Draw();
          GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.CutsceneWipe));
          GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
          this.StencilMesh.Draw();
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.NotEqual, StencilMask.CutsceneWipe);
          GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
          this.CubeMesh.Draw();
          GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
          break;
      }
    }

    private enum State
    {
      Wait,
      RotateZoom,
      GridFadeAlign,
      CubeRise,
    }
  }
}
