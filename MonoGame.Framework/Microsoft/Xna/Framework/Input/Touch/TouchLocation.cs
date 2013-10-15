// Type: Microsoft.Xna.Framework.Input.Touch.TouchLocation
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Input.Touch
{
  public struct TouchLocation : IEquatable<TouchLocation>
  {
    internal static readonly TouchLocation Invalid = new TouchLocation();
    private int _id;
    private Vector2 _position;
    private Vector2 _previousPosition;
    private TouchLocationState _state;
    private TouchLocationState _previousState;
    private float _pressure;
    private float _previousPressure;
    private Vector2 _velocity;
    private Vector2 _pressPosition;
    private TimeSpan _pressTimestamp;
    private TimeSpan _timestamp;

    internal Vector2 PressPosition
    {
      get
      {
        return this._pressPosition;
      }
    }

    internal TimeSpan PressTimestamp
    {
      get
      {
        return this._pressTimestamp;
      }
    }

    internal TimeSpan Timestamp
    {
      get
      {
        return this._timestamp;
      }
    }

    internal Vector2 Velocity
    {
      get
      {
        return this._velocity;
      }
    }

    public int Id
    {
      get
      {
        return this._id;
      }
    }

    public Vector2 Position
    {
      get
      {
        return this._position;
      }
    }

    public float Pressure
    {
      get
      {
        return this._pressure;
      }
    }

    public TouchLocationState State
    {
      get
      {
        return this._state;
      }
    }

    static TouchLocation()
    {
    }

    public TouchLocation(int id, TouchLocationState state, Vector2 position)
    {
      this = new TouchLocation(id, state, position, TouchLocationState.Invalid, Vector2.Zero);
    }

    public TouchLocation(int id, TouchLocationState state, Vector2 position, TouchLocationState previousState, Vector2 previousPosition)
    {
      this._id = id;
      this._state = state;
      this._position = position;
      this._pressure = 0.0f;
      this._previousState = previousState;
      this._previousPosition = previousPosition;
      this._previousPressure = 0.0f;
      this._timestamp = TimeSpan.FromTicks(DateTime.Now.Ticks);
      this._velocity = Vector2.Zero;
      if (state == TouchLocationState.Pressed)
      {
        this._pressPosition = this._position;
        this._pressTimestamp = this._timestamp;
      }
      else
      {
        this._pressPosition = Vector2.Zero;
        this._pressTimestamp = TimeSpan.Zero;
      }
    }

    public static bool operator !=(TouchLocation value1, TouchLocation value2)
    {
      if (value1._id == value2._id && value1._state == value2._state && (!(value1._position != value2._position) && value1._previousState == value2._previousState))
        return value1._previousPosition != value2._previousPosition;
      else
        return true;
    }

    public static bool operator ==(TouchLocation value1, TouchLocation value2)
    {
      if (value1._id == value2._id && value1._state == value2._state && (value1._position == value2._position && value1._previousState == value2._previousState))
        return value1._previousPosition == value2._previousPosition;
      else
        return false;
    }

    internal TouchLocation AsMovedState()
    {
      TouchLocation touchLocation = this;
      touchLocation._previousState = touchLocation._state;
      touchLocation._previousPosition = touchLocation._position;
      touchLocation._previousPressure = touchLocation._pressure;
      touchLocation._state = TouchLocationState.Moved;
      return touchLocation;
    }

    internal bool UpdateState(TouchLocation touchEvent)
    {
      this._previousPosition = this._position;
      this._previousState = this._state;
      this._previousPressure = this._pressure;
      this._position = touchEvent._position;
      this._state = touchEvent._state;
      this._pressure = touchEvent._pressure;
      Vector2 vector2 = this._position - this._previousPosition;
      TimeSpan timeSpan = touchEvent.Timestamp - this._timestamp;
      if (timeSpan > TimeSpan.Zero)
        this._velocity += (vector2 / (float) timeSpan.TotalSeconds - this._velocity) * 0.45f;
      this._timestamp = touchEvent.Timestamp;
      if (this._state == this._previousState)
        return (double) vector2.LengthSquared() > 1.0 / 1000.0;
      else
        return true;
    }

    public override bool Equals(object obj)
    {
      if (obj is TouchLocation)
        return this.Equals((TouchLocation) obj);
      else
        return false;
    }

    public bool Equals(TouchLocation other)
    {
      if (this._id.Equals(other._id) && this._position.Equals(other._position))
        return this._previousPosition.Equals(other._previousPosition);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this._id;
    }

    public override string ToString()
    {
      return "Touch id:" + (object) this._id + " state:" + (string) (object) this._state + " position:" + (string) (object) this._position + " pressure:" + (string) (object) this._pressure + " prevState:" + (string) (object) this._previousState + " prevPosition:" + (string) (object) this._previousPosition + " previousPressure:" + (string) (object) this._previousPressure;
    }

    public bool TryGetPreviousLocation(out TouchLocation aPreviousLocation)
    {
      if (this._previousState == TouchLocationState.Invalid)
      {
        aPreviousLocation._id = -1;
        aPreviousLocation._state = TouchLocationState.Invalid;
        aPreviousLocation._position = Vector2.Zero;
        aPreviousLocation._previousState = TouchLocationState.Invalid;
        aPreviousLocation._previousPosition = Vector2.Zero;
        aPreviousLocation._pressure = 0.0f;
        aPreviousLocation._previousPressure = 0.0f;
        aPreviousLocation._timestamp = TimeSpan.Zero;
        aPreviousLocation._pressPosition = Vector2.Zero;
        aPreviousLocation._pressTimestamp = TimeSpan.Zero;
        aPreviousLocation._velocity = Vector2.Zero;
        return false;
      }
      else
      {
        aPreviousLocation._id = this._id;
        aPreviousLocation._state = this._previousState;
        aPreviousLocation._position = this._previousPosition;
        aPreviousLocation._previousState = TouchLocationState.Invalid;
        aPreviousLocation._previousPosition = Vector2.Zero;
        aPreviousLocation._pressure = this._previousPressure;
        aPreviousLocation._previousPressure = 0.0f;
        aPreviousLocation._timestamp = this._timestamp;
        aPreviousLocation._pressPosition = this._pressPosition;
        aPreviousLocation._pressTimestamp = this._pressTimestamp;
        aPreviousLocation._velocity = this._velocity;
        return true;
      }
    }
  }
}
