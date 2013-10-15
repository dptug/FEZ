// Type: OpenTK.Platform.MacOS.Carbon.CarbonPoint
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Platform.MacOS.Carbon
{
  internal struct CarbonPoint
  {
    internal short V;
    internal short H;

    public CarbonPoint(int x, int y)
    {
      this.V = (short) x;
      this.H = (short) y;
    }
  }
}
