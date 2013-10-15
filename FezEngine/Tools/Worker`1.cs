// Type: FezEngine.Tools.Worker`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using System;
using System.Threading;

namespace FezEngine.Tools
{
  public class Worker<TContext> : IWorker
  {
    internal Action<TContext> task;
    private readonly PersistentThread thread;
    private TContext cachedContext;

    public bool Aborted { get; private set; }

    public ThreadPriority Priority
    {
      set
      {
        this.thread.Priority = value;
      }
    }

    internal PersistentThread UnderlyingThread
    {
      get
      {
        return this.thread;
      }
    }

    public event Action Finished = new Action(Util.NullAction);

    internal Worker(PersistentThread thread, Action<TContext> task)
    {
      this.task = task;
      this.thread = thread;
    }

    public void OnFinished()
    {
      this.Finished();
    }

    public void Act()
    {
      this.task(this.cachedContext);
    }

    public void Start(TContext context)
    {
      if (this.thread.Started)
        throw new InvalidOperationException("Thread is already started");
      if (this.thread.Disposed)
        throw new ObjectDisposedException("PersistentThread");
      this.cachedContext = context;
      this.thread.CurrentWorker = (IWorker) this;
      this.thread.Start();
    }

    public void Join()
    {
      if (!this.thread.Started)
        return;
      if (this.thread.Disposed)
        throw new ObjectDisposedException("PersistentThread");
      this.thread.Join();
    }

    internal void Dispose()
    {
      if (this.thread.Started)
        this.thread.Join();
      this.thread.Priority = ThreadPriority.Lowest;
      this.Finished = (Action) null;
      this.task = (Action<TContext>) null;
    }

    public void Abort()
    {
      this.Aborted = true;
    }
  }
}
