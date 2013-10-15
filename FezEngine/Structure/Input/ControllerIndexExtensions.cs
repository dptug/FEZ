// Type: FezEngine.Structure.Input.ControllerIndexExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;

namespace FezEngine.Structure.Input
{
  public static class ControllerIndexExtensions
  {
    private static readonly PlayerIndex[] None = new PlayerIndex[0];
    private static readonly PlayerIndex[] One = new PlayerIndex[1];
    private static readonly PlayerIndex[] Two = new PlayerIndex[1]
    {
      PlayerIndex.Two
    };
    private static readonly PlayerIndex[] Three = new PlayerIndex[1]
    {
      PlayerIndex.Three
    };
    private static readonly PlayerIndex[] Four = new PlayerIndex[1]
    {
      PlayerIndex.Four
    };
    private static readonly PlayerIndex[] Any = new PlayerIndex[4]
    {
      PlayerIndex.One,
      PlayerIndex.Two,
      PlayerIndex.Three,
      PlayerIndex.Four
    };

    static ControllerIndexExtensions()
    {
    }

    public static PlayerIndex GetPlayer(this ControllerIndex index)
    {
      switch (index)
      {
        case ControllerIndex.One:
          return PlayerIndex.One;
        case ControllerIndex.Two:
          return PlayerIndex.Two;
        case ControllerIndex.Three:
          return PlayerIndex.Three;
        case ControllerIndex.Four:
          return PlayerIndex.Four;
        default:
          return PlayerIndex.One;
      }
    }

    public static PlayerIndex[] GetPlayers(this ControllerIndex index)
    {
      switch (index)
      {
        case ControllerIndex.None:
          return ControllerIndexExtensions.None;
        case ControllerIndex.One:
          return ControllerIndexExtensions.One;
        case ControllerIndex.Two:
          return ControllerIndexExtensions.Two;
        case ControllerIndex.Three:
          return ControllerIndexExtensions.Three;
        case ControllerIndex.Four:
          return ControllerIndexExtensions.Four;
        case ControllerIndex.Any:
          return ControllerIndexExtensions.Any;
        default:
          return ControllerIndexExtensions.None;
      }
    }

    public static ControllerIndex ToControllerIndex(this PlayerIndex index)
    {
      switch (index)
      {
        case PlayerIndex.One:
          return ControllerIndex.One;
        case PlayerIndex.Two:
          return ControllerIndex.Two;
        case PlayerIndex.Three:
          return ControllerIndex.Three;
        case PlayerIndex.Four:
          return ControllerIndex.Four;
        default:
          return ControllerIndex.None;
      }
    }
  }
}
