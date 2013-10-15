// Type: OpenTK.Platform.IGameWindow
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using System;

namespace OpenTK.Platform
{
  public interface IGameWindow : INativeWindow, IDisposable
  {
    event EventHandler<EventArgs> Load;

    event EventHandler<EventArgs> Unload;

    event EventHandler<FrameEventArgs> UpdateFrame;

    event EventHandler<FrameEventArgs> RenderFrame;

    void Run();

    void Run(double updateRate);

    void MakeCurrent();

    void SwapBuffers();
  }
}
