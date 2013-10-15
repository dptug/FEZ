// Type: FezEngine.Readers.ScriptReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Scripting;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class ScriptReader : ContentTypeReader<Script>
  {
    protected override Script Read(ContentReader input, Script existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new Script();
      existingInstance.Name = input.ReadString();
      existingInstance.Timeout = input.ReadObject<TimeSpan?>();
      existingInstance.Triggers = input.ReadObject<List<ScriptTrigger>>(existingInstance.Triggers);
      existingInstance.Conditions = input.ReadObject<List<ScriptCondition>>(existingInstance.Conditions);
      existingInstance.Actions = input.ReadObject<List<ScriptAction>>(existingInstance.Actions);
      existingInstance.OneTime = input.ReadBoolean();
      existingInstance.Triggerless = input.ReadBoolean();
      existingInstance.IgnoreEndTriggers = input.ReadBoolean();
      existingInstance.LevelWideOneTime = input.ReadBoolean();
      existingInstance.Disabled = input.ReadBoolean();
      existingInstance.IsWinCondition = input.ReadBoolean();
      return existingInstance;
    }
  }
}
