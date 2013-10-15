// Type: Microsoft.Xna.Framework.Graphics.DepthStencilState
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Graphics.OpenGL;

namespace Microsoft.Xna.Framework.Graphics
{
  public class DepthStencilState : GraphicsResource
  {
    public static readonly DepthStencilState Default = new DepthStencilState()
    {
      DepthBufferEnable = true,
      DepthBufferWriteEnable = true
    };
    public static readonly DepthStencilState DepthRead = new DepthStencilState()
    {
      DepthBufferEnable = true,
      DepthBufferWriteEnable = false
    };
    public static readonly DepthStencilState None = new DepthStencilState()
    {
      DepthBufferEnable = false,
      DepthBufferWriteEnable = false
    };

    public bool DepthBufferEnable { get; set; }

    public bool DepthBufferWriteEnable { get; set; }

    public StencilOperation CounterClockwiseStencilDepthBufferFail { get; set; }

    public StencilOperation CounterClockwiseStencilFail { get; set; }

    public CompareFunction CounterClockwiseStencilFunction { get; set; }

    public StencilOperation CounterClockwiseStencilPass { get; set; }

    public CompareFunction DepthBufferFunction { get; set; }

    public int ReferenceStencil { get; set; }

    public StencilOperation StencilDepthBufferFail { get; set; }

    public bool StencilEnable { get; set; }

    public StencilOperation StencilFail { get; set; }

    public CompareFunction StencilFunction { get; set; }

    public int StencilMask { get; set; }

    public StencilOperation StencilPass { get; set; }

    public int StencilWriteMask { get; set; }

    public bool TwoSidedStencilMode { get; set; }

    static DepthStencilState()
    {
    }

    public DepthStencilState()
    {
      this.DepthBufferEnable = true;
      this.DepthBufferWriteEnable = true;
      this.DepthBufferFunction = CompareFunction.LessEqual;
      this.StencilEnable = false;
      this.StencilFunction = CompareFunction.Always;
      this.StencilPass = StencilOperation.Keep;
      this.StencilFail = StencilOperation.Keep;
      this.StencilDepthBufferFail = StencilOperation.Keep;
      this.TwoSidedStencilMode = false;
      this.CounterClockwiseStencilFunction = CompareFunction.Always;
      this.CounterClockwiseStencilFail = StencilOperation.Keep;
      this.CounterClockwiseStencilPass = StencilOperation.Keep;
      this.CounterClockwiseStencilDepthBufferFail = StencilOperation.Keep;
      this.StencilMask = int.MaxValue;
      this.StencilWriteMask = int.MaxValue;
      this.ReferenceStencil = 0;
    }

    internal void ApplyState(GraphicsDevice device)
    {
      if (!this.DepthBufferEnable)
      {
        GL.Disable(EnableCap.DepthTest);
      }
      else
      {
        GL.Enable(EnableCap.DepthTest);
        DepthFunction func;
        switch (this.DepthBufferFunction)
        {
          case CompareFunction.Never:
            func = DepthFunction.Never;
            break;
          case CompareFunction.Less:
            func = DepthFunction.Less;
            break;
          case CompareFunction.LessEqual:
            func = DepthFunction.Lequal;
            break;
          case CompareFunction.Equal:
            func = DepthFunction.Equal;
            break;
          case CompareFunction.GreaterEqual:
            func = DepthFunction.Gequal;
            break;
          case CompareFunction.Greater:
            func = DepthFunction.Greater;
            break;
          case CompareFunction.NotEqual:
            func = DepthFunction.Notequal;
            break;
          default:
            func = DepthFunction.Always;
            break;
        }
        GL.DepthFunc(func);
      }
      GL.DepthMask(this.DepthBufferWriteEnable);
      if (!this.StencilEnable)
      {
        GL.Disable(EnableCap.StencilTest);
      }
      else
      {
        GL.Enable(EnableCap.StencilTest);
        StencilFunction func;
        switch (this.StencilFunction)
        {
          case CompareFunction.Never:
            func = StencilFunction.Never;
            break;
          case CompareFunction.Less:
            func = StencilFunction.Less;
            break;
          case CompareFunction.LessEqual:
            func = StencilFunction.Lequal;
            break;
          case CompareFunction.Equal:
            func = StencilFunction.Equal;
            break;
          case CompareFunction.GreaterEqual:
            func = StencilFunction.Gequal;
            break;
          case CompareFunction.Greater:
            func = StencilFunction.Greater;
            break;
          case CompareFunction.NotEqual:
            func = StencilFunction.Notequal;
            break;
          default:
            func = StencilFunction.Always;
            break;
        }
        GL.StencilFunc(func, this.ReferenceStencil, this.StencilMask);
        GL.StencilOp(DepthStencilState.GetStencilOp(this.StencilFail), DepthStencilState.GetStencilOp(this.StencilDepthBufferFail), DepthStencilState.GetStencilOp(this.StencilPass));
      }
    }

    private static StencilOp GetStencilOp(StencilOperation operation)
    {
      switch (operation)
      {
        case StencilOperation.Keep:
          return StencilOp.Keep;
        case StencilOperation.Zero:
          return StencilOp.Zero;
        case StencilOperation.Replace:
          return StencilOp.Replace;
        case StencilOperation.Increment:
          return StencilOp.Incr;
        case StencilOperation.Decrement:
          return StencilOp.Decr;
        case StencilOperation.IncrementSaturation:
          return StencilOp.IncrWrap;
        case StencilOperation.DecrementSaturation:
          return StencilOp.DecrWrap;
        case StencilOperation.Invert:
          return StencilOp.Invert;
        default:
          return StencilOp.Keep;
      }
    }
  }
}
