// Type: Microsoft.Xna.Framework.Graphics.GraphicsDevice
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
  public class GraphicsDevice : IDisposable
  {
    private static readonly RenderTargetBinding[] EmptyRenderTargetBinding = new RenderTargetBinding[0];
    private static readonly Color DiscardColor = new Color(68, 34, 136, (int) byte.MaxValue);
    private static readonly float[] _posFixup = new float[4];
    internal static readonly List<int> _enabledVertexAttributes = new List<int>();
    private BlendState _blendState = BlendState.Opaque;
    private DepthStencilState _depthStencilState = DepthStencilState.Default;
    private RasterizerState _rasterizerState = RasterizerState.CullCounterClockwise;
    private List<Action> disposeActions = new List<Action>();
    private object disposeActionsLock = new object();
    private readonly ConstantBufferCollection _vertexConstantBuffers = new ConstantBufferCollection(ShaderStage.Vertex, 16);
    private readonly ConstantBufferCollection _pixelConstantBuffers = new ConstantBufferCollection(ShaderStage.Pixel, 16);
    private readonly ShaderProgramCache _programCache = new ShaderProgramCache();
    private int _shaderProgram = -1;
    private readonly List<string> _extensions = new List<string>();
    private const FramebufferTarget GLFramebuffer = FramebufferTarget.Framebuffer;
    private const RenderbufferTarget GLRenderbuffer = RenderbufferTarget.Renderbuffer;
    private const FramebufferAttachment GLDepthAttachment = FramebufferAttachment.DepthAttachment;
    private const FramebufferAttachment GLStencilAttachment = FramebufferAttachment.StencilAttachment;
    private const FramebufferAttachment GLColorAttachment0 = FramebufferAttachment.ColorAttachment0;
    private const GetPName GLFramebufferBinding = GetPName.DrawFramebufferBinding;
    private const RenderbufferStorage GLDepthComponent16 = RenderbufferStorage.DepthComponent16;
    private const RenderbufferStorage GLDepthComponent24 = RenderbufferStorage.DepthComponent24;
    private const RenderbufferStorage GLDepth24Stencil8 = RenderbufferStorage.Depth24Stencil8;
    private const FramebufferErrorCode GLFramebufferComplete = FramebufferErrorCode.FramebufferComplete;
    private Viewport _viewport;
    private bool _isDisposed;
    private bool _blendStateDirty;
    private bool _depthStencilStateDirty;
    private bool _rasterizerStateDirty;
    private Rectangle _scissorRectangle;
    private bool _scissorRectangleDirty;
    private VertexBuffer _vertexBuffer;
    private bool _vertexBufferDirty;
    private IndexBuffer _indexBuffer;
    private bool _indexBufferDirty;
    private RenderTargetBinding[] _currentRenderTargetBindings;
    private Shader _vertexShader;
    private bool _vertexShaderDirty;
    private Shader _pixelShader;
    private bool _pixelShaderDirty;
    internal int glFramebuffer;
    internal int MaxVertexAttributes;
    internal int MaxTextureSlots;

    public TextureCollection Textures { get; private set; }

    public SamplerStateCollection SamplerStates { get; private set; }

    public bool IsDisposed
    {
      get
      {
        return this._isDisposed;
      }
    }

    public bool IsContentLost
    {
      get
      {
        return this.IsDisposed;
      }
    }

    internal bool IsRenderTargetBound
    {
      get
      {
        return this._currentRenderTargetBindings != null && this._currentRenderTargetBindings.Length > 0;
      }
    }

    public RasterizerState RasterizerState
    {
      get
      {
        return this._rasterizerState;
      }
      set
      {
        if (this._rasterizerState == value)
          return;
        this._rasterizerState = value;
        this._rasterizerStateDirty = true;
      }
    }

    public BlendState BlendState
    {
      get
      {
        return this._blendState;
      }
      set
      {
        if (this._blendState == value)
          return;
        this._blendState = value;
        this._blendStateDirty = true;
      }
    }

    public DepthStencilState DepthStencilState
    {
      get
      {
        return this._depthStencilState;
      }
      set
      {
        if (this._depthStencilState == value)
          return;
        this._depthStencilState = value;
        this._depthStencilStateDirty = true;
      }
    }

    public DisplayMode DisplayMode
    {
      get
      {
        return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
      }
    }

    public GraphicsDeviceStatus GraphicsDeviceStatus
    {
      get
      {
        return GraphicsDeviceStatus.Normal;
      }
    }

    public PresentationParameters PresentationParameters { get; private set; }

    public Viewport Viewport
    {
      get
      {
        return this._viewport;
      }
      set
      {
        this._viewport = value;
        if (this.IsRenderTargetBound)
          GL.Viewport(value.X, value.Y, value.Width, value.Height);
        else
          GL.Viewport(value.X, this.PresentationParameters.BackBufferHeight - value.Y - value.Height, value.Width, value.Height);
        GL.DepthRange((double) value.MinDepth, (double) value.MaxDepth);
      }
    }

    public GraphicsProfile GraphicsProfile { get; set; }

    public Rectangle ScissorRectangle
    {
      get
      {
        return this._scissorRectangle;
      }
      set
      {
        if (this._scissorRectangle == value)
          return;
        this._scissorRectangle = value;
        this._scissorRectangleDirty = true;
      }
    }

    public IndexBuffer Indices
    {
      set
      {
        this.SetIndexBuffer(value);
      }
    }

    internal Shader VertexShader
    {
      get
      {
        return this._vertexShader;
      }
      set
      {
        if (this._vertexShader == value)
          return;
        this._vertexShader = value;
        this._vertexShaderDirty = true;
      }
    }

    internal Shader PixelShader
    {
      get
      {
        return this._pixelShader;
      }
      set
      {
        if (this._pixelShader == value)
          return;
        this._pixelShader = value;
        this._pixelShaderDirty = true;
      }
    }

    public bool ResourcesLost { get; set; }

    public event EventHandler<EventArgs> DeviceLost;

    public event EventHandler<EventArgs> DeviceReset;

    public event EventHandler<EventArgs> DeviceResetting;

    static GraphicsDevice()
    {
    }

    public GraphicsDevice()
    {
      this._viewport = new Viewport(0, 0, this.DisplayMode.Width, this.DisplayMode.Height);
      this._viewport.MaxDepth = 1f;
      this.MaxTextureSlots = 16;
      GL.GetInteger(GetPName.MaxTextureImageUnits, out this.MaxTextureSlots);
      GL.GetInteger(GetPName.MaxVertexAttribs, out this.MaxVertexAttributes);
      this.Textures = new TextureCollection(this.MaxTextureSlots);
      this.SamplerStates = new SamplerStateCollection(this.MaxTextureSlots);
      this.PresentationParameters = new PresentationParameters();
      this.PresentationParameters.DepthStencilFormat = DepthFormat.Depth24Stencil8;
    }

    ~GraphicsDevice()
    {
      this.Dispose(false);
    }

    internal void SetVertexAttributeArray(bool[] attrs)
    {
      for (int index = 0; index < attrs.Length; ++index)
      {
        if (attrs[index] && !GraphicsDevice._enabledVertexAttributes.Contains(index))
        {
          GraphicsDevice._enabledVertexAttributes.Add(index);
          GL.EnableVertexAttribArray(index);
        }
        else if (!attrs[index] && GraphicsDevice._enabledVertexAttributes.Contains(index))
        {
          GraphicsDevice._enabledVertexAttributes.Remove(index);
          GL.DisableVertexAttribArray(index);
        }
      }
    }

    internal void Initialize()
    {
      string @string = GL.GetString(StringName.Extensions);
      if (!string.IsNullOrEmpty(@string))
      {
        this._extensions.AddRange((IEnumerable<string>) @string.Split(new char[1]
        {
          ' '
        }));
        foreach (string str in this._extensions)
          ;
      }
      this._viewport = new Viewport(0, 0, this.PresentationParameters.BackBufferWidth, this.PresentationParameters.BackBufferHeight);
      this._blendStateDirty = this._depthStencilStateDirty = this._rasterizerStateDirty = true;
      this.BlendState = BlendState.Opaque;
      this.DepthStencilState = DepthStencilState.Default;
      this.RasterizerState = RasterizerState.CullCounterClockwise;
      this.Textures.Clear();
      this.SamplerStates.Clear();
      this._vertexConstantBuffers.Clear();
      this._pixelConstantBuffers.Clear();
      GraphicsDevice._enabledVertexAttributes.Clear();
      this._indexBufferDirty = true;
      this._vertexBufferDirty = true;
      this._vertexShaderDirty = true;
      this._pixelShaderDirty = true;
      this._scissorRectangleDirty = true;
      this.ScissorRectangle = this._viewport.Bounds;
      this.ApplyRenderTargets((RenderTargetBinding[]) null);
      this._programCache.Clear();
      this._shaderProgram = -1;
    }

    public void Clear(Color color)
    {
      this.Clear((ClearOptions) (1 | 2 | 4), color.ToVector4(), this._viewport.MaxDepth, 0);
    }

    public void Clear(ClearOptions options, Color color, float depth, int stencil)
    {
      this.Clear(options, color.ToVector4(), depth, stencil);
    }

    public void Clear(ClearOptions options, Vector4 color, float depth, int stencil)
    {
      Rectangle scissorRectangle = this.ScissorRectangle;
      DepthStencilState depthStencilState = this.DepthStencilState;
      BlendState blendState = this.BlendState;
      this.ScissorRectangle = this._viewport.Bounds;
      this.DepthStencilState = DepthStencilState.Default;
      this.BlendState = BlendState.Opaque;
      this.ApplyState(false);
      ClearBufferMask mask = (ClearBufferMask) 0;
      if (options.HasFlag((Enum) ClearOptions.Target))
      {
        GL.ClearColor(color.X, color.Y, color.Z, color.W);
        mask |= ClearBufferMask.ColorBufferBit;
      }
      if (options.HasFlag((Enum) ClearOptions.Stencil))
      {
        GL.ClearStencil(stencil);
        mask |= ClearBufferMask.StencilBufferBit;
      }
      if (options.HasFlag((Enum) ClearOptions.DepthBuffer))
      {
        GL.ClearDepth((double) depth);
        mask |= ClearBufferMask.DepthBufferBit;
      }
      GL.Clear(mask);
      this.ScissorRectangle = scissorRectangle;
      this.DepthStencilState = depthStencilState;
      this.BlendState = blendState;
    }

    public void Clear(ClearOptions options, Color color, float depth, int stencil, Rectangle[] regions)
    {
      throw new NotImplementedException();
    }

    public void Clear(ClearOptions options, Vector4 color, float depth, int stencil, Rectangle[] regions)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._isDisposed)
        return;
      if (disposing)
      {
        GraphicsResource.DisposeAll();
        this._programCache.Dispose();
      }
      this._isDisposed = true;
    }

    internal void AddDisposeAction(Action disposeAction)
    {
      if (disposeAction == null)
        throw new ArgumentNullException("disposeAction");
      if (Threading.IsOnUIThread())
      {
        disposeAction();
      }
      else
      {
        lock (this.disposeActionsLock)
          this.disposeActions.Add(disposeAction);
      }
    }

    public void Present()
    {
      GL.Flush();
      lock (this.disposeActionsLock)
      {
        if (this.disposeActions.Count <= 0)
          return;
        foreach (Action item_0 in this.disposeActions)
          item_0();
        this.disposeActions.Clear();
      }
    }

    public void Present(Rectangle? sourceRectangle, Rectangle? destinationRectangle, IntPtr overrideWindowHandle)
    {
      throw new NotImplementedException();
    }

    public void Reset()
    {
      throw new NotImplementedException();
    }

    public void Reset(PresentationParameters presentationParameters)
    {
      throw new NotImplementedException();
    }

    public void Reset(PresentationParameters presentationParameters, GraphicsAdapter graphicsAdapter)
    {
      throw new NotImplementedException();
    }

    internal void OnDeviceResetting()
    {
      if (this.DeviceResetting == null)
        return;
      this.DeviceResetting((object) this, EventArgs.Empty);
    }

    internal void OnDeviceReset()
    {
      if (this.DeviceReset != null)
        this.DeviceReset((object) this, EventArgs.Empty);
      GraphicsResource.DoGraphicsDeviceResetting();
    }

    public void SetRenderTarget(RenderTarget2D renderTarget)
    {
      if (renderTarget == null)
        this.SetRenderTargets((RenderTargetBinding[]) null);
      else
        this.SetRenderTargets(new RenderTargetBinding[1]
        {
          new RenderTargetBinding(renderTarget)
        });
    }

    public void SetRenderTargets(params RenderTargetBinding[] renderTargets)
    {
      if (this._currentRenderTargetBindings == null && renderTargets == null)
        return;
      if (this._currentRenderTargetBindings != null && renderTargets != null && this._currentRenderTargetBindings.Length == renderTargets.Length)
      {
        bool flag = true;
        for (int index = 0; index < this._currentRenderTargetBindings.Length; ++index)
        {
          if (this._currentRenderTargetBindings[index].RenderTarget != renderTargets[index].RenderTarget)
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return;
      }
      this.ApplyRenderTargets(renderTargets);
    }

    internal void ApplyRenderTargets(RenderTargetBinding[] renderTargets)
    {
      RenderTargetBinding[] renderTargetBindingArray = this._currentRenderTargetBindings;
      this._currentRenderTargetBindings = renderTargets;
      this._vertexShaderDirty = true;
      bool flag;
      if (this._currentRenderTargetBindings == null || this._currentRenderTargetBindings.Length == 0)
      {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, this.glFramebuffer);
        flag = true;
        this.Viewport = new Viewport(0, 0, this.PresentationParameters.BackBufferWidth, this.PresentationParameters.BackBufferHeight);
      }
      else
      {
        RenderTarget2D renderTarget = this._currentRenderTargetBindings[0].RenderTarget as RenderTarget2D;
        if ((int) renderTarget.glFramebuffer == 0)
          GL.GenFramebuffers(1, out renderTarget.glFramebuffer);
        Threading.BlockOnUIThread((Action) (() =>
        {
          GL.BindFramebuffer(FramebufferTarget.Framebuffer, renderTarget.glFramebuffer);
          GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, renderTarget.glTexture, 0);
          if (renderTarget.DepthStencilFormat != DepthFormat.None)
          {
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, renderTarget.glDepthStencilBuffer);
            if (renderTarget.DepthStencilFormat == DepthFormat.Depth24Stencil8)
              GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer, renderTarget.glDepthStencilBuffer);
          }
          FramebufferErrorCode local_0 = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
          if (local_0 == FramebufferErrorCode.FramebufferComplete)
            return;
          string local_1 = "Framebuffer Incomplete.";
          switch (local_0)
          {
            case FramebufferErrorCode.FramebufferIncompleteAttachment:
              local_1 = "Not all framebuffer attachment points are framebuffer attachment complete.";
              break;
            case FramebufferErrorCode.FramebufferIncompleteMissingAttachment:
              local_1 = "No images are attached to the framebuffer.";
              break;
            case FramebufferErrorCode.FramebufferUnsupported:
              local_1 = "The combination of internal formats of the attached images violates an implementation-dependent set of restrictions.";
              break;
          }
          throw new InvalidOperationException(local_1);
        }));
        this.Viewport = new Viewport(0, 0, renderTarget.Width, renderTarget.Height);
        flag = renderTarget.RenderTargetUsage == RenderTargetUsage.DiscardContents;
      }
      if (flag)
        this.Clear(GraphicsDevice.DiscardColor);
      if (renderTargetBindingArray != null)
      {
        for (int index = 0; index < renderTargetBindingArray.Length; ++index)
        {
          if (renderTargetBindingArray[index].RenderTarget.LevelCount > 1)
            throw new NotImplementedException();
        }
      }
      this._rasterizerStateDirty = true;
    }

    public RenderTargetBinding[] GetRenderTargets()
    {
      if (!this.IsRenderTargetBound)
        return GraphicsDevice.EmptyRenderTargetBinding;
      else
        return this._currentRenderTargetBindings;
    }

    private static BeginMode PrimitiveTypeGL(PrimitiveType primitiveType)
    {
      switch (primitiveType)
      {
        case PrimitiveType.TriangleList:
          return BeginMode.Triangles;
        case PrimitiveType.TriangleStrip:
          return BeginMode.TriangleStrip;
        case PrimitiveType.LineList:
          return BeginMode.Lines;
        case PrimitiveType.LineStrip:
          return BeginMode.LineStrip;
        default:
          throw new NotImplementedException();
      }
    }

    public void SetVertexBuffer(VertexBuffer vertexBuffer)
    {
      if (this._vertexBuffer == vertexBuffer)
        return;
      this._vertexBuffer = vertexBuffer;
      this._vertexBufferDirty = true;
    }

    private void SetIndexBuffer(IndexBuffer indexBuffer)
    {
      if (this._indexBuffer == indexBuffer)
        return;
      this._indexBuffer = indexBuffer;
      this._indexBufferDirty = true;
    }

    internal void SetConstantBuffer(ShaderStage stage, int slot, ConstantBuffer buffer)
    {
      if (stage == ShaderStage.Vertex)
        this._vertexConstantBuffers[slot] = buffer;
      else
        this._pixelConstantBuffers[slot] = buffer;
    }

    private void ActivateShaderProgram()
    {
      ShaderProgramInfo programInfo = this._programCache.GetProgramInfo(this.VertexShader, this.PixelShader);
      if (programInfo.program == -1)
        return;
      if (this._shaderProgram != programInfo.program)
      {
        GL.UseProgram(programInfo.program);
        this._shaderProgram = programInfo.program;
      }
      if (programInfo.posFixupLoc == -1)
        return;
      GraphicsDevice._posFixup[0] = 1f;
      GraphicsDevice._posFixup[1] = 1f;
      GraphicsDevice._posFixup[2] = 63.0 / 64.0 / (float) this.Viewport.Width;
      GraphicsDevice._posFixup[3] = -63.0 / 64.0 / (float) this.Viewport.Height;
      if (this.IsRenderTargetBound)
      {
        GraphicsDevice._posFixup[1] *= -1f;
        GraphicsDevice._posFixup[3] *= -1f;
      }
      GL.Uniform4(programInfo.posFixupLoc, 1, GraphicsDevice._posFixup);
    }

    internal void ApplyState(bool applyShaders)
    {
      if (this._scissorRectangleDirty)
      {
        Rectangle rectangle = this._scissorRectangle;
        if (!this.IsRenderTargetBound)
          rectangle.Y = this._viewport.Height - rectangle.Y - rectangle.Height;
        GL.Scissor(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        this._scissorRectangleDirty = false;
      }
      if (this._blendStateDirty)
      {
        this._blendState.ApplyState(this);
        this._blendStateDirty = false;
      }
      if (this._depthStencilStateDirty)
      {
        this._depthStencilState.ApplyState(this);
        this._depthStencilStateDirty = false;
      }
      if (this._rasterizerStateDirty)
      {
        this._rasterizerState.ApplyState(this);
        this._rasterizerStateDirty = false;
      }
      if (!applyShaders)
        return;
      if (this._indexBufferDirty)
      {
        if (this._indexBuffer != null)
          GL.BindBuffer(BufferTarget.ElementArrayBuffer, this._indexBuffer.ibo);
        this._indexBufferDirty = false;
      }
      if (this._vertexBufferDirty && this._vertexBuffer != null)
        GL.BindBuffer(BufferTarget.ArrayBuffer, this._vertexBuffer.vbo);
      if (this._vertexShader == null)
        throw new InvalidOperationException("A vertex shader must be set!");
      if (this._pixelShader == null)
        throw new InvalidOperationException("A pixel shader must not set!");
      if (this._vertexShaderDirty || this._pixelShaderDirty)
      {
        this.ActivateShaderProgram();
        this._vertexShaderDirty = this._pixelShaderDirty = false;
      }
      this._vertexConstantBuffers.SetConstantBuffers(this, this._shaderProgram);
      this._pixelConstantBuffers.SetConstantBuffers(this, this._shaderProgram);
      this.Textures.SetTextures(this);
      this.SamplerStates.SetSamplers(this);
    }

    public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numbVertices, int startIndex, int primitiveCount)
    {
      if (minVertexIndex > 0)
        throw new NotImplementedException("minVertexIndex > 0 is supported");
      this.ApplyState(true);
      bool flag = this._indexBuffer.IndexElementSize == IndexElementSize.SixteenBits;
      DrawElementsType type = flag ? DrawElementsType.UnsignedShort : DrawElementsType.UnsignedInt;
      int num = flag ? 2 : 4;
      IntPtr indices = (IntPtr) (startIndex * num);
      int elementCountArray = GraphicsDevice.GetElementCountArray(primitiveType, primitiveCount);
      BeginMode mode = GraphicsDevice.PrimitiveTypeGL(primitiveType);
      this._vertexBuffer.VertexDeclaration.Apply(this._vertexShader, (IntPtr) (this._vertexBuffer.VertexDeclaration.VertexStride * baseVertex));
      GL.DrawElements(mode, elementCountArray, type, indices);
    }

    public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount) where T : struct, IVertexType
    {
      this.DrawUserPrimitives<T>(primitiveType, vertexData, vertexOffset, primitiveCount, VertexDeclarationCache<T>.VertexDeclaration);
    }

    public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct, IVertexType
    {
      int elementCountArray = GraphicsDevice.GetElementCountArray(primitiveType, primitiveCount);
      this.ApplyState(true);
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
      this._vertexBufferDirty = this._indexBufferDirty = true;
      GCHandle gcHandle = GCHandle.Alloc((object) vertexData, GCHandleType.Pinned);
      vertexDeclaration.GraphicsDevice = this;
      vertexDeclaration.Apply(this._vertexShader, gcHandle.AddrOfPinnedObject());
      GL.DrawArrays(GraphicsDevice.PrimitiveTypeGL(primitiveType), vertexOffset, elementCountArray);
      gcHandle.Free();
    }

    public void DrawPrimitives(PrimitiveType primitiveType, int vertexStart, int primitiveCount)
    {
      int elementCountArray = GraphicsDevice.GetElementCountArray(primitiveType, primitiveCount);
      this.ApplyState(true);
      this._vertexBuffer.VertexDeclaration.Apply(this._vertexShader, IntPtr.Zero);
      GL.DrawArrays(GraphicsDevice.PrimitiveTypeGL(primitiveType), vertexStart, elementCountArray);
    }

    public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount) where T : struct, IVertexType
    {
      this.DrawUserIndexedPrimitives<T>(primitiveType, vertexData, vertexOffset, numVertices, indexData, indexOffset, primitiveCount, VertexDeclarationCache<T>.VertexDeclaration);
    }

    public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct, IVertexType
    {
      this.ApplyState(true);
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
      this._vertexBufferDirty = this._indexBufferDirty = true;
      GCHandle gcHandle1 = GCHandle.Alloc((object) vertexData, GCHandleType.Pinned);
      GCHandle gcHandle2 = GCHandle.Alloc((object) indexData, GCHandleType.Pinned);
      vertexDeclaration.GraphicsDevice = this;
      vertexDeclaration.Apply(this._vertexShader, gcHandle1.AddrOfPinnedObject());
      GL.DrawElements(GraphicsDevice.PrimitiveTypeGL(primitiveType), GraphicsDevice.GetElementCountArray(primitiveType, primitiveCount), DrawElementsType.UnsignedShort, (IntPtr) (gcHandle2.AddrOfPinnedObject().ToInt64() + (long) (indexOffset * 2)));
      gcHandle2.Free();
      gcHandle1.Free();
    }

    public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount) where T : struct, IVertexType
    {
      this.DrawUserIndexedPrimitives<T>(primitiveType, vertexData, vertexOffset, numVertices, indexData, indexOffset, primitiveCount, VertexDeclarationCache<T>.VertexDeclaration);
    }

    public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct, IVertexType
    {
      this.ApplyState(true);
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
      this._vertexBufferDirty = this._indexBufferDirty = true;
      GCHandle gcHandle1 = GCHandle.Alloc((object) vertexData, GCHandleType.Pinned);
      GCHandle gcHandle2 = GCHandle.Alloc((object) indexData, GCHandleType.Pinned);
      vertexDeclaration.GraphicsDevice = this;
      vertexDeclaration.Apply(this._vertexShader, gcHandle1.AddrOfPinnedObject());
      GL.DrawElements(GraphicsDevice.PrimitiveTypeGL(primitiveType), GraphicsDevice.GetElementCountArray(primitiveType, primitiveCount), DrawElementsType.UnsignedInt, (IntPtr) (gcHandle2.AddrOfPinnedObject().ToInt64() + (long) (indexOffset * 4)));
      gcHandle2.Free();
      gcHandle1.Free();
    }

    private static int GetElementCountArray(PrimitiveType primitiveType, int primitiveCount)
    {
      switch (primitiveType)
      {
        case PrimitiveType.TriangleList:
          return primitiveCount * 3;
        case PrimitiveType.TriangleStrip:
          return 3 + (primitiveCount - 1);
        case PrimitiveType.LineList:
          return primitiveCount * 2;
        case PrimitiveType.LineStrip:
          return primitiveCount + 1;
        default:
          throw new NotSupportedException();
      }
    }
  }
}
