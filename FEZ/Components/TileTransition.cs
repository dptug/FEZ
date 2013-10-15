// Type: FezGame.Components.TileTransition
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  public class TileTransition : DrawableGameComponent
  {
    private static readonly object Mutex = new object();
    private const int TilesWide = 1;
    private static SoundEffect sTransition;
    private static TileTransition CurrentTransition;
    private RenderTargetHandle textureA;
    private RenderTargetHandle textureB;
    private bool taCaptured;
    private bool tbCaptured;
    private float sinceStarted;
    private Mesh mesh;

    public Action ScreenCaptured { private get; set; }

    public Func<bool> WaitFor { private get; set; }

    public bool IsDisposed { get; private set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    static TileTransition()
    {
    }

    public TileTransition(Game game)
      : base(game)
    {
      if (TileTransition.CurrentTransition != null)
      {
        ServiceHelper.RemoveComponent<TileTransition>(TileTransition.CurrentTransition);
        TileTransition.CurrentTransition = (TileTransition) null;
      }
      TileTransition.CurrentTransition = this;
      this.DrawOrder = 2099;
    }

    public override void Initialize()
    {
      base.Initialize();
      lock (TileTransition.Mutex)
      {
        if (TileTransition.sTransition == null)
          TileTransition.sTransition = this.CMProvider.Global.Load<SoundEffect>("Sounds/Ui/CubeTransition");
      }
      TileTransition tileTransition = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.Textured textured1 = new DefaultEffect.Textured();
      textured1.ForcedProjectionMatrix = new Matrix?(Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), 1f, 0.1f, 100f));
      textured1.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, -1.365f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.Up));
      DefaultEffect.Textured textured2 = textured1;
      mesh2.Effect = (BaseEffect) textured2;
      mesh1.DepthWrites = false;
      mesh1.AlwaysOnTop = true;
      mesh1.Blending = new BlendingMode?(BlendingMode.Opaque);
      Mesh mesh3 = mesh1;
      tileTransition.mesh = mesh3;
      for (int index1 = 0; index1 < 1; ++index1)
      {
        for (int index2 = 0; index2 < 1; ++index2)
        {
          float x = (float) index1 / 1f;
          float num1 = (float) (index1 + 1) / 1f;
          float y = (float) index2 / 1f;
          float num2 = (float) (index2 + 1) / 1f;
          bool flag1 = RandomHelper.Probability(0.5);
          bool flag2 = RandomHelper.Probability(0.5);
          Group group1 = this.mesh.AddGroup();
          if (flag2)
            group1.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionTexture>(new VertexPositionTexture[4]
            {
              new VertexPositionTexture(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(1f - x, 1f - num2)),
              new VertexPositionTexture(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1f - num1, 1f - num2)),
              new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(1f - x, 1f - y)),
              new VertexPositionTexture(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1f - num1, 1f - y))
            }, new int[6]
            {
              0,
              2,
              1,
              2,
              3,
              1
            }, PrimitiveType.TriangleList);
          else
            group1.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionTexture>(new VertexPositionTexture[4]
            {
              new VertexPositionTexture(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(1f - x, 1f - num2)),
              new VertexPositionTexture(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1f - num1, 1f - num2)),
              new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(1f - x, 1f - y)),
              new VertexPositionTexture(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1f - num1, 1f - y))
            }, new int[6]
            {
              0,
              2,
              1,
              2,
              3,
              1
            }, PrimitiveType.TriangleList);
          group1.Scale = new Vector3(1f, 1f, (float) (1.0 / (flag2 ? 1.0 : 1.0)));
          group1.Position = new Vector3(x, y, 0.0f);
          group1.CustomData = (object) new TileTransition.TileData()
          {
            X = (x + RandomHelper.Centered(0.150000005960464)),
            Y = (y + RandomHelper.Centered(0.150000005960464)),
            B = false,
            Inverted = flag1,
            Vertical = flag2
          };
          group1.Material = new Material();
          Group group2 = this.mesh.AddGroup();
          if (flag2)
          {
            Group group3 = group2;
            VertexPositionTexture[] vertices = new VertexPositionTexture[4]
            {
              new VertexPositionTexture(new Vector3(-0.5f, flag1 ? 0.5f : -0.5f, -0.5f), new Vector2(1f - x, flag1 ? 1f - y : 1f - num2)),
              new VertexPositionTexture(new Vector3(0.5f, flag1 ? 0.5f : -0.5f, -0.5f), new Vector2(1f - num1, flag1 ? 1f - y : 1f - num2)),
              new VertexPositionTexture(new Vector3(-0.5f, flag1 ? 0.5f : -0.5f, 0.5f), new Vector2(1f - x, flag1 ? 1f - num2 : 1f - y)),
              new VertexPositionTexture(new Vector3(0.5f, flag1 ? 0.5f : -0.5f, 0.5f), new Vector2(1f - num1, flag1 ? 1f - num2 : 1f - y))
            };
            int[] indices;
            if (!flag1)
              indices = new int[6]
              {
                0,
                2,
                1,
                2,
                3,
                1
              };
            else
              indices = new int[6]
              {
                0,
                1,
                2,
                2,
                1,
                3
              };
            int num3 = 0;
            IndexedUserPrimitives<VertexPositionTexture> indexedUserPrimitives = new IndexedUserPrimitives<VertexPositionTexture>(vertices, indices, (PrimitiveType) num3);
            group3.Geometry = (IIndexedPrimitiveCollection) indexedUserPrimitives;
          }
          else
          {
            Group group3 = group2;
            VertexPositionTexture[] vertices = new VertexPositionTexture[4]
            {
              new VertexPositionTexture(new Vector3(flag1 ? 0.5f : -0.5f, 0.5f, 0.5f), new Vector2(flag1 ? 1f - num1 : 1f - x, 1f - num2)),
              new VertexPositionTexture(new Vector3(flag1 ? 0.5f : -0.5f, 0.5f, -0.5f), new Vector2(flag1 ? 1f - x : 1f - num1, 1f - num2)),
              new VertexPositionTexture(new Vector3(flag1 ? 0.5f : -0.5f, -0.5f, 0.5f), new Vector2(flag1 ? 1f - num1 : 1f - x, 1f - y)),
              new VertexPositionTexture(new Vector3(flag1 ? 0.5f : -0.5f, -0.5f, -0.5f), new Vector2(flag1 ? 1f - x : 1f - num1, 1f - y))
            };
            int[] indices;
            if (!flag1)
              indices = new int[6]
              {
                0,
                2,
                1,
                2,
                3,
                1
              };
            else
              indices = new int[6]
              {
                0,
                1,
                2,
                2,
                1,
                3
              };
            int num3 = 0;
            IndexedUserPrimitives<VertexPositionTexture> indexedUserPrimitives = new IndexedUserPrimitives<VertexPositionTexture>(vertices, indices, (PrimitiveType) num3);
            group3.Geometry = (IIndexedPrimitiveCollection) indexedUserPrimitives;
          }
          group2.Scale = new Vector3(1f, 1f, (float) (1.0 / (flag2 ? 1.0 : 1.0)));
          group2.Position = new Vector3(x, y, 0.0f);
          group2.CustomData = (object) new TileTransition.TileData()
          {
            X = (x + RandomHelper.Centered(0.150000005960464)),
            Y = (y + RandomHelper.Centered(0.150000005960464)),
            B = true,
            Inverted = flag1,
            Vertical = flag2
          };
          group2.Material = new Material();
        }
      }
      this.mesh.Position = new Vector3(0.0f, 0.0f, 0.0f);
      this.textureA = this.TargetRenderer.TakeTarget();
      this.textureB = this.TargetRenderer.TakeTarget();
      foreach (Group group in this.mesh.Groups)
        group.Texture = !((TileTransition.TileData) group.CustomData).B ? (Texture) this.textureA.Target : (Texture) this.textureB.Target;
      this.TargetRenderer.ScheduleHook(this.DrawOrder, this.textureA.Target);
      SoundEffectExtensions.Emit(TileTransition.sTransition);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.textureA != null)
      {
        this.TargetRenderer.ReturnTarget(this.textureA);
        this.TargetRenderer.UnscheduleHook(this.textureA.Target);
      }
      if (this.textureB != null)
      {
        this.TargetRenderer.ReturnTarget(this.textureB);
        this.TargetRenderer.UnscheduleHook(this.textureB.Target);
      }
      this.textureA = this.textureB = (RenderTargetHandle) null;
      this.mesh.Dispose();
      this.IsDisposed = true;
      TileTransition.CurrentTransition = (TileTransition) null;
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.tbCaptured)
        return;
      this.sinceStarted += (float) (gameTime.ElapsedGameTime.TotalSeconds * 1.5);
      int num1 = 0;
      foreach (Group group in this.mesh.Groups)
      {
        TileTransition.TileData tileData = (TileTransition.TileData) group.CustomData;
        float num2 = Easing.EaseOut((double) FezMath.Saturate(this.sinceStarted), EasingType.Quadratic) * 1.570796f;
        group.Rotation = Quaternion.CreateFromAxisAngle(tileData.Vertical ? Vector3.Left : Vector3.Up, tileData.Inverted ? num2 : -num2);
        group.Material.Diffuse = new Vector3(0.125f) + 0.875f * (tileData.B ? new Vector3((float) Math.Sin((double) num2)) : new Vector3((float) (1.0 - Math.Sin((double) num2))));
        if ((double) num2 >= 1.57079637050629)
          ++num1;
      }
      if (num1 != this.mesh.Groups.Count)
        return;
      ServiceHelper.RemoveComponent<TileTransition>(this);
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.taCaptured && this.TargetRenderer.IsHooked(this.textureA.Target))
      {
        this.TargetRenderer.Resolve(this.textureA.Target, false);
        this.taCaptured = true;
        if (this.ScreenCaptured != null)
        {
          this.ScreenCaptured();
          this.ScreenCaptured = (Action) null;
        }
      }
      if (this.TargetRenderer.IsHooked(this.textureB.Target))
      {
        this.TargetRenderer.Resolve(this.textureB.Target, true);
        this.tbCaptured = true;
        this.WaitFor = (Func<bool>) null;
      }
      if ((this.WaitFor == null || this.WaitFor()) && !this.TargetRenderer.IsHooked(this.textureB.Target) && !this.tbCaptured)
        this.TargetRenderer.ScheduleHook(this.DrawOrder, this.textureB.Target);
      this.GraphicsDevice.Clear(Color.Black);
      SettingsManager.SetupViewport(this.GraphicsDevice, false);
      this.mesh.Draw();
    }

    private struct TileData
    {
      public float X;
      public float Y;
      public bool B;
      public bool Inverted;
      public bool Vertical;
    }
  }
}
