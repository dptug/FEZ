// Type: Microsoft.Xna.Framework.Threading
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
