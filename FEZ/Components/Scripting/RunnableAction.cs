// Type: FezGame.Components.Scripting.RunnableAction
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure.Scripting;
using System;

namespace FezGame.Components.Scripting
{
  internal struct RunnableAction
  {
    public ScriptAction Action;
    public Func<object> Invocation;
  }
}
