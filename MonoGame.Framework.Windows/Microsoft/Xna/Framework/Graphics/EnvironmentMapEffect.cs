// Type: Microsoft.Xna.Framework.Graphics.EnvironmentMapEffect
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class EnvironmentMapEffect : Effect, IEffectMatrices, IEffectLights, IEffectFog
  {
    private static readonly byte[] Bytecode = Effect.LoadEffectResource("Microsoft.Xna.Framework.Graphics.Effect.Resources.EnvironmentMapEffect.ogl.mgfxo");
    private int _shaderIndex = -1;
    private Matrix world = Matrix.Identity;
    private Matrix view = Matrix.Identity;
    private Matrix projection = Matrix.Identity;
    private Vector3 diffuseColor = Vector3.One;
    private Vector3 emissiveColor = Vector3.Zero;
    private Vector3 ambientLightColor = Vector3.Zero;
    private float alpha = 1f;
    private float fogStart = 0.0f;
    private float fogEnd = 1f;
    private EffectDirtyFlags dirtyFlags = EffectDirtyFlags.All;
    private EffectParameter textureParam;
    private EffectParameter environmentMapParam;
    private EffectParameter environmentMapAmountParam;
    private EffectParameter environmentMapSpecularParam;
    private EffectParameter fresnelFactorParam;
    private EffectParameter diffuseColorParam;
    private EffectParameter emissiveColorParam;
    private EffectParameter eyePositionParam;
    private EffectParameter fogColorParam;
    private EffectParameter fogVectorParam;
    private EffectParameter worldParam;
    private EffectParameter worldInverseTransposeParam;
    private EffectParameter worldViewProjParam;
    private bool oneLight;
    private bool fogEnabled;
    private bool fresnelEnabled;
    private bool specularEnabled;
    private Matrix worldView;
    private DirectionalLight light0;
    private DirectionalLight light1;
    private DirectionalLight light2;

    public Matrix World
    {
      get
      {
        return this.world;
      }
      set
      {
        this.world = value;
        this.dirtyFlags |= EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.World | EffectDirtyFlags.Fog;
      }
    }

    public Matrix View
    {
      get
      {
        return this.view;
      }
      set
      {
        this.view = value;
        this.dirtyFlags |= EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.EyePosition | EffectDirtyFlags.Fog;
      }
    }

    public Matrix Projection
    {
      get
      {
        return this.projection;
      }
      set
      {
        this.projection = value;
        this.dirtyFlags |= EffectDirtyFlags.WorldViewProj;
      }
    }

    public Vector3 DiffuseColor
    {
      get
      {
        return this.diffuseColor;
      }
      set
      {
        this.diffuseColor = value;
        this.dirtyFlags |= EffectDirtyFlags.MaterialColor;
      }
    }

    public Vector3 EmissiveColor
    {
      get
      {
        return this.emissiveColor;
      }
      set
      {
        this.emissiveColor = value;
        this.dirtyFlags |= EffectDirtyFlags.MaterialColor;
      }
    }

    public float Alpha
    {
      get
      {
        return this.alpha;
      }
      set
      {
        this.alpha = value;
        this.dirtyFlags |= EffectDirtyFlags.MaterialColor;
      }
    }

    public Vector3 AmbientLightColor
    {
      get
      {
        return this.ambientLightColor;
      }
      set
      {
        this.ambientLightColor = value;
        this.dirtyFlags |= EffectDirtyFlags.MaterialColor;
      }
    }

    public DirectionalLight DirectionalLight0
    {
      get
      {
        return this.light0;
      }
    }

    public DirectionalLight DirectionalLight1
    {
      get
      {
        return this.light1;
      }
    }

    public DirectionalLight DirectionalLight2
    {
      get
      {
        return this.light2;
      }
    }

    public bool FogEnabled
    {
      get
      {
        return this.fogEnabled;
      }
      set
      {
        if (this.fogEnabled == value)
          return;
        this.fogEnabled = value;
        this.dirtyFlags |= EffectDirtyFlags.FogEnable | EffectDirtyFlags.ShaderIndex;
      }
    }

    public float FogStart
    {
      get
      {
        return this.fogStart;
      }
      set
      {
        this.fogStart = value;
        this.dirtyFlags |= EffectDirtyFlags.Fog;
      }
    }

    public float FogEnd
    {
      get
      {
        return this.fogEnd;
      }
      set
      {
        this.fogEnd = value;
        this.dirtyFlags |= EffectDirtyFlags.Fog;
      }
    }

    public Vector3 FogColor
    {
      get
      {
        return this.fogColorParam.GetValueVector3();
      }
      set
      {
        this.fogColorParam.SetValue(value);
      }
    }

    public Texture2D Texture
    {
      get
      {
        return this.textureParam.GetValueTexture2D();
      }
      set
      {
        this.textureParam.SetValue((Texture) value);
      }
    }

    public TextureCube EnvironmentMap
    {
      get
      {
        return this.environmentMapParam.GetValueTextureCube();
      }
      set
      {
        this.environmentMapParam.SetValue((Texture) value);
      }
    }

    public float EnvironmentMapAmount
    {
      get
      {
        return this.environmentMapAmountParam.GetValueSingle();
      }
      set
      {
        this.environmentMapAmountParam.SetValue(value);
      }
    }

    public Vector3 EnvironmentMapSpecular
    {
      get
      {
        return this.environmentMapSpecularParam.GetValueVector3();
      }
      set
      {
        this.environmentMapSpecularParam.SetValue(value);
        bool flag = value != Vector3.Zero;
        if (this.specularEnabled == flag)
          return;
        this.specularEnabled = flag;
        this.dirtyFlags |= EffectDirtyFlags.ShaderIndex;
      }
    }

    public float FresnelFactor
    {
      get
      {
        return this.fresnelFactorParam.GetValueSingle();
      }
      set
      {
        this.fresnelFactorParam.SetValue(value);
        bool flag = (double) value != 0.0;
        if (this.fresnelEnabled == flag)
          return;
        this.fresnelEnabled = flag;
        this.dirtyFlags |= EffectDirtyFlags.ShaderIndex;
      }
    }

    bool IEffectLights.LightingEnabled
    {
      get
      {
        return true;
      }
      set
      {
        if (!value)
          throw new NotSupportedException("EnvironmentMapEffect does not support setting LightingEnabled to false.");
      }
    }

    static EnvironmentMapEffect()
    {
    }

    public EnvironmentMapEffect(GraphicsDevice device)
      : base(device, EnvironmentMapEffect.Bytecode)
    {
      this.CacheEffectParameters((EnvironmentMapEffect) null);
      this.DirectionalLight0.Enabled = true;
      this.EnvironmentMapAmount = 1f;
      this.EnvironmentMapSpecular = Vector3.Zero;
      this.FresnelFactor = 1f;
    }

    protected EnvironmentMapEffect(EnvironmentMapEffect cloneSource)
      : base((Effect) cloneSource)
    {
      this.CacheEffectParameters(cloneSource);
      this.fogEnabled = cloneSource.fogEnabled;
      this.fresnelEnabled = cloneSource.fresnelEnabled;
      this.specularEnabled = cloneSource.specularEnabled;
      this.world = cloneSource.world;
      this.view = cloneSource.view;
      this.projection = cloneSource.projection;
      this.diffuseColor = cloneSource.diffuseColor;
      this.emissiveColor = cloneSource.emissiveColor;
      this.ambientLightColor = cloneSource.ambientLightColor;
      this.alpha = cloneSource.alpha;
      this.fogStart = cloneSource.fogStart;
      this.fogEnd = cloneSource.fogEnd;
    }

    public override Effect Clone()
    {
      return (Effect) new EnvironmentMapEffect(this);
    }

    public void EnableDefaultLighting()
    {
      this.AmbientLightColor = EffectHelpers.EnableDefaultLighting(this.light0, this.light1, this.light2);
    }

    private void CacheEffectParameters(EnvironmentMapEffect cloneSource)
    {
      this.textureParam = this.Parameters["Texture"];
      this.environmentMapParam = this.Parameters["EnvironmentMap"];
      this.environmentMapAmountParam = this.Parameters["EnvironmentMapAmount"];
      this.environmentMapSpecularParam = this.Parameters["EnvironmentMapSpecular"];
      this.fresnelFactorParam = this.Parameters["FresnelFactor"];
      this.diffuseColorParam = this.Parameters["DiffuseColor"];
      this.emissiveColorParam = this.Parameters["EmissiveColor"];
      this.eyePositionParam = this.Parameters["EyePosition"];
      this.fogColorParam = this.Parameters["FogColor"];
      this.fogVectorParam = this.Parameters["FogVector"];
      this.worldParam = this.Parameters["World"];
      this.worldInverseTransposeParam = this.Parameters["WorldInverseTranspose"];
      this.worldViewProjParam = this.Parameters["WorldViewProj"];
      this.light0 = new DirectionalLight(this.Parameters["DirLight0Direction"], this.Parameters["DirLight0DiffuseColor"], (EffectParameter) null, cloneSource != null ? cloneSource.light0 : (DirectionalLight) null);
      this.light1 = new DirectionalLight(this.Parameters["DirLight1Direction"], this.Parameters["DirLight1DiffuseColor"], (EffectParameter) null, cloneSource != null ? cloneSource.light1 : (DirectionalLight) null);
      this.light2 = new DirectionalLight(this.Parameters["DirLight2Direction"], this.Parameters["DirLight2DiffuseColor"], (EffectParameter) null, cloneSource != null ? cloneSource.light2 : (DirectionalLight) null);
    }

    protected internal override bool OnApply()
    {
      this.dirtyFlags = EffectHelpers.SetWorldViewProjAndFog(this.dirtyFlags, ref this.world, ref this.view, ref this.projection, ref this.worldView, this.fogEnabled, this.fogStart, this.fogEnd, this.worldViewProjParam, this.fogVectorParam);
      this.dirtyFlags = EffectHelpers.SetLightingMatrices(this.dirtyFlags, ref this.world, ref this.view, this.worldParam, this.worldInverseTransposeParam, this.eyePositionParam);
      if ((this.dirtyFlags & EffectDirtyFlags.MaterialColor) != ~EffectDirtyFlags.All)
      {
        EffectHelpers.SetMaterialColor(true, this.alpha, ref this.diffuseColor, ref this.emissiveColor, ref this.ambientLightColor, this.diffuseColorParam, this.emissiveColorParam);
        this.dirtyFlags &= ~EffectDirtyFlags.MaterialColor;
      }
      bool flag = !this.light1.Enabled && !this.light2.Enabled;
      if (this.oneLight != flag)
      {
        this.oneLight = flag;
        this.dirtyFlags |= EffectDirtyFlags.ShaderIndex;
      }
      if ((this.dirtyFlags & EffectDirtyFlags.ShaderIndex) != ~EffectDirtyFlags.All)
      {
        int num = 0;
        if (!this.fogEnabled)
          ++num;
        if (this.fresnelEnabled)
          num += 2;
        if (this.specularEnabled)
          num += 4;
        if (this.oneLight)
          num += 8;
        this.dirtyFlags &= ~EffectDirtyFlags.ShaderIndex;
        if (this._shaderIndex != num)
        {
          this._shaderIndex = num;
          this.CurrentTechnique = this.Techniques[this._shaderIndex];
          return true;
        }
      }
      return false;
    }
  }
}
