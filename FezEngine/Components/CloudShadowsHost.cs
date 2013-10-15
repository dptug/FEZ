// Type: FezEngine.Components.CloudShadowsHost
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezEngine.Components
{
  public class CloudShadowsHost : DrawableGameComponent
  {
    private readonly Dictionary<Group, Axis> axisPerGroup = new Dictionary<Group, Axis>();
    private CloudShadowEffect shadowEffect;
    private Mesh shadowMesh;
    private SkyHost Host;
    private float SineAccumulator;
    private float SineSpeed;

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { private get; set; }

    public CloudShadowsHost(Game game, SkyHost host)
      : base(game)
    {
      this.DrawOrder = 100;
      this.Host = host;
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.shadowEffect = new CloudShadowEffect();
      this.shadowMesh = new Mesh()
      {
        DepthWrites = false,
        AlwaysOnTop = true,
        SamplerState = SamplerState.LinearWrap
      };
      foreach (FaceOrientation faceOrientation in Util.GetValues<FaceOrientation>())
      {
        if (FezMath.IsSide(faceOrientation))
          this.axisPerGroup.Add(this.shadowMesh.AddFace(Vector3.One, Vector3.Zero, faceOrientation, true), FezMath.AsAxis(faceOrientation) == Axis.X ? Axis.Z : Axis.X);
      }
      this.shadowMesh.Effect = (BaseEffect) this.shadowEffect;
      this.LevelManager.SkyChanged += new Action(this.InitializeShadows);
      this.InitializeShadows();
      this.LightingPostProcess.DrawOnTopLights += new Action(this.DrawLights);
    }

    private void InitializeShadows()
    {
      if (this.LevelManager.Name == null || this.LevelManager.Sky == null || this.LevelManager.Sky.Shadows == null)
      {
        this.shadowMesh.Texture.Set((Texture) null);
        this.shadowMesh.Enabled = false;
      }
      else
      {
        this.shadowMesh.Enabled = true;
        this.shadowMesh.Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Skies/" + this.LevelManager.Sky.Name + "/" + this.LevelManager.Sky.Shadows));
        this.shadowMesh.Scale = this.LevelManager.Size + new Vector3(65f, 65f, 65f);
        this.shadowMesh.Position = this.LevelManager.Size / 2f;
        int num1 = 0;
        foreach (Group index in this.axisPerGroup.Keys)
        {
          index.Material = new Material();
          Axis axis = this.axisPerGroup[index];
          float m11 = FezMath.Dot(this.shadowMesh.Scale, FezMath.GetMask(axis)) / 32f;
          float num2 = this.shadowMesh.Scale.Y / FezMath.Dot(this.shadowMesh.Scale, FezMath.GetMask(axis));
          index.TextureMatrix = (Dirtyable<Matrix?>) new Matrix?(new Matrix(m11, 0.0f, 0.0f, 0.0f, 0.0f, m11 * num2, 0.0f, 0.0f, (float) num1 / 2f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f));
          ++num1;
        }
        this.SineAccumulator = 0.0f;
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.shadowMesh.Enabled || this.EngineState.Paused || (this.EngineState.InMap || this.EngineState.Loading))
        return;
      if (this.LevelManager.Sky != null && !this.LevelManager.Sky.FoliageShadows)
      {
        float m31 = (float) (-gameTime.ElapsedGameTime.TotalSeconds * 0.00999999977648258 * (double) this.TimeManager.TimeFactor / 360.0) * this.LevelManager.Sky.WindSpeed;
        foreach (Group index in this.axisPerGroup.Keys)
        {
          if (this.axisPerGroup[index] != FezMath.VisibleAxis(this.CameraManager.Viewpoint))
            index.TextureMatrix.Set(new Matrix?(index.TextureMatrix.Value.Value + new Matrix(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, m31, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f)));
        }
      }
      else
      {
        this.SineSpeed = MathHelper.Lerp(this.SineSpeed, RandomHelper.Between(0.0, gameTime.ElapsedGameTime.TotalSeconds), 0.1f);
        this.SineAccumulator += this.SineSpeed;
      }
      foreach (Group index in this.axisPerGroup.Keys)
      {
        Vector3 mask = FezMath.GetMask(this.axisPerGroup[index]);
        index.Material.Opacity = (1f - Math.Abs(FezMath.Dot(this.CameraManager.View.Forward, mask))) * this.LevelManager.Sky.ShadowOpacity;
        if (!this.LevelManager.Sky.FoliageShadows)
          index.Material.Opacity *= (float) this.LevelManager.ActualDiffuse.G / (float) byte.MaxValue;
        if (this.CameraManager.ProjectionTransition)
          index.Material.Opacity *= FezMath.IsOrthographic(this.CameraManager.Viewpoint) ? this.CameraManager.ViewTransitionStep : 1f - this.CameraManager.ViewTransitionStep;
        else if (this.CameraManager.Viewpoint == Viewpoint.Perspective)
          index.Material.Opacity = 0.0f;
      }
    }

    private void DrawLights()
    {
      if (!this.shadowMesh.Enabled || this.EngineState.Loading)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.PrepareStencilRead(graphicsDevice, CompareFunction.LessEqual, StencilMask.Level);
      this.EngineState.SkyRender = true;
      Vector3 viewOffset = this.CameraManager.ViewOffset;
      this.CameraManager.ViewOffset -= viewOffset;
      if (this.LevelManager.Sky != null && this.LevelManager.Sky.FoliageShadows)
      {
        this.shadowEffect.Pass = CloudShadowPasses.Canopy;
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Minimum);
        float num1 = (float) Math.Sin((double) this.SineAccumulator);
        foreach (Group group in this.axisPerGroup.Keys)
        {
          float m11 = group.TextureMatrix.Value.Value.M11;
          float m22 = group.TextureMatrix.Value.Value.M11;
          group.TextureMatrix.Set(new Matrix?(new Matrix(m11, 0.0f, 0.0f, 0.0f, 0.0f, m22, 0.0f, 0.0f, num1 / 100f, num1 / 100f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f)));
        }
        this.shadowMesh.Draw();
        float num2 = (float) Math.Cos((double) this.SineAccumulator);
        foreach (Group group in this.axisPerGroup.Keys)
        {
          float m11 = group.TextureMatrix.Value.Value.M11;
          float m22 = group.TextureMatrix.Value.Value.M11;
          group.TextureMatrix.Set(new Matrix?(new Matrix(m11, 0.0f, 0.0f, 0.0f, 0.0f, m22, 0.0f, 0.0f, (float) (-(double) num2 / 100.0 + 0.100000001490116), (float) ((double) num2 / 100.0 + 0.100000001490116), 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f)));
        }
        this.shadowMesh.Draw();
      }
      else
      {
        this.shadowEffect.Pass = CloudShadowPasses.Standard;
        float depthBias = GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).DepthBias;
        float slopeScaleDepthBias = GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).SlopeScaleDepthBias;
        GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).DepthBias = 0.0f;
        GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).SlopeScaleDepthBias = 0.0f;
        Color color = new Color(this.LevelManager.ActualAmbient.ToVector3() / 2f);
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Subtract);
        this.TargetRenderer.DrawFullscreen(color);
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Multiply);
        this.shadowMesh.Draw();
        GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Additive);
        this.TargetRenderer.DrawFullscreen(color);
        GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).DepthBias = depthBias;
        GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).SlopeScaleDepthBias = slopeScaleDepthBias;
      }
      GraphicsDeviceExtensions.SetBlendingMode(graphicsDevice, BlendingMode.Alphablending);
      this.CameraManager.ViewOffset += viewOffset;
      this.EngineState.SkyRender = false;
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.EngineState.Loading || this.LevelManager.Sky == null || this.Host.BgLayers == null)
        return;
      this.GraphicsDevice.SamplerStates[0] = this.LevelManager.Sky.VerticalTiling ? SamplerState.PointWrap : SamplerStates.PointUWrapVClamp;
      foreach (Group group1 in this.Host.BgLayers.Groups)
      {
        Group group2 = group1;
        bool? alwaysOnTop = group1.AlwaysOnTop;
        int num = alwaysOnTop.HasValue ? (alwaysOnTop.GetValueOrDefault() ? 1 : 0) : 0;
        group2.Enabled = num != 0;
      }
      this.Host.BgLayers.Draw();
    }
  }
}
