// Type: SharpDX.XInput.Gamepad
// Assembly: SharpDX.XInput, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 7810FD3A-F5EE-4EAB-B451-0D4E18D9FE4F
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.XInput.dll

using System.Runtime.InteropServices;

namespace SharpDX.XInput
{
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public struct Gamepad
  {
    public const byte TriggerThreshold = (byte) 30;
    public const short LeftThumbDeadZone = (short) 7849;
    public const short RightThumbDeadZone = (short) 8689;
    public GamepadButtonFlags Buttons;
    public byte LeftTrigger;
    public byte RightTrigger;
    public short LeftThumbX;
    public short LeftThumbY;
    public short RightThumbX;
    public short RightThumbY;

    public override string ToString()
    {
      return string.Format("Buttons: {0}, LeftTrigger: {1}, RightTrigger: {2}, LeftThumbX: {3}, LeftThumbY: {4}, RightThumbX: {5}, RightThumbY: {6}", (object) this.Buttons, (object) this.LeftTrigger, (object) this.RightTrigger, (object) this.LeftThumbX, (object) this.LeftThumbY, (object) this.RightThumbX, (object) this.RightThumbY);
    }
  }
}
