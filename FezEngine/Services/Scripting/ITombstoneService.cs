// Type: FezEngine.Services.Scripting.ITombstoneService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Static = true)]
  public interface ITombstoneService : IScriptingBase
  {
    [Description("When more than one tombstones are aligned")]
    event Action MoreThanOneAligned;

    void OnMoreThanOneAligned();

    int get_AlignedCount();

    void UpdateAlignCount(int count);
  }
}
