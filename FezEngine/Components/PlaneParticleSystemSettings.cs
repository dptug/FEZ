// Type: FezEngine.Components.PlaneParticleSystemSettings
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Components
{
  public sealed class PlaneParticleSystemSettings
  {
    public float SpawningSpeed { get; set; }

    public bool RandomizeSpawnTime { get; set; }

    public int SpawnBatchSize { get; set; }

    public bool NoLightDraw { get; set; }

    public BoundingBox SpawnVolume { get; set; }

    public VaryingVector3 SizeBirth { get; set; }

    public VaryingVector3 SizeDeath { get; set; }

    public Vector3 Gravity { get; set; }

    public Vector3? EnergySource { get; set; }

    public VaryingVector3 Velocity { get; set; }

    public float Acceleration { get; set; }

    public float SystemLifetime { get; set; }

    public float ParticleLifetime { get; set; }

    public VaryingColor ColorBirth { get; set; }

    public VaryingColor ColorLife { get; set; }

    public VaryingColor ColorDeath { get; set; }

    public float FadeInDuration { get; set; }

    public float FadeOutDuration { get; set; }

    public Texture2D Texture { get; set; }

    public BlendingMode BlendingMode { get; set; }

    public bool FullBright { get; set; }

    public bool Billboarding { get; set; }

    public bool Doublesided { get; set; }

    public FaceOrientation? Orientation { get; set; }

    public bool UseCallback { get; set; }

    public bool ClampToTrixels { get; set; }

    public StencilMask? StencilMask { get; set; }

    public PlaneParticleSystemSettings()
    {
      this.SizeBirth = (VaryingVector3) new Vector3(1.0 / 16.0);
      this.SpawningSpeed = 1f;
      this.ParticleLifetime = 1f;
      this.BlendingMode = BlendingMode.Alphablending;
      this.FadeInDuration = 0.1f;
      this.FadeOutDuration = 0.9f;
      this.SpawnBatchSize = 1;
      this.ColorLife = (VaryingColor) Color.White;
      this.NoLightDraw = false;
      this.SizeDeath = (VaryingVector3) new Vector3(-1f);
      this.Velocity = new VaryingVector3();
    }

    public PlaneParticleSystemSettings Clone()
    {
      return new PlaneParticleSystemSettings()
      {
        Acceleration = this.Acceleration,
        Billboarding = this.Billboarding,
        BlendingMode = this.BlendingMode,
        ColorBirth = this.ColorBirth,
        ColorLife = this.ColorLife,
        ColorDeath = this.ColorDeath,
        Doublesided = this.Doublesided,
        EnergySource = this.EnergySource,
        FadeInDuration = this.FadeInDuration,
        FadeOutDuration = this.FadeOutDuration,
        FullBright = this.FullBright,
        Gravity = this.Gravity,
        ParticleLifetime = this.ParticleLifetime,
        RandomizeSpawnTime = this.RandomizeSpawnTime,
        SizeBirth = this.SizeBirth,
        SizeDeath = this.SizeDeath,
        SpawnBatchSize = this.SpawnBatchSize,
        SpawningSpeed = this.SpawningSpeed,
        SpawnVolume = this.SpawnVolume,
        SystemLifetime = this.SystemLifetime,
        Texture = this.Texture,
        Velocity = this.Velocity,
        UseCallback = this.UseCallback,
        ClampToTrixels = this.ClampToTrixels,
        Orientation = this.Orientation,
        StencilMask = this.StencilMask
      };
    }
  }
}
