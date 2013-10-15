// Type: FezGame.Components.EndCutscene64.DotsAplenty
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezGame.Components.EndCutscene64
{
  internal class DotsAplenty : DrawableGameComponent
  {
    private const int Rows = 54;
    private Mesh DotMesh;
    private Mesh CloneMesh;
    private InstancedDotEffect DotEffect;
    private float Theta;
    private Texture2D NoiseTexture;
    private SoundEffect sAppear;
    private SoundEffect sStartMove;
    private SoundEffect sProgressiveAppear;
    private SoundEffect sNoise;
    private SoundEmitter eNoise;
    private bool sMovePlayed;
    private bool sProgPlayed;
    private float EightShapeStep;
    private VignetteEffect VignetteEffect;
    private ScanlineEffect ScanlineEffect;
    private RenderTargetHandle RtHandle;
    private readonly EndCutscene64Host Host;
    private float StepTime;
    private DotsAplenty.State ActiveState;
    private Matrix NoiseOffset;

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    public DotsAplenty(Game game, EndCutscene64Host host)
      : base(game)
    {
      this.Host = host;
      this.DrawOrder = 1000;
      this.UpdateOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.MakeDot();
      this.RtHandle = this.TargetRenderer.TakeTarget();
      this.VignetteEffect = new VignetteEffect();
      this.ScanlineEffect = new ScanlineEffect();
      this.NoiseTexture = this.CMProvider.Get(CM.EndCutscene).Load<Texture2D>("Other Textures/noise");
      this.sAppear = this.CMProvider.Get(CM.EndCutscene).Load<SoundEffect>("Sounds/Ending/Cutscene64/DotAppear");
      this.sStartMove = this.CMProvider.Get(CM.EndCutscene).Load<SoundEffect>("Sounds/Ending/Cutscene64/DotStartMove");
      this.sProgressiveAppear = this.CMProvider.Get(CM.EndCutscene).Load<SoundEffect>("Sounds/Ending/Cutscene64/DotsProgressiveAppear");
      this.sNoise = this.CMProvider.Get(CM.EndCutscene).Load<SoundEffect>("Sounds/Ending/Cutscene64/WhiteNoise");
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.TargetRenderer.ReturnTarget(this.RtHandle);
      this.RtHandle = (RenderTargetHandle) null;
      this.CloneMesh.Dispose();
      this.DotMesh.Dispose();
      this.CloneMesh = this.DotMesh = (Mesh) null;
      this.VignetteEffect.Dispose();
      this.VignetteEffect = (VignetteEffect) null;
      this.ScanlineEffect.Dispose();
      this.ScanlineEffect = (ScanlineEffect) null;
      this.DotEffect = (InstancedDotEffect) null;
    }

    private void MakeDot()
    {
      this.CloneMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        DepthWrites = false,
        Culling = CullMode.None,
        AlwaysOnTop = true,
        SamplerState = SamplerState.PointWrap
      };
      for (int index1 = -10; index1 <= 10; ++index1)
      {
        for (int index2 = -10; index2 <= 10; ++index2)
          this.CloneMesh.AddFace(new Vector3(600f, 337.5f, 1f), Vector3.Zero, FaceOrientation.Front, true).Position = new Vector3((float) (index2 * 575), (float) ((double) (index1 * 575) / 16.0 * 9.0), 0.0f);
      }
      this.CloneMesh.CollapseToBuffer<FezVertexPositionNormalTexture>();
      this.DotMesh = new Mesh()
      {
        Effect = (BaseEffect) (this.DotEffect = new InstancedDotEffect()),
        Blending = new BlendingMode?(BlendingMode.Additive),
        DepthWrites = false,
        Culling = CullMode.None,
        AlwaysOnTop = true,
        Material = {
          Opacity = 0.4f
        }
      };
      Group group = this.DotMesh.AddGroup();
      ShaderInstancedIndexedPrimitives<VertexPosition4ColorInstance, Vector4> indexedPrimitives = new ShaderInstancedIndexedPrimitives<VertexPosition4ColorInstance, Vector4>(PrimitiveType.TriangleList, 100);
      group.Geometry = (IIndexedPrimitiveCollection) indexedPrimitives;
      List<Vector4> list = new List<Vector4>()
      {
        new Vector4(-1f, -1f, -1f, -1f),
        new Vector4(1f, -1f, -1f, -1f),
        new Vector4(-1f, 1f, -1f, -1f),
        new Vector4(1f, 1f, -1f, -1f),
        new Vector4(-1f, -1f, 1f, -1f),
        new Vector4(1f, -1f, 1f, -1f),
        new Vector4(-1f, 1f, 1f, -1f),
        new Vector4(1f, 1f, 1f, -1f),
        new Vector4(-1f, -1f, -1f, 1f),
        new Vector4(1f, -1f, -1f, 1f),
        new Vector4(-1f, 1f, -1f, 1f),
        new Vector4(1f, 1f, -1f, 1f),
        new Vector4(-1f, -1f, 1f, 1f),
        new Vector4(1f, -1f, 1f, 1f),
        new Vector4(-1f, 1f, 1f, 1f),
        new Vector4(1f, 1f, 1f, 1f)
      };
      int[] numArray = new int[96]
      {
        0,
        2,
        3,
        1,
        1,
        3,
        7,
        5,
        5,
        7,
        6,
        4,
        4,
        6,
        2,
        0,
        0,
        4,
        5,
        1,
        2,
        6,
        7,
        3,
        8,
        10,
        11,
        9,
        9,
        11,
        15,
        13,
        13,
        15,
        14,
        12,
        12,
        14,
        10,
        8,
        8,
        12,
        13,
        9,
        10,
        14,
        15,
        11,
        0,
        1,
        9,
        8,
        0,
        2,
        10,
        8,
        2,
        3,
        11,
        10,
        3,
        1,
        9,
        11,
        4,
        5,
        13,
        12,
        6,
        7,
        15,
        14,
        4,
        6,
        14,
        12,
        5,
        7,
        15,
        13,
        4,
        0,
        8,
        12,
        6,
        2,
        10,
        14,
        3,
        7,
        15,
        11,
        1,
        5,
        13,
        9
      };
      indexedPrimitives.Vertices = new VertexPosition4ColorInstance[96];
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 6; ++index2)
        {
          Vector3 vector3 = Vector3.Zero;
          switch ((index2 + index1 * 6) % 6)
          {
            case 0:
              vector3 = new Vector3(0.0f, 1f, 0.75f);
              break;
            case 1:
              vector3 = new Vector3(0.1666667f, 1f, 0.75f);
              break;
            case 2:
              vector3 = new Vector3(0.3333333f, 1f, 0.75f);
              break;
            case 3:
              vector3 = new Vector3(0.5f, 1f, 0.75f);
              break;
            case 4:
              vector3 = new Vector3(0.6666667f, 1f, 0.75f);
              break;
            case 5:
              vector3 = new Vector3(0.8333333f, 1f, 0.75f);
              break;
          }
          for (int index3 = 0; index3 < 4; ++index3)
          {
            int index4 = index3 + index2 * 4 + index1 * 24;
            indexedPrimitives.Vertices[index4].Color = new Color(vector3.X, vector3.Y, vector3.Z);
            indexedPrimitives.Vertices[index4].Position = list[numArray[index4]];
          }
        }
      }
      indexedPrimitives.Indices = new int[144]
      {
        0,
        2,
        1,
        0,
        3,
        2,
        4,
        6,
        5,
        4,
        7,
        6,
        8,
        10,
        9,
        8,
        11,
        10,
        12,
        14,
        13,
        12,
        15,
        14,
        16,
        17,
        18,
        16,
        18,
        19,
        20,
        22,
        21,
        20,
        23,
        22,
        24,
        26,
        25,
        24,
        27,
        26,
        28,
        30,
        29,
        28,
        31,
        30,
        32,
        34,
        33,
        32,
        35,
        34,
        36,
        38,
        37,
        36,
        39,
        38,
        40,
        41,
        42,
        40,
        42,
        43,
        44,
        46,
        45,
        44,
        47,
        46,
        48,
        50,
        49,
        48,
        51,
        50,
        52,
        54,
        53,
        52,
        55,
        54,
        56,
        58,
        57,
        56,
        59,
        58,
        60,
        62,
        61,
        60,
        63,
        62,
        64,
        65,
        66,
        64,
        66,
        67,
        68,
        70,
        69,
        68,
        71,
        70,
        72,
        74,
        73,
        72,
        75,
        74,
        76,
        78,
        77,
        76,
        79,
        78,
        80,
        82,
        81,
        80,
        83,
        82,
        84,
        86,
        85,
        84,
        87,
        86,
        88,
        89,
        90,
        88,
        90,
        91,
        92,
        94,
        93,
        92,
        95,
        94
      };
      int num1 = (int) Math.Floor(96.0);
      int length = 54 * num1;
      indexedPrimitives.Instances = new Vector4[length];
      int num2 = (int) Math.Floor(27.0);
      int num3 = (int) Math.Floor((double) num1 / 2.0);
      Random random = RandomHelper.Random;
      for (int index1 = -num2; index1 < num2; ++index1)
      {
        for (int index2 = -num3; index2 < num3; ++index2)
        {
          int index3 = (index1 + num2) * num1 + (index2 + num3);
          indexedPrimitives.Instances[index3] = new Vector4((float) (index2 * 6), (float) (index1 * 6 + (Math.Abs(index2) % 2 == 0 ? 0 : 3)), 0.0f, index2 != 0 || index1 != 0 ? (float) (random.NextDouble() * 3.14159274101257 * 2.0) : 0.0f);
        }
      }
      indexedPrimitives.InstanceCount = length;
      indexedPrimitives.UpdateBuffers();
    }

    private void Reset()
    {
      this.CameraManager.Center = Vector3.Zero;
      this.CameraManager.Direction = Vector3.UnitZ;
      this.CameraManager.Radius = 1.0 / 16.0;
      this.CameraManager.SnapInterpolation();
      this.DotMesh.Position = Vector3.Zero;
      this.DotMesh.Scale = Vector3.One;
      this.sMovePlayed = this.sProgPlayed = false;
      if (this.eNoise != null && !this.eNoise.Dead)
        this.eNoise.FadeOutAndDie(0.0f);
      this.eNoise = (SoundEmitter) null;
      this.StepTime = 0.0f;
      this.Theta = 0.0f;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      float elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) elapsed == 0.0 || (double) this.StepTime == 0.0)
      {
        this.Reset();
        SoundEffectExtensions.Emit(this.sAppear);
      }
      this.StepTime += elapsed;
      if ((double) this.CameraManager.Radius < 11000.0)
      {
        IGameCameraManager cameraManager = this.CameraManager;
        double num = (double) cameraManager.Radius * 1.00625002384186;
        cameraManager.Radius = (float) num;
        this.CameraManager.SnapInterpolation();
      }
      if (!this.sMovePlayed && (double) this.StepTime >= 4.0)
      {
        SoundEffectExtensions.Emit(this.sStartMove);
        this.sMovePlayed = true;
      }
      if (!this.sProgPlayed && (double) this.StepTime >= 14.0)
      {
        SoundEffectExtensions.Emit(this.sProgressiveAppear);
        this.sProgPlayed = true;
      }
      if ((double) this.StepTime >= 29.0)
      {
        if (this.eNoise == null)
          this.eNoise = SoundEffectExtensions.Emit(this.sNoise, true, 0.0f, 0.0f);
        this.eNoise.VolumeFactor = FezMath.Saturate((float) (((double) this.StepTime - 29.0) / 7.0));
      }
      if ((double) this.StepTime < 33.0)
      {
        this.UpdateDot(elapsed);
        float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
        if ((double) this.CameraManager.Radius >= 500.0)
        {
          float radius = this.CameraManager.Radius;
          this.CameraManager.Radius = 600f * viewScale;
          this.CameraManager.SnapInterpolation();
          this.GraphicsDevice.SetRenderTarget(this.RtHandle.Target);
          this.GraphicsDevice.Clear(ColorEx.TransparentWhite);
          this.DrawDot();
          this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
          this.CloneMesh.Texture = (Dirtyable<Texture>) ((Texture) this.RtHandle.Target);
          this.CameraManager.Radius = radius;
          this.CameraManager.SnapInterpolation();
        }
      }
      if ((double) this.StepTime <= 36.0)
        return;
      this.ChangeState();
    }

    private void DrawDot()
    {
      this.DotMesh.Blending = new BlendingMode?(BlendingMode.Alphablending);
      this.DotMesh.Material.Diffuse = Vector3.Zero;
      this.DotMesh.Material.Opacity = 0.4f;
      this.DotMesh.Draw();
      this.DotMesh.Blending = new BlendingMode?(BlendingMode.Additive);
      this.DotMesh.Material.Diffuse = Vector3.One;
      this.DotMesh.Material.Opacity = 1f;
      this.DotMesh.Draw();
    }

    private void UpdateDot(float elapsed)
    {
      float num1 = (float) Math.Sqrt((double) Math.Max((float) (((double) this.CameraManager.Radius - 50.0) / 200.0), 1f));
      elapsed *= num1;
      this.DotEffect.DistanceFactor = num1;
      float num2 = Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.StepTime - 2.0) / 7.0)), EasingType.Sine);
      this.Theta += elapsed * num2;
      this.DotEffect.Theta = this.Theta;
      float num3 = (float) (Math.Sin((double) this.StepTime / 3.0) * 0.5 + 1.0);
      this.EightShapeStep += elapsed * num3;
      this.DotEffect.EightShapeStep = this.EightShapeStep;
      this.DotEffect.ImmobilityFactor = Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.StepTime - 5.0) / 10.0)), EasingType.Sine);
    }

    private void ChangeState()
    {
      if (this.ActiveState == DotsAplenty.State.Zooming)
      {
        this.Host.eNoise = this.eNoise;
        this.Host.Cycle();
      }
      else
      {
        this.StepTime = 0.0f;
        ++this.ActiveState;
        this.Update(new GameTime());
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      this.GraphicsDevice.Clear(Color.White);
      if ((double) this.StepTime < 33.0)
      {
        if ((double) this.CameraManager.Radius < 500.0)
          this.DrawDot();
        else
          this.CloneMesh.Draw();
      }
      Vector3 vector3 = EndCutscene64Host.PurpleBlack.ToVector3();
      if ((double) this.StepTime < 2.0)
        this.TargetRenderer.DrawFullscreen(new Color(vector3.X, vector3.Y, vector3.Z, 1f - Easing.EaseInOut((double) FezMath.Saturate(this.StepTime / 2f), EasingType.Sine)));
      if ((double) this.StepTime <= 27.0)
        return;
      int width = this.GraphicsDevice.Viewport.Width;
      int height = this.GraphicsDevice.Viewport.Height;
      this.NoiseOffset = new Matrix()
      {
        M11 = (float) width / 1024f,
        M22 = (float) height / 512f,
        M33 = 1f,
        M44 = 1f,
        M31 = RandomHelper.Unit(),
        M32 = RandomHelper.Unit()
      };
      float alpha = Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.StepTime - 27.0) / 6.0)), EasingType.Sine);
      this.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
      this.TargetRenderer.DrawFullscreen((BaseEffect) this.ScanlineEffect, (Texture) this.NoiseTexture, new Matrix?(this.NoiseOffset), new Color(1f, 1f, 1f, alpha));
      GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Multiply);
      this.TargetRenderer.DrawFullscreen((BaseEffect) this.VignetteEffect, new Color(1f, 1f, 1f, alpha * 0.425f));
      GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
    }

    private enum State
    {
      Zooming,
    }
  }
}
