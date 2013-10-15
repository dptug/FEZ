// Type: FezGame.Components.EndCutscene64.MulticoloredSpace
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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezGame.Components.EndCutscene64
{
  internal class MulticoloredSpace : DrawableGameComponent
  {
    private static readonly Color MainCubeColor = new Color(0.2705882f, 0.9764706f, 1f);
    private static readonly Color[] Colors = new Color[10]
    {
      new Color(20, 1, 28),
      new Color(108, 27, 44),
      new Color(225, 125, 53),
      new Color(246, 231, 108),
      new Color(155, 226, 177),
      new Color(67, 246, (int) byte.MaxValue),
      new Color(100, 154, 224),
      new Color(214, 133, 180),
      new Color(189, 63, 117),
      new Color(98, 21, 88)
    };
    private readonly EndCutscene64Host Host;
    private Mesh PointsMesh;
    private Mesh CubesMesh;
    private DefaultEffect CubesEffect;
    private SoundEffect sBlueZoomOut;
    private SoundEffect sProgressiveAppear;
    private SoundEffect sFadeOut;
    private bool sBluePlayed;
    private bool sProgPlayed;
    private bool sFadePlayed;
    private float preWaitTime;
    private float StepTime;
    private MulticoloredSpace.State ActiveState;

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

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    static MulticoloredSpace()
    {
    }

    public MulticoloredSpace(Game game, EndCutscene64Host host)
      : base(game)
    {
      this.Host = host;
      this.DrawOrder = 1000;
      this.UpdateOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.sBlueZoomOut = this.CMProvider.Get(CM.EndCutscene).Load<SoundEffect>("Sounds/Ending/Cutscene64/BlueZoomOut");
      this.sProgressiveAppear = this.CMProvider.Get(CM.EndCutscene).Load<SoundEffect>("Sounds/Ending/Cutscene64/CubesProgressiveAppear");
      this.sFadeOut = this.CMProvider.Get(CM.EndCutscene).Load<SoundEffect>("Sounds/Ending/Cutscene64/CubesFadeOut");
      this.LevelManager.ActualAmbient = new Color(0.25f, 0.25f, 0.25f);
      this.LevelManager.ActualDiffuse = Color.White;
      Random random = RandomHelper.Random;
      float y1 = 1f / (float) Math.Sqrt(6.0);
      float z1 = (float) Math.Sqrt(3.0) / 2f;
      float y2 = (float) Math.Sqrt(2.0 / 3.0);
      float z2 = (float) (1.0 / (2.0 * Math.Sqrt(3.0)));
      float x = 1f / (float) Math.Sqrt(2.0);
      VertexPositionNormalColor[] vertices = new VertexPositionNormalColor[864000];
      int[] indices = new int[1296000];
      int num1 = 0;
      int num2 = 0;
      for (int index1 = -100; index1 < 100; ++index1)
      {
        for (int index2 = -180; index2 < 180; ++index2)
        {
          Color color = index2 != 0 || index1 != 0 ? MulticoloredSpace.Colors[random.Next(0, MulticoloredSpace.Colors.Length)] : MulticoloredSpace.MainCubeColor;
          Vector3 vector3 = new Vector3((float) (index2 * 6), (float) (index1 * 6 + (Math.Abs(index2) % 2 == 0 ? 0 : 3)), 0.0f);
          int num3 = num1;
          VertexPositionNormalColor[] positionNormalColorArray1 = vertices;
          int index3 = num1;
          int num4 = 1;
          int num5 = index3 + num4;
          positionNormalColorArray1[index3] = new VertexPositionNormalColor(new Vector3(x, -y1, -z2) + vector3, new Vector3(-1f, 0.0f, 0.0f), color);
          VertexPositionNormalColor[] positionNormalColorArray2 = vertices;
          int index4 = num5;
          int num6 = 1;
          int num7 = index4 + num6;
          positionNormalColorArray2[index4] = new VertexPositionNormalColor(new Vector3(x, y1, z2) + vector3, new Vector3(-1f, 0.0f, 0.0f), color);
          VertexPositionNormalColor[] positionNormalColorArray3 = vertices;
          int index5 = num7;
          int num8 = 1;
          int num9 = index5 + num8;
          positionNormalColorArray3[index5] = new VertexPositionNormalColor(new Vector3(0.0f, 0.0f, z1) + vector3, new Vector3(-1f, 0.0f, 0.0f), color);
          VertexPositionNormalColor[] positionNormalColorArray4 = vertices;
          int index6 = num9;
          int num10 = 1;
          int num11 = index6 + num10;
          positionNormalColorArray4[index6] = new VertexPositionNormalColor(new Vector3(0.0f, -y2, z2) + vector3, new Vector3(-1f, 0.0f, 0.0f), color);
          VertexPositionNormalColor[] positionNormalColorArray5 = vertices;
          int index7 = num11;
          int num12 = 1;
          int num13 = index7 + num12;
          positionNormalColorArray5[index7] = new VertexPositionNormalColor(new Vector3(0.0f, -y2, z2) + vector3, new Vector3(0.0f, 0.0f, -1f), color);
          VertexPositionNormalColor[] positionNormalColorArray6 = vertices;
          int index8 = num13;
          int num14 = 1;
          int num15 = index8 + num14;
          positionNormalColorArray6[index8] = new VertexPositionNormalColor(new Vector3(0.0f, 0.0f, z1) + vector3, new Vector3(0.0f, 0.0f, -1f), color);
          VertexPositionNormalColor[] positionNormalColorArray7 = vertices;
          int index9 = num15;
          int num16 = 1;
          int num17 = index9 + num16;
          positionNormalColorArray7[index9] = new VertexPositionNormalColor(new Vector3(-x, y1, z2) + vector3, new Vector3(0.0f, 0.0f, -1f), color);
          VertexPositionNormalColor[] positionNormalColorArray8 = vertices;
          int index10 = num17;
          int num18 = 1;
          int num19 = index10 + num18;
          positionNormalColorArray8[index10] = new VertexPositionNormalColor(new Vector3(-x, -y1, -z2) + vector3, new Vector3(0.0f, 0.0f, -1f), color);
          VertexPositionNormalColor[] positionNormalColorArray9 = vertices;
          int index11 = num19;
          int num20 = 1;
          int num21 = index11 + num20;
          positionNormalColorArray9[index11] = new VertexPositionNormalColor(new Vector3(0.0f, y2, -z2) + vector3, new Vector3(0.0f, 1f, 0.0f), color);
          VertexPositionNormalColor[] positionNormalColorArray10 = vertices;
          int index12 = num21;
          int num22 = 1;
          int num23 = index12 + num22;
          positionNormalColorArray10[index12] = new VertexPositionNormalColor(new Vector3(-x, y1, z2) + vector3, new Vector3(0.0f, 1f, 0.0f), color);
          VertexPositionNormalColor[] positionNormalColorArray11 = vertices;
          int index13 = num23;
          int num24 = 1;
          int num25 = index13 + num24;
          positionNormalColorArray11[index13] = new VertexPositionNormalColor(new Vector3(0.0f, 0.0f, z1) + vector3, new Vector3(0.0f, 1f, 0.0f), color);
          VertexPositionNormalColor[] positionNormalColorArray12 = vertices;
          int index14 = num25;
          int num26 = 1;
          num1 = index14 + num26;
          positionNormalColorArray12[index14] = new VertexPositionNormalColor(new Vector3(x, y1, z2) + vector3, new Vector3(0.0f, 1f, 0.0f), color);
          int[] numArray1 = indices;
          int index15 = num2;
          int num27 = 1;
          int num28 = index15 + num27;
          int num29 = num3;
          numArray1[index15] = num29;
          int[] numArray2 = indices;
          int index16 = num28;
          int num30 = 1;
          int num31 = index16 + num30;
          int num32 = 2 + num3;
          numArray2[index16] = num32;
          int[] numArray3 = indices;
          int index17 = num31;
          int num33 = 1;
          int num34 = index17 + num33;
          int num35 = 1 + num3;
          numArray3[index17] = num35;
          int[] numArray4 = indices;
          int index18 = num34;
          int num36 = 1;
          int num37 = index18 + num36;
          int num38 = num3;
          numArray4[index18] = num38;
          int[] numArray5 = indices;
          int index19 = num37;
          int num39 = 1;
          int num40 = index19 + num39;
          int num41 = 3 + num3;
          numArray5[index19] = num41;
          int[] numArray6 = indices;
          int index20 = num40;
          int num42 = 1;
          int num43 = index20 + num42;
          int num44 = 2 + num3;
          numArray6[index20] = num44;
          int[] numArray7 = indices;
          int index21 = num43;
          int num45 = 1;
          int num46 = index21 + num45;
          int num47 = 4 + num3;
          numArray7[index21] = num47;
          int[] numArray8 = indices;
          int index22 = num46;
          int num48 = 1;
          int num49 = index22 + num48;
          int num50 = 6 + num3;
          numArray8[index22] = num50;
          int[] numArray9 = indices;
          int index23 = num49;
          int num51 = 1;
          int num52 = index23 + num51;
          int num53 = 5 + num3;
          numArray9[index23] = num53;
          int[] numArray10 = indices;
          int index24 = num52;
          int num54 = 1;
          int num55 = index24 + num54;
          int num56 = 4 + num3;
          numArray10[index24] = num56;
          int[] numArray11 = indices;
          int index25 = num55;
          int num57 = 1;
          int num58 = index25 + num57;
          int num59 = 7 + num3;
          numArray11[index25] = num59;
          int[] numArray12 = indices;
          int index26 = num58;
          int num60 = 1;
          int num61 = index26 + num60;
          int num62 = 6 + num3;
          numArray12[index26] = num62;
          int[] numArray13 = indices;
          int index27 = num61;
          int num63 = 1;
          int num64 = index27 + num63;
          int num65 = 8 + num3;
          numArray13[index27] = num65;
          int[] numArray14 = indices;
          int index28 = num64;
          int num66 = 1;
          int num67 = index28 + num66;
          int num68 = 10 + num3;
          numArray14[index28] = num68;
          int[] numArray15 = indices;
          int index29 = num67;
          int num69 = 1;
          int num70 = index29 + num69;
          int num71 = 9 + num3;
          numArray15[index29] = num71;
          int[] numArray16 = indices;
          int index30 = num70;
          int num72 = 1;
          int num73 = index30 + num72;
          int num74 = 8 + num3;
          numArray16[index30] = num74;
          int[] numArray17 = indices;
          int index31 = num73;
          int num75 = 1;
          int num76 = index31 + num75;
          int num77 = 11 + num3;
          numArray17[index31] = num77;
          int[] numArray18 = indices;
          int index32 = num76;
          int num78 = 1;
          num2 = index32 + num78;
          int num79 = 10 + num3;
          numArray18[index32] = num79;
        }
      }
      this.CubesMesh = new Mesh()
      {
        Effect = (BaseEffect) (this.CubesEffect = (DefaultEffect) new DefaultEffect.LitVertexColored()),
        DepthWrites = false,
        AlwaysOnTop = true
      };
      Group group = this.CubesMesh.AddGroup();
      BufferedIndexedPrimitives<VertexPositionNormalColor> indexedPrimitives = new BufferedIndexedPrimitives<VertexPositionNormalColor>(vertices, indices, PrimitiveType.TriangleList);
      indexedPrimitives.UpdateBuffers();
      indexedPrimitives.CleanUp();
      group.Geometry = (IIndexedPrimitiveCollection) indexedPrimitives;
      this.PointsMesh = new Mesh()
      {
        Effect = (BaseEffect) new PointsFromLinesEffect(),
        DepthWrites = false,
        AlwaysOnTop = true
      };
      Color[] colorArray = new Color[32640];
      Vector3[] vector3Array = new Vector3[32640];
      int index33 = 0;
      for (int index1 = -68; index1 < 68; ++index1)
      {
        for (int index2 = -120; index2 < 120; ++index2)
        {
          vector3Array[index33] = new Vector3((float) index2 / 8f, (float) ((double) index1 / 8.0 + (Math.Abs(index2) % 2 == 0 ? 0.0 : 1.0 / 16.0)), 0.0f);
          colorArray[index33++] = RandomHelper.InList<Color>(MulticoloredSpace.Colors);
        }
      }
      this.PointsMesh.AddPoints((IList<Color>) colorArray, (IEnumerable<Vector3>) vector3Array, true);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.CubesMesh.Dispose();
      this.PointsMesh.Dispose();
    }

    private void Reset()
    {
      this.CameraManager.Center = Vector3.Zero;
      this.CameraManager.Direction = Vector3.UnitZ;
      this.CameraManager.Radius = 1.25f;
      this.CameraManager.SnapInterpolation();
      this.PointsMesh.Scale = Vector3.One;
      this.sBluePlayed = this.sProgPlayed = this.sFadePlayed = false;
      this.preWaitTime = 0.0f;
      this.StepTime = 0.0f;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      float num1 = (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) num1 == 0.0 || (double) this.StepTime == 0.0)
        this.Reset();
      if ((double) this.preWaitTime > 2.0)
      {
        this.StepTime += num1;
        IGameCameraManager cameraManager = this.CameraManager;
        double num2 = (double) cameraManager.Radius * 1.00499999523163;
        cameraManager.Radius = (float) num2;
        this.PointsMesh.Scale *= 1.00125f;
      }
      else
      {
        this.StepTime = 1.0 / 1000.0;
        this.preWaitTime += num1;
      }
      this.CameraManager.SnapInterpolation();
      this.CubesEffect.Emissive = 1f - Easing.EaseInOut((double) FezMath.Saturate(this.StepTime / 10f), EasingType.Quadratic);
      this.CubesMesh.Material.Opacity = 1f - Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.StepTime - 23.0) / 3.0)), EasingType.Sine);
      this.PointsMesh.Material.Opacity = 1f - Easing.EaseIn((double) FezMath.Saturate((float) (((double) this.StepTime - 5.0) / 10.0)), EasingType.Sine);
      if (!this.sBluePlayed && (double) this.StepTime > 0.25)
      {
        SoundEffectExtensions.Emit(this.sBlueZoomOut);
        this.sBluePlayed = true;
      }
      if (!this.sProgPlayed && (double) this.StepTime > 7.5)
      {
        SoundEffectExtensions.Emit(this.sProgressiveAppear);
        this.sProgPlayed = true;
      }
      if (!this.sFadePlayed && (double) this.StepTime > 24.0)
      {
        SoundEffectExtensions.Emit(this.sFadeOut);
        this.sFadePlayed = true;
      }
      if ((double) this.StepTime <= 26.0)
        return;
      this.ChangeState();
    }

    private void ChangeState()
    {
      if (this.ActiveState == MulticoloredSpace.State.Zooming)
      {
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
      this.GraphicsDevice.Clear(EndCutscene64Host.PurpleBlack);
      if ((double) this.PointsMesh.Material.Opacity > 0.0)
        this.PointsMesh.Draw();
      this.CubesMesh.Draw();
    }

    private enum State
    {
      Zooming,
    }
  }
}
