// Type: FezEngine.Tools.PersistentThread
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using System;
using System.Threading;

namespace FezEngine.Tools
{
  internal class PersistentThread : IDisposable
  {
    private readonly Thread thread;
    private readonly ManualResetEvent startEvent;
    private readonly ManualResetEvent joinEvent;

    public bool Started { get; private set; }

    public bool Disposed { get; private set; }

    public IWorker CurrentWorker { private get; set; }

    public ThreadPriority Priority
    {
      set
      {
        if (PersistentThreadPool.SingleThreaded)
          return;
        this.thread.Priority = value;
      }
    }

    public PersistentThread()
    {
      if (PersistentThreadPool.SingleThreaded)
        return;
      this.startEvent = new ManualResetEvent(false);
      this.joinEvent = new ManualResetEvent(false);
      this.thread = new Thread(new ThreadStart(this.DoWork))
      {
        Priority = ThreadPriority.Lowest
      };
      this.thread.Start();
    }

    ~PersistentThread()
    {
      this.DisposeInternal();
    }

    public void Start()
    {
      this.Started = true;
      if (PersistentThreadPool.SingleThreaded)
      {
        this.CurrentWorker.Act();
        this.CurrentWorker.OnFinished();
      }
      else
        this.startEvent.Set();
    }

    public void Join()
    {
      if (!PersistentThreadPool.SingleThreaded && this.thread != Thread.CurrentThread)
      {
        this.joinEvent.WaitOne();
        this.joinEvent.Reset();
      }
      this.Started = false;
    }

    private void DoWork()
    {
      Logger.Try(new Action(this.DoActualWork));
    }

    private void DoActualWork()
    {
      if (PersistentThreadPool.SingleThreaded)
        return;
      this.startEvent.WaitOne();
      this.startEvent.Reset();
      while (!this.Disposed)
      {
        this.CurrentWorker.Act();
        this.CurrentWorker.OnFinished();
        this.joinEvent.Set();
        this.startEvent.WaitOne();
        this.startEvent.Reset();
      }
    }

    public void Dispose()
    {
      if (!this.Disposed)
        GC.SuppressFinalize((object) this);
      this.DisposeInternal();
    }

    private void DisposeInternal()
    {
      if (this.Disposed)
        return;
      this.Disposed = true;
      if (PersistentThreadPool.SingleThreaded)
        return;
      this.startEvent.Set();
    }
  }
}
