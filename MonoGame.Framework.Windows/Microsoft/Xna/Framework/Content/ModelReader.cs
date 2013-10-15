// Type: Microsoft.Xna.Framework.Content.ModelReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.Content
{
  public class ModelReader : ContentTypeReader<Model>
  {
    private List<VertexBuffer> vertexBuffers = new List<VertexBuffer>();
    private List<IndexBuffer> indexBuffers = new List<IndexBuffer>();
    private List<Effect> effects = new List<Effect>();
    private List<GraphicsResource> sharedResources = new List<GraphicsResource>();

    private static int ReadBoneReference(ContentReader reader, uint boneCount)
    {
      uint num = boneCount >= (uint) byte.MaxValue ? reader.ReadUInt32() : (uint) reader.ReadByte();
      if ((int) num != 0)
        return (int) num - 1;
      else
        return -1;
    }

    protected internal override Model Read(ContentReader reader, Model existingInstance)
    {
      List<ModelBone> bones = new List<ModelBone>();
      uint boneCount = reader.ReadUInt32();
      for (uint index = 0U; index < boneCount; ++index)
      {
        string str = reader.ReadObject<string>();
        Matrix matrix = reader.ReadMatrix();
        ModelBone modelBone = new ModelBone()
        {
          Transform = matrix,
          Index = (int) index,
          Name = str
        };
        bones.Add(modelBone);
      }
      for (int index1 = 0; (long) index1 < (long) boneCount; ++index1)
      {
        ModelBone modelBone = bones[index1];
        int index2 = ModelReader.ReadBoneReference(reader, boneCount);
        if (index2 != -1)
          modelBone.Parent = bones[index2];
        uint num = reader.ReadUInt32();
        if ((int) num != 0)
        {
          for (uint index3 = 0U; index3 < num; ++index3)
          {
            int index4 = ModelReader.ReadBoneReference(reader, boneCount);
            if (index4 != -1)
              modelBone.AddChild(bones[index4]);
          }
        }
      }
      List<ModelMesh> meshes = new List<ModelMesh>();
      int num1 = reader.ReadInt32();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        string str = reader.ReadObject<string>();
        int index2 = ModelReader.ReadBoneReference(reader, boneCount);
        BoundingSphere boundingSphere = reader.ReadBoundingSphere();
        reader.ReadObject<object>();
        int num2 = reader.ReadInt32();
        List<ModelMeshPart> parts = new List<ModelMeshPart>();
        for (uint index3 = 0U; (long) index3 < (long) num2; ++index3)
        {
          ModelMeshPart modelMeshPart = existingInstance == null ? new ModelMeshPart() : ((ReadOnlyCollection<ModelMesh>) existingInstance.Meshes)[index1].MeshParts[(int) index3];
          modelMeshPart.VertexOffset = reader.ReadInt32();
          modelMeshPart.NumVertices = reader.ReadInt32();
          modelMeshPart.StartIndex = reader.ReadInt32();
          modelMeshPart.PrimitiveCount = reader.ReadInt32();
          modelMeshPart.Tag = reader.ReadObject<object>();
          parts.Add(modelMeshPart);
          int jj = (int) index3;
          reader.ReadSharedResource<VertexBuffer>((Action<VertexBuffer>) (v => parts[jj].VertexBuffer = v));
          reader.ReadSharedResource<IndexBuffer>((Action<IndexBuffer>) (v => parts[jj].IndexBuffer = v));
          reader.ReadSharedResource<Effect>((Action<Effect>) (v => parts[jj].Effect = v));
        }
        if (existingInstance == null)
        {
          ModelMesh mesh = new ModelMesh(reader.GraphicsDevice, parts);
          mesh.Name = str;
          mesh.ParentBone = bones[index2];
          mesh.ParentBone.AddMesh(mesh);
          mesh.BoundingSphere = boundingSphere;
          meshes.Add(mesh);
        }
      }
      if (existingInstance != null)
      {
        ModelReader.ReadBoneReference(reader, boneCount);
        reader.ReadObject<object>();
        return existingInstance;
      }
      else
      {
        int index = ModelReader.ReadBoneReference(reader, boneCount);
        Model model = new Model(reader.GraphicsDevice, bones, meshes);
        model.Root = bones[index];
        model.BuildHierarchy();
        model.Tag = reader.ReadObject<object>();
        return model;
      }
    }
  }
}
