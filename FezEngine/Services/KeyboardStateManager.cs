// Type: FezEngine.Services.KeyboardStateManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Input;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FezEngine.Services
{
  public class KeyboardStateManager : IKeyboardStateManager
  {
    private readonly Dictionary<Keys, FezButtonState> keyStates = new Dictionary<Keys, FezButtonState>((IEqualityComparer<Keys>) KeysEqualityComparer.Default);
    private readonly List<Keys> registeredKeys = new List<Keys>();
    private readonly List<Keys> activeKeys = new List<Keys>();
    private Dictionary<MappedAction, Keys> lastMapping;

    public bool IgnoreMapping { get; set; }

    public FezButtonState Up
    {
      get
      {
        return this.GetMappedKeyState(MappedAction.Up, Keys.Up);
      }
    }

    public FezButtonState Down
    {
      get
      {
        return this.GetMappedKeyState(MappedAction.Down, Keys.Down);
      }
    }

    public FezButtonState Left
    {
      get
      {
        return this.GetMappedKeyState(MappedAction.Left, Keys.Left);
      }
    }

    public FezButtonState Right
    {
      get
      {
        return this.GetMappedKeyState(MappedAction.Right, Keys.Right);
      }
    }

    public FezButtonState Jump
    {
      get
      {
        return this.GetMappedKeyState(MappedAction.Jump, Keys.Enter);
      }
    }

    public FezButtonState CancelTalk
    {
      get
      {
        return this.GetMappedKeyState(MappedAction.CancelTalk, Keys.Escape);
      }
    }

    public FezButtonState GrabThrow
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.GrabThrow]);
      }
    }

    public FezButtonState LookUp
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.LookUp]);
      }
    }

    public FezButtonState LookDown
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.LookDown]);
      }
    }

    public FezButtonState LookRight
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.LookRight]);
      }
    }

    public FezButtonState LookLeft
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.LookLeft]);
      }
    }

    public FezButtonState OpenMap
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.OpenMap]);
      }
    }

    public FezButtonState MapZoomIn
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.MapZoomIn]);
      }
    }

    public FezButtonState MapZoomOut
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.MapZoomOut]);
      }
    }

    public FezButtonState Pause
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.Pause]);
      }
    }

    public FezButtonState OpenInventory
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.OpenInventory]);
      }
    }

    public FezButtonState RotateLeft
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.RotateLeft]);
      }
    }

    public FezButtonState RotateRight
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.RotateRight]);
      }
    }

    public FezButtonState FpViewToggle
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.FpViewToggle]);
      }
    }

    public FezButtonState ClampLook
    {
      get
      {
        return this.GetKeyState(this.lastMapping[MappedAction.ClampLook]);
      }
    }

    public KeyboardStateManager()
    {
      this.UpdateMapping();
    }

    public FezButtonState GetKeyState(Keys key)
    {
      FezButtonState fezButtonState;
      if (!this.keyStates.TryGetValue(key, out fezButtonState))
        fezButtonState = FezButtonState.Up;
      return fezButtonState;
    }

    public void RegisterKey(Keys key)
    {
      lock (this)
      {
        if (this.registeredKeys.Contains(key))
          return;
        this.registeredKeys.Add(key);
      }
    }

    public void UpdateMapping()
    {
      Dictionary<MappedAction, Keys> keyboardMapping = SettingsManager.Settings.KeyboardMapping;
      if (this.lastMapping != null)
      {
        foreach (Keys keys in this.lastMapping.Values)
          this.registeredKeys.Remove(keys);
      }
      foreach (Keys key in keyboardMapping.Values)
        this.RegisterKey(key);
      this.RegisterKey(Keys.Down);
      this.RegisterKey(Keys.Up);
      this.RegisterKey(Keys.Right);
      this.RegisterKey(Keys.Left);
      this.RegisterKey(Keys.Enter);
      this.RegisterKey(Keys.Escape);
      this.lastMapping = keyboardMapping;
    }

    private FezButtonState GetMappedKeyState(MappedAction action, Keys ifIgnored)
    {
      return this.GetKeyState(this.IgnoreMapping ? ifIgnored : this.lastMapping[action]);
    }

    public void Update(KeyboardState state, GameTime time)
    {
      KeyboardState state1 = Keyboard.GetState();
      this.activeKeys.Clear();
      lock (this)
        this.activeKeys.AddRange((IEnumerable<Keys>) this.registeredKeys);
      foreach (Keys key in this.activeKeys)
      {
        bool pressed = state1.IsKeyDown(key);
        FezButtonState state2;
        if (this.keyStates.TryGetValue(key, out state2))
        {
          if ((pressed || state2 != FezButtonState.Up) && FezButtonStateExtensions.NextState(state2, pressed) != state2)
          {
            this.keyStates.Remove(key);
            this.keyStates.Add(key, FezButtonStateExtensions.NextState(state2, pressed));
          }
        }
        else
          this.keyStates.Add(key, FezButtonStateExtensions.NextState(state2, pressed));
      }
    }
  }
}
