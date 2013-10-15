// Type: FezGame.Services.GameLevelMaterializer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;

namespace FezGame.Services
{
  public class GameLevelMaterializer : LevelMaterializer
  {
    public GameLevelMaterializer(Game game)
      : base(game)
    {
    }

    public override void RebuildTrile(Trile trile)
    {
      TrileMaterializer trileMaterializer = new TrileMaterializer(trile, this.TrilesMesh, false);
      this.trileMaterializers.Add(trile, trileMaterializer);
      trileMaterializer.Geometry = trile.Geometry;
      trileMaterializer.DetermineFlags();
    }
  }
}
