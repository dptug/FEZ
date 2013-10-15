// Type: FezGame.Components.SaveIndicator
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Effects;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  public class SaveIndicator : DrawableGameComponent
  {
    private float sinceLastSaveStarted = 4f;
    private const float FadeInTime = 0.1f;
    private const float FadeOutTime = 0.1f;
    private const float LongShowTime = 0.5f;
    private const float ShortShowTime = 0.5f;
    private Mesh mesh;
    private float planeOpacity;
    private bool wasSaving;
    private float sinceLoadingVisible;
    private float currentShowTime;

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public SaveIndicator(Game game)
      : base(game)
    {
      this.DrawOrder = 2101;
    }

    protected override void LoadContent()
    {
      float aspectRatio = this.GraphicsDevice.Viewport.AspectRatio;
      SaveIndicator saveIndicator = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
      vertexColored1.ForcedProjectionMatrix = new Matrix?(Matrix.CreateOrthographic(14f * aspectRatio, 14f, 0.1f, 100f));
      vertexColored1.ForcedViewMatrix = new Matrix?(Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10f), Vector3.Zero, Vector3.Up));
      DefaultEffect.VertexColored vertexColored2 = vertexColored1;
      mesh2.Effect = (BaseEffect) vertexColored2;
      mesh1.Blending = new BlendingMode?(BlendingMode.Alphablending);
      mesh1.AlwaysOnTop = true;
      mesh1.DepthWrites = false;
      mesh1.Position = new Vector3(5.5f * aspectRatio, -4.5f, 0.0f);
      Mesh mesh3 = mesh1;
      saveIndicator.mesh = mesh3;
      this.mesh.AddFace(Vector3.One, Vector3.Zero, FaceOrientation.Front, Color.Red, true);
    }

    public override void Update(GameTime gameTime)
    {
      float num = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.sinceLoadingVisible = this.GameState.LoadingVisible ? FezMath.Saturate(this.sinceLoadingVisible + num * 2f) : FezMath.Saturate(this.sinceLoadingVisible - num * 3f);
      if (this.GameState.Saving || this.GameState.IsAchievementSave)
      {
        if (!this.wasSaving)
        {
          this.wasSaving = true;
          this.sinceLastSaveStarted = 0.0f;
          this.currentShowTime = this.GameState.IsAchievementSave ? 0.5f : 0.5f;
          this.GameState.IsAchievementSave = false;
        }
      }
      else if (this.wasSaving)
        this.wasSaving = false;
      if ((double) this.sinceLastSaveStarted < (double) this.currentShowTime + 0.200000002980232)
        this.sinceLastSaveStarted += num;
      this.planeOpacity = FezMath.Saturate(this.sinceLastSaveStarted / 0.1f) * FezMath.Saturate((float) (((double) this.currentShowTime - (double) this.sinceLastSaveStarted + 0.200000002980232) / 0.100000001490116));
    }

    public override void Draw(GameTime gameTime)
    {
      if ((double) this.planeOpacity == 0.0 || Fez.LongScreenshot)
        return;
      this.mesh.Material.Opacity = this.planeOpacity;
      this.mesh.FirstGroup.Position = new Vector3(0.0f, (float) (1.75 * (double) Easing.EaseIn((double) this.sinceLoadingVisible, EasingType.Quadratic) * (this.GameState.DotLoading ? 0.0 : 1.0)), 0.0f);
      this.mesh.FirstGroup.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float) (-gameTime.ElapsedGameTime.TotalSeconds * 3.0)) * this.mesh.FirstGroup.Rotation;
      this.mesh.Draw();
    }
  }
}
