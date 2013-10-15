// Type: FezEngine.Tools.TrileMaterializer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Tools
{
  public class TrileMaterializer : IDisposable
  {
    private static readonly InvalidTrixelFaceComparer InvalidTrixelFaceComparer = new InvalidTrixelFaceComparer();
    private static readonly Vector4 OutOfSight = new Vector4(float.MinValue);
    public const int InstancesPerBatch = 220;
    private const int PredictiveBatchSize = 16;
    protected ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry;
    protected readonly List<Vector4> tempInstances;
    protected readonly List<TrileInstance> tempInstanceIds;
    private readonly List<TrixelSurface> surfaces;
    private readonly HashSet<TrixelFace> added;
    private readonly HashSet<TrixelFace> removed;
    protected readonly Trile trile;
    protected readonly Group group;

    public static bool NoTrixelsMode { get; set; }

    public ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> Geometry
    {
      get
      {
        return this.geometry;
      }
      set
      {
        this.group.Geometry = (IIndexedPrimitiveCollection) (this.geometry = value);
      }
    }

    public bool BatchNeedsCommit { get; private set; }

    public Group Group
    {
      get
      {
        return this.group;
      }
    }

    internal Trile Trile
    {
      get
      {
        return this.trile;
      }
    }

    public IEnumerable<TrixelSurface> TrixelSurfaces
    {
      get
      {
        return (IEnumerable<TrixelSurface>) this.surfaces;
      }
    }

    [ServiceDependency(Optional = true)]
    public IContentManagerProvider CMProvider { protected get; set; }

    [ServiceDependency(Optional = true)]
    public ILevelManager LevelManager { protected get; set; }

    [ServiceDependency(Optional = true)]
    public ILevelMaterializer LevelMaterializer { protected get; set; }

    [ServiceDependency(Optional = true)]
    public IDefaultCameraManager CameraManager { protected get; set; }

    [ServiceDependency(Optional = true)]
    public IDebuggingBag DebuggingBag { protected get; set; }

    [ServiceDependency(Optional = true)]
    public IEngineStateManager EngineState { protected get; set; }

    static TrileMaterializer()
    {
    }

    public TrileMaterializer(Trile trile)
      : this(trile, (Mesh) null)
    {
    }

    public TrileMaterializer(Trile trile, Mesh levelMesh)
      : this(trile, levelMesh, false)
    {
    }

    public TrileMaterializer(Trile trile, Mesh levelMesh, bool mutableSurfaces)
    {
      ServiceHelper.InjectServices((object) this);
      this.trile = trile;
      if (mutableSurfaces)
      {
        this.surfaces = new List<TrixelSurface>();
        this.added = new HashSet<TrixelFace>();
        this.removed = new HashSet<TrixelFace>();
      }
      if (levelMesh == null)
        return;
      this.group = levelMesh.AddGroup();
      this.tempInstances = new List<Vector4>();
      this.tempInstanceIds = new List<TrileInstance>();
      this.group.Geometry = (IIndexedPrimitiveCollection) this.geometry;
    }

    public void Rebuild()
    {
      this.MarkMissingCells();
      this.UpdateSurfaces();
      this.RebuildGeometry();
    }

    public void MarkMissingCells()
    {
      this.added.Clear();
      this.removed.Clear();
      this.InitializeSurfaces();
      if (TrileMaterializer.NoTrixelsMode)
        return;
      this.MarkRemoved(this.trile.MissingTrixels.Cells);
    }

    private void InitializeSurfaces()
    {
      foreach (FaceOrientation faceOrientation in Util.GetValues<FaceOrientation>())
      {
        TrixelEmplacement firstTrixel = new TrixelEmplacement(FezMath.IsPositive(faceOrientation) ? FezMath.AsVector(faceOrientation) * (new Vector3(16f) - Vector3.One) : Vector3.Zero);
        TrixelSurface trixelSurface = new TrixelSurface(faceOrientation, firstTrixel);
        Vector3 mask1 = FezMath.GetMask(FezMath.AsAxis(FezMath.GetTangent(faceOrientation)));
        int num1 = (int) Vector3.Dot(Vector3.One, mask1) * 16;
        Vector3 mask2 = FezMath.GetMask(FezMath.AsAxis(FezMath.GetBitangent(faceOrientation)));
        int num2 = (int) Vector3.Dot(Vector3.One, mask2) * 16;
        trixelSurface.RectangularParts.Add(new RectangularTrixelSurfacePart()
        {
          Orientation = faceOrientation,
          TangentSize = num1,
          BitangentSize = num2,
          Start = firstTrixel
        });
        for (int index1 = 0; index1 < num1; ++index1)
        {
          for (int index2 = 0; index2 < num2; ++index2)
            trixelSurface.Trixels.Add(new TrixelEmplacement(firstTrixel + mask1 * (float) index1 + mask2 * (float) index2));
        }
        this.surfaces.Add(trixelSurface);
      }
    }

    public void MarkAdded(IEnumerable<TrixelEmplacement> trixels)
    {
      this.Invalidate(trixels, true);
    }

    public void MarkRemoved(IEnumerable<TrixelEmplacement> trixels)
    {
      this.Invalidate(trixels, false);
    }

    private void Invalidate(IEnumerable<TrixelEmplacement> trixels, bool trixelExists)
    {
      using (IEnumerator<TrixelEmplacement> enumerator = trixels.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TrixelEmplacement trixel = enumerator.Current;
          for (int index = 0; index < 6; ++index)
          {
            FaceOrientation face = (FaceOrientation) index;
            TrixelEmplacement traversed = trixel.GetTraversal(face);
            if (this.Trile.IsBorderTrixelFace(traversed))
            {
              if (Enumerable.Any<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, bool>) (x =>
              {
                if (x.Orientation == face)
                  return x.Trixels.Contains(trixel);
                else
                  return false;
              })))
                this.removed.Add(new TrixelFace(trixel, face));
              if (trixelExists)
                this.added.Add(new TrixelFace(trixel, face));
            }
            else
            {
              FaceOrientation oppositeFace = FezMath.GetOpposite(face);
              if (Enumerable.Any<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, bool>) (x =>
              {
                if (x.Orientation == oppositeFace)
                  return x.Trixels.Contains(traversed);
                else
                  return false;
              })))
                this.removed.Add(new TrixelFace(traversed, oppositeFace));
              if (!trixelExists)
                this.added.Add(new TrixelFace(traversed, oppositeFace));
            }
          }
        }
      }
    }

    public void UpdateSurfaces()
    {
      TrixelFace[] array1 = Enumerable.ToArray<TrixelFace>((IEnumerable<TrixelFace>) this.removed);
      Array.Sort<TrixelFace>(array1, (IComparer<TrixelFace>) TrileMaterializer.InvalidTrixelFaceComparer);
      foreach (TrixelFace trixelFace in array1)
      {
        TrixelFace tf = trixelFace;
        TrixelSurface trixelSurface1 = (TrixelSurface) null;
        foreach (TrixelSurface trixelSurface2 in Enumerable.Where<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, bool>) (x =>
        {
          if (x.Orientation == tf.Face)
            return x.Trixels.Contains(tf.Id);
          else
            return false;
        })))
        {
          trixelSurface2.Trixels.Remove(tf.Id);
          if (trixelSurface2.Trixels.Count == 0)
            trixelSurface1 = trixelSurface2;
          else
            trixelSurface2.MarkAsDirty();
        }
        if (trixelSurface1 != null)
          this.surfaces.Remove(trixelSurface1);
      }
      this.removed.Clear();
      TrixelFace[] array2 = Enumerable.ToArray<TrixelFace>((IEnumerable<TrixelFace>) this.added);
      Array.Sort<TrixelFace>(array2, (IComparer<TrixelFace>) TrileMaterializer.InvalidTrixelFaceComparer);
      foreach (TrixelFace trixelFace in array2)
      {
        TrixelFace tf = trixelFace;
        TrixelSurface[] trixelSurfaceArray = Enumerable.ToArray<TrixelSurface>(Enumerable.Where<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, bool>) (x => x.CanContain(tf.Id, tf.Face))));
        if (trixelSurfaceArray.Length > 0)
        {
          TrixelSurface trixelSurface1 = trixelSurfaceArray[0];
          trixelSurface1.Trixels.Add(tf.Id);
          trixelSurface1.MarkAsDirty();
          if (trixelSurfaceArray.Length > 1)
          {
            foreach (TrixelSurface trixelSurface2 in Enumerable.Skip<TrixelSurface>((IEnumerable<TrixelSurface>) trixelSurfaceArray, 1))
            {
              trixelSurface1.Trixels.UnionWith((IEnumerable<TrixelEmplacement>) trixelSurface2.Trixels);
              this.surfaces.Remove(trixelSurface2);
            }
          }
        }
        else
          this.surfaces.Add(new TrixelSurface(tf.Face, tf.Id));
      }
      this.added.Clear();
      foreach (TrixelSurface trixelSurface in Enumerable.Where<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, bool>) (x => x.Dirty)))
        trixelSurface.RebuildParts();
    }

    public void RebuildGeometry()
    {
      int capacity = Enumerable.Sum<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, int>) (x => x.RectangularParts.Count));
      VertexGroup<VertexPositionNormalTextureInstance> vertexGroup = new VertexGroup<VertexPositionNormalTextureInstance>(capacity * 4);
      Dictionary<RectangularTrixelSurfacePart, FaceMaterialization<VertexPositionNormalTextureInstance>> dictionary = new Dictionary<RectangularTrixelSurfacePart, FaceMaterialization<VertexPositionNormalTextureInstance>>(capacity);
      Vector3 vector3_1 = new Vector3(0.5f);
      foreach (RectangularTrixelSurfacePart key in Enumerable.SelectMany<TrixelSurface, RectangularTrixelSurfacePart>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, IEnumerable<RectangularTrixelSurfacePart>>) (x => (IEnumerable<RectangularTrixelSurfacePart>) x.RectangularParts)))
      {
        Vector3 normal = FezMath.AsVector(key.Orientation);
        Vector3 vector3_2 = FezMath.AsVector(FezMath.GetTangent(key.Orientation)) * (float) key.TangentSize / 16f;
        Vector3 vector3_3 = FezMath.AsVector(FezMath.GetBitangent(key.Orientation)) * (float) key.BitangentSize / 16f;
        Vector3 position = key.Start.Position / 16f + (key.Orientation >= FaceOrientation.Right ? 1f : 0.0f) * normal / 16f - vector3_1;
        if (!dictionary.ContainsKey(key))
        {
          FaceMaterialization<VertexPositionNormalTextureInstance> faceMaterialization = new FaceMaterialization<VertexPositionNormalTextureInstance>()
          {
            V0 = vertexGroup.Reference(new VertexPositionNormalTextureInstance(position, normal)),
            V1 = vertexGroup.Reference(new VertexPositionNormalTextureInstance(position + vector3_2, normal)),
            V2 = vertexGroup.Reference(new VertexPositionNormalTextureInstance(position + vector3_2 + vector3_3, normal)),
            V3 = vertexGroup.Reference(new VertexPositionNormalTextureInstance(position + vector3_3, normal))
          };
          faceMaterialization.SetupIndices(key.Orientation);
          dictionary.Add(key, faceMaterialization);
        }
      }
      VertexPositionNormalTextureInstance[] normalTextureInstanceArray = new VertexPositionNormalTextureInstance[vertexGroup.Vertices.Count];
      int index = 0;
      foreach (SharedVertex<VertexPositionNormalTextureInstance> sharedVertex in (IEnumerable<SharedVertex<VertexPositionNormalTextureInstance>>) vertexGroup.Vertices)
      {
        normalTextureInstanceArray[index] = sharedVertex.Vertex;
        normalTextureInstanceArray[index].TextureCoordinate = FezMath.ComputeTexCoord<VertexPositionNormalTextureInstance>(normalTextureInstanceArray[index]) * (this.EngineState == null || !this.EngineState.InEditor ? Vector2.One : new Vector2(1.333333f, 1f));
        sharedVertex.Index = index++;
      }
      int[] numArray = new int[dictionary.Count * 6];
      int num = 0;
      foreach (FaceMaterialization<VertexPositionNormalTextureInstance> faceMaterialization in dictionary.Values)
      {
        for (ushort relativeIndex = (ushort) 0; (int) relativeIndex < 6; ++relativeIndex)
          numArray[num++] = faceMaterialization.GetIndex(relativeIndex);
      }
      if (this.geometry == null)
      {
        this.geometry = new ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4>(PrimitiveType.TriangleList, 220);
        this.geometry.NeedsEffectCommit = true;
        if (this.group != null)
          this.group.Geometry = (IIndexedPrimitiveCollection) this.geometry;
      }
      this.geometry.Vertices = normalTextureInstanceArray;
      this.geometry.Indices = numArray;
      this.DetermineFlags();
    }

    public void DetermineFlags()
    {
      if (this.group == null)
        return;
      ActorType type = this.trile.ActorSettings.Type;
      this.group.CustomData = (object) new TrileCustomData()
      {
        Unstable = (type == ActorType.GoldenCube),
        TiltTwoAxis = (type == ActorType.CubeShard || type == ActorType.SecretCube || type == ActorType.PieceOfHeart),
        Shiny = (type == ActorType.CubeShard || type == ActorType.SkeletonKey || type == ActorType.SecretCube || type == ActorType.PieceOfHeart)
      };
      (this.group.CustomData as TrileCustomData).DetermineCustom();
    }

    public void ClearBatch()
    {
      this.tempInstances.Clear();
      this.tempInstanceIds.Clear();
      this.tempInstances.TrimExcess();
      this.tempInstanceIds.TrimExcess();
      if (this.geometry == null)
        return;
      this.geometry.ResetBuffers();
    }

    public void ResetBatch()
    {
      this.BatchNeedsCommit = true;
      foreach (TrileInstance trileInstance in this.tempInstanceIds)
        trileInstance.InstanceId = -1;
      this.tempInstances.Clear();
      this.tempInstanceIds.Clear();
    }

    public void AddToBatch(TrileInstance instance)
    {
      this.BatchNeedsCommit = true;
      instance.InstanceId = this.tempInstances.Count;
      this.tempInstances.Add(instance.Enabled ? instance.Data.PositionPhi : TrileMaterializer.OutOfSight);
      this.tempInstanceIds.Add(instance);
    }

    public void RemoveFromBatch(TrileInstance instance)
    {
      int instanceId = instance.InstanceId;
      if (instance != this.tempInstanceIds[instanceId])
      {
        int num;
        while ((num = this.tempInstanceIds.IndexOf(instance)) != -1)
        {
          instance.InstanceId = num;
          this.RemoveFromBatch(instance);
        }
      }
      else
      {
        this.BatchNeedsCommit = true;
        for (int index = instanceId + 1; index < this.tempInstanceIds.Count; ++index)
        {
          TrileInstance trileInstance = this.tempInstanceIds[index];
          if (trileInstance.InstanceId >= 0)
            --trileInstance.InstanceId;
        }
        this.tempInstances.RemoveAt(instanceId);
        this.tempInstanceIds.RemoveAt(instanceId);
        instance.InstanceId = -1;
      }
    }

    public void CommitBatch()
    {
      if (this.geometry == null)
        return;
      this.BatchNeedsCommit = false;
      int length = (int) Math.Ceiling((double) this.tempInstances.Count / 16.0) * 16;
      if (this.geometry.Instances == null || length > this.geometry.Instances.Length)
        this.geometry.Instances = new Vector4[length];
      this.tempInstances.CopyTo(this.geometry.Instances, 0);
      this.geometry.InstanceCount = this.tempInstances.Count;
      this.geometry.UpdateBuffers();
    }

    public void UpdateInstance(TrileInstance instance)
    {
      int instanceId = instance.InstanceId;
      if (instanceId == -1 || this.geometry == null)
        return;
      Vector4 vector4 = instance.Enabled ? instance.Data.PositionPhi : TrileMaterializer.OutOfSight;
      if (instanceId < this.geometry.Instances.Length)
        this.geometry.Instances[instanceId] = vector4;
      if (instanceId >= this.tempInstances.Count)
        return;
      this.tempInstances[instanceId] = vector4;
    }

    public void FakeUpdate(int instanceId, Vector4 data)
    {
      if (instanceId >= this.geometry.Instances.Length)
        return;
      this.geometry.Instances[instanceId] = data;
    }

    public void Dispose()
    {
      this.ClearBatch();
      this.group.Mesh.RemoveGroup(this.group, true);
    }
  }
}
