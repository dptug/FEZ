// Type: OpenTK.Input.MouseState
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Input
{
  public struct MouseState : IEquatable<MouseState>
  {
    private const int IntSize = 4;
    private const int NumInts = 3;
    private unsafe fixed int Buttons[3];
    private int x;
    private int y;
    private float wheel;
    private bool is_connected;

    public bool this[MouseButton button]
    {
      get
      {
        return this.IsButtonDown(button);
      }
      internal set
      {
        if (value)
          this.EnableBit((int) button);
        else
          this.DisableBit((int) button);
      }
    }

    public int Wheel
    {
      get
      {
        return (int) Math.Round((double) this.wheel, MidpointRounding.AwayFromZero);
      }
    }

    public float WheelPrecise
    {
      get
      {
        return this.wheel;
      }
      internal set
      {
        this.wheel = value;
      }
    }

    public int X
    {
      get
      {
        return this.x;
      }
      internal set
      {
        this.x = value;
      }
    }

    public int Y
    {
      get
      {
        return this.y;
      }
      internal set
      {
        this.y = value;
      }
    }

    public ButtonState LeftButton
    {
      get
      {
        return !this.IsButtonDown(MouseButton.Left) ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState MiddleButton
    {
      get
      {
        return !this.IsButtonDown(MouseButton.Middle) ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState RightButton
    {
      get
      {
        return !this.IsButtonDown(MouseButton.Right) ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState XButton1
    {
      get
      {
        return !this.IsButtonDown(MouseButton.Button1) ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public ButtonState XButton2
    {
      get
      {
        return !this.IsButtonDown(MouseButton.Button2) ? ButtonState.Released : ButtonState.Pressed;
      }
    }

    public int ScrollWheelValue
    {
      get
      {
        return this.Wheel;
      }
    }

    public bool IsConnected
    {
      get
      {
        return this.is_connected;
      }
      internal set
      {
        this.is_connected = value;
      }
    }

    public static bool operator ==(MouseState left, MouseState right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(MouseState left, MouseState right)
    {
      return !left.Equals(right);
    }

    public bool IsButtonDown(MouseButton button)
    {
      return this.ReadBit((int) button);
    }

    public bool IsButtonUp(MouseButton button)
    {
      return !this.ReadBit((int) button);
    }

    public override bool Equals(object obj)
    {
      if (obj is MouseState)
        return this == (MouseState) obj;
      else
        return false;
    }

    public override unsafe int GetHashCode()
    {
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr = &this.Buttons.FixedElementField)
        return numPtr->GetHashCode() ^ this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.WheelPrecise.GetHashCode();
    }

    internal unsafe bool ReadBit(int offset)
    {
      MouseState.ValidateOffset(offset);
      int index = offset / 32;
      int num = offset % 32;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr = &this.Buttons.FixedElementField)
        return (long) (numPtr[index] & 1 << num) != 0L;
    }

    internal unsafe void EnableBit(int offset)
    {
      MouseState.ValidateOffset(offset);
      int num1 = offset / 32;
      int num2 = offset % 32;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr = &this.Buttons.FixedElementField)
      {
        IntPtr num3 = (IntPtr) (numPtr + num1);
        int num4 = *(int*) num3 | 1 << num2;
        *(int*) num3 = num4;
      }
    }

    internal unsafe void DisableBit(int offset)
    {
      MouseState.ValidateOffset(offset);
      int num1 = offset / 32;
      int num2 = offset % 32;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr = &this.Buttons.FixedElementField)
      {
        IntPtr num3 = (IntPtr) (numPtr + num1);
        int num4 = *(int*) num3 & ~(1 << num2);
        *(int*) num3 = num4;
      }
    }

    internal unsafe void MergeBits(MouseState other)
    {
      // ISSUE: reference to a compiler-generated field
      int* numPtr1 = &other.Buttons.FixedElementField;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr2 = &this.Buttons.FixedElementField)
      {
        for (int index = 0; index < 3; ++index)
        {
          IntPtr num1 = (IntPtr) (numPtr2 + index);
          int num2 = *(int*) num1 | numPtr1[index];
          *(int*) num1 = num2;
        }
      }
      this.WheelPrecise += other.WheelPrecise;
      this.X += other.X;
      this.Y += other.Y;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      MouseState& local = @this;
      // ISSUE: explicit reference operation
      int num = (^local).IsConnected | other.IsConnected ? 1 : 0;
      // ISSUE: explicit reference operation
      (^local).IsConnected = num != 0;
    }

    private static void ValidateOffset(int offset)
    {
      if (offset < 0 || offset >= 12)
        throw new ArgumentOutOfRangeException("offset");
    }

    public unsafe bool Equals(MouseState other)
    {
      bool flag = true;
      // ISSUE: reference to a compiler-generated field
      int* numPtr1 = &other.Buttons.FixedElementField;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr2 = &this.Buttons.FixedElementField)
      {
        for (int index = 0; flag && index < 3; ++index)
          flag = flag & numPtr2[index] == numPtr1[index];
      }
      return ((flag ? 1 : 0) & (this.X != other.X || this.Y != other.Y ? 0 : ((double) this.WheelPrecise == (double) other.WheelPrecise ? 1 : 0))) != 0;
    }
  }
}
