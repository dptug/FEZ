// Type: FezGame.Tools.BackgroundTrileMaterializer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;

namespace FezGame.Tools
{
  internal class BackgroundTrileMaterializer : TrileMaterializer
  {
    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    public BackgroundTrileMaterializer(Trile trile, Mesh levelMesh)
      : base(trile, levelMesh)
    {
    }
  }
}
