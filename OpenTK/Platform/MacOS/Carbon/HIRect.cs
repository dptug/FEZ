// Type: OpenTK.Platform.MacOS.Carbon.HIRect
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Platform.MacOS.Carbon
{
  internal struct HIRect
  {
    public HIPoint Origin;
    public HISize Size;

    public override string ToString()
    {
      return string.Format("Rect: [{0}, {1}, {2}, {3}]", (object) this.Origin.X, (object) this.Origin.Y, (object) this.Size.Width, (object) this.Size.Height);
    }
  }
}
