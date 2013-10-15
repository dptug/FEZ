// Type: Microsoft.Xna.Framework.Graphics.DualTextureEffect
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
  public class DualTextureEffect : Effect, IEffectMatrices, IEffectFog
  {
    private static readonly byte[] Bytecode = Effect.LoadEffectResource("Microsoft.Xna.Framework.Graphics.Effect.Resources.DualTextureEffect.ogl.mgfxo");
    private int _shaderIndex = -1;
    private Matrix world = Matrix.Identity;
    private Matrix view = Matrix.Identity;
    private Matrix projection = Matrix.Identity;
    private Vector3 diffuseColor = Vector3.One;
    private float alpha = 1f;
    private float fogStart = 0.0f;
    private float fogEnd = 1f;
    private EffectDirtyFlags dirtyFlags = EffectDirtyFlags.All;
    private EffectParameter textureParam;
    private EffectParameter texture2Param;
    private EffectParameter diffuseColorParam;
    private EffectParameter fogColorParam;
    private EffectParameter fogVectorParam;
    private EffectParameter worldViewProjParam;
    private bool fogEnabled;
    private bool vertexColorEnabled;
    private Matrix worldView;

    public Matrix World
    {
      get
      {
        return this.world;
      }
      set
      {
        this.world = value;
        this.dirtyFlags |= EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.Fog;
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
        this.dirtyFlags |= EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.Fog;
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

    public Texture2D Texture2
    {
      get
      {
        return this.texture2Param.GetValueTexture2D();
      }
      set
      {
        this.texture2Param.SetValue((Texture) value);
      }
    }

    public bool VertexColorEnabled
    {
      get
      {
        return this.vertexColorEnabled;
      }
      set
      {
        if (this.vertexColorEnabled == value)
          return;
        this.vertexColorEnabled = value;
        this.dirtyFlags |= EffectDirtyFlags.ShaderIndex;
      }
    }

    static DualTextureEffect()
    {
    }

    public DualTextureEffect(GraphicsDevice device)
      : base(device, DualTextureEffect.Bytecode)
    {
      this.CacheEffectParameters();
    }

    protected DualTextureEffect(DualTextureEffect cloneSource)
      : base((Effect) cloneSource)
    {
      this.CacheEffectParameters();
      this.fogEnabled = cloneSource.fogEnabled;
      this.vertexColorEnabled = cloneSource.vertexColorEnabled;
      this.world = cloneSource.world;
      this.view = cloneSource.view;
      this.projection = cloneSource.projection;
      this.diffuseColor = cloneSource.diffuseColor;
      this.alpha = cloneSource.alpha;
      this.fogStart = cloneSource.fogStart;
      this.fogEnd = cloneSource.fogEnd;
    }

    public override Effect Clone()
    {
      return (Effect) new DualTextureEffect(this);
    }

    private void CacheEffectParameters()
    {
      this.textureParam = this.Parameters["Texture"];
      this.texture2Param = this.Parameters["Texture2"];
      this.diffuseColorParam = this.Parameters["DiffuseColor"];
      this.fogColorParam = this.Parameters["FogColor"];
      this.fogVectorParam = this.Parameters["FogVector"];
      this.worldViewProjParam = this.Parameters["WorldViewProj"];
    }

    protected internal override bool OnApply()
    {
      this.dirtyFlags = EffectHelpers.SetWorldViewProjAndFog(this.dirtyFlags, ref this.world, ref this.view, ref this.projection, ref this.worldView, this.fogEnabled, this.fogStart, this.fogEnd, this.worldViewProjParam, this.fogVectorParam);
      if ((this.dirtyFlags & EffectDirtyFlags.MaterialColor) != ~EffectDirtyFlags.All)
      {
        this.diffuseColorParam.SetValue(new Vector4(this.diffuseColor * this.alpha, this.alpha));
        this.dirtyFlags &= ~EffectDirtyFlags.MaterialColor;
      }
      if ((this.dirtyFlags & EffectDirtyFlags.ShaderIndex) != ~EffectDirtyFlags.All)
      {
        int num = 0;
        if (!this.fogEnabled)
          ++num;
        if (this.vertexColorEnabled)
          num += 2;
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
