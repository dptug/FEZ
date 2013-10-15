// Type: Microsoft.Xna.Framework.Graphics.OcclusionQuery
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK.Graphics.OpenGL;
using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public class OcclusionQuery : GraphicsResource
  {
    private uint glQueryId;

    public bool IsComplete
    {
      get
      {
        int[] @params = new int[1];
        GL.GetQueryObject(this.glQueryId, GetQueryObjectParam.QueryResultAvailable, @params);
        return @params[0] != 0;
      }
    }

    public int PixelCount
    {
      get
      {
        int[] @params = new int[1];
        GL.GetQueryObject(this.glQueryId, GetQueryObjectParam.QueryResultAvailable, @params);
        return @params[0];
      }
    }

    public OcclusionQuery(GraphicsDevice graphicsDevice)
    {
      this.GraphicsDevice = graphicsDevice;
      GL.GenQueries(1, out this.glQueryId);
    }

    public void Begin()
    {
      GL.BeginQuery(QueryTarget.SamplesPassed, this.glQueryId);
    }

    public void End()
    {
      GL.EndQuery(QueryTarget.SamplesPassed);
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed)
        GraphicsDevice.AddDisposeAction((Action) (() => GL.DeleteQueries(1, ref this.glQueryId)));
      base.Dispose(disposing);
    }
  }
}
