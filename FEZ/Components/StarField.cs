// Type: FezGame.Components.StarField
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  public class StarField : DrawableGameComponent
  {
    private static readonly Color[] Colors = new Color[11]
    {
      new Color(20, 1, 28),
      new Color(108, 27, 44),
      new Color(225, 125, 53),
      new Color(246, 231, 108),
      new Color(155, 226, 177),
      new Color(67, 246, (int) byte.MaxValue),
      new Color(100, 154, 224),
      new Color(214, 133, 180),
      new Color(189, 63, 117),
      new Color(98, 21, 88),
      new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)
    };
    public float Opacity = 1f;
    private static IIndexedPrimitiveCollection StarGeometry;
    private Mesh StarsMesh;
    private Mesh TrailsMesh;
    private FakePointSpritesEffect StarEffect;
    private bool Done;
    private TimeSpan sinceStarted;
    private Matrix? savedViewMatrix;

    public bool HasHorizontalTrails { get; set; }

    public bool ReverseTiming { get; set; }

    public bool FollowCamera { get; set; }

    public bool HasZoomed { get; set; }

    public float AdditionalZoom { get; set; }

    public float AdditionalScale { get; set; }

    public bool IsDisposed { get; private set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    static StarField()
    {
    }

    public StarField(Game game)
      : base(game)
    {
      this.DrawOrder = 2006;
      this.Enabled = false;
      this.Visible = false;
    }

    public override void Initialize()
    {
      base.Initialize();
      StarField starField1 = this;
      Mesh mesh1 = new Mesh();
      mesh1.AlwaysOnTop = true;
      mesh1.DepthWrites = false;
      mesh1.Blending = new BlendingMode?(BlendingMode.Additive);
      mesh1.Culling = CullMode.None;
      Mesh mesh2 = mesh1;
      StarField starField2 = this;
      FakePointSpritesEffect pointSpritesEffect1 = new FakePointSpritesEffect();
      pointSpritesEffect1.ForcedProjectionMatrix = new Matrix?(Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(75f), this.CameraManager.AspectRatio, 0.1f, 1000f));
      FakePointSpritesEffect pointSpritesEffect2;
      FakePointSpritesEffect pointSpritesEffect3 = pointSpritesEffect2 = pointSpritesEffect1;
      starField2.StarEffect = pointSpritesEffect2;
      FakePointSpritesEffect pointSpritesEffect4 = pointSpritesEffect3;
      mesh2.Effect = (BaseEffect) pointSpritesEffect4;
      Mesh mesh3 = mesh1;
      starField1.StarsMesh = mesh3;
      if (this.HasHorizontalTrails)
      {
        StarField starField3 = this;
        Mesh mesh4 = new Mesh();
        mesh4.AlwaysOnTop = true;
        mesh4.DepthWrites = false;
        mesh4.Blending = new BlendingMode?(BlendingMode.Additive);
        Mesh mesh5 = mesh4;
        HorizontalTrailsEffect horizontalTrailsEffect1 = new HorizontalTrailsEffect();
        horizontalTrailsEffect1.ForcedProjectionMatrix = this.StarEffect.ForcedProjectionMatrix;
        HorizontalTrailsEffect horizontalTrailsEffect2 = horizontalTrailsEffect1;
        mesh5.Effect = (BaseEffect) horizontalTrailsEffect2;
        Mesh mesh6 = mesh4;
        starField3.TrailsMesh = mesh6;
      }
      this.AddStars();
      if (this.FollowCamera)
        return;
      this.StarEffect.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up));
      if (!this.HasHorizontalTrails)
        return;
      (this.TrailsMesh.Effect as HorizontalTrailsEffect).ForcedViewMatrix = this.StarEffect.ForcedViewMatrix;
    }

    private void AddStars()
    {
      Texture2D texture2D = this.CMProvider.Global.Load<Texture2D>("Other Textures/FullWhite");
      if (StarField.StarGeometry != null && !this.HasHorizontalTrails)
      {
        Group group = this.StarsMesh.AddGroup();
        group.Texture = (Texture) texture2D;
        group.Geometry = StarField.StarGeometry;
      }
      else
      {
        Color[] pointColors = (Color[]) null;
        Vector3[] pointPairs = (Vector3[]) null;
        float num1 = 49f;
        float num2 = num1;
        Vector3[] pointCenters = new Vector3[(int) ((double) num2 * (double) num1 * (double) num2)];
        if (this.HasHorizontalTrails)
        {
          pointColors = new Color[(int) ((double) num2 * (double) num1 * (double) num2) * 2];
          pointPairs = new Vector3[(int) ((double) num2 * (double) num1 * (double) num2) * 2];
        }
        Random random = RandomHelper.Random;
        int num3 = 0;
        int index1 = 0;
        for (int index2 = 0; (double) index2 < (double) num2; ++index2)
        {
          for (int index3 = 0; (double) index3 < (double) num1; ++index3)
          {
            for (int index4 = 0; (double) index4 < (double) num2; ++index4)
            {
              Vector3 vector3 = new Vector3((float) (((double) index2 - (double) num2 / 2.0) * 100.0), (float) (((double) index3 - (double) num1 / 2.0) * 100.0), (float) (((double) index4 - (double) num2 / 2.0) * 100.0));
              pointCenters[num3++] = vector3;
              if (this.HasHorizontalTrails)
              {
                pointPairs[index1] = vector3;
                pointPairs[index1 + 1] = vector3;
                byte num4 = (byte) random.Next(0, 256);
                byte num5 = (byte) random.Next(0, 256);
                pointColors[index1] = new Color((int) num5, 0, (int) num4, 0);
                pointColors[index1 + 1] = new Color((int) num5, 0, (int) num4, (int) byte.MaxValue);
                index1 += 2;
              }
            }
          }
        }
        Group group;
        StarField.AddPoints(group = this.StarsMesh.AddGroup(), pointCenters, (Texture) texture2D, 2f);
        StarField.StarGeometry = group.Geometry;
        if (!this.HasHorizontalTrails)
          return;
        this.TrailsMesh.AddLines(pointColors, pointPairs, true);
      }
    }

    private static void AddPoints(Group g, Vector3[] pointCenters, Texture texture, float size)
    {
      BufferedIndexedPrimitives<VertexFakePointSprite> indexedPrimitives = new BufferedIndexedPrimitives<VertexFakePointSprite>(PrimitiveType.TriangleList);
      g.Geometry = (IIndexedPrimitiveCollection) indexedPrimitives;
      indexedPrimitives.Vertices = new VertexFakePointSprite[pointCenters.Length * 4];
      indexedPrimitives.Indices = new int[pointCenters.Length * 6];
      Random random = RandomHelper.Random;
      int length = StarField.Colors.Length;
      VertexFakePointSprite[] vertices = indexedPrimitives.Vertices;
      int[] indices = indexedPrimitives.Indices;
      for (int index1 = 0; index1 < pointCenters.Length; ++index1)
      {
        Color color = StarField.Colors[random.Next(0, length)];
        int index2 = index1 * 4;
        vertices[index2] = new VertexFakePointSprite(pointCenters[index1], color, new Vector2(0.0f, 0.0f), new Vector2(-size, -size));
        vertices[index2 + 1] = new VertexFakePointSprite(pointCenters[index1], color, new Vector2(1f, 0.0f), new Vector2(size, -size));
        vertices[index2 + 2] = new VertexFakePointSprite(pointCenters[index1], color, new Vector2(1f, 1f), new Vector2(size, size));
        vertices[index2 + 3] = new VertexFakePointSprite(pointCenters[index1], color, new Vector2(0.0f, 1f), new Vector2(-size, size));
        int index3 = index1 * 6;
        indices[index3] = index2;
        indices[index3 + 1] = index2 + 1;
        indices[index3 + 2] = index2 + 2;
        indices[index3 + 3] = index2;
        indices[index3 + 4] = index2 + 2;
        indices[index3 + 5] = index2 + 3;
      }
      indexedPrimitives.UpdateBuffers();
      indexedPrimitives.CleanUp();
      g.Texture = texture;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.TrailsMesh != null)
        this.TrailsMesh.Dispose();
      this.TrailsMesh = (Mesh) null;
      if (this.StarsMesh != null)
        this.StarsMesh.Effect.Dispose();
      this.StarsMesh = (Mesh) null;
      this.IsDisposed = true;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.ReverseTiming)
      {
        this.sinceStarted -= gameTime.ElapsedGameTime;
        this.sinceStarted -= gameTime.ElapsedGameTime;
      }
      else
        this.sinceStarted += gameTime.ElapsedGameTime;
      float num = Easing.EaseIn(this.sinceStarted.TotalSeconds / 3.0, EasingType.Quartic);
      if (this.HasHorizontalTrails)
        (this.TrailsMesh.Effect as HorizontalTrailsEffect).Timing = (float) this.sinceStarted.TotalSeconds;
      if (!this.FollowCamera)
      {
        this.AdditionalZoom = (float) FezMath.AsNumeric(this.HasZoomed) + num / 3f;
        this.AdditionalScale = num / 6f;
      }
      if (!this.HasHorizontalTrails && (double) num > 40.0 && !this.Done)
      {
        this.Done = true;
        ServiceHelper.RemoveComponent<StarField>(this);
      }
      if (!this.HasHorizontalTrails || !(this.sinceStarted <= TimeSpan.Zero))
        return;
      this.Enabled = false;
      this.sinceStarted = TimeSpan.Zero;
    }

    public override void Draw(GameTime gameTime)
    {
      this.TargetRenderer.DrawFullscreen(Color.Black);
      this.Draw();
    }

    public void Draw()
    {
      if (!this.FollowCamera)
      {
        this.StarsMesh.Position = this.AdditionalZoom * Vector3.Forward * 125f - 2400f * Vector3.Forward;
        this.StarsMesh.Scale = new Vector3(1f + this.AdditionalScale, 1f + this.AdditionalScale, 1f);
      }
      else if (!this.GameState.InFpsMode)
      {
        FakePointSpritesEffect pointSpritesEffect = this.StarEffect;
        StarField starField1 = this;
        StarField starField2 = this;
        Matrix? nullable1 = new Matrix?();
        Matrix? nullable2 = nullable1;
        starField2.savedViewMatrix = nullable2;
        Matrix? nullable3;
        Matrix? nullable4 = nullable3 = nullable1;
        starField1.savedViewMatrix = nullable3;
        Matrix? nullable5 = nullable4;
        pointSpritesEffect.ForcedViewMatrix = nullable5;
        this.StarsMesh.Position = this.CameraManager.InterpolatedCenter * 0.5f;
        this.StarsMesh.Scale = new Vector3((float) (112.5 / ((double) this.CameraManager.Radius / (double) SettingsManager.GetViewScale(this.GraphicsDevice) + 40.0)));
        if (this.HasHorizontalTrails)
        {
          this.TrailsMesh.Position = this.StarsMesh.Position;
          this.TrailsMesh.Scale = this.StarsMesh.Scale = new Vector3((float) (65.0 / ((double) this.CameraManager.Radius / (double) SettingsManager.GetViewScale(this.GraphicsDevice) + 25.0)));
        }
      }
      else if (this.CameraManager.ProjectionTransition)
      {
        if (this.CameraManager.Viewpoint != Viewpoint.Perspective)
          this.StarEffect.ForcedViewMatrix = new Matrix?(Matrix.Lerp(this.CameraManager.View, this.CameraManager.View, Easing.EaseOut((double) this.CameraManager.ViewTransitionStep, EasingType.Quadratic)));
        else if (!this.savedViewMatrix.HasValue)
          this.savedViewMatrix = new Matrix?(this.CameraManager.View);
      }
      this.StarsMesh.Material.Opacity = this.Opacity;
      this.StarsMesh.Draw();
      if (!this.Enabled || !this.HasHorizontalTrails)
        return;
      this.TrailsMesh.Draw();
    }
  }
}
