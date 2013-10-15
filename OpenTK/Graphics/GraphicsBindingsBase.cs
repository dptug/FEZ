// Type: OpenTK.Graphics.GraphicsBindingsBase
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using System;

namespace OpenTK.Graphics
{
  public abstract class GraphicsBindingsBase : BindingsBase
  {
    protected override IntPtr GetAddress(string funcname)
    {
      return (GraphicsContext.CurrentContext as IGraphicsContextInternal).GetAddress(funcname);
    }
  }
}
