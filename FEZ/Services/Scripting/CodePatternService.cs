// Type: FezGame.Services.Scripting.CodePatternService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Services.Scripting;
using System;

namespace FezGame.Services.Scripting
{
  internal class CodePatternService : ICodePatternService, IScriptingBase
  {
    public event Action<int> Activated;

    public void ResetEvents()
    {
      this.Activated = new Action<int>(Util.NullAction<int>);
    }

    public void OnActivate(int id)
    {
      this.Activated(id);
    }
  }
}
