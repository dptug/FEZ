// Type: FezGame.Services.Scripting.BitDoorService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Services.Scripting;
using System;

namespace FezGame.Services.Scripting
{
  internal class BitDoorService : IBitDoorService, IScriptingBase
  {
    public event Action<int> Open;

    public void OnOpen(int id)
    {
      this.Open(id);
    }

    public void ResetEvents()
    {
      this.Open = new Action<int>(Util.NullAction<int>);
    }
  }
}
