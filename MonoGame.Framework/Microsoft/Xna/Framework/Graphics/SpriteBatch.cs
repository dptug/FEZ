// Type: Microsoft.Xna.Framework.Graphics.SpriteBatch
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
  public class SpriteBatch : GraphicsResource
  {
    private Rectangle _tempRect = new Rectangle(0, 0, 0, 0);
    private Vector2 _texCoordTL = new Vector2(0.0f, 0.0f);
    private Vector2 _texCoordBR = new Vector2(0.0f, 0.0f);
    private readonly SpriteBatcher _batcher;
    private SpriteSortMode _sortMode;
    private BlendState _blendState;
    private SamplerState _samplerState;
    private DepthStencilState _depthStencilState;
    private RasterizerState _rasterizerState;
    private Effect _effect;
    private bool _beginCalled;
    private Effect _spriteEffect;
    private Matrix _matrix;

    public SpriteBatch(GraphicsDevice graphicsDevice)
    {
      if (graphicsDevice == null)
        throw new ArgumentException("graphicsDevice");
      this.GraphicsDevice = graphicsDevice;
      this._spriteEffect = new Effect(graphicsDevice, SpriteEffect.Bytecode);
      this._batcher = new SpriteBatcher(graphicsDevice);
      this._beginCalled = false;
    }

    public void Begin()
    {
      this.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect) null, Matrix.Identity);
    }

    public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
    {
      if (this._beginCalled)
        throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");
      this._sortMode = sortMode;
      this._blendState = blendState ?? BlendState.AlphaBlend;
      this._samplerState = samplerState ?? SamplerState.LinearClamp;
      this._depthStencilState = depthStencilState ?? DepthStencilState.None;
      this._rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
      this._effect = effect;
      this._matrix = transformMatrix;
      if (sortMode == SpriteSortMode.Immediate)
        this.Setup();
      this._beginCalled = true;
    }

    public void Begin(SpriteSortMode sortMode, BlendState blendState)
    {
      this.Begin(sortMode, blendState, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect) null, Matrix.Identity);
    }

    public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
    {
      this.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, (Effect) null, Matrix.Identity);
    }

    public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
    {
      this.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, Matrix.Identity);
    }

    public void End()
    {
      this._beginCalled = false;
      if (this._sortMode != SpriteSortMode.Immediate)
        this.Setup();
      this._batcher.DrawBatch(this._sortMode);
    }

    private void Setup()
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      graphicsDevice.BlendState = this._blendState;
      graphicsDevice.DepthStencilState = this._depthStencilState;
      graphicsDevice.RasterizerState = this._rasterizerState;
      graphicsDevice.SamplerStates[0] = this._samplerState;
      Viewport viewport = graphicsDevice.Viewport;
      this._spriteEffect.Parameters["MatrixTransform"].SetValue(this._matrix * Matrix.CreateTranslation(-0.5f, -0.5f, 0.0f) * Matrix.CreateOrthographicOffCenter(0.0f, (float) viewport.Width, (float) viewport.Height, 0.0f, 0.0f, 1f));
      this._spriteEffect.CurrentTechnique.Passes[0].Apply();
      if (this._effect == null)
        return;
      this._effect.CurrentTechnique.Passes[0].Apply();
    }

    private void CheckValid(Texture2D texture)
    {
      if (texture == null)
        throw new ArgumentNullException("texture");
      if (!this._beginCalled)
        throw new InvalidOperationException("Draw was called, but Begin has not yet been called. Begin must be called successfully before you can call Draw.");
    }

    private void CheckValid(SpriteFont spriteFont, string text)
    {
      if (spriteFont == null)
        throw new ArgumentNullException("spriteFont");
      if (text == null)
        throw new ArgumentNullException("text");
      if (!this._beginCalled)
        throw new InvalidOperationException("DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
    }

    private void CheckValid(SpriteFont spriteFont, StringBuilder text)
    {
      if (spriteFont == null)
        throw new ArgumentNullException("spriteFont");
      if (text == null)
        throw new ArgumentNullException("text");
      if (!this._beginCalled)
        throw new InvalidOperationException("DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
    }

    public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
    {
      this.CheckValid(texture);
      float z = (float) texture.Width * scale.X;
      float w = (float) texture.Height * scale.Y;
      if (sourceRectangle.HasValue)
      {
        z = (float) sourceRectangle.Value.Width * scale.X;
        w = (float) sourceRectangle.Value.Height * scale.Y;
      }
      this.DrawInternal(texture, new Vector4(position.X, position.Y, z, w), sourceRectangle, color, rotation, origin * scale, effect, depth);
    }

    public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float depth)
    {
      this.CheckValid(texture);
      float z = (float) texture.Width * scale;
      float w = (float) texture.Height * scale;
      if (sourceRectangle.HasValue)
      {
        z = (float) sourceRectangle.Value.Width * scale;
        w = (float) sourceRectangle.Value.Height * scale;
      }
      this.DrawInternal(texture, new Vector4(position.X, position.Y, z, w), sourceRectangle, color, rotation, origin * scale, effect, depth);
    }

    public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effect, float depth)
    {
      this.CheckValid(texture);
      this.DrawInternal(texture, new Vector4((float) destinationRectangle.X, (float) destinationRectangle.Y, (float) destinationRectangle.Width, (float) destinationRectangle.Height), sourceRectangle, color, rotation, new Vector2(origin.X * ((float) destinationRectangle.Width / (!sourceRectangle.HasValue || sourceRectangle.Value.Width == 0 ? (float) texture.Width : (float) sourceRectangle.Value.Width)), (float) ((double) origin.Y * (double) destinationRectangle.Height / (!sourceRectangle.HasValue || sourceRectangle.Value.Height == 0 ? (double) texture.Height : (double) sourceRectangle.Value.Height))), effect, depth);
    }

    internal void DrawInternal(Texture2D texture, Vector4 destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effect, float depth)
    {
      SpriteBatchItem batchItem = this._batcher.CreateBatchItem();
      batchItem.Depth = depth;
      batchItem.Texture = texture;
      if (sourceRectangle.HasValue)
      {
        this._tempRect = sourceRectangle.Value;
      }
      else
      {
        this._tempRect.X = 0;
        this._tempRect.Y = 0;
        this._tempRect.Width = texture.Width;
        this._tempRect.Height = texture.Height;
      }
      this._texCoordTL.X = (float) this._tempRect.X / (float) texture.Width;
      this._texCoordTL.Y = (float) this._tempRect.Y / (float) texture.Height;
      this._texCoordBR.X = (float) (this._tempRect.X + this._tempRect.Width) / (float) texture.Width;
      this._texCoordBR.Y = (float) (this._tempRect.Y + this._tempRect.Height) / (float) texture.Height;
      if ((effect & SpriteEffects.FlipVertically) != SpriteEffects.None)
      {
        float num = this._texCoordBR.Y;
        this._texCoordBR.Y = this._texCoordTL.Y;
        this._texCoordTL.Y = num;
      }
      if ((effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
      {
        float num = this._texCoordBR.X;
        this._texCoordBR.X = this._texCoordTL.X;
        this._texCoordTL.X = num;
      }
      batchItem.Set(destinationRectangle.X, destinationRectangle.Y, -origin.X, -origin.Y, destinationRectangle.Z, destinationRectangle.W, (float) Math.Sin((double) rotation), (float) Math.Cos((double) rotation), color, this._texCoordTL, this._texCoordBR);
      if (this._sortMode != SpriteSortMode.Immediate)
        return;
      this._batcher.DrawBatch(this._sortMode);
    }

    public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
    {
      this.Draw(texture, position, sourceRectangle, color, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
    }

    public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
    {
      this.Draw(texture, destinationRectangle, sourceRectangle, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
    }

    public void Draw(Texture2D texture, Vector2 position, Color color)
    {
      this.Draw(texture, position, new Rectangle?(), color);
    }

    public void Draw(Texture2D texture, Rectangle rectangle, Color color)
    {
      this.Draw(texture, rectangle, new Rectangle?(), color);
    }

    public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
    {
      this.CheckValid(spriteFont, text);
      SpriteFont.CharacterSource text1 = new SpriteFont.CharacterSource(text);
      spriteFont.DrawInto(this, ref text1, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }

    public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
    {
      this.CheckValid(spriteFont, text);
      Vector2 scale1 = new Vector2(scale, scale);
      SpriteFont.CharacterSource text1 = new SpriteFont.CharacterSource(text);
      spriteFont.DrawInto(this, ref text1, position, color, rotation, origin, scale1, effects, depth);
    }

    public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
    {
      this.CheckValid(spriteFont, text);
      SpriteFont.CharacterSource text1 = new SpriteFont.CharacterSource(text);
      spriteFont.DrawInto(this, ref text1, position, color, rotation, origin, scale, effect, depth);
    }

    public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
    {
      this.CheckValid(spriteFont, text);
      SpriteFont.CharacterSource text1 = new SpriteFont.CharacterSource(text);
      spriteFont.DrawInto(this, ref text1, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }

    public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
    {
      this.CheckValid(spriteFont, text);
      Vector2 scale1 = new Vector2(scale, scale);
      SpriteFont.CharacterSource text1 = new SpriteFont.CharacterSource(text);
      spriteFont.DrawInto(this, ref text1, position, color, rotation, origin, scale1, effects, depth);
    }

    public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
    {
      this.CheckValid(spriteFont, text);
      SpriteFont.CharacterSource text1 = new SpriteFont.CharacterSource(text);
      spriteFont.DrawInto(this, ref text1, position, color, rotation, origin, scale, effect, depth);
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed && disposing && this._spriteEffect != null)
      {
        this._spriteEffect.Dispose();
        this._spriteEffect = (Effect) null;
      }
      base.Dispose(disposing);
    }
  }
}
