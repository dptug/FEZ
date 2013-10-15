// Type: FezGame.Components.FlickeringNeon
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  public class FlickeringNeon : GameComponent
  {
    private readonly List<FlickeringNeon.NeonState> NeonPlanes = new List<FlickeringNeon.NeonState>();
    private readonly List<SoundEffect> Glitches = new List<SoundEffect>();

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public FlickeringNeon(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TrackNeons);
      this.Enabled = false;
    }

    private void TrackNeons()
    {
      this.NeonPlanes.Clear();
      this.Glitches.Clear();
      this.Enabled = false;
      foreach (BackgroundPlane backgroundPlane in (IEnumerable<BackgroundPlane>) this.LevelManager.BackgroundPlanes.Values)
      {
        if (backgroundPlane.TextureName != null && backgroundPlane.TextureName.EndsWith("_GLOW") && backgroundPlane.TextureName.Contains("NEON"))
          this.NeonPlanes.Add(new FlickeringNeon.NeonState()
          {
            Neon = backgroundPlane,
            Time = RandomHelper.Between(2.0, 4.0)
          });
      }
      this.Enabled = this.NeonPlanes.Count > 0;
      if (!this.Enabled)
        return;
      this.Glitches.Add(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/Glitches/Glitch1"));
      this.Glitches.Add(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/Glitches/Glitch2"));
      this.Glitches.Add(this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Intro/Elders/Glitches/Glitch3"));
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.InMap || (this.GameState.InMenuCube || this.GameState.Paused))
        return;
      bool flag1 = !FezMath.IsOrthographic(this.CameraManager.Viewpoint);
      Vector3 forward = this.CameraManager.InverseView.Forward;
      BoundingFrustum frustum = this.CameraManager.Frustum;
      foreach (FlickeringNeon.NeonState neonState in this.NeonPlanes)
      {
        neonState.Time -= (float) gameTime.ElapsedGameTime.TotalSeconds;
        if ((double) neonState.Time <= 0.0)
        {
          if (neonState.FlickersLeft == 0)
            neonState.FlickersLeft = RandomHelper.Random.Next(4, 18);
          BackgroundPlane backgroundPlane = neonState.Neon;
          bool flag2 = backgroundPlane.Visible = !backgroundPlane.Hidden && (flag1 || backgroundPlane.Doublesided || (backgroundPlane.Crosshatch || backgroundPlane.Billboard) || (double) FezMath.Dot(forward, backgroundPlane.Forward) > 0.0) && frustum.Contains(backgroundPlane.Bounds) != ContainmentType.Disjoint;
          neonState.Enabled = !neonState.Enabled;
          neonState.Neon.Hidden = neonState.Enabled;
          neonState.Neon.Visible = !neonState.Neon.Hidden;
          neonState.Neon.Update();
          if (flag2 && RandomHelper.Probability(0.5))
            SoundEffectExtensions.EmitAt(RandomHelper.InList<SoundEffect>(this.Glitches), neonState.Neon.Position, false, RandomHelper.Centered(0.100000001490116), RandomHelper.Between(0.0, 1.0), false);
          neonState.Time = Easing.EaseIn((double) RandomHelper.Between(0.0, 0.449999988079071), EasingType.Quadratic);
          --neonState.FlickersLeft;
          if (neonState.FlickersLeft == 0)
          {
            neonState.Enabled = true;
            neonState.Neon.Hidden = false;
            neonState.Neon.Visible = true;
            neonState.Neon.Update();
            neonState.Time = RandomHelper.Between(3.0, 8.0);
          }
        }
      }
    }

    private class NeonState
    {
      public BackgroundPlane Neon;
      public float Time;
      public bool Enabled;
      public int FlickersLeft;
    }
  }
}
