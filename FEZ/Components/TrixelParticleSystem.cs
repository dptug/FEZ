// Type: FezGame.Components.TrixelParticleSystem
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  public class TrixelParticleSystem : DrawableGameComponent
  {
    private static readonly object StaticLock = new object();
    private readonly List<TrixelParticleSystem.Particle> particles = new List<TrixelParticleSystem.Particle>();
    private float Opacity = 1f;
    private const int InstancesPerBatch = 60;
    private const int FadeOutStartSeconds = 4;
    private const int LifetimeSeconds = 6;
    private const float VelocityFactor = 0.2f;
    private const float EnergyDecay = 1.5f;
    private readonly IPhysicsManager PhysicsManager;
    private readonly IGameCameraManager CameraManager;
    private readonly IGameStateManager GameState;
    private readonly ILightingPostProcess LightingPostProcess;
    private readonly ICollisionManager CollisionManager;
    private readonly TrixelParticleSystem.Settings settings;
    private static volatile TrixelParticleEffect effect;
    private Mesh mesh;
    private TimeSpan age;
    private ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix> Geometry;

    public Vector3 Offset { get; set; }

    public int ActiveParticles
    {
      get
      {
        return Enumerable.Count<TrixelParticleSystem.Particle>((IEnumerable<TrixelParticleSystem.Particle>) this.particles, (Func<TrixelParticleSystem.Particle, bool>) (x =>
        {
          if (x.Enabled)
            return !x.Static;
          else
            return false;
        }));
      }
    }

    public bool Dead { get; private set; }

    static TrixelParticleSystem()
    {
    }

    public TrixelParticleSystem(Game game, TrixelParticleSystem.Settings settings)
      : base(game)
    {
      this.settings = settings;
      this.DrawOrder = 10;
      this.PhysicsManager = ServiceHelper.Get<IPhysicsManager>();
      this.CameraManager = ServiceHelper.Get<IGameCameraManager>();
      this.GameState = ServiceHelper.Get<IGameStateManager>();
      this.LightingPostProcess = ServiceHelper.Get<ILightingPostProcess>();
      this.CollisionManager = ServiceHelper.Get<ICollisionManager>();
    }

    public override void Initialize()
    {
      base.Initialize();
      this.SetupGeometry();
      this.SetupInstances();
      this.LightingPostProcess.DrawGeometryLights += new Action(this.PreDraw);
    }

    private void SetupGeometry()
    {
      lock (TrixelParticleSystem.StaticLock)
      {
        if (TrixelParticleSystem.effect == null)
          TrixelParticleSystem.effect = new TrixelParticleEffect();
      }
      this.mesh = new Mesh()
      {
        Effect = (BaseEffect) TrixelParticleSystem.effect,
        SkipStates = true
      };
      this.mesh.AddGroup().Geometry = (IIndexedPrimitiveCollection) (this.Geometry = new ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix>(PrimitiveType.TriangleList, 60));
      Vector3 vector3 = new Vector3(0.0f);
      this.Geometry.Vertices = new VertexPositionNormalTextureInstance[24]
      {
        new VertexPositionNormalTextureInstance(new Vector3(-1f, -1f, -1f) * 0.5f + vector3, -Vector3.UnitZ)
        {
          TextureCoordinate = new Vector2(0.125f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, 1f, -1f) * 0.5f + vector3, -Vector3.UnitZ)
        {
          TextureCoordinate = new Vector2(0.125f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, 1f, -1f) * 0.5f + vector3, -Vector3.UnitZ)
        {
          TextureCoordinate = new Vector2(0.0f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, -1f, -1f) * 0.5f + vector3, -Vector3.UnitZ)
        {
          TextureCoordinate = new Vector2(0.0f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, -1f, -1f) * 0.5f + vector3, Vector3.UnitX)
        {
          TextureCoordinate = new Vector2(0.125f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, 1f, -1f) * 0.5f + vector3, Vector3.UnitX)
        {
          TextureCoordinate = new Vector2(0.125f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, 1f, 1f) * 0.5f + vector3, Vector3.UnitX)
        {
          TextureCoordinate = new Vector2(0.0f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, -1f, 1f) * 0.5f + vector3, Vector3.UnitX)
        {
          TextureCoordinate = new Vector2(0.0f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, -1f, 1f) * 0.5f + vector3, Vector3.UnitZ)
        {
          TextureCoordinate = new Vector2(0.125f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, 1f, 1f) * 0.5f + vector3, Vector3.UnitZ)
        {
          TextureCoordinate = new Vector2(0.125f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, 1f, 1f) * 0.5f + vector3, Vector3.UnitZ)
        {
          TextureCoordinate = new Vector2(0.0f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, -1f, 1f) * 0.5f + vector3, Vector3.UnitZ)
        {
          TextureCoordinate = new Vector2(0.0f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, -1f, 1f) * 0.5f + vector3, -Vector3.UnitX)
        {
          TextureCoordinate = new Vector2(0.125f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, 1f, 1f) * 0.5f + vector3, -Vector3.UnitX)
        {
          TextureCoordinate = new Vector2(0.125f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, 1f, -1f) * 0.5f + vector3, -Vector3.UnitX)
        {
          TextureCoordinate = new Vector2(0.0f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, -1f, -1f) * 0.5f + vector3, -Vector3.UnitX)
        {
          TextureCoordinate = new Vector2(0.0f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, -1f, -1f) * 0.5f + vector3, -Vector3.UnitY)
        {
          TextureCoordinate = new Vector2(0.125f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, -1f, 1f) * 0.5f + vector3, -Vector3.UnitY)
        {
          TextureCoordinate = new Vector2(0.125f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, -1f, 1f) * 0.5f + vector3, -Vector3.UnitY)
        {
          TextureCoordinate = new Vector2(0.0f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, -1f, -1f) * 0.5f + vector3, -Vector3.UnitY)
        {
          TextureCoordinate = new Vector2(0.0f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, 1f, -1f) * 0.5f + vector3, Vector3.UnitY)
        {
          TextureCoordinate = new Vector2(0.125f, 1f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(-1f, 1f, 1f) * 0.5f + vector3, Vector3.UnitY)
        {
          TextureCoordinate = new Vector2(0.125f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, 1f, 1f) * 0.5f + vector3, Vector3.UnitY)
        {
          TextureCoordinate = new Vector2(0.0f, 0.0f)
        },
        new VertexPositionNormalTextureInstance(new Vector3(1f, 1f, -1f) * 0.5f + vector3, Vector3.UnitY)
        {
          TextureCoordinate = new Vector2(0.0f, 1f)
        }
      };
      this.Geometry.Indices = new int[36]
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
      };
      this.Geometry.Instances = new Matrix[this.settings.ParticleCount];
      this.Geometry.MaximizeBuffers(this.settings.ParticleCount);
      this.Geometry.InstanceCount = this.settings.ParticleCount;
      this.mesh.Texture.Set((Texture) this.settings.ExplodingInstance.Trile.TrileSet.TextureAtlas);
    }

    private void SetupInstances()
    {
      float num1 = this.settings.ExplodingInstance.Trile.ActorSettings.Type == ActorType.Vase ? 0.05f : 0.15f;
      int maxValue = 16 - this.settings.MinimumSize;
      Vector3 b1 = FezMath.SideMask(this.CameraManager.Viewpoint);
      Vector3 b2 = FezMath.DepthMask(this.CameraManager.Viewpoint);
      bool flag1 = (double) b1.X != 0.0;
      bool flag2 = this.CameraManager.Viewpoint == Viewpoint.Front || this.CameraManager.Viewpoint == Viewpoint.Left;
      Random random = RandomHelper.Random;
      Vector3 a1 = this.settings.EnergySource.Value;
      Vector3 vector3_1 = new Vector3(FezMath.Dot(a1, b1), a1.Y, 0.0f);
      Vector3 vector3_2 = FezMath.Dot(this.settings.ExplodingInstance.Center, b2) * b2;
      Vector3 vector3_3 = a1 - FezMath.Dot(a1, b2) * b2;
      Vector2 vector2_1 = new Vector2((float) this.settings.ExplodingInstance.Trile.TrileSet.TextureAtlas.Width, (float) this.settings.ExplodingInstance.Trile.TrileSet.TextureAtlas.Height);
      Vector2 vector2_2 = new Vector2(128f / vector2_1.X, 16f / vector2_1.Y);
      Vector2 atlasOffset = this.settings.ExplodingInstance.Trile.AtlasOffset;
      List<SpaceDivider.DividedCell> list = (List<SpaceDivider.DividedCell>) null;
      if (this.settings.Crumble)
        list = SpaceDivider.Split(this.settings.ParticleCount);
      for (int instanceIndex = 0; instanceIndex < this.settings.ParticleCount; ++instanceIndex)
      {
        TrixelParticleSystem.Particle particle = new TrixelParticleSystem.Particle(this, instanceIndex)
        {
          Elasticity = num1
        };
        Vector3 vector3_4;
        Vector3 vector1;
        if (this.settings.Crumble)
        {
          SpaceDivider.DividedCell dividedCell = list[instanceIndex];
          vector3_4 = ((float) (dividedCell.Left - 8) * b1 + (float) (dividedCell.Bottom - 8) * Vector3.UnitY + (float) (dividedCell.Left - 8) * b2) / 16f;
          vector1 = ((float) dividedCell.Width * (b1 + b2) + (float) dividedCell.Height * Vector3.UnitY) / 16f;
        }
        else
        {
          vector3_4 = new Vector3((float) random.Next(0, maxValue), (float) random.Next(0, maxValue), (float) random.Next(0, maxValue));
          do
          {
            vector1 = new Vector3((float) random.Next(this.settings.MinimumSize, Math.Min(17 - (int) vector3_4.X, this.settings.MaximumSize)), (float) random.Next(this.settings.MinimumSize, Math.Min(17 - (int) vector3_4.Y, this.settings.MaximumSize)), (float) random.Next(this.settings.MinimumSize, Math.Min(17 - (int) vector3_4.Z, this.settings.MaximumSize)));
          }
          while ((double) Math.Abs(vector1.X - vector1.Y) > ((double) vector1.X + (double) vector1.Y) / 2.0 || (double) Math.Abs(vector1.Z - vector1.Y) > ((double) vector1.Z + (double) vector1.Y) / 2.0);
          vector3_4 = (vector3_4 - new Vector3(8f)) / 16f;
          vector1 /= 16f;
        }
        particle.Size = vector1;
        float num2 = flag1 ? vector1.X : vector1.Z;
        particle.TextureMatrix = new Vector4(num2 * vector2_2.X, vector1.Y * vector2_2.Y, (float) (((flag2 ? 1.0 : -1.0) * (flag1 ? (double) vector3_4.X : (double) vector3_4.Z) + (flag2 ? 0.0 : -(double) num2) + 0.5 + 1.0 / 16.0) / 8.0) * vector2_2.X + atlasOffset.X, (float) (-((double) vector3_4.Y + (double) vector1.Y) + 0.5 + 1.0 / 16.0) * vector2_2.Y + atlasOffset.Y);
        float num3 = this.settings.Darken ? RandomHelper.Between(0.3, 1.0) : 1f;
        particle.Color = new Vector3(num3, num3, num3);
        Vector3 a2 = this.settings.ExplodingInstance.Center + vector3_4 + vector1 / 2f;
        Vector3 vector3_5 = new Vector3(FezMath.Dot(a2, b1), a2.Y, 0.0f);
        Vector3 vector3_6 = a2 - vector3_2 - vector3_3;
        if (vector3_6 != Vector3.Zero)
          vector3_6.Normalize();
        if (this.settings.Crumble)
          vector3_6 = Vector3.Normalize(new Vector3(RandomHelper.Centered(1.0), RandomHelper.Centered(1.0), RandomHelper.Centered(1.0)));
        float num4 = Math.Min(1f, 1.5f - Vector3.Dot(vector1, Vector3.One));
        float num5 = (float) Math.Pow(1.0 / (1.0 + (double) (vector3_5 - vector3_1).Length()), 1.5);
        particle.Center = a2;
        particle.Velocity = vector3_6 * this.settings.Energy * num4 * 0.2f * num5 + this.settings.BaseVelocity;
        if (this.settings.Incandesce)
          particle.Incandescence = 2f;
        particle.Update();
        this.particles.Add(particle);
        if (this.settings.Crumble)
          particle.Delay = FezMath.Saturate(Easing.EaseOut((double) vector3_4.Y + 0.5, EasingType.Cubic) + RandomHelper.Centered(0.100000001490116));
      }
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.LightingPostProcess != null)
        this.LightingPostProcess.DrawGeometryLights -= new Action(this.PreDraw);
      if (this.Geometry == null)
        return;
      this.Geometry.Dispose();
      this.Geometry = (ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Matrix>) null;
    }

    public void UnGround()
    {
      foreach (TrixelParticleSystem.Particle particle in this.particles)
      {
        if (particle.Enabled)
        {
          particle.Ground = new MultipleHits<TrileInstance>();
          particle.Static = false;
        }
      }
    }

    public void AddImpulse(Vector3 energySource, float energy)
    {
      Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = energySource * vector3_1;
      foreach (TrixelParticleSystem.Particle particle in this.particles)
      {
        if (particle.Enabled)
        {
          Vector3 vector = particle.Center * vector3_1 - vector3_2;
          float num = (float) Math.Pow(1.0 / (1.0 + (double) vector.Length()), 1.5);
          if ((double) num > 0.100000001490116)
          {
            Vector3 vector3_3 = vector == Vector3.Zero ? Vector3.Zero : Vector3.Normalize(vector);
            particle.Velocity += vector3_3 * energy * 0.2f * num;
            particle.Static = false;
          }
        }
      }
    }

    public void Update(TimeSpan elapsed)
    {
      this.age += elapsed;
      if (this.age.TotalSeconds > 6.0)
      {
        this.Dead = true;
      }
      else
      {
        Vector3 vector3 = 0.4725f * this.CollisionManager.GravityFactor * this.settings.GravityModifier * (float) elapsed.TotalSeconds * Vector3.Down;
        float num1 = this.CameraManager.Radius / 2f;
        float num2 = this.CameraManager.Center.Y;
        float num3 = (float) this.age.TotalSeconds;
        bool flag1 = (double) num3 > 4.0;
        if (flag1)
          this.Opacity = FezMath.Saturate(1f - (float) (((double) num3 - 4.0) / 2.0));
        foreach (TrixelParticleSystem.Particle particle in this.particles)
        {
          if (this.settings.Crumble && this.age.TotalSeconds < (double) particle.Delay)
          {
            particle.Center += this.Offset;
            particle.Update();
          }
          else if (particle.Enabled)
          {
            bool flag2 = false;
            if (!particle.Static)
            {
              if ((double) num2 - (double) particle.Center.Y > (double) num1)
              {
                particle.Hide();
                particle.Enabled = false;
                continue;
              }
              else
              {
                particle.Velocity += vector3;
                particle.Incandescence *= 0.95f;
                flag2 = this.PhysicsManager.Update((ISimplePhysicsEntity) particle, true, false);
                particle.Static = !flag2 && particle.Grounded && particle.StaticGrounds;
              }
            }
            if (flag2 || flag1)
              particle.Update();
          }
        }
      }
    }

    private void PreDraw()
    {
      if (this.GameState.Loading || !this.Visible)
        return;
      GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDevice, new StencilMask?(StencilMask.Level));
      TrixelParticleSystem.effect.Pass = LightingEffectPass.Pre;
      this.mesh.Draw();
      TrixelParticleSystem.effect.Pass = LightingEffectPass.Main;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.StereoMode)
        return;
      this.DoDraw();
    }

    public void DoDraw()
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      if ((double) this.Opacity == 1.0)
        GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
      this.mesh.Draw();
      if ((double) this.Opacity != 1.0)
        return;
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }

    private class Particle : ISimplePhysicsEntity, IPhysicsEntity
    {
      private readonly int InstanceIndex;
      private readonly TrixelParticleSystem System;
      public bool Enabled;
      public bool Static;
      public float Incandescence;
      public Vector3 Color;
      public Vector4 TextureMatrix;

      public float Delay { get; set; }

      public bool NoVelocityClamping
      {
        get
        {
          return false;
        }
      }

      public bool IgnoreCollision
      {
        get
        {
          return false;
        }
      }

      public MultipleHits<TrileInstance> Ground { get; set; }

      public Vector3 Center { get; set; }

      public Vector3 Velocity { get; set; }

      public Vector3 GroundMovement { get; set; }

      public float Elasticity { get; set; }

      public bool Background { get; set; }

      public PointCollision[] CornerCollision { get; private set; }

      public MultipleHits<CollisionResult> WallCollision { get; set; }

      public Vector3 Size { get; set; }

      public bool Grounded
      {
        get
        {
          if (this.Ground.NearLow == null)
            return this.Ground.FarHigh != null;
          else
            return true;
        }
      }

      public bool Sliding
      {
        get
        {
          if (FezMath.AlmostEqual(this.Velocity.X, 0.0f))
            return !FezMath.AlmostEqual(this.Velocity.Z, 0.0f);
          else
            return true;
        }
      }

      public bool StaticGrounds
      {
        get
        {
          if (TrixelParticleSystem.Particle.IsGroundStatic(this.Ground.NearLow))
            return TrixelParticleSystem.Particle.IsGroundStatic(this.Ground.FarHigh);
          else
            return false;
        }
      }

      public Particle(TrixelParticleSystem system, int instanceIndex)
      {
        this.CornerCollision = new PointCollision[1];
        this.Enabled = true;
        this.InstanceIndex = instanceIndex;
        this.System = system;
      }

      public void Update()
      {
        this.System.Geometry.Instances[this.InstanceIndex] = new Matrix(this.Center.X, this.Center.Y, this.Center.Z, 0.0f, this.Size.X, this.Size.Y, this.Size.Z, 0.0f, this.Color.X * (1f + this.Incandescence), this.Color.Y * (1f + this.Incandescence), this.Color.Z * (1f + this.Incandescence), this.System.Opacity, this.TextureMatrix.X, this.TextureMatrix.Y, this.TextureMatrix.Z, this.TextureMatrix.W);
      }

      public void Hide()
      {
        this.System.Geometry.Instances[this.InstanceIndex] = new Matrix();
      }

      private static bool IsGroundStatic(TrileInstance ground)
      {
        if (ground == null || ground.PhysicsState == null)
          return true;
        if (ground.PhysicsState.Velocity == Vector3.Zero)
          return ground.PhysicsState.GroundMovement == Vector3.Zero;
        else
          return false;
      }
    }

    public class Settings
    {
      private TrileInstance explodingInstance;

      public Vector3 BaseVelocity { get; set; }

      public int MinimumSize { get; set; }

      public int MaximumSize { get; set; }

      public float Energy { get; set; }

      public float GravityModifier { get; set; }

      public Vector3? EnergySource { get; set; }

      public int ParticleCount { get; set; }

      public bool Darken { get; set; }

      public bool Incandesce { get; set; }

      public bool Crumble { get; set; }

      public TrileInstance ExplodingInstance
      {
        get
        {
          return this.explodingInstance;
        }
        set
        {
          if (!this.EnergySource.HasValue)
            this.EnergySource = new Vector3?(value.Position);
          this.explodingInstance = value;
        }
      }

      public Settings()
      {
        this.MinimumSize = 1;
        this.MaximumSize = 8;
        this.Energy = 1f;
        this.GravityModifier = 1f;
        this.ParticleCount = 40;
      }
    }
  }
}
