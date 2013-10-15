// Type: FezEngine.Readers.TrileReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class TrileReader : ContentTypeReader<Trile>
  {
    protected override Trile Read(ContentReader input, Trile existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new Trile();
      existingInstance.Name = input.ReadString();
      existingInstance.CubemapPath = input.ReadString();
      existingInstance.Size = input.ReadVector3();
      existingInstance.Offset = input.ReadVector3();
      existingInstance.Immaterial = input.ReadBoolean();
      existingInstance.SeeThrough = input.ReadBoolean();
      existingInstance.Thin = input.ReadBoolean();
      existingInstance.ForceHugging = input.ReadBoolean();
      existingInstance.Faces = input.ReadObject<Dictionary<FaceOrientation, CollisionType>>(existingInstance.Faces);
      existingInstance.Geometry = input.ReadObject<ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4>>(existingInstance.Geometry);
      existingInstance.ActorSettings.Type = input.ReadObject<ActorType>();
      existingInstance.ActorSettings.Face = input.ReadObject<FaceOrientation>();
      existingInstance.SurfaceType = input.ReadObject<SurfaceType>();
      existingInstance.AtlasOffset = input.ReadVector2();
      return existingInstance;
    }
  }
}
