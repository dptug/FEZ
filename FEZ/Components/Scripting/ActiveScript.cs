// Type: FezGame.Components.Scripting.ActiveScript
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Components;
using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Structure.Scripting;
using FezEngine.Tools;
using System;
using System.Collections.Generic;

namespace FezGame.Components.Scripting
{
  internal class ActiveScript : IDisposable
  {
    private readonly List<LongRunningAction> runningActions = new List<LongRunningAction>();
    private readonly Queue<RunnableAction> queuedActions = new Queue<RunnableAction>();
    public readonly ScriptTrigger InitiatingTrigger;
    public readonly Script Script;
    private TimeSpan RunningTime;

    public bool IsDisposed { get; private set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IScriptingManager Scripting { private get; set; }

    public event Action Disposed = new Action(Util.NullAction);

    public ActiveScript(Script script, ScriptTrigger initiatingTrigger)
    {
      ServiceHelper.InjectServices((object) this);
      this.Script = script;
      this.InitiatingTrigger = initiatingTrigger;
    }

    public void EnqueueAction(RunnableAction runnableAction)
    {
      this.queuedActions.Enqueue(runnableAction);
    }

    public void Update(TimeSpan elapsed)
    {
      if (this.IsDisposed)
        return;
      this.RunningTime += elapsed;
      while (this.queuedActions.Count > 0 && (this.runningActions.Count == 0 || !this.queuedActions.Peek().Action.Blocking))
        this.StartAction(this.queuedActions.Dequeue());
      if (this.runningActions.Count != 0)
      {
        if (!this.Script.Timeout.HasValue)
          return;
        TimeSpan timeSpan = this.RunningTime;
        TimeSpan? timeout = this.Script.Timeout;
        if ((timeout.HasValue ? (timeSpan > timeout.GetValueOrDefault() ? 1 : 0) : 0) == 0)
          return;
      }
      this.Dispose();
    }

    private void StartAction(RunnableAction runnableAction)
    {
      LongRunningAction runningAction = runnableAction.Invocation() as LongRunningAction;
      runnableAction.Invocation = (Func<object>) null;
      if (runningAction == null)
        return;
      runningAction.Ended += (Action) (() =>
      {
        this.runningActions.Remove(runningAction);
        if (!runnableAction.Action.Killswitch)
          return;
        this.Dispose();
      });
      this.runningActions.Add(runningAction);
    }

    public void Dispose()
    {
      foreach (LongRunningAction component in this.runningActions.ToArray())
        ServiceHelper.RemoveComponent<LongRunningAction>(component);
      this.IsDisposed = true;
      this.Disposed();
    }
  }
}
