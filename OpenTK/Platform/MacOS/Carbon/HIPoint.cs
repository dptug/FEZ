// Type: OpenTK.Platform.MacOS.Carbon.HIPoint
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Platform.MacOS.Carbon
{
  internal struct HIPoint
  {
    public float X;
    public float Y;

    public HIPoint(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }

    public HIPoint(double x, double y)
    {
      this = new HIPoint((float) x, (float) y);
    }
  }
}
