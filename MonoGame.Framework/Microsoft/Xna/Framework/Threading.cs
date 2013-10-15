// Type: Microsoft.Xna.Framework.Threading
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System;
using System.Threading;

namespace Microsoft.Xna.Framework
{
  internal class Threading
  {
    private static int mainThreadId = Thread.CurrentThread.ManagedThreadId;
    private static int currentThreadId;
    public static IGraphicsContext BackgroundContext;
    public static IWindowInfo WindowInfo;

    static Threading()
    {
    }

    public static bool IsOnUIThread()
    {
      return Threading.mainThreadId == Thread.CurrentThread.ManagedThreadId;
    }

    public static void EnsureUIThread()
    {
      if (Threading.mainThreadId != Thread.CurrentThread.ManagedThreadId)
        throw new InvalidOperationException(string.Format("Operation not called on UI thread. UI thread ID = {0}. This thread ID = {1}.", (object) Threading.mainThreadId, (object) Thread.CurrentThread.ManagedThreadId));
    }

    internal static void BlockOnUIThread(Action action)
    {
      if (action == null)
        throw new ArgumentNullException("action");
      if (Threading.mainThreadId == Thread.CurrentThread.ManagedThreadId)
        action();
      else if (Threading.BackgroundContext == null)
      {
        action();
      }
      else
      {
        lock (Threading.BackgroundContext)
        {
          Threading.BackgroundContext.MakeCurrent(Threading.WindowInfo);
          action();
          GL.Flush();
          Threading.BackgroundContext.MakeCurrent((IWindowInfo) null);
        }
      }
    }
  }
}
