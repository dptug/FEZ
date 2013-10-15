// Type: FezGame.Components.Scripting.ScriptingHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure.Scripting;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components.Scripting
{
  internal class ScriptingHost : GameComponent, IScriptingManager
  {
    private Script[] levelScripts = new Script[0];
    private readonly Dictionary<string, IScriptingBase> services = new Dictionary<string, IScriptingBase>();
    private readonly List<ActiveScript> activeScripts = new List<ActiveScript>();
    private string LastLevel;
    public static ScriptingHost Instance;

    public ActiveScript EvaluatedScript { get; private set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IScriptService ScriptService { private get; set; }

    public event Action CutsceneSkipped;

    public ScriptingHost(Game game)
      : base(game)
    {
      ScriptingHost.Instance = this;
    }

    public void OnCutsceneSkipped()
    {
      if (this.CutsceneSkipped == null)
        return;
      this.CutsceneSkipped();
    }

    public override void Initialize()
    {
      base.Initialize();
      foreach (KeyValuePair<string, EntityTypeDescriptor> keyValuePair in (IEnumerable<KeyValuePair<string, EntityTypeDescriptor>>) EntityTypes.Types)
        this.services.Add(keyValuePair.Key, ServiceHelper.Get(keyValuePair.Value.Interface) as IScriptingBase);
      this.LevelManager.LevelChanged += new Action(this.PrepareScripts);
      this.PrepareScripts();
    }

    private void PrepareScripts()
    {
      this.levelScripts = Enumerable.ToArray<Script>((IEnumerable<Script>) this.LevelManager.Scripts.Values);
      foreach (ActiveScript activeScript in this.activeScripts)
      {
        activeScript.Dispose();
        if (activeScript.Script.OneTime)
        {
          activeScript.Script.Disabled = true;
          LevelSaveData levelSaveData;
          if (!activeScript.Script.LevelWideOneTime && this.GameState.SaveData.World.TryGetValue(this.LastLevel, out levelSaveData))
            levelSaveData.InactiveEvents.Add(activeScript.Script.Id);
        }
      }
      this.activeScripts.Clear();
      foreach (IScriptingBase scriptingBase in this.services.Values)
        scriptingBase.ResetEvents();
      if (this.LevelManager.Name != null)
      {
        foreach (int key in this.GameState.SaveData.ThisLevel.InactiveEvents)
        {
          Script script;
          if (this.LevelManager.Scripts.TryGetValue(key, out script))
            script.Disabled = true;
        }
      }
      this.LastLevel = this.LevelManager.Name;
      foreach (Script script in this.levelScripts)
        this.HookScriptTriggers(script);
    }

    private void HookScriptTriggers(Script script)
    {
      foreach (ScriptTrigger scriptTrigger in script.Triggers)
      {
        ScriptTrigger triggerCopy = scriptTrigger;
        EntityTypeDescriptor entityTypeDescriptor = EntityTypes.Types[scriptTrigger.Object.Type];
        EventDescriptor eventDescriptor = entityTypeDescriptor.Events[scriptTrigger.Event];
        if (entityTypeDescriptor.Static)
        {
          Action action = (Action) (() => this.ProcessTrigger(triggerCopy, script));
          object obj = eventDescriptor.AddHandler((object) this.services[scriptTrigger.Object.Type], new object[1]
          {
            (object) action
          });
        }
        else
        {
          Action<int> action = (Action<int>) (id => this.ProcessTrigger(triggerCopy, script, new int?(id)));
          object obj = eventDescriptor.AddHandler((object) this.services[scriptTrigger.Object.Type], new object[1]
          {
            (object) action
          });
        }
      }
    }

    private void ProcessTrigger(ScriptTrigger trigger, Script script)
    {
      this.ProcessTrigger(trigger, script, new int?());
    }

    private void ProcessTrigger(ScriptTrigger trigger, Script script, int? id)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ScriptingHost.\u003C\u003Ec__DisplayClass11 cDisplayClass11 = new ScriptingHost.\u003C\u003Ec__DisplayClass11();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass11.trigger = trigger;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass11.script = script;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass11.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.GameState.Loading && cDisplayClass11.trigger.Object.Type != "Level" && cDisplayClass11.trigger.Event != "Start" || cDisplayClass11.script.Disabled)
        return;
      int? nullable = id;
      // ISSUE: reference to a compiler-generated field
      int? identifier = cDisplayClass11.trigger.Object.Identifier;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if ((nullable.GetValueOrDefault() != identifier.GetValueOrDefault() ? 1 : (nullable.HasValue != identifier.HasValue ? 1 : 0)) != 0 || cDisplayClass11.script.Conditions != null && Enumerable.Any<ScriptCondition>((IEnumerable<ScriptCondition>) cDisplayClass11.script.Conditions, (Func<ScriptCondition, bool>) (c => !c.Check(this.services[c.Object.Type]))) || cDisplayClass11.script.OneTime && Enumerable.Any<ActiveScript>((IEnumerable<ActiveScript>) this.activeScripts, new Func<ActiveScript, bool>(cDisplayClass11.\u003CProcessTrigger\u003Eb__a)))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass11.activeScript = new ActiveScript(cDisplayClass11.script, cDisplayClass11.trigger);
      // ISSUE: reference to a compiler-generated field
      this.activeScripts.Add(cDisplayClass11.activeScript);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass11.script.IsWinCondition && !this.GameState.SaveData.ThisLevel.FilledConditions.ScriptIds.Contains(cDisplayClass11.script.Id))
      {
        // ISSUE: reference to a compiler-generated field
        this.GameState.SaveData.ThisLevel.FilledConditions.ScriptIds.Add(cDisplayClass11.script.Id);
        this.GameState.SaveData.ThisLevel.FilledConditions.ScriptIds.Sort();
      }
      // ISSUE: reference to a compiler-generated field
      foreach (ScriptAction scriptAction in cDisplayClass11.script.Actions)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ScriptingHost.\u003C\u003Ec__DisplayClass13 cDisplayClass13 = new ScriptingHost.\u003C\u003Ec__DisplayClass13();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass13.CS\u0024\u003C\u003E8__locals12 = cDisplayClass11;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass13.runnableAction = new RunnableAction()
        {
          Action = scriptAction
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        cDisplayClass13.runnableAction.Invocation = new Func<object>(cDisplayClass13.\u003CProcessTrigger\u003Eb__b);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cDisplayClass11.activeScript.EnqueueAction(cDisplayClass13.runnableAction);
      }
      // ISSUE: reference to a compiler-generated field
      if (!cDisplayClass11.script.IgnoreEndTriggers)
      {
        // ISSUE: reference to a compiler-generated field
        foreach (ScriptTrigger scriptTrigger in cDisplayClass11.script.Triggers)
        {
          EntityTypeDescriptor entityTypeDescriptor = EntityTypes.Types[scriptTrigger.Object.Type];
          DynamicMethodDelegate dynamicMethodDelegate = entityTypeDescriptor.Events[scriptTrigger.Event].AddEndTriggerHandler;
          if (dynamicMethodDelegate != null)
          {
            if (entityTypeDescriptor.Static)
            {
              // ISSUE: reference to a compiler-generated method
              Action action = new Action(cDisplayClass11.\u003CProcessTrigger\u003Eb__c);
              // ISSUE: reference to a compiler-generated field
              object obj = dynamicMethodDelegate((object) this.services[cDisplayClass11.trigger.Object.Type], new object[1]
              {
                (object) action
              });
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              Action<int> action = new Action<int>(cDisplayClass11.\u003CProcessTrigger\u003Eb__d);
              // ISSUE: reference to a compiler-generated field
              object obj = dynamicMethodDelegate((object) this.services[cDisplayClass11.trigger.Object.Type], new object[1]
              {
                (object) action
              });
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      cDisplayClass11.activeScript.Disposed += new Action(cDisplayClass11.\u003CProcessTrigger\u003Eb__e);
    }

    private static void ProcessEndTrigger(ScriptTrigger trigger, ActiveScript script)
    {
      ScriptingHost.ProcessEndTrigger(trigger, script, new int?());
    }

    private static void ProcessEndTrigger(ScriptTrigger trigger, ActiveScript script, int? id)
    {
      int? nullable = id;
      int? identifier = trigger.Object.Identifier;
      if ((nullable.GetValueOrDefault() != identifier.GetValueOrDefault() ? 0 : (nullable.HasValue == identifier.HasValue ? 1 : 0)) == 0)
        return;
      script.Dispose();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.Loading || (this.GameState.InCutscene || this.GameState.InFpsMode))
        return;
      this.ForceUpdate(gameTime);
    }

    public void ForceUpdate(GameTime gameTime)
    {
      foreach (Script script in this.levelScripts)
      {
        if (script.ScheduleEvalulation)
        {
          this.ProcessTrigger((ScriptTrigger) ScriptingHost.NullTrigger.Instance, script);
          script.ScheduleEvalulation = false;
        }
      }
      for (int index = this.activeScripts.Count - 1; index != -1; --index)
      {
        ActiveScript activeScript = this.activeScripts[index];
        this.EvaluatedScript = activeScript;
        activeScript.Update(gameTime.ElapsedGameTime);
        if (activeScript.IsDisposed && this.activeScripts.Count > 0 && this.activeScripts[index] == activeScript)
        {
          this.activeScripts.RemoveAt(index);
          if (activeScript.Script.OneTime)
          {
            activeScript.Script.Disabled = true;
            if (!activeScript.Script.LevelWideOneTime)
              this.GameState.SaveData.ThisLevel.InactiveEvents.Add(activeScript.Script.Id);
            this.GameState.Save();
          }
        }
      }
      this.EvaluatedScript = (ActiveScript) null;
    }

    private class NullTrigger : ScriptTrigger
    {
      public static readonly ScriptingHost.NullTrigger Instance = new ScriptingHost.NullTrigger();
      private const string NullEvent = "Null Event";

      static NullTrigger()
      {
      }

      private NullTrigger()
      {
        this.Event = "Null Event";
        this.Object = new Entity()
        {
          Type = (string) null,
          Identifier = new int?()
        };
      }
    }
  }
}
