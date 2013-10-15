// Type: FezGame.Services.Scripting.TimeswitchService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Services.Scripting;
using System;

namespace FezGame.Services.Scripting
{
  public class TimeswitchService : ITimeswitchService, IScriptingBase
  {
    public event Action<int> ScrewedOut = new Action<int>(Util.NullAction<int>);

    public event Action<int> HitBase = new Action<int>(Util.NullAction<int>);

    public void ResetEvents()
    {
      this.ScrewedOut = new Action<int>(Util.NullAction<int>);
      this.HitBase = new Action<int>(Util.NullAction<int>);
    }

    public void OnScrewedOut(int id)
    {
      this.ScrewedOut(id);
    }

    public void OnHitBase(int id)
    {
      this.HitBase(id);
    }
  }
}
