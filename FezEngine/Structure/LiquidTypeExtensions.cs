// Type: FezEngine.Structure.LiquidTypeExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

namespace FezEngine.Structure
{
  public static class LiquidTypeExtensions
  {
    public static bool IsWater(this LiquidType waterType)
    {
      if (waterType != LiquidType.Blood && waterType != LiquidType.Water && waterType != LiquidType.Purple)
        return waterType == LiquidType.Green;
      else
        return true;
    }
  }
}
