// Type: FezEngine.Components.GlyphTextRenderer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.Localization;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FezEngine.Components
{
  public class GlyphTextRenderer
  {
    private static readonly Dictionary<string, MappedAction> ActionMap = new Dictionary<string, MappedAction>()
    {
      {
        "BACK",
        MappedAction.OpenMap
      },
      {
        "START",
        MappedAction.Pause
      },
      {
        "A",
        MappedAction.Jump
      },
      {
        "B",
        MappedAction.CancelTalk
      },
      {
        "X",
        MappedAction.GrabThrow
      },
      {
        "Y",
        MappedAction.OpenInventory
      },
      {
        "RB",
        MappedAction.MapZoomIn
      },
      {
        "LB",
        MappedAction.MapZoomOut
      },
      {
        "RT",
        MappedAction.RotateRight
      },
      {
        "LT",
        MappedAction.RotateLeft
      },
      {
        "UP",
        MappedAction.Up
      },
      {
        "DOWN",
        MappedAction.Down
      },
      {
        "LEFT",
        MappedAction.Left
      },
      {
        "RIGHT",
        MappedAction.Right
      }
    };
    private readonly Dictionary<string, GlyphTextRenderer.GlyphDescription> ButtonImages = new Dictionary<string, GlyphTextRenderer.GlyphDescription>();
    private readonly List<GlyphTextRenderer.GlyphLocation> GlyphLocations = new List<GlyphTextRenderer.GlyphLocation>();
    public const float SafeArea = 0.15f;
    private readonly Game game;
    private bool bigGlyphDetected;

    public Vector2 Margin
    {
      get
      {
        return FezMath.Round(new Vector2((float) ((double) this.game.GraphicsDevice.Viewport.Width * 0.150000005960464 / 2.0), (float) ((double) this.game.GraphicsDevice.Viewport.Height * 0.150000005960464 / 2.0)));
      }
    }

    public Vector2 SafeViewportSize
    {
      get
      {
        return FezMath.Round(new Vector2((float) this.game.GraphicsDevice.Viewport.Width * 0.85f, (float) this.game.GraphicsDevice.Viewport.Height * 0.85f));
      }
    }

    static GlyphTextRenderer()
    {
    }

    public GlyphTextRenderer(Game game)
    {
      this.game = game;
      ContentManager global = ServiceHelper.Get<IContentManagerProvider>().Global;
      string str1 = "Other Textures/glyphs/";
      GlyphTextRenderer.GlyphMetadata md1 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 1,
        SpacesAfter = 1,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md2 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 1,
        SpacesAfter = 1,
        IsTall = true
      };
      GlyphTextRenderer.GlyphMetadata md3 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 2,
        SpacesAfter = 2,
        IsTall = true
      };
      GlyphTextRenderer.GlyphMetadata md4 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 2,
        SpacesAfter = 2,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md5 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 2,
        SpacesAfter = 1,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md6 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 3,
        SpacesAfter = 2,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md7 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 3,
        SpacesAfter = 3,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md8 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 4,
        SpacesAfter = 4,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md9 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 5,
        SpacesAfter = 5,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md10 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 6,
        SpacesAfter = 6,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md11 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 7,
        SpacesAfter = 7,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md12 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 8,
        SpacesAfter = 8,
        IsTall = false
      };
      GlyphTextRenderer.GlyphMetadata md13 = new GlyphTextRenderer.GlyphMetadata()
      {
        SpacesBefore = 9,
        SpacesAfter = 9,
        IsTall = false
      };
      this.ButtonImages.Add("BACK", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "BackButton"), md5));
      this.ButtonImages.Add("START", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "StartButton"), md5));
      this.ButtonImages.Add("A", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "AButton"), md1));
      this.ButtonImages.Add("B", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "BButton"), md1));
      this.ButtonImages.Add("X", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "XButton"), md1));
      this.ButtonImages.Add("Y", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "YButton"), md1));
      this.ButtonImages.Add("RB", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "RightBumper"), md6));
      this.ButtonImages.Add("LB", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "LeftBumper"), md6));
      this.ButtonImages.Add("RT", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "RightTrigger"), md2));
      this.ButtonImages.Add("LT", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "LeftTrigger"), md2));
      this.ButtonImages.Add("LS", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "LeftStick"), md3));
      this.ButtonImages.Add("RS", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "RightStick"), md3));
      this.ButtonImages.Add("UP", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "DPadUp"), md3));
      this.ButtonImages.Add("DOWN", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "DPadDown"), md3));
      this.ButtonImages.Add("LEFT", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "DPadLeft"), md3));
      this.ButtonImages.Add("RIGHT", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "DPadRight"), md3));
      this.ButtonImages.Add("LA", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "LeftArrow"), md1));
      this.ButtonImages.Add("RA", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str1 + "RightArrow"), md1));
      string str2 = "Other Textures/keyboard_glyphs/";
      this.ButtonImages.Add("P_D0", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_0"), md1));
      this.ButtonImages.Add("P_D1", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_1"), md1));
      this.ButtonImages.Add("P_D2", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_2"), md1));
      this.ButtonImages.Add("P_D3", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_3"), md1));
      this.ButtonImages.Add("P_D4", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_4"), md1));
      this.ButtonImages.Add("P_D5", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_5"), md1));
      this.ButtonImages.Add("P_D6", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_6"), md1));
      this.ButtonImages.Add("P_D7", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_7"), md1));
      this.ButtonImages.Add("P_D8", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_8"), md1));
      this.ButtonImages.Add("P_D9", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_9"), md1));
      this.ButtonImages.Add("P_Q", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_Q"), md1));
      this.ButtonImages.Add("P_W", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_W"), md1));
      this.ButtonImages.Add("P_E", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_E"), md1));
      this.ButtonImages.Add("P_R", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_R"), md1));
      this.ButtonImages.Add("P_T", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_T"), md1));
      this.ButtonImages.Add("P_Y", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_Y"), md1));
      this.ButtonImages.Add("P_U", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_U"), md1));
      this.ButtonImages.Add("P_I", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_I"), md1));
      this.ButtonImages.Add("P_O", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_O"), md1));
      this.ButtonImages.Add("P_P", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_P"), md1));
      this.ButtonImages.Add("P_A", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_A"), md1));
      this.ButtonImages.Add("P_S", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_S"), md1));
      this.ButtonImages.Add("P_D", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_D"), md1));
      this.ButtonImages.Add("P_F", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F"), md1));
      this.ButtonImages.Add("P_G", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_G"), md1));
      this.ButtonImages.Add("P_H", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_H"), md1));
      this.ButtonImages.Add("P_J", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_J"), md1));
      this.ButtonImages.Add("P_K", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_K"), md1));
      this.ButtonImages.Add("P_L", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_L"), md1));
      this.ButtonImages.Add("P_Z", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_Z"), md1));
      this.ButtonImages.Add("P_X", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_X"), md1));
      this.ButtonImages.Add("P_C", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_C"), md1));
      this.ButtonImages.Add("P_V", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_V"), md1));
      this.ButtonImages.Add("P_B", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_B"), md1));
      this.ButtonImages.Add("P_N", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_N"), md1));
      this.ButtonImages.Add("P_M", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_M"), md1));
      this.ButtonImages.Add("P_NumLock", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_LOCK"), md11));
      this.ButtonImages.Add("P_NumPad0", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_0"), md8));
      this.ButtonImages.Add("P_NumPad1", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_1"), md8));
      this.ButtonImages.Add("P_NumPad2", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_2"), md8));
      this.ButtonImages.Add("P_NumPad3", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_3"), md8));
      this.ButtonImages.Add("P_NumPad4", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_4"), md8));
      this.ButtonImages.Add("P_NumPad5", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_5"), md8));
      this.ButtonImages.Add("P_NumPad6", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_6"), md8));
      this.ButtonImages.Add("P_NumPad7", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_7"), md8));
      this.ButtonImages.Add("P_NumPad8", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_8"), md8));
      this.ButtonImages.Add("P_NumPad9", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_NUM_9"), md8));
      this.ButtonImages.Add("P_Up", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_ARROW_UP"), md1));
      this.ButtonImages.Add("P_Down", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_ARROW_DOWN"), md1));
      this.ButtonImages.Add("P_Left", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_ARROW_LEFT"), md1));
      this.ButtonImages.Add("P_Right", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_ARROW_RIGHT"), md1));
      this.ButtonImages.Add("P_Home", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_HOME"), md8));
      this.ButtonImages.Add("P_End", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_END"), md7));
      this.ButtonImages.Add("P_PageUp", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_PAGE_UP"), md7));
      this.ButtonImages.Add("P_PageDown", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_PAGE_DOWN"), md9));
      this.ButtonImages.Add("P_Insert", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_INSERT"), md9));
      this.ButtonImages.Add("P_Scroll", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_SCROLL"), md9));
      this.ButtonImages.Add("P_RightAlt", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_R_ALT"), md8));
      this.ButtonImages.Add("P_RightControl", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_R_CONTROL"), md12));
      this.ButtonImages.Add("P_RightWindows", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_R_WINDOWS"), md12));
      this.ButtonImages.Add("P_RightShift", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_R_SHIFT"), md10));
      this.ButtonImages.Add("P_LeftAlt", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_L_ALT"), md8));
      this.ButtonImages.Add("P_LeftControl", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_L_CONTROL"), md12));
      this.ButtonImages.Add("P_LeftWindows", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_L_WINDOWS"), md12));
      this.ButtonImages.Add("P_LeftShift", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_L_SHIFT"), md10));
      this.ButtonImages.Add("P_Escape", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_ESCAPE"), md10));
      this.ButtonImages.Add("P_F1", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F1"), md4));
      this.ButtonImages.Add("P_F2", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F2"), md4));
      this.ButtonImages.Add("P_F3", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F3"), md4));
      this.ButtonImages.Add("P_F4", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F4"), md4));
      this.ButtonImages.Add("P_F5", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F5"), md4));
      this.ButtonImages.Add("P_F6", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F6"), md4));
      this.ButtonImages.Add("P_F7", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F7"), md4));
      this.ButtonImages.Add("P_F8", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F8"), md4));
      this.ButtonImages.Add("P_F9", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F9"), md4));
      this.ButtonImages.Add("P_F10", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F10"), md7));
      this.ButtonImages.Add("P_F11", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F11"), md7));
      this.ButtonImages.Add("P_F12", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_F12"), md7));
      this.ButtonImages.Add("P_OemPeriod", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_PERIOD"), md1));
      this.ButtonImages.Add("P_OemComma", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_COMMA"), md1));
      this.ButtonImages.Add("P_OemSemicolon", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_SEMICOLON"), md1));
      this.ButtonImages.Add("P_OemQuotes", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_QUOTES"), md1));
      this.ButtonImages.Add("P_OemTilde", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_TILDE"), md1));
      this.ButtonImages.Add("P_OemOpenBrackets", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_R_BRACKET"), md1));
      this.ButtonImages.Add("P_OemCloseBrackets", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_L_BRACKET"), md1));
      this.ButtonImages.Add("P_OemPipe", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_PIPE"), md1));
      this.ButtonImages.Add("P_OemQuestion", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_QUESTION"), md1));
      this.ButtonImages.Add("P_Divide", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_DIVIDE"), md1));
      this.ButtonImages.Add("P_Add", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_PLUS"), md1));
      this.ButtonImages.Add("P_OemMinus", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_MINUS"), md1));
      this.ButtonImages.Add("P_Multiply", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_MULTIPLY"), md1));
      this.ButtonImages.Add("P_OemPlus", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_EQUAL"), md1));
      this.ButtonImages.Add("P_Space", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_SPACE"), md9));
      this.ButtonImages.Add("P_Tab", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_TAB"), md7));
      this.ButtonImages.Add("P_Back", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_BACKSPACE"), md13));
      this.ButtonImages.Add("P_Delete", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_DELETE"), md10));
      this.ButtonImages.Add("P_CapsLock", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_CAPS"), md13));
      this.ButtonImages.Add("P_Enter", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_ENTER"), md9));
      this.ButtonImages.Add("P_WASD", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_WASD"), md9));
      this.ButtonImages.Add("P_IJKL", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_IJKL"), md9));
      this.ButtonImages.Add("P_Arrows", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_ARROWS"), md9));
      this.ButtonImages.Add("P_ZQSD", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_ZQSD"), md9));
      this.ButtonImages.Add("P_ESDF", new GlyphTextRenderer.GlyphDescription(global.Load<Texture2D>(str2 + "P_ESDF"), md9));
    }

    public void DrawString(SpriteBatch batch, SpriteFont font, string text, Vector2 position)
    {
      this.DrawString(batch, font, text, position, Color.White);
    }

    public void DrawString(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color)
    {
      this.DrawString(batch, font, text, position, color, 1f);
    }

    public void DrawString(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color, float scale)
    {
      text = this.FindButtonGlyphs(text);
      position = FezMath.Round(position);
      batch.DrawString(font, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
      if (Culture.IsCJK)
      {
        batch.End();
        GraphicsDeviceExtensions.BeginPoint(batch);
      }
      float num1 = font.MeasureString("X").Y;
      foreach (GlyphTextRenderer.GlyphLocation glyphLocation in this.GlyphLocations)
      {
        GlyphTextRenderer.GlyphDescription glyphDescription;
        if (this.ButtonImages.TryGetValue(glyphLocation.Glyph, out glyphDescription))
        {
          Texture2D texture = glyphDescription.Image;
          float num2 = (font.MeasureString(glyphLocation.UpToThere) * scale).Y;
          string text1 = glyphLocation.UpToThere.Substring(glyphLocation.UpToThere.LastIndexOf('\n') + 1);
          Vector2 vector2_1 = font.MeasureString(text1) * scale;
          vector2_1.Y = num2 - vector2_1.Y * 0.5f;
          Vector2 vector2_2 = position + vector2_1;
          float num3 = num1 / (float) texture.Height * scale;
          if (texture.Height <= 32)
          {
            float num4 = num3;
            num3 = (float) Math.Ceiling((double) num3);
            if ((double) num4 == (double) num3 && (double) SettingsManager.GetViewScale(this.game.GraphicsDevice) >= 2.0)
              ++num3;
          }
          if (Culture.IsCJK && Culture.Language != Language.Chinese)
            num3 *= 1.2f;
          if (Culture.IsCJK)
            vector2_2.Y -= (float) ((double) texture.Height * (double) num3 * 0.0500000007450581);
          Rectangle rectangle = new Rectangle(FezMath.Round((double) vector2_2.X - (double) texture.Width * (double) num3 / 2.0), FezMath.Round((double) vector2_2.Y - (double) texture.Height * (double) num3 / 2.0), FezMath.Round((double) texture.Width * (double) num3), FezMath.Round((double) texture.Height * (double) num3));
          batch.Draw(texture, rectangle, new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) color.A));
        }
      }
      if (!Culture.IsCJK)
        return;
      batch.End();
      GraphicsDeviceExtensions.BeginLinear(batch);
    }

    public Vector2 MeasureWithGlyphs(SpriteFont font, string text, float scale)
    {
      return font.MeasureString(this.FindButtonGlyphs(text)) * scale;
    }

    public Vector2 MeasureWithGlyphs(SpriteFont font, string text, float scale, out bool multilineGlyphs)
    {
      Vector2 vector2 = font.MeasureString(this.FindButtonGlyphs(text)) * scale;
      multilineGlyphs = text.IndexOf('\n') != -1 && this.bigGlyphDetected;
      return vector2;
    }

    private string TryReplaceGlyph(string glyph)
    {
      if (glyph == "RS")
      {
        switch (SettingsManager.Settings.KeyboardMapping[MappedAction.LookUp])
        {
          case Keys.I:
            glyph = "P_IJKL";
            break;
          case Keys.W:
            glyph = "P_WASD";
            break;
          case Keys.Z:
            glyph = "P_ZQSD";
            break;
          case Keys.Up:
            glyph = "P_Arrows";
            break;
          case Keys.E:
            glyph = "P_ESDF";
            break;
        }
      }
      else if (glyph == "LS")
      {
        if (ServiceHelper.Get<ILevelManager>().Name == "ELDERS")
        {
          glyph = "P_" + (object) SettingsManager.Settings.KeyboardMapping[MappedAction.FpViewToggle];
        }
        else
        {
          switch (SettingsManager.Settings.KeyboardMapping[MappedAction.Left])
          {
            case Keys.L:
              glyph = "P_IJKL";
              break;
            case Keys.Q:
              glyph = "P_ZQSD";
              break;
            case Keys.S:
              glyph = "P_ESDF";
              break;
            case Keys.Left:
              glyph = "P_Arrows";
              break;
            case Keys.A:
              glyph = "P_WASD";
              break;
          }
        }
      }
      else
      {
        MappedAction index;
        if (GlyphTextRenderer.ActionMap.TryGetValue(glyph, out index))
          glyph = "P_" + (object) SettingsManager.Settings.KeyboardMapping[index];
      }
      return glyph;
    }

    private string FindButtonGlyphs(string text)
    {
      this.GlyphLocations.Clear();
      this.bigGlyphDetected = false;
      int num1;
      for (; (num1 = text.IndexOf("{")) != -1 && text.IndexOf("}", num1) != -1; {
        int num2;
        string str1;
        string str2;
        text = str1 + str2 + text.Substring(num2 + 1);
      }
      )
      {
        num2 = text.IndexOf('}', num1);
        string str = text.Substring(num1 + 1, num2 - num1 - 1);
        str1 = text.Substring(0, num1);
        if (!GamepadState.AnyConnected)
          str = this.TryReplaceGlyph(str);
        str2 = "";
        GlyphTextRenderer.GlyphDescription glyphDescription;
        if (this.ButtonImages.TryGetValue(str, out glyphDescription))
        {
          this.bigGlyphDetected = glyphDescription.Metadata.IsTall;
          str2 = new string(' ', glyphDescription.Metadata.SpacesAfter);
          str1 = str1 + new string(' ', glyphDescription.Metadata.SpacesBefore);
        }
        this.GlyphLocations.Add(new GlyphTextRenderer.GlyphLocation()
        {
          UpToThere = str1,
          Glyph = str
        });
      }
      return text;
    }

    public Texture2D GetReplacedGlyphTexture(string glyph)
    {
      glyph = glyph.Substring(1, glyph.Length - 2);
      if (!GamepadState.AnyConnected)
        glyph = this.TryReplaceGlyph(glyph);
      return this.ButtonImages[glyph].Image;
    }

    public string FillInGlyphs(string text, out List<GlyphTextRenderer.FilledInGlyph> glyphLocations)
    {
      glyphLocations = new List<GlyphTextRenderer.FilledInGlyph>();
      this.bigGlyphDetected = false;
      int num1;
      for (; (num1 = text.IndexOf("{")) != -1; {
        int num2;
        string str1;
        string str2;
        text = str1 + str2 + text.Substring(num2 + 1);
      }
      )
      {
        num2 = text.IndexOf('}', num1);
        string str1 = text.Substring(num1 + 1, num2 - num1 - 1);
        str1 = text.Substring(0, num1);
        string str2 = str1;
        if (!GamepadState.AnyConnected)
          str1 = this.TryReplaceGlyph(str1);
        str2 = "";
        int num2 = 0;
        GlyphTextRenderer.GlyphDescription glyphDescription;
        if (this.ButtonImages.TryGetValue(str1, out glyphDescription))
        {
          this.bigGlyphDetected = glyphDescription.Metadata.IsTall;
          str2 = new string('^', glyphDescription.Metadata.SpacesAfter);
          str1 = str1 + new string('^', glyphDescription.Metadata.SpacesBefore);
          num2 = glyphDescription.Metadata.SpacesAfter + glyphDescription.Metadata.SpacesBefore;
        }
        glyphLocations.Add(new GlyphTextRenderer.FilledInGlyph()
        {
          Length = num2,
          OriginalGlyph = "{" + str2 + "}"
        });
      }
      return text;
    }

    public string FillInGlyphs(string text)
    {
      this.bigGlyphDetected = false;
      int num1;
      for (; (num1 = text.IndexOf("{")) != -1; {
        int num2;
        string str1;
        string str2;
        text = str1 + str2 + text.Substring(num2 + 1);
      }
      )
      {
        num2 = text.IndexOf('}', num1);
        string str = text.Substring(num1 + 1, num2 - num1 - 1);
        str1 = text.Substring(0, num1);
        if (!GamepadState.AnyConnected)
          str = this.TryReplaceGlyph(str);
        str2 = "";
        GlyphTextRenderer.GlyphDescription glyphDescription;
        if (this.ButtonImages.TryGetValue(str, out glyphDescription))
        {
          this.bigGlyphDetected = glyphDescription.Metadata.IsTall;
          str2 = new string('^', glyphDescription.Metadata.SpacesAfter);
          str1 = str1 + new string('^', glyphDescription.Metadata.SpacesBefore);
        }
      }
      return text;
    }

    public void DrawShadowedText(SpriteBatch batch, SpriteFont font, string text, Vector2 position)
    {
      this.DrawShadowedText(batch, font, text, position, Color.White);
    }

    public void DrawShadowedText(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color)
    {
      this.DrawShadowedText(batch, font, text, position, color, 1f);
    }

    public void DrawShadowedText(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color, float scale)
    {
      this.DrawShadowedText(batch, font, text, position, color, scale, Color.Black, 0.3333333f, 1f);
    }

    public void DrawShadowedText(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color, float scale, Color shadowColor, float shadowOpacity, float shadowOffset)
    {
      this.DrawString(batch, font, text, position + shadowOffset * Vector2.One, new Color((int) shadowColor.R, (int) shadowColor.G, (int) shadowColor.B, (int) (byte) ((double) shadowOpacity * ((double) color.A / (double) byte.MaxValue) * (double) byte.MaxValue)), scale);
      this.DrawString(batch, font, text, position, color, scale);
    }

    public void DrawBloomedText(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color, Color bloomColor, float bloomOpacity)
    {
      bloomColor = new Color((int) bloomColor.R, (int) bloomColor.G, (int) bloomColor.B, (int) (byte) ((double) bloomOpacity * (double) byte.MaxValue * (double) color.A / (double) byte.MaxValue));
      this.DrawString(batch, font, text, position + Vector2.One, bloomColor);
      this.DrawString(batch, font, text, position - Vector2.One, bloomColor);
      this.DrawString(batch, font, text, position + new Vector2(-1f, 1f), bloomColor);
      this.DrawString(batch, font, text, position + new Vector2(1f, -1f), bloomColor);
      this.DrawString(batch, font, text, position, color);
    }

    public void DrawStringLF(SpriteBatch batch, SpriteFont font, string text, Color color, Vector2 offset, float scale)
    {
      string[] strArray = text.Split(new char[1]
      {
        '\r'
      });
      float y = 0.0f;
      foreach (string str in strArray)
      {
        string text1 = str.Trim();
        this.DrawString(batch, font, text1, new Vector2(0.0f, y) + offset, color, scale);
        y += (float) ((double) font.MeasureString(text1).Y * (double) scale * 1.0 / 3.0 + 15.0);
      }
    }

    public void DrawStringLFLeftAlign(SpriteBatch batch, SpriteFont font, string text, Color color, Vector2 offset, float scale)
    {
      string[] strArray = text.Split(new char[1]
      {
        '\r'
      });
      float y1 = 0.0f;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      foreach (string str in strArray)
      {
        Vector2 vector2 = this.MeasureWithGlyphs(font, str.Trim(), scale);
        num1 = Math.Max(vector2.X, num1);
        num2 += vector2.Y;
        num3 = vector2.Y;
      }
      float y2 = num2 + num3;
      offset -= new Vector2(num1, y2);
      foreach (string str in strArray)
      {
        string text1 = str.Trim();
        Vector2 vector2 = this.MeasureWithGlyphs(font, text1, scale);
        this.DrawString(batch, font, text1, new Vector2(num1 - vector2.X, y1) + offset, color, scale);
        y1 += vector2.Y;
      }
    }

    public void DrawCenteredString(SpriteBatch batch, SpriteFont font, string text, Color color, Vector2 offset, float scale)
    {
      this.DrawCenteredString(batch, font, text, color, offset, scale, true);
    }

    public void DrawCenteredString(SpriteBatch batch, SpriteFont font, string text, Color color, Vector2 offset, float scale, bool shadow)
    {
      float num1 = (float) this.game.GraphicsDevice.Viewport.Width * 0.8f;
      text = WordWrap.Split(text, font, (num1 - offset.X / 2f) / scale);
      float num2 = font.MeasureString(text).Y * scale;
      int startIndex1 = 0;
      int num3 = 0;
      while (startIndex1 < text.Length && startIndex1 != -1)
      {
        startIndex1 = text.IndexOf(Environment.NewLine, startIndex1);
        if (startIndex1 != -1)
          startIndex1 += 2;
        ++num3;
      }
      int num4 = this.game.GraphicsDevice.Viewport.Width / 2;
      int startIndex2 = 0;
      Vector2 vector2 = offset + (float) num4 * Vector2.UnitX;
      while (startIndex2 < text.Length && startIndex2 != -1)
      {
        int num5 = text.IndexOf(Environment.NewLine, startIndex2);
        string text1 = text.Substring(startIndex2, num5 == -1 ? text.Length - startIndex2 : num5 - startIndex2);
        float num6 = font.MeasureString(text1).X * scale;
        if (shadow)
          this.DrawString(batch, font, text1, vector2 - new Vector2(num6 / 2f, 0.0f) + new Vector2(1f, 1f), new Color(0, 0, 0, (int) (byte) ((uint) color.A / 2U)), scale);
        this.DrawString(batch, font, text1, vector2 - new Vector2(num6 / 2f, 0.0f), color, scale);
        vector2.Y += num2 / (float) num3;
        startIndex2 = num5;
        if (startIndex2 != -1)
          startIndex2 += 2;
      }
    }

    private struct GlyphDescription
    {
      public Texture2D Image;
      public GlyphTextRenderer.GlyphMetadata Metadata;

      public GlyphDescription(Texture2D image, GlyphTextRenderer.GlyphMetadata md)
      {
        this.Image = image;
        this.Metadata = md;
      }
    }

    private struct GlyphMetadata
    {
      public int SpacesBefore;
      public int SpacesAfter;
      public bool IsTall;
    }

    public struct GlyphLocation
    {
      public string UpToThere;
      public string Glyph;
    }

    public struct FilledInGlyph
    {
      public int Length;
      public string OriginalGlyph;
    }
  }
}
