// Type: Microsoft.Xna.Framework.Graphics.DepthStencilState
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
        if (this.TwoSidedStencilMode)
        {
          Version20 face1 = (Version20) 1028;
          Version20 face2 = (Version20) 1029;
          StencilFace face3 = StencilFace.Front;
          StencilFace face4 = StencilFace.Back;
          GL.StencilFuncSeparate(face1, DepthStencilState.GetStencilFunc(this.StencilFunction), this.ReferenceStencil, this.StencilMask);
          GL.StencilFuncSeparate(face2, DepthStencilState.GetStencilFunc(this.CounterClockwiseStencilFunction), this.ReferenceStencil, this.StencilMask);
          GL.StencilOpSeparate(face3, DepthStencilState.GetStencilOp(this.StencilFail), DepthStencilState.GetStencilOp(this.StencilDepthBufferFail), DepthStencilState.GetStencilOp(this.StencilPass));
          GL.StencilOpSeparate(face4, DepthStencilState.GetStencilOp(this.CounterClockwiseStencilFail), DepthStencilState.GetStencilOp(this.CounterClockwiseStencilDepthBufferFail), DepthStencilState.GetStencilOp(this.CounterClockwiseStencilPass));
        }
        else
        {
          GL.StencilFunc(DepthStencilState.GetStencilFunc(this.StencilFunction), this.ReferenceStencil, this.StencilMask);
          GL.StencilOp(DepthStencilState.GetStencilOp(this.StencilFail), DepthStencilState.GetStencilOp(this.StencilDepthBufferFail), DepthStencilState.GetStencilOp(this.StencilPass));
        }
      }
    }

    private static StencilFunction GetStencilFunc(CompareFunction function)
    {
      switch (function)
      {
        case CompareFunction.Always:
          return StencilFunction.Always;
        case CompareFunction.Never:
          return StencilFunction.Never;
        case CompareFunction.Less:
          return StencilFunction.Less;
        case CompareFunction.LessEqual:
          return StencilFunction.Lequal;
        case CompareFunction.Equal:
          return StencilFunction.Equal;
        case CompareFunction.GreaterEqual:
          return StencilFunction.Gequal;
        case CompareFunction.Greater:
          return StencilFunction.Greater;
        case CompareFunction.NotEqual:
          return StencilFunction.Notequal;
        default:
          return StencilFunction.Always;
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
          return StencilOp.IncrWrap;
        case StencilOperation.Decrement:
          return StencilOp.DecrWrap;
        case StencilOperation.IncrementSaturation:
          return StencilOp.Incr;
        case StencilOperation.DecrementSaturation:
          return StencilOp.Decr;
        case StencilOperation.Invert:
          return StencilOp.Invert;
        default:
          return StencilOp.Keep;
      }
    }
  }
}
