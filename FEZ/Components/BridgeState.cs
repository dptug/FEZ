// Type: FezGame.Components.BridgeState
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  internal class BridgeState
  {
    public readonly TrileInstance Instance;
    public Vector3 OriginalPosition;
    public float Downforce;
    public bool Dirty;

    public BridgeState(TrileInstance instance)
    {
      this.Instance = instance;
      this.OriginalPosition = instance.Position;
      if (instance.PhysicsState != null)
        return;
      instance.PhysicsState = new InstancePhysicsState(instance)
      {
        Sticky = true
      };
    }
  }
}
