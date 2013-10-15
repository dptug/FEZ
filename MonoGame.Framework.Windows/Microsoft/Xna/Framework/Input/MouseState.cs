// Type: Microsoft.Xna.Framework.Input.MouseState
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Input
{
  public struct MouseState
  {
    private int _x;
    private int _y;
    private int _scrollWheelValue;
    private ButtonState _leftButton;
    private ButtonState _rightButton;
    private ButtonState _middleButton;

    public int X
    {
      get
      {
        return this._x;
      }
      internal set
      {
        this._x = value;
      }
    }

    public int Y
    {
      get
      {
        return this._y;
      }
      internal set
      {
        this._y = value;
      }
    }

    public ButtonState LeftButton
    {
      get
      {
        return this._leftButton;
      }
      internal set
      {
        this._leftButton = value;
      }
    }

    public ButtonState MiddleButton
    {
      get
      {
        return this._middleButton;
      }
      internal set
      {
        this._middleButton = value;
      }
    }

    public ButtonState RightButton
    {
      get
      {
        return this._rightButton;
      }
      internal set
      {
        this._rightButton = value;
      }
    }

    public int ScrollWheelValue
    {
      get
      {
        return this._scrollWheelValue;
      }
      internal set
      {
        this._scrollWheelValue = value;
      }
    }

    public ButtonState XButton1
    {
      get
      {
        return ButtonState.Released;
      }
    }

    public ButtonState XButton2
    {
      get
      {
        return ButtonState.Released;
      }
    }

    public MouseState(int x, int y, int scrollWheel, ButtonState leftButton, ButtonState middleButton, ButtonState rightButton, ButtonState xButton1, ButtonState xButton2)
    {
      this._x = x;
      this._y = y;
      this._scrollWheelValue = scrollWheel;
      this._leftButton = leftButton;
      this._middleButton = middleButton;
      this._rightButton = rightButton;
    }

    public static bool operator ==(MouseState left, MouseState right)
    {
      return left._x == right._x && left._y == right._y && (left._leftButton == right._leftButton && left._middleButton == right._middleButton) && left._rightButton == right._rightButton && left._scrollWheelValue == right._scrollWheelValue;
    }

    public static bool operator !=(MouseState left, MouseState right)
    {
      return !(left == right);
    }

    public override bool Equals(object obj)
    {
      if (obj is MouseState)
        return this == (MouseState) obj;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
