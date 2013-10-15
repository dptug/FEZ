// Type: Microsoft.Xna.Framework.Input.Touch.GestureSample
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Input.Touch
{
  public struct GestureSample
  {
    private GestureType _gestureType;
    private TimeSpan _timestamp;
    private Vector2 _position;
    private Vector2 _position2;
    private Vector2 _delta;
    private Vector2 _delta2;

    public GestureType GestureType
    {
      get
      {
        return this._gestureType;
      }
    }

    public TimeSpan Timestamp
    {
      get
      {
        return this._timestamp;
      }
    }

    public Vector2 Position
    {
      get
      {
        return this._position;
      }
    }

    public Vector2 Position2
    {
      get
      {
        return this._position2;
      }
    }

    public Vector2 Delta
    {
      get
      {
        return this._delta;
      }
    }

    public Vector2 Delta2
    {
      get
      {
        return this._delta2;
      }
    }

    public GestureSample(GestureType gestureType, TimeSpan timestamp, Vector2 position, Vector2 position2, Vector2 delta, Vector2 delta2)
    {
      this._gestureType = gestureType;
      this._timestamp = timestamp;
      this._position = position;
      this._position2 = position2;
      this._delta = delta;
      this._delta2 = delta2;
    }
  }
}
