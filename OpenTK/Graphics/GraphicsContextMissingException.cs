// Type: OpenTK.Graphics.GraphicsContextMissingException
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Threading;

namespace OpenTK.Graphics
{
  public class GraphicsContextMissingException : GraphicsContextException
  {
    public GraphicsContextMissingException()
      : base(string.Format("No context is current in the calling thread (ThreadId: {0}).", (object) Thread.CurrentThread.ManagedThreadId))
    {
    }
  }
}
