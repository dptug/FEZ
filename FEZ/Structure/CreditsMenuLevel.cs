// Type: FezGame.Structure.CreditsMenuLevel
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Tools;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FezGame.Structure
{
  internal class CreditsMenuLevel : MenuLevel
  {
    private List<CreditsEntry> scroller;
    private float sinceStarted;
    private float destinationTime;
    private RenderTarget2D maskRT;
    private GraphicsDevice GraphicsDevice;
    private bool started;
    private float wasFactor;
    private SpriteBatch SpriteBatch;
    private GlyphTextRenderer gtr;
    private bool ready;

    public IFontManager FontManager { private get; set; }

    public override void Initialize()
    {
      base.Initialize();
      this.GraphicsDevice = ServiceHelper.Get<IGraphicsDeviceService>().GraphicsDevice;
      this.GraphicsDevice.DeviceReset += new EventHandler<EventArgs>(this.LoadCredits);
      this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.gtr = new GlyphTextRenderer(ServiceHelper.Game);
      this.OnClose = (Action) (() =>
      {
        if (this.started)
        {
          ServiceHelper.Get<ISoundManager>().PlayNewSong((string) null, 0.5f);
          ServiceHelper.Get<ISoundManager>().UnshelfSong();
          ServiceHelper.Get<ISoundManager>().MusicVolumeFactor = this.wasFactor;
        }
        if (this.maskRT != null)
        {
          this.maskRT.Dispose();
          this.maskRT = (RenderTarget2D) null;
        }
        this.started = false;
      });
      this.scroller = new List<CreditsEntry>()
      {
        new CreditsEntry()
        {
          Size = new Vector2(512f, 512f)
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("PolytronProduction"),
          IsTitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("Design"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("ArtLevelDesignCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("Programming"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("ProgrammingCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("Producer"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("ProducerCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("Music"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("MusicCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("SoundEffects"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("SoundEffectsCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("Animation"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("AnimationCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("PcQaTeam"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("PcQaTeamCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("SupportedBy"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("SupportedByCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("PhilSpecialThanks"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("PhilSpecialThanksCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("RenoSpecialThanks"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("RenoSpecialThanksCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("XblaSpecialThanks"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("XblaSpecialThanksCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("ThirdParty"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("ThirdPartyCredits")
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("LicenseInfo"),
          IsSubtitle = true
        },
        new CreditsEntry()
        {
          Text = CreditsText.GetString("PolytronFooter"),
          IsTitle = true
        }
      };
      this.LoadCredits();
      foreach (CreditsEntry creditsEntry in this.scroller)
      {
        if (creditsEntry.Text != null)
        {
          creditsEntry.Text = creditsEntry.Text.ToUpper(CultureInfo.InvariantCulture);
          creditsEntry.Size = !creditsEntry.IsTitle ? this.FontManager.Small.MeasureString(creditsEntry.Text) * this.FontManager.SmallFactor : this.FontManager.Big.MeasureString(creditsEntry.Text) * this.FontManager.BigFactor;
        }
      }
    }

    private void LoadCredits(object _, EventArgs __)
    {
      this.LoadCredits();
    }

    private void LoadCredits()
    {
      if (this.maskRT != null)
        this.maskRT.Dispose();
      this.maskRT = (RenderTarget2D) null;
      this.ready = false;
      ContentManager global = this.CMProvider.Global;
      bool flag = (double) SettingsManager.GetViewScale(this.GraphicsDevice) >= 1.5;
      this.scroller[0].Size = new Vector2(512f, 512f);
      this.scroller[0].Image = global.Load<Texture2D>("Other Textures/credits_logo" + (flag ? "_1440" : ""));
      this.ready = true;
    }

    public override void Dispose()
    {
      base.Dispose();
      this.GraphicsDevice.DeviceReset -= new EventHandler<EventArgs>(this.LoadCredits);
      if (this.maskRT != null)
        this.maskRT.Dispose();
      this.maskRT = (RenderTarget2D) null;
    }

    public override void Reset()
    {
      this.destinationTime = 0.0f;
      this.sinceStarted = -0.5f;
      if (!this.started)
      {
        ServiceHelper.Get<ISoundManager>().PlayNewSong("Gomez", 0.0f, false);
        this.wasFactor = ServiceHelper.Get<ISoundManager>().MusicVolumeFactor;
        ServiceHelper.Get<ISoundManager>().MusicVolumeFactor = 1f;
      }
      this.started = true;
    }

    public override void Update(TimeSpan elapsed)
    {
      if (!ServiceHelper.Game.IsActive || !this.ready)
        return;
      this.sinceStarted += (float) elapsed.TotalSeconds;
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      this.GraphicsDevice.SetRenderTarget(this.maskRT);
      this.GraphicsDevice.Clear(ClearOptions.Target, ColorEx.TransparentWhite, 1f, 0);
      GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
      float num1 = 27.5f * viewScale;
      SpriteFont small = this.FontManager.Small;
      float num2 = 0.0f;
      this.destinationTime = MathHelper.Lerp(this.destinationTime, Math.Max(this.sinceStarted, 0.0f), 0.01f);
      bool flag = true;
      foreach (CreditsEntry creditsEntry in this.scroller)
      {
        float num3 = num2 - num1 * this.destinationTime;
        if ((double) num3 + (double) creditsEntry.Size.Y * (double) viewScale > -50.0 * (double) viewScale && (double) num3 < 600.0 * (double) viewScale)
        {
          flag = false;
          if (creditsEntry.Image != null)
          {
            float num4 = FezMath.Saturate(this.sinceStarted * 2f);
            this.SpriteBatch.Draw(creditsEntry.Image, new Vector2(512f * viewScale - (float) FezMath.Round((double) creditsEntry.Size.X / 2.0 * (double) viewScale), (float) FezMath.Round(256.0 * (double) viewScale - (double) FezMath.Round((double) creditsEntry.Size.Y / 2.0) * (double) viewScale + (double) num3)), new Color(num4, num4, num4, num4));
            GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.All);
          }
          else
          {
            if (creditsEntry.IsTitle)
            {
              num3 += 60f * viewScale;
              num2 += 60f * viewScale;
            }
            if (creditsEntry.IsSubtitle)
            {
              num3 += 30f * viewScale;
              num2 += 30f * viewScale;
            }
            Color color = creditsEntry.IsSubtitle ? new Color(0.7f, 0.7f, 0.7f, 1f) : Color.White;
            float num4 = creditsEntry.IsTitle ? this.FontManager.BigFactor : this.FontManager.SmallFactor;
            this.gtr.DrawCenteredString(this.SpriteBatch, creditsEntry.IsTitle ? this.FontManager.Big : small, creditsEntry.Text, color, new Vector2(0.0f, (float) FezMath.Round((double) num3)), num4 * viewScale, true);
            if (creditsEntry.IsTitle)
              num2 += 15f * viewScale;
          }
        }
        else
        {
          if (creditsEntry.IsTitle)
            num2 += 75f * viewScale;
          if (creditsEntry.IsSubtitle)
            num2 += 30f * viewScale;
        }
        num2 += creditsEntry.Size.Y * viewScale;
      }
      if (flag)
        this.ForceCancel = true;
      this.SpriteBatch.End();
      this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
    }

    public override void PostDraw(SpriteBatch batch, SpriteFont font, GlyphTextRenderer tr, float alpha)
    {
      int width = batch.GraphicsDevice.Viewport.Width;
      int height = batch.GraphicsDevice.Viewport.Height;
      float viewScale = SettingsManager.GetViewScale(batch.GraphicsDevice);
      if (this.maskRT == null)
      {
        this.maskRT = new RenderTarget2D(this.GraphicsDevice, (int) (1024.0 * (double) viewScale), (int) (512.0 * (double) viewScale), false, this.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8, this.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PlatformContents);
        this.Update(TimeSpan.Zero);
      }
      batch.Draw((Texture2D) this.maskRT, new Vector2((float) ((double) width / 2.0 - 512.0 * (double) viewScale), (float) ((double) height / 2.0 - 256.0 * (double) viewScale)), Color.White);
    }
  }
}
