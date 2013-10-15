// Type: Microsoft.Xna.Framework.Graphics.OcclusionQuery
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
      if (!this.IsDisposed && (this.GraphicsDevice != null && !this.GraphicsDevice.IsDisposed))
        this.GraphicsDevice.AddDisposeAction((Action) (() => GL.DeleteQueries(1, ref this.glQueryId)));
      base.Dispose(disposing);
    }
  }
}
