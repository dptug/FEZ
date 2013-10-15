// Type: Microsoft.Xna.Framework.Input.Touch.GestureSample
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
