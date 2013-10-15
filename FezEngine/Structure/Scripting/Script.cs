// Type: FezEngine.Structure.Scripting.Script
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure.Scripting
{
  public class Script
  {
    internal const string MemberSeparator = ".";

    [Serialization(Ignore = true)]
    public int Id { get; set; }

    public string Name { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool OneTime { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool LevelWideOneTime { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Disabled { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Triggerless { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool IgnoreEndTriggers { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool IsWinCondition { get; set; }

    [Serialization(Optional = true)]
    public TimeSpan? Timeout { get; set; }

    [Serialization(CollectionItemName = "Trigger")]
    public List<ScriptTrigger> Triggers { get; set; }

    [Serialization(CollectionItemName = "Action")]
    public List<ScriptAction> Actions { get; set; }

    [Serialization(CollectionItemName = "Condition", Optional = true)]
    public List<ScriptCondition> Conditions { get; set; }

    [Serialization(Ignore = true)]
    public bool ScheduleEvalulation { get; set; }

    public Script()
    {
      this.Name = "Untitled";
      this.Triggers = new List<ScriptTrigger>();
      this.Actions = new List<ScriptAction>();
    }

    public Script Clone()
    {
      List<ScriptTrigger> list1 = Enumerable.ToList<ScriptTrigger>(Enumerable.Select<ScriptTrigger, ScriptTrigger>((IEnumerable<ScriptTrigger>) this.Triggers, (Func<ScriptTrigger, ScriptTrigger>) (t => t.Clone())));
      List<ScriptAction> list2 = Enumerable.ToList<ScriptAction>(Enumerable.Select<ScriptAction, ScriptAction>((IEnumerable<ScriptAction>) this.Actions, (Func<ScriptAction, ScriptAction>) (a => a.Clone())));
      List<ScriptCondition> list3 = this.Conditions == null ? (List<ScriptCondition>) null : Enumerable.ToList<ScriptCondition>(Enumerable.Select<ScriptCondition, ScriptCondition>((IEnumerable<ScriptCondition>) this.Conditions, (Func<ScriptCondition, ScriptCondition>) (c => c.Clone())));
      return new Script()
      {
        Id = -1,
        Name = this.Name,
        Triggers = list1,
        Actions = list2,
        Conditions = list3,
        OneTime = this.OneTime,
        LevelWideOneTime = this.LevelWideOneTime,
        Disabled = this.Disabled,
        Triggerless = this.Triggerless,
        IgnoreEndTriggers = this.IgnoreEndTriggers,
        Timeout = this.Timeout,
        ScheduleEvalulation = this.ScheduleEvalulation
      };
    }
  }
}
