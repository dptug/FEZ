// Type: OpenTK.Platform.X11.EventMask
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  [Flags]
  public enum EventMask
  {
    NoEventMask = 0,
    KeyPressMask = 1,
    KeyReleaseMask = 2,
    ButtonPressMask = 4,
    ButtonReleaseMask = 8,
    EnterWindowMask = 16,
    LeaveWindowMask = 32,
    PointerMotionMask = 64,
    PointerMotionHintMask = 128,
    Button1MotionMask = 256,
    Button2MotionMask = 512,
    Button3MotionMask = 1024,
    Button4MotionMask = 2048,
    Button5MotionMask = 4096,
    ButtonMotionMask = 8192,
    KeymapStateMask = 16384,
    ExposureMask = 32768,
    VisibilityChangeMask = 65536,
    StructureNotifyMask = 131072,
    ResizeRedirectMask = 262144,
    SubstructureNotifyMask = 524288,
    SubstructureRedirectMask = 1048576,
    FocusChangeMask = 2097152,
    PropertyChangeMask = 4194304,
    ColormapChangeMask = 8388608,
    OwnerGrabButtonMask = 16777216,
  }
}
