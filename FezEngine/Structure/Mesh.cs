// Type: FezEngine.Structure.Mesh
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Effects;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class Mesh : IDisposable
  {
    private static readonly Comparison<Group> DefaultOrder = (Comparison<Group>) ((a, b) => 0);
    private Quaternion rotation = Quaternion.Identity;
    private readonly Dirtyable<Texture> texture = new Dirtyable<Texture>();
    private readonly Dirtyable<Matrix> textureMatrix = new Dirtyable<Matrix>();
    private Matrix worldMatrix;
    private Vector3 position;
    private Vector3 scale;
    private bool groupsDirty;
    private TexturingType texturingType;
    private Comparison<Group> _groupOrder;

    internal GraphicsDevice GraphicsDevice { get; private set; }

    public Mesh Parent { get; set; }

    public object CustomData { get; set; }

    public Group FirstGroup
    {
      get
      {
        return this.Groups[0];
      }
    }

    public List<Group> Groups { get; private set; }

    public BaseEffect Effect { get; set; }

    public Mesh.RenderingHandler CustomRenderingHandler { get; set; }

    public Material Material { get; set; }

    public bool Enabled { get; set; }

    public bool AlwaysOnTop { get; set; }

    public bool DepthWrites { get; set; }

    public CullMode Culling { get; set; }

    public BlendingMode? Blending { get; set; }

    public int UsedSamplers { get; set; }

    public SamplerState SamplerState { get; set; }

    public bool RotateOffCenter { get; set; }

    public bool ScaleAfterRotation { get; set; }

    public bool SkipStates { get; set; }

    public bool SkipGroupCheck { get; set; }

    public TexturingType TexturingType
    {
      get
      {
        return this.texturingType;
      }
    }

    public Dirtyable<Texture> Texture
    {
      get
      {
        return this.texture;
      }
      set
      {
        if (value == null)
          this.texture.Set((Texture) null);
        else
          this.texture.Set((Texture) value);
        this.texturingType = value == null ? TexturingType.None : (value.Value is Texture2D ? TexturingType.Texture2D : (value.Value is TextureCube ? TexturingType.Cubemap : TexturingType.None));
      }
    }

    public Texture2D TextureMap
    {
      get
      {
        return this.texture.Value as Texture2D;
      }
    }

    public TextureCube CubeMap
    {
      get
      {
        return this.texture.Value as TextureCube;
      }
    }

    public Dirtyable<Matrix> TextureMatrix
    {
      get
      {
        return this.textureMatrix;
      }
      set
      {
        this.textureMatrix.Set((Matrix) value);
      }
    }

    public Vector3 Position
    {
      get
      {
        return this.position;
      }
      set
      {
        if (!(this.position != value))
          return;
        this.position = value;
        this.RebuildWorld();
      }
    }

    public Vector3 Scale
    {
      get
      {
        return this.scale;
      }
      set
      {
        if (!(this.scale != value))
          return;
        this.scale = value;
        this.RebuildWorld();
      }
    }

    public Quaternion Rotation
    {
      get
      {
        return this.rotation;
      }
      set
      {
        if (!(this.rotation != value))
          return;
        this.rotation = value;
        this.RebuildWorld();
      }
    }

    public Matrix WorldMatrix
    {
      get
      {
        return this.worldMatrix;
      }
      set
      {
        this.worldMatrix = value;
        this.worldMatrix.Decompose(out this.scale, out this.rotation, out this.position);
        foreach (Group group in this.Groups)
          group.RebuildCompositeWorld();
      }
    }

    public Comparison<Group> GroupOrder
    {
      get
      {
        return this._groupOrder;
      }
      set
      {
        this._groupOrder = value;
        this.groupsDirty = true;
      }
    }

    public bool GroupLazyMatrices { get; set; }

    static Mesh()
    {
    }

    public Mesh()
    {
      this.Material = new Material();
      this.Groups = new List<Group>();
      this.WorldMatrix = (Matrix) (this.TextureMatrix = (Dirtyable<Matrix>) Matrix.Identity);
      this.DepthWrites = true;
      this.Culling = CullMode.CullCounterClockwiseFace;
      this.texturingType = TexturingType.None;
      this.UsedSamplers = 1;
      this.GroupOrder = Mesh.DefaultOrder;
      this.Enabled = true;
      this.GraphicsDevice = ServiceHelper.Get<IGraphicsDeviceService>().GraphicsDevice;
    }

    public void SetFastPosition(Vector3 position)
    {
      this.position = position;
      this.worldMatrix = Matrix.CreateTranslation(position);
      foreach (Group group in this.Groups)
        group.RebuildCompositeWorld(false);
    }

    public void Recenter<T>(Vector3 center) where T : struct, IVertex
    {
      foreach (Group group in this.Groups)
      {
        T[] vertices = (group.Geometry as IndexedUserPrimitives<T>).Vertices;
        for (int index = 0; index < vertices.Length; ++index)
          vertices[index].Position = center - vertices[index].Position;
      }
    }

    public void BakeTransform<T>() where T : struct, IVertex
    {
      foreach (Group group in this.Groups)
        group.BakeTransform<T>();
      this.WorldMatrix = Matrix.Identity;
    }

    public void BakeTransformWithNormal<T>() where T : struct, ILitVertex
    {
      foreach (Group group in this.Groups)
        group.BakeTransformWithNormal<T>();
      this.WorldMatrix = Matrix.Identity;
    }

    public Group CollapseToBufferWithNormal<T>() where T : struct, ILitVertex
    {
      return this.CollapseToBufferWithNormal<T>(0, this.Groups.Count);
    }

    public Group CollapseToBufferWithNormal<T>(int fromGroup, int count) where T : struct, ILitVertex
    {
      if (count == 0)
        return (Group) null;
      List<T> vertices = new List<T>();
      List<int> list = new List<int>();
      PrimitiveType type = PrimitiveType.TriangleList;
      for (int i = fromGroup + count - 1; i >= fromGroup; --i)
      {
        Group group = this.Groups[i];
        group.BakeTransformWithNormal<T>();
        IndexedUserPrimitives<T> indexedUserPrimitives = group.Geometry as IndexedUserPrimitives<T>;
        list.AddRange(Enumerable.Select<int, int>((IEnumerable<int>) indexedUserPrimitives.Indices, (Func<int, int>) (x => x + vertices.Count)));
        vertices.AddRange((IEnumerable<T>) indexedUserPrimitives.Vertices);
        type = indexedUserPrimitives.PrimitiveType;
        this.RemoveGroupAt(i);
      }
      Group group1 = this.AddGroup();
      BufferedIndexedPrimitives<T> indexedPrimitives = new BufferedIndexedPrimitives<T>(vertices.ToArray(), list.ToArray(), type);
      indexedPrimitives.UpdateBuffers();
      group1.Geometry = (IIndexedPrimitiveCollection) indexedPrimitives;
      return group1;
    }

    public Group CollapseToBufferWithNormalTexture<T>() where T : struct, ILitVertex, ITexturedVertex
    {
      return this.CollapseToBufferWithNormalTexture<T>(0, this.Groups.Count);
    }

    public Group CollapseToBufferWithNormalTexture<T>(int fromGroup, int count) where T : struct, ILitVertex, ITexturedVertex
    {
      if (count == 0)
        return (Group) null;
      List<T> vertices = new List<T>();
      List<int> list = new List<int>();
      PrimitiveType type = PrimitiveType.TriangleList;
      for (int i = fromGroup + count - 1; i >= fromGroup; --i)
      {
        Group group = this.Groups[i];
        group.BakeTransformWithNormalTexture<T>();
        IndexedUserPrimitives<T> indexedUserPrimitives = group.Geometry as IndexedUserPrimitives<T>;
        list.AddRange(Enumerable.Select<int, int>((IEnumerable<int>) indexedUserPrimitives.Indices, (Func<int, int>) (x => x + vertices.Count)));
        vertices.AddRange((IEnumerable<T>) indexedUserPrimitives.Vertices);
        type = indexedUserPrimitives.PrimitiveType;
        this.RemoveGroupAt(i);
      }
      Group group1 = this.AddGroup();
      BufferedIndexedPrimitives<T> indexedPrimitives = new BufferedIndexedPrimitives<T>(vertices.ToArray(), list.ToArray(), type);
      indexedPrimitives.UpdateBuffers();
      group1.Geometry = (IIndexedPrimitiveCollection) indexedPrimitives;
      return group1;
    }

    public Group CollapseWithNormalTexture<T>() where T : struct, ILitVertex, ITexturedVertex
    {
      return this.CollapseWithNormalTexture<T>(0, this.Groups.Count);
    }

    public Group CollapseWithNormalTexture<T>(int fromGroup, int count) where T : struct, ILitVertex, ITexturedVertex
    {
      if (count == 0)
        return (Group) null;
      List<T> vertices = new List<T>();
      List<int> list = new List<int>();
      PrimitiveType type = PrimitiveType.TriangleList;
      for (int i = fromGroup + count - 1; i >= fromGroup; --i)
      {
        Group group = this.Groups[i];
        group.BakeTransformWithNormalTexture<T>();
        IndexedUserPrimitives<T> indexedUserPrimitives = group.Geometry as IndexedUserPrimitives<T>;
        list.AddRange(Enumerable.Select<int, int>((IEnumerable<int>) indexedUserPrimitives.Indices, (Func<int, int>) (x => x + vertices.Count)));
        vertices.AddRange((IEnumerable<T>) indexedUserPrimitives.Vertices);
        type = indexedUserPrimitives.PrimitiveType;
        this.RemoveGroupAt(i);
      }
      Group group1 = this.AddGroup();
      group1.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<T>(vertices.ToArray(), list.ToArray(), type);
      return group1;
    }

    public Group CollapseWithNormal<T>() where T : struct, ILitVertex
    {
      return this.CollapseWithNormal<T>(0, this.Groups.Count);
    }

    public Group CollapseWithNormal<T>(int fromGroup, int count) where T : struct, ILitVertex
    {
      if (count == 0)
        return (Group) null;
      List<T> vertices = new List<T>();
      List<int> list = new List<int>();
      PrimitiveType type = PrimitiveType.TriangleList;
      for (int i = fromGroup + count - 1; i >= fromGroup; --i)
      {
        Group group = this.Groups[i];
        group.BakeTransformWithNormal<T>();
        IndexedUserPrimitives<T> indexedUserPrimitives = group.Geometry as IndexedUserPrimitives<T>;
        list.AddRange(Enumerable.Select<int, int>((IEnumerable<int>) indexedUserPrimitives.Indices, (Func<int, int>) (x => x + vertices.Count)));
        vertices.AddRange((IEnumerable<T>) indexedUserPrimitives.Vertices);
        type = indexedUserPrimitives.PrimitiveType;
        this.RemoveGroupAt(i);
      }
      Group group1 = this.AddGroup();
      group1.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<T>(vertices.ToArray(), list.ToArray(), type);
      return group1;
    }

    public Group CollapseToBuffer<T>() where T : struct, IVertex
    {
      return this.CollapseToBuffer<T>(0, this.Groups.Count);
    }

    public Group CollapseToBuffer<T>(int fromGroup, int count) where T : struct, IVertex
    {
      if (count == 0)
        return (Group) null;
      List<T> vertices = new List<T>();
      List<int> list = new List<int>();
      PrimitiveType type = PrimitiveType.TriangleList;
      for (int i = fromGroup + count - 1; i >= fromGroup; --i)
      {
        Group group = this.Groups[i];
        group.BakeTransform<T>();
        IndexedUserPrimitives<T> indexedUserPrimitives = group.Geometry as IndexedUserPrimitives<T>;
        list.AddRange(Enumerable.Select<int, int>((IEnumerable<int>) indexedUserPrimitives.Indices, (Func<int, int>) (x => x + vertices.Count)));
        vertices.AddRange((IEnumerable<T>) indexedUserPrimitives.Vertices);
        type = indexedUserPrimitives.PrimitiveType;
        this.RemoveGroupAt(i);
      }
      Group group1 = this.AddGroup();
      BufferedIndexedPrimitives<T> indexedPrimitives = new BufferedIndexedPrimitives<T>(vertices.ToArray(), list.ToArray(), type);
      indexedPrimitives.UpdateBuffers();
      group1.Geometry = (IIndexedPrimitiveCollection) indexedPrimitives;
      return group1;
    }

    public Group Collapse<T>() where T : struct, IVertex
    {
      return this.Collapse<T>(0, this.Groups.Count);
    }

    public Group Collapse<T>(int fromGroup, int count) where T : struct, IVertex
    {
      if (count == 0)
        return (Group) null;
      List<T> vertices = new List<T>();
      List<int> list = new List<int>();
      PrimitiveType type = PrimitiveType.TriangleList;
      for (int i = fromGroup + count - 1; i >= fromGroup; --i)
      {
        Group group = this.Groups[i];
        group.BakeTransform<T>();
        IndexedUserPrimitives<T> indexedUserPrimitives = group.Geometry as IndexedUserPrimitives<T>;
        list.AddRange(Enumerable.Select<int, int>((IEnumerable<int>) indexedUserPrimitives.Indices, (Func<int, int>) (x => x + vertices.Count)));
        vertices.AddRange((IEnumerable<T>) indexedUserPrimitives.Vertices);
        type = indexedUserPrimitives.PrimitiveType;
        this.RemoveGroupAt(i);
      }
      Group group1 = this.AddGroup();
      group1.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<T>(vertices.ToArray(), list.ToArray(), type);
      return group1;
    }

    private void RebuildWorld()
    {
      Matrix matrix1;
      Matrix matrix2;
      Matrix matrix3;
      if (this.RotateOffCenter)
      {
        matrix1 = Matrix.CreateScale(this.scale);
        matrix2 = Matrix.CreateTranslation(this.position);
        matrix3 = Matrix.CreateFromQuaternion(this.rotation);
      }
      else if (this.ScaleAfterRotation)
      {
        matrix1 = Matrix.CreateFromQuaternion(this.rotation);
        matrix2 = Matrix.CreateScale(this.scale);
        matrix3 = Matrix.CreateTranslation(this.position);
      }
      else
      {
        matrix1 = Matrix.CreateScale(this.scale);
        matrix2 = Matrix.CreateFromQuaternion(this.rotation);
        matrix3 = Matrix.CreateTranslation(this.position);
      }
      this.worldMatrix = matrix1 * matrix2 * matrix3;
      if (this.GroupLazyMatrices)
        return;
      foreach (Group group in this.Groups)
        group.RebuildCompositeWorld();
    }

    public void Draw()
    {
      if (!this.Enabled || this.Effect == null || this.Effect.IsDisposed)
        return;
      BlendingMode blendingMode = GraphicsDeviceExtensions.GetBlendCombiner(this.GraphicsDevice).BlendingMode;
      if (!this.SkipGroupCheck)
      {
        bool flag = false;
        for (int index = 0; index < this.Groups.Count && !flag; ++index)
          flag = flag | this.Groups[index].Enabled;
        if (!flag)
          return;
      }
      this.Effect.Prepare(this);
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      if (!this.SkipStates)
      {
        GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferFunction = this.AlwaysOnTop ? CompareFunction.Always : CompareFunction.LessEqual;
        GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferWriteEnable = this.DepthWrites;
        GraphicsDeviceExtensions.SetCullMode(graphicsDevice, this.Culling);
        if (this.Blending.HasValue)
          GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, this.Blending.Value);
        if (this.SamplerState != null)
        {
          for (int index = 0; index < this.UsedSamplers; ++index)
            graphicsDevice.SamplerStates[index] = this.SamplerState;
        }
      }
      if (this.CustomRenderingHandler == null)
      {
        if (this._groupOrder != Mesh.DefaultOrder && this.groupsDirty)
        {
          this.Groups.Sort(this._groupOrder);
          this.groupsDirty = false;
        }
        foreach (Group group in this.Groups)
        {
          if (group.Enabled)
          {
            group.Draw(this.Effect);
            if (!this.SkipStates)
            {
              if (group.SamplerState != null && this.SamplerState != null)
              {
                for (int index = 0; index < this.UsedSamplers; ++index)
                  graphicsDevice.SamplerStates[index] = this.SamplerState;
              }
              if (group.Blending.HasValue)
                GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, !this.Blending.HasValue ? blendingMode : this.Blending.Value);
              if (group.CullMode.HasValue)
                GraphicsDeviceExtensions.SetCullMode(graphicsDevice, this.Culling);
              if (group.AlwaysOnTop.HasValue)
                GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferFunction = this.AlwaysOnTop ? CompareFunction.Always : CompareFunction.LessEqual;
              if (group.NoAlphaWrite.HasValue)
                GraphicsDeviceExtensions.GetBlendCombiner(graphicsDevice).ColorWriteChannels = ColorWriteChannels.All;
            }
          }
        }
      }
      else
        this.CustomRenderingHandler(this, this.Effect);
      if (this.SkipStates)
        return;
      if (this.Blending.HasValue && this.Blending.Value != blendingMode)
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, blendingMode);
      if (!this.DepthWrites)
      {
        GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferWriteEnable = true;
        GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).Apply(graphicsDevice);
      }
      GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).Apply(graphicsDevice);
      GraphicsDeviceExtensions.GetBlendCombiner(graphicsDevice).Apply(graphicsDevice);
    }

    public Mesh Clone()
    {
      Mesh mesh = new Mesh()
      {
        AlwaysOnTop = this.AlwaysOnTop,
        CustomRenderingHandler = this.CustomRenderingHandler,
        DepthWrites = this.DepthWrites,
        Effect = this.Effect.Clone(),
        RotateOffCenter = this.RotateOffCenter,
        WorldMatrix = this.WorldMatrix,
        TextureMatrix = this.TextureMatrix,
        ScaleAfterRotation = this.ScaleAfterRotation,
        Texture = this.Texture,
        SamplerState = this.SamplerState,
        UsedSamplers = this.UsedSamplers,
        Blending = this.Blending,
        Culling = this.Culling,
        Material = this.Material == null ? (Material) null : this.Material.Clone()
      };
      foreach (Group group1 in this.Groups)
      {
        Group group2 = group1.Clone(mesh);
        mesh.Groups.Add(group2);
      }
      return mesh;
    }

    public void ClearGroups()
    {
      foreach (Group group in this.Groups)
      {
        if (group.Geometry is IDisposable)
          (group.Geometry as IDisposable).Dispose();
        if (group.Geometry is IFakeDisposable)
          (group.Geometry as IFakeDisposable).Dispose();
      }
      this.Groups.Clear();
      this.groupsDirty = true;
    }

    public void RemoveGroup(Group group)
    {
      this.RemoveGroup(group, false);
    }

    public void RemoveGroup(Group group, bool skipDispose)
    {
      if (!skipDispose)
      {
        if (group.Geometry is IDisposable)
          (group.Geometry as IDisposable).Dispose();
        if (group.Geometry is IFakeDisposable)
          (group.Geometry as IFakeDisposable).Dispose();
      }
      this.Groups.Remove(group);
      this.groupsDirty = true;
    }

    public void RemoveGroupAt(int i)
    {
      if (this.Groups[i].Geometry is IDisposable)
        (this.Groups[i].Geometry as IDisposable).Dispose();
      if (this.Groups[i].Geometry is IFakeDisposable)
        (this.Groups[i].Geometry as IFakeDisposable).Dispose();
      this.Groups.RemoveAt(i);
      this.groupsDirty = true;
    }

    public Group AddGroup()
    {
      Group group = new Group(this, this.Groups.Count);
      this.Groups.Add(group);
      this.groupsDirty = true;
      return group;
    }

    public Group CloneGroup(Group group)
    {
      Group group1 = group.Clone(this);
      this.Groups.Add(group1);
      this.groupsDirty = true;
      return group1;
    }

    public Group AddTexturedCylinder(Vector3 size, Vector3 origin, int stacks, int slices, bool centeredOnOrigin)
    {
      return this.AddTexturedCylinder(size, origin, stacks, slices, centeredOnOrigin, true);
    }

    public Group AddTexturedCylinder(Vector3 size, Vector3 origin, int stacks, int slices, bool centeredOnOrigin, bool capped)
    {
      size /= 2f;
      if (!centeredOnOrigin)
        origin += size;
      Group group = new Group(this, this.Groups.Count);
      List<FezVertexPositionNormalTexture> list1 = new List<FezVertexPositionNormalTexture>();
      for (int index1 = 0; index1 <= stacks; ++index1)
      {
        if (capped || index1 != 0 && index1 != stacks)
        {
          int num1 = MathHelper.Clamp(index1 - 1, 0, stacks - 2) / (stacks - 2) * 2 - 1;
          int num2 = index1 == 0 || index1 == stacks ? 0 : 1;
          for (int index2 = 0; index2 <= slices; ++index2)
          {
            float x = (float) index2 / (float) slices;
            float num3 = x * 6.283185f;
            list1.Add(new FezVertexPositionNormalTexture(new Vector3((float) Math.Sin((double) num3) * (float) num2, (float) num1, (float) Math.Cos((double) num3) * (float) num2) * size + origin, new Vector3((float) Math.Sin((double) num3), 0.0f, (float) Math.Cos((double) num3)), new Vector2(x, (float) (1 - (num1 + 1) / 2))));
          }
        }
      }
      List<int> list2 = new List<int>();
      for (int index1 = 0; index1 < stacks; ++index1)
      {
        if (capped || index1 != 0 && index1 != stacks - 1)
        {
          int num = index1;
          if (!capped)
            --num;
          for (int index2 = 0; index2 <= slices; ++index2)
          {
            list2.Add(num * (slices + 1) + index2);
            list2.Add((num + 1) * (slices + 1) + index2);
          }
        }
      }
      group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionNormalTexture>(list1.ToArray(), list2.ToArray(), PrimitiveType.TriangleStrip);
      this.Groups.Add(group);
      return group;
    }

    public Group AddColoredBox(Vector3 size, Vector3 origin, Color color, bool centeredOnOrigin)
    {
      size /= 2f;
      if (!centeredOnOrigin)
        origin += size;
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(new FezVertexPositionColor[8]
        {
          new FezVertexPositionColor(new Vector3(-1f, -1f, -1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(1f, -1f, -1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(1f, 1f, -1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(-1f, 1f, -1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(-1f, -1f, 1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(1f, -1f, 1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(1f, 1f, 1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(-1f, 1f, 1f) * size + origin, color)
        }, new int[36]
        {
          0,
          1,
          2,
          0,
          2,
          3,
          1,
          5,
          6,
          1,
          6,
          2,
          0,
          7,
          4,
          0,
          3,
          7,
          3,
          2,
          6,
          3,
          6,
          7,
          4,
          6,
          5,
          4,
          7,
          6,
          0,
          5,
          1,
          0,
          4,
          5
        }, PrimitiveType.TriangleList)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddWireframeBox(Vector3 size, Vector3 origin, Color color, bool centeredOnOrigin)
    {
      size /= 2f;
      if (!centeredOnOrigin)
        origin += size;
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(new FezVertexPositionColor[8]
        {
          new FezVertexPositionColor(new Vector3(-1f, -1f, -1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(1f, -1f, -1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(1f, 1f, -1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(-1f, 1f, -1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(-1f, -1f, 1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(1f, -1f, 1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(1f, 1f, 1f) * size + origin, color),
          new FezVertexPositionColor(new Vector3(-1f, 1f, 1f) * size + origin, color)
        }, new int[24]
        {
          0,
          1,
          1,
          2,
          2,
          3,
          3,
          0,
          4,
          5,
          5,
          6,
          6,
          7,
          7,
          4,
          0,
          4,
          1,
          5,
          2,
          6,
          3,
          7
        }, PrimitiveType.LineList)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddFlatShadedBox(Vector3 size, Vector3 origin, Color color, bool centeredOnOrigin)
    {
      size /= 2f;
      if (!centeredOnOrigin)
        origin += size;
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalColor>(new VertexPositionNormalColor[24]
        {
          new VertexPositionNormalColor(new Vector3(-1f, -1f, -1f) * size + origin, -Vector3.UnitZ, color),
          new VertexPositionNormalColor(new Vector3(-1f, 1f, -1f) * size + origin, -Vector3.UnitZ, color),
          new VertexPositionNormalColor(new Vector3(1f, 1f, -1f) * size + origin, -Vector3.UnitZ, color),
          new VertexPositionNormalColor(new Vector3(1f, -1f, -1f) * size + origin, -Vector3.UnitZ, color),
          new VertexPositionNormalColor(new Vector3(1f, -1f, -1f) * size + origin, Vector3.UnitX, color),
          new VertexPositionNormalColor(new Vector3(1f, 1f, -1f) * size + origin, Vector3.UnitX, color),
          new VertexPositionNormalColor(new Vector3(1f, 1f, 1f) * size + origin, Vector3.UnitX, color),
          new VertexPositionNormalColor(new Vector3(1f, -1f, 1f) * size + origin, Vector3.UnitX, color),
          new VertexPositionNormalColor(new Vector3(1f, -1f, 1f) * size + origin, Vector3.UnitZ, color),
          new VertexPositionNormalColor(new Vector3(1f, 1f, 1f) * size + origin, Vector3.UnitZ, color),
          new VertexPositionNormalColor(new Vector3(-1f, 1f, 1f) * size + origin, Vector3.UnitZ, color),
          new VertexPositionNormalColor(new Vector3(-1f, -1f, 1f) * size + origin, Vector3.UnitZ, color),
          new VertexPositionNormalColor(new Vector3(-1f, -1f, 1f) * size + origin, -Vector3.UnitX, color),
          new VertexPositionNormalColor(new Vector3(-1f, 1f, 1f) * size + origin, -Vector3.UnitX, color),
          new VertexPositionNormalColor(new Vector3(-1f, 1f, -1f) * size + origin, -Vector3.UnitX, color),
          new VertexPositionNormalColor(new Vector3(-1f, -1f, -1f) * size + origin, -Vector3.UnitX, color),
          new VertexPositionNormalColor(new Vector3(-1f, -1f, -1f) * size + origin, -Vector3.UnitY, color),
          new VertexPositionNormalColor(new Vector3(-1f, -1f, 1f) * size + origin, -Vector3.UnitY, color),
          new VertexPositionNormalColor(new Vector3(1f, -1f, 1f) * size + origin, -Vector3.UnitY, color),
          new VertexPositionNormalColor(new Vector3(1f, -1f, -1f) * size + origin, -Vector3.UnitY, color),
          new VertexPositionNormalColor(new Vector3(-1f, 1f, -1f) * size + origin, Vector3.UnitY, color),
          new VertexPositionNormalColor(new Vector3(-1f, 1f, 1f) * size + origin, Vector3.UnitY, color),
          new VertexPositionNormalColor(new Vector3(1f, 1f, 1f) * size + origin, Vector3.UnitY, color),
          new VertexPositionNormalColor(new Vector3(1f, 1f, -1f) * size + origin, Vector3.UnitY, color)
        }, new int[36]
        {
          0,
          2,
          1,
          0,
          3,
          2,
          4,
          6,
          5,
          4,
          7,
          6,
          8,
          10,
          9,
          8,
          11,
          10,
          12,
          14,
          13,
          12,
          15,
          14,
          16,
          17,
          18,
          16,
          18,
          19,
          20,
          22,
          21,
          20,
          23,
          22
        }, PrimitiveType.TriangleList)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddCubemappedBox(Vector3 size, Vector3 origin, bool centeredOnOrigin)
    {
      size /= 2f;
      if (!centeredOnOrigin)
        origin += size;
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionNormalTexture>(new FezVertexPositionNormalTexture[24]
        {
          new FezVertexPositionNormalTexture(new Vector3(-1f, -1f, -1f) * size + origin, -Vector3.UnitZ),
          new FezVertexPositionNormalTexture(new Vector3(-1f, 1f, -1f) * size + origin, -Vector3.UnitZ),
          new FezVertexPositionNormalTexture(new Vector3(1f, 1f, -1f) * size + origin, -Vector3.UnitZ),
          new FezVertexPositionNormalTexture(new Vector3(1f, -1f, -1f) * size + origin, -Vector3.UnitZ),
          new FezVertexPositionNormalTexture(new Vector3(1f, -1f, -1f) * size + origin, Vector3.UnitX),
          new FezVertexPositionNormalTexture(new Vector3(1f, 1f, -1f) * size + origin, Vector3.UnitX),
          new FezVertexPositionNormalTexture(new Vector3(1f, 1f, 1f) * size + origin, Vector3.UnitX),
          new FezVertexPositionNormalTexture(new Vector3(1f, -1f, 1f) * size + origin, Vector3.UnitX),
          new FezVertexPositionNormalTexture(new Vector3(1f, -1f, 1f) * size + origin, Vector3.UnitZ),
          new FezVertexPositionNormalTexture(new Vector3(1f, 1f, 1f) * size + origin, Vector3.UnitZ),
          new FezVertexPositionNormalTexture(new Vector3(-1f, 1f, 1f) * size + origin, Vector3.UnitZ),
          new FezVertexPositionNormalTexture(new Vector3(-1f, -1f, 1f) * size + origin, Vector3.UnitZ),
          new FezVertexPositionNormalTexture(new Vector3(-1f, -1f, 1f) * size + origin, -Vector3.UnitX),
          new FezVertexPositionNormalTexture(new Vector3(-1f, 1f, 1f) * size + origin, -Vector3.UnitX),
          new FezVertexPositionNormalTexture(new Vector3(-1f, 1f, -1f) * size + origin, -Vector3.UnitX),
          new FezVertexPositionNormalTexture(new Vector3(-1f, -1f, -1f) * size + origin, -Vector3.UnitX),
          new FezVertexPositionNormalTexture(new Vector3(-1f, -1f, -1f) * size + origin, -Vector3.UnitY),
          new FezVertexPositionNormalTexture(new Vector3(-1f, -1f, 1f) * size + origin, -Vector3.UnitY),
          new FezVertexPositionNormalTexture(new Vector3(1f, -1f, 1f) * size + origin, -Vector3.UnitY),
          new FezVertexPositionNormalTexture(new Vector3(1f, -1f, -1f) * size + origin, -Vector3.UnitY),
          new FezVertexPositionNormalTexture(new Vector3(-1f, 1f, -1f) * size + origin, Vector3.UnitY),
          new FezVertexPositionNormalTexture(new Vector3(-1f, 1f, 1f) * size + origin, Vector3.UnitY),
          new FezVertexPositionNormalTexture(new Vector3(1f, 1f, 1f) * size + origin, Vector3.UnitY),
          new FezVertexPositionNormalTexture(new Vector3(1f, 1f, -1f) * size + origin, Vector3.UnitY)
        }, new int[36]
        {
          0,
          2,
          1,
          0,
          3,
          2,
          4,
          6,
          5,
          4,
          7,
          6,
          8,
          10,
          9,
          8,
          11,
          10,
          12,
          14,
          13,
          12,
          15,
          14,
          16,
          17,
          18,
          16,
          18,
          19,
          20,
          22,
          21,
          20,
          23,
          22
        }, PrimitiveType.TriangleList)
      };
      IndexedUserPrimitives<FezVertexPositionNormalTexture> indexedUserPrimitives = group.Geometry as IndexedUserPrimitives<FezVertexPositionNormalTexture>;
      for (int index = 0; index < indexedUserPrimitives.Vertices.Length; ++index)
        indexedUserPrimitives.Vertices[index].TextureCoordinate = FezMath.ComputeTexCoord<FezVertexPositionNormalTexture>(indexedUserPrimitives.Vertices[index]);
      this.Groups.Add(group);
      return group;
    }

    public Group AddLine(Vector3 origin, Vector3 destination, Color color)
    {
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(new FezVertexPositionColor[2]
        {
          new FezVertexPositionColor(origin, color),
          new FezVertexPositionColor(destination, color)
        }, new int[2]
        {
          0,
          1
        }, PrimitiveType.LineList)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddLine(Vector3 origin, Vector3 destination, Color colorA, Color colorB)
    {
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(new FezVertexPositionColor[2]
        {
          new FezVertexPositionColor(origin, colorA),
          new FezVertexPositionColor(destination, colorB)
        }, new int[2]
        {
          0,
          1
        }, PrimitiveType.LineList)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddLines(Color[] pointColors, Vector3[] pointPairs, bool buffered)
    {
      FezVertexPositionColor[] vertices = new FezVertexPositionColor[pointColors.Length];
      int[] indices = new int[pointColors.Length];
      for (int index = 0; index < pointColors.Length; ++index)
      {
        vertices[index] = new FezVertexPositionColor(pointPairs[index], pointColors[index]);
        indices[index] = index;
      }
      IIndexedPrimitiveCollection primitiveCollection;
      if (buffered)
      {
        primitiveCollection = (IIndexedPrimitiveCollection) new BufferedIndexedPrimitives<FezVertexPositionColor>(vertices, indices, PrimitiveType.LineList);
        (primitiveCollection as BufferedIndexedPrimitives<FezVertexPositionColor>).UpdateBuffers();
        (primitiveCollection as BufferedIndexedPrimitives<FezVertexPositionColor>).CleanUp();
      }
      else
        primitiveCollection = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(vertices, indices, PrimitiveType.LineList);
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = primitiveCollection
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddLines(Color[] pointColors, params Vector3[] pointPairs)
    {
      return this.AddLines(pointColors, pointPairs, false);
    }

    public Group AddWireframePolygon(Color color, params Vector3[] points)
    {
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(Enumerable.ToArray<FezVertexPositionColor>(Enumerable.Select<Vector3, FezVertexPositionColor>((IEnumerable<Vector3>) points, (Func<Vector3, FezVertexPositionColor>) (x => new FezVertexPositionColor(x, color)))), Enumerable.ToArray<int>(Enumerable.Skip<int>(Enumerable.SelectMany<Vector3, int>((IEnumerable<Vector3>) points, (Func<Vector3, int, IEnumerable<int>>) ((x, i) => (IEnumerable<int>) new int[2]
        {
          i - 1,
          i
        })), 2)), PrimitiveType.LineList)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddWireframeArrow(float size, float arrowSize, Vector3 origin, FaceOrientation direction, Color color)
    {
      Vector3 direction1 = FezMath.AsVector(direction);
      return this.AddWireframeArrow(size, arrowSize, origin, direction1, color);
    }

    public Group AddWireframeArrow(float size, float arrowSize, Vector3 origin, Vector3 direction, Color color)
    {
      direction.Normalize();
      Vector3 vector3 = FezMath.AlmostEqual(FezMath.Abs(direction), Vector3.UnitX) ? Vector3.Cross(direction, Vector3.UnitZ) : Vector3.Cross(direction, Vector3.UnitX);
      vector3.Normalize();
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(new FezVertexPositionColor[4]
        {
          new FezVertexPositionColor(origin, color),
          new FezVertexPositionColor(direction * size + origin, color),
          new FezVertexPositionColor(direction * (size - arrowSize) + vector3 * arrowSize + origin, color),
          new FezVertexPositionColor(direction * (size - arrowSize) - vector3 * arrowSize + origin, color)
        }, new int[6]
        {
          0,
          1,
          1,
          2,
          1,
          3
        }, PrimitiveType.LineList)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddColoredQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Color aColor, Color bColor, Color cColor, Color dColor)
    {
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(new FezVertexPositionColor[4]
        {
          new FezVertexPositionColor(a, aColor),
          new FezVertexPositionColor(b, bColor),
          new FezVertexPositionColor(c, cColor),
          new FezVertexPositionColor(d, dColor)
        }, new int[6]
        {
          0,
          1,
          2,
          1,
          0,
          3
        }, PrimitiveType.TriangleList)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddColoredTriangle(Vector3 a, Vector3 b, Vector3 c, Color aColor, Color bColor, Color cColor)
    {
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(new FezVertexPositionColor[3]
        {
          new FezVertexPositionColor(a, aColor),
          new FezVertexPositionColor(b, bColor),
          new FezVertexPositionColor(c, cColor)
        }, new int[3]
        {
          0,
          1,
          2
        }, PrimitiveType.TriangleList)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddFace(Vector3 size, Vector3 origin, FaceOrientation face, Color color, bool centeredOnOrigin)
    {
      return this.AddFace(size, origin, face, color, centeredOnOrigin, true, false);
    }

    public Group AddFace(Vector3 size, Vector3 origin, FaceOrientation face, Color color, bool centeredOnOrigin, bool doublesided)
    {
      return this.AddFace(size, origin, face, color, centeredOnOrigin, true, doublesided, false);
    }

    public Group AddFace(Vector3 size, Vector3 origin, FaceOrientation face, Color color, bool centeredOnOrigin, bool doublesided, bool crosshatch)
    {
      return this.AddFace(size, origin, face, color, centeredOnOrigin, true, doublesided, crosshatch);
    }

    public Group AddFace(Vector3 size, Vector3 origin, FaceOrientation face, bool centeredOnOrigin)
    {
      return this.AddFace(size, origin, face, centeredOnOrigin, false);
    }

    public Group AddFace(Vector3 size, Vector3 origin, FaceOrientation face, bool centeredOnOrigin, bool doublesided)
    {
      return this.AddFace(size, origin, face, Color.White, centeredOnOrigin, false, doublesided, false);
    }

    public Group AddFace(Vector3 size, Vector3 origin, FaceOrientation face, bool centeredOnOrigin, bool doublesided, bool crosshatch)
    {
      return this.AddFace(size, origin, face, Color.White, centeredOnOrigin, false, doublesided, crosshatch);
    }

    private Group AddFace(Vector3 size, Vector3 origin, FaceOrientation face, Color color, bool centeredOnOrigin, bool colored, bool doublesided, bool crosshatch)
    {
      Vector3 normal1 = FezMath.AsVector(face);
      FaceOrientation tangent1 = FezMath.GetTangent(face);
      FaceOrientation bitangent1 = FezMath.GetBitangent(face);
      Vector3 vector3_1 = FezMath.AsVector(tangent1);
      Vector3 vector3_2 = FezMath.AsVector(bitangent1);
      if (centeredOnOrigin)
        origin -= (vector3_1 + vector3_2) * size / 2f;
      Vector3 position1 = origin;
      Vector3 position2 = origin + vector3_1 * size;
      Vector3 position3 = origin + (vector3_1 + vector3_2) * size;
      Vector3 position4 = origin + vector3_2 * size;
      Group group = new Group(this, this.Groups.Count);
      this.Groups.Add(group);
      if (colored)
      {
        Mesh.AddFace<VertexPositionNormalColor>(group, face, new VertexPositionNormalColor(position1, normal1, color), new VertexPositionNormalColor(position2, normal1, color), new VertexPositionNormalColor(position3, normal1, color), new VertexPositionNormalColor(position4, normal1, color));
        if (doublesided)
          Mesh.AddFace<VertexPositionNormalColor>(group, face, new VertexPositionNormalColor(position1, -normal1, color), new VertexPositionNormalColor(position4, -normal1, color), new VertexPositionNormalColor(position3, -normal1, color), new VertexPositionNormalColor(position2, -normal1, color));
        if (crosshatch)
        {
          if (centeredOnOrigin)
            origin += (vector3_1 + vector3_2) * size / 2f;
          face = FezMath.IsSide(FezMath.GetTangent(face)) ? FezMath.GetTangent(face) : FezMath.GetBitangent(face);
          float num = size.Z;
          size.Z = size.X;
          size.X = num;
          Vector3 normal2 = FezMath.AsVector(face);
          FaceOrientation tangent2 = FezMath.GetTangent(face);
          FaceOrientation bitangent2 = FezMath.GetBitangent(face);
          Vector3 vector3_3 = FezMath.AsVector(tangent2);
          Vector3 vector3_4 = FezMath.AsVector(bitangent2);
          if (centeredOnOrigin)
            origin -= (vector3_3 + vector3_4) * size / 2f;
          Vector3 position5 = origin;
          Vector3 position6 = origin + vector3_3 * size;
          Vector3 position7 = origin + (vector3_3 + vector3_4) * size;
          Vector3 position8 = origin + vector3_4 * size;
          Mesh.AddFace<VertexPositionNormalColor>(group, face, new VertexPositionNormalColor(position5, normal2, color), new VertexPositionNormalColor(position6, normal2, color), new VertexPositionNormalColor(position7, normal2, color), new VertexPositionNormalColor(position8, normal2, color));
          if (doublesided)
            Mesh.AddFace<VertexPositionNormalColor>(group, face, new VertexPositionNormalColor(position5, -normal2, color), new VertexPositionNormalColor(position8, -normal2, color), new VertexPositionNormalColor(position7, -normal2, color), new VertexPositionNormalColor(position6, -normal2, color));
        }
      }
      else
      {
        Vector2 vector2_1 = FezMath.AsAxis(tangent1) == Axis.Y ? Vector2.UnitY : Vector2.UnitX;
        Vector2 vector2_2 = Vector2.One - vector2_1;
        Vector2 vector2_3 = new Vector2(FezMath.AsAxis(tangent1) == Axis.Y ? (float) FezMath.AsNumeric(!FezMath.IsPositive(face)) : (float) FezMath.AsNumeric(FezMath.IsPositive(face)), 1f);
        Vector2 texCoord1 = vector2_3;
        Vector2 texCoord2 = FezMath.Abs(vector2_1 - vector2_3);
        Vector2 texCoord3 = FezMath.Abs(Vector2.One - vector2_3);
        Vector2 texCoord4 = FezMath.Abs(vector2_2 - vector2_3);
        Mesh.AddFace<FezVertexPositionNormalTexture>(group, face, new FezVertexPositionNormalTexture(position1, normal1, texCoord1), new FezVertexPositionNormalTexture(position2, normal1, texCoord2), new FezVertexPositionNormalTexture(position3, normal1, texCoord3), new FezVertexPositionNormalTexture(position4, normal1, texCoord4));
        if (doublesided)
          Mesh.AddFace<FezVertexPositionNormalTexture>(group, face, new FezVertexPositionNormalTexture(position1, -normal1, texCoord1), new FezVertexPositionNormalTexture(position4, -normal1, texCoord4), new FezVertexPositionNormalTexture(position3, -normal1, texCoord3), new FezVertexPositionNormalTexture(position2, -normal1, texCoord2));
        if (crosshatch)
        {
          if (centeredOnOrigin)
            origin += (vector3_1 + vector3_2) * size / 2f;
          face = FezMath.IsSide(FezMath.GetTangent(face)) ? FezMath.GetTangent(face) : FezMath.GetBitangent(face);
          float num = size.Z;
          size.Z = size.X;
          size.X = num;
          Vector3 normal2 = FezMath.AsVector(face);
          FaceOrientation tangent2 = FezMath.GetTangent(face);
          FaceOrientation bitangent2 = FezMath.GetBitangent(face);
          Vector3 vector3_3 = FezMath.AsVector(tangent2);
          Vector3 vector3_4 = FezMath.AsVector(bitangent2);
          if (centeredOnOrigin)
            origin -= (vector3_3 + vector3_4) * size / 2f;
          Vector3 position5 = origin;
          Vector3 position6 = origin + vector3_3 * size;
          Vector3 position7 = origin + (vector3_3 + vector3_4) * size;
          Vector3 position8 = origin + vector3_4 * size;
          Vector2 vector2_4 = FezMath.AsAxis(tangent2) == Axis.Y ? Vector2.UnitY : Vector2.UnitX;
          Vector2 vector2_5 = Vector2.One - vector2_4;
          vector2_3 = new Vector2(FezMath.AsAxis(tangent2) == Axis.Y ? (float) FezMath.AsNumeric(!FezMath.IsPositive(face)) : (float) FezMath.AsNumeric(FezMath.IsPositive(face)), 1f);
          Vector2 texCoord5 = vector2_3;
          Vector2 texCoord6 = FezMath.Abs(vector2_4 - vector2_3);
          Vector2 texCoord7 = FezMath.Abs(Vector2.One - vector2_3);
          Vector2 texCoord8 = FezMath.Abs(vector2_5 - vector2_3);
          Mesh.AddFace<FezVertexPositionNormalTexture>(group, face, new FezVertexPositionNormalTexture(position5, normal2, texCoord5), new FezVertexPositionNormalTexture(position6, normal2, texCoord6), new FezVertexPositionNormalTexture(position7, normal2, texCoord7), new FezVertexPositionNormalTexture(position8, normal2, texCoord8));
          if (doublesided)
            Mesh.AddFace<FezVertexPositionNormalTexture>(group, face, new FezVertexPositionNormalTexture(position5, -normal2, texCoord5), new FezVertexPositionNormalTexture(position8, -normal2, texCoord8), new FezVertexPositionNormalTexture(position7, -normal2, texCoord7), new FezVertexPositionNormalTexture(position6, -normal2, texCoord6));
        }
      }
      return group;
    }

    private static void AddFace<TVertex>(Group group, FaceOrientation face, TVertex v0, TVertex v1, TVertex v2, TVertex v3) where TVertex : struct, IVertex, IEquatable<TVertex>
    {
      FaceMaterialization<TVertex> faceMaterialization = new FaceMaterialization<TVertex>()
      {
        V0 = new SharedVertex<TVertex>()
        {
          Vertex = v0,
          Index = 0
        },
        V1 = new SharedVertex<TVertex>()
        {
          Vertex = v1,
          Index = 1
        },
        V2 = new SharedVertex<TVertex>()
        {
          Vertex = v2,
          Index = 2
        },
        V3 = new SharedVertex<TVertex>()
        {
          Vertex = v3,
          Index = 3
        }
      };
      faceMaterialization.SetupIndices(face);
      IndexedUserPrimitives<TVertex> indexedUserPrimitives = group.Geometry as IndexedUserPrimitives<TVertex>;
      if (indexedUserPrimitives == null)
      {
        group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<TVertex>(new TVertex[4]
        {
          faceMaterialization.V0.Vertex,
          faceMaterialization.V1.Vertex,
          faceMaterialization.V2.Vertex,
          faceMaterialization.V3.Vertex
        }, new int[6]
        {
          faceMaterialization.GetIndex((ushort) 0),
          faceMaterialization.GetIndex((ushort) 1),
          faceMaterialization.GetIndex((ushort) 2),
          faceMaterialization.GetIndex((ushort) 3),
          faceMaterialization.GetIndex((ushort) 4),
          faceMaterialization.GetIndex((ushort) 5)
        }, PrimitiveType.TriangleList);
      }
      else
      {
        int length = indexedUserPrimitives.Vertices.Length;
        indexedUserPrimitives.Vertices = Util.JoinArrays<TVertex>(indexedUserPrimitives.Vertices, new TVertex[4]
        {
          faceMaterialization.V0.Vertex,
          faceMaterialization.V1.Vertex,
          faceMaterialization.V2.Vertex,
          faceMaterialization.V3.Vertex
        });
        indexedUserPrimitives.Indices = Util.JoinArrays<int>(indexedUserPrimitives.Indices, new int[6]
        {
          faceMaterialization.GetIndex((ushort) 0) + length,
          faceMaterialization.GetIndex((ushort) 1) + length,
          faceMaterialization.GetIndex((ushort) 2) + length,
          faceMaterialization.GetIndex((ushort) 3) + length,
          faceMaterialization.GetIndex((ushort) 4) + length,
          faceMaterialization.GetIndex((ushort) 5) + length
        });
      }
    }

    public Group AddWireframeFace(Vector3 size, Vector3 origin, FaceOrientation face, Color color, bool centeredOnOrigin)
    {
      if (centeredOnOrigin)
        origin -= size / 2f;
      Vector3 vector3_1 = FezMath.AsVector(face) * size;
      Vector3 vector3_2 = FezMath.AsVector(FezMath.GetTangent(face)) * size;
      Vector3 vector3_3 = FezMath.AsVector(FezMath.GetBitangent(face)) * size;
      origin += (FezMath.IsPositive(face) ? 1f : 0.0f) * vector3_1;
      Group group = new Group(this, this.Groups.Count)
      {
        Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(new FezVertexPositionColor[4]
        {
          new FezVertexPositionColor(origin, color),
          new FezVertexPositionColor(origin + vector3_2, color),
          new FezVertexPositionColor(origin + vector3_2 + vector3_3, color),
          new FezVertexPositionColor(origin + vector3_3, color)
        }, new int[5]
        {
          0,
          1,
          2,
          3,
          0
        }, PrimitiveType.LineStrip)
      };
      this.Groups.Add(group);
      return group;
    }

    public Group AddPoints(Color color, IEnumerable<Vector3> points, bool buffered)
    {
      Group group;
      if (buffered)
      {
        group = new Group(this, this.Groups.Count)
        {
          Geometry = (IIndexedPrimitiveCollection) new BufferedIndexedPrimitives<FezVertexPositionColor>(Enumerable.ToArray<FezVertexPositionColor>(Enumerable.SelectMany<Vector3, FezVertexPositionColor>(points, (Func<Vector3, IEnumerable<FezVertexPositionColor>>) (p => (IEnumerable<FezVertexPositionColor>) new FezVertexPositionColor[2]
          {
            new FezVertexPositionColor(p, new Color((int) color.R, (int) color.G, (int) color.B, 0)),
            new FezVertexPositionColor(p, new Color((int) color.R, (int) color.G, (int) color.B, (int) byte.MaxValue))
          }))), Enumerable.ToArray<int>(Enumerable.SelectMany<Vector3, int>(points, (Func<Vector3, int, IEnumerable<int>>) ((p, i) => (IEnumerable<int>) new int[2]
          {
            i * 2,
            i * 2 + 1
          }))), PrimitiveType.LineList)
        };
        (group.Geometry as BufferedIndexedPrimitives<FezVertexPositionColor>).UpdateBuffers();
      }
      else
        group = new Group(this, this.Groups.Count)
        {
          Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(Enumerable.ToArray<FezVertexPositionColor>(Enumerable.SelectMany<Vector3, FezVertexPositionColor>(points, (Func<Vector3, IEnumerable<FezVertexPositionColor>>) (p => (IEnumerable<FezVertexPositionColor>) new FezVertexPositionColor[2]
          {
            new FezVertexPositionColor(p, new Color((int) color.R, (int) color.G, (int) color.B, 0)),
            new FezVertexPositionColor(p, new Color((int) color.R, (int) color.G, (int) color.B, (int) byte.MaxValue))
          }))), Enumerable.ToArray<int>(Enumerable.SelectMany<Vector3, int>(points, (Func<Vector3, int, IEnumerable<int>>) ((p, i) => (IEnumerable<int>) new int[2]
          {
            i * 2,
            i * 2 + 1
          }))), PrimitiveType.LineList)
        };
      this.Groups.Add(group);
      return group;
    }

    public Group AddPoints(IList<Color> colors, IEnumerable<Vector3> points, bool buffered)
    {
      Group group;
      if (buffered)
      {
        group = new Group(this, this.Groups.Count)
        {
          Geometry = (IIndexedPrimitiveCollection) new BufferedIndexedPrimitives<FezVertexPositionColor>(Enumerable.ToArray<FezVertexPositionColor>(Enumerable.SelectMany<Vector3, FezVertexPositionColor>(points, (Func<Vector3, int, IEnumerable<FezVertexPositionColor>>) ((p, i) => (IEnumerable<FezVertexPositionColor>) new FezVertexPositionColor[2]
          {
            new FezVertexPositionColor(p, new Color((int) colors[i].R, (int) colors[i].G, (int) colors[i].B, 0)),
            new FezVertexPositionColor(p, new Color((int) colors[i].R, (int) colors[i].G, (int) colors[i].B, (int) byte.MaxValue))
          }))), Enumerable.ToArray<int>(Enumerable.SelectMany<Vector3, int>(points, (Func<Vector3, int, IEnumerable<int>>) ((p, i) => (IEnumerable<int>) new int[2]
          {
            i * 2,
            i * 2 + 1
          }))), PrimitiveType.LineList)
        };
        (group.Geometry as BufferedIndexedPrimitives<FezVertexPositionColor>).UpdateBuffers();
      }
      else
        group = new Group(this, this.Groups.Count)
        {
          Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(Enumerable.ToArray<FezVertexPositionColor>(Enumerable.SelectMany<Vector3, FezVertexPositionColor>(points, (Func<Vector3, int, IEnumerable<FezVertexPositionColor>>) ((p, i) => (IEnumerable<FezVertexPositionColor>) new FezVertexPositionColor[2]
          {
            new FezVertexPositionColor(p, new Color((int) colors[i].R, (int) colors[i].G, (int) colors[i].B, 0)),
            new FezVertexPositionColor(p, new Color((int) colors[i].R, (int) colors[i].G, (int) colors[i].B, (int) byte.MaxValue))
          }))), Enumerable.ToArray<int>(Enumerable.SelectMany<Vector3, int>(points, (Func<Vector3, int, IEnumerable<int>>) ((p, i) => (IEnumerable<int>) new int[2]
          {
            i * 2,
            i * 2 + 1
          }))), PrimitiveType.LineList)
        };
      this.Groups.Add(group);
      return group;
    }

    public void Dispose()
    {
      this.Dispose(true);
    }

    public void Dispose(bool disposeEffect)
    {
      foreach (Group group in this.Groups)
      {
        if (group.Geometry is IDisposable)
          (group.Geometry as IDisposable).Dispose();
        if (group.Geometry is IFakeDisposable)
          (group.Geometry as IFakeDisposable).Dispose();
      }
      if (!disposeEffect)
        return;
      this.Effect.Dispose();
    }

    public delegate void RenderingHandler(Mesh mesh, BaseEffect effect);
  }
}
