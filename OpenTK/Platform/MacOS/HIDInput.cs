// Type: OpenTK.Platform.MacOS.HIDInput
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Input;
using OpenTK.Platform.MacOS.Carbon;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS
{
  internal class HIDInput : IInputDriver2, IMouseDriver2, IKeyboardDriver2
  {
    private static readonly Key[] RawKeyMap = new Key[232]
    {
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.A,
      Key.B,
      Key.C,
      Key.D,
      Key.E,
      Key.F,
      Key.G,
      Key.H,
      Key.I,
      Key.J,
      Key.K,
      Key.L,
      Key.M,
      Key.N,
      Key.O,
      Key.P,
      Key.Q,
      Key.R,
      Key.S,
      Key.T,
      Key.U,
      Key.V,
      Key.W,
      Key.X,
      Key.Y,
      Key.Z,
      Key.Number1,
      Key.Number2,
      Key.Number3,
      Key.Number4,
      Key.Number5,
      Key.Number6,
      Key.Number7,
      Key.Number8,
      Key.Number9,
      Key.Number0,
      Key.Enter,
      Key.Escape,
      Key.BackSpace,
      Key.Tab,
      Key.Space,
      Key.Minus,
      Key.Plus,
      Key.BracketLeft,
      Key.BracketRight,
      Key.BackSlash,
      Key.Minus,
      Key.Semicolon,
      Key.Quote,
      Key.Tilde,
      Key.Comma,
      Key.Period,
      Key.Slash,
      Key.CapsLock,
      Key.F1,
      Key.F2,
      Key.F3,
      Key.F4,
      Key.F5,
      Key.F6,
      Key.F7,
      Key.F8,
      Key.F9,
      Key.F10,
      Key.F11,
      Key.F12,
      Key.PrintScreen,
      Key.ScrollLock,
      Key.Pause,
      Key.Insert,
      Key.Home,
      Key.PageUp,
      Key.Delete,
      Key.End,
      Key.PageDown,
      Key.Right,
      Key.Left,
      Key.Down,
      Key.Up,
      Key.NumLock,
      Key.KeypadDivide,
      Key.KeypadMultiply,
      Key.KeypadSubtract,
      Key.KeypadAdd,
      Key.KeypadEnter,
      Key.Keypad1,
      Key.Keypad2,
      Key.Keypad3,
      Key.Keypad4,
      Key.Keypad5,
      Key.Keypad6,
      Key.Keypad7,
      Key.Keypad8,
      Key.Keypad9,
      Key.Keypad0,
      Key.KeypadDecimal,
      Key.BackSlash,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.F13,
      Key.F14,
      Key.F15,
      Key.F16,
      Key.F17,
      Key.F18,
      Key.F19,
      Key.F20,
      Key.F21,
      Key.F22,
      Key.F23,
      Key.F24,
      Key.Unknown,
      Key.Unknown,
      Key.Menu,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.CapsLock,
      Key.NumLock,
      Key.ScrollLock,
      Key.KeypadDecimal,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Enter,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.Unknown,
      Key.ControlLeft,
      Key.ShiftLeft,
      Key.AltLeft,
      Key.WinLeft,
      Key.ControlRight,
      Key.ShiftRight,
      Key.AltRight,
      Key.WinRight
    };
    private readonly Dictionary<IntPtr, MouseState> MouseDevices = new Dictionary<IntPtr, MouseState>((IEqualityComparer<IntPtr>) new IntPtrEqualityComparer());
    private readonly Dictionary<int, IntPtr> MouseIndexToDevice = new Dictionary<int, IntPtr>();
    private readonly Dictionary<IntPtr, KeyboardState> KeyboardDevices = new Dictionary<IntPtr, KeyboardState>((IEqualityComparer<IntPtr>) new IntPtrEqualityComparer());
    private readonly Dictionary<int, IntPtr> KeyboardIndexToDevice = new Dictionary<int, IntPtr>();
    private readonly IntPtr RunLoop = CF.CFRunLoopGetMain();
    private readonly IntPtr InputLoopMode = CF.RunLoopModeDefault;
    private readonly CFDictionary DeviceTypes = new CFDictionary();
    private readonly IntPtr hidmanager;
    private readonly HIDInput.NativeMethods.IOHIDDeviceCallback HandleDeviceAdded;
    private readonly HIDInput.NativeMethods.IOHIDDeviceCallback HandleDeviceRemoved;
    private readonly HIDInput.NativeMethods.IOHIDValueCallback HandleDeviceValueReceived;

    public IMouseDriver2 MouseDriver
    {
      get
      {
        return (IMouseDriver2) this;
      }
    }

    public IKeyboardDriver2 KeyboardDriver
    {
      get
      {
        return (IKeyboardDriver2) this;
      }
    }

    public IGamePadDriver GamePadDriver
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    static HIDInput()
    {
    }

    public HIDInput()
    {
      this.HandleDeviceAdded = new HIDInput.NativeMethods.IOHIDDeviceCallback(this.DeviceAdded);
      this.HandleDeviceRemoved = new HIDInput.NativeMethods.IOHIDDeviceCallback(this.DeviceRemoved);
      this.HandleDeviceValueReceived = new HIDInput.NativeMethods.IOHIDValueCallback(this.DeviceValueReceived);
      this.hidmanager = this.CreateHIDManager();
      this.RegisterHIDCallbacks(this.hidmanager);
    }

    private IntPtr CreateHIDManager()
    {
      return HIDInput.NativeMethods.IOHIDManagerCreate(IntPtr.Zero, IntPtr.Zero);
    }

    private void RegisterHIDCallbacks(IntPtr hidmanager)
    {
      HIDInput.NativeMethods.IOHIDManagerRegisterDeviceMatchingCallback(hidmanager, this.HandleDeviceAdded, IntPtr.Zero);
      HIDInput.NativeMethods.IOHIDManagerRegisterDeviceRemovalCallback(hidmanager, this.HandleDeviceRemoved, IntPtr.Zero);
      HIDInput.NativeMethods.IOHIDManagerScheduleWithRunLoop(hidmanager, this.RunLoop, this.InputLoopMode);
      HIDInput.NativeMethods.IOHIDManagerSetDeviceMatching(hidmanager, this.DeviceTypes.Ref);
      HIDInput.NativeMethods.IOHIDManagerOpen(hidmanager, IntPtr.Zero);
      int num = (int) CF.CFRunLoopRunInMode(this.InputLoopMode, 0.0, true);
    }

    private void DeviceAdded(IntPtr context, IntPtr res, IntPtr sender, IntPtr device)
    {
      if (!(HIDInput.NativeMethods.IOHIDDeviceOpen(device, IntPtr.Zero) == IntPtr.Zero))
        return;
      if (HIDInput.NativeMethods.IOHIDDeviceConformsTo(device, HIDInput.HIDPage.GenericDesktop, 2))
      {
        if (!this.MouseDevices.ContainsKey(device))
        {
          MouseState mouseState = new MouseState();
          mouseState.IsConnected = true;
          this.MouseIndexToDevice.Add(this.MouseDevices.Count, device);
          this.MouseDevices.Add(device, mouseState);
        }
        else
        {
          MouseState mouseState = this.MouseDevices[device];
          mouseState.IsConnected = true;
          this.MouseDevices[device] = mouseState;
        }
      }
      if (HIDInput.NativeMethods.IOHIDDeviceConformsTo(device, HIDInput.HIDPage.GenericDesktop, 6))
      {
        if (!this.KeyboardDevices.ContainsKey(device))
        {
          KeyboardState keyboardState = new KeyboardState();
          keyboardState.IsConnected = true;
          this.KeyboardIndexToDevice.Add(this.KeyboardDevices.Count, device);
          this.KeyboardDevices.Add(device, keyboardState);
        }
        else
        {
          KeyboardState keyboardState = this.KeyboardDevices[device];
          keyboardState.IsConnected = true;
          this.KeyboardDevices[device] = keyboardState;
        }
      }
      HIDInput.NativeMethods.IOHIDDeviceRegisterInputValueCallback(device, this.HandleDeviceValueReceived, IntPtr.Zero);
      HIDInput.NativeMethods.IOHIDDeviceScheduleWithRunLoop(device, this.RunLoop, this.InputLoopMode);
    }

    private void DeviceRemoved(IntPtr context, IntPtr res, IntPtr sender, IntPtr device)
    {
      if (HIDInput.NativeMethods.IOHIDDeviceConformsTo(device, HIDInput.HIDPage.GenericDesktop, 2) && this.MouseDevices.ContainsKey(device))
      {
        MouseState mouseState = this.MouseDevices[device];
        mouseState.IsConnected = false;
        this.MouseDevices[device] = mouseState;
      }
      if (HIDInput.NativeMethods.IOHIDDeviceConformsTo(device, HIDInput.HIDPage.GenericDesktop, 6) && this.KeyboardDevices.ContainsKey(device))
      {
        KeyboardState keyboardState = this.KeyboardDevices[device];
        keyboardState.IsConnected = false;
        this.KeyboardDevices[device] = keyboardState;
      }
      HIDInput.NativeMethods.IOHIDDeviceRegisterInputValueCallback(device, (HIDInput.NativeMethods.IOHIDValueCallback) null, IntPtr.Zero);
      HIDInput.NativeMethods.IOHIDDeviceUnscheduleWithRunLoop(device, this.RunLoop, this.InputLoopMode);
    }

    private void DeviceValueReceived(IntPtr context, IntPtr res, IntPtr sender, IntPtr val)
    {
      MouseState state1;
      if (this.MouseDevices.TryGetValue(sender, out state1))
      {
        this.MouseDevices[sender] = HIDInput.UpdateMouse(state1, val);
      }
      else
      {
        KeyboardState state2;
        if (!this.KeyboardDevices.TryGetValue(sender, out state2))
          return;
        this.KeyboardDevices[sender] = HIDInput.UpdateKeyboard(state2, val);
      }
    }

    private static MouseState UpdateMouse(MouseState state, IntPtr val)
    {
      IntPtr element = HIDInput.NativeMethods.IOHIDValueGetElement(val);
      int num = HIDInput.NativeMethods.IOHIDValueGetIntegerValue(val).ToInt32();
      HIDInput.HIDPage usagePage = HIDInput.NativeMethods.IOHIDElementGetUsagePage(element);
      int usage = HIDInput.NativeMethods.IOHIDElementGetUsage(element);
      switch (usagePage)
      {
        case HIDInput.HIDPage.GenericDesktop:
          switch ((HIDInput.HIDUsageGD) usage)
          {
            case HIDInput.HIDUsageGD.X:
              state.X += num;
              break;
            case HIDInput.HIDUsageGD.Y:
              state.Y += num;
              break;
            case HIDInput.HIDUsageGD.Wheel:
              state.WheelPrecise += (float) num;
              break;
          }
        case HIDInput.HIDPage.Button:
          state[(OpenTK.Input.MouseButton) (usage - 1)] = num == 1;
          break;
      }
      return state;
    }

    private static KeyboardState UpdateKeyboard(KeyboardState state, IntPtr val)
    {
      IntPtr element = HIDInput.NativeMethods.IOHIDValueGetElement(val);
      int num = HIDInput.NativeMethods.IOHIDValueGetIntegerValue(val).ToInt32();
      HIDInput.HIDPage usagePage = HIDInput.NativeMethods.IOHIDElementGetUsagePage(element);
      int usage = HIDInput.NativeMethods.IOHIDElementGetUsage(element);
      switch (usagePage)
      {
        case HIDInput.HIDPage.GenericDesktop:
        case HIDInput.HIDPage.KeyboardOrKeypad:
          int index1 = usage;
          if (index1 >= HIDInput.RawKeyMap.Length || index1 < 0)
            return state;
          Key index2 = HIDInput.RawKeyMap[index1];
          state[index2] = num != 0;
          break;
      }
      return state;
    }

    MouseState IMouseDriver2.GetState()
    {
      MouseState mouseState = new MouseState();
      foreach (KeyValuePair<IntPtr, MouseState> keyValuePair in this.MouseDevices)
        mouseState.MergeBits(keyValuePair.Value);
      return mouseState;
    }

    MouseState IMouseDriver2.GetState(int index)
    {
      IntPtr index1;
      if (this.MouseIndexToDevice.TryGetValue(index, out index1))
        return this.MouseDevices[index1];
      else
        return new MouseState();
    }

    void IMouseDriver2.SetPosition(double x, double y)
    {
      int num1 = (int) CG.SetLocalEventsSuppressionInterval(0.0);
      int num2 = (int) CG.WarpMouseCursorPosition(new HIPoint(x, y));
    }

    KeyboardState IKeyboardDriver2.GetState()
    {
      KeyboardState keyboardState = new KeyboardState();
      foreach (KeyValuePair<IntPtr, KeyboardState> keyValuePair in this.KeyboardDevices)
        keyboardState.MergeBits(keyValuePair.Value);
      return keyboardState;
    }

    KeyboardState IKeyboardDriver2.GetState(int index)
    {
      IntPtr index1;
      if (this.KeyboardIndexToDevice.TryGetValue(index, out index1))
        return this.KeyboardDevices[index1];
      else
        return new KeyboardState();
    }

    string IKeyboardDriver2.GetDeviceName(int index)
    {
      IntPtr device;
      if (this.KeyboardIndexToDevice.TryGetValue(index, out device))
        return string.Format("{0}:{1}", (object) HIDInput.NativeMethods.IOHIDDeviceGetProperty(device, HIDInput.NativeMethods.IOHIDVendorIDKey), (object) HIDInput.NativeMethods.IOHIDDeviceGetProperty(device, HIDInput.NativeMethods.IOHIDProductIDKey));
      else
        return string.Empty;
    }

    private class NativeMethods
    {
      public static readonly IntPtr IOHIDVendorIDKey = CF.CFSTR("VendorID");
      public static readonly IntPtr IOHIDVendorIDSourceKey = CF.CFSTR("VendorIDSource");
      public static readonly IntPtr IOHIDProductIDKey = CF.CFSTR("ProductID");
      public static readonly IntPtr IOHIDVersionNumberKey = CF.CFSTR("VersionNumber");
      public static readonly IntPtr IOHIDManufacturerKey = CF.CFSTR("Manufacturer");
      public static readonly IntPtr IOHIDProductKey = CF.CFSTR("Product");
      public static readonly IntPtr IOHIDDeviceUsageKey = CF.CFSTR("DeviceUsage");
      public static readonly IntPtr IOHIDDeviceUsagePageKey = CF.CFSTR("DeviceUsagePage");
      public static readonly IntPtr IOHIDDeviceUsagePairsKey = CF.CFSTR("DeviceUsagePairs");
      private const string hid = "/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit";

      static NativeMethods()
      {
      }

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static IntPtr IOHIDManagerCreate(IntPtr allocator, IntPtr options);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static void IOHIDManagerRegisterDeviceMatchingCallback(IntPtr inIOHIDManagerRef, HIDInput.NativeMethods.IOHIDDeviceCallback inIOHIDDeviceCallback, IntPtr inContext);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static void IOHIDManagerRegisterDeviceRemovalCallback(IntPtr inIOHIDManagerRef, HIDInput.NativeMethods.IOHIDDeviceCallback inIOHIDDeviceCallback, IntPtr inContext);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static void IOHIDManagerScheduleWithRunLoop(IntPtr inIOHIDManagerRef, IntPtr inCFRunLoop, IntPtr inCFRunLoopMode);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static void IOHIDManagerSetDeviceMatching(IntPtr manager, IntPtr matching);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static IntPtr IOHIDManagerOpen(IntPtr manager, IntPtr options);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static IntPtr IOHIDDeviceOpen(IntPtr manager, IntPtr opts);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static IntPtr IOHIDDeviceGetProperty(IntPtr device, IntPtr key);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static bool IOHIDDeviceConformsTo(IntPtr inIOHIDDeviceRef, HIDInput.HIDPage inUsagePage, int inUsage);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static void IOHIDDeviceRegisterInputValueCallback(IntPtr device, HIDInput.NativeMethods.IOHIDValueCallback callback, IntPtr context);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static void IOHIDDeviceScheduleWithRunLoop(IntPtr device, IntPtr inCFRunLoop, IntPtr inCFRunLoopMode);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static void IOHIDDeviceUnscheduleWithRunLoop(IntPtr device, IntPtr inCFRunLoop, IntPtr inCFRunLoopMode);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static IntPtr IOHIDValueGetElement(IntPtr value);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static IntPtr IOHIDValueGetIntegerValue(IntPtr value);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static double IOHIDValueGetScaledValue(IntPtr value, HIDInput.IOHIDValueScaleType type);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static int IOHIDElementGetUsage(IntPtr elem);

      [DllImport("/System/Library/Frameworks/IOKit.framework/Versions/Current/IOKit")]
      public static HIDInput.HIDPage IOHIDElementGetUsagePage(IntPtr elem);

      public delegate void IOHIDDeviceCallback(IntPtr ctx, IntPtr res, IntPtr sender, IntPtr device);

      public delegate void IOHIDValueCallback(IntPtr ctx, IntPtr res, IntPtr sender, IntPtr val);
    }

    private enum IOHIDValueScaleType
    {
      Physical,
      Calibrated,
    }

    private enum HIDPage
    {
      Undefined = 0,
      GenericDesktop = 1,
      Simulation = 2,
      VR = 3,
      Sport = 4,
      Game = 5,
      KeyboardOrKeypad = 7,
      LEDs = 8,
      Button = 9,
      Ordinal = 10,
      Telephony = 11,
      Consumer = 12,
      Digitizer = 13,
      PID = 15,
      Unicode = 16,
      AlphanumericDisplay = 20,
      PowerDevice = 132,
      BatterySystem = 133,
      BarCodeScanner = 140,
      Scale = 141,
      WeighingDevice = 141,
      MagneticStripeReader = 142,
      CameraControl = 144,
      Arcade = 145,
      VendorDefinedStart = 65280,
    }

    private enum HIDUsageGD
    {
      Pointer = 1,
      Mouse = 2,
      Joystick = 4,
      GamePad = 5,
      Keyboard = 6,
      Keypad = 7,
      MultiAxisController = 8,
      X = 48,
      Y = 49,
      Z = 50,
      Rx = 51,
      Ry = 52,
      Rz = 53,
      Slider = 54,
      Dial = 55,
      Wheel = 56,
      Hatswitch = 57,
      CountedBuffer = 58,
      ByteCount = 59,
      MotionWakeup = 60,
      Start = 61,
      Select = 62,
      Vx = 64,
      Vy = 65,
      Vz = 66,
      Vbrx = 67,
      Vbry = 68,
      Vbrz = 69,
      Vno = 70,
      SystemControl = 128,
      SystemPowerDown = 129,
      SystemSleep = 130,
      SystemWakeUp = 131,
      SystemContextMenu = 132,
      SystemMainMenu = 133,
      SystemAppMenu = 134,
      SystemMenuHelp = 135,
      SystemMenuExit = 136,
      SystemMenu = 137,
      SystemMenuRight = 138,
      SystemMenuLeft = 139,
      SystemMenuUp = 140,
      SystemMenuDown = 141,
      DPadUp = 144,
      DPadDown = 145,
      DPadRight = 146,
      DPadLeft = 147,
      Reserved = 65535,
    }

    private enum HIDButton
    {
      Button_1 = 1,
      Button_2 = 2,
      Button_3 = 3,
      Button_4 = 4,
      Button_65535 = 65535,
    }

    private enum HIDKey
    {
      ErrorRollOver = 1,
      POSTFail = 2,
      ErrorUndefined = 3,
      A = 4,
      B = 5,
      C = 6,
      D = 7,
      E = 8,
      F = 9,
      G = 10,
      H = 11,
      I = 12,
      J = 13,
      K = 14,
      L = 15,
      M = 16,
      N = 17,
      O = 18,
      P = 19,
      Q = 20,
      R = 21,
      S = 22,
      T = 23,
      U = 24,
      V = 25,
      W = 26,
      X = 27,
      Y = 28,
      Z = 29,
      Number1 = 30,
      Number2 = 31,
      Number3 = 32,
      Number4 = 33,
      Number5 = 34,
      Number6 = 35,
      Number7 = 36,
      Number8 = 37,
      Number9 = 38,
      Number0 = 39,
      ReturnOrEnter = 40,
      Escape = 41,
      DeleteOrBackspace = 42,
      Tab = 43,
      Spacebar = 44,
      Hyphen = 45,
      EqualSign = 46,
      OpenBracket = 47,
      CloseBracket = 48,
      Backslash = 49,
      NonUSPound = 50,
      Semicolon = 51,
      Quote = 52,
      GraveAccentAndTilde = 53,
      Comma = 54,
      Period = 55,
      Slash = 56,
      CapsLock = 57,
      F1 = 58,
      F2 = 59,
      F3 = 60,
      F4 = 61,
      F5 = 62,
      F6 = 63,
      F7 = 64,
      F8 = 65,
      F9 = 66,
      F10 = 67,
      F11 = 68,
      F12 = 69,
      PrintScreen = 70,
      ScrollLock = 71,
      Pause = 72,
      Insert = 73,
      Home = 74,
      PageUp = 75,
      DeleteForward = 76,
      End = 77,
      PageDown = 78,
      RightArrow = 79,
      LeftArrow = 80,
      DownArrow = 81,
      UpArrow = 82,
      KeypadNumLock = 83,
      KeypadSlash = 84,
      KeypadAsterisk = 85,
      KeypadHyphen = 86,
      KeypadPlus = 87,
      KeypadEnter = 88,
      Keypad1 = 89,
      Keypad2 = 90,
      Keypad3 = 91,
      Keypad4 = 92,
      Keypad5 = 93,
      Keypad6 = 94,
      Keypad7 = 95,
      Keypad8 = 96,
      Keypad9 = 97,
      Keypad0 = 98,
      KeypadPeriod = 99,
      NonUSBackslash = 100,
      Application = 101,
      Power = 102,
      KeypadEqualSign = 103,
      F13 = 104,
      F14 = 105,
      F15 = 106,
      F16 = 107,
      F17 = 108,
      F18 = 109,
      F19 = 110,
      F20 = 111,
      F21 = 112,
      F22 = 113,
      F23 = 114,
      F24 = 115,
      Execute = 116,
      Help = 117,
      Menu = 118,
      Select = 119,
      Stop = 120,
      Again = 121,
      Undo = 122,
      Cut = 123,
      Copy = 124,
      Paste = 125,
      Find = 126,
      Mute = 127,
      VolumeUp = 128,
      VolumeDown = 129,
      LockingCapsLock = 130,
      LockingNumLock = 131,
      LockingScrollLock = 132,
      KeypadComma = 133,
      KeypadEqualSignAS400 = 134,
      International1 = 135,
      International2 = 136,
      International3 = 137,
      International4 = 138,
      International5 = 139,
      International6 = 140,
      International7 = 141,
      International8 = 142,
      International9 = 143,
      LANG1 = 144,
      LANG2 = 145,
      LANG3 = 146,
      LANG4 = 147,
      LANG5 = 148,
      LANG6 = 149,
      LANG7 = 150,
      LANG8 = 151,
      LANG9 = 152,
      AlternateErase = 153,
      SysReqOrAttention = 154,
      Cancel = 155,
      Clear = 156,
      Prior = 157,
      Return = 158,
      Separator = 159,
      Out = 160,
      Oper = 161,
      ClearOrAgain = 162,
      CrSelOrProps = 163,
      ExSel = 164,
      LeftControl = 224,
      LeftShift = 225,
      LeftAlt = 226,
      LeftGUI = 227,
      RightControl = 228,
      RightShift = 229,
      RightAlt = 230,
      RightGUI = 231,
    }
  }
}
