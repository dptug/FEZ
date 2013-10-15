// Type: FezGame.Components.MovingGroundsPickupComparer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  internal class MovingGroundsPickupComparer : Comparer<PickupState>
  {
    public static readonly MovingGroundsPickupComparer Default = new MovingGroundsPickupComparer();

    static MovingGroundsPickupComparer()
    {
    }

    public override int Compare(PickupState x, PickupState y)
    {
      int num1 = 0;
      int num2 = 0;
      TrileInstance trileInstance1 = (TrileInstance) null;
      TrileInstance trileInstance2 = (TrileInstance) null;
      for (InstancePhysicsState physicsState = x.Instance.PhysicsState; physicsState.Grounded && physicsState.Ground.First.PhysicsState != null && (physicsState.Ground.First != x.Instance && physicsState.Ground.First != trileInstance2); physicsState = trileInstance1.PhysicsState)
      {
        trileInstance2 = trileInstance1;
        ++num1;
        trileInstance1 = physicsState.Ground.First;
      }
      InstancePhysicsState physicsState1 = y.Instance.PhysicsState;
      TrileInstance trileInstance3;
      for (TrileInstance trileInstance4 = trileInstance3 = (TrileInstance) null; physicsState1.Grounded && physicsState1.Ground.First.PhysicsState != null && (physicsState1.Ground.First != y.Instance && physicsState1.Ground.First != trileInstance3); physicsState1 = trileInstance4.PhysicsState)
      {
        trileInstance3 = trileInstance4;
        ++num2;
        trileInstance4 = physicsState1.Ground.First;
      }
      if (num1 - num2 != 0)
        return num1 - num2;
      Vector3 b = FezMath.Sign(x.Instance.PhysicsState.Velocity) * FezMath.XZMask;
      if (b == FezMath.Sign(y.Instance.PhysicsState.Velocity) * FezMath.XZMask)
        return Math.Sign(FezMath.Dot(x.Instance.Position - y.Instance.Position, b));
      else
        return Math.Sign(x.Instance.Position.Y - y.Instance.Position.Y);
    }
  }
}
