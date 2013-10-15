// Type: FezGame.Components.EndCutscene32.TetraordialOoze
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezGame.Components.EndCutscene32
{
  internal class TetraordialOoze : DrawableGameComponent
  {
    private const float NoiseZoomDuration = 14f;
    private const int TetraCount = 2000;
    private readonly EndCutscene32Host Host;
    public Mesh NoiseMesh;
    private Mesh TetraMesh;
    private float Time;
    private TetraordialOoze.State ActiveState;

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    public TetraordialOoze(Game game, EndCutscene32Host host)
      : base(game)
    {
      this.Host = host;
      this.DrawOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.Reset();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.TetraMesh != null)
        this.TetraMesh.Dispose();
      this.TetraMesh = (Mesh) null;
    }

    private void Reset()
    {
      this.CameraManager.Center = Vector3.Zero;
      this.CameraManager.Direction = Vector3.UnitZ;
      this.CameraManager.Radius = 10f;
      this.CameraManager.SnapInterpolation();
      this.TetraMesh = new Mesh()
      {
        Effect = (BaseEffect) new DefaultEffect.VertexColored()
      };
      for (int index = 0; index < 2000; ++index)
      {
        float num1 = RandomHelper.Unit();
        Group group;
        switch (RandomHelper.Random.Next(0, 5))
        {
          case 0:
            group = TetraordialOoze.AddL(this.TetraMesh);
            break;
          case 1:
            group = TetraordialOoze.AddO(this.TetraMesh);
            break;
          case 2:
            group = TetraordialOoze.AddI(this.TetraMesh);
            break;
          case 3:
            group = TetraordialOoze.AddS(this.TetraMesh);
            break;
          default:
            group = TetraordialOoze.AddT(this.TetraMesh);
            break;
        }
        if (index == 0)
        {
          group.Position = Vector3.Zero;
          num1 = 0.0f;
        }
        else
        {
          float num2 = 5.714286f;
          Vector3 vector3 = new Vector3(RandomHelper.Between(-(double) num2 / 2.0, (double) num2 / 2.0), RandomHelper.Between(-(double) num2 / 2.0, (double) num2 / 2.0) / this.CameraManager.AspectRatio, -1f);
          while ((double) vector3.LengthSquared() <= 1.00999999046326)
            vector3 = new Vector3(RandomHelper.Between(-(double) num2 / 2.0, (double) num2 / 2.0), RandomHelper.Between(-(double) num2 / 2.0, (double) num2 / 2.0) / this.CameraManager.AspectRatio, -1f);
          group.Position = vector3;
        }
        group.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, RandomHelper.Between(0.0, 6.28318548202515));
        group.Scale = new Vector3(MathHelper.Lerp(0.0005f, 0.0075f, 1f - num1));
        group.Material = new Material()
        {
          Diffuse = new Vector3(1f - num1),
          Opacity = 0.0f
        };
      }
    }

    private static Group AddL(Mesh m)
    {
      Vector3 vector3 = new Vector3(4f, 1f, 0.0f);
      Group group = m.AddGroup();
      group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionColor>(new VertexPositionColor[6]
      {
        new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(4f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(4f, 1f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(1f, 1f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(1f, 2f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(0.0f, 2f, 0.0f) - vector3 / 2f, Color.White)
      }, new int[7]
      {
        0,
        1,
        2,
        3,
        4,
        5,
        0
      }, PrimitiveType.LineStrip);
      return group;
    }

    private static Group AddO(Mesh m)
    {
      Vector3 vector3 = new Vector3(2f, 2f, 0.0f);
      Group group = m.AddGroup();
      group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionColor>(new VertexPositionColor[4]
      {
        new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(2f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(2f, 2f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(0.0f, 2f, 0.0f) - vector3 / 2f, Color.White)
      }, new int[5]
      {
        0,
        1,
        2,
        3,
        0
      }, PrimitiveType.LineStrip);
      return group;
    }

    private static Group AddI(Mesh m)
    {
      Vector3 vector3 = new Vector3(4f, 1f, 0.0f);
      Group group = m.AddGroup();
      group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionColor>(new VertexPositionColor[4]
      {
        new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(4f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(4f, 1f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(0.0f, 1f, 0.0f) - vector3 / 2f, Color.White)
      }, new int[5]
      {
        0,
        1,
        2,
        3,
        0
      }, PrimitiveType.LineStrip);
      return group;
    }

    private static Group AddS(Mesh m)
    {
      Vector3 vector3 = new Vector3(3f, 2f, 0.0f);
      Group group = m.AddGroup();
      group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionColor>(new VertexPositionColor[8]
      {
        new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(2f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(2f, 1f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(3f, 1f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(3f, 2f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(1f, 2f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(1f, 1f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(0.0f, 1f, 0.0f) - vector3 / 2f, Color.White)
      }, new int[9]
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        0
      }, PrimitiveType.LineStrip);
      return group;
    }

    private static Group AddT(Mesh m)
    {
      Vector3 vector3 = new Vector3(3f, 2f, 0.0f);
      Group group = m.AddGroup();
      group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionColor>(new VertexPositionColor[8]
      {
        new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(3f, 0.0f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(3f, 1f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(2f, 1f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(2f, 2f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(1f, 2f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(1f, 1f, 0.0f) - vector3 / 2f, Color.White),
        new VertexPositionColor(new Vector3(0.0f, 1f, 0.0f) - vector3 / 2f, Color.White)
      }, new int[9]
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        0
      }, PrimitiveType.LineStrip);
      return group;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.Loading)
        return;
      this.Time += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (this.ActiveState != TetraordialOoze.State.Zoom)
        return;
      if ((double) this.Time == 0.0)
      {
        this.CameraManager.Center = Vector3.Zero;
        this.CameraManager.Direction = Vector3.UnitZ;
        this.CameraManager.Radius = 10f;
        this.CameraManager.SnapInterpolation();
      }
      if ((double) this.Time == 0.0)
      {
        this.NoiseMesh.Scale = new Vector3(1.87495f);
        this.TetraMesh.Scale = Vector3.One;
      }
      for (int index = 0; index < this.TetraMesh.Groups.Count / 10; ++index)
        this.SwapTetraminos();
      if (this.TetraMesh.Groups.Count < 10 && RandomHelper.Probability(0.100000001490116))
        this.SwapTetraminos();
      float amount = FezMath.Saturate(this.Time / 14f);
      if ((double) amount != 1.0)
      {
        this.NoiseMesh.Scale *= MathHelper.Lerp(1.0025f, 1.01625f, amount);
        IGameCameraManager cameraManager = this.CameraManager;
        double num = (double) cameraManager.Radius / (double) MathHelper.Lerp(1.0025f, 1.01625f, amount);
        cameraManager.Radius = (float) num;
        this.CameraManager.SnapInterpolation();
      }
      float num1 = MathHelper.Lerp(0.0f, 1f, Easing.EaseIn((double) FezMath.Saturate(amount * 4f), EasingType.Linear));
      foreach (Group group in this.TetraMesh.Groups)
        group.Material.Opacity = num1;
      this.NoiseMesh.Material.Opacity = 1f - FezMath.Saturate(amount * 1.5f);
      if ((double) amount != 1.0)
        return;
      this.ChangeState();
    }

    private void SwapTetraminos()
    {
      int i1 = RandomHelper.Random.Next(0, this.TetraMesh.Groups.Count);
      int i2 = RandomHelper.Random.Next(0, this.TetraMesh.Groups.Count);
      Group group1 = this.TetraMesh.Groups[i1];
      Group group2 = this.TetraMesh.Groups[i2];
      if (this.CameraManager.Frustum.Contains(Vector3.Transform(group1.Position, (Matrix) group1.WorldMatrix)) == ContainmentType.Disjoint)
        this.TetraMesh.RemoveGroupAt(i1);
      else if (this.CameraManager.Frustum.Contains(Vector3.Transform(group2.Position, (Matrix) group2.WorldMatrix)) == ContainmentType.Disjoint)
      {
        this.TetraMesh.RemoveGroupAt(i2);
      }
      else
      {
        Vector3 position = group1.Position;
        Material material = group1.Material;
        Vector3 scale = group1.Scale;
        group1.Position = group2.Position;
        group1.Scale = group2.Scale;
        group1.Material = group2.Material;
        group2.Position = position;
        group2.Scale = scale;
        group2.Material = material;
      }
    }

    private void ChangeState()
    {
      if (this.ActiveState == TetraordialOoze.State.Zoom)
      {
        this.Host.Cycle();
      }
      else
      {
        this.Time = 0.0f;
        ++this.ActiveState;
        this.Update(new GameTime());
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      this.GraphicsDevice.Clear(EndCutscene32Host.PurpleBlack);
      if (this.ActiveState != TetraordialOoze.State.Zoom)
        return;
      if ((double) this.NoiseMesh.Material.Opacity > 0.0)
        this.NoiseMesh.Draw();
      if (FezMath.AlmostEqual(this.TetraMesh.FirstGroup.Material.Opacity, 0.0f))
        return;
      this.TetraMesh.Draw();
    }

    private enum State
    {
      Zoom,
    }
  }
}
