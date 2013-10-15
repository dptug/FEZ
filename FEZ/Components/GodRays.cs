// Type: FezGame.Components.GodRays
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  public class GodRays : DrawableGameComponent
  {
    private readonly Dictionary<Viewpoint, Mesh> Meshes = new Dictionary<Viewpoint, Mesh>((IEqualityComparer<Viewpoint>) ViewpointComparer.Default);
    private bool viewLocked;
    private Viewpoint lockedTo;

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IDotManager DotManager { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { get; set; }

    public GodRays(Game game)
      : base(game)
    {
      this.DrawOrder = 1;
      this.UpdateOrder = 11;
      this.Enabled = this.Visible = false;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.Meshes.Add(Viewpoint.Front, new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false,
        Blending = new BlendingMode?(BlendingMode.Additive),
        Culling = CullMode.CullClockwiseFace,
        Effect = (BaseEffect) new DefaultEffect.VertexColored(),
        CustomRenderingHandler = new Mesh.RenderingHandler(this.DrawLayered),
        Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.0f)
      });
      this.Meshes.Add(Viewpoint.Right, new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false,
        Blending = new BlendingMode?(BlendingMode.Additive),
        Culling = CullMode.CullClockwiseFace,
        Effect = (BaseEffect) new DefaultEffect.VertexColored(),
        CustomRenderingHandler = new Mesh.RenderingHandler(this.DrawLayered),
        Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, 1.570796f)
      });
      this.Meshes.Add(Viewpoint.Back, new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false,
        Blending = new BlendingMode?(BlendingMode.Additive),
        Culling = CullMode.CullClockwiseFace,
        Effect = (BaseEffect) new DefaultEffect.VertexColored(),
        CustomRenderingHandler = new Mesh.RenderingHandler(this.DrawLayered),
        Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, -3.141593f)
      });
      this.Meshes.Add(Viewpoint.Left, new Mesh()
      {
        AlwaysOnTop = true,
        DepthWrites = false,
        Blending = new BlendingMode?(BlendingMode.Additive),
        Culling = CullMode.CullClockwiseFace,
        Effect = (BaseEffect) new DefaultEffect.VertexColored(),
        CustomRenderingHandler = new Mesh.RenderingHandler(this.DrawLayered),
        Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, -1.570796f)
      });
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
    }

    private void TryInitialize()
    {
      bool enabled = this.Enabled;
      this.Enabled = this.Visible = this.LevelManager.Sky != null && this.LevelManager.Sky.Name == "WATERFRONT";
      if (this.Enabled)
      {
        bool wide = this.LevelManager.Name == "NATURE_HUB";
        if (enabled)
          return;
        for (int index = 0; index < 60; ++index)
          this.TryAddRay(wide, true);
      }
      else
      {
        foreach (Mesh mesh in this.Meshes.Values)
        {
          lock (mesh)
            mesh.ClearGroups();
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading && !this.GameState.FarawaySettings.InTransition || (this.GameState.Paused || this.GameState.InMenuCube) || (this.GameState.InMap || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)))
        return;
      if (this.GameState.FarawaySettings.InTransition && !this.viewLocked)
      {
        foreach (Mesh mesh in this.Meshes.Values)
        {
          (mesh.Effect as DefaultEffect).ForcedViewMatrix = new Matrix?(this.CameraManager.View);
          (mesh.Effect as DefaultEffect).ForcedProjectionMatrix = new Matrix?(this.CameraManager.Projection);
          foreach (Group group in mesh.Groups)
            (group.CustomData as GodRays.RayCustomData).AccumulatedTime = TimeSpan.FromSeconds((group.CustomData as GodRays.RayCustomData).AccumulatedTime.TotalSeconds / 8.0 * 6.0);
        }
        this.viewLocked = true;
        this.lockedTo = this.CameraManager.Viewpoint;
      }
      if (!this.GameState.FarawaySettings.InTransition && this.viewLocked)
      {
        foreach (Mesh mesh in this.Meshes.Values)
        {
          (mesh.Effect as DefaultEffect).ForcedViewMatrix = new Matrix?();
          (mesh.Effect as DefaultEffect).ForcedProjectionMatrix = new Matrix?();
        }
        this.viewLocked = false;
        this.lockedTo = Viewpoint.None;
      }
      if (this.DotManager.Behaviour == DotHost.BehaviourType.SpiralAroundWithCamera || Fez.LongScreenshot)
      {
        this.TryAddRay(true, false);
        this.AlignMesh(Viewpoint.Back);
        this.AlignMesh(Viewpoint.Front);
        this.AlignMesh(Viewpoint.Left);
        this.AlignMesh(Viewpoint.Right);
        this.ScrollRays(Viewpoint.Back, gameTime.ElapsedGameTime);
        this.ScrollRays(Viewpoint.Front, gameTime.ElapsedGameTime);
        this.ScrollRays(Viewpoint.Left, gameTime.ElapsedGameTime);
        this.ScrollRays(Viewpoint.Right, gameTime.ElapsedGameTime);
      }
      else if (!this.viewLocked && !this.CameraManager.ProjectionTransition)
      {
        if (RandomHelper.Probability(0.25))
          this.TryAddRay(false, false);
        this.AlignMesh(this.CameraManager.Viewpoint);
        this.ScrollRays(this.CameraManager.Viewpoint, gameTime.ElapsedGameTime, 8f);
      }
      else
      {
        this.ScrollRays(Viewpoint.Back, gameTime.ElapsedGameTime, 6f);
        this.ScrollRays(Viewpoint.Front, gameTime.ElapsedGameTime, 6f);
        this.ScrollRays(Viewpoint.Left, gameTime.ElapsedGameTime, 6f);
        this.ScrollRays(Viewpoint.Right, gameTime.ElapsedGameTime, 6f);
      }
    }

    private void TryAddRay(bool wide, bool midLife)
    {
      Mesh mesh = this.Meshes[FezMath.AsViewpoint(FezMath.OrientationFromPhi((float) RandomHelper.Random.Next(0, 4) * 1.570796f))];
      if (mesh.Groups.Count >= 15)
        return;
      float num1 = wide ? this.CameraManager.Radius * 2f : this.CameraManager.Radius;
      Vector3 vector3 = RandomHelper.Between(-(double) num1 / 2.0, (double) (num1 / 2f)) * Vector3.UnitX;
      float num2 = RandomHelper.Between(0.75, 4.0);
      float num3 = 8f + RandomHelper.Centered(4.0);
      lock (mesh)
      {
        Group local_9 = mesh.AddColoredQuad(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(-num3 - num2, -num3, 0.0f), new Vector3(-num3, -num3, 0.0f), new Vector3(-num2, 0.0f, 0.0f), new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), Color.Black, Color.Black, new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
        local_9.CustomData = (object) new GodRays.RayCustomData()
        {
          RandomOpacity = RandomHelper.Between(0.125, 0.5),
          Layer = RandomHelper.Random.Next(0, 4),
          RandomSpeed = RandomHelper.Between(0.25, 2.0),
          AccumulatedTime = (midLife ? TimeSpan.FromSeconds((double) RandomHelper.Between(0.0, 8.0)) : TimeSpan.Zero)
        };
        local_9.Material = new Material()
        {
          Diffuse = Vector3.Zero
        };
        local_9.Position = vector3;
      }
    }

    private void AlignMesh(Viewpoint viewpoint)
    {
      float num = this.CameraManager.Radius / SettingsManager.GetViewScale(this.GraphicsDevice);
      Vector3 interpolatedCenter = this.CameraManager.InterpolatedCenter;
      Vector3 vector1 = FezMath.ForwardVector(viewpoint);
      Vector3 vector2 = FezMath.RightVector(viewpoint);
      this.Meshes[viewpoint].Position = new Vector3(0.0f, interpolatedCenter.Y + num / 2f / this.CameraManager.AspectRatio, 0.0f) + interpolatedCenter * FezMath.Abs(vector2) + this.LevelManager.Size / 2f * FezMath.Abs(vector1) + num * vector1;
    }

    private void ScrollRays(Viewpoint viewpoint, TimeSpan elapsed)
    {
      this.ScrollRays(viewpoint, elapsed, 8f);
    }

    private void ScrollRays(Viewpoint viewpoint, TimeSpan elapsed, float lifetime)
    {
      float val2 = (double) this.TimeManager.DayFraction > 0.800000011920929 ? 1f : this.TimeManager.DuskContribution / 0.85f;
      try
      {
        for (int i = 0; i < this.Meshes[viewpoint].Groups.Count; ++i)
        {
          Group group = this.Meshes[viewpoint].Groups[i];
          GodRays.RayCustomData rayCustomData = (GodRays.RayCustomData) group.CustomData;
          rayCustomData.AccumulatedTime += elapsed;
          group.Material.Diffuse = new Vector3((float) Math.Sin(rayCustomData.AccumulatedTime.TotalSeconds / (double) lifetime * 3.14159274101257) * rayCustomData.RandomOpacity) * (1f - FezMath.Saturate(Math.Max(this.TimeManager.NightContribution * 2f, val2)));
          if ((double) this.TimeManager.DuskContribution != 0.0)
            group.Material.Diffuse *= new Vector3(1f, 1f - this.TimeManager.DuskContribution, 0.0f);
          else if ((double) this.TimeManager.DawnContribution != 0.0)
            group.Material.Diffuse *= new Vector3(1f, (float) (1.0 - (double) this.TimeManager.DawnContribution * 0.5), 0.0f);
          else
            group.Material.Diffuse *= new Vector3(1f, 1f, 0.0f);
          group.Position += (float) elapsed.TotalSeconds * Vector3.UnitX * 0.25f * (float) (0.25 + (double) rayCustomData.Layer / 3.0 * 1.25) * rayCustomData.RandomSpeed;
          if (rayCustomData.AccumulatedTime.TotalSeconds > (double) lifetime)
          {
            lock (this.Meshes[viewpoint])
              this.Meshes[viewpoint].RemoveGroupAt(i);
            --i;
          }
        }
      }
      catch (NullReferenceException ex)
      {
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading && !this.GameState.FarawaySettings.InTransition || ((double) this.TimeManager.NightContribution > 0.5 || this.GameState.InMap) || !FezMath.IsOrthographic(this.CameraManager.Viewpoint))
        return;
      if (this.CameraManager.ProjectionTransition)
      {
        foreach (Mesh mesh in this.Meshes.Values)
        {
          foreach (Group group in mesh.Groups)
            group.Material.Diffuse *= new Vector3(this.CameraManager.ViewTransitionStep / 2f);
        }
      }
      if (this.DotManager.Behaviour == DotHost.BehaviourType.SpiralAroundWithCamera || this.GameState.FarawaySettings.InTransition)
      {
        foreach (Mesh mesh in this.Meshes.Values)
          mesh.Draw();
      }
      else
      {
        this.Meshes[this.CameraManager.Viewpoint].Draw();
        if (this.CameraManager.ViewTransitionReached)
          return;
        this.Meshes[this.CameraManager.LastViewpoint].Draw();
      }
    }

    private void DrawLayered(Mesh m, BaseEffect e)
    {
      lock (m)
      {
        foreach (Group item_0 in m.Groups)
        {
          switch (((GodRays.RayCustomData) item_0.CustomData).Layer)
          {
            case 0:
              GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Greater, StencilMask.SkyLayer3);
              break;
            case 1:
              GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Greater, StencilMask.SkyLayer2);
              break;
            case 2:
              GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Greater, StencilMask.SkyLayer1);
              break;
            case 3:
              GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
              break;
          }
          item_0.Draw(e);
        }
      }
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
    }

    private class RayCustomData
    {
      public TimeSpan AccumulatedTime;
      public float RandomSpeed;
      public float RandomOpacity;
      public int Layer;
    }
  }
}
