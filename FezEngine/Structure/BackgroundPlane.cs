// Type: FezEngine.Structure.BackgroundPlane
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine;
using FezEngine.Services;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class BackgroundPlane
  {
    private Color filter = Color.White;
    private Vector3 scale = new Vector3(1f);
    private Quaternion rotation = Quaternion.Identity;
    private float opacity = 1f;
    private bool drawDirty = true;
    private bool boundsDirty = true;
    private IContentManagerProvider CMProvider;
    private ILevelManager LevelManager;
    private ILevelMaterializer LevelMaterializer;
    private ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix> Geometry;
    private Vector3 position;
    private int actualWidth;
    private int actualHeight;
    private bool lightMap;
    private bool allowOverbrightness;
    private bool fullbright;
    private bool pixelatedLightmap;
    private bool doublesided;
    private bool crosshatch;
    private bool billboard;
    private bool alwaysOnTop;
    private bool xTextureRepeat;
    private bool yTextureRepeat;
    private bool clampTexture;
    private AnimatedTexture animation;
    [Serialization(Ignore = true)]
    public BoundingBox Bounds;
    private bool visible;

    [Serialization(Ignore = true)]
    public int Id { get; set; }

    public Group Group { get; private set; }

    public Mesh HostMesh { private get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public ActorType ActorType { get; set; }

    [Serialization(Ignore = true)]
    public int InstanceIndex { get; private set; }

    [Serialization(Ignore = true)]
    public bool Visible
    {
      get
      {
        return this.visible;
      }
      set
      {
        BackgroundPlane backgroundPlane = this;
        int num = backgroundPlane.drawDirty | this.visible != value ? 1 : 0;
        backgroundPlane.drawDirty = num != 0;
        this.visible = value;
      }
    }

    [Serialization(Ignore = true)]
    public bool Hidden { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Animated { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Billboard
    {
      get
      {
        return this.billboard;
      }
      set
      {
        if (this.billboard && !value)
          this.Rotation = Quaternion.Identity;
        this.billboard = value;
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool SyncWithSamples { get; set; }

    [Serialization(Ignore = true)]
    public AnimationTiming Timing { get; set; }

    [Serialization(Ignore = true)]
    public bool Loop { get; set; }

    [Serialization(Ignore = true)]
    public Vector3 Forward { get; private set; }

    [Serialization(Ignore = true)]
    public Vector3? OriginalPosition { get; set; }

    [Serialization(Ignore = true)]
    public Quaternion OriginalRotation { get; set; }

    public Vector3 Position
    {
      get
      {
        return this.position;
      }
      set
      {
        this.position = value;
        this.drawDirty = this.boundsDirty = true;
      }
    }

    [Serialization(Ignore = true)]
    public FaceOrientation Orientation { get; private set; }

    [Serialization(Optional = true)]
    public Quaternion Rotation
    {
      get
      {
        return this.rotation;
      }
      set
      {
        this.rotation = value;
        this.drawDirty = this.boundsDirty = true;
        this.Orientation = FezMath.OrientationFromDirection(FezMath.AlmostClamp(Vector3.Transform(Vector3.UnitZ, this.rotation)));
        this.Forward = FezMath.Round(Vector3.Transform(Vector3.Forward, this.rotation));
      }
    }

    [Serialization(Optional = true)]
    public Vector3 Scale
    {
      get
      {
        return this.scale;
      }
      set
      {
        this.scale = value;
        this.drawDirty = this.boundsDirty = true;
      }
    }

    public Vector3 Size { get; set; }

    public string TextureName { get; set; }

    [Serialization(Ignore = true)]
    public Texture Texture { get; set; }

    [Serialization(Optional = true)]
    public float Opacity
    {
      get
      {
        return this.opacity;
      }
      set
      {
        this.opacity = value;
        this.drawDirty = true;
      }
    }

    [Serialization(Optional = true)]
    public float ParallaxFactor { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool LightMap
    {
      get
      {
        return this.lightMap;
      }
      set
      {
        this.lightMap = value;
        if (this.Group == null)
          return;
        this.InitializeGroup();
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool AllowOverbrightness
    {
      get
      {
        return this.allowOverbrightness;
      }
      set
      {
        this.allowOverbrightness = value;
        if (this.Group == null)
          return;
        this.InitializeGroup();
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Doublesided
    {
      get
      {
        return this.doublesided;
      }
      set
      {
        this.doublesided = value;
        if (this.Group == null)
          return;
        this.InitializeGroup();
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Crosshatch
    {
      get
      {
        return this.crosshatch;
      }
      set
      {
        this.crosshatch = value;
        if (this.Group == null)
          return;
        this.InitializeGroup();
      }
    }

    [Serialization(Optional = true)]
    public int? AttachedGroup { get; set; }

    [Serialization(Optional = true)]
    public int? AttachedPlane { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public Color Filter
    {
      get
      {
        return this.filter;
      }
      set
      {
        this.filter = value;
        this.drawDirty = true;
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool AlwaysOnTop
    {
      get
      {
        return this.alwaysOnTop;
      }
      set
      {
        this.alwaysOnTop = value;
        if (this.Group == null)
          return;
        this.InitializeGroup();
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Fullbright
    {
      get
      {
        return this.fullbright;
      }
      set
      {
        this.fullbright = value;
        this.drawDirty = true;
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool PixelatedLightmap
    {
      get
      {
        return this.pixelatedLightmap;
      }
      set
      {
        this.pixelatedLightmap = value;
        if (this.Group == null)
          return;
        this.InitializeGroup();
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool ClampTexture
    {
      get
      {
        return this.clampTexture;
      }
      set
      {
        this.clampTexture = value;
        if (this.Group == null)
          return;
        this.InitializeGroup();
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool XTextureRepeat
    {
      get
      {
        return this.xTextureRepeat;
      }
      set
      {
        this.xTextureRepeat = value;
        this.drawDirty = true;
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool YTextureRepeat
    {
      get
      {
        return this.yTextureRepeat;
      }
      set
      {
        this.yTextureRepeat = value;
        this.drawDirty = true;
      }
    }

    [Serialization(Ignore = true)]
    public bool Disposed { get; set; }

    public BackgroundPlane()
    {
      this.Loop = true;
      this.Visible = true;
      this.Orientation = FaceOrientation.Front;
      this.OriginalRotation = Quaternion.Identity;
    }

    public BackgroundPlane(Mesh hostMesh, AnimatedTexture animation)
      : this()
    {
      this.HostMesh = hostMesh;
      this.Timing = animation.Timing.Clone();
      this.Texture = (Texture) animation.Texture;
      this.Animated = true;
      this.animation = animation;
      this.actualWidth = animation.FrameWidth;
      this.actualHeight = animation.FrameHeight;
      this.Initialize();
    }

    public BackgroundPlane(Mesh hostMesh, Texture texture)
      : this()
    {
      this.HostMesh = hostMesh;
      this.Texture = texture;
      this.Animated = false;
      this.Initialize();
    }

    public BackgroundPlane(Mesh hostMesh, string textureName, bool animated)
      : this()
    {
      this.HostMesh = hostMesh;
      this.TextureName = textureName;
      this.Animated = animated;
      this.Initialize();
    }

    public void Initialize()
    {
      if (ServiceHelper.IsFull)
      {
        this.CMProvider = ServiceHelper.Get<IContentManagerProvider>();
        this.LevelManager = ServiceHelper.Get<ILevelManager>();
        this.LevelMaterializer = ServiceHelper.Get<ILevelMaterializer>();
      }
      if (this.Animated)
      {
        if (this.animation == null)
        {
          this.animation = this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Background Planes/" + this.TextureName);
          this.Timing = this.animation.Timing.Clone();
          this.Texture = (Texture) this.animation.Texture;
          this.actualWidth = this.animation.FrameWidth;
          this.actualHeight = this.animation.FrameHeight;
        }
        this.Timing.Loop = true;
        this.Timing.RandomizeStep();
        this.Size = new Vector3((float) this.actualWidth / 16f, (float) this.actualHeight / 16f, 0.125f);
      }
      else
      {
        if (this.Texture == null)
          this.Texture = (Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Background Planes/" + this.TextureName);
        this.Size = new Vector3((float) (this.Texture as Texture2D).Width / 16f, (float) (this.Texture as Texture2D).Height / 16f, 0.125f);
      }
      this.InitializeGroup();
    }

    private void InitializeGroup()
    {
      if (this.Group != null)
        this.DestroyInstancedGroup();
      BackgroundPlane backgroundPlane = Enumerable.FirstOrDefault<BackgroundPlane>((IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values, (Func<BackgroundPlane, bool>) (x =>
      {
        if (x != this && x.Animated == this.Animated && (x.doublesided == this.doublesided && x.crosshatch == this.crosshatch) && (this.Texture != null && x.Texture == this.Texture || this.TextureName != null && x.TextureName == this.TextureName) && (x.Group != null && x.clampTexture == this.clampTexture && (x.lightMap == this.lightMap && x.allowOverbrightness == this.allowOverbrightness)))
          return x.pixelatedLightmap == this.pixelatedLightmap;
        else
          return false;
      }));
      if (backgroundPlane == null)
      {
        this.Group = this.HostMesh.AddFace(this.Size, Vector3.Zero, FaceOrientation.Front, true, this.doublesided, this.crosshatch);
        this.Geometry = new ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix>(PrimitiveType.TriangleList, 58);
        this.Geometry.Vertices = Enumerable.ToArray<VertexPositionNormalTextureInstance>(Enumerable.Select<FezVertexPositionNormalTexture, VertexPositionNormalTextureInstance>((IEnumerable<FezVertexPositionNormalTexture>) (this.Group.Geometry as IndexedUserPrimitives<FezVertexPositionNormalTexture>).Vertices, (Func<FezVertexPositionNormalTexture, VertexPositionNormalTextureInstance>) (x => new VertexPositionNormalTextureInstance()
        {
          Position = x.Position,
          Normal = x.Normal,
          TextureCoordinate = x.TextureCoordinate
        })));
        this.Geometry.Indices = Enumerable.ToArray<int>((IEnumerable<int>) (this.Group.Geometry as IndexedUserPrimitives<FezVertexPositionNormalTexture>).Indices);
        this.Geometry.PredictiveBatchSize = 1;
        this.Group.Geometry = (IIndexedPrimitiveCollection) this.Geometry;
        this.Group.Texture = this.Texture;
        this.Geometry.Instances = new Matrix[4];
      }
      else
      {
        this.Group = backgroundPlane.Group;
        this.Geometry = backgroundPlane.Geometry;
      }
      if (this.Animated)
        this.Group.CustomData = (object) new Vector2((float) this.animation.Offsets[0].Width / (float) this.animation.Texture.Width, (float) this.animation.Offsets[0].Height / (float) this.animation.Texture.Height);
      this.InstanceIndex = this.Geometry.InstanceCount++;
      this.UpdateGroupSetings();
    }

    private void DestroyInstancedGroup()
    {
      int num = 0;
      foreach (BackgroundPlane backgroundPlane in (IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values)
      {
        if (backgroundPlane != this && backgroundPlane.Group == this.Group)
        {
          backgroundPlane.InstanceIndex = num++;
          backgroundPlane.drawDirty = true;
          backgroundPlane.Update();
        }
      }
      if (this.Geometry == null)
        return;
      this.Geometry.InstanceCount = num;
      if (num == 0 && this.Group != null)
      {
        this.Geometry = (ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix>) null;
        if (this.Animated)
          this.LevelMaterializer.AnimatedPlanesMesh.RemoveGroup(this.Group);
        else
          this.LevelMaterializer.StaticPlanesMesh.RemoveGroup(this.Group);
      }
      this.Group = (Group) null;
    }

    public void Update()
    {
      if (!this.drawDirty)
        return;
      if (this.Geometry.Instances.Length < this.Geometry.InstanceCount)
        Array.Resize<Matrix>(ref this.Geometry.Instances, this.Geometry.InstanceCount + 4);
      this.Geometry.UpdateBuffers();
      Vector3 vector3_1 = this.Visible ? this.Position : new Vector3(float.MinValue);
      Vector3 vector3_2 = this.Filter.ToVector3();
      int num = (this.fullbright ? 1 : 0) | (this.clampTexture ? 2 : 0) | (this.xTextureRepeat ? 4 : 0) | (this.yTextureRepeat ? 8 : 0);
      Vector2 zero = Vector2.Zero;
      if (this.Animated)
      {
        int frame = this.Timing.Frame;
        zero.X = (float) this.animation.Offsets[frame].X / (float) this.animation.Texture.Width;
        zero.Y = (float) this.animation.Offsets[frame].Y / (float) this.animation.Texture.Height;
      }
      this.Group.NoAlphaWrite = (double) this.opacity == 0.0 || (double) this.opacity == 1.0 ? new bool?() : new bool?(true);
      this.Geometry.Instances[this.InstanceIndex] = new Matrix(vector3_1.X, vector3_1.Y, vector3_1.Z, zero.X, this.Rotation.X, this.Rotation.Y, this.Rotation.Z, this.Rotation.W, this.Scale.X, this.Scale.Y, zero.Y, (float) num, vector3_2.X, vector3_2.Y, vector3_2.Z, this.Opacity);
      this.drawDirty = false;
    }

    public void UpdateBounds()
    {
      if (!this.boundsDirty)
        return;
      Vector3 vector3 = FezMath.Abs(Vector3.Transform(this.Size / 2f * this.scale, this.rotation));
      this.Bounds = FezMath.Enclose(this.position - vector3, this.position + vector3);
      this.boundsDirty = false;
    }

    public void MarkDirty()
    {
      this.drawDirty = true;
    }

    public void UpdateGroupSetings()
    {
      this.Group.Blending = !this.lightMap ? new BlendingMode?(BlendingMode.Alphablending) : new BlendingMode?(this.allowOverbrightness ? BlendingMode.Additive : BlendingMode.Maximum);
      this.Group.SamplerState = !this.lightMap || this.pixelatedLightmap ? (this.clampTexture || !this.xTextureRepeat && !this.yTextureRepeat ? SamplerState.PointClamp : SamplerState.PointWrap) : (this.clampTexture || !this.xTextureRepeat && !this.yTextureRepeat ? SamplerState.LinearClamp : SamplerState.LinearWrap);
      this.drawDirty = true;
    }

    public void Dispose()
    {
      this.DestroyInstancedGroup();
      this.Disposed = true;
    }

    public BackgroundPlane Clone()
    {
      BackgroundPlane backgroundPlane = new BackgroundPlane()
      {
        HostMesh = this.HostMesh,
        Animated = this.Animated,
        TextureName = this.TextureName,
        Texture = this.Texture,
        Timing = this.Timing == null ? (AnimationTiming) null : this.Timing.Clone(),
        Position = this.position,
        Rotation = this.rotation,
        Scale = this.scale,
        LightMap = this.lightMap,
        AllowOverbrightness = this.allowOverbrightness,
        Filter = this.filter,
        Doublesided = this.doublesided,
        Opacity = this.opacity,
        Crosshatch = this.crosshatch,
        SyncWithSamples = this.SyncWithSamples,
        AlwaysOnTop = this.alwaysOnTop,
        Fullbright = this.fullbright,
        Loop = this.Loop,
        Billboard = this.billboard,
        AttachedGroup = this.AttachedGroup,
        YTextureRepeat = this.YTextureRepeat,
        XTextureRepeat = this.XTextureRepeat,
        ClampTexture = this.ClampTexture,
        AttachedPlane = this.AttachedPlane,
        ActorType = this.ActorType,
        ParallaxFactor = this.ParallaxFactor
      };
      backgroundPlane.Initialize();
      return backgroundPlane;
    }
  }
}
