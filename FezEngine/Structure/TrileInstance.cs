// Type: FezEngine.Structure.TrileInstance
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine;
using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class TrileInstance
  {
    private Vector3 lastUpdatePosition = -Vector3.One;
    private static readonly Quaternion[] QuatLookup = new Quaternion[4]
    {
      FezMath.QuaternionFromPhi(-3.141593f),
      FezMath.QuaternionFromPhi(-1.570796f),
      FezMath.QuaternionFromPhi(0.0f),
      FezMath.QuaternionFromPhi(1.570796f)
    };
    private static readonly FaceOrientation[] OrientationLookup = new FaceOrientation[4]
    {
      FaceOrientation.Back,
      FaceOrientation.Left,
      FaceOrientation.Front,
      FaceOrientation.Right
    };
    private static readonly ILevelManager LevelManager;
    private Vector3 cachedPosition;
    private TrileEmplacement cachedEmplacement;
    private TrileInstanceData data;
    private Quaternion phiQuat;
    private FaceOrientation phiOri;
    [Serialization(Ignore = true)]
    public Trile Trile;
    [Serialization(Ignore = true)]
    public Trile VisualTrile;

    [Serialization(Name = "Id", UseAttribute = true)]
    public int TrileId { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public InstanceActorSettings ActorSettings { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float Phi
    {
      get
      {
        return this.data.PositionPhi.W;
      }
      set
      {
        this.data.PositionPhi.W = value;
        this.phiQuat = FezMath.QuaternionFromPhi(value);
        this.phiOri = FezMath.OrientationFromPhi(value);
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public List<TrileInstance> OverlappedTriles { get; set; }

    [Serialization(Optional = true)]
    public Vector3 Position
    {
      get
      {
        return this.cachedPosition;
      }
      set
      {
        this.data.PositionPhi.X = value.X;
        this.data.PositionPhi.Y = value.Y;
        this.data.PositionPhi.Z = value.Z;
        this.cachedPosition = value;
        if (this.PhysicsState != null)
          this.PhysicsState.Center = this.Center;
        this.cachedEmplacement = new TrileEmplacement(this.cachedPosition);
      }
    }

    [Serialization(Ignore = true)]
    public int? VisualTrileId { get; set; }

    [Serialization(Ignore = true)]
    public int InstanceId { get; set; }

    [Serialization(Ignore = true)]
    public bool Enabled { get; set; }

    [Serialization(Ignore = true)]
    public bool Removed { get; set; }

    [Serialization(Ignore = true)]
    public bool Collected { get; set; }

    [Serialization(Ignore = true)]
    public bool Hidden { get; set; }

    [Serialization(Ignore = true)]
    public bool Foreign { get; set; }

    [Serialization(Ignore = true)]
    public bool ForceSeeThrough { get; set; }

    [Serialization(Ignore = true)]
    public bool ForceTopMaybe { get; set; }

    [Serialization(Ignore = true)]
    public bool ForceClampToGround { get; set; }

    [Serialization(Ignore = true)]
    public bool SkipCulling { get; set; }

    [Serialization(Ignore = true)]
    public bool NeedsRandomCleanup { get; set; }

    [Serialization(Ignore = true)]
    public float LastTreasureSin { get; set; }

    [Serialization(Ignore = true)]
    internal Vector3 LastUpdatePosition
    {
      get
      {
        return this.lastUpdatePosition;
      }
    }

    [Serialization(Ignore = true)]
    public InstancePhysicsState PhysicsState { get; set; }

    [Serialization(Ignore = true)]
    public TrileEmplacement Emplacement
    {
      get
      {
        return this.cachedEmplacement;
      }
      set
      {
        this.data.PositionPhi.X = (float) value.X;
        this.data.PositionPhi.Y = (float) value.Y;
        this.data.PositionPhi.Z = (float) value.Z;
        this.cachedPosition = new Vector3((float) value.X, (float) value.Y, (float) value.Z);
        if (this.PhysicsState != null)
          this.PhysicsState.Center = this.Center;
        this.cachedEmplacement = value;
      }
    }

    public TrileInstanceData Data
    {
      get
      {
        return this.data;
      }
    }

    [Serialization(Ignore = true)]
    public bool IsMovingGroup { get; set; }

    [Serialization(Ignore = true)]
    public TrileEmplacement OriginalEmplacement { get; set; }

    [Serialization(Ignore = true)]
    public bool Unsafe { get; set; }

    [Serialization(Ignore = true)]
    public Point? OldSsEmplacement { get; set; }

    public bool Overlaps
    {
      get
      {
        if (this.OverlappedTriles != null)
          return this.OverlappedTriles.Count > 0;
        else
          return false;
      }
    }

    public Vector3 TransformedSize
    {
      get
      {
        if (this.phiOri == FaceOrientation.Left || this.phiOri == FaceOrientation.Right)
          return FezMath.ZYX(this.Trile.Size);
        else
          return this.Trile.Size;
      }
    }

    public Vector3 Center
    {
      get
      {
        return Vector3.Transform(this.Trile.Offset, this.phiQuat) / 2f + new Vector3(0.5f) + this.Position;
      }
    }

    static TrileInstance()
    {
      if (!ServiceHelper.IsFull)
        return;
      TrileInstance.LevelManager = ServiceHelper.Get<ILevelManager>();
    }

    public TrileInstance()
    {
      this.ActorSettings = new InstanceActorSettings();
      this.Enabled = true;
      this.InstanceId = -1;
    }

    public TrileInstance(TrileEmplacement emplacement, int trileId)
      : this(emplacement.AsVector, trileId)
    {
    }

    public TrileInstance(Vector3 position, int trileId)
      : this()
    {
      this.data.PositionPhi = new Vector4(position, 0.0f);
      this.cachedPosition = position;
      this.cachedEmplacement = new TrileEmplacement(position);
      this.Update();
      this.TrileId = trileId;
      this.RefreshTrile();
    }

    public void SetPhiLight(float phi)
    {
      this.data.PositionPhi.W = phi;
    }

    public void SetPhiLight(byte orientation)
    {
      this.data.PositionPhi.W = (float) ((int) orientation - 2) * 1.570796f;
      this.phiQuat = TrileInstance.QuatLookup[(int) orientation];
      this.phiOri = TrileInstance.OrientationLookup[(int) orientation];
    }

    public void Update()
    {
      this.lastUpdatePosition = this.Position;
    }

    public TrileInstance PopOverlap()
    {
      if (this.OverlappedTriles == null || this.OverlappedTriles.Count == 0)
        throw new InvalidOperationException();
      int index1 = this.OverlappedTriles.Count - 1;
      TrileInstance trileInstance = this.OverlappedTriles[index1];
      this.OverlappedTriles.RemoveAt(index1);
      for (int index2 = index1 - 1; index2 >= 0 && this.OverlappedTriles.Count > index2; --index2)
        trileInstance.PushOverlap(this.OverlappedTriles[index2]);
      this.OverlappedTriles.Clear();
      return trileInstance;
    }

    public void PushOverlap(TrileInstance instance)
    {
      if (this.OverlappedTriles == null)
        this.OverlappedTriles = new List<TrileInstance>();
      this.OverlappedTriles.Add(instance);
      if (!instance.Overlaps)
        return;
      this.OverlappedTriles.AddRange((IEnumerable<TrileInstance>) instance.OverlappedTriles);
      instance.OverlappedTriles.Clear();
    }

    public void ResetTrile()
    {
      this.VisualTrile = this.Trile = (Trile) null;
      if (TrileInstance.LevelManager.TrileSet != null && !TrileInstance.LevelManager.TrileSet.Triles.ContainsKey(this.TrileId))
        this.TrileId = -1;
      this.RefreshTrile();
    }

    public void RefreshTrile()
    {
      this.Trile = TrileInstance.LevelManager.SafeGetTrile(this.TrileId);
      if (this.VisualTrileId.HasValue)
        this.VisualTrile = TrileInstance.LevelManager.SafeGetTrile(this.VisualTrileId.Value);
      else
        this.VisualTrile = this.Trile;
    }

    public CollisionType GetRotatedFace(FaceOrientation face)
    {
      FaceOrientation index = FezMath.OrientationFromPhi(FezMath.ToPhi(face) - this.Phi);
      CollisionType collisionType = this.Trile.Faces[index];
      if (collisionType == CollisionType.TopOnly)
      {
        TrileEmplacement emplacement = this.Emplacement;
        ++emplacement.Y;
        Vector3 mask = FezMath.GetMask(FezMath.AsAxis(face));
        TrileInstance trileInstance;
        if (TrileInstance.LevelManager.Triles.TryGetValue(emplacement, out trileInstance) && trileInstance.Enabled && !trileInstance.IsMovingGroup && (trileInstance.Trile.Geometry == null || !trileInstance.Trile.Geometry.Empty || trileInstance.Trile.Faces[index] != CollisionType.None) && (!trileInstance.Trile.Immaterial && trileInstance.Trile.Faces[index] != CollisionType.Immaterial && (!trileInstance.Trile.Thin && !ActorTypeExtensions.IsPickable(trileInstance.Trile.ActorSettings.Type)) && (((double) trileInstance.Trile.Size.Y == 1.0 || trileInstance.ForceTopMaybe) && FezMath.AlmostEqual(FezMath.Dot(trileInstance.Center, mask), FezMath.Dot(this.Center, mask)))))
          collisionType = CollisionType.None;
      }
      return collisionType;
    }

    public TrileInstance Clone()
    {
      TrileInstance trileInstance1 = new TrileInstance(this.Position, this.TrileId)
      {
        ActorSettings = new InstanceActorSettings(this.ActorSettings),
        TrileId = this.TrileId,
        Phi = this.Phi
      };
      if (this.Overlaps)
      {
        trileInstance1.OverlappedTriles = new List<TrileInstance>();
        foreach (TrileInstance trileInstance2 in this.OverlappedTriles)
          trileInstance1.OverlappedTriles.Add(trileInstance2.Clone());
      }
      return trileInstance1;
    }
  }
}
