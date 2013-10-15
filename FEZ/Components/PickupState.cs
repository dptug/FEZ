// Type: FezGame.Components.PickupState
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  internal class PickupState
  {
    public readonly TrileInstance Instance;
    public readonly Vector3 OriginalCenter;
    public TrileGroup Group;
    public Vector3 LastGroundedCenter;
    public Vector3 LastVelocity;
    public float FlightApex;
    public bool TouchesWater;
    public float FloatSeed;
    public float FloatMalus;
    public Vector3 LastMovement;
    public PickupState VisibleOverlapper;
    public ArtObjectInstance[] AttachedAOs;

    public PickupState(TrileInstance ti, TrileGroup group)
    {
      this.Instance = ti;
      this.OriginalCenter = ti.Center;
      this.Group = group;
    }
  }
}
