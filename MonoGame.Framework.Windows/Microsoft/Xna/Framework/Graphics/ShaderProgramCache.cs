// Type: Microsoft.Xna.Framework.Graphics.ShaderProgramCache
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  internal class ShaderProgramCache : IDisposable
  {
    private readonly Dictionary<int, ShaderProgramInfo> _programCache = new Dictionary<int, ShaderProgramInfo>();
    private bool disposed;

    ~ShaderProgramCache()
    {
      this.Dispose(true);
    }

    public void Clear()
    {
      foreach (KeyValuePair<int, ShaderProgramInfo> keyValuePair in this._programCache)
      {
        if (GL.IsProgram(keyValuePair.Value.program))
          GL.DeleteProgram(keyValuePair.Value.program);
      }
      this._programCache.Clear();
    }

    public ShaderProgramInfo GetProgramInfo(Shader vertexShader, Shader pixelShader)
    {
      int key = vertexShader.HashKey | pixelShader.HashKey;
      if (!this._programCache.ContainsKey(key))
        this.Link(vertexShader, pixelShader);
      return this._programCache[key];
    }

    private void Link(Shader vertexShader, Shader pixelShader)
    {
      int program = GL.CreateProgram();
      GL.AttachShader(program, vertexShader.GetShaderHandle());
      GL.AttachShader(program, pixelShader.GetShaderHandle());
      GL.LinkProgram(program);
      GL.UseProgram(program);
      vertexShader.GetVertexAttributeLocations(program);
      pixelShader.ApplySamplerTextureUnits(program);
      int @params = 0;
      GL.GetProgram(program, ProgramParameter.LinkStatus, out @params);
      if (@params == 0)
      {
        Console.WriteLine(GL.GetProgramInfoLog(program));
        throw new InvalidOperationException("Unable to link effect program");
      }
      else
      {
        ShaderProgramInfo shaderProgramInfo;
        shaderProgramInfo.program = program;
        shaderProgramInfo.posFixupLoc = GL.GetUniformLocation(program, "posFixup");
        this._programCache.Add(vertexShader.HashKey | pixelShader.HashKey, shaderProgramInfo);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.Clear();
      this.disposed = true;
    }
  }
}
