// Type: OpenTK.Input.KeyboardState
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Input
{
  public struct KeyboardState : IEquatable<KeyboardState>
  {
    private const int IntSize = 4;
    private const int NumInts = 33;
    private unsafe fixed int Keys[33];
    private bool is_connected;

    public bool this[Key key]
    {
      get
      {
        return this.IsKeyDown(key);
      }
      internal set
      {
        if (value)
          this.EnableBit((int) key);
        else
          this.DisableBit((int) key);
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

    public static bool operator ==(KeyboardState left, KeyboardState right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(KeyboardState left, KeyboardState right)
    {
      return !left.Equals(right);
    }

    public bool IsKeyDown(Key key)
    {
      return this.ReadBit((int) key);
    }

    public bool IsKeyUp(Key key)
    {
      return !this.ReadBit((int) key);
    }

    public override bool Equals(object obj)
    {
      if (obj is KeyboardState)
        return this == (KeyboardState) obj;
      else
        return false;
    }

    public override unsafe int GetHashCode()
    {
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr = &this.Keys.FixedElementField)
      {
        int num = 0;
        for (int index = 0; index < 33; ++index)
          num ^= numPtr[index].GetHashCode();
        return num;
      }
    }

    internal unsafe bool ReadBit(int offset)
    {
      KeyboardState.ValidateOffset(offset);
      int index = offset / 32;
      int num = offset % 32;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr = &this.Keys.FixedElementField)
        return (long) (numPtr[index] & 1 << num) != 0L;
    }

    internal unsafe void EnableBit(int offset)
    {
      KeyboardState.ValidateOffset(offset);
      int num1 = offset / 32;
      int num2 = offset % 32;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr = &this.Keys.FixedElementField)
      {
        IntPtr num3 = (IntPtr) (numPtr + num1);
        int num4 = *(int*) num3 | 1 << num2;
        *(int*) num3 = num4;
      }
    }

    internal unsafe void DisableBit(int offset)
    {
      KeyboardState.ValidateOffset(offset);
      int num1 = offset / 32;
      int num2 = offset % 32;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr = &this.Keys.FixedElementField)
      {
        IntPtr num3 = (IntPtr) (numPtr + num1);
        int num4 = *(int*) num3 & ~(1 << num2);
        *(int*) num3 = num4;
      }
    }

    internal unsafe void MergeBits(KeyboardState other)
    {
      // ISSUE: reference to a compiler-generated field
      int* numPtr1 = &other.Keys.FixedElementField;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr2 = &this.Keys.FixedElementField)
      {
        for (int index = 0; index < 33; ++index)
        {
          IntPtr num1 = (IntPtr) (numPtr2 + index);
          int num2 = *(int*) num1 | numPtr1[index];
          *(int*) num1 = num2;
        }
      }
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      KeyboardState& local = @this;
      // ISSUE: explicit reference operation
      int num = (^local).IsConnected | other.IsConnected ? 1 : 0;
      // ISSUE: explicit reference operation
      (^local).IsConnected = num != 0;
    }

    private static void ValidateOffset(int offset)
    {
      if (offset < 0 || offset >= 132)
        throw new ArgumentOutOfRangeException("offset");
    }

    public unsafe bool Equals(KeyboardState other)
    {
      bool flag = true;
      // ISSUE: reference to a compiler-generated field
      int* numPtr1 = &other.Keys.FixedElementField;
      // ISSUE: reference to a compiler-generated field
      fixed (int* numPtr2 = &this.Keys.FixedElementField)
      {
        for (int index = 0; flag && index < 33; ++index)
          flag = flag & numPtr2[index] == numPtr1[index];
      }
      return flag;
    }
  }
}
