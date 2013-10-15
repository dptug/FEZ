// Type: FezGame.Tools.MovementHelper
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Tools
{
  public class MovementHelper
  {
    private const float RunInputThreshold = 0.5f;

    public float WalkAcceleration { get; set; }

    public float RunAcceleration { get; set; }

    public float RunTimeThreshold { get; set; }

    public float RunTime { get; private set; }

    public IPhysicsEntity Entity { private get; set; }

    public bool Running
    {
      get
      {
        return (double) this.RunTime > (double) this.RunTimeThreshold;
      }
    }

    [ServiceDependency]
    public ICollisionManager CollisionManager { protected get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { protected get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { protected get; set; }

    [ServiceDependency]
    public IInputManager InputProvider { protected get; set; }

    public MovementHelper(float walkAcceleration, float runAcceleration, float runTimeThreshold)
    {
      this.WalkAcceleration = walkAcceleration;
      this.RunAcceleration = runAcceleration;
      this.RunTimeThreshold = runTimeThreshold;
      ServiceHelper.InjectServices((object) this);
    }

    public void Update(float elapsedSeconds)
    {
      this.Update(elapsedSeconds, this.InputProvider.Movement.X);
    }

    public void Update(float elapsedSeconds, float input)
    {
      if ((double) Math.Abs(input) > 0.5)
        this.RunTime += elapsedSeconds;
      else
        this.RunTime = 0.0f;
      this.Entity.Velocity += Vector3.Transform(new Vector3(input, 0.0f, 0.0f), this.CameraManager.Rotation) * 0.15f * (this.Running ? this.RunAcceleration : this.WalkAcceleration) * elapsedSeconds * (float) (0.5 + (double) Math.Abs(this.CollisionManager.GravityFactor) * 1.5) / 2f;
    }

    public void Reset()
    {
      this.RunTime = 0.0f;
    }
  }
}
