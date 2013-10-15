// Type: FezGame.Components.WorldAxisHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Effects;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  internal class WorldAxisHost : DrawableGameComponent
  {
    private Mesh axisMesh;

    public WorldAxisHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.axisMesh = new Mesh()
      {
        AlwaysOnTop = true
      };
      this.axisMesh.AddWireframeArrow(1f, 0.1f, Vector3.Zero, FaceOrientation.Right, Color.Red);
      this.axisMesh.AddWireframeArrow(1f, 0.1f, Vector3.Zero, FaceOrientation.Top, Color.Green);
      this.axisMesh.AddWireframeArrow(1f, 0.1f, Vector3.Zero, FaceOrientation.Front, Color.Blue);
      this.DrawOrder = 1000;
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.axisMesh.Effect = (BaseEffect) new DefaultEffect.VertexColored();
    }

    public override void Draw(GameTime gameTime)
    {
      this.axisMesh.Draw();
    }
  }
}
