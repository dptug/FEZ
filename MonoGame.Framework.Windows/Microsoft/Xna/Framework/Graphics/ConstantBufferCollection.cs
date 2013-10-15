// Type: Microsoft.Xna.Framework.Graphics.ConstantBufferCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
