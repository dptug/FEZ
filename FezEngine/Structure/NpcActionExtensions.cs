// Type: FezEngine.Structure.NpcActionExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

namespace FezEngine.Structure
{
  public static class NpcActionExtensions
  {
    public static bool AllowsRandomChange(this NpcAction action)
    {
      switch (action)
      {
        case NpcAction.Idle:
        case NpcAction.Idle3:
        case NpcAction.Walk:
          return true;
        default:
          return false;
      }
    }

    public static bool Loops(this NpcAction action)
    {
      switch (action)
      {
        case NpcAction.Idle2:
        case NpcAction.Turn:
        case NpcAction.Burrow:
        case NpcAction.Hide:
        case NpcAction.ComeOut:
        case NpcAction.TakeOff:
        case NpcAction.Land:
          return false;
        default:
          return true;
      }
    }

    public static bool IsSpecialIdle(this NpcAction action)
    {
      switch (action)
      {
        case NpcAction.Idle2:
        case NpcAction.Idle3:
          return true;
        default:
          return false;
      }
    }
  }
}
