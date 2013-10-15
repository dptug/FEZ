// Type: FezEngine.Components.CameraPathsHost
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace FezEngine.Components
{
  public class CameraPathsHost : GameComponent
  {
    private readonly List<CameraPathsHost.CameraPathState> trackedPaths = new List<CameraPathsHost.CameraPathState>();

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    public CameraPathsHost(Game game)
      : base(game)
    {
      this.UpdateOrder = -2;
    }

    private void TrackNewPaths()
    {
      this.trackedPaths.Clear();
      foreach (MovementPath path in (IEnumerable<MovementPath>) this.LevelManager.Paths.Values)
        this.trackedPaths.Add(new CameraPathsHost.CameraPathState(path));
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TrackNewPaths);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EngineState.Loading || this.EngineState.TimePaused)
        return;
      if (this.trackedPaths.Count != this.LevelManager.Paths.Count)
        this.TrackNewPaths();
      if (this.trackedPaths.Count == 0)
        return;
      foreach (CameraPathsHost.CameraPathState cameraPathState in this.trackedPaths)
        cameraPathState.Update(gameTime.ElapsedGameTime);
    }

    private class CameraPathState
    {
      private readonly MovementPath Path;
      private readonly List<PathSegment> Nodes;
      private TimeSpan sinceSegmentStarted;
      private int nodeIndex;
      private Viewpoint originalViewpoint;
      private Viewpoint firstNodeViewpoint;
      private float originalPixelsPerTrixel;
      private Vector3 originalCenter;
      private Vector3 originalDirection;
      private float originalRadius;
      private bool justStarted;

      private bool Enabled { get; set; }

      private PathSegment CurrentNode
      {
        get
        {
          return this.Nodes[this.nodeIndex];
        }
      }

      [ServiceDependency]
      public IDefaultCameraManager CameraManager { private get; set; }

      [ServiceDependency]
      public IDebuggingBag DebuggingBag { private get; set; }

      [ServiceDependency]
      public IContentManagerProvider CMProvider { private get; set; }

      public CameraPathState(MovementPath path)
      {
        ServiceHelper.InjectServices((object) this);
        this.Path = path;
        this.Nodes = path.Segments;
        this.Enabled = true;
        foreach (PathSegment pathSegment in this.Nodes)
        {
          CameraNodeData cameraNodeData = pathSegment.CustomData as CameraNodeData;
          if (cameraNodeData.SoundName != null)
            pathSegment.Sound = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/" + cameraNodeData.SoundName);
        }
        this.Reset();
        this.StartNewSegment();
      }

      private void Reset()
      {
        this.nodeIndex = 1;
        this.sinceSegmentStarted = TimeSpan.Zero;
        this.justStarted = true;
      }

      public void Update(TimeSpan elapsed)
      {
        if (!this.Enabled || this.Path.NeedsTrigger)
          return;
        if (this.justStarted)
        {
          this.originalViewpoint = this.CameraManager.Viewpoint;
          this.originalCenter = this.CameraManager.Center;
          this.originalDirection = this.CameraManager.Direction;
          this.originalPixelsPerTrixel = this.CameraManager.PixelsPerTrixel;
          this.originalRadius = this.CameraManager.Radius;
          bool perspective = (this.Nodes[0].CustomData as CameraNodeData).Perspective;
          if (this.Path.InTransition)
          {
            this.nodeIndex = 1;
            this.Nodes.Insert(0, new PathSegment()
            {
              Destination = this.originalCenter,
              Orientation = Quaternion.Inverse(this.CameraManager.Rotation),
              CustomData = (ICloneable) new CameraNodeData()
              {
                PixelsPerTrixel = (int) this.originalPixelsPerTrixel,
                Perspective = perspective
              }
            });
          }
          if (this.Path.OutTransition)
            this.Nodes.Add(new PathSegment()
            {
              Destination = this.originalCenter,
              Orientation = Quaternion.Inverse(this.CameraManager.Rotation),
              CustomData = (ICloneable) new CameraNodeData()
              {
                PixelsPerTrixel = (int) this.originalPixelsPerTrixel,
                Perspective = perspective
              }
            });
          if (this.Nodes.Count < 2)
          {
            this.EndPath();
            return;
          }
          else
          {
            CameraNodeData cameraNodeData = this.Nodes[0].CustomData as CameraNodeData;
            this.firstNodeViewpoint = FezMath.AsViewpoint(FezMath.OrientationFromDirection(FezMath.MaxClampXZ(Vector3.Transform(Vector3.Forward, this.Nodes[0].Orientation))));
            this.CameraManager.ChangeViewpoint(cameraNodeData.Perspective ? Viewpoint.Perspective : this.firstNodeViewpoint);
            if (cameraNodeData.Perspective)
              this.CameraManager.Radius = 1.0 / 1000.0;
            if (cameraNodeData.PixelsPerTrixel != 0 && (double) this.CameraManager.PixelsPerTrixel != (double) cameraNodeData.PixelsPerTrixel)
              this.CameraManager.PixelsPerTrixel = (float) cameraNodeData.PixelsPerTrixel;
            this.StartNewSegment();
            this.justStarted = false;
          }
        }
        if (this.CameraManager.ActionRunning)
          this.sinceSegmentStarted += elapsed;
        if (this.sinceSegmentStarted >= this.CurrentNode.Duration + this.CurrentNode.WaitTimeOnFinish)
          this.ChangeSegment();
        if (!this.Enabled || this.Path.NeedsTrigger)
          return;
        float num1 = (float) FezMath.Saturate(this.sinceSegmentStarted.TotalSeconds / this.CurrentNode.Duration.TotalSeconds);
        float amount = (double) this.CurrentNode.Deceleration != 0.0 || (double) this.CurrentNode.Acceleration != 0.0 ? ((double) this.CurrentNode.Acceleration != 0.0 ? ((double) this.CurrentNode.Deceleration != 0.0 ? Easing.EaseInOut((double) num1, EasingType.Sine, this.CurrentNode.Acceleration, EasingType.Sine, this.CurrentNode.Deceleration) : Easing.Ease((double) num1, this.CurrentNode.Acceleration, EasingType.Quadratic)) : Easing.Ease((double) num1, -this.CurrentNode.Deceleration, EasingType.Quadratic)) : num1;
        PathSegment pathSegment1 = this.Nodes[Math.Max(this.nodeIndex - 1, 0)];
        PathSegment currentNode = this.CurrentNode;
        Vector3 vector3_1;
        Quaternion quat;
        if (this.Path.IsSpline)
        {
          PathSegment pathSegment2 = this.Nodes[Math.Max(this.nodeIndex - 2, 0)];
          PathSegment pathSegment3 = this.Nodes[Math.Min(this.nodeIndex + 1, this.Nodes.Count - 1)];
          vector3_1 = Vector3.CatmullRom(pathSegment2.Destination, pathSegment1.Destination, currentNode.Destination, pathSegment3.Destination, amount);
          quat = Quaternion.Slerp(pathSegment1.Orientation, currentNode.Orientation, amount);
        }
        else
        {
          vector3_1 = Vector3.Lerp(pathSegment1.Destination, currentNode.Destination, amount);
          quat = Quaternion.Slerp(pathSegment1.Orientation, currentNode.Orientation, amount);
        }
        float num2 = MathHelper.Lerp(pathSegment1.JitterFactor, currentNode.JitterFactor, amount);
        if ((double) num2 > 0.0)
          vector3_1 += new Vector3(RandomHelper.Centered((double) num2) * 0.5f, RandomHelper.Centered((double) num2) * 0.5f, RandomHelper.Centered((double) num2) * 0.5f);
        Vector3 vector3_2 = Vector3.Transform(Vector3.Forward, quat);
        CameraNodeData cameraNodeData1 = pathSegment1.CustomData as CameraNodeData;
        CameraNodeData cameraNodeData2 = currentNode.CustomData as CameraNodeData;
        if (!cameraNodeData2.Perspective)
          this.CameraManager.PixelsPerTrixel = MathHelper.Lerp(cameraNodeData1.PixelsPerTrixel == 0 ? this.originalPixelsPerTrixel : (float) cameraNodeData1.PixelsPerTrixel, cameraNodeData2.PixelsPerTrixel == 0 ? this.originalPixelsPerTrixel : (float) cameraNodeData2.PixelsPerTrixel, amount);
        Viewpoint view = cameraNodeData2.Perspective ? Viewpoint.Perspective : this.firstNodeViewpoint;
        if (view != this.CameraManager.Viewpoint)
        {
          if (view == Viewpoint.Perspective)
            this.CameraManager.Radius = 1.0 / 1000.0;
          this.CameraManager.ChangeViewpoint(view);
        }
        this.CameraManager.Center = vector3_1;
        this.CameraManager.Direction = vector3_2;
        if (!cameraNodeData2.Perspective)
          return;
        if (this.nodeIndex == 1)
          this.CameraManager.Radius = MathHelper.Lerp(this.originalRadius, 1.0 / 1000.0, amount);
        else if (this.nodeIndex == this.Nodes.Count - 1)
          this.CameraManager.Radius = MathHelper.Lerp(1.0 / 1000.0, this.originalRadius, amount);
        else
          this.CameraManager.Radius = 1.0 / 1000.0;
      }

      private void ChangeSegment()
      {
        this.sinceSegmentStarted -= this.CurrentNode.Duration + this.CurrentNode.WaitTimeOnFinish;
        if (this.CurrentNode.Sound != null)
          SoundEffectExtensions.EmitAt(this.CurrentNode.Sound, this.CameraManager.Center, false, RandomHelper.Centered(0.0750000029802322), false);
        ++this.nodeIndex;
        if (this.nodeIndex == this.Nodes.Count || this.nodeIndex == -1)
          this.EndPath();
        if (this.Enabled && !this.Path.NeedsTrigger)
          this.StartNewSegment();
        if (!this.Path.RunSingleSegment)
          return;
        this.Path.NeedsTrigger = true;
        this.Path.RunSingleSegment = false;
      }

      private void StartNewSegment()
      {
        this.sinceSegmentStarted -= this.CurrentNode.WaitTimeOnStart;
      }

      private void EndPath()
      {
        if (this.Path.InTransition)
          this.Nodes.RemoveAt(0);
        if (this.Path.OutTransition)
          this.Nodes.RemoveAt(this.Nodes.Count - 1);
        this.Path.NeedsTrigger = true;
        this.Path.RunOnce = false;
        this.CameraManager.ChangeViewpoint(this.originalViewpoint);
        this.Reset();
      }
    }
  }
}
