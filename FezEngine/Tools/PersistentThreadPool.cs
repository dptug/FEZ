// Type: FezEngine.Tools.PersistentThreadPool
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public class PersistentThreadPool : GameComponent, IThreadPool
  {
    private const int InitialMaxThreads = 1;
    private readonly Stack<PersistentThread> stack;
    private bool disposed;
    public static bool SingleThreaded;

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    static PersistentThreadPool()
    {
    }

    public PersistentThreadPool(Game game)
      : base(game)
    {
      this.stack = new Stack<PersistentThread>(1);
      for (int index = 0; index < 1; ++index)
        this.stack.Push(this.CreateThread());
    }

    private PersistentThread CreateThread()
    {
      return new PersistentThread();
    }

    public Worker<TContext> Take<TContext>(Action<TContext> task)
    {
      return new Worker<TContext>(this.stack.Count > 0 ? this.stack.Pop() : this.CreateThread(), task);
    }

    public Worker<TContext> TakeShared<TContext>(Action<TContext> task)
    {
      return new Worker<TContext>(this.stack.Count > 0 ? this.stack.Peek() : this.CreateThread(), task);
    }

    public void Return<TContext>(Worker<TContext> worker)
    {
      if (worker == null)
        return;
      worker.Dispose();
      if (this.disposed)
        worker.UnderlyingThread.Dispose();
      else
        this.stack.Push(worker.UnderlyingThread);
    }

    protected override void Dispose(bool disposing)
    {
      while (this.stack.Count > 0)
        this.stack.Pop().Dispose();
      this.disposed = true;
      base.Dispose(disposing);
    }
  }
}
