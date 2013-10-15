// Type: Microsoft.Xna.Framework.Graphics.Shader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using MonoGame.Utilities;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
  internal class Shader : GraphicsResource
  {
    private int _shaderHandle = -1;
    private readonly string _glslCode;
    private Shader.Attribute[] _attributes;

    internal int HashKey { get; private set; }

    public SamplerInfo[] Samplers { get; private set; }

    public int[] CBuffers { get; private set; }

    public ShaderStage Stage { get; private set; }

    internal Shader(GraphicsDevice device, BinaryReader reader)
    {
      this.GraphicsDevice = device;
      this.Stage = reader.ReadBoolean() ? ShaderStage.Vertex : ShaderStage.Pixel;
      int count = (int) reader.ReadUInt16();
      byte[] bytes = reader.ReadBytes(count);
      int length1 = (int) reader.ReadByte();
      this.Samplers = new SamplerInfo[length1];
      for (int index = 0; index < length1; ++index)
      {
        this.Samplers[index].type = (SamplerType) reader.ReadByte();
        this.Samplers[index].index = (int) reader.ReadByte();
        this.Samplers[index].name = reader.ReadString();
        this.Samplers[index].parameter = (int) reader.ReadByte();
      }
      int length2 = (int) reader.ReadByte();
      this.CBuffers = new int[length2];
      for (int index = 0; index < length2; ++index)
        this.CBuffers[index] = (int) reader.ReadByte();
      this._glslCode = Encoding.ASCII.GetString(bytes);
      this.HashKey = Hash.ComputeHash(bytes);
      int length3 = (int) reader.ReadByte();
      this._attributes = new Shader.Attribute[length3];
      for (int index = 0; index < length3; ++index)
      {
        this._attributes[index].name = reader.ReadString();
        this._attributes[index].usage = (VertexElementUsage) reader.ReadByte();
        this._attributes[index].index = (int) reader.ReadByte();
        this._attributes[index].format = reader.ReadInt16();
      }
    }

    internal int GetShaderHandle()
    {
      if (this._shaderHandle != -1)
        return this._shaderHandle;
      this._shaderHandle = GL.CreateShader(this.Stage == ShaderStage.Vertex ? ShaderType.VertexShader : ShaderType.FragmentShader);
      GL.ShaderSource(this._shaderHandle, this._glslCode);
      GL.CompileShader(this._shaderHandle);
      int @params = 0;
      GL.GetShader(this._shaderHandle, ShaderParameter.CompileStatus, out @params);
      if (@params != 0)
        return this._shaderHandle;
      Console.WriteLine(GL.GetShaderInfoLog(this._shaderHandle));
      if (GL.IsShader(this._shaderHandle))
        GL.DeleteShader(this._shaderHandle);
      this._shaderHandle = -1;
      throw new InvalidOperationException("Shader Compilation Failed");
    }

    internal void GetVertexAttributeLocations(int program)
    {
      for (int index = 0; index < this._attributes.Length; ++index)
        this._attributes[index].location = GL.GetAttribLocation(program, this._attributes[index].name);
    }

    internal int GetAttribLocation(VertexElementUsage usage, int index)
    {
      for (int index1 = 0; index1 < this._attributes.Length; ++index1)
      {
        if (this._attributes[index1].usage == usage)
          return this._attributes[index1].location + index;
      }
      return -1;
    }

    internal void ApplySamplerTextureUnits(int program)
    {
      foreach (SamplerInfo samplerInfo in this.Samplers)
      {
        int uniformLocation = GL.GetUniformLocation(program, samplerInfo.name);
        if (uniformLocation != -1)
          GL.Uniform1(uniformLocation, samplerInfo.index);
      }
    }

    protected internal new virtual void GraphicsDeviceResetting()
    {
      if (this._shaderHandle == -1)
        return;
      if (GL.IsShader(this._shaderHandle))
        GL.DeleteShader(this._shaderHandle);
      this._shaderHandle = -1;
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed && (this.GraphicsDevice != null && !this.GraphicsDevice.IsDisposed))
        this.GraphicsDevice.AddDisposeAction((Action) (() =>
        {
          if (this._shaderHandle == -1)
            return;
          if (GL.IsShader(this._shaderHandle))
            GL.DeleteShader(this._shaderHandle);
          this._shaderHandle = -1;
        }));
      base.Dispose(disposing);
    }

    private struct Attribute
    {
      public VertexElementUsage usage;
      public int index;
      public string name;
      public short format;
      public int location;
    }
  }
}
