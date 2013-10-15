// Type: OpenTK.Graphics.GraphicsContextVersion
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Graphics
{
  public sealed class GraphicsContextVersion
  {
    private string vendor = string.Empty;
    private string renderer = string.Empty;
    private int minor;
    private int major;

    public int Minor
    {
      get
      {
        return this.minor;
      }
      private set
      {
        this.minor = value;
      }
    }

    public int Major
    {
      get
      {
        return this.major;
      }
      private set
      {
        this.major = value;
      }
    }

    public string Vendor
    {
      get
      {
        return this.vendor;
      }
      private set
      {
        this.vendor = value;
      }
    }

    public string Renderer
    {
      get
      {
        return this.renderer;
      }
      private set
      {
        this.renderer = value;
      }
    }

    internal GraphicsContextVersion(int minor, int major, string vendor, string renderer)
    {
      this.Minor = minor;
      this.Major = major;
      this.Vendor = vendor;
      this.Renderer = renderer;
    }
  }
}
