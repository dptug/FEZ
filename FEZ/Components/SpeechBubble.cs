// Type: FezGame.Components.SpeechBubble
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Effects.Structures;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FezGame.Components
{
  public class SpeechBubble : DrawableGameComponent, ISpeechBubbleManager
  {
    private readonly Color TextColor = Color.White;
    private const int TextBorder = 4;
    private Vector2 scalableMiddleSize;
    private float sinceShown;
    private readonly Mesh textMesh;
    private readonly Mesh canvasMesh;
    private Group scalableMiddle;
    private Group scalableTop;
    private Group scalableBottom;
    private Group neGroup;
    private Group nwGroup;
    private Group seGroup;
    private Group swGroup;
    private Group tailGroup;
    private Group bGroup;
    private Group textGroup;
    private GlyphTextRenderer GTR;
    private SpriteBatch spriteBatch;
    private SpriteFont zuishFont;
    private RenderTarget2D text;
    private string originalString;
    private string textString;
    private float distanceFromCenterAtTextChange;
    private bool changingText;
    private bool show;
    private Vector3 origin;
    private Vector3 lastUsedOrigin;
    private RenderTarget2D bTexture;
    private Vector3 oldCamPos;

    public bool Hidden
    {
      get
      {
        if (!this.show)
          return !this.changingText;
        else
          return false;
      }
    }

    public Vector3 Origin
    {
      private get
      {
        return this.origin;
      }
      set
      {
        this.origin = value;
        if (FezMath.AlmostEqual(this.lastUsedOrigin, this.origin, 1.0 / 16.0) || (double) this.sinceShown < 1.0 || this.changingText)
          return;
        this.OnTextChanged(true);
      }
    }

    public SpeechFont Font { get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IFontManager FontManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public SpeechBubble(Game game)
      : base(game)
    {
      this.textMesh = new Mesh()
      {
        AlwaysOnTop = true,
        SamplerState = SamplerState.PointClamp,
        Blending = new BlendingMode?(BlendingMode.Alphablending)
      };
      this.canvasMesh = new Mesh()
      {
        AlwaysOnTop = true,
        SamplerState = SamplerState.PointClamp,
        Blending = new BlendingMode?(BlendingMode.Alphablending)
      };
      this.DrawOrder = 150;
      this.Font = SpeechFont.Pixel;
      this.show = false;
    }

    public void ChangeText(string toText)
    {
      this.originalString = toText.ToUpper(CultureInfo.InvariantCulture);
      if (this.changingText)
        return;
      this.changingText = true;
      Waiters.Wait((Func<bool>) (() => (double) this.sinceShown == 0.0), (Action) (() =>
      {
        if (!this.changingText)
          return;
        this.changingText = false;
        this.UpdateBTexture();
        this.OnTextChanged(false);
        this.show = true;
      })).AutoPause = true;
    }

    public void Hide()
    {
      this.show = false;
      this.changingText = false;
    }

    public override void Initialize()
    {
      this.scalableTop = this.canvasMesh.AddFace(new Vector3(1f, 0.5f, 0.0f), Vector3.Zero, FaceOrientation.Front, false);
      this.scalableBottom = this.canvasMesh.CloneGroup(this.scalableTop);
      this.scalableMiddle = this.canvasMesh.AddFace(new Vector3(1f, 1f, 0.0f), Vector3.Zero, FaceOrientation.Front, false);
      this.neGroup = this.canvasMesh.AddFace(new Vector3(0.5f, 0.5f, 0.0f), Vector3.Zero, FaceOrientation.Front, false);
      this.nwGroup = this.canvasMesh.CloneGroup(this.neGroup);
      this.seGroup = this.canvasMesh.CloneGroup(this.neGroup);
      this.swGroup = this.canvasMesh.CloneGroup(this.neGroup);
      this.tailGroup = this.canvasMesh.AddFace(new Vector3(5.0 / 16.0, 0.25f, 0.0f), Vector3.Zero, FaceOrientation.Front, false, true);
      this.textGroup = this.textMesh.AddFace(new Vector3(1f, 1f, 0.0f), Vector3.Zero, FaceOrientation.Front, false);
      this.bGroup = this.canvasMesh.AddFace(new Vector3(1f, 1f, 0.0f), Vector3.Zero, FaceOrientation.Front, false);
      this.swGroup.Position = Vector3.Zero;
      this.scalableBottom.Position = new Vector3(0.5f, 0.0f, 0.0f);
      this.scalableMiddle.Position = new Vector3(0.0f, 0.5f, 0.0f);
      this.tailGroup.Position = new Vector3(0.5f, -0.25f, 0.0f);
      this.GTR = new GlyphTextRenderer(this.Game);
      this.LevelManager.LevelChanged += new Action(this.Hide);
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.textMesh.Effect = this.canvasMesh.Effect = (BaseEffect) new DefaultEffect.Textured();
      this.tailGroup.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/speech_bubble/SpeechBubbleTail");
      this.neGroup.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/speech_bubble/SpeechBubbleNE");
      this.nwGroup.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/speech_bubble/SpeechBubbleNW");
      this.seGroup.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/speech_bubble/SpeechBubbleSE");
      this.swGroup.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/speech_bubble/SpeechBubbleSW");
      this.scalableBottom.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/FullBlack");
      this.scalableMiddle.Texture = this.scalableTop.Texture = this.scalableBottom.Texture;
      this.zuishFont = this.CMProvider.Global.Load<SpriteFont>("Fonts/Zuish");
      ++this.zuishFont.LineSpacing;
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
    }

    protected override void UnloadContent()
    {
      if (this.text == null)
        return;
      TextureExtensions.Unhook((Texture) this.text);
      this.text.Dispose();
    }

    private void OnTextChanged(bool update)
    {
      float num1 = 2f;
      string str = this.textString;
      this.textString = this.originalString;
      SpriteFont spriteFont = this.Font == SpeechFont.Pixel ? this.FontManager.Big : this.zuishFont;
      if (this.Font == SpeechFont.Zuish)
        this.textString = this.textString.Replace(" ", "  ");
      float scale = !Culture.IsCJK || this.Font != SpeechFont.Pixel ? 1f : this.FontManager.SmallFactor;
      bool flag1 = this.GraphicsDevice.DisplayMode.Width < 1280 && this.Font == SpeechFont.Pixel;
      float num2 = 0.0f;
      if (this.Font != SpeechFont.Zuish)
      {
        float num3 = update ? 0.9f : 0.85f;
        num2 = FezMath.Dot(this.Origin - this.CameraManager.InterpolatedCenter, FezMath.RightVector(this.CameraManager.Viewpoint));
        float val1 = flag1 ? Math.Max((float) (-(double) num2 * 16.0 * (double) this.CameraManager.PixelsPerTrixel + 640.0 * (double) num3), 50f) * 0.6666667f : Math.Max((float) (-(double) num2 * 16.0 * (double) this.CameraManager.PixelsPerTrixel + 640.0 * (double) num3), 50f) / (this.CameraManager.PixelsPerTrixel / 2f);
        if (this.GameState.InMap)
          val1 = 500f;
        float num4 = Math.Max(val1, 70f);
        List<GlyphTextRenderer.FilledInGlyph> glyphLocations;
        string text = this.GTR.FillInGlyphs(this.textString, out glyphLocations);
        if (Culture.IsCJK)
          scale /= 2f;
        StringBuilder stringBuilder = new StringBuilder(WordWrap.Split(text, spriteFont, num4 / scale));
        if (Culture.IsCJK)
          scale *= 2f;
        bool flag2 = true;
        int index1 = 0;
        for (int index2 = 0; index2 < stringBuilder.Length; ++index2)
        {
          if (flag2 && (int) stringBuilder[index2] == 94)
          {
            for (int startIndex = index2; startIndex < index2 + glyphLocations[index1].Length; ++startIndex)
            {
              if ((int) stringBuilder[startIndex] == 13 || (int) stringBuilder[startIndex] == 10)
              {
                stringBuilder.Remove(startIndex, 1);
                --startIndex;
              }
            }
            stringBuilder.Remove(index2, glyphLocations[index1].Length);
            stringBuilder.Insert(index2, glyphLocations[index1].OriginalGlyph);
            ++index1;
          }
          else
            flag2 = (int) stringBuilder[index2] == 32 || (int) stringBuilder[index2] == 13 || (int) stringBuilder[index2] == 10;
        }
        this.textString = ((object) stringBuilder).ToString();
        if (!update)
          this.distanceFromCenterAtTextChange = num2;
      }
      if (update && (str == this.textString || (double) Math.Abs(this.distanceFromCenterAtTextChange - num2) < 1.5))
      {
        this.textString = str;
      }
      else
      {
        if (Culture.IsCJK && this.Font == SpeechFont.Pixel)
        {
          if ((double) SettingsManager.GetViewScale(this.GraphicsDevice) < 1.5)
          {
            spriteFont = this.FontManager.Small;
          }
          else
          {
            spriteFont = this.FontManager.Big;
            scale /= 2f;
          }
          scale *= num1;
        }
        bool multilineGlyphs;
        Vector2 vector2_1 = this.GTR.MeasureWithGlyphs(spriteFont, this.textString, scale, out multilineGlyphs);
        if (!Culture.IsCJK && multilineGlyphs)
        {
          spriteFont.LineSpacing += 8;
          bool flag2 = multilineGlyphs;
          vector2_1 = this.GTR.MeasureWithGlyphs(spriteFont, this.textString, scale, out multilineGlyphs);
          multilineGlyphs = flag2;
        }
        float num3 = 1f;
        if (Culture.IsCJK && this.Font == SpeechFont.Pixel)
          num3 = num1;
        this.scalableMiddleSize = vector2_1 + Vector2.One * 4f * 2f * num3 + Vector2.UnitX * 4f * 2f * num3;
        if (this.Font == SpeechFont.Zuish)
          this.scalableMiddleSize += Vector2.UnitY * 2f;
        int width = (int) this.scalableMiddleSize.X;
        int height = (int) this.scalableMiddleSize.Y;
        if (Culture.IsCJK && this.Font == SpeechFont.Pixel)
        {
          scale *= 2f;
          width *= 2;
          height *= 2;
        }
        Vector2 vector2_2 = this.scalableMiddleSize;
        if (this.text != null)
        {
          TextureExtensions.Unhook((Texture) this.text);
          this.text.Dispose();
        }
        this.text = new RenderTarget2D(this.GraphicsDevice, width, height, false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, this.GraphicsDevice.PresentationParameters.DepthStencilFormat, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
        this.GraphicsDevice.SetRenderTarget(this.text);
        GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
        this.GraphicsDevice.Clear(ClearOptions.Target, ColorEx.TransparentWhite, 1f, 0);
        Vector2 vector2_3 = Culture.IsCJK ? new Vector2(4f * num1) : Vector2.Zero;
        if (Culture.IsCJK)
          GraphicsDeviceExtensions.BeginLinear(this.spriteBatch);
        else
          GraphicsDeviceExtensions.BeginPoint(this.spriteBatch);
        if (this.Font == SpeechFont.Pixel)
          this.GTR.DrawString(this.spriteBatch, spriteFont, this.textString, FezMath.Round(vector2_2 / 2f - vector2_1 / 2f + vector2_3), this.TextColor, scale);
        else
          this.spriteBatch.DrawString(spriteFont, this.textString, vector2_2 / 2f - vector2_1 / 2f, this.TextColor, 0.0f, Vector2.Zero, this.scalableMiddleSize / vector2_2, SpriteEffects.None, 0.0f);
        this.spriteBatch.End();
        this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
        if (this.Font == SpeechFont.Zuish)
        {
          float num4 = this.scalableMiddleSize.X;
          this.scalableMiddleSize.X = this.scalableMiddleSize.Y;
          this.scalableMiddleSize.Y = num4;
        }
        if (Culture.IsCJK && this.Font == SpeechFont.Pixel)
          this.scalableMiddleSize /= num1;
        this.scalableMiddleSize /= 16f;
        this.scalableMiddleSize -= Vector2.One;
        this.textMesh.SamplerState = !Culture.IsCJK || this.Font != SpeechFont.Pixel ? SamplerState.PointClamp : SamplerState.AnisotropicClamp;
        this.textGroup.Texture = (Texture) this.text;
        this.oldCamPos = this.CameraManager.InterpolatedCenter;
        this.lastUsedOrigin = this.Origin;
        if (Culture.IsCJK || !multilineGlyphs)
          return;
        spriteFont.LineSpacing -= 8;
      }
    }

    private void UpdateBTexture()
    {
      SpriteFont small = this.FontManager.Small;
      Vector2 vector2 = small.MeasureString(this.GTR.FillInGlyphs(" {B} ")) * FezMath.Saturate(this.FontManager.SmallFactor);
      if (this.bTexture != null)
        this.bTexture.Dispose();
      this.bTexture = new RenderTarget2D(this.GraphicsDevice, (int) vector2.X, (int) vector2.Y, false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, this.GraphicsDevice.PresentationParameters.DepthStencilFormat, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
      this.GraphicsDevice.SetRenderTarget(this.bTexture);
      GraphicsDeviceExtensions.PrepareDraw(this.GraphicsDevice);
      this.GraphicsDevice.Clear(ClearOptions.Target, ColorEx.TransparentWhite, 1f, 0);
      GraphicsDeviceExtensions.BeginPoint(this.spriteBatch);
      this.GTR.DrawString(this.spriteBatch, small, " {B} ", new Vector2(0.0f, 0.0f), Color.White, FezMath.Saturate(this.FontManager.SmallFactor));
      this.spriteBatch.End();
      this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      this.bGroup.Texture = (Texture) this.bTexture;
      float num = Culture.IsCJK ? 25f : 24f;
      this.bGroup.Scale = new Vector3(vector2.X / num, vector2.Y / num, 1f);
      if (this.bGroup.Material != null)
        return;
      this.bGroup.Material = new Material();
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.show || this.changingText || FezMath.AlmostEqual(this.CameraManager.InterpolatedCenter, this.oldCamPos, 1.0 / 16.0))
        return;
      this.OnTextChanged(true);
    }

    public override void Draw(GameTime gameTime)
    {
      bool flag1 = this.show;
      if (this.show && this.changingText)
        flag1 = false;
      if (!flag1 && (double) this.sinceShown > 1.0)
        this.sinceShown = 1f;
      this.sinceShown += (float) (gameTime.ElapsedGameTime.TotalSeconds * (flag1 ? 1.0 : -2.0) * 5.0);
      if ((double) this.sinceShown < 0.0)
        this.sinceShown = 0.0f;
      if ((double) this.sinceShown == 0.0 && !flag1)
      {
        if (this.changingText || this.Font != SpeechFont.Zuish)
          return;
        this.Font = SpeechFont.Pixel;
      }
      else
      {
        this.scalableBottom.Scale = new Vector3(this.scalableMiddleSize.X, 1f, 1f);
        this.seGroup.Position = new Vector3(this.scalableMiddleSize.X + 0.5f, 0.0f, 0.0f);
        this.scalableMiddle.Scale = new Vector3(this.scalableMiddleSize.X + 1f, this.scalableMiddleSize.Y, 1f);
        this.nwGroup.Position = new Vector3(0.0f, this.scalableMiddleSize.Y + 0.5f, 0.0f);
        this.scalableTop.Position = new Vector3(0.5f, this.nwGroup.Position.Y, 0.0f);
        this.scalableTop.Scale = this.scalableBottom.Scale;
        this.neGroup.Position = new Vector3(this.seGroup.Position.X, this.nwGroup.Position.Y, 0.0f);
        bool flag2 = (this.GraphicsDevice.DisplayMode.Width < 1280 || this.GameState.InMap) && this.Font == SpeechFont.Pixel;
        if ((double) this.CameraManager.PixelsPerTrixel != 3.0 && !flag2)
          this.seGroup.Scale = new Vector3(1f, 1f, 1f);
        float x = (float) (3.0 * (double) this.bGroup.Scale.X / 4.0);
        float y = 0.0f;
        if (Culture.IsCJK)
        {
          x /= 2f;
          y = 0.25f;
        }
        this.bGroup.Position = this.seGroup.Position + new Vector3(0.5f, -0.5f, 0.0f) - new Vector3(x, y, 0.0f);
        float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
        float num = flag2 ? (float) (0.5 * (double) this.CameraManager.Radius / 26.6666660308838) / viewScale : 0.5f;
        this.canvasMesh.Scale = new Vector3(num);
        this.tailGroup.Scale = new Vector3(this.Font == SpeechFont.Zuish ? -1f : 1f, 1f, 1f);
        this.canvasMesh.Rotation = Quaternion.Normalize(this.CameraManager.Rotation);
        this.canvasMesh.Position = this.Origin + this.canvasMesh.WorldMatrix.Left * 2f + Vector3.UnitY * 0.65f;
        this.canvasMesh.Position = FezMath.Round(this.canvasMesh.Position * 16f * this.CameraManager.PixelsPerTrixel) / 16f / this.CameraManager.PixelsPerTrixel;
        this.textMesh.Scale = this.Font != SpeechFont.Zuish ? new Vector3(this.scalableMiddleSize.X + 1f, this.scalableMiddleSize.Y + 1f, 1f) * num : new Vector3(this.scalableMiddleSize.Y + 1f, this.scalableMiddleSize.X + 1f, 1f) * num;
        this.textMesh.Rotation = this.canvasMesh.Rotation;
        if (this.Font == SpeechFont.Zuish)
          this.textMesh.Rotation *= Quaternion.CreateFromYawPitchRoll(0.0f, 0.0f, -1.570796f);
        this.textMesh.Position = this.canvasMesh.Position;
        if (this.Font == SpeechFont.Zuish)
          this.textMesh.Position += (this.scalableMiddleSize.Y + 1f) * Vector3.UnitY / 2f;
        this.canvasMesh.Material.Opacity = FezMath.Saturate(this.sinceShown);
        this.textMesh.Material.Opacity = FezMath.Saturate(this.sinceShown);
        this.bGroup.Material.Opacity = !flag1 ? Math.Min(this.bGroup.Material.Opacity, FezMath.Saturate(this.sinceShown)) : FezMath.Saturate(this.sinceShown - (float) ((0.0750000029802322 * (double) Util.StripPunctuation(this.textString).Length + 2.0) * 2.0));
        this.canvasMesh.Draw();
        this.textMesh.Draw();
      }
    }

    public void ForceDrawOrder(int drawOrder)
    {
      this.DrawOrder = drawOrder;
      this.OnDrawOrderChanged((object) this, EventArgs.Empty);
    }

    public void RevertDrawOrder()
    {
      this.DrawOrder = 150;
      this.OnDrawOrderChanged((object) this, EventArgs.Empty);
    }
  }
}
