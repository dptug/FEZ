// Type: FezGame.Components.Scripting.IScriptingManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using System;

namespace FezGame.Components.Scripting
{
  internal interface IScriptingManager
  {
    ActiveScript EvaluatedScript { get; }

    event Action CutsceneSkipped;

    void OnCutsceneSkipped();
  }
}
