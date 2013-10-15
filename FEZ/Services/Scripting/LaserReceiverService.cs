// Type: FezGame.Services.Scripting.LaserReceiverService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Services.Scripting;
using System;

namespace FezGame.Services.Scripting
{
  internal class LaserReceiverService : ILaserReceiverService, IScriptingBase
  {
    public event Action<int> Activate = new Action<int>(Util.NullAction<int>);

    public void ResetEvents()
    {
      this.Activate = new Action<int>(Util.NullAction<int>);
    }

    public void OnActivated(int id)
    {
      this.Activate(id);
    }
  }
}
