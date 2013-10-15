// Type: FezGame.Services.Scripting.TombstoneService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services.Scripting;
using System;

namespace FezGame.Services.Scripting
{
  public class TombstoneService : ITombstoneService, IScriptingBase
  {
    private int alignCount;

    public event Action MoreThanOneAligned;

    public void ResetEvents()
    {
      this.MoreThanOneAligned = (Action) null;
    }

    public void OnMoreThanOneAligned()
    {
      if (this.MoreThanOneAligned == null)
        return;
      this.MoreThanOneAligned();
    }

    public int get_AlignedCount()
    {
      return this.alignCount;
    }

    public void UpdateAlignCount(int count)
    {
      this.alignCount = count;
    }
  }
}
