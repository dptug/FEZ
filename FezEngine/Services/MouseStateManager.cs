// Type: FezEngine.Services.MouseStateManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FezEngine.Services
{
  public class MouseStateManager : IMouseStateManager
  {
    private const int DraggingThreshold = 3;
    private MouseState lastState;
    private MouseButtonState leftButton;
    private MouseButtonState middleButton;
    private MouseButtonState rightButton;
    private FezButtonState wheelTurnedUp;
    private FezButtonState wheelTurnedDown;
    private int wheelTurns;
    private Point position;
    private Point movement;
    private IntPtr renderPanelHandle;
    private IntPtr parentFormHandle;

    public MouseButtonState LeftButton
    {
      get
      {
        return this.leftButton;
      }
    }

    public MouseButtonState MiddleButton
    {
      get
      {
        return this.middleButton;
      }
    }

    public MouseButtonState RightButton
    {
      get
      {
        return this.rightButton;
      }
    }

    public int WheelTurns
    {
      get
      {
        return this.wheelTurns;
      }
    }

    public Point Position
    {
      get
      {
        return this.position;
      }
    }

    public Point Movement
    {
      get
      {
        return this.movement;
      }
    }

    public IntPtr RenderPanelHandle
    {
      set
      {
        this.renderPanelHandle = value;
      }
    }

    public IntPtr ParentFormHandle
    {
      set
      {
        this.parentFormHandle = value;
      }
    }

    public FezButtonState WheelTurnedUp
    {
      get
      {
        return this.wheelTurnedUp;
      }
    }

    public FezButtonState WheelTurnedDown
    {
      get
      {
        return this.wheelTurnedDown;
      }
    }

    public void Update(GameTime time)
    {
      MouseState state = Mouse.GetState();
      this.wheelTurns = state.ScrollWheelValue - this.lastState.ScrollWheelValue;
      this.wheelTurnedUp = FezButtonStateExtensions.NextState(this.wheelTurnedUp, this.wheelTurns > 0);
      this.wheelTurnedDown = FezButtonStateExtensions.NextState(this.wheelTurnedDown, this.wheelTurns < 0);
      if (this.renderPanelHandle != this.parentFormHandle)
        state = Mouse.GetState();
      this.movement = new Point(state.X - this.position.X, state.Y - this.position.Y);
      this.position = new Point(state.X, state.Y);
      if (state != this.lastState)
      {
        bool hasMoved = this.movement.X != 0 || this.movement.Y != 0;
        this.leftButton = this.DeduceMouseButtonState(this.leftButton, this.lastState.LeftButton, state.LeftButton, hasMoved);
        this.middleButton = this.DeduceMouseButtonState(this.middleButton, this.lastState.MiddleButton, state.MiddleButton, hasMoved);
        this.rightButton = this.DeduceMouseButtonState(this.rightButton, this.lastState.RightButton, state.RightButton, hasMoved);
        this.lastState = state;
      }
      else
      {
        this.ResetButton(ref this.leftButton);
        this.ResetButton(ref this.middleButton);
        this.ResetButton(ref this.rightButton);
      }
    }

    private MouseButtonState DeduceMouseButtonState(MouseButtonState lastMouseButtonState, ButtonState lastButtonState, ButtonState buttonState, bool hasMoved)
    {
      if (lastButtonState == ButtonState.Released && buttonState == ButtonState.Released)
        return new MouseButtonState(MouseButtonStates.Idle);
      if (lastButtonState == ButtonState.Released && buttonState == ButtonState.Pressed)
        return new MouseButtonState(MouseButtonStates.Pressed, new MouseDragState(this.position, this.position));
      if (lastButtonState == ButtonState.Pressed && buttonState == ButtonState.Pressed)
      {
        if (!hasMoved)
          return lastMouseButtonState;
        if (!lastMouseButtonState.DragState.PreDrag || Math.Abs(this.position.X - lastMouseButtonState.DragState.Start.X) <= 3 && Math.Abs(this.position.Y - lastMouseButtonState.DragState.Start.Y) <= 3)
          return new MouseButtonState(MouseButtonStates.Down, new MouseDragState(lastMouseButtonState.DragState.Start, this.position, true));
        if (lastMouseButtonState.State == MouseButtonStates.DragStarted || lastMouseButtonState.State == MouseButtonStates.Dragging)
          return new MouseButtonState(MouseButtonStates.Dragging, new MouseDragState(lastMouseButtonState.DragState.Start, this.position, true));
        else
          return new MouseButtonState(MouseButtonStates.DragStarted, new MouseDragState(lastMouseButtonState.DragState.Start, this.position, true));
      }
      else
      {
        if (lastButtonState != ButtonState.Pressed || buttonState != ButtonState.Released)
          throw new InvalidOperationException();
        if ((lastMouseButtonState.State == MouseButtonStates.Pressed || lastMouseButtonState.State == MouseButtonStates.Down) && !hasMoved)
          return new MouseButtonState(MouseButtonStates.Clicked);
        else
          return new MouseButtonState(MouseButtonStates.DragEnded);
      }
    }

    private void ResetButton(ref MouseButtonState button)
    {
      if (button.State == MouseButtonStates.Pressed)
        button = new MouseButtonState(MouseButtonStates.Down, button.DragState);
      if (button.State == MouseButtonStates.Clicked || button.State == MouseButtonStates.DragEnded)
        button = new MouseButtonState(MouseButtonStates.Idle);
      if (button.State != MouseButtonStates.DragStarted)
        return;
      button = new MouseButtonState(MouseButtonStates.Dragging, button.DragState);
    }
  }
}
