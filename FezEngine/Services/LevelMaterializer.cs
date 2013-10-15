// Type: FezEngine.Services.LevelMaterializer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Effects;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Services
{
  public class LevelMaterializer : GameComponent, ILevelMaterializer
  {
    private readonly List<NpcInstance> levelNPCs = new List<NpcInstance>();
    private readonly Dictionary<int, List<TrileInstance>> trileRows = new Dictionary<int, List<TrileInstance>>();
    private ArtObjectInstance[] aoCache = new ArtObjectInstance[0];
    private BackgroundPlane[] plCache = new BackgroundPlane[0];
    private NpcInstance[] npCache = new NpcInstance[0];
    private const int OrthographicCullingDistance = 1;
    private const int PerspectiveCullingDistance = 50;
    protected readonly Dictionary<Trile, TrileMaterializer> trileMaterializers;
    private TrileMaterializer fallbackMaterializer;
    private readonly Dictionary<Point, List<TrileInstance>> viewedInstances;
    private Rectangle lastCullingBounds;
    private Rectangle cullingBounds;
    private FaceOrientation xOrientation;
    private FaceOrientation zOrientation;
    private float lastHeight;
    private float lastCullHeight;
    private float lastRadius;
    private TrileUpdateAction lastUpdateAction;
    private bool rowsCleared;
    private Mesh.RenderingHandler drawTrileLights;
    private Mesh.RenderingHandler trileRenderingHandler;
    private RenderPass renderPass;

    public List<ArtObjectInstance> LevelArtObjects { get; private set; }

    public List<BackgroundPlane> LevelPlanes { get; private set; }

    public Mesh TrilesMesh { get; private set; }

    public Mesh ArtObjectsMesh { get; private set; }

    public Mesh StaticPlanesMesh { get; private set; }

    public Mesh AnimatedPlanesMesh { get; private set; }

    public Mesh NpcMesh { get; private set; }

    public IEnumerable<Trile> MaterializedTriles
    {
      get
      {
        return (IEnumerable<Trile>) this.trileMaterializers.Keys;
      }
    }

    public TrileEffect TrilesEffect
    {
      get
      {
        return this.TrilesMesh.Effect as TrileEffect;
      }
    }

    public InstancedArtObjectEffect ArtObjectsEffect
    {
      get
      {
        return this.ArtObjectsMesh.Effect as InstancedArtObjectEffect;
      }
    }

    public InstancedStaticPlaneEffect StaticPlanesEffect
    {
      get
      {
        return this.StaticPlanesMesh.Effect as InstancedStaticPlaneEffect;
      }
    }

    public InstancedAnimatedPlaneEffect AnimatedPlanesEffect
    {
      get
      {
        return this.AnimatedPlanesMesh.Effect as InstancedAnimatedPlaneEffect;
      }
    }

    public AnimatedPlaneEffect NpcEffect
    {
      get
      {
        return this.NpcMesh.Effect as AnimatedPlaneEffect;
      }
    }

    public virtual RenderPass RenderPass
    {
      get
      {
        return this.renderPass;
      }
      set
      {
        this.renderPass = value;
        switch (this.renderPass)
        {
          case RenderPass.LightInAlphaEmitters:
            this.NpcEffect.Pass = this.StaticPlanesEffect.Pass = this.AnimatedPlanesEffect.Pass = this.TrilesEffect.Pass = this.ArtObjectsEffect.Pass = LightingEffectPass.Pre;
            if (this.LevelManager.TrileSet != null)
            {
              this.trileRenderingHandler = this.TrilesMesh.CustomRenderingHandler;
              if (this.drawTrileLights == null)
                this.drawTrileLights = new Mesh.RenderingHandler(this.DrawTrileLights);
              this.TrilesMesh.CustomRenderingHandler = this.drawTrileLights;
            }
            foreach (Group group in this.StaticPlanesMesh.Groups)
              group.Enabled = false;
            foreach (Group group in this.AnimatedPlanesMesh.Groups)
              group.Enabled = false;
            using (List<BackgroundPlane>.Enumerator enumerator = this.LevelPlanes.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                BackgroundPlane current = enumerator.Current;
                Group group = current.Group;
                int num = (group.Enabled ? 1 : 0) | (current.LightMap ? 0 : (current.Visible ? 1 : 0));
                group.Enabled = num != 0;
              }
              break;
            }
          case RenderPass.WorldspaceLightmaps:
            this.AnimatedPlanesEffect.Pass = this.StaticPlanesEffect.Pass = LightingEffectPass.Main;
            this.AnimatedPlanesEffect.IgnoreFog = this.StaticPlanesEffect.IgnoreFog = true;
            this.AnimatedPlanesMesh.DepthWrites = this.StaticPlanesMesh.DepthWrites = false;
            foreach (Group group in this.StaticPlanesMesh.Groups)
              group.Enabled = false;
            foreach (Group group in this.AnimatedPlanesMesh.Groups)
              group.Enabled = false;
            using (List<BackgroundPlane>.Enumerator enumerator = this.LevelPlanes.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                BackgroundPlane current = enumerator.Current;
                Group group = current.Group;
                int num = (group.Enabled ? 1 : 0) | (!current.LightMap || current.AlwaysOnTop ? 0 : (current.Visible ? 1 : 0));
                group.Enabled = num != 0;
              }
              break;
            }
          case RenderPass.ScreenspaceLightmaps:
            this.AnimatedPlanesMesh.AlwaysOnTop = this.StaticPlanesMesh.AlwaysOnTop = true;
            foreach (Group group in this.StaticPlanesMesh.Groups)
              group.Enabled = false;
            foreach (Group group in this.AnimatedPlanesMesh.Groups)
              group.Enabled = false;
            using (List<BackgroundPlane>.Enumerator enumerator = this.LevelPlanes.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                BackgroundPlane current = enumerator.Current;
                Group group = current.Group;
                int num = (group.Enabled ? 1 : 0) | (!current.LightMap || !current.AlwaysOnTop ? 0 : (current.Visible ? 1 : 0));
                group.Enabled = num != 0;
              }
              break;
            }
          case RenderPass.Ghosts:
            using (List<NpcInstance>.Enumerator enumerator = this.levelNPCs.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                NpcInstance current = enumerator.Current;
                if (current.Group != null)
                  current.Group.Enabled = current.ActorType == ActorType.LightningGhost && current.Enabled && current.Visible;
              }
              break;
            }
          case RenderPass.Normal:
            this.NpcEffect.Pass = this.ArtObjectsEffect.Pass = this.TrilesEffect.Pass = LightingEffectPass.Main;
            if (this.trileRenderingHandler != null)
            {
              this.TrilesMesh.CustomRenderingHandler = this.trileRenderingHandler;
              this.trileRenderingHandler = (Mesh.RenderingHandler) null;
            }
            this.AnimatedPlanesEffect.IgnoreFog = this.StaticPlanesEffect.IgnoreFog = false;
            this.AnimatedPlanesMesh.DepthWrites = this.StaticPlanesMesh.DepthWrites = true;
            this.AnimatedPlanesMesh.AlwaysOnTop = this.StaticPlanesMesh.AlwaysOnTop = false;
            foreach (Group group in this.StaticPlanesMesh.Groups)
              group.Enabled = false;
            foreach (Group group in this.AnimatedPlanesMesh.Groups)
              group.Enabled = false;
            foreach (BackgroundPlane backgroundPlane in this.LevelPlanes)
            {
              Group group = backgroundPlane.Group;
              int num = (group.Enabled ? 1 : 0) | (backgroundPlane.LightMap ? 0 : (backgroundPlane.Visible ? 1 : 0));
              group.Enabled = num != 0;
            }
            using (List<NpcInstance>.Enumerator enumerator = this.levelNPCs.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                NpcInstance current = enumerator.Current;
                if (current.Group != null)
                  current.Group.Enabled = (current.ActorType != ActorType.LightningGhost || current.Talking) && current.Enabled && current.Visible;
              }
              break;
            }
        }
      }
    }

    [ServiceDependency]
    public IGraphicsDeviceService GraphicsDeviceService { protected get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { protected get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { protected get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { protected get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { protected get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { protected get; set; }

    public event Action<TrileInstance> TrileInstanceBatched;

    protected LevelMaterializer(Game game)
      : base(game)
    {
      this.LevelArtObjects = new List<ArtObjectInstance>();
      this.LevelPlanes = new List<BackgroundPlane>();
      this.trileMaterializers = new Dictionary<Trile, TrileMaterializer>();
      this.viewedInstances = new Dictionary<Point, List<TrileInstance>>();
      this.TrilesMesh = new Mesh()
      {
        SamplerState = SamplerState.PointClamp,
        SkipGroupCheck = true
      };
      this.ArtObjectsMesh = new Mesh()
      {
        SamplerState = SamplerState.PointClamp,
        Blending = new BlendingMode?(BlendingMode.Alphablending),
        SkipGroupCheck = true
      };
      this.StaticPlanesMesh = new Mesh()
      {
        SamplerState = SamplerState.PointClamp,
        GroupOrder = (Comparison<Group>) ((a, b) =>
        {
          BlendingMode? local_0 = a.Blending;
          if ((local_0.GetValueOrDefault() != BlendingMode.Additive ? 0 : (local_0.HasValue ? 1 : 0)) != 0)
          {
            BlendingMode? local_1 = b.Blending;
            return (local_1.GetValueOrDefault() != BlendingMode.Additive ? 0 : (local_1.HasValue ? 1 : 0)) != 0 ? 0 : -1;
          }
          else
          {
            BlendingMode? local_2 = b.Blending;
            return (local_2.GetValueOrDefault() != BlendingMode.Additive ? 0 : (local_2.HasValue ? 1 : 0)) != 0 ? 1 : 0;
          }
        }),
        SkipGroupCheck = true
      };
      this.AnimatedPlanesMesh = new Mesh()
      {
        SamplerState = SamplerState.PointClamp,
        GroupOrder = (Comparison<Group>) ((a, b) =>
        {
          BlendingMode? local_0 = a.Blending;
          if ((local_0.GetValueOrDefault() != BlendingMode.Additive ? 0 : (local_0.HasValue ? 1 : 0)) != 0)
          {
            BlendingMode? local_1 = b.Blending;
            return (local_1.GetValueOrDefault() != BlendingMode.Additive ? 0 : (local_1.HasValue ? 1 : 0)) != 0 ? 0 : -1;
          }
          else
          {
            BlendingMode? local_2 = b.Blending;
            return (local_2.GetValueOrDefault() != BlendingMode.Additive ? 0 : (local_2.HasValue ? 1 : 0)) != 0 ? 1 : 0;
          }
        }),
        SkipGroupCheck = true
      };
      this.NpcMesh = new Mesh()
      {
        RotateOffCenter = true,
        SamplerState = SamplerState.PointClamp,
        Blending = new BlendingMode?(BlendingMode.Alphablending)
      };
    }

    public override void Initialize()
    {
      this.TrilesMesh.Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/FullWhite"));
      this.TrilesMesh.CustomRenderingHandler = new Mesh.RenderingHandler(this.DrawTriles);
      this.ArtObjectsMesh.CustomRenderingHandler = new Mesh.RenderingHandler(this.DrawArtObjects);
      this.CameraManager.ViewChanged += new Action(this.CullEverything);
      this.CameraManager.ProjectionChanged += new Action(this.CullEverything);
      this.fallbackMaterializer = new TrileMaterializer(this.LevelManager.SafeGetTrile(-1), this.TrilesMesh, true);
      this.fallbackMaterializer.Rebuild();
      this.fallbackMaterializer.Group.Texture = (Texture) this.CMProvider.Global.Load<Texture2D>("Other Textures/TransparentWhite");
      this.LevelManager.LevelChanging += (Action) (() =>
      {
        this.aoCache = new ArtObjectInstance[0];
        this.plCache = new BackgroundPlane[0];
        this.npCache = new NpcInstance[0];
        this.RegisterSatellites();
        this.UnRowify(false);
      });
    }

    public void Rowify()
    {
      this.rowsCleared = false;
      foreach (TrileInstance trileInstance in (IEnumerable<TrileInstance>) this.LevelManager.Triles.Values)
      {
        trileInstance.InstanceId = -1;
        ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry = trileInstance.VisualTrile.Geometry;
        if (geometry == null || !geometry.Empty || trileInstance.Overlaps)
        {
          int key = trileInstance.Emplacement.Y;
          List<TrileInstance> list;
          if (!this.trileRows.TryGetValue(key, out list))
            this.trileRows.Add(key, list = new List<TrileInstance>());
          list.Add(trileInstance);
        }
      }
    }

    public void UpdateRow(TrileEmplacement oldEmplacement, TrileInstance instance)
    {
      if (this.rowsCleared || this.lastUpdateAction == TrileUpdateAction.SingleFaceCullFull || this.lastUpdateAction == TrileUpdateAction.SingleFaceCullPartial)
        return;
      List<TrileInstance> list;
      if (this.trileRows.TryGetValue(oldEmplacement.Y, out list))
        list.Remove(instance);
      int key = instance.Emplacement.Y;
      if (!this.trileRows.TryGetValue(key, out list))
        this.trileRows.Add(key, list = new List<TrileInstance>());
      list.Add(instance);
    }

    public void UnRowify()
    {
      this.UnRowify(false);
    }

    private void UnRowify(bool soft)
    {
      if (soft)
      {
        foreach (KeyValuePair<int, List<TrileInstance>> keyValuePair in this.trileRows)
          keyValuePair.Value.Clear();
      }
      else
        this.trileRows.Clear();
      this.rowsCleared = true;
    }

    public void CleanFallbackTriles()
    {
      foreach (TrileInstance instance in this.fallbackMaterializer.Trile.Instances.ToArray())
        this.LevelManager.ClearTrile(instance, true);
    }

    public void ForceCull()
    {
      this.CullInstances();
      this.CullArtObjects();
      this.CullPlanes();
    }

    public void CullEverything()
    {
      if (this.EngineState.SkyRender || this.EngineState.Loading || this.EngineState.SkipRendering)
        return;
      this.CullInstances(false);
      this.CullArtObjects();
      this.CullPlanes();
    }

    private void CullPlanes()
    {
      bool flag = !FezMath.IsOrthographic(this.CameraManager.Viewpoint);
      Vector3 forward = this.CameraManager.InverseView.Forward;
      BoundingFrustum frustum = this.CameraManager.Frustum;
      if (this.LevelManager.Loops)
      {
        foreach (BackgroundPlane backgroundPlane in this.LevelPlanes)
        {
          backgroundPlane.UpdateBounds();
          backgroundPlane.Visible = !backgroundPlane.Hidden;
          if (backgroundPlane.Visible)
            backgroundPlane.Group.Enabled = true;
        }
      }
      else
      {
        foreach (BackgroundPlane backgroundPlane in this.LevelPlanes)
        {
          if (!backgroundPlane.Disposed)
          {
            backgroundPlane.UpdateBounds();
            backgroundPlane.Visible = !backgroundPlane.Hidden && (flag || backgroundPlane.Doublesided || (backgroundPlane.Crosshatch || backgroundPlane.Billboard) || (double) FezMath.Dot(forward, backgroundPlane.Forward) > 0.0) && frustum.Contains(backgroundPlane.Bounds) != ContainmentType.Disjoint;
            if (backgroundPlane.Visible)
              backgroundPlane.Group.Enabled = true;
          }
        }
      }
    }

    private void CullArtObjects()
    {
      foreach (Group group in this.ArtObjectsMesh.Groups)
        group.Enabled = false;
      BoundingFrustum frustum = this.CameraManager.Frustum;
      foreach (ArtObjectInstance artObjectInstance in this.LevelArtObjects)
      {
        artObjectInstance.RebuildBounds();
        ContainmentType result = ContainmentType.Disjoint;
        if (!artObjectInstance.Hidden)
          frustum.Contains(ref artObjectInstance.Bounds, out result);
        artObjectInstance.Visible = !artObjectInstance.Hidden && result != ContainmentType.Disjoint;
        if (artObjectInstance.Visible)
          artObjectInstance.ArtObject.Group.Enabled = true;
      }
    }

    public void PrepareFullCull()
    {
      this.lastUpdateAction = TrileUpdateAction.None;
    }

    public void CullInstances()
    {
      this.lastUpdateAction = TrileUpdateAction.None;
      this.CullInstances(false);
    }

    private void CullInstances(bool viewChanged)
    {
      if (this.CameraManager.ProjectionTransition && (FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.lastUpdateAction == TrileUpdateAction.NoCull) || this.EngineState.SkipRendering)
        return;
      if (this.EngineState.LoopRender)
        this.lastUpdateAction = TrileUpdateAction.None;
      TrileUpdateAction action = this.DetermineCullType();
      bool flag1 = false;
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      switch (action)
      {
        case TrileUpdateAction.SingleFaceCullPartial:
        case TrileUpdateAction.SingleFaceCullFull:
          Vector3 vector3_1 = FezMath.SideMask(this.CameraManager.Viewpoint);
          BoundingFrustum frustum1 = this.CameraManager.Frustum;
          BoundingBox boundingBox1 = new BoundingBox()
          {
            Min = {
              X = -frustum1.Left.D * frustum1.Left.DotNormal(vector3_1),
              Y = -frustum1.Bottom.D * frustum1.Bottom.Normal.Y
            },
            Max = {
              X = -frustum1.Right.D * frustum1.Right.DotNormal(vector3_1),
              Y = -frustum1.Top.D * frustum1.Top.Normal.Y
            }
          };
          Vector3 vector3_2 = FezMath.Min(boundingBox1.Min, boundingBox1.Max);
          Vector3 vector3_3 = FezMath.Max(boundingBox1.Min, boundingBox1.Max);
          this.cullingBounds = new Rectangle()
          {
            X = (int) Math.Floor((double) vector3_2.X) - 1,
            Y = (int) Math.Floor((double) vector3_2.Y) - 1,
            Width = (int) Math.Ceiling((double) vector3_3.X - (double) vector3_2.X) + 3,
            Height = (int) Math.Ceiling((double) vector3_3.Y - (double) vector3_2.Y) + 3
          };
          if (this.cullingBounds.Width < 0 || this.cullingBounds.Height < 0 || (double) this.CameraManager.Radius > 120.0 * (double) viewScale)
            this.cullingBounds = this.lastCullingBounds;
          flag1 = ((flag1 ? 1 : 0) | (Math.Abs(this.lastCullingBounds.X - this.cullingBounds.X) > 0 ? 1 : (Math.Abs(this.lastCullingBounds.Y - this.cullingBounds.Y) > 0 ? 1 : 0))) != 0;
          break;
        case TrileUpdateAction.TwoFaceCullPartial:
        case TrileUpdateAction.TwoFaceCullFull:
          Vector3 vector3_4 = FezMath.SideMask(this.CameraManager.Viewpoint);
          BoundingFrustum frustum2 = this.CameraManager.Frustum;
          Vector3 vector3_5 = FezMath.Sign(this.CameraManager.View.Forward);
          FaceOrientation faceOrientation1 = FezMath.OrientationFromDirection(vector3_5.X * Vector3.UnitX);
          FaceOrientation faceOrientation2 = FezMath.OrientationFromDirection(-vector3_5.Z * Vector3.UnitZ);
          bool flag2 = ((flag1 ? 1 : 0) | (faceOrientation1 != this.xOrientation ? 1 : (faceOrientation2 != this.zOrientation ? 1 : 0))) != 0;
          this.xOrientation = faceOrientation1;
          this.zOrientation = faceOrientation2;
          BoundingBox boundingBox2 = new BoundingBox()
          {
            Min = {
              X = -frustum2.Left.D * frustum2.Left.DotNormal(vector3_4),
              Y = -frustum2.Bottom.D * frustum2.Bottom.Normal.Y
            },
            Max = {
              X = -frustum2.Right.D * frustum2.Right.DotNormal(vector3_4),
              Y = -frustum2.Top.D * frustum2.Top.Normal.Y
            }
          };
          Vector3 vector3_6 = FezMath.Min(boundingBox2.Min, boundingBox2.Max);
          Vector3 vector3_7 = FezMath.Max(boundingBox2.Min, boundingBox2.Max);
          this.cullingBounds = new Rectangle()
          {
            X = (int) Math.Floor((double) vector3_6.X) - 1,
            Y = (int) Math.Floor((double) vector3_6.Y) - 1,
            Width = (int) Math.Ceiling((double) vector3_7.X - (double) vector3_6.X) + 3,
            Height = (int) Math.Ceiling((double) vector3_7.Y - (double) vector3_6.Y) + 3
          };
          if (this.cullingBounds.Width < 0 || this.cullingBounds.Height < 0 || (double) this.CameraManager.Radius > 120.0 * (double) viewScale)
            this.cullingBounds = this.lastCullingBounds;
          flag1 = flag2 | Math.Abs(this.lastCullingBounds.Y - this.cullingBounds.Y) > 0;
          break;
        case TrileUpdateAction.TriFaceCull:
          Vector3 vector3_8 = FezMath.Sign(this.CameraManager.View.Forward);
          FaceOrientation faceOrientation3 = FezMath.OrientationFromDirection(vector3_8.X * Vector3.UnitX);
          FaceOrientation faceOrientation4 = FezMath.OrientationFromDirection(-vector3_8.Z * Vector3.UnitZ);
          flag1 = ((flag1 ? 1 : 0) | (faceOrientation3 != this.xOrientation ? 1 : (faceOrientation4 != this.zOrientation ? 1 : 0))) != 0;
          this.xOrientation = faceOrientation3;
          this.zOrientation = faceOrientation4;
          if ((double) Math.Abs(this.CameraManager.InterpolatedCenter.Y - this.lastCullHeight) >= 1.0 || (double) this.lastRadius != (double) this.CameraManager.Radius)
          {
            flag1 = true;
            break;
          }
          else
            break;
        case TrileUpdateAction.NoCull:
          if ((double) Math.Abs(this.CameraManager.InterpolatedCenter.Y - this.lastCullHeight) >= 5.0)
          {
            flag1 = true;
            break;
          }
          else
            break;
      }
      this.lastHeight = this.CameraManager.InterpolatedCenter.Y;
      this.lastRadius = this.CameraManager.Radius;
      if (((flag1 | viewChanged ? 1 : 0) | (this.lastUpdateAction == action || this.lastUpdateAction == TrileUpdateAction.SingleFaceCullFull && action == TrileUpdateAction.SingleFaceCullPartial ? 0 : (this.lastUpdateAction != TrileUpdateAction.TwoFaceCullFull ? 1 : (action != TrileUpdateAction.TwoFaceCullPartial ? 1 : 0)))) != 0)
      {
        this.UpdateInstances(action);
        this.lastCullingBounds = this.cullingBounds;
        this.lastCullHeight = this.CameraManager.InterpolatedCenter.Y;
      }
      this.lastUpdateAction = action;
    }

    private TrileUpdateAction DetermineCullType()
    {
      TrileUpdateAction trileUpdateAction = TrileUpdateAction.None;
      if (this.CameraManager.Viewpoint == Viewpoint.Perspective)
      {
        trileUpdateAction = TrileUpdateAction.NoCull;
      }
      else
      {
        Vector3 a = FezMath.Round(this.CameraManager.View.Forward, 5);
        if (this.CameraManager.Viewpoint == Viewpoint.Left || this.CameraManager.Viewpoint == Viewpoint.Right)
          a *= -1f;
        float num = FezMath.Dot(a, FezMath.ForwardVector(this.CameraManager.Viewpoint));
        if ((double) num == 1.0)
          trileUpdateAction = this.lastUpdateAction == TrileUpdateAction.SingleFaceCullFull || this.lastUpdateAction == TrileUpdateAction.SingleFaceCullPartial ? TrileUpdateAction.SingleFaceCullPartial : TrileUpdateAction.SingleFaceCullFull;
        else if ((double) a.Y == 0.0 && (double) num != 0.0)
        {
          Vector3 vector3 = FezMath.Sign(this.CameraManager.View.Forward);
          trileUpdateAction = FezMath.OrientationFromDirection(vector3.X * Vector3.UnitX) != this.xOrientation || FezMath.OrientationFromDirection(-vector3.Z * Vector3.UnitZ) != this.zOrientation ? TrileUpdateAction.TwoFaceCullFull : (this.lastUpdateAction == TrileUpdateAction.TwoFaceCullFull || this.lastUpdateAction == TrileUpdateAction.TwoFaceCullPartial ? TrileUpdateAction.TwoFaceCullPartial : TrileUpdateAction.TwoFaceCullFull);
        }
        else if ((double) Math.Abs(FezMath.Dot(a, Vector3.One)) != 1.0 && (double) num != 0.0)
          trileUpdateAction = TrileUpdateAction.TriFaceCull;
      }
      return trileUpdateAction;
    }

    public void UpdateInstance(TrileInstance instance)
    {
      if (this.lastUpdateAction != TrileUpdateAction.SingleFaceCullFull && this.lastUpdateAction != TrileUpdateAction.SingleFaceCullPartial)
        return;
      if (instance.SkipCulling)
      {
        if (this.cullingBounds.Contains(new Point((double) FezMath.ForwardVector(this.CameraManager.Viewpoint).Z == 1.0 ? instance.Emplacement.X : instance.Emplacement.Z, instance.Emplacement.Y)))
          this.RegisterViewedInstance(instance);
        else
          this.CullInstanceOut(instance, true);
      }
      else
      {
        if (this.UnregisterViewedInstance(instance))
          this.CullInstanceOut(instance, true);
        if (!this.RegisterViewedInstance(instance))
          return;
        this.SafeAddToBatch(instance, true);
      }
    }

    public void CullInstanceIn(TrileInstance instance)
    {
      this.CullInstanceIn(instance, false);
    }

    public void CullInstanceInNoRegister(TrileInstance instance)
    {
      this.SafeAddToBatch(instance, false);
    }

    public void CullInstanceIn(TrileInstance instance, bool forceAdd)
    {
      if (this.lastUpdateAction != TrileUpdateAction.SingleFaceCullFull && this.lastUpdateAction != TrileUpdateAction.SingleFaceCullPartial || (this.RegisterViewedInstance(instance) || forceAdd))
        this.SafeAddToBatch(instance, true);
      if (!forceAdd || this.lastUpdateAction == TrileUpdateAction.SingleFaceCullFull || (this.lastUpdateAction == TrileUpdateAction.SingleFaceCullPartial || this.rowsCleared))
        return;
      int key = instance.Emplacement.Y;
      List<TrileInstance> list;
      if (!this.trileRows.TryGetValue(key, out list))
        this.trileRows.Add(key, list = new List<TrileInstance>());
      list.Add(instance);
    }

    public void CullInstanceOut(TrileInstance toRemove)
    {
      this.CullInstanceOut(toRemove, false);
    }

    public void CullInstanceOut(TrileInstance toRemove, bool skipUnregister)
    {
      if (!this.rowsCleared)
      {
        foreach (List<TrileInstance> list in this.trileRows.Values)
          list.Remove(toRemove);
      }
      if (!skipUnregister && !this.UnregisterViewedInstance(toRemove) && (this.lastUpdateAction != TrileUpdateAction.TriFaceCull && this.lastUpdateAction != TrileUpdateAction.TwoFaceCullFull) && (this.lastUpdateAction != TrileUpdateAction.TwoFaceCullPartial && this.lastUpdateAction != TrileUpdateAction.None && this.viewedInstances.Count > 0) || toRemove.InstanceId == -1)
        return;
      TrileMaterializer trileMaterializer = this.GetTrileMaterializer(toRemove.VisualTrile);
      if (trileMaterializer == null)
        return;
      trileMaterializer.RemoveFromBatch(toRemove);
    }

    public TrileMaterializer GetTrileMaterializer(Trile trile)
    {
      TrileMaterializer trileMaterializer;
      if (trile.Id < 0)
        trileMaterializer = this.fallbackMaterializer;
      else if (!this.trileMaterializers.TryGetValue(trile, out trileMaterializer))
        trileMaterializer = (TrileMaterializer) null;
      return trileMaterializer;
    }

    public virtual void InitializeArtObjects()
    {
      foreach (ArtObject artObject in Enumerable.Distinct<ArtObject>(Enumerable.Select<ArtObjectInstance, ArtObject>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, ArtObject>) (x => x.ArtObject))))
      {
        ArtObjectMaterializer objectMaterializer = new ArtObjectMaterializer(artObject);
        if (artObject.MissingTrixels != null)
        {
          objectMaterializer.MarkMissingCells();
          objectMaterializer.UpdateSurfaces();
        }
        if (artObject.Geometry == null)
          objectMaterializer.RebuildGeometry();
        else
          objectMaterializer.PostInitialize();
      }
      VertexGroup<FezVertexPositionNormalTexture>.Deallocate();
      foreach (ArtObjectInstance artObjectInstance in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
        artObjectInstance.Initialize();
    }

    public void RebuildTriles(bool quick)
    {
      this.RebuildTriles(this.LevelManager.TrileSet, quick);
    }

    public virtual void RebuildTriles(TrileSet trileSet, bool quick)
    {
      if (!quick)
        this.DestroyMaterializers(trileSet);
      if (trileSet != null)
      {
        foreach (Trile trile in Enumerable.Where<Trile>((IEnumerable<Trile>) trileSet.Triles.Values, (Func<Trile, bool>) (x => !this.trileMaterializers.ContainsKey(x))))
          this.RebuildTrile(trile);
        this.TrilesMesh.Texture = (Dirtyable<Texture>) ((Texture) trileSet.TextureAtlas);
      }
      VertexGroup<VertexPositionNormalTextureInstance>.Deallocate();
    }

    public virtual void RebuildTrile(Trile trile)
    {
      TrileMaterializer trileMaterializer = new TrileMaterializer(trile, this.TrilesMesh);
      this.trileMaterializers.Add(trile, trileMaterializer);
      trileMaterializer.Rebuild();
    }

    public void DestroyMaterializers(TrileSet trileSet)
    {
      if (trileSet != null)
      {
        foreach (TrileMaterializer trileMaterializer in Enumerable.ToArray<TrileMaterializer>(Enumerable.Where<TrileMaterializer>((IEnumerable<TrileMaterializer>) this.trileMaterializers.Values, (Func<TrileMaterializer, bool>) (x => trileSet.Triles.ContainsValue(x.Trile)))))
        {
          trileMaterializer.Dispose();
          this.trileMaterializers.Remove(trileMaterializer.Trile);
        }
      }
      else
        this.trileMaterializers.Clear();
    }

    public void ClearBatches()
    {
      foreach (TrileMaterializer trileMaterializer in this.trileMaterializers.Values)
        trileMaterializer.ClearBatch();
    }

    public void RebuildInstances()
    {
      this.fallbackMaterializer.Trile.Instances.Clear();
      foreach (Trile trile in this.trileMaterializers.Keys)
        trile.Instances.Clear();
      this.UnregisterAllViewedInstances();
      foreach (TrileInstance instance1 in (IEnumerable<TrileInstance>) this.LevelManager.Triles.Values)
      {
        instance1.ResetTrile();
        this.AddInstance(instance1);
        if (instance1.Overlaps)
        {
          foreach (TrileInstance instance2 in instance1.OverlappedTriles)
          {
            instance2.ResetTrile();
            this.AddInstance(instance2);
          }
        }
      }
    }

    public void CleanUp()
    {
      foreach (TrileMaterializer trileMaterializer in Enumerable.ToArray<TrileMaterializer>((IEnumerable<TrileMaterializer>) this.trileMaterializers.Values))
      {
        Trile trile = trileMaterializer.Trile;
        ActorType type = trileMaterializer.Trile.ActorSettings.Type;
        if (!trile.ForceKeep && trile.Instances.Count == 0 && (!ActorTypeExtensions.IsTreasure(type) && !ActorTypeExtensions.IsCollectible(type)))
        {
          trileMaterializer.Dispose();
          this.trileMaterializers.Remove(trileMaterializer.Trile);
        }
      }
    }

    public void AddInstance(TrileInstance instance)
    {
      if (instance.TrileId == -1 || this.LevelManager.TrileSet == null || !this.LevelManager.TrileSet.Triles.ContainsKey(instance.TrileId))
      {
        instance.TrileId = -1;
        instance.RefreshTrile();
        this.fallbackMaterializer.Trile.Instances.Add(instance);
      }
      else
        instance.VisualTrile.Instances.Add(instance);
    }

    public void RemoveInstance(TrileInstance instance)
    {
      if (instance.TrileId == -1 || this.LevelManager.TrileSet == null || !this.LevelManager.TrileSet.Triles.ContainsKey(instance.TrileId))
        this.fallbackMaterializer.Trile.Instances.Remove(instance);
      else
        instance.VisualTrile.Instances.Remove(instance);
    }

    private void UnregisterAllViewedInstances()
    {
      foreach (List<TrileInstance> list in this.viewedInstances.Values)
      {
        foreach (TrileInstance trileInstance in list)
          trileInstance.InstanceId = -1;
      }
      this.viewedInstances.Clear();
    }

    private void UpdateInstances(TrileUpdateAction action)
    {
      switch (action)
      {
        case TrileUpdateAction.None:
          return;
        case TrileUpdateAction.SingleFaceCullPartial:
          this.LevelManager.WaitForScreenInvalidation();
          for (int right = this.cullingBounds.Right; right < this.lastCullingBounds.Right; ++right)
          {
            for (int top = this.lastCullingBounds.Top; top < this.lastCullingBounds.Bottom; ++top)
              this.FreeScreenSpace(right, top);
          }
          for (int right = this.lastCullingBounds.Right; right < this.cullingBounds.Right; ++right)
          {
            for (int top = this.cullingBounds.Top; top < this.cullingBounds.Bottom; ++top)
              this.FillScreenSpace(right, top);
          }
          for (int left = this.lastCullingBounds.Left; left < this.cullingBounds.Left; ++left)
          {
            for (int top = this.lastCullingBounds.Top; top < this.lastCullingBounds.Bottom; ++top)
              this.FreeScreenSpace(left, top);
          }
          for (int left = this.cullingBounds.Left; left < this.lastCullingBounds.Left; ++left)
          {
            for (int top = this.cullingBounds.Top; top < this.cullingBounds.Bottom; ++top)
              this.FillScreenSpace(left, top);
          }
          for (int top = this.lastCullingBounds.Top; top < this.cullingBounds.Top; ++top)
          {
            for (int left = this.lastCullingBounds.Left; left < this.lastCullingBounds.Right; ++left)
              this.FreeScreenSpace(left, top);
          }
          for (int top = this.cullingBounds.Top; top < this.lastCullingBounds.Top; ++top)
          {
            for (int left = this.cullingBounds.Left; left < this.cullingBounds.Right; ++left)
              this.FillScreenSpace(left, top);
          }
          for (int bottom = this.cullingBounds.Bottom; bottom < this.lastCullingBounds.Bottom; ++bottom)
          {
            for (int left = this.lastCullingBounds.Left; left < this.lastCullingBounds.Right; ++left)
              this.FreeScreenSpace(left, bottom);
          }
          for (int bottom = this.lastCullingBounds.Bottom; bottom < this.cullingBounds.Bottom; ++bottom)
          {
            for (int left = this.cullingBounds.Left; left < this.cullingBounds.Right; ++left)
              this.FillScreenSpace(left, bottom);
          }
          break;
        case TrileUpdateAction.SingleFaceCullFull:
          foreach (TrileMaterializer trileMaterializer in this.trileMaterializers.Values)
            trileMaterializer.ResetBatch();
          this.fallbackMaterializer.ResetBatch();
          foreach (TrileInstance instance in (IEnumerable<TrileInstance>) this.LevelManager.Triles.Values)
          {
            if (instance.SkipCulling)
              this.SafeAddToBatch(instance, false);
            else
              instance.InstanceId = -1;
          }
          this.viewedInstances.Clear();
          if (!this.rowsCleared)
            this.UnRowify(true);
          this.LevelManager.WaitForScreenInvalidation();
          for (int left = this.cullingBounds.Left; left < this.cullingBounds.Right; ++left)
          {
            for (int top = this.cullingBounds.Top; top < this.cullingBounds.Bottom; ++top)
              this.FillScreenSpace(left, top);
          }
          break;
        case TrileUpdateAction.TwoFaceCullPartial:
          int top1 = this.lastCullingBounds.Top;
          int top2 = this.cullingBounds.Top;
          int bottom1 = this.cullingBounds.Bottom;
          int bottom2 = this.lastCullingBounds.Bottom;
          for (int key = top1; key < top2; ++key)
          {
            List<TrileInstance> list;
            if (this.trileRows.TryGetValue(key, out list))
            {
              foreach (TrileInstance instance in list)
              {
                if (instance.InstanceId != -1)
                  this.SafeRemoveFromBatchWithOverlaps(instance);
              }
            }
          }
          for (int key = top2; key < top1; ++key)
          {
            List<TrileInstance> list;
            if (this.trileRows.TryGetValue(key, out list))
            {
              foreach (TrileInstance instance in list)
              {
                TrileEmplacement emplacement = instance.Emplacement;
                if (instance.Enabled && !instance.Hidden && (instance.ForceSeeThrough || this.LevelManager.IsCornerTrile(ref emplacement, ref this.xOrientation, ref this.zOrientation)) && instance.Trile.Id >= 0)
                  this.SafeAddToBatchWithOverlaps(instance, false);
              }
            }
          }
          for (int key = bottom1; key < bottom2; ++key)
          {
            List<TrileInstance> list;
            if (this.trileRows.TryGetValue(key, out list))
            {
              foreach (TrileInstance instance in list)
              {
                if (instance.InstanceId != -1)
                  this.SafeRemoveFromBatchWithOverlaps(instance);
              }
            }
          }
          for (int key = bottom2; key < bottom1; ++key)
          {
            List<TrileInstance> list;
            if (this.trileRows.TryGetValue(key, out list))
            {
              foreach (TrileInstance instance in list)
              {
                TrileEmplacement emplacement = instance.Emplacement;
                if (instance.Enabled && !instance.Hidden && (instance.ForceSeeThrough || this.LevelManager.IsCornerTrile(ref emplacement, ref this.xOrientation, ref this.zOrientation)) && instance.Trile.Id >= 0)
                  this.SafeAddToBatchWithOverlaps(instance, false);
              }
            }
          }
          break;
        case TrileUpdateAction.TwoFaceCullFull:
          foreach (TrileMaterializer trileMaterializer in this.trileMaterializers.Values)
            trileMaterializer.ResetBatch();
          this.fallbackMaterializer.ResetBatch();
          if (this.rowsCleared)
            this.Rowify();
          this.viewedInstances.Clear();
          for (int top3 = this.cullingBounds.Top; top3 <= this.cullingBounds.Bottom; ++top3)
          {
            List<TrileInstance> list;
            if (this.trileRows.TryGetValue(top3, out list))
            {
              foreach (TrileInstance instance in list)
              {
                TrileEmplacement emplacement = instance.Emplacement;
                if (instance.Enabled && !instance.Hidden && (instance.ForceSeeThrough || this.LevelManager.IsCornerTrile(ref emplacement, ref this.xOrientation, ref this.zOrientation)) && instance.Trile.Id >= 0)
                  this.SafeAddToBatchWithOverlaps(instance, false);
              }
            }
          }
          break;
        case TrileUpdateAction.TriFaceCull:
          foreach (TrileMaterializer trileMaterializer in this.trileMaterializers.Values)
            trileMaterializer.ResetBatch();
          this.fallbackMaterializer.ResetBatch();
          this.UnregisterAllViewedInstances();
          float num1 = (float) ((double) this.CameraManager.Radius / (double) this.CameraManager.AspectRatio / 2.0 + 1.0);
          int num2 = this.EngineState.InEditor ? 8 : 0;
          using (IEnumerator<TrileInstance> enumerator = this.LevelManager.Triles.Values.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              TrileInstance current = enumerator.Current;
              TrileEmplacement emplacement = current.Emplacement;
              FaceOrientation face = FaceOrientation.Top;
              Vector3 position = current.Position;
              if (current.Enabled && !current.Hidden && (current.VisualTrile.Geometry == null || !current.VisualTrile.Geometry.Empty || current.Overlaps) && ((double) position.Y > (double) this.lastHeight - (double) num1 - 1.0 - (double) num2 && (double) position.Y < (double) this.lastHeight + (double) num1 + (double) num2 && (this.LevelManager.IsBorderTrileFace(ref emplacement, ref this.xOrientation) || this.LevelManager.IsBorderTrileFace(ref emplacement, ref this.zOrientation) || this.LevelManager.IsBorderTrileFace(ref emplacement, ref face))))
                this.SafeAddToBatchWithOverlaps(current, false);
            }
            break;
          }
        case TrileUpdateAction.NoCull:
          foreach (TrileMaterializer trileMaterializer in this.trileMaterializers.Values)
            trileMaterializer.ResetBatch();
          this.fallbackMaterializer.ResetBatch();
          this.UnregisterAllViewedInstances();
          using (IEnumerator<TrileInstance> enumerator = this.LevelManager.Triles.Values.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              TrileInstance current = enumerator.Current;
              TrileEmplacement emplacement = current.Emplacement;
              if (current.Enabled && !current.Hidden && ((double) current.Position.Y > (double) this.lastHeight - 50.0 && (double) current.Position.Y < (double) this.lastHeight + 50.0) && this.LevelManager.IsBorderTrile(ref emplacement))
                this.SafeAddToBatchWithOverlaps(current, false);
            }
            break;
          }
      }
      this.CommitBatchesIfNeeded();
    }

    public void CommitBatchesIfNeeded()
    {
      foreach (TrileMaterializer trileMaterializer in this.trileMaterializers.Values)
      {
        if (trileMaterializer.BatchNeedsCommit)
          trileMaterializer.CommitBatch();
      }
      if (!this.fallbackMaterializer.BatchNeedsCommit)
        return;
      this.fallbackMaterializer.CommitBatch();
    }

    public void FreeScreenSpace(int i, int j)
    {
      Point key = new Point(i, j);
      List<TrileInstance> list;
      if (this.viewedInstances.TryGetValue(key, out list))
      {
        foreach (TrileInstance instance in list)
        {
          if (instance.InstanceId != -1)
            this.SafeRemoveFromBatch(instance);
        }
        list.Clear();
      }
      this.viewedInstances.Remove(key);
    }

    public void FillScreenSpace(int i, int j)
    {
      Vector3 vector3 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      bool flag1 = (double) vector3.Z != 0.0;
      bool flag2 = flag1;
      int num = flag1 ? (int) vector3.Z : (int) vector3.X;
      Limit limit;
      if (!this.LevelManager.ScreenSpaceLimits.TryGetValue(new Point(i, j), out limit))
        return;
      limit.End += num;
      TrileEmplacement id = new TrileEmplacement(flag2 ? i : limit.Start, j, flag2 ? limit.Start : i);
      bool flag3 = ((flag1 ? (id.Z != limit.End ? 1 : 0) : (id.X != limit.End ? 1 : 0)) & (Math.Sign(flag1 ? limit.End - id.Z : limit.End - id.X) == num ? 1 : 0)) != 0;
      bool flag4 = true;
      for (; flag3; flag3 = flag1 ? id.Z != limit.End : id.X != limit.End)
      {
        TrileInstance instance1 = this.LevelManager.TrileInstanceAt(ref id);
        if (instance1 != null && instance1.Enabled && !instance1.Hidden && ((flag4 || instance1.PhysicsState != null) && !instance1.SkipCulling))
        {
          Point ssPos = new Point(i, j);
          this.RegisterViewedInstance(ssPos, instance1);
          this.SafeAddToBatch(instance1, false);
          flag4 = ((flag4 ? 1 : 0) & (instance1.VisualTrile.SeeThrough || id.AsVector != instance1.Position ? 1 : (instance1.ForceSeeThrough ? 1 : 0))) != 0;
          if (instance1.Overlaps)
          {
            foreach (TrileInstance instance2 in instance1.OverlappedTriles)
            {
              this.RegisterViewedInstance(ssPos, instance2);
              this.SafeAddToBatch(instance2, false);
              flag4 = ((flag4 ? 1 : 0) & (instance2.VisualTrile.SeeThrough || id.AsVector != instance2.Position ? 1 : (instance1.ForceSeeThrough ? 1 : 0))) != 0;
            }
          }
        }
        if (flag1)
          id.Z += num;
        else
          id.X += num;
      }
    }

    public bool UnregisterViewedInstance(TrileInstance instance)
    {
      Vector3 b = FezMath.SideMask(this.CameraManager.Viewpoint);
      Vector3 a = FezMath.Round(instance.LastUpdatePosition);
      Point key = new Point((int) FezMath.Dot(a, b), (int) a.Y);
      bool flag = false;
      if (this.viewedInstances.Count > 0)
      {
        List<TrileInstance> list;
        if (this.viewedInstances.TryGetValue(key, out list))
        {
          while (list.Remove(instance))
            flag = true;
        }
        else if (instance.OldSsEmplacement.HasValue && this.viewedInstances.TryGetValue(instance.OldSsEmplacement.Value, out list))
        {
          while (list.Remove(instance))
            flag = true;
        }
      }
      if (flag)
        instance.OldSsEmplacement = new Point?();
      return flag;
    }

    private bool RegisterViewedInstance(TrileInstance instance)
    {
      Vector3 b = FezMath.SideMask(this.CameraManager.Viewpoint);
      TrileEmplacement emplacement = instance.Emplacement;
      return this.RegisterViewedInstance(new Point((int) FezMath.Dot(emplacement.AsVector, b), emplacement.Y), instance);
    }

    private bool RegisterViewedInstance(Point ssPos, TrileInstance instance)
    {
      if (!this.cullingBounds.Contains(ssPos))
        return false;
      List<TrileInstance> list;
      if (!this.viewedInstances.TryGetValue(ssPos, out list))
        list = this.viewedInstances[ssPos] = new List<TrileInstance>();
      list.Add(instance);
      instance.OldSsEmplacement = new Point?(ssPos);
      return true;
    }

    private void SafeRemoveFromBatchWithOverlaps(TrileInstance instance)
    {
      this.SafeRemoveFromBatch(instance);
      if (!instance.Overlaps)
        return;
      foreach (TrileInstance instance1 in instance.OverlappedTriles)
        this.SafeRemoveFromBatch(instance1);
    }

    private void SafeRemoveFromBatch(TrileInstance instance)
    {
      if (instance.InstanceId < 0)
        return;
      TrileMaterializer trileMaterializer = this.GetTrileMaterializer(instance.VisualTrile);
      if (trileMaterializer == null)
        return;
      trileMaterializer.RemoveFromBatch(instance);
    }

    private void SafeAddToBatchWithOverlaps(TrileInstance instance, bool autoCommit)
    {
      this.SafeAddToBatch(instance, autoCommit);
      if (!instance.Overlaps)
        return;
      foreach (TrileInstance instance1 in instance.OverlappedTriles)
        this.SafeAddToBatch(instance1, autoCommit);
    }

    private void SafeAddToBatch(TrileInstance instance, bool autoCommit)
    {
      TrileMaterializer trileMaterializer = this.GetTrileMaterializer(instance.VisualTrile);
      if (trileMaterializer == null)
        return;
      trileMaterializer.AddToBatch(instance);
      if (autoCommit)
        trileMaterializer.CommitBatch();
      if (this.TrileInstanceBatched == null)
        return;
      this.TrileInstanceBatched(instance);
    }

    private void DrawArtObjects(Mesh m, BaseEffect effect)
    {
      GraphicsDevice graphicsDevice = this.GraphicsDeviceService.GraphicsDevice;
      foreach (ArtObjectInstance artObjectInstance in this.LevelArtObjects)
      {
        artObjectInstance.ArtObject.Geometry.InstanceCount = artObjectInstance.ArtObject.InstanceCount;
        artObjectInstance.Update();
      }
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.NoSilhouette));
      foreach (Group group in this.ArtObjectsMesh.Groups)
      {
        ArtObjectCustomData objectCustomData = (ArtObjectCustomData) group.CustomData;
        if (group.Enabled && objectCustomData.ArtObject.NoSihouette)
          group.Draw(effect);
      }
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
      foreach (Group group in this.ArtObjectsMesh.Groups)
      {
        ArtObjectCustomData objectCustomData = (ArtObjectCustomData) group.CustomData;
        if (group.Enabled && !objectCustomData.ArtObject.NoSihouette)
          group.Draw(effect);
      }
    }

    protected virtual void DrawTriles(Mesh m, BaseEffect effect)
    {
      GraphicsDevice graphicsDevice = this.GraphicsDeviceService.GraphicsDevice;
      foreach (TrileMaterializer trileMaterializer in this.trileMaterializers.Values)
      {
        if (trileMaterializer.Geometry != null && trileMaterializer.Geometry.InstanceCount != 0 && !trileMaterializer.Geometry.Empty)
        {
          Trile trile = trileMaterializer.Trile;
          ActorType type = trile.ActorSettings.Type;
          bool flag1 = false;
          bool flag2 = false;
          if (ActorTypeExtensions.IsBomb(type))
          {
            flag1 = true;
            GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Bomb));
          }
          else if (type == ActorType.LightningPlatform)
          {
            GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Ghosts));
            flag1 = true;
            GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.None);
            GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferWriteEnable = false;
            flag2 = true;
          }
          else if (type == ActorType.Hole)
          {
            flag1 = true;
            GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Hole));
          }
          else if (trile.Immaterial || trile.Thin || ActorTypeExtensions.IsPickable(type))
          {
            flag1 = true;
            GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.NoSilhouette));
          }
          trileMaterializer.Group.Draw(effect);
          if (flag1)
            GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
          if (flag2)
          {
            GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.All);
            GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferWriteEnable = true;
          }
        }
      }
      this.fallbackMaterializer.Group.Draw(effect);
    }

    public void RegisterSatellites()
    {
      int count1 = this.LevelManager.ArtObjects.Count;
      if (this.aoCache.Length < count1)
        Array.Resize<ArtObjectInstance>(ref this.aoCache, count1);
      this.LevelManager.ArtObjects.Values.CopyTo(this.aoCache, 0);
      this.LevelArtObjects.Clear();
      for (int index = 0; index < count1; ++index)
        this.LevelArtObjects.Add(this.aoCache[index]);
      int count2 = this.LevelManager.BackgroundPlanes.Count;
      if (this.plCache.Length < count2)
        Array.Resize<BackgroundPlane>(ref this.plCache, count2);
      this.LevelManager.BackgroundPlanes.Values.CopyTo(this.plCache, 0);
      this.LevelPlanes.Clear();
      for (int index = 0; index < count2; ++index)
      {
        this.LevelPlanes.Add(this.plCache[index]);
        this.plCache[index].Update();
      }
      int count3 = this.LevelManager.NonPlayerCharacters.Count;
      if (this.npCache.Length < count3)
        Array.Resize<NpcInstance>(ref this.npCache, count3);
      this.LevelManager.NonPlayerCharacters.Values.CopyTo(this.npCache, 0);
      this.levelNPCs.Clear();
      for (int index = 0; index < count3; ++index)
        this.levelNPCs.Add(this.npCache[index]);
    }

    private void DrawTrileLights(Mesh m, BaseEffect effect)
    {
      GraphicsDevice graphicsDevice = this.TrilesMesh.GraphicsDevice;
      foreach (TrileMaterializer trileMaterializer in this.trileMaterializers.Values)
      {
        if (trileMaterializer.Geometry != null && trileMaterializer.Geometry.InstanceCount != 0)
        {
          ActorType type = trileMaterializer.Trile.ActorSettings.Type;
          switch (type)
          {
            case ActorType.LightningPlatform:
              continue;
            case ActorType.GoldenCube:
              GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Sky));
              break;
          }
          if (this.LevelManager.BlinkingAlpha)
            this.TrilesEffect.Blink = type != ActorType.Crystal;
          trileMaterializer.Group.Draw(effect);
          if (type == ActorType.GoldenCube)
            GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
        }
      }
      this.TrilesEffect.Blink = false;
    }
  }
}
