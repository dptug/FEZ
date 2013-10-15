// Type: FezEngine.Components.CloudLayerExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

namespace FezEngine.Components
{
  internal static class CloudLayerExtensions
  {
    public static float SpeedFactor(this Layer layer)
    {
      switch (layer)
      {
        case Layer.Middle:
          return 0.6f;
        case Layer.Near:
          return 1f;
        default:
          return 0.2f;
      }
    }

    public static float DistanceFactor(this Layer layer)
    {
      switch (layer)
      {
        case Layer.Middle:
          return 0.5f;
        case Layer.Near:
          return 0.0f;
        default:
          return 1f;
      }
    }

    public static float ParallaxFactor(this Layer layer)
    {
      switch (layer)
      {
        case Layer.Middle:
          return 0.4f;
        case Layer.Near:
          return 0.2f;
        default:
          return 0.6f;
      }
    }

    public static float Opacity(this Layer layer)
    {
      switch (layer)
      {
        case Layer.Middle:
          return 0.6f;
        case Layer.Near:
          return 1f;
        default:
          return 0.3f;
      }
    }
  }
}
