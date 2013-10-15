// Type: Microsoft.Xna.Framework.Input.Touch.TouchPanel
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Input.Touch
{
  public static class TouchPanel
  {
    private static readonly List<TouchLocation> _touchState = new List<TouchLocation>();
    private static readonly List<TouchLocation> _touchEvents = new List<TouchLocation>();
    private static readonly List<TouchLocation> _gestureState = new List<TouchLocation>();
    private static readonly List<TouchLocation> _gestureEvents = new List<TouchLocation>();
    private static Vector2 _touchScale = Vector2.One;
    private static Point _displaySize = Point.Zero;
    private static int _nextTouchId = 1;
    private static readonly Dictionary<int, int> _touchIds = new Dictionary<int, int>();
    private static readonly Queue<GestureSample> GestureList = new Queue<GestureSample>();
    private static TouchPanelCapabilities Capabilities = new TouchPanelCapabilities();
    private static readonly TimeSpan _maxTicksToProcessHold = TimeSpan.FromMilliseconds(1024.0);
    private static readonly TouchLocation[] _pinchTouch = new TouchLocation[2];
    private static GestureType _dragGestureStarted = GestureType.None;
    private const int MaxEvents = 100;
    private const float TapJitterTolerance = 35f;
    private static bool _pinchGestureStarted;
    private static bool _tapDisabled;
    private static bool _holdDisabled;
    private static TouchLocation _lastTap;

    public static IntPtr WindowHandle { get; set; }

    public static int DisplayHeight
    {
      get
      {
        return TouchPanel._displaySize.Y;
      }
      set
      {
        TouchPanel._displaySize.Y = value;
        TouchPanel.UpdateTouchScale();
      }
    }

    public static DisplayOrientation DisplayOrientation { get; set; }

    public static int DisplayWidth
    {
      get
      {
        return TouchPanel._displaySize.X;
      }
      set
      {
        TouchPanel._displaySize.X = value;
        TouchPanel.UpdateTouchScale();
      }
    }

    public static GestureType EnabledGestures { get; set; }

    public static bool IsGestureAvailable
    {
      get
      {
        TouchPanel.UpdateGestures(TouchPanel.RefreshState(true, TouchPanel._gestureState, TouchPanel._gestureEvents));
        return TouchPanel.GestureList.Count > 0;
      }
    }

    static TouchPanel()
    {
    }

    public static TouchPanelCapabilities GetCapabilities()
    {
      TouchPanel.Capabilities.Initialize();
      return TouchPanel.Capabilities;
    }

    private static bool RefreshState(bool consumeState, List<TouchLocation> state, List<TouchLocation> events)
    {
      bool flag1 = false;
      for (int index1 = 0; index1 < state.Count; ++index1)
      {
        TouchLocation touchLocation = state[index1];
        if (consumeState || touchLocation.State == TouchLocationState.Moved)
        {
          if (consumeState && touchLocation.State == TouchLocationState.Released)
          {
            state.RemoveAt(index1);
            --index1;
          }
          else
          {
            bool flag2 = false;
            for (int index2 = 0; index2 < events.Count; ++index2)
            {
              TouchLocation touchEvent = events[index2];
              if ((consumeState || touchEvent.State != TouchLocationState.Released) && touchEvent.Id == touchLocation.Id)
              {
                flag1 = flag1 | touchLocation.UpdateState(touchEvent);
                flag2 = true;
                events.RemoveAt(index2);
                break;
              }
            }
            state[index1] = !flag2 ? touchLocation.AsMovedState() : touchLocation;
          }
        }
      }
      int index = 0;
      while (index < events.Count)
      {
        TouchLocation touchLocation = events[index];
        if (touchLocation.State == TouchLocationState.Pressed)
        {
          state.Add(touchLocation);
          events.RemoveAt(index);
          flag1 = true;
        }
        else
          ++index;
      }
      return flag1;
    }

    public static TouchCollection GetState()
    {
      bool consumeState = true;
      while (TouchPanel.RefreshState(consumeState, TouchPanel._touchState, TouchPanel._touchEvents))
        consumeState = false;
      return new TouchCollection(TouchPanel._touchState.ToArray());
    }

    internal static void AddEvent(int id, TouchLocationState state, Vector2 position)
    {
      if (state == TouchLocationState.Pressed)
        TouchPanel._touchIds[id] = TouchPanel._nextTouchId++;
      int id1;
      if (!TouchPanel._touchIds.TryGetValue(id, out id1))
        return;
      TouchLocation touchLocation = new TouchLocation(id1, state, position * TouchPanel._touchScale);
      TouchPanel._touchEvents.Add(touchLocation);
      if (TouchPanel._touchEvents.Count > 100)
        TouchPanel._touchEvents.RemoveRange(0, TouchPanel._touchEvents.Count - 100);
      if (TouchPanel.EnabledGestures != GestureType.None)
      {
        TouchPanel._gestureEvents.Add(touchLocation);
        if (TouchPanel._gestureEvents.Count > 100)
          TouchPanel._gestureEvents.RemoveRange(0, TouchPanel._gestureEvents.Count - 100);
      }
      if (state != TouchLocationState.Released)
        return;
      TouchPanel._touchIds.Remove(id);
    }

    internal static void ReleaseAllTouches()
    {
      TouchPanel._touchEvents.Clear();
      TouchPanel._gestureEvents.Clear();
      foreach (TouchLocation touchLocation in TouchPanel._touchState)
      {
        if (touchLocation.State != TouchLocationState.Released)
          TouchPanel._touchEvents.Add(new TouchLocation(touchLocation.Id, TouchLocationState.Released, touchLocation.Position));
      }
      foreach (TouchLocation touchLocation in TouchPanel._gestureState)
      {
        if (touchLocation.State != TouchLocationState.Released)
          TouchPanel._gestureEvents.Add(new TouchLocation(touchLocation.Id, TouchLocationState.Released, touchLocation.Position));
      }
      TouchPanel._touchIds.Clear();
    }

    private static void UpdateTouchScale()
    {
      Vector2 vector2 = Vector2.One;
      if (Game.Instance != null)
        vector2 = new Vector2((float) Game.Instance.Window.ClientBounds.Width, (float) Game.Instance.Window.ClientBounds.Height);
      TouchPanel._touchScale = new Vector2((float) TouchPanel.DisplayWidth / vector2.X, (float) TouchPanel.DisplayHeight / vector2.Y);
    }

    public static GestureSample ReadGesture()
    {
      return TouchPanel.GestureList.Dequeue();
    }

    private static bool GestureIsEnabled(GestureType gestureType)
    {
      return (TouchPanel.EnabledGestures & gestureType) != GestureType.None;
    }

    private static void UpdateGestures(bool stateChanged)
    {
      int num1 = 0;
      foreach (TouchLocation touchLocation in TouchPanel._gestureState)
        num1 += touchLocation.State != TouchLocationState.Released ? 1 : 0;
      if (num1 > 1)
      {
        TouchPanel._tapDisabled = true;
        TouchPanel._holdDisabled = true;
      }
      foreach (TouchLocation touch in TouchPanel._gestureState)
      {
        switch (touch.State)
        {
          case TouchLocationState.Moved:
          case TouchLocationState.Pressed:
            if (touch.State != TouchLocationState.Pressed || !TouchPanel.ProcessDoubleTap(touch))
            {
              if (TouchPanel.GestureIsEnabled(GestureType.Pinch) && num1 > 1)
              {
                if (TouchPanel._pinchTouch[0].State == TouchLocationState.Invalid || TouchPanel._pinchTouch[0].Id == touch.Id)
                {
                  TouchPanel._pinchTouch[0] = touch;
                  break;
                }
                else if (TouchPanel._pinchTouch[1].State == TouchLocationState.Invalid || TouchPanel._pinchTouch[1].Id == touch.Id)
                {
                  TouchPanel._pinchTouch[1] = touch;
                  break;
                }
                else
                  break;
              }
              else
              {
                float num2 = Vector2.Distance(touch.Position, touch.PressPosition);
                if (TouchPanel._dragGestureStarted == GestureType.None && (double) num2 < 35.0)
                {
                  TouchPanel.ProcessHold(touch);
                  break;
                }
                else if (stateChanged)
                {
                  TouchPanel.ProcessDrag(touch);
                  break;
                }
                else
                  break;
              }
            }
            else
              break;
          case TouchLocationState.Released:
            if (stateChanged)
            {
              if (TouchPanel._pinchGestureStarted && (touch.Id == TouchPanel._pinchTouch[0].Id || touch.Id == TouchPanel._pinchTouch[1].Id))
              {
                if (TouchPanel.GestureIsEnabled(GestureType.PinchComplete))
                  TouchPanel.GestureList.Enqueue(new GestureSample(GestureType.PinchComplete, touch.Timestamp, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero));
                TouchPanel._pinchGestureStarted = false;
                TouchPanel._pinchTouch[0] = TouchLocation.Invalid;
                TouchPanel._pinchTouch[1] = TouchLocation.Invalid;
                break;
              }
              else if (num1 == 0)
              {
                if ((double) Vector2.Distance(touch.Position, touch.PressPosition) > 35.0 && (double) touch.Velocity.Length() > 100.0 && TouchPanel.GestureIsEnabled(GestureType.Flick))
                {
                  TouchPanel.GestureList.Enqueue(new GestureSample(GestureType.Flick, touch.Timestamp, Vector2.Zero, Vector2.Zero, touch.Velocity, Vector2.Zero));
                  TouchPanel._dragGestureStarted = GestureType.None;
                  break;
                }
                else if (TouchPanel._dragGestureStarted != GestureType.None)
                {
                  if (TouchPanel.GestureIsEnabled(GestureType.DragComplete))
                    TouchPanel.GestureList.Enqueue(new GestureSample(GestureType.DragComplete, touch.Timestamp, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero));
                  TouchPanel._dragGestureStarted = GestureType.None;
                  break;
                }
                else
                {
                  TouchPanel.ProcessTap(touch);
                  break;
                }
              }
              else
                break;
            }
            else
              break;
        }
      }
      if (!stateChanged)
        return;
      if (TouchPanel.GestureIsEnabled(GestureType.Pinch) && TouchPanel._pinchTouch[0].State != TouchLocationState.Invalid && TouchPanel._pinchTouch[1].State != TouchLocationState.Invalid)
      {
        TouchPanel.ProcessPinch(TouchPanel._pinchTouch);
      }
      else
      {
        TouchPanel._pinchGestureStarted = false;
        TouchPanel._pinchTouch[0] = TouchLocation.Invalid;
        TouchPanel._pinchTouch[1] = TouchLocation.Invalid;
      }
      if (num1 != 0)
        return;
      TouchPanel._tapDisabled = false;
      TouchPanel._holdDisabled = false;
      TouchPanel._dragGestureStarted = GestureType.None;
    }

    private static void ProcessHold(TouchLocation touch)
    {
      if (!TouchPanel.GestureIsEnabled(GestureType.Hold) || TouchPanel._holdDisabled || TimeSpan.FromTicks(DateTime.Now.Ticks) - touch.PressTimestamp < TouchPanel._maxTicksToProcessHold)
        return;
      TouchPanel._holdDisabled = true;
      TouchPanel.GestureList.Enqueue(new GestureSample(GestureType.Hold, touch.Timestamp, touch.Position, Vector2.Zero, Vector2.Zero, Vector2.Zero));
    }

    private static bool ProcessDoubleTap(TouchLocation touch)
    {
      if (!TouchPanel.GestureIsEnabled(GestureType.DoubleTap) || TouchPanel._tapDisabled || (double) Vector2.Distance(touch.Position, TouchPanel._lastTap.Position) > 35.0 || (touch.Timestamp - TouchPanel._lastTap.Timestamp).TotalMilliseconds > 300.0)
        return false;
      TouchPanel.GestureList.Enqueue(new GestureSample(GestureType.DoubleTap, touch.Timestamp, touch.Position, Vector2.Zero, Vector2.Zero, Vector2.Zero));
      TouchPanel._tapDisabled = true;
      return true;
    }

    private static void ProcessTap(TouchLocation touch)
    {
      if (!TouchPanel.GestureIsEnabled(GestureType.Tap) || TouchPanel._tapDisabled || (double) Vector2.Distance(touch.PressPosition, touch.Position) > 35.0 || TimeSpan.FromTicks(DateTime.Now.Ticks) - touch.PressTimestamp > TouchPanel._maxTicksToProcessHold)
        return;
      TouchPanel._lastTap = touch;
      GestureSample gestureSample = new GestureSample(GestureType.Tap, touch.Timestamp, touch.Position, Vector2.Zero, Vector2.Zero, Vector2.Zero);
      TouchPanel.GestureList.Enqueue(gestureSample);
    }

    private static void ProcessDrag(TouchLocation touch)
    {
      bool flag1 = TouchPanel.GestureIsEnabled(GestureType.HorizontalDrag);
      bool flag2 = TouchPanel.GestureIsEnabled(GestureType.VerticalDrag);
      bool flag3 = TouchPanel.GestureIsEnabled(GestureType.FreeDrag);
      TouchLocation aPreviousLocation;
      if (!flag1 && !flag2 && !flag3 || (touch.State != TouchLocationState.Moved || !touch.TryGetPreviousLocation(out aPreviousLocation)))
        return;
      Vector2 delta = touch.Position - aPreviousLocation.Position;
      if (TouchPanel._dragGestureStarted != GestureType.FreeDrag)
      {
        bool flag4 = (double) Math.Abs(delta.X) > (double) Math.Abs(delta.Y * 2f);
        bool flag5 = (double) Math.Abs(delta.Y) > (double) Math.Abs(delta.X * 2f);
        bool flag6 = TouchPanel._dragGestureStarted == GestureType.None;
        if (flag1 && (flag6 && flag4 || TouchPanel._dragGestureStarted == GestureType.HorizontalDrag))
        {
          delta.Y = 0.0f;
          TouchPanel._dragGestureStarted = GestureType.HorizontalDrag;
        }
        else if (flag2 && (flag6 && flag5 || TouchPanel._dragGestureStarted == GestureType.VerticalDrag))
        {
          delta.X = 0.0f;
          TouchPanel._dragGestureStarted = GestureType.VerticalDrag;
        }
        else
          TouchPanel._dragGestureStarted = !flag3 || !flag6 ? GestureType.DragComplete : GestureType.FreeDrag;
      }
      if (TouchPanel._dragGestureStarted == GestureType.None || TouchPanel._dragGestureStarted == GestureType.DragComplete)
        return;
      TouchPanel._tapDisabled = true;
      TouchPanel._holdDisabled = true;
      TouchPanel.GestureList.Enqueue(new GestureSample(TouchPanel._dragGestureStarted, touch.Timestamp, touch.Position, Vector2.Zero, delta, Vector2.Zero));
    }

    private static void ProcessPinch(TouchLocation[] touches)
    {
      TouchLocation aPreviousLocation1;
      if (!touches[0].TryGetPreviousLocation(out aPreviousLocation1))
        aPreviousLocation1 = touches[0];
      TouchLocation aPreviousLocation2;
      if (!touches[1].TryGetPreviousLocation(out aPreviousLocation2))
        aPreviousLocation2 = touches[1];
      Vector2 delta = touches[0].Position - aPreviousLocation1.Position;
      Vector2 delta2 = touches[1].Position - aPreviousLocation2.Position;
      TimeSpan timestamp = touches[0].Timestamp > touches[1].Timestamp ? touches[0].Timestamp : touches[1].Timestamp;
      if (TouchPanel._dragGestureStarted != GestureType.None)
      {
        if (TouchPanel.GestureIsEnabled(GestureType.DragComplete))
          TouchPanel.GestureList.Enqueue(new GestureSample(GestureType.DragComplete, timestamp, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero));
        TouchPanel._dragGestureStarted = GestureType.None;
      }
      TouchPanel.GestureList.Enqueue(new GestureSample(GestureType.Pinch, timestamp, touches[0].Position, touches[1].Position, delta, delta2));
      TouchPanel._pinchGestureStarted = true;
      TouchPanel._tapDisabled = true;
      TouchPanel._holdDisabled = true;
    }
  }
}
