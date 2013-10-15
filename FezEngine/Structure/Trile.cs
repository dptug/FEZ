// Type: FezEngine.Structure.Trile
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine;
using FezEngine.Structure.Geometry;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class Trile : ITrixelObject
  {
    [Serialization(Ignore = true)]
    public int Id { get; set; }

    public string Name { get; set; }

    public string CubemapPath { get; set; }

    [Serialization(Ignore = true)]
    public List<TrileInstance> Instances { get; set; }

    public Dictionary<FaceOrientation, CollisionType> Faces { get; set; }

    [Serialization(Optional = true)]
    public ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> Geometry { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public TrixelCluster MissingTrixels { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public TrileActorSettings ActorSettings { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Immaterial { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool SeeThrough { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Thin { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool ForceHugging { get; set; }

    [Serialization(Ignore = true)]
    public TrileSet TrileSet { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public Vector3 Size { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public Vector3 Offset { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public SurfaceType SurfaceType { get; set; }

    [Serialization(Ignore = true)]
    public Vector2 AtlasOffset { get; set; }

    [Serialization(Ignore = true)]
    public bool ForceKeep { get; set; }

    public Trile()
    {
      this.MissingTrixels = new TrixelCluster();
      this.Instances = new List<TrileInstance>();
      this.ActorSettings = new TrileActorSettings();
      this.Name = "Untitled";
      this.Size = Vector3.One;
      this.Faces = new Dictionary<FaceOrientation, CollisionType>(4, (IEqualityComparer<FaceOrientation>) FaceOrientationComparer.Default);
    }

    public Trile(CollisionType faceType)
      : this()
    {
      this.Faces.Add(FaceOrientation.Back, faceType);
      this.Faces.Add(FaceOrientation.Front, faceType);
      this.Faces.Add(FaceOrientation.Left, faceType);
      this.Faces.Add(FaceOrientation.Right, faceType);
      this.MissingTrixels.OnDeserialization();
    }

    public override string ToString()
    {
      return this.Name;
    }

    public bool TrixelExists(TrixelEmplacement trixelIdentifier)
    {
      if (!this.MissingTrixels.Empty)
        return !this.MissingTrixels.IsFilled(trixelIdentifier);
      else
        return true;
    }

    public bool CanContain(TrixelEmplacement trixel)
    {
      return Trile.TrixelInRange(trixel);
    }

    public static bool TrixelInRange(TrixelEmplacement trixel)
    {
      if (trixel.X < 16 && trixel.Y < 16 && (trixel.Z < 16 && trixel.X >= 0) && trixel.Y >= 0)
        return trixel.Z >= 0;
      else
        return false;
    }

    public bool IsBorderTrixelFace(TrixelEmplacement id, FaceOrientation face)
    {
      return this.IsBorderTrixelFace(id.GetTraversal(face));
    }

    public bool IsBorderTrixelFace(TrixelEmplacement traversed)
    {
      if (Trile.TrixelInRange(traversed))
        return !this.TrixelExists(traversed);
      else
        return true;
    }

    public Trile Clone()
    {
      return new Trile()
      {
        CubemapPath = this.CubemapPath,
        Faces = new Dictionary<FaceOrientation, CollisionType>((IDictionary<FaceOrientation, CollisionType>) this.Faces),
        Id = this.Id,
        Immaterial = this.Immaterial,
        Name = this.Name,
        SeeThrough = this.SeeThrough,
        Thin = this.Thin,
        ForceHugging = this.ForceHugging,
        ActorSettings = new TrileActorSettings(this.ActorSettings),
        SurfaceType = this.SurfaceType
      };
    }

    public void CopyFrom(Trile copy)
    {
      this.ActorSettings = new TrileActorSettings(copy.ActorSettings);
      this.Faces = new Dictionary<FaceOrientation, CollisionType>((IDictionary<FaceOrientation, CollisionType>) copy.Faces);
      this.Immaterial = copy.Immaterial;
      this.Thin = copy.Thin;
      this.Name = copy.Name;
      this.ForceHugging = copy.ForceHugging;
      this.SeeThrough = copy.SeeThrough;
      this.SurfaceType = copy.SurfaceType;
    }

    public void Dispose()
    {
      if (this.Geometry != null)
      {
        this.Geometry.Dispose();
        this.Geometry = (ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4>) null;
      }
      this.TrileSet = (TrileSet) null;
    }
  }
}
