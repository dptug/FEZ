// Type: FezGame.Components.TextScroll
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  public class TextScroll : DrawableGameComponent
  {
    private static readonly Color TextColor = Color.Black;
    private const float OpenCloseDuration = 0.5f;
    private readonly bool OnTop;
    private string Text;
    private Vector2 MiddlePartSize;
    private bool Ready;
    private Mesh ScrollMesh;
    private Group LeftPart;
    private Group MiddlePart;
    private Group RightPart;
    private Group TextGroup;
    private RenderTarget2D TextTexture;
    private GlyphTextRenderer GTR;
    private SoundEffect sOpen;
    private SoundEffect sClose;
    private TimeSpan SinceOpen;
    private TimeSpan SinceClose;

    public string Key { get; set; }

    public float? Timeout { get; set; }

    public bool Closing { get; set; }

    public TextScroll NextScroll { get; set; }

    [ServiceDependency]
    public IFontManager FontManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    static TextScroll()
    {
    }

    public TextScroll(Game game, string text, bool onTop)
      : base(game)
    {
      this.DrawOrder = 1001;
      this.OnTop = onTop;
      this.Text = text;
    }

    public static void PreInitialize()
    {
      IContentManagerProvider contentManagerProvider = ServiceHelper.Get<IContentManagerProvider>();
      contentManagerProvider.Global.Load<Texture2D>("Other Textures/SCROLL/SCROLL_A");
      contentManagerProvider.Global.Load<Texture2D>("Other Textures/SCROLL/SCROLL_B");
      contentManagerProvider.Global.Load<Texture2D>("Other Textures/SCROLL/SCROLL_C");
    }

    public void Close()
    {
      if (this.Closing)
        return;
      SoundEffectExtensions.Emit(this.sClose);
      this.Closing = true;
      this.SinceClose = TimeSpan.Zero;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.GraphicsDevice != null)
        this.GraphicsDevice.DeviceReset -= new EventHandler<EventArgs>(this.UpdateViewScale);
      if (this.TextTexture != null)
      {
        this.TextTexture.Dispose();
        this.TextTexture = (RenderTarget2D) null;
      }
      if (this.ScrollMesh != null)
      {
        this.ScrollMesh.Dispose();
        this.ScrollMesh = (Mesh) null;
      }
      if (this.GameState == null)
        return;
      this.GameState.ActiveScroll = this.NextScroll;
      if (this.NextScroll == null)
        return;
      ServiceHelper.AddComponent((IGameComponent) this.GameState.ActiveScroll);
    }

    public override void Initialize()
    {
      base.Initialize();
      this.sOpen = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/ScrollOpen");
      this.sClose = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/ScrollClose");
      SoundEffectExtensions.Emit(this.sOpen);
    }

    private void LateLoadContent()
    {
      this.GTR = new GlyphTextRenderer(this.Game);
      SpriteFont big = this.FontManager.Big;
      float scale = (Culture.IsCJK ? 0.6f : 1f) * SettingsManager.GetViewScale(this.GraphicsDevice);
      Vector2 vector2_1 = this.GTR.MeasureWithGlyphs(big, this.Text, scale);
      this.MiddlePartSize = vector2_1 * new Vector2(1f, 1.25f);
      int width = (int) this.MiddlePartSize.X;
      int height = (int) this.MiddlePartSize.Y;
      Vector2 vector2_2 = this.MiddlePartSize;
      if (this.TextTexture != null)
      {
        this.TextTexture.Dispose();
        this.TextTexture = (RenderTarget2D) null;
      }
      this.TextTexture = new RenderTarget2D(this.GraphicsDevice, width, height, false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, this.GraphicsDevice.PresentationParameters.DepthStencilFormat, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
      using (SpriteBatch spriteBatch = new SpriteBatch(this.GraphicsDevice))
      {
        this.GraphicsDevice.SetRenderTarget(this.TextTexture);
        GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
        this.GraphicsDevice.Clear(ClearOptions.Target, new Color(215, 188, 122, 0), 1f, 0);
        GraphicsDeviceExtensions.BeginPoint(spriteBatch);
        this.GTR.DrawString(spriteBatch, big, this.Text, vector2_2 / 2f - vector2_1 / 2f + new Vector2(0.0f, this.FontManager.TopSpacing / 2f), TextScroll.TextColor, scale);
        spriteBatch.End();
        this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      }
      this.MiddlePartSize /= Culture.IsCJK ? 3f : 2f;
      this.MiddlePartSize -= new Vector2(16f, 0.0f);
      this.ScrollMesh = new Mesh()
      {
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        Effect = (BaseEffect) new DefaultEffect.Textured(),
        AlwaysOnTop = true,
        DepthWrites = false,
        Material = {
          Opacity = 0.0f
        },
        SamplerState = SamplerState.PointClamp
      };
      this.LeftPart = this.ScrollMesh.AddFace(new Vector3(2f), new Vector3(0.0f, 0.5f, 0.0f), FaceOrientation.Front, true);
      this.LeftPart.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/SCROLL/SCROLL_A");
      this.MiddlePart = this.ScrollMesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, true);
      this.MiddlePart.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/SCROLL/SCROLL_B");
      this.RightPart = this.ScrollMesh.AddFace(new Vector3(2f), new Vector3(0.0f, -0.5f, 0.0f), FaceOrientation.Front, true);
      this.RightPart.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/SCROLL/SCROLL_C");
      this.TextGroup = this.ScrollMesh.AddFace(new Vector3((float) (((double) this.MiddlePartSize.X + 16.0) / 16.0), this.MiddlePartSize.Y / 16f, 1f), new Vector3(-0.125f, 0.0f, 0.0f), FaceOrientation.Front, true);
      this.TextGroup.SamplerState = Culture.IsCJK ? SamplerState.AnisotropicClamp : SamplerState.PointClamp;
      this.TextGroup.Texture = (Texture) this.TextTexture;
      this.TextGroup.Material = new Material()
      {
        Opacity = 0.0f
      };
      this.ScrollMesh.Effect.ForcedProjectionMatrix = new Matrix?(Matrix.Identity);
      this.ScrollMesh.Effect.ForcedViewMatrix = new Matrix?(Matrix.Identity);
      this.MiddlePartSize /= SettingsManager.GetViewScale(this.GraphicsDevice);
      this.GraphicsDevice.DeviceReset += new EventHandler<EventArgs>(this.UpdateViewScale);
      if (Culture.IsCJK)
        this.MiddlePartSize.X /= 2f;
      this.Ready = true;
    }

    private void UpdateViewScale(object sender, EventArgs e)
    {
      if (this.Closing)
        return;
      this.LateLoadContent();
      this.OpenOrClose(1f);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      if (!this.Ready)
        this.LateLoadContent();
      this.ScrollMesh.Scale = new Vector3(96f / (float) this.GraphicsDevice.Viewport.Width, 96f / (float) this.GraphicsDevice.Viewport.Height, 1f) * SettingsManager.GetViewScale(this.GraphicsDevice);
      this.TextGroup.Scale = !Culture.IsCJK ? new Vector3(1f / SettingsManager.GetViewScale(this.GraphicsDevice)) : new Vector3(1f / SettingsManager.GetViewScale(this.GraphicsDevice)) / 2f;
      this.ScrollMesh.Position = new Vector3(0.01f, this.OnTop ? 0.6125f : -0.6125f, 0.0f);
      if (this.Timeout.HasValue)
      {
        TextScroll textScroll = this;
        float? timeout = textScroll.Timeout;
        float num = (float) gameTime.ElapsedGameTime.TotalSeconds;
        float? nullable = timeout.HasValue ? new float?(timeout.GetValueOrDefault() - num) : new float?();
        textScroll.Timeout = nullable;
        if ((double) this.Timeout.Value <= 0.0)
        {
          SoundEffectExtensions.Emit(this.sClose);
          this.Closing = true;
          this.Timeout = new float?();
          this.SinceClose = TimeSpan.Zero;
        }
      }
      if (this.SinceOpen.TotalSeconds < 0.5)
      {
        this.SinceOpen += gameTime.ElapsedGameTime;
        this.OpenOrClose(Easing.EaseInOut(this.SinceOpen.TotalSeconds / 0.5, EasingType.Cubic, EasingType.Sine));
      }
      else
      {
        if (!this.Closing)
          return;
        this.SinceClose += gameTime.ElapsedGameTime;
        this.OpenOrClose(Easing.EaseInOut(1.0 - this.SinceClose.TotalSeconds / 0.5, EasingType.Cubic, EasingType.Sine));
        if (this.SinceClose.TotalSeconds <= 0.5)
          return;
        ServiceHelper.RemoveComponent<TextScroll>(this);
      }
    }

    private void OpenOrClose(float step)
    {
      this.ScrollMesh.Material.Opacity = FezMath.Saturate(step / 0.4f);
      this.LeftPart.Position = Vector3.Lerp(new Vector3(-1f, 0.0f, 0.0f), new Vector3((float) (-(double) this.MiddlePartSize.X / 16.0 / 2.0 - 1.0), 0.0f, 0.0f), step);
      this.RightPart.Position = Vector3.Lerp(new Vector3(1f, 0.0f, 0.0f), new Vector3((float) ((double) this.MiddlePartSize.X / 16.0 / 2.0 + 1.0), 0.0f, 0.0f), step);
      this.MiddlePart.Scale = Vector3.Lerp(new Vector3(0.0f, 1f, 1f), new Vector3((float) (((double) this.MiddlePartSize.X + 1.0) / 16.0), 1f, 1f), step);
      this.MiddlePart.TextureMatrix.Set(new Matrix?(new Matrix(MathHelper.Lerp(0.0f, this.MiddlePartSize.X / 32f, step), 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f)));
      this.TextGroup.Material.Opacity = Easing.EaseIn((double) FezMath.Saturate((float) (((double) step - 0.75) / 0.25)), EasingType.Quadratic);
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.Ready)
        return;
      this.ScrollMesh.Draw();
    }
  }
}
