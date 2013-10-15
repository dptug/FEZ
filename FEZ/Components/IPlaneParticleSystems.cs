// Type: FezGame.Components.IPlaneParticleSystems
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  public interface IPlaneParticleSystems
  {
    PlaneParticleSystem RainSplash(Vector3 center);

    void Splash(IPhysicsEntity entity, bool outwards);

    void Splash(IPhysicsEntity entity, bool outwards, float velocityBonus);

    void Add(PlaneParticleSystem system);

    void Remove(PlaneParticleSystem system, bool returnToPool);

    void ForceDraw();
  }
}
