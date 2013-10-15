// Type: FezGame.Components.PolytronLogo
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
using System.Linq;

namespace FezGame.Components
{
  internal class PolytronLogo : DrawableGameComponent
  {
    private static readonly Color[] StripColors = new Color[4]
    {
      new Color(0, 170, (int) byte.MaxValue),
      new Color((int) byte.MaxValue, (int) byte.MaxValue, 0),
      new Color((int) byte.MaxValue, 106, 0),
      new Color(0, 0, 0)
    };
    private const int Detail = 100;
    private const float StripThickness = 0.091f;
    private const float Zoom = 0.7f;
    private Mesh LogoMesh;
    private Texture2D PolytronText;
    private SoundEffect sPolytron;
    private SoundEmitter iPolytron;
    private SpriteBatch spriteBatch;
    private float SinceStarted;

    public float Opacity { get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    static PolytronLogo()
    {
    }

    public PolytronLogo(Game game)
      : base(game)
    {
      this.Visible = false;
      this.Enabled = false;
    }

    public override void Initialize()
    {
      base.Initialize();
      PolytronLogo polytronLogo = this;
      Mesh mesh1 = new Mesh();
      mesh1.AlwaysOnTop = true;
      Mesh mesh2 = mesh1;
      DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
      vertexColored1.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(1.428571f, 1.428571f, 0.1f, 100f));
      vertexColored1.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(Vector3.UnitZ, -Vector3.UnitZ, Vector3.Up));
      DefaultEffect.VertexColored vertexColored2 = vertexColored1;
      mesh2.Effect = (BaseEffect) vertexColored2;
      Mesh mesh3 = mesh1;
      polytronLogo.LogoMesh = mesh3;
      for (int index1 = 0; index1 < 4; ++index1)
      {
        FezVertexPositionColor[] vertices = new FezVertexPositionColor[202];
        for (int index2 = 0; index2 < vertices.Length; ++index2)
          vertices[index2] = new FezVertexPositionColor(Vector3.Zero, PolytronLogo.StripColors[index1]);
        this.LogoMesh.AddGroup().Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<FezVertexPositionColor>(vertices, Enumerable.ToArray<int>(Enumerable.Range(0, vertices.Length)), PrimitiveType.TriangleStrip);
      }
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      int width = this.GraphicsDevice.Viewport.Width;
      int height = this.GraphicsDevice.Viewport.Height;
      this.LogoMesh.Position = new Vector3(-0.1975f, -0.25f, 0.0f);
      this.LogoMesh.Scale = new Vector3(new Vector2(500f) * viewScale / new Vector2((float) width, (float) height), 1f);
      bool flag = (double) viewScale >= 1.5;
      this.sPolytron = this.CMProvider.Get(CM.Intro).Load<SoundEffect>("Sounds/Intro/PolytronJingle");
      this.PolytronText = this.CMProvider.Get(CM.Intro).Load<Texture2D>("Other Textures/splash/polytron" + (flag ? "_1440" : ""));
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
    }

    protected override void Dispose(bool disposing)
    {
      this.LogoMesh.Dispose();
      this.spriteBatch.Dispose();
    }

