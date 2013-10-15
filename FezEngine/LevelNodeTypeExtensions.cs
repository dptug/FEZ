// Type: FezEngine.LevelNodeTypeExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine
{
  public static class LevelNodeTypeExtensions
  {
    public static float GetSizeFactor(this LevelNodeType nodeType)
    {
      switch (nodeType)
      {
        case LevelNodeType.Node:
          return 1f;
        case LevelNodeType.Hub:
          return 2f;
        case LevelNodeType.Lesser:
          return 0.5f;
        default:
          throw new InvalidOperationException();
      }
    }
  }
}
