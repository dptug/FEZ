// Type: OpenTK.Graphics.IGraphicsContext
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform;
using System;

namespace OpenTK.Graphics
{
  public interface IGraphicsContext : IDisposable
  {
    bool IsCurrent { get; }

    bool IsDisposed { get; }

    [Obsolete("Use SwapInterval property instead.")]
    bool VSync { get; set; }

    int SwapInterval { get; set; }

    GraphicsMode GraphicsMode { get; }

    bool ErrorChecking { get; set; }

    void SwapBuffers();

    void MakeCurrent(IWindowInfo window);

    void Update(IWindowInfo window);

    void LoadAll();
  }
}
