// Type: FezEngine.Tools.IThreadPool
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Tools
{
  public interface IThreadPool
  {
    Worker<TContext> Take<TContext>(Action<TContext> task);

    Worker<TContext> TakeShared<TContext>(Action<TContext> task);

    void Return<TContext>(Worker<TContext> thread);
  }
}
