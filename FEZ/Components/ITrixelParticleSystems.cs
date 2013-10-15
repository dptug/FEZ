// Type: FezGame.Components.ITrixelParticleSystems
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  public interface ITrixelParticleSystems
  {
    int Count { get; }

    void Add(TrixelParticleSystem system);

    void PropagateEnergy(Vector3 energySource, float energy);

    void UnGroundAll();

    void ForceDraw();
  }
}
