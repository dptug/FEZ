// Type: FezEngine.Effects.BaseEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Effects.Structures;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FezEngine.Effects
{
  public abstract class BaseEffect : IDisposable
  {
    public static readonly object DeviceLock = new object();
    internal FogEffectStructure fog;
    protected SemanticMappedSingle aspectRatio;
    protected SemanticMappedSingle time;
    protected SemanticMappedVector3 centerPosition;
    protected SemanticMappedVector3 eye;
    protected SemanticMappedVector3 baseAmbient;
    protected SemanticMappedVector2 texelOffset;
    protected SemanticMappedVector3 diffuseLight;
    protected SemanticMappedVector3 eyeSign;
    protected SemanticMappedVector3 levelCenter;
    protected Matrix viewProjection;
    private Stopwatch stopWatch;
    internal readonly MatricesEffectStructure matrices;
    internal readonly MaterialEffectStructure material;
    protected Effect effect;
    protected EffectPass currentPass;
    protected EffectTechnique currentTechnique;
    protected bool SimpleGroupPrepare;
    protected bool SimpleMeshPrepare;
    public bool IsDisposed;
    private static Vector3 sharedEyeSign;
    private static Vector3 sharedLevelCenter;
    protected bool textureMatrixDirty;
    public bool IgnoreCache;

    public static Vector3 EyeSign
    {
      set
      {
        BaseEffect.sharedEyeSign = value;
      }
    }

    public static Vector3 LevelCenter
    {
      set
      {
        BaseEffect.sharedLevelCenter = value;
      }
    }

    public EffectPass CurrentPass
    {
      get
      {
        return this.currentPass;
      }
    }

    public Matrix? ForcedViewMatrix { get; set; }

    public Matrix? ForcedProjectionMatrix { get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { protected get; set; }

    [ServiceDependency]
    public IGraphicsDeviceService GraphicsDeviceService { protected get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraProvider { protected get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { protected get; set; }

    [ServiceDependency]
    public IFogManager FogProvider { protected get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { protected get; set; }

    static BaseEffect()
    {
    }

    protected BaseEffect(string effectName)
      : this(effectName, false)
    {
    }

    protected BaseEffect(string effectName, bool skipClone)
    {
      ServiceHelper.InjectServices((object) this);
      this.effect = this.CMProvider.Global.Load<Effect>("Effects/" + effectName);
      if (!skipClone)
      {
        this.TryCloneEffect(this.effect);
        while (this.currentPass == null)
        {
          Logger.Log("Effect", Common.LogSeverity.Warning, "Could not validate effect " + effectName);
          this.TryCloneEffect(this.effect);
        }
      }
      else
      {
        this.currentTechnique = this.effect.Techniques[0];
        this.currentPass = this.currentTechnique.Passes[0];
      }
      this.matrices = new MatricesEffectStructure(this.effect.Parameters);
      this.material = new MaterialEffectStructure(this.effect.Parameters);
      this.Initialize();
    }

    private void TryCloneEffect(Effect sharedEffect)
    {
      this.effect = sharedEffect.Clone();
      using (IEnumerator<EffectTechnique> enumerator = this.effect.Techniques.GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        EffectTechnique current = enumerator.Current;
        this.currentTechnique = current;
        this.currentPass = current.Passes[0];
      }
    }

    public virtual BaseEffect Clone()
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      if (this.IsDisposed)
        return;
      this.effect = (Effect) null;
      this.currentPass = (EffectPass) null;
      this.currentTechnique = (EffectTechnique) null;
      this.IsDisposed = true;
      this.EngineState.PauseStateChanged -= new Action(this.CheckPause);
      this.CameraProvider.ViewChanged -= new Action(this.RefreshViewProjection);
      this.CameraProvider.ProjectionChanged -= new Action(this.RefreshViewProjection);
      this.CameraProvider.ViewChanged -= new Action(this.RefreshCenterPosition);
      this.CameraProvider.ProjectionChanged -= new Action(this.RefreshAspectRatio);
      this.FogProvider.FogSettingsChanged -= new Action(this.RefreshFog);
      this.LevelManager.LightingChanged -= new Action(this.RefreshLighting);
      this.GraphicsDeviceService.DeviceReset -= new EventHandler<EventArgs>(this.RefreshTexelSize);
      if (this.stopWatch != null)
        this.stopWatch.Stop();
      this.stopWatch = (Stopwatch) null;
    }

    private void Initialize()
    {
      this.fog = new FogEffectStructure(this.effect.Parameters);
      this.centerPosition = new SemanticMappedVector3(this.effect.Parameters, "CenterPosition");
      this.aspectRatio = new SemanticMappedSingle(this.effect.Parameters, "AspectRatio");
      this.texelOffset = new SemanticMappedVector2(this.effect.Parameters, "TexelOffset");
      this.time = new SemanticMappedSingle(this.effect.Parameters, "Time");
      this.baseAmbient = new SemanticMappedVector3(this.effect.Parameters, "BaseAmbient");
      this.eye = new SemanticMappedVector3(this.effect.Parameters, "Eye");
      this.diffuseLight = new SemanticMappedVector3(this.effect.Parameters, "DiffuseLight");
      this.eyeSign = new SemanticMappedVector3(this.effect.Parameters, "EyeSign");
      this.levelCenter = new SemanticMappedVector3(this.effect.Parameters, "LevelCenter");
      this.stopWatch = Stopwatch.StartNew();
      this.EngineState.PauseStateChanged += new Action(this.CheckPause);
      this.CameraProvider.ViewChanged += new Action(this.RefreshViewProjection);
      this.CameraProvider.ProjectionChanged += new Action(this.RefreshViewProjection);
      this.RefreshViewProjection();
      this.CameraProvider.ViewChanged += new Action(this.RefreshCenterPosition);
      this.RefreshCenterPosition();
      this.CameraProvider.ProjectionChanged += new Action(this.RefreshAspectRatio);
      this.RefreshAspectRatio();
      this.FogProvider.FogSettingsChanged += new Action(this.RefreshFog);
      this.RefreshFog();
      this.LevelManager.LightingChanged += new Action(this.RefreshLighting);
      this.RefreshLighting();
      this.GraphicsDeviceService.DeviceReset += new EventHandler<EventArgs>(this.RefreshTexelSize);
      this.RefreshTexelSize();
      this.eyeSign.Set(BaseEffect.sharedEyeSign);
      this.levelCenter.Set(BaseEffect.sharedLevelCenter);
    }

    private void RefreshTexelSize(object sender, EventArgs ea)
    {
      this.RefreshTexelSize();
    }

    private void RefreshTexelSize()
    {
      this.texelOffset.Set(new Vector2(-0.5f / (float) this.GraphicsDeviceService.GraphicsDevice.Viewport.Width, 0.5f / (float) this.GraphicsDeviceService.GraphicsDevice.Viewport.Height));
    }

    private void RefreshLighting()
    {
      this.baseAmbient.Set(this.LevelManager.ActualAmbient.ToVector3());
      this.diffuseLight.Set(this.LevelManager.ActualDiffuse.ToVector3());
    }

    private void CheckPause()
    {
      if (this.stopWatch.IsRunning && this.EngineState.Paused)
      {
        this.stopWatch.Stop();
      }
      else
      {
        if (this.stopWatch.IsRunning || this.EngineState.Paused)
          return;
        this.stopWatch.Start();
      }
    }

    private void RefreshFog()
    {
      this.fog.FogType = this.FogProvider.Type;
      this.fog.FogColor = this.FogProvider.Color;
      if (this.EngineState.InEditor)
        this.fog.FogDensity = this.FogProvider.Density;
      else
        this.fog.FogDensity = this.FogProvider.Density * 1.25f;
    }

    private void RefreshViewProjection()
    {
      this.viewProjection = this.CameraProvider.View * this.CameraProvider.Projection;
      this.matrices.ViewProjection = this.viewProjection;
    }

    private void RefreshCenterPosition()
    {
      this.centerPosition.Set(this.CameraProvider.InterpolatedCenter);
      this.eye.Set(this.CameraProvider.InverseView.Forward);
    }

    private void RefreshAspectRatio()
    {
      this.aspectRatio.Set(this.CameraProvider.AspectRatio);
    }

    public virtual void Prepare(Mesh mesh)
    {
      this.eyeSign.Set(BaseEffect.sharedEyeSign);
      this.levelCenter.Set(BaseEffect.sharedLevelCenter);
      this.time.Set((float) this.stopWatch.Elapsed.TotalSeconds);
      if (mesh.TextureMatrix.Dirty || this.IgnoreCache)
      {
        this.matrices.TextureMatrix = (Matrix) mesh.TextureMatrix;
        mesh.TextureMatrix.Clean();
      }
      if (this.SimpleMeshPrepare)
        this.matrices.WorldViewProjection = this.viewProjection;
      else if (this.SimpleGroupPrepare)
      {
        Matrix matrix = this.viewProjection;
        if (this.ForcedViewMatrix.HasValue && !this.ForcedProjectionMatrix.HasValue)
          matrix = this.ForcedViewMatrix.Value * this.CameraProvider.Projection;
        else if (!this.ForcedViewMatrix.HasValue && this.ForcedProjectionMatrix.HasValue)
          matrix = this.CameraProvider.View * this.ForcedProjectionMatrix.Value;
        else if (this.ForcedViewMatrix.HasValue && this.ForcedProjectionMatrix.HasValue)
          matrix = this.ForcedViewMatrix.Value * this.ForcedProjectionMatrix.Value;
        this.matrices.WorldViewProjection = mesh.WorldMatrix * matrix;
      }
      else
        this.material.Opacity = mesh.Material.Opacity;
    }

    public virtual void Prepare(Group group)
    {
      if (this.SimpleGroupPrepare)
        return;
      Matrix matrix = this.viewProjection;
      if (this.ForcedViewMatrix.HasValue && !this.ForcedProjectionMatrix.HasValue)
        matrix = this.ForcedViewMatrix.Value * this.CameraProvider.Projection;
      else if (!this.ForcedViewMatrix.HasValue && this.ForcedProjectionMatrix.HasValue)
        matrix = this.CameraProvider.View * this.ForcedProjectionMatrix.Value;
      else if (this.ForcedViewMatrix.HasValue && this.ForcedProjectionMatrix.HasValue)
        matrix = this.ForcedViewMatrix.Value * this.ForcedProjectionMatrix.Value;
      this.matrices.WorldViewProjection = group.WorldMatrix.Value * matrix;
      this.material.Opacity = group.Material != null ? group.Material.Opacity : group.Mesh.Material.Opacity;
      if (group.TextureMatrix.Value.HasValue)
      {
        this.matrices.TextureMatrix = group.TextureMatrix.Value.Value;
        this.textureMatrixDirty = true;
      }
      else
      {
        if (!this.textureMatrixDirty)
          return;
        this.matrices.TextureMatrix = Matrix.Identity;
        this.textureMatrixDirty = false;
      }
    }

    public void Apply()
    {
      this.currentPass.Apply();
    }
  }
}
