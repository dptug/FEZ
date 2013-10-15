// Type: FezEngine.Structure.Input.FezButtonStateExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

namespace FezEngine.Structure.Input
{
  public static class FezButtonStateExtensions
  {
    public static bool IsDown(this FezButtonState state)
    {
      if (state != FezButtonState.Pressed)
        return state == FezButtonState.Down;
      else
        return true;
    }

    public static FezButtonState NextState(this FezButtonState state, bool pressed)
    {
      switch (state)
      {
        case FezButtonState.Up:
          return !pressed ? FezButtonState.Up : FezButtonState.Pressed;
        case FezButtonState.Pressed:
          return !pressed ? FezButtonState.Released : FezButtonState.Down;
        case FezButtonState.Released:
          return !pressed ? FezButtonState.Up : FezButtonState.Pressed;
        default:
          return !pressed ? FezButtonState.Released : FezButtonState.Down;
      }
    }
  }
}
