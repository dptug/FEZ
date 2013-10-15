// Type: FezGame.Components.FezLogo
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  public class FezLogo : DrawableGameComponent
  {
    private float[] glitchTilt = new float[3];
    private Vector3[] glitchScale = new Vector3[3];
    private float[] glitchOpacity = new float[3];
    private Mesh LogoMesh;
    private Mesh WireMesh;
    private DefaultEffect FezEffect;
    public StarField Starfield;
    public float SinceStarted;
    private bool inverted;
    public Mesh InLogoMesh;
    private SoundEffect sGlitch1;
    private SoundEffect sGlitch2;
    private SoundEffect sGlitch3;
    public bool IsDisposed;
    private float untilGlitch;
    private int forFrames;
    public float Zoom;

    public bool Inverted
    {
      get
      {
        return this.inverted;
      }
      set
      {
        this.inverted = value;
        this.Enabled = true;
        this.SinceStarted = this.inverted ? 6f : 0.0f;
      }
    }

    public bool Glitched { get; set; }

    public bool DoubleTime { get; set; }

    public bool HalfSpeed { get; set; }

    public bool IsFullscreen { get; set; }

    public Texture LogoTexture { get; set; }

    public float LogoTextureXFade { get; set; }

    public float Opacity
    {
      set
      {
        this.LogoMesh.Material.Opacity = this.WireMesh.Material.Opacity = value;
        this.Starfield.Opacity = value;
      }
    }

    public bool TransitionStarted { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public FezLogo(Game game)
      : base(game)
    {
      this.Visible = false;
      this.DrawOrder = 2006;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.LogoMesh.Dispose();
      this.WireMesh.Dispose();
      this.IsDisposed = true;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LogoMesh = new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        Effect = (BaseEffect) (this.FezEffect = (DefaultEffect) new DefaultEffect.VertexColored())
      };
      this.WireMesh = new Mesh()
      {
        DepthWrites = false,
        AlwaysOnTop = true,
        Effect = this.LogoMesh.Effect
      };
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(0.0f, 0.0f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(0.0f, 1f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(0.0f, 2f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(1f, 2f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(0.0f, 3f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(1f, 3f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(2f, 3f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(4f, 0.0f, 0.0f) + new Vector3(0.0f, 0.0f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(4f, 0.0f, 0.0f) + new Vector3(1f, 0.0f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(4f, 0.0f, 0.0f) + new Vector3(2f, 0.0f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(4f, 0.0f, 0.0f) + new Vector3(0.0f, 1f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(4f, 0.0f, 0.0f) + new Vector3(0.0f, 2f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(4f, 0.0f, 0.0f) + new Vector3(1f, 2f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(4f, 0.0f, 0.0f) + new Vector3(0.0f, 3f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(4f, 0.0f, 0.0f) + new Vector3(1f, 3f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(4f, 0.0f, 0.0f) + new Vector3(2f, 3f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(8f, 0.0f, 0.0f) + new Vector3(0.0f, 0.0f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(8f, 0.0f, 0.0f) + new Vector3(1f, 0.0f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(8f, 0.0f, 0.0f) + new Vector3(2f, 0.0f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(8f, 0.0f, 0.0f) + new Vector3(0.0f, 1f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(8f, 0.0f, 0.0f) + new Vector3(1f, 1f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(8f, 0.0f, 0.0f) + new Vector3(2f, 2f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(8f, 0.0f, 0.0f) + new Vector3(0.0f, 3f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(8f, 0.0f, 0.0f) + new Vector3(1f, 3f, 0.0f), Color.Black, false);
      this.LogoMesh.AddColoredBox(Vector3.One, new Vector3(8f, 0.0f, 0.0f) + new Vector3(2f, 3f, 0.0f), Color.Black, false);
      Group group1 = this.WireMesh.AddGroup();
      IndexedUserPrimitives<FezVertexPositionColor> indexedUserPrimitives1 = new IndexedUserPrimitives<FezVertexPositionColor>(PrimitiveType.LineList);
      group1.Geometry = (IIndexedPrimitiveCollection) indexedUserPrimitives1;
      indexedUserPrimitives1.Vertices = new FezVertexPositionColor[16]
      {
        new FezVertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(1f, 0.0f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(1f, 2f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(2f, 2f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(2f, 3f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(3f, 3f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(3f, 4f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(0.0f, 4f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(0.0f, 0.0f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(1f, 0.0f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(1f, 2f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(2f, 2f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(2f, 3f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(3f, 3f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(3f, 4f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(0.0f, 4f, 1f), Color.White)
      };
      indexedUserPrimitives1.Indices = new int[50]
      {
        0,
        1,
        1,
        2,
        2,
        3,
        3,
        4,
        4,
        5,
        5,
        6,
        6,
        7,
        7,
        0,
        8,
        9,
        9,
        10,
        10,
        11,
        11,
        12,
        12,
        13,
        13,
        14,
        14,
        15,
        15,
        8,
        0,
        8,
        1,
        9,
        2,
        10,
        3,
        11,
        4,
        12,
        5,
        13,
        6,
        14,
        7,
        15,
        0,
        8
      };
      Group group2 = this.WireMesh.AddGroup();
      IndexedUserPrimitives<FezVertexPositionColor> indexedUserPrimitives2 = new IndexedUserPrimitives<FezVertexPositionColor>(PrimitiveType.LineList);
      group2.Geometry = (IIndexedPrimitiveCollection) indexedUserPrimitives2;
      indexedUserPrimitives2.Vertices = new FezVertexPositionColor[20]
      {
        new FezVertexPositionColor(new Vector3(4f, 0.0f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(7f, 0.0f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(7f, 1f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(5f, 1f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(5f, 2f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(6f, 2f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(6f, 3f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(7f, 3f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(7f, 4f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(4f, 4f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(4f, 0.0f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(7f, 0.0f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(7f, 1f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(5f, 1f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(5f, 2f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(6f, 2f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(6f, 3f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(7f, 3f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(7f, 4f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(4f, 4f, 1f), Color.White)
      };
      indexedUserPrimitives2.Indices = new int[60]
      {
        0,
        1,
        1,
        2,
        2,
        3,
        3,
        4,
        4,
        5,
        5,
        6,
        6,
        7,
        7,
        8,
        8,
        9,
        9,
        0,
        10,
        11,
        11,
        12,
        12,
        13,
        13,
        14,
        14,
        15,
        15,
        16,
        16,
        17,
        17,
        18,
        18,
        19,
        19,
        10,
        0,
        10,
        1,
        11,
        2,
        12,
        3,
        13,
        4,
        14,
        5,
        15,
        6,
        16,
        7,
        17,
        8,
        18,
        9,
        19
      };
      Group group3 = this.WireMesh.AddGroup();
      IndexedUserPrimitives<FezVertexPositionColor> indexedUserPrimitives3 = new IndexedUserPrimitives<FezVertexPositionColor>(PrimitiveType.LineList);
      group3.Geometry = (IIndexedPrimitiveCollection) indexedUserPrimitives3;
      indexedUserPrimitives3.Vertices = new FezVertexPositionColor[22]
      {
        new FezVertexPositionColor(new Vector3(8f, 0.0f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(11f, 0.0f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(11f, 1f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(10f, 1f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(10f, 2f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(11f, 2f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(11f, 4f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(8f, 4f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(8f, 3f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(10f, 3f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(8f, 2f, 0.0f), Color.White),
        new FezVertexPositionColor(new Vector3(8f, 0.0f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(11f, 0.0f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(11f, 1f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(10f, 1f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(10f, 2f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(11f, 2f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(11f, 4f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(8f, 4f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(8f, 3f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(10f, 3f, 1f), Color.White),
        new FezVertexPositionColor(new Vector3(8f, 2f, 1f), Color.White)
      };
      indexedUserPrimitives3.Indices = new int[70]
      {
        0,
        1,
        1,
        2,
        2,
        3,
        3,
        4,
        4,
        5,
        5,
        6,
        6,
        7,
        7,
        8,
        8,
        9,
        9,
        4,
        4,
        10,
        10,
        0,
        11,
        12,
        12,
        13,
        13,
        14,
        14,
        15,
        15,
        16,
        16,
        17,
        17,
        18,
        18,
        19,
        19,
        20,
        20,
        15,
        15,
        21,
        21,
        11,
        0,
        11,
        1,
        12,
        2,
        13,
        3,
        14,
        4,
        15,
        5,
        16,
        6,
        17,
        7,
        18,
        8,
        19,
        9,
        20,
        10,
        21
      };
      this.WireMesh.Position = this.LogoMesh.Position = new Vector3(-5.5f, -2f, -0.5f);
      this.WireMesh.BakeTransform<FezVertexPositionColor>();
      this.LogoMesh.BakeTransform<FezVertexPositionColor>();
      ContentManager contentManager = this.CMProvider.Get(CM.Menu);
      this.sGlitch1 = contentManager.Load<SoundEffect>("Sounds/Intro/FezLogoGlitch1");
      this.sGlitch2 = contentManager.Load<SoundEffect>("Sounds/Intro/FezLogoGlitch2");
      this.sGlitch3 = contentManager.Load<SoundEffect>("Sounds/Intro/FezLogoGlitch3");
      ServiceHelper.AddComponent((IGameComponent) (this.Starfield = new StarField(this.Game)));
      this.Starfield.Opacity = 0.0f;
      this.LogoMesh.Material.Opacity = 0.0f;
      this.untilGlitch = RandomHelper.Between(1.0 / 3.0, 1.0);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.IsDisposed || this.GameState.Paused)
        return;
      if (this.TransitionStarted)
        this.SinceStarted += (float) (gameTime.ElapsedGameTime.TotalSeconds * (this.Inverted ? -0.100000001490116 : (this.DoubleTime ? 2.0 : (this.HalfSpeed ? 0.75 : 1.0))));
      float num = Math.Min(Easing.EaseIn((double) FezMath.Saturate(this.SinceStarted / 5f), EasingType.Sine), 0.999f);
      this.Zoom = num;
      if ((double) this.LogoMesh.Material.Opacity == 1.0 && this.Glitched && (double) num <= 0.75)
      {
        this.untilGlitch -= (float) gameTime.ElapsedGameTime.TotalSeconds;
        if ((double) this.untilGlitch <= 0.0)
        {
          this.untilGlitch = RandomHelper.Between(1.0 / 3.0, 2.0);
          this.glitchTilt[0] = RandomHelper.Between(0.0, 1.0);
          this.glitchTilt[1] = RandomHelper.Between(0.0, 1.0);
          this.glitchTilt[2] = RandomHelper.Between(0.0, 1.0);
          this.glitchOpacity[0] = RandomHelper.Between(0.25, 1.0);
          this.glitchOpacity[1] = RandomHelper.Between(0.25, 1.0);
          this.glitchOpacity[2] = RandomHelper.Between(0.25, 1.0);
          this.glitchScale[0] = new Vector3(RandomHelper.Between(0.75, 1.5), RandomHelper.Between(0.75, 1.5), RandomHelper.Between(0.75, 1.5));
          this.glitchScale[1] = new Vector3(RandomHelper.Between(0.75, 1.5), RandomHelper.Between(0.75, 1.5), RandomHelper.Between(0.75, 1.5));
          this.glitchScale[2] = new Vector3(RandomHelper.Between(0.75, 1.5), RandomHelper.Between(0.75, 1.5), RandomHelper.Between(0.75, 1.5));
          this.forFrames = RandomHelper.Random.Next(1, 7);
          if (RandomHelper.Probability(1.0 / 3.0))
            SoundEffectExtensions.Emit(this.sGlitch1);
          if (RandomHelper.Probability(0.5))
            SoundEffectExtensions.Emit(this.sGlitch2);
          else
            SoundEffectExtensions.Emit(this.sGlitch3);
        }
      }
      float aspectRatio = this.GraphicsDevice.Viewport.AspectRatio;
      this.WireMesh.Position = this.LogoMesh.Position = new Vector3(0.0f, -num, 0.0f);
      this.FezEffect.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic((float) (10.0 * (double) aspectRatio * (1.0 - (double) num)), (float) (10.0 * (1.0 - (double) num)), 0.1f, 100f));
      if (this.Starfield != null)
      {
        this.Starfield.AdditionalZoom = Easing.EaseInOut((double) num, EasingType.Quadratic);
        this.Starfield.HasZoomed = true;
      }
      if (this.Inverted || (double) num < 0.999000012874603)
        return;
      this.IsFullscreen = true;
    }

    public override void Draw(GameTime gameTime)
    {
      this.DoDraw();
    }

    private void DoDraw()
    {
      if (this.IsDisposed)
        return;
      if (Fez.LongScreenshot)
        this.TargetRenderer.DrawFullscreen(Color.White);
      if (this.forFrames == 0)
      {
        float amount = 1f - Easing.EaseInOut((double) FezMath.Saturate(this.SinceStarted / 5f), EasingType.Sine);
        this.WireMesh.Scale = this.LogoMesh.Scale = new Vector3(1f, 1f, 1f);
        this.FezEffect.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.Lerp(new Vector3(0.0f, 0.0f, 10f), new Vector3(-10f, 10f, 10f), amount), Vector3.Zero, Vector3.Up));
        this.DrawMaskedLogo();
      }
      else
      {
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.Red | ColorWriteChannels.Green);
        this.LogoMesh.Scale = this.glitchScale[0];
        this.LogoMesh.Material.Opacity = this.glitchOpacity[0];
        this.FezEffect.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.Lerp(new Vector3(0.0f, 0.0f, 10f), new Vector3(-10f, 10f, 10f), this.glitchTilt[0]), Vector3.Zero, Vector3.Up));
        this.DrawMaskedLogo();
        this.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0.0f, 0);
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.Green | ColorWriteChannels.Blue);
        this.LogoMesh.Scale = this.glitchScale[1];
        this.LogoMesh.Material.Opacity = this.glitchOpacity[1];
        this.FezEffect.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.Lerp(new Vector3(0.0f, 0.0f, 10f), new Vector3(-10f, 10f, 10f), this.glitchTilt[1]), Vector3.Zero, Vector3.Up));
        this.DrawMaskedLogo();
        this.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0.0f, 0);
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.Red | ColorWriteChannels.Blue);
        this.LogoMesh.Scale = this.glitchScale[2];
        this.LogoMesh.Material.Opacity = this.glitchOpacity[2];
        this.FezEffect.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.Lerp(new Vector3(0.0f, 0.0f, 10f), new Vector3(-10f, 10f, 10f), this.glitchTilt[2]), Vector3.Zero, Vector3.Up));
        this.DrawMaskedLogo();
        GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
        this.LogoMesh.Material.Opacity = 1f;
        --this.forFrames;
      }
      if (this.LogoTexture != null)
      {
        this.TargetRenderer.DrawFullscreen(this.LogoTexture, new Color(1f, 1f, 1f, this.LogoTextureXFade));
        this.Starfield.Opacity = 1f - this.LogoTextureXFade;
      }
      if (this.InLogoMesh != null)
        this.InLogoMesh.Draw();
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      this.WireMesh.Draw();
    }

    private void DrawMaskedLogo()
    {
      GraphicsDeviceExtensions.PrepareStencilReadWrite(this.GraphicsDevice, CompareFunction.NotEqual, StencilMask.BlackHoles);
      this.LogoMesh.Draw();
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Equal, StencilMask.BlackHoles);
      this.Starfield.Draw();
    }
  }
}
