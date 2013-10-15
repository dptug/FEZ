// Type: FezEngine.Structure.ArtObject
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class ArtObject : ITrixelObject, IDisposable
  {
    public string Name { get; set; }

    public string CubemapPath { get; set; }

    [Serialization(Ignore = true)]
    public Texture2D Cubemap { get; set; }

    public Vector3 Size { get; set; }

    [Serialization(Optional = true)]
    public ActorType ActorType { get; set; }

    [Serialization(Optional = true)]
    public bool NoSihouette { get; set; }

    [Serialization(Optional = true)]
    public TrixelCluster MissingTrixels { get; set; }

    [Serialization(CollectionItemName = "surface", Optional = true)]
    public List<TrixelSurface> TrixelSurfaces { get; set; }

    [Serialization(Optional = true)]
    public ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix> Geometry { get; set; }

    [Serialization(Ignore = true)]
    public ArtObjectMaterializer Materializer { get; set; }

    [Serialization(Ignore = true)]
    public Group Group { get; set; }

    [Serialization(Ignore = true)]
    public int InstanceCount { get; set; }

    public bool TrixelExists(TrixelEmplacement trixelIdentifier)
    {
      if (!this.MissingTrixels.Empty)
        return !this.MissingTrixels.IsFilled(trixelIdentifier);
      else
        return true;
    }

    public bool CanContain(TrixelEmplacement trixel)
    {
      if ((double) trixel.X < (double) this.Size.X * 16.0 && (double) trixel.Y < (double) this.Size.Y * 16.0 && ((double) trixel.Z < (double) this.Size.Z * 16.0 && trixel.X >= 0) && trixel.Y >= 0)
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
      if (this.CanContain(traversed))
        return !this.TrixelExists(traversed);
      else
        return true;
    }

    public void Dispose()
    {
      if (this.Cubemap != null)
        this.Cubemap.Dispose();
      this.Cubemap = (Texture2D) null;
      if (this.Geometry != null)
        this.Geometry.Dispose();
      this.Geometry = (ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix>) null;
      this.Group = (Group) null;
      this.Materializer = (ArtObjectMaterializer) null;
    }
  }
}
