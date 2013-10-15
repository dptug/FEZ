// Type: FezGame.Components.HeavyGroupState
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
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
  internal class HeavyGroupState
  {
    private readonly TrileGroup group;
    private readonly TrileInstance[] bottomTriles;
    private bool moving;
    private bool velocityNeedsReset;

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ICollisionManager CollisionManager { private get; set; }

    public HeavyGroupState(TrileGroup group)
    {
      ServiceHelper.InjectServices((object) this);
      this.group = group;
      int minY = Enumerable.Min<TrileInstance>((IEnumerable<TrileInstance>) group.Triles, (Func<TrileInstance, int>) (x => x.Emplacement.Y));
      group.Triles.Sort((Comparison<TrileInstance>) ((a, b) => a.Emplacement.Y - b.Emplacement.Y));
      this.bottomTriles = Enumerable.ToArray<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) group.Triles, (Func<TrileInstance, bool>) (x => x.Emplacement.Y == minY)));
      foreach (TrileInstance instance in group.Triles)
        instance.PhysicsState = new InstancePhysicsState(instance);
      this.MarkGrounds();
    }

    private void MarkGrounds()
    {
      foreach (TrileInstance trileInstance1 in this.group.Triles)
      {
        TrileInstance trileInstance2 = this.LevelManager.ActualInstanceAt(trileInstance1.Center - trileInstance1.Trile.Size.Y * Vector3.UnitY);
        trileInstance1.PhysicsState.Ground = new MultipleHits<TrileInstance>()
        {
          NearLow = trileInstance2
        };
        trileInstance1.IsMovingGroup = false;
      }
    }

    public void Update(TimeSpan elapsed)
    {
      if (!this.moving)
      {
        if (this.velocityNeedsReset)
        {
          foreach (TrileInstance trileInstance in this.group.Triles)
            trileInstance.PhysicsState.Velocity = Vector3.Zero;
          this.velocityNeedsReset = false;
        }
        bool flag = false;
        foreach (TrileInstance trileInstance in this.bottomTriles)
        {
          TrileInstance first = trileInstance.PhysicsState.Ground.First;
          flag = ((flag ? 1 : 0) | (first == null || !first.Enabled ? 0 : (first.PhysicsState == null ? 1 : (first.PhysicsState.Grounded ? 1 : 0)))) != 0;
        }
        if (!flag)
        {
          this.moving = true;
          foreach (TrileInstance trileInstance in this.group.Triles)
          {
            trileInstance.PhysicsState.Ground = new MultipleHits<TrileInstance>();
            trileInstance.IsMovingGroup = true;
          }
        }
      }
      if (!this.moving)
        return;
      Vector3 vector3_1 = 0.4725f * (float) elapsed.TotalSeconds * -Vector3.UnitY;
      foreach (TrileInstance trileInstance in this.group.Triles)
        trileInstance.PhysicsState.UpdatingPhysics = true;
      bool flag1 = false;
      Vector3 vector3_2 = Vector3.Zero;
      foreach (TrileInstance trileInstance in this.bottomTriles)
      {
        MultipleHits<CollisionResult> multipleHits = this.CollisionManager.CollideEdge(trileInstance.Center, trileInstance.PhysicsState.Velocity + vector3_1, trileInstance.TransformedSize / 2f, Direction2D.Vertical);
        if (multipleHits.First.Collided)
        {
          flag1 = true;
          vector3_2 = Vector3.Max(vector3_2, multipleHits.First.Response);
        }
      }
      Vector3 vector3_3 = vector3_1 + vector3_2;
      foreach (TrileInstance instance in this.group.Triles)
      {
        instance.Position += instance.PhysicsState.Velocity += vector3_3;
        this.LevelManager.UpdateInstance(instance);
        instance.PhysicsState.UpdatingPhysics = false;
      }
      if (!flag1)
        return;
      this.MarkGrounds();
      this.moving = false;
      this.velocityNeedsReset = true;
    }
  }
}
