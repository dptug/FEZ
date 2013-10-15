// Type: FezGame.Components.OrreryHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class OrreryHost : GameComponent
  {
    private ArtObjectInstance Moon;
    private ArtObjectInstance Earth;
    private ArtObjectInstance Sun;
    private ArtObjectInstance PlanetW;
    private ArtObjectInstance MoonBranch;
    private ArtObjectInstance EarthBranch;
    private ArtObjectInstance SubBranch;
    private ArtObjectInstance SunBranch;
    private ArtObjectInstance PlanetWBranch;
    private ArtObjectInstance SmallGear1;
    private ArtObjectInstance SmallGear2;
    private ArtObjectInstance MediumGear;
    private ArtObjectInstance LargeGear1;
    private ArtObjectInstance LargeGear2;
    private float MoonBranchHeight;
    private float EarthBranchHeight;
    private float PlanetWBranchHeight;
    private float MoonDistance;
    private float EarthDistance;
    private float PlanetWDistance;

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public OrreryHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.Enabled = this.LevelManager.Name != null && this.LevelManager.Name == "ORRERY";
      this.Moon = this.Earth = this.Sun = this.PlanetW = (ArtObjectInstance) null;
      this.MoonBranch = this.EarthBranch = this.SubBranch = this.SunBranch = this.PlanetWBranch = (ArtObjectInstance) null;
      this.SmallGear1 = this.SmallGear2 = this.MediumGear = this.LargeGear1 = this.LargeGear2 = (ArtObjectInstance) null;
      if (!this.Enabled)
        return;
      this.Moon = Enumerable.Single<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_MOONAO"));
      this.Earth = Enumerable.Single<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_EARTHAO"));
      this.Sun = Enumerable.Single<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_SUNAO"));
      this.PlanetW = Enumerable.Single<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_PLANET_WAO"));
      this.MoonBranch = Enumerable.Single<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_MOON_BRANCHAO"));
      this.EarthBranch = Enumerable.Single<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_EARTH_BRANCHAO"));
      this.SubBranch = Enumerable.First<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_SUN_BRANCHAO"));
      this.SunBranch = Enumerable.First<ArtObjectInstance>(Enumerable.Skip<ArtObjectInstance>(Enumerable.Where<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_SUN_BRANCHAO")), 1));
      this.PlanetWBranch = Enumerable.Single<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_W_BRANCHAO"));
      this.SmallGear1 = Enumerable.First<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_COG_SAO"));
      this.SmallGear2 = Enumerable.First<ArtObjectInstance>(Enumerable.Skip<ArtObjectInstance>(Enumerable.Where<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_COG_SAO")), 1));
      this.MediumGear = Enumerable.First<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_COG_MAO"));
      this.LargeGear1 = Enumerable.First<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_COG_LAO"));
      this.LargeGear2 = Enumerable.First<ArtObjectInstance>(Enumerable.Skip<ArtObjectInstance>(Enumerable.Where<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "ORR_COG_LAO")), 1));
      this.MoonDistance = Vector3.Distance(this.Earth.Position, this.Moon.Position);
      this.EarthDistance = Vector3.Distance(this.Sun.Position, this.Earth.Position);
      this.PlanetWDistance = Vector3.Distance(this.Sun.Position, this.PlanetW.Position);
      this.MoonBranchHeight = this.MoonBranch.Position.Y;
      this.EarthBranchHeight = this.EarthBranch.Position.Y;
      this.PlanetWBranchHeight = this.PlanetWBranch.Position.Y;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || this.GameState.InMenuCube) || this.GameState.InFpsMode)
        return;
      float num = (float) gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
      this.Sun.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, num * 1f);
      this.Earth.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, num * 0.75f);
      this.Moon.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, num * 0.5f);
      this.PlanetW.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, num * 0.25f);
      this.Earth.Position = this.Sun.Position + Vector3.Transform(Vector3.UnitZ, this.Earth.Rotation) * this.EarthDistance;
      this.Moon.Position = this.Earth.Position + Vector3.Transform(Vector3.Right, this.Moon.Rotation) * this.MoonDistance;
      this.PlanetW.Position = this.Sun.Position + Vector3.Transform(Vector3.Left, this.PlanetW.Rotation) * this.PlanetWDistance;
      this.SmallGear1.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, num * 3f);
      this.SmallGear2.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, num * -3f);
      this.MediumGear.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, num * -1f);
      this.LargeGear1.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, num * 0.5f);
      this.LargeGear2.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, num * -0.5f);
      this.SunBranch.Rotation = this.Sun.Rotation;
      this.SubBranch.Rotation = this.Sun.Rotation;
      Vector3 scale;
      Quaternion rotation;
      Vector3 translation;
      (Matrix.CreateTranslation(1.875f, 0.0f, -1.875f) * Matrix.CreateFromQuaternion(this.Earth.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitY, -1.570796f)) * Matrix.CreateTranslation(this.Sun.Position * FezMath.XZMask + Vector3.UnitY * this.EarthBranchHeight)).Decompose(out scale, out rotation, out translation);
      this.EarthBranch.Position = translation;
      this.EarthBranch.Rotation = rotation;
      (Matrix.CreateTranslation(1.875f, 0.0f, 1.875f) * Matrix.CreateFromQuaternion(this.Moon.Rotation) * Matrix.CreateTranslation(this.Earth.Position * FezMath.XZMask + Vector3.UnitY * this.MoonBranchHeight)).Decompose(out scale, out rotation, out translation);
      this.MoonBranch.Position = translation;
      this.MoonBranch.Rotation = rotation;
      (Matrix.CreateTranslation(3.875f, 0.0f, -3.875f) * Matrix.CreateFromQuaternion(this.PlanetW.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitY, 3.141593f)) * Matrix.CreateTranslation(this.Sun.Position * FezMath.XZMask + Vector3.UnitY * this.PlanetWBranchHeight)).Decompose(out scale, out rotation, out translation);
      this.PlanetWBranch.Position = translation;
      this.PlanetWBranch.Rotation = rotation;
    }
  }
}
