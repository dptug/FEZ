// Type: Microsoft.Xna.Framework.Graphics.EffectPass
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Graphics
{
  public class EffectPass
  {
    private Effect _effect;
    private Shader _pixelShader;
    private Shader _vertexShader;
    private BlendState _blendState;
    private DepthStencilState _depthStencilState;
    private RasterizerState _rasterizerState;

    public string Name { get; private set; }

    public EffectAnnotationCollection Annotations { get; private set; }

    internal EffectPass(Effect effect, string name, Shader vertexShader, Shader pixelShader, BlendState blendState, DepthStencilState depthStencilState, RasterizerState rasterizerState, EffectAnnotationCollection annotations)
    {
      this._effect = effect;
      this.Name = name;
      this._vertexShader = vertexShader;
      this._pixelShader = pixelShader;
      this._blendState = blendState;
      this._depthStencilState = depthStencilState;
      this._rasterizerState = rasterizerState;
      this.Annotations = annotations;
      this.Initialize();
    }

    internal EffectPass(Effect effect, EffectPass cloneSource)
    {
      this._effect = effect;
      this.Name = cloneSource.Name;
      this._blendState = cloneSource._blendState;
      this._depthStencilState = cloneSource._depthStencilState;
      this._rasterizerState = cloneSource._rasterizerState;
      this.Annotations = cloneSource.Annotations;
      this._vertexShader = cloneSource._vertexShader;
      this._pixelShader = cloneSource._pixelShader;
    }

    public void Initialize()
    {
    }

    public void Apply()
    {
      if (this._effect.OnApply())
      {
        this._effect.CurrentTechnique.Passes[0].Apply();
      }
      else
      {
        GraphicsDevice graphicsDevice = this._effect.GraphicsDevice;
        if (this._vertexShader != null)
        {
          graphicsDevice.VertexShader = this._vertexShader;
          for (int slot = 0; slot < this._vertexShader.CBuffers.Length; ++slot)
          {
            ConstantBuffer buffer = this._effect.ConstantBuffers[this._vertexShader.CBuffers[slot]];
            buffer.Update(this._effect.Parameters);
            graphicsDevice.SetConstantBuffer(ShaderStage.Vertex, slot, buffer);
          }
        }
        if (this._pixelShader != null)
        {
          graphicsDevice.PixelShader = this._pixelShader;
          foreach (SamplerInfo samplerInfo in this._pixelShader.Samplers)
          {
            Texture texture = this._effect.Parameters[samplerInfo.parameter].Data as Texture;
            if (texture != null)
              graphicsDevice.Textures[samplerInfo.index] = texture;
          }
          for (int slot = 0; slot < this._pixelShader.CBuffers.Length; ++slot)
          {
            ConstantBuffer buffer = this._effect.ConstantBuffers[this._pixelShader.CBuffers[slot]];
            buffer.Update(this._effect.Parameters);
            graphicsDevice.SetConstantBuffer(ShaderStage.Pixel, slot, buffer);
          }
        }
        if (this._rasterizerState != null)
          graphicsDevice.RasterizerState = this._rasterizerState;
        if (this._blendState != null)
          graphicsDevice.BlendState = this._blendState;
        if (this._depthStencilState == null)
          return;
        graphicsDevice.DepthStencilState = this._depthStencilState;
      }
    }
  }
}
