// Type: Microsoft.Xna.Framework.Graphics.AlphaTestEffect
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
  public class AlphaTestEffect : Effect, IEffectMatrices, IEffectFog
  {
    private static readonly byte[] Bytecode = Effect.LoadEffectResource("Microsoft.Xna.Framework.Graphics.Effect.Resources.AlphaTestEffect.ogl.mgfxo");
    private Matrix world = Matrix.Identity;
    private Matrix view = Matrix.Identity;
    private Matrix projection = Matrix.Identity;
    private Vector3 diffuseColor = Vector3.One;
    private float alpha = 1f;
    private float fogEnd = 1f;
    private CompareFunction alphaFunction = CompareFunction.Greater;
    private EffectDirtyFlags dirtyFlags = EffectDirtyFlags.All;
    private EffectParameter textureParam;
    private EffectParameter diffuseColorParam;
    private EffectParameter alphaTestParam;
    private EffectParameter fogColorParam;
    private EffectParameter fogVectorParam;
    private EffectParameter worldViewProjParam;
    private int _shaderIndex;
    private bool fogEnabled;
    private bool vertexColorEnabled;
    private Matrix worldView;
    private float fogStart;
    private int referenceAlpha;
    private bool isEqNe;

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

    public CompareFunction AlphaFunction
    {
      get
      {
        return this.alphaFunction;
      }
      set
      {
        this.alphaFunction = value;
        this.dirtyFlags |= EffectDirtyFlags.AlphaTest;
      }
    }

    public int ReferenceAlpha
    {
      get
      {
        return this.referenceAlpha;
      }
      set
      {
        this.referenceAlpha = value;
        this.dirtyFlags |= EffectDirtyFlags.AlphaTest;
      }
    }

    static AlphaTestEffect()
    {
    }

    public AlphaTestEffect(GraphicsDevice device)
      : base(device, AlphaTestEffect.Bytecode)
    {
      this.CacheEffectParameters();
    }

    protected AlphaTestEffect(AlphaTestEffect cloneSource)
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
      this.alphaFunction = cloneSource.alphaFunction;
      this.referenceAlpha = cloneSource.referenceAlpha;
    }

    public override Effect Clone()
    {
      return (Effect) new AlphaTestEffect(this);
    }

    private void CacheEffectParameters()
    {
      this.textureParam = this.Parameters["Texture"];
      this.diffuseColorParam = this.Parameters["DiffuseColor"];
      this.alphaTestParam = this.Parameters["AlphaTest"];
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
      if ((this.dirtyFlags & EffectDirtyFlags.AlphaTest) != ~EffectDirtyFlags.All)
      {
        Vector4 vector4 = new Vector4();
        bool flag = false;
        float num = (float) this.referenceAlpha / (float) byte.MaxValue;
        switch (this.alphaFunction)
        {
          case CompareFunction.Never:
            vector4.Z = -1f;
            vector4.W = -1f;
            break;
          case CompareFunction.Less:
            vector4.X = num - 0.001960784f;
            vector4.Z = 1f;
            vector4.W = -1f;
            break;
          case CompareFunction.LessEqual:
            vector4.X = num + 0.001960784f;
            vector4.Z = 1f;
            vector4.W = -1f;
            break;
          case CompareFunction.Equal:
            vector4.X = num;
            vector4.Y = 0.001960784f;
            vector4.Z = 1f;
            vector4.W = -1f;
            flag = true;
            break;
          case CompareFunction.GreaterEqual:
            vector4.X = num - 0.001960784f;
            vector4.Z = -1f;
            vector4.W = 1f;
            break;
          case CompareFunction.Greater:
            vector4.X = num + 0.001960784f;
            vector4.Z = -1f;
            vector4.W = 1f;
            break;
          case CompareFunction.NotEqual:
            vector4.X = num;
            vector4.Y = 0.001960784f;
            vector4.Z = -1f;
            vector4.W = 1f;
            flag = true;
            break;
          default:
            vector4.Z = 1f;
            vector4.W = 1f;
            break;
        }
        this.alphaTestParam.SetValue(vector4);
        this.dirtyFlags &= ~EffectDirtyFlags.AlphaTest;
        if (this.isEqNe != flag)
        {
          this.isEqNe = flag;
          this.dirtyFlags |= EffectDirtyFlags.ShaderIndex;
        }
      }
      if ((this.dirtyFlags & EffectDirtyFlags.ShaderIndex) != ~EffectDirtyFlags.All)
      {
        int num = 0;
        if (!this.fogEnabled)
          ++num;
        if (this.vertexColorEnabled)
          num += 2;
        if (this.isEqNe)
          num += 4;
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
