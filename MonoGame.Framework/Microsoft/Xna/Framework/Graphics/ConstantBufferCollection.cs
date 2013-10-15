// Type: Microsoft.Xna.Framework.Graphics.ConstantBufferCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Graphics
{
  internal sealed class ConstantBufferCollection
  {
    private readonly ConstantBuffer[] _buffers;
    private ShaderStage _stage;
    private int _valid;

    public ConstantBuffer this[int index]
    {
      get
      {
        return this._buffers[index];
      }
      set
      {
        if (this._buffers[index] == value)
          return;
        if (value != null)
        {
          this._buffers[index] = value;
          this._valid |= 1 << index;
        }
        else
        {
          this._buffers[index] = (ConstantBuffer) null;
          this._valid &= ~(1 << index);
        }
      }
    }

    internal ConstantBufferCollection(ShaderStage stage, int maxBuffers)
    {
      this._stage = stage;
      this._buffers = new ConstantBuffer[maxBuffers];
      this._valid = 0;
    }

    internal void Clear()
    {
      for (int index = 0; index < this._buffers.Length; ++index)
        this._buffers[index] = (ConstantBuffer) null;
      this._valid = 0;
    }

    internal void SetConstantBuffers(GraphicsDevice device, int shaderProgram)
    {
      if (this._valid == 0)
        return;
      int num = this._valid;
      for (int index = 0; index < this._buffers.Length; ++index)
      {
        ConstantBuffer constantBuffer = this._buffers[index];
        if (constantBuffer != null)
          constantBuffer.Apply(device, shaderProgram);
        num &= ~(1 << index);
        if (num == 0)
          break;
      }
    }
  }
}
