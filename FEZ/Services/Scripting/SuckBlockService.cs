// Type: FezGame.Services.Scripting.SuckBlockService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Services.Scripting;
using System;
using System.Collections.Generic;

namespace FezGame.Services.Scripting
{
  internal class SuckBlockService : ISuckBlockService, IScriptingBase
  {
    private readonly Dictionary<int, bool> SuckState = new Dictionary<int, bool>();

    public event Action<int> Sucked;

    public void ResetEvents()
    {
      this.Sucked = new Action<int>(Util.NullAction<int>);
      this.SuckState.Clear();
    }

    public void OnSuck(int id)
    {
      this.SuckState[id] = true;
      this.Sucked(id);
    }

    public bool get_IsSucked(int id)
    {
      bool flag;
      if (this.SuckState.TryGetValue(id, out flag))
        return flag;
      else
        return false;
    }
  }
}
