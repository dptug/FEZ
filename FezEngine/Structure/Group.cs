// Type: FezEngine.Structure.Group
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Structure
{
  public class Group
  {
    private readonly Dirtyable<Matrix?> textureMatrix = new Dirtyable<Matrix?>();
    private Vector3 scale = Vector3.One;
    private Quaternion rotation = Quaternion.Identity;
    private readonly Dirtyable<Matrix> compositeWorldMatrix = new Dirtyable<Matrix>();
    private readonly Dirtyable<Matrix> inverseTransposeCompositeWorldMatrix = new Dirtyable<Matrix>();
    private Matrix worldMatrix;
    private Matrix translationMatrix;
    private Matrix scalingMatrix;
    private Matrix rotationMatrix;
    private Matrix scalingRotationMatrix;
    private Texture texture;
    private Vector3 position;

    public int Id { get; set; }

    public Mesh Mesh { get; set; }

    public bool RotateOffCenter { get; set; }

    public bool Enabled { get; set; }

    public Material Material { get; set; }

    public Dirtyable<Matrix?> TextureMatrix
    {
      get
      {
        return this.textureMatrix;
      }
      set
      {
        this.textureMatrix.Set((Matrix?) value);
      }
    }

    public bool EffectOwner
    {
      get
      {
        return this.Mesh.Groups.Count == 1;
      }
    }

    public CullMode? CullMode { get; set; }

    public bool? AlwaysOnTop { get; set; }

    public BlendingMode? Blending { get; set; }

    public SamplerState SamplerState { get; set; }

    public bool? NoAlphaWrite { get; set; }

    public TexturingType TexturingType { get; private set; }

    public Texture Texture
    {
      get
      {
        return this.texture;
      }
      set
      {
        this.texture = value;
        this.TexturingType = value is Texture2D ? TexturingType.Texture2D : (value is TextureCube ? TexturingType.Cubemap : TexturingType.None);
      }
    }

    public Texture2D TextureMap
    {
      get
      {
        return this.texture as Texture2D;
      }
    }

    public TextureCube CubeMap
    {
      get
      {
        return this.texture as TextureCube;
      }
    }

    public object CustomData { get; set; }

    public IIndexedPrimitiveCollection Geometry { get; set; }

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
        this.translationMatrix = Matrix.CreateTranslation(this.position);
        this.RebuildWorld(false);
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
        this.scalingMatrix = Matrix.CreateScale(this.scale);
        this.scalingRotationMatrix = this.scalingMatrix * this.rotationMatrix;
        this.RebuildWorld(true);
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
        this.rotationMatrix = Matrix.CreateFromQuaternion(this.rotation);
        this.scalingRotationMatrix = this.scalingMatrix * this.rotationMatrix;
        this.RebuildWorld(true);
      }
    }

    public Dirtyable<Matrix> WorldMatrix
    {
      get
      {
        return this.compositeWorldMatrix;
      }
      set
      {
        this.worldMatrix = (Matrix) value;
        this.worldMatrix.Decompose(out this.scale, out this.rotation, out this.position);
        this.translationMatrix = Matrix.CreateTranslation(this.position);
        this.scalingMatrix = Matrix.CreateScale(this.scale);
        this.rotationMatrix = Matrix.CreateFromQuaternion(this.rotation);
        this.scalingRotationMatrix = this.scalingMatrix * this.rotationMatrix;
        this.RebuildCompositeWorld(true);
      }
    }

    public Dirtyable<Matrix> InverseTransposeWorldMatrix
    {
      get
      {
        return this.inverseTransposeCompositeWorldMatrix;
      }
    }

    internal Group(Mesh mesh, int id)
    {
      this.Id = id;
      this.Mesh = mesh;
      this.Enabled = true;
      this.textureMatrix.Dirty = true;
      this.translationMatrix = this.scalingRotationMatrix = this.scalingMatrix = this.rotationMatrix = Matrix.Identity;
      this.worldMatrix = Matrix.Identity;
      this.RebuildCompositeWorld();
    }

    public void SetLazyScale(Vector3 scale)
    {
      this.scale = scale;
      this.scalingMatrix = Matrix.CreateScale(scale);
      this.scalingRotationMatrix = this.scalingMatrix * this.rotationMatrix;
    }

    public void SetLazyRotation(Quaternion r)
    {
      this.rotation = r;
      this.rotationMatrix = Matrix.CreateFromQuaternion(this.rotation);
      this.scalingRotationMatrix = this.scalingMatrix * this.rotationMatrix;
    }

    public void SetLazyPosition(Vector3 position)
    {
      this.position = position;
      this.translationMatrix = Matrix.CreateTranslation(position);
    }

    public void RecomputeMatrices()
    {
      this.RebuildWorld(true);
    }

    public void RecomputeMatrices(bool noInvert)
    {
      this.RebuildWorld(noInvert);
    }

    private void RebuildWorld(bool invert)
    {
      this.worldMatrix = !this.RotateOffCenter ? this.scalingRotationMatrix * this.translationMatrix : this.translationMatrix * this.scalingRotationMatrix;
      this.RebuildCompositeWorld(invert);
    }

    internal void RebuildCompositeWorld()
    {
      this.RebuildCompositeWorld(true);
    }

    internal void RebuildCompositeWorld(bool invert)
    {
      this.compositeWorldMatrix.Set(this.worldMatrix * this.Mesh.WorldMatrix);
      if (!invert)
        return;
      this.inverseTransposeCompositeWorldMatrix.Set(Matrix.Transpose(Matrix.Invert((Matrix) this.compositeWorldMatrix)));
    }

    public void Draw(BaseEffect effect)
    {
      if (this.Geometry == null)
        return;
      GraphicsDevice graphicsDevice = this.Mesh.GraphicsDevice;
      if (this.AlwaysOnTop.HasValue)
        GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferFunction = this.AlwaysOnTop.Value ? CompareFunction.Always : CompareFunction.LessEqual;
      if (this.Blending.HasValue)
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, this.Blending.Value);
      if (this.SamplerState != null)
      {
        for (int index = 0; index < this.Mesh.UsedSamplers; ++index)
          graphicsDevice.SamplerStates[index] = this.SamplerState;
      }
      if (this.CullMode.HasValue)
        GraphicsDeviceExtensions.SetCullMode(graphicsDevice, this.CullMode.Value);
      if (this.NoAlphaWrite.HasValue)
        GraphicsDeviceExtensions.GetBlendCombiner(graphicsDevice).ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;
      effect.Prepare(this);
      GraphicsDeviceExtensions.ApplyCombiners(graphicsDevice);
      this.Geometry.Draw(effect);
    }

    internal Group Clone(Mesh mesh)
    {
      return new Group(mesh, mesh.Groups.Count)
      {
        Geometry = this.Geometry.Clone(),
        Material = this.Material == null ? (Material) null : this.Material.Clone(),
        Texture = this.Texture,
        WorldMatrix = (Dirtyable<Matrix>) this.worldMatrix,
        SamplerState = this.SamplerState,
        Blending = this.Blending,
        AlwaysOnTop = this.AlwaysOnTop,
        CullMode = this.CullMode,
        TexturingType = this.TexturingType,
        TextureMatrix = this.TextureMatrix,
        RotateOffCenter = this.RotateOffCenter,
        Enabled = this.Enabled
      };
    }

    public void BakeTransformInstanced<VertexType, InstanceType>() where VertexType : struct, IShaderInstantiatableVertex, IVertex where InstanceType : struct
    {
      VertexType[] vertices = (this.Geometry as ShaderInstancedIndexedPrimitives<VertexType, InstanceType>).Vertices;
      for (int index = 0; index < vertices.Length; ++index)
        vertices[index].Position = Vector3.Transform(vertices[index].Position, (Matrix) this.compositeWorldMatrix);
      this.WorldMatrix = (Dirtyable<Matrix>) Matrix.Identity;
    }

    public void BakeTransform<T>() where T : struct, IVertex
    {
      T[] vertices = (this.Geometry as IndexedUserPrimitives<T>).Vertices;
      for (int index = 0; index < vertices.Length; ++index)
        vertices[index].Position = Vector3.Transform(vertices[index].Position, (Matrix) this.compositeWorldMatrix);
      this.WorldMatrix = (Dirtyable<Matrix>) Matrix.Identity;
    }

    public void BakeTransformWithNormal<T>() where T : struct, ILitVertex
    {
      T[] vertices = (this.Geometry as IndexedUserPrimitives<T>).Vertices;
      for (int index = 0; index < vertices.Length; ++index)
      {
        vertices[index].Position = Vector3.Transform(vertices[index].Position, (Matrix) this.compositeWorldMatrix);
        vertices[index].Normal = Vector3.Normalize(Vector3.TransformNormal(vertices[index].Normal, (Matrix) this.compositeWorldMatrix));
      }
      this.WorldMatrix = (Dirtyable<Matrix>) Matrix.Identity;
    }

    public void BakeTransformWithNormalTexture<T>() where T : struct, ILitVertex, ITexturedVertex
    {
      T[] vertices = (this.Geometry as IndexedUserPrimitives<T>).Vertices;
      Matrix transform = this.textureMatrix.Value.HasValue ? this.textureMatrix.Value.Value : this.Mesh.TextureMatrix.Value;
      for (int index = 0; index < vertices.Length; ++index)
      {
        vertices[index].Position = Vector3.Transform(vertices[index].Position, (Matrix) this.compositeWorldMatrix);
        vertices[index].Normal = Vector3.Normalize(Vector3.TransformNormal(vertices[index].Normal, (Matrix) this.compositeWorldMatrix));
        vertices[index].TextureCoordinate = FezMath.TransformTexCoord(vertices[index].TextureCoordinate, transform);
      }
      this.TextureMatrix.Set(new Matrix?());
      this.WorldMatrix = (Dirtyable<Matrix>) Matrix.Identity;
    }

    public void InvertNormals<T>() where T : struct, ILitVertex
    {
      T[] vertices = (this.Geometry as IndexedUserPrimitives<T>).Vertices;
      for (int index = 0; index < vertices.Length; ++index)
        vertices[index].Normal = -vertices[index].Normal;
    }
  }
}
