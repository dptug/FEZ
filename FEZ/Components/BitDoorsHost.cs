// Type: FezGame.Components.BitDoorsHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class BitDoorsHost : GameComponent
  {
    private readonly List<BitDoorState> BitDoors = new List<BitDoorState>();
    private readonly List<int> ToReactivate = new List<int>();

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IBitDoorService BitDoorService { private get; set; }

    public BitDoorsHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanging += new Action(this.InitBitDoors);
      if (this.LevelManager.Name == null)
        return;
      this.InitBitDoors();
    }

    private void InitBitDoors()
    {
      this.ToReactivate.Clear();
      this.BitDoors.Clear();
      foreach (ArtObjectInstance artObject in Enumerable.Where<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => ActorTypeExtensions.IsBitDoor(x.ArtObject.ActorType))))
        this.BitDoors.Add(new BitDoorState(artObject));
      foreach (int key in this.GameState.SaveData.ThisLevel.InactiveArtObjects)
      {
        ArtObjectInstance door;
        if (key >= 0 && this.LevelManager.ArtObjects.TryGetValue(key, out door) && ActorTypeExtensions.IsBitDoor(door.ArtObject.ActorType))
        {
          door.Position += Enumerable.First<BitDoorState>((IEnumerable<BitDoorState>) this.BitDoors, (Func<BitDoorState, bool>) (x => x.AoInstance == door)).GetOpenOffset();
          door.ActorSettings.Inactive = true;
          this.ToReactivate.Add(key);
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || (this.GameState.Loading || !this.CameraManager.ActionRunning) || (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.BitDoors.Count == 0))
        return;
      if (this.ToReactivate.Count > 0)
      {
        foreach (int id in this.ToReactivate)
          this.BitDoorService.OnOpen(id);
        this.ToReactivate.Clear();
      }
      foreach (BitDoorState bitDoorState in this.BitDoors)
        bitDoorState.Update(gameTime.ElapsedGameTime);
    }
  }
}