    private void UpdateStripe(int i, float step)
    {
      FezVertexPositionColor[] vertices = (this.LogoMesh.Groups[i].Geometry as IndexedUserPrimitives<FezVertexPositionColor>).Vertices;
      Vector3 vector3_1 = Vector3.Zero;
      Vector3 vector3_2 = Vector3.Zero;
      for (int index = 0; index <= 100; ++index)
      {
        if (index < 20)
        {
          float num = (float) index / 20f * FezMath.Saturate(step / 0.2f);
          vector3_1 = vertices[index * 2].Position = new Vector3((float) (i + 1) * 0.091f, num * 0.5f, 0.0f);
          vector3_2 = vertices[index * 2 + 1].Position = new Vector3((float) i * 0.091f, num * 0.5f, 0.0f);
        }
        else if (index > 80 && (double) step > 0.800000011920929)
        {
          float num = (float) (((double) index - 80.0) / 20.0) * FezMath.Saturate((float) (((double) step - 0.800000011920929) / 0.200000002980232 / 0.272000014781952));
          vector3_1 = vertices[index * 2].Position = new Vector3((float) (0.5 - (double) num * 0.136000007390976), (float) (i + 1) * 0.091f, 0.0f);
          vector3_2 = vertices[index * 2 + 1].Position = new Vector3((float) (0.5 - (double) num * 0.136000007390976), (float) i * 0.091f, 0.0f);
        }
        else if (index >= 20 && index <= 80 && (double) step > 0.200000002980232)
        {
          float num = (float) ((double) ((float) (((double) index - 20.0) / 60.0) * FezMath.Saturate((float) (((double) step - 0.200000002980232) / 0.600000023841858))) * 1.57079637050629 * 3.0 - 1.57079637050629);
          vector3_1 = vertices[index * 2].Position = new Vector3((float) (Math.Sin((double) num) * (0.5 - (double) (i + 1) * 0.090999998152256) + 0.5), (float) (Math.Cos((double) num) * (0.5 - (double) (i + 1) * 0.090999998152256) + 0.5), 0.0f);
          vector3_2 = vertices[index * 2 + 1].Position = new Vector3((float) (Math.Sin((double) num) * (0.5 - (double) i * 0.090999998152256) + 0.5), (float) (Math.Cos((double) num) * (0.5 - (double) i * 0.090999998152256) + 0.5), 0.0f);
        }
        else
        {
          vertices[index * 2].Position = vector3_1;
          vertices[index * 2 + 1].Position = vector3_2;
        }
      }
    }

    public void End()
    {
      if (!this.iPolytron.Dead)
        this.iPolytron.FadeOutAndDie(0.1f);
      this.iPolytron = (SoundEmitter) null;
    }

    public override void Update(GameTime gameTime)
    {
      if ((double) this.SinceStarted == 0.0 && (gameTime.ElapsedGameTime.Ticks != 0L && this.sPolytron != null))
      {
        this.iPolytron = SoundEffectExtensions.Emit(this.sPolytron);
        this.sPolytron = (SoundEffect) null;
      }
      this.SinceStarted += (float) gameTime.ElapsedGameTime.TotalSeconds;
      float num = FezMath.Saturate(this.SinceStarted / 1.75f);
      this.UpdateStripe(3, Easing.EaseOut((double) Easing.EaseIn((double) num, EasingType.Quadratic), EasingType.Quartic) * 0.86f);
      this.UpdateStripe(2, Easing.EaseOut((double) Easing.EaseIn((double) num, EasingType.Cubic), EasingType.Quartic) * 0.86f);
      this.UpdateStripe(1, Easing.EaseOut((double) Easing.EaseIn((double) num, EasingType.Quartic), EasingType.Quartic) * 0.86f);
      this.UpdateStripe(0, Easing.EaseOut((double) Easing.EaseIn((double) num, EasingType.Quintic), EasingType.Quartic) * 0.86f);
    }

    public override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
      Vector2 vector2 = FezMath.Round(new Vector2((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height) / 2f);
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      this.LogoMesh.Material.Opacity = this.Opacity;
      this.LogoMesh.Draw();
      this.spriteBatch.Begin();
      float num = Easing.EaseOut((double) FezMath.Saturate((float) (((double) this.SinceStarted - 1.5) / 0.25)), EasingType.Quadratic);
      this.spriteBatch.Draw(this.PolytronText, vector2 + FezMath.Round(new Vector2((float) -this.PolytronText.Width / 2f, 120f * viewScale)), new Color(1f, 1f, 1f, this.Opacity * num));
      this.spriteBatch.End();
    }
  }
}
