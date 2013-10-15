// Type: FezEngine.Tools.ArtObjectMaterializer
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
using System.Threading;

namespace FezEngine.Tools
{
  public class ArtObjectMaterializer
  {
    private static readonly InvalidTrixelFaceComparer invalidTrixelFaceComparer = new InvalidTrixelFaceComparer();
    public const int InstancesPerBatch = 60;
    private readonly HashSet<TrixelFace> added;
    private readonly HashSet<TrixelFace> removed;
    private readonly ArtObject artObject;
    private readonly TrixelCluster missingTrixels;
    private readonly Vector3 size;
    private readonly List<TrixelSurface> surfaces;
    private ArtObjectMaterializer.InvalidationContext otherThreadContext;

    public bool Dirty { get; set; }

    public ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix> Geometry { get; set; }

    [ServiceDependency(Optional = true)]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency(Optional = true)]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency(Optional = true)]
    public ILevelManager LevelManager { protected get; set; }

    static ArtObjectMaterializer()
    {
    }

    public ArtObjectMaterializer(TrixelCluster missingTrixels, List<TrixelSurface> surfaces, Vector3 size)
    {
      this.missingTrixels = missingTrixels;
      ServiceHelper.InjectServices((object) this);
      this.added = new HashSet<TrixelFace>();
      this.removed = new HashSet<TrixelFace>();
      this.surfaces = surfaces ?? new List<TrixelSurface>();
      this.size = size;
    }

    public ArtObjectMaterializer(ArtObject artObject)
    {
      this.artObject = artObject;
      ServiceHelper.InjectServices((object) this);
      artObject.Materializer = this;
      this.size = artObject.Size;
      if (artObject.MissingTrixels == null)
        return;
      this.added = new HashSet<TrixelFace>();
      this.removed = new HashSet<TrixelFace>();
      this.missingTrixels = artObject.MissingTrixels;
      if (artObject.TrixelSurfaces == null)
        artObject.TrixelSurfaces = this.surfaces = new List<TrixelSurface>();
      else
        this.surfaces = artObject.TrixelSurfaces;
    }

    public void Rebuild()
    {
      this.Rebuild(false);
    }

    public void Rebuild(bool force)
    {
      if (force || this.surfaces.Count == 0)
      {
        this.MarkMissingCells();
        this.UpdateSurfaces();
      }
      this.RebuildGeometry();
    }

    public void Update()
    {
      this.UpdateSurfaces();
      this.RebuildGeometry();
    }

    public void MarkMissingCells()
    {
      this.added.Clear();
      this.removed.Clear();
      this.InitializeSurfaces();
      this.MarkRemoved(this.missingTrixels.Cells);
    }

    public void MarkAdded(IEnumerable<TrixelEmplacement> trixels)
    {
      this.Invalidate(trixels, true);
    }

    public void MarkRemoved(IEnumerable<TrixelEmplacement> trixels)
    {
      this.Invalidate(trixels, false);
    }

    private void Invalidate(IEnumerable<TrixelEmplacement> trixels, bool trixelsExist)
    {
      int num = Enumerable.Count<TrixelEmplacement>(trixels);
      ArtObjectMaterializer.InvalidationContext context = new ArtObjectMaterializer.InvalidationContext()
      {
        Trixels = Enumerable.Take<TrixelEmplacement>(trixels, num / 2),
        TrixelsExist = trixelsExist,
        Added = (ICollection<TrixelFace>) new List<TrixelFace>(),
        Removed = (ICollection<TrixelFace>) new List<TrixelFace>()
      };
      this.otherThreadContext = new ArtObjectMaterializer.InvalidationContext()
      {
        Trixels = Enumerable.Skip<TrixelEmplacement>(trixels, num / 2),
        TrixelsExist = trixelsExist,
        Added = (ICollection<TrixelFace>) new List<TrixelFace>(),
        Removed = (ICollection<TrixelFace>) new List<TrixelFace>()
      };
      Thread thread = new Thread(new ThreadStart(this.InvalidateOtherThread));
      thread.Start();
      this.Invalidate(context);
      thread.Join();
      this.added.UnionWith((IEnumerable<TrixelFace>) context.Added);
      this.added.UnionWith((IEnumerable<TrixelFace>) this.otherThreadContext.Added);
      this.removed.UnionWith((IEnumerable<TrixelFace>) context.Removed);
      this.removed.UnionWith((IEnumerable<TrixelFace>) this.otherThreadContext.Removed);
      this.Dirty = true;
    }

    private void InvalidateOtherThread()
    {
      this.Invalidate(this.otherThreadContext);
    }

    private bool TrixelExists(TrixelEmplacement trixelIdentifier)
    {
      if (!this.missingTrixels.Empty)
        return !this.missingTrixels.IsFilled(trixelIdentifier);
      else
        return true;
    }

    private bool CanContain(TrixelEmplacement trixel)
    {
      if ((double) trixel.X < (double) this.size.X * 16.0 && (double) trixel.Y < (double) this.size.Y * 16.0 && ((double) trixel.Z < (double) this.size.Z * 16.0 && trixel.X >= 0) && trixel.Y >= 0)
        return trixel.Z >= 0;
      else
        return false;
    }

    private bool IsBorderTrixelFace(TrixelEmplacement traversed)
    {
      if (this.CanContain(traversed))
        return !this.TrixelExists(traversed);
      else
        return true;
    }

    private void Invalidate(ArtObjectMaterializer.InvalidationContext context)
    {
      using (IEnumerator<TrixelEmplacement> enumerator = context.Trixels.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TrixelEmplacement trixel = enumerator.Current;
          for (int index = 0; index < 6; ++index)
          {
            FaceOrientation face = (FaceOrientation) index;
            TrixelEmplacement traversed = trixel.GetTraversal(face);
            if (this.IsBorderTrixelFace(traversed))
            {
              if (Enumerable.Any<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, bool>) (x =>
              {
                if (x.Orientation == face)
                  return x.AnyRectangleContains(trixel);
                else
                  return false;
              })))
                context.Removed.Add(new TrixelFace(trixel, face));
              if (context.TrixelsExist)
                context.Added.Add(new TrixelFace(trixel, face));
            }
            else
            {
              FaceOrientation oppositeFace = FezMath.GetOpposite(face);
              if (Enumerable.Any<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, bool>) (x =>
              {
                if (x.Orientation == oppositeFace)
                  return x.AnyRectangleContains(traversed);
                else
                  return false;
              })))
                context.Removed.Add(new TrixelFace(traversed, oppositeFace));
              if (!context.TrixelsExist)
                context.Added.Add(new TrixelFace(traversed, oppositeFace));
            }
          }
        }
      }
    }

    public void UpdateSurfaces()
    {
      TrixelFace[] array1 = Enumerable.ToArray<TrixelFace>((IEnumerable<TrixelFace>) this.removed);
      Array.Sort<TrixelFace>(array1, (IComparer<TrixelFace>) ArtObjectMaterializer.invalidTrixelFaceComparer);
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
      Array.Sort<TrixelFace>(array2, (IComparer<TrixelFace>) ArtObjectMaterializer.invalidTrixelFaceComparer);
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
      this.RebuildParts();
    }

    private void RebuildParts()
    {
      foreach (TrixelSurface trixelSurface in Enumerable.Where<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, bool>) (x => x.Dirty)))
        trixelSurface.RebuildParts();
    }

    private void InitializeSurfaces()
    {
      this.surfaces.Clear();
      foreach (FaceOrientation faceOrientation in Util.GetValues<FaceOrientation>())
      {
        TrixelEmplacement firstTrixel = new TrixelEmplacement(FezMath.IsPositive(faceOrientation) ? FezMath.AsVector(faceOrientation) * (this.size * 16f - Vector3.One) : Vector3.Zero);
        TrixelSurface trixelSurface = new TrixelSurface(faceOrientation, firstTrixel);
        Vector3 mask1 = FezMath.GetMask(FezMath.AsAxis(FezMath.GetTangent(faceOrientation)));
        int num1 = (int) Vector3.Dot(this.size, mask1) * 16;
        Vector3 mask2 = FezMath.GetMask(FezMath.AsAxis(FezMath.GetBitangent(faceOrientation)));
        int num2 = (int) Vector3.Dot(this.size, mask2) * 16;
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

    public void RebuildGeometry()
    {
      if (this.surfaces == null)
        return;
      if (this.Geometry == null)
        this.Geometry = new ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix>(PrimitiveType.TriangleList, 60);
      int capacity = Enumerable.Sum<TrixelSurface>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, int>) (x => x.RectangularParts.Count));
      Dictionary<RectangularTrixelSurfacePart, FaceMaterialization<VertexPositionNormalTextureInstance>> dictionary = new Dictionary<RectangularTrixelSurfacePart, FaceMaterialization<VertexPositionNormalTextureInstance>>(capacity * 4);
      VertexGroup<VertexPositionNormalTextureInstance> vertexGroup = new VertexGroup<VertexPositionNormalTextureInstance>(capacity);
      Vector3 vector3_1 = this.size / 2f;
      foreach (RectangularTrixelSurfacePart key in Enumerable.SelectMany<TrixelSurface, RectangularTrixelSurfacePart>((IEnumerable<TrixelSurface>) this.surfaces, (Func<TrixelSurface, IEnumerable<RectangularTrixelSurfacePart>>) (x => (IEnumerable<RectangularTrixelSurfacePart>) x.RectangularParts)))
      {
        if (!dictionary.ContainsKey(key))
        {
          Vector3 normal = FezMath.AsVector(key.Orientation);
          Vector3 vector3_2 = FezMath.AsVector(FezMath.GetTangent(key.Orientation)) * (float) key.TangentSize / 16f;
          Vector3 vector3_3 = FezMath.AsVector(FezMath.GetBitangent(key.Orientation)) * (float) key.BitangentSize / 16f;
          Vector3 v = key.Start.Position / 16f + (FezMath.IsPositive(key.Orientation) ? 1f : 0.0f) * normal / 16f - vector3_1;
          FaceMaterialization<VertexPositionNormalTextureInstance> faceMaterialization = new FaceMaterialization<VertexPositionNormalTextureInstance>()
          {
            V0 = vertexGroup.Reference(new VertexPositionNormalTextureInstance(FezMath.Round(v, 4), normal)),
            V1 = vertexGroup.Reference(new VertexPositionNormalTextureInstance(FezMath.Round(v + vector3_2, 4), normal)),
            V2 = vertexGroup.Reference(new VertexPositionNormalTextureInstance(FezMath.Round(v + vector3_2 + vector3_3, 4), normal)),
            V3 = vertexGroup.Reference(new VertexPositionNormalTextureInstance(FezMath.Round(v + vector3_3, 4), normal))
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
        normalTextureInstanceArray[index].TextureCoordinate = FezMath.ComputeTexCoord<VertexPositionNormalTextureInstance>(normalTextureInstanceArray[index], this.size) * new Vector2(1.333333f, 1f);
        sharedVertex.Index = index++;
      }
      int[] numArray = new int[dictionary.Count * 6];
      int num = 0;
      foreach (FaceMaterialization<VertexPositionNormalTextureInstance> faceMaterialization in dictionary.Values)
      {
        for (ushort relativeIndex = (ushort) 0; (int) relativeIndex < 6; ++relativeIndex)
          numArray[num++] = faceMaterialization.GetIndex(relativeIndex);
      }
      this.Geometry.Vertices = normalTextureInstanceArray;
      this.Geometry.Indices = numArray;
      if (this.artObject == null)
        return;
      this.PostInitialize();
    }

    public void PostInitialize()
    {
      if (this.CMProvider != null && this.artObject.CubemapPath != null && this.artObject.Cubemap == null)
        this.artObject.Cubemap = this.CMProvider.CurrentLevel.Load<Texture2D>("Art Objects/" + this.artObject.CubemapPath);
      if (this.artObject.Geometry != null || this.Geometry == null)
        return;
      this.artObject.Geometry = this.Geometry;
    }

    public void RecomputeTexCoords(bool widen)
    {
      for (int index = 0; index < this.artObject.Geometry.Vertices.Length; ++index)
        this.artObject.Geometry.Vertices[index].TextureCoordinate = FezMath.ComputeTexCoord<VertexPositionNormalTextureInstance>(this.artObject.Geometry.Vertices[index], this.artObject.Size) * (!widen ? Vector2.One : new Vector2(1.333333f, 1f));
    }

    private struct InvalidationContext
    {
      public IEnumerable<TrixelEmplacement> Trixels;
      public bool TrixelsExist;
      public ICollection<TrixelFace> Added;
      public ICollection<TrixelFace> Removed;
    }
  }
}
