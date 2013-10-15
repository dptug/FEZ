// Type: FezGame.Structure.ControlsMenuLevel
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FezGame.Structure
{
  internal class ControlsMenuLevel : MenuLevel
  {
    private static readonly MappedAction[] KeyboardActionOrder = new MappedAction[14]
    {
      MappedAction.Jump,
      MappedAction.GrabThrow,
      MappedAction.CancelTalk,
      MappedAction.Up,
      MappedAction.LookUp,
      MappedAction.OpenMap,
      MappedAction.OpenInventory,
      MappedAction.Pause,
      MappedAction.RotateLeft,
      MappedAction.RotateRight,
      MappedAction.ClampLook,
      MappedAction.MapZoomIn,
      MappedAction.MapZoomOut,
      MappedAction.FpViewToggle
    };
    private static readonly MappedAction[] GamepadActionOrder = new MappedAction[12]
    {
      MappedAction.Jump,
      MappedAction.GrabThrow,
      MappedAction.CancelTalk,
      MappedAction.OpenMap,
      MappedAction.OpenInventory,
      MappedAction.Pause,
      MappedAction.RotateLeft,
      MappedAction.RotateRight,
      MappedAction.ClampLook,
      MappedAction.MapZoomIn,
      MappedAction.MapZoomOut,
      MappedAction.FpViewToggle
    };
    private HashSet<Keys> lastPressed = new HashSet<Keys>();
    private HashSet<Keys> thisPressed = new HashSet<Keys>();
    private readonly HashSet<Keys> keysDown = new HashSet<Keys>();
    private readonly MenuBase menuBase;
    private Texture2D ControlsImage;
    private SoundEffect sSliderValueIncrease;
    private SoundEffect sSliderValueDecrease;
    private int chosen;
    private MenuItem keyGrabFor;
    private bool forGamepad;
    private bool hasBeenIdle;
    private bool pendingApply;
    private bool noArrows;
    private int otherGamepadsStart;
    private int keyboardStart;
    private int selectorStart;
    private Rectangle? leftSliderRect;
    private Rectangle? rightSliderRect;

    public override string AButtonString
    {
      get
      {
        if (this.chosen != this.selectorStart && !this.SelectedItem.IsSlider && (this.SelectedIndex != this.chosen && this.SelectedItem.SuffixText != null))
          return StaticText.GetString("ChangeWithGlyph");
        else
          return (string) null;
      }
    }

    public IFontManager FontManager { private get; set; }

    public IInputManager InputManager { private get; set; }

    public IKeyboardStateManager KeyboardManager { private get; set; }

    public IMouseStateManager MouseState { private get; set; }

    public IGameStateManager GameState { private get; set; }

    static ControlsMenuLevel()
    {
    }

    public ControlsMenuLevel(MenuBase menuBase)
    {
      this.InputManager = ServiceHelper.Get<IInputManager>();
      this.GameState = ServiceHelper.Get<IGameStateManager>();
      this.KeyboardManager = ServiceHelper.Get<IKeyboardStateManager>();
      this.MouseState = ServiceHelper.Get<IMouseStateManager>();
      this.menuBase = menuBase;
      this.IsDynamic = true;
      Dictionary<MappedAction, Keys> kmap = SettingsManager.Settings.KeyboardMapping;
      Dictionary<MappedAction, int> gmap = SettingsManager.Settings.GamepadMapping;
      for (int index = 0; index < 9; ++index)
        this.AddItem((string) null, MenuBase.SliderAction, -1).Selectable = false;
      this.otherGamepadsStart = this.Items.Count;
      MenuItem gjmi = this.AddItem("ControlsJump", -1);
      gjmi.Selected = (Action) (() => this.ChangeButton(gjmi));
      gjmi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.Jump]));
      MenuItem gami = this.AddItem("ControlsAction", -1);
      gami.Selected = (Action) (() => this.ChangeButton(gami));
      gami.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.GrabThrow]));
      MenuItem gtmi = this.AddItem("ControlsTalk", -1);
      gtmi.Selected = (Action) (() => this.ChangeButton(gtmi));
      gtmi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.CancelTalk]));
      MenuItem gmami = this.AddItem("Map_Title", -1);
      gmami.Selected = (Action) (() => this.ChangeButton(gmami));
      gmami.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.OpenMap]));
      MenuItem gimi = this.AddItem("ControlsInventory", -1);
      gimi.Selected = (Action) (() => this.ChangeButton(gimi));
      gimi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.OpenInventory]));
      MenuItem gpmi = this.AddItem("ControlsPause", -1);
      gpmi.Selected = (Action) (() => this.ChangeButton(gpmi));
      gpmi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.Pause]));
      MenuItem grlmi = this.AddItem("ControlsRotateLeft", -1);
      grlmi.Selected = (Action) (() => this.ChangeButton(grlmi));
      grlmi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.RotateLeft]));
      MenuItem grrmi = this.AddItem("ControlsRotateRight", -1);
      grrmi.Selected = (Action) (() => this.ChangeButton(grrmi));
      grrmi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.RotateRight]));
      MenuItem gclmi = this.AddItem("ControlsClampLook", -1);
      gclmi.Selected = (Action) (() => this.ChangeButton(gclmi));
      gclmi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.ClampLook]));
      MenuItem gmzimi = this.AddItem("ControlsMapZoomIn", -1);
      gmzimi.Selected = (Action) (() => this.ChangeButton(gmzimi));
      gmzimi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.MapZoomIn]));
      MenuItem gmzomi = this.AddItem("ControlsZoomOut", -1);
      gmzomi.Selected = (Action) (() => this.ChangeButton(gmzomi));
      gmzomi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.MapZoomOut]));
      if (this.GameState.SaveData.HasFPView)
      {
        MenuItem gtfvmi = this.AddItem("ControlsToggleFpView", -1);
        gtfvmi.Selected = (Action) (() => this.ChangeButton(gtfvmi));
        gtfvmi.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gmap[MappedAction.FpViewToggle]));
      }
      this.AddItem((string) null, MenuBase.SliderAction, -1).Selectable = false;
      this.AddItem("ResetToDefault", (Action) (() => this.ResetToDefault(false, true)), -1);
      this.keyboardStart = this.Items.Count;
      MenuItem jmi = this.AddItem("ControlsJump", -1);
      jmi.Selected = (Action) (() => this.ChangeKey(jmi));
      jmi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.Jump]));
      MenuItem ami = this.AddItem("ControlsAction", -1);
      ami.Selected = (Action) (() => this.ChangeKey(ami));
      ami.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.GrabThrow]));
      MenuItem tmi = this.AddItem("ControlsTalk", -1);
      tmi.Selected = (Action) (() => this.ChangeKey(tmi));
      tmi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.CancelTalk]));
      this.AddItem<ControlsMenuLevel.ArrowKeyMapping>("ControlsMove", MenuBase.SliderAction, false, (Func<ControlsMenuLevel.ArrowKeyMapping>) (() => this.UpToAKM(kmap[MappedAction.Up])), (Action<ControlsMenuLevel.ArrowKeyMapping, int>) ((lastValue, change) =>
      {
        ControlsMenuLevel.ArrowKeyMapping local_0_1 = this.UpToAKM(kmap[MappedAction.Up]) + change;
        if (local_0_1 == (ControlsMenuLevel.ArrowKeyMapping) 5)
          local_0_1 = ControlsMenuLevel.ArrowKeyMapping.WASD;
        if (local_0_1 < ControlsMenuLevel.ArrowKeyMapping.WASD)
          local_0_1 = ControlsMenuLevel.ArrowKeyMapping.Arrows;
        kmap[MappedAction.Up] = this.AKMToKey(local_0_1, 0);
        kmap[MappedAction.Left] = this.AKMToKey(local_0_1, 1);
        kmap[MappedAction.Down] = this.AKMToKey(local_0_1, 2);
        kmap[MappedAction.Right] = this.AKMToKey(local_0_1, 3);
        this.KeyboardManager.UpdateMapping();
      }), -1).SuffixText = (Func<string>) (() => " : " + this.Localize((object) this.UpToAKM(kmap[MappedAction.Up])));
      this.AddItem<ControlsMenuLevel.ArrowKeyMapping>("ControlsLook", MenuBase.SliderAction, false, (Func<ControlsMenuLevel.ArrowKeyMapping>) (() => this.UpToAKM(kmap[MappedAction.LookUp])), (Action<ControlsMenuLevel.ArrowKeyMapping, int>) ((lastValue, change) =>
      {
        ControlsMenuLevel.ArrowKeyMapping local_0_1 = this.UpToAKM(kmap[MappedAction.LookUp]) + change;
        if (local_0_1 == (ControlsMenuLevel.ArrowKeyMapping) 5)
          local_0_1 = ControlsMenuLevel.ArrowKeyMapping.WASD;
        if (local_0_1 < ControlsMenuLevel.ArrowKeyMapping.WASD)
          local_0_1 = ControlsMenuLevel.ArrowKeyMapping.Arrows;
        kmap[MappedAction.LookUp] = this.AKMToKey(local_0_1, 0);
        kmap[MappedAction.LookLeft] = this.AKMToKey(local_0_1, 1);
        kmap[MappedAction.LookDown] = this.AKMToKey(local_0_1, 2);
        kmap[MappedAction.LookRight] = this.AKMToKey(local_0_1, 3);
        this.KeyboardManager.UpdateMapping();
      }), -1).SuffixText = (Func<string>) (() => " : " + this.Localize((object) this.UpToAKM(kmap[MappedAction.LookUp])));
      MenuItem mami = this.AddItem("Map_Title", -1);
      mami.Selected = (Action) (() => this.ChangeKey(mami));
      mami.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.OpenMap]));
      MenuItem imi = this.AddItem("ControlsInventory", -1);
      imi.Selected = (Action) (() => this.ChangeKey(imi));
      imi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.OpenInventory]));
      MenuItem pmi = this.AddItem("ControlsPause", -1);
      pmi.Selected = (Action) (() => this.ChangeKey(pmi));
      pmi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.Pause]));
      MenuItem rlmi = this.AddItem("ControlsRotateLeft", -1);
      rlmi.Selected = (Action) (() => this.ChangeKey(rlmi));
      rlmi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.RotateLeft]));
      MenuItem rrmi = this.AddItem("ControlsRotateRight", -1);
      rrmi.Selected = (Action) (() => this.ChangeKey(rrmi));
      rrmi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.RotateRight]));
      MenuItem clmi = this.AddItem("ControlsClampLook", -1);
      clmi.Selected = (Action) (() => this.ChangeKey(clmi));
      clmi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.ClampLook]));
      MenuItem mzimi = this.AddItem("ControlsMapZoomIn", -1);
      mzimi.Selected = (Action) (() => this.ChangeKey(mzimi));
      mzimi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.MapZoomIn]));
      MenuItem mzomi = this.AddItem("ControlsZoomOut", -1);
      mzomi.Selected = (Action) (() => this.ChangeKey(mzomi));
      mzomi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.MapZoomOut]));
      if (this.GameState.SaveData.HasFPView)
      {
        MenuItem tfvmi = this.AddItem("ControlsToggleFpView", -1);
        tfvmi.Selected = (Action) (() => this.ChangeKey(tfvmi));
        tfvmi.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kmap[MappedAction.FpViewToggle]));
      }
      this.AddItem((string) null, MenuBase.SliderAction, -1).Selectable = false;
      this.AddItem("ResetToDefault", (Action) (() => this.ResetToDefault(true, false)), -1);
      this.selectorStart = this.Items.Count;
      this.AddItem("XboxGamepad", MenuBase.SliderAction, true, -1).UpperCase = true;
      this.AddItem("OtherGamepad", MenuBase.SliderAction, true, -1).UpperCase = true;
      this.AddItem("Keyboard", MenuBase.SliderAction, true, -1).UpperCase = true;
    }

    public override void Reset()
    {
      base.Reset();
      this.Items[this.selectorStart].Hidden = true;
      this.Items[this.selectorStart].Selectable = false;
      this.Items[this.selectorStart + 1].Hidden = true;
      this.Items[this.selectorStart + 1].Selectable = false;
      this.Items[this.selectorStart + 2].Hidden = true;
      this.Items[this.selectorStart + 2].Selectable = false;
      this.chosen = this.Items.Count - 1;
      this.FakeSlideRight(true);
      this.noArrows = SdlGamePad.GetPadCount() == 0 && !GamepadState.AnyXInputConnected;
    }

    private void ResetToDefault(bool forKeyboard, bool forGamepad)
    {
      SettingsManager.Settings.ResetMapping(forKeyboard, forGamepad);
      if (!forGamepad)
        return;
      this.pendingApply = true;
    }

    private void ChangeButton(MenuItem mi)
    {
      if (this.TrapInput)
        return;
      mi.SuffixText = (Func<string>) (() => " : " + StaticText.GetString("ChangeGamepadMapping"));
      this.keyGrabFor = mi;
      this.TrapInput = true;
      this.forGamepad = true;
      this.hasBeenIdle = false;
    }

    private void ChangeKey(MenuItem mi)
    {
      if (this.TrapInput)
        return;
      mi.SuffixText = (Func<string>) (() => " : " + StaticText.GetString("ChangeMapping"));
      this.keyGrabFor = mi;
      this.TrapInput = true;
      this.forGamepad = false;
      this.lastPressed.Clear();
      this.keysDown.Clear();
      foreach (Keys keys in Keyboard.GetState().GetPressedKeys())
        this.lastPressed.Add(keys);
    }

    private string Buttonize(int index)
    {
      return string.Format(StaticText.GetString("GamepadButton"), (object) (index + 1));
    }

    private string Localize(object input)
    {
      string str = input.ToString();
      if (Enumerable.All<char>((IEnumerable<char>) str.ToCharArray(), new Func<char, bool>(char.IsUpper)))
        return str;
      if (str.StartsWith("D") && str.Length == 2 && char.IsNumber(str[1]))
        return str[1].ToString();
      string input1 = str.Replace("Oem", string.Empty);
      string text;
      if (StaticText.TryGetString("Keyboard" + input1, out text))
        return text;
      else
        return Regex.Replace(input1, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
    }

    private ControlsMenuLevel.ArrowKeyMapping UpToAKM(Keys key)
    {
      switch (key)
      {
        case Keys.I:
          return ControlsMenuLevel.ArrowKeyMapping.IJKL;
        case Keys.W:
          return ControlsMenuLevel.ArrowKeyMapping.WASD;
        case Keys.Z:
          return ControlsMenuLevel.ArrowKeyMapping.ZQSD;
        case Keys.E:
          return ControlsMenuLevel.ArrowKeyMapping.ESDF;
        default:
          return ControlsMenuLevel.ArrowKeyMapping.Arrows;
      }
    }

    private Keys AKMToKey(ControlsMenuLevel.ArrowKeyMapping akm, int i)
    {
      Keys keys;
      switch (akm)
      {
        case ControlsMenuLevel.ArrowKeyMapping.WASD:
          keys = i != 0 ? (i != 1 ? (i != 2 ? Keys.D : Keys.S) : Keys.A) : Keys.W;
          break;
        case ControlsMenuLevel.ArrowKeyMapping.ZQSD:
          keys = i != 0 ? (i != 1 ? (i != 2 ? Keys.D : Keys.S) : Keys.Q) : Keys.Z;
          break;
        case ControlsMenuLevel.ArrowKeyMapping.IJKL:
          keys = i != 0 ? (i != 1 ? (i != 2 ? Keys.L : Keys.K) : Keys.J) : Keys.I;
          break;
        case ControlsMenuLevel.ArrowKeyMapping.ESDF:
          keys = i != 0 ? (i != 1 ? (i != 2 ? Keys.F : Keys.D) : Keys.S) : Keys.E;
          break;
        default:
          keys = i != 0 ? (i != 1 ? (i != 2 ? Keys.Right : Keys.Down) : Keys.Left) : Keys.Up;
          break;
      }
      return keys;
    }

    public override void Update(TimeSpan elapsed)
    {
      base.Update(elapsed);
      if (this.pendingApply && !SdlGamePad.GetPressedButtonId().HasValue)
      {
        this.pendingApply = false;
        SettingsManager.Settings.ApplyGamepadMapping();
      }
      if (this.TrapInput)
      {
        if (this.forGamepad)
        {
          if (this.KeyboardManager.GetKeyState(Keys.Escape) == FezButtonState.Pressed)
          {
            int num = this.Items.IndexOf(this.keyGrabFor);
            MappedAction mappedAction = ControlsMenuLevel.GamepadActionOrder[num - this.otherGamepadsStart];
            Dictionary<MappedAction, int> gMap = SettingsManager.Settings.GamepadMapping;
            this.keyGrabFor.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gMap[mappedAction]));
            this.TrapInput = false;
            this.forGamepad = false;
            this.pendingApply = true;
          }
          else
          {
            int? pressedButtonId = SdlGamePad.GetPressedButtonId();
            ControlsMenuLevel controlsMenuLevel = this;
            int num1 = controlsMenuLevel.hasBeenIdle | !pressedButtonId.HasValue ? 1 : 0;
            controlsMenuLevel.hasBeenIdle = num1 != 0;
            if (this.hasBeenIdle && pressedButtonId.HasValue)
            {
              int num2 = this.Items.IndexOf(this.keyGrabFor);
              MappedAction mappedAction = ControlsMenuLevel.GamepadActionOrder[num2 - this.otherGamepadsStart];
              Dictionary<MappedAction, int> gMap = SettingsManager.Settings.GamepadMapping;
              gMap[mappedAction] = pressedButtonId.Value;
              this.keyGrabFor.SuffixText = (Func<string>) (() => " : " + this.Buttonize(gMap[mappedAction]));
              this.TrapInput = false;
              this.forGamepad = false;
              this.pendingApply = true;
            }
          }
        }
        else
        {
          this.thisPressed.Clear();
          foreach (Keys keys in Keyboard.GetState().GetPressedKeys())
            this.thisPressed.Add(keys);
          foreach (Keys keys1 in this.keysDown)
          {
            if (!this.thisPressed.Contains(keys1))
            {
              Keys keys2 = keys1;
              int num = this.Items.IndexOf(this.keyGrabFor);
              MappedAction mappedAction = ControlsMenuLevel.KeyboardActionOrder[num - this.keyboardStart];
              Dictionary<MappedAction, Keys> kMap = SettingsManager.Settings.KeyboardMapping;
              kMap[mappedAction] = keys2;
              this.keyGrabFor.SuffixText = (Func<string>) (() => " : " + this.Localize((object) kMap[mappedAction]));
              this.KeyboardManager.UpdateMapping();
              this.TrapInput = false;
              break;
            }
          }
          foreach (Keys keys in this.thisPressed)
          {
            if (!this.lastPressed.Contains(keys))
              this.keysDown.Add(keys);
          }
          HashSet<Keys> hashSet = this.thisPressed;
          this.thisPressed = this.lastPressed;
          this.lastPressed = hashSet;
        }
      }
      if (this.SelectedIndex < this.Items.Count - 3)
        return;
      Point position = this.MouseState.Position;
      if (this.leftSliderRect.HasValue && this.leftSliderRect.Value.Contains(position))
      {
        this.menuBase.CursorSelectable = true;
        if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
          this.FakeSlideLeft(false);
      }
      else if (this.rightSliderRect.HasValue && this.rightSliderRect.Value.Contains(position))
      {
        this.menuBase.CursorSelectable = true;
        if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
          this.FakeSlideRight(false);
      }
      if (this.InputManager.Right == FezButtonState.Pressed)
      {
        this.FakeSlideRight(false);
      }
      else
      {
        if (this.InputManager.Left != FezButtonState.Pressed)
          return;
        this.FakeSlideLeft(false);
      }
    }

    private void FakeSlideRight(bool silent = false)
    {
      this.Items[this.chosen].Hidden = true;
      this.Items[this.chosen].Selectable = false;
      int num1 = this.chosen;
      ++this.chosen;
      if (this.chosen == this.Items.Count)
        this.chosen = this.selectorStart;
      if (!GamepadState.AnyXInputConnected && this.chosen == this.selectorStart)
        ++this.chosen;
      if ((SdlGamePad.GetPadCount() == 0 || GamepadState.AnyXInputConnected) && this.chosen == this.selectorStart + 1)
        ++this.chosen;
      int num2 = this.chosen - this.selectorStart;
      for (int index = 0; index < this.otherGamepadsStart; ++index)
        this.Items[index].Hidden = num2 != 0;
      for (int index = this.otherGamepadsStart; index < this.keyboardStart; ++index)
        this.Items[index].Hidden = num2 != 1;
      for (int index = this.keyboardStart; index < this.selectorStart; ++index)
        this.Items[index].Hidden = num2 != 2;
      this.Items[this.chosen].Hidden = false;
      this.Items[this.chosen].Selectable = true;
      this.SelectedIndex = this.chosen;
      if (silent || num1 == this.chosen)
        return;
      SoundEffectExtensions.Emit(this.sSliderValueIncrease);
    }

    private void FakeSlideLeft(bool silent = false)
    {
      this.Items[this.chosen].Hidden = true;
      this.Items[this.chosen].Selectable = false;
      int num1 = this.chosen;
      --this.chosen;
      if ((SdlGamePad.GetPadCount() == 0 || GamepadState.AnyXInputConnected) && this.chosen == this.selectorStart + 1)
        --this.chosen;
      if (!GamepadState.AnyXInputConnected && this.chosen == this.selectorStart)
        --this.chosen;
      if (this.chosen == this.selectorStart - 1)
        this.chosen = this.Items.Count - 1;
      int num2 = this.chosen - this.selectorStart;
      for (int index = 0; index < this.otherGamepadsStart; ++index)
        this.Items[index].Hidden = num2 != 0;
      for (int index = this.otherGamepadsStart; index < this.keyboardStart; ++index)
        this.Items[index].Hidden = num2 != 1;
      for (int index = this.keyboardStart; index < this.selectorStart; ++index)
        this.Items[index].Hidden = num2 != 2;
      this.Items[this.chosen].Hidden = false;
      this.Items[this.chosen].Selectable = true;
      this.SelectedIndex = this.chosen;
      if (silent || num1 == this.chosen)
        return;
      SoundEffectExtensions.Emit(this.sSliderValueDecrease);
    }

    public override void Initialize()
    {
      ContentManager contentManager = this.CMProvider.Get(CM.Menu);
      this.ControlsImage = contentManager.Load<Texture2D>("Other Textures/controls");
      this.sSliderValueDecrease = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/SliderValueDecrease");
      this.sSliderValueIncrease = contentManager.Load<SoundEffect>("Sounds/Ui/Menu/SliderValueIncrease");
      base.Initialize();
    }

    public override void PostDraw(SpriteBatch batch, SpriteFont font, GlyphTextRenderer tr, float alpha)
    {
      float smallFactor = this.FontManager.SmallFactor;
      float num1 = (float) batch.GraphicsDevice.Viewport.Height / 2f;
      Vector2 position = FezMath.Round(new Vector2((float) (batch.GraphicsDevice.Viewport.Width - this.ControlsImage.Width) / 2f, num1 - (float) this.ControlsImage.Height / 2f));
      float viewScale = SettingsManager.GetViewScale(batch.GraphicsDevice);
      int num2 = batch.GraphicsDevice.Viewport.Width / 2;
      int num3 = batch.GraphicsDevice.Viewport.Height / 2;
      int num4 = num2 - 464;
      int num5 = num2 + 464;
      if (this.SelectedIndex == this.Items.Count - 3)
      {
        batch.Draw(this.ControlsImage, position, new Color(1f, 1f, 1f, alpha));
        if (Culture.IsCJK)
          smallFactor /= viewScale;
        tr.DrawShadowedText(batch, font, StaticText.GetString("Map_Title").ToUpper(CultureInfo.InvariantCulture), new Vector2((float) num4, num1 - 222f), new Color(1f, 1f, 1f, alpha), smallFactor);
        tr.DrawShadowedText(batch, font, StaticText.GetString("ControlsRotateLeft").ToUpper(CultureInfo.InvariantCulture), new Vector2((float) num4, num1 - 170f), new Color(1f, 1f, 1f, alpha), smallFactor);
        tr.DrawShadowedText(batch, font, StaticText.GetString("ControlsMove").ToUpper(CultureInfo.InvariantCulture), new Vector2((float) num4, num1 - 15f), new Color(1f, 1f, 1f, alpha), smallFactor);
        this.DrawLeftAligned(tr, batch, font, StaticText.GetString("ControlsPause").ToUpper(CultureInfo.InvariantCulture), alpha, new Vector2((float) num5, num1 - 222f), smallFactor);
        this.DrawLeftAligned(tr, batch, font, StaticText.GetString("ControlsRotateRight").ToUpper(CultureInfo.InvariantCulture), alpha, new Vector2((float) num5, num1 - 170f), smallFactor);
        this.DrawLeftAligned(tr, batch, font, StaticText.GetString("ControlsAction").ToUpper(CultureInfo.InvariantCulture), alpha, new Vector2((float) num5, num1 - 114f), smallFactor);
        this.DrawLeftAligned(tr, batch, font, StaticText.GetString("ControlsInventory").ToUpper(CultureInfo.InvariantCulture), alpha, new Vector2((float) num5, num1 - 81f), smallFactor);
        this.DrawLeftAligned(tr, batch, font, StaticText.GetString("ControlsTalk").ToUpper(CultureInfo.InvariantCulture), alpha, new Vector2((float) num5, num1 - 40f), smallFactor);
        this.DrawLeftAligned(tr, batch, font, StaticText.GetString("ControlsJump").ToUpper(CultureInfo.InvariantCulture), alpha, new Vector2((float) num5, num1 + 3f), smallFactor);
        this.DrawLeftAligned(tr, batch, font, StaticText.GetString("ControlsLook").ToUpper(CultureInfo.InvariantCulture), alpha, new Vector2((float) num5, num1 + 72f), smallFactor);
        if (Culture.IsCJK)
        {
          float num6 = smallFactor * viewScale;
        }
      }
      float num7 = this.Items[this.chosen].Size.X + 70f;
      if (Culture.IsCJK)
        num7 *= 0.5f;
      if (this.SelectedIndex >= this.Items.Count - 3)
      {
        float num6 = 25f;
        float num8;
        if (!Culture.IsCJK)
        {
          num8 = num6 * viewScale;
        }
        else
        {
          num7 = num7 * 0.4f + 25f;
          num8 = 5f * viewScale;
          if (Culture.Language == Language.Chinese)
            num8 = (float) (10.0 + 25.0 * (double) viewScale);
        }
        int num9 = ServiceHelper.Game.GraphicsDevice.Viewport.Width / 2;
        Vector2 offset = new Vector2((float) (-(double) num7 + 25.0 * (double) viewScale - 40.0 * ((double) viewScale - 1.0)), (float) num3 + 180f * viewScale + num8);
        if (!this.noArrows)
        {
          tr.DrawCenteredString(batch, this.FontManager.Big, "{LA}", new Color(1f, 1f, 1f, alpha), offset, (Culture.IsCJK ? 0.2f : 1f) * viewScale);
          this.leftSliderRect = new Rectangle?(new Rectangle((int) ((double) offset.X + (double) num9 - 25.0 * (double) viewScale), (int) offset.Y, (int) (40.0 * (double) viewScale), (int) (25.0 * (double) viewScale)));
        }
        else
          this.leftSliderRect = new Rectangle?();
        offset = new Vector2(num7 + (float) (40.0 * ((double) viewScale - 1.0)), (float) num3 + 180f * viewScale + num8);
        if (!this.noArrows)
        {
          tr.DrawCenteredString(batch, this.FontManager.Big, "{RA}", new Color(1f, 1f, 1f, alpha), offset, (Culture.IsCJK ? 0.2f : 1f) * viewScale);
          this.rightSliderRect = new Rectangle?(new Rectangle((int) ((double) offset.X + (double) num9 - 30.0 * (double) viewScale), (int) offset.Y, (int) (40.0 * (double) viewScale), (int) (25.0 * (double) viewScale)));
        }
        else
          this.rightSliderRect = new Rectangle?();
      }
      else
      {
        ControlsMenuLevel controlsMenuLevel1 = this;
        ControlsMenuLevel controlsMenuLevel2 = this;
        ControlsMenuLevel controlsMenuLevel3 = this;
        Rectangle? nullable1 = new Rectangle?();
        Rectangle? nullable2 = nullable1;
        controlsMenuLevel3.rightSliderRect = nullable2;
        Rectangle? nullable3;
        Rectangle? nullable4 = nullable3 = nullable1;
        controlsMenuLevel2.rightSliderRect = nullable3;
        Rectangle? nullable5 = nullable4;
        controlsMenuLevel1.leftSliderRect = nullable5;
      }
    }

    private void DrawLeftAligned(GlyphTextRenderer tr, SpriteBatch batch, SpriteFont font, string text, float alpha, Vector2 offset, float size)
    {
      float num = font.MeasureString(text).X * size;
      tr.DrawShadowedText(batch, font, text, offset - num * Vector2.UnitX, new Color(1f, 1f, 1f, alpha), size);
    }

    private enum ArrowKeyMapping
    {
      WASD,
      ZQSD,
      IJKL,
      ESDF,
      Arrows,
    }
  }
}
