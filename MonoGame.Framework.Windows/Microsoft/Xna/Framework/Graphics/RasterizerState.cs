// Type: Microsoft.Xna.Framework.Graphics.RasterizerState
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Graphics.OpenGL;

namespace Microsoft.Xna.Framework.Graphics
{
  public class RasterizerState : GraphicsResource
  {
    public static readonly RasterizerState CullClockwise = new RasterizerState()
    {
      CullMode = CullMode.CullClockwiseFace
    };
    public static readonly RasterizerState CullCounterClockwise = new RasterizerState()
    {
      CullMode = CullMode.CullCounterClockwiseFace
    };
    public static readonly RasterizerState CullNone = new RasterizerState()
    {
      CullMode = CullMode.None
    };

    public CullMode CullMode { get; set; }

    public float DepthBias { get; set; }

    public FillMode FillMode { get; set; }

    public bool MultiSampleAntiAlias { get; set; }

    public bool ScissorTestEnable { get; set; }

    public float SlopeScaleDepthBias { get; set; }

    static RasterizerState()
    {
    }

    public RasterizerState()
    {
      this.CullMode = CullMode.CullCounterClockwiseFace;
      this.FillMode = FillMode.Solid;
      this.DepthBias = 0.0f;
      this.MultiSampleAntiAlias = true;
      this.ScissorTestEnable = false;
      this.SlopeScaleDepthBias = 0.0f;
    }

    internal void ApplyState(GraphicsDevice device)
    {
      bool flag = device.GetRenderTargets().Length > 0;
      if (this.CullMode == CullMode.None)
      {
        GL.Disable(EnableCap.CullFace);
      }
      else
      {
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Back);
        if (this.CullMode == CullMode.CullClockwiseFace)
        {
          if (flag)
            GL.FrontFace(FrontFaceDirection.Cw);
          else
            GL.FrontFace(FrontFaceDirection.Ccw);
        }
        else if (flag)
          GL.FrontFace(FrontFaceDirection.Ccw);
        else
          GL.FrontFace(FrontFaceDirection.Cw);
      }
      if (this.FillMode == FillMode.Solid)
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
      else
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
      if (this.ScissorTestEnable)
        GL.Enable(EnableCap.ScissorTest);
      else
        GL.Disable(EnableCap.ScissorTest);
      GL.Enable(EnableCap.PolygonOffsetFill);
      GL.PolygonOffset(this.SlopeScaleDepthBias, this.DepthBias * 1E+07f);
    }
  }
}
