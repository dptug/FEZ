// Type: OpenTK.Platform.EmbeddedGraphicsContext
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;

namespace OpenTK.Platform
{
  internal abstract class EmbeddedGraphicsContext : GraphicsContextBase
  {
    public override void LoadAll()
    {
      new OpenTK.Graphics.ES10.GL().LoadEntryPoints();
      new OpenTK.Graphics.ES11.GL().LoadEntryPoints();
      new OpenTK.Graphics.ES20.GL().LoadEntryPoints();
    }
  }
}
