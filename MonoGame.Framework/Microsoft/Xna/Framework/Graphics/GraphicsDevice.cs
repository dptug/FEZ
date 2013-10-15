// Type: Microsoft.Xna.Framework.Graphics.GraphicsDevice
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microsoft.Xna.Framework.Graphics
{
  public class GraphicsDevice : IDisposable
  {
    private static readonly RenderTargetBinding[] EmptyRenderTargetBinding = new RenderTargetBinding[0];
    private static readonly Color DiscardColor = new Color(68, 34, 136, (int) byte.MaxValue);
    private static List<Action> disposeActions = new List<Action>();
    private static object disposeActionsLock = new object();
    private static readonly float[] _posFixup = new float[4];
    internal static readonly List<int> _enabledVertexAttributes = new List<int>();
    private BlendState _blendState = BlendState.Opaque;
    private DepthStencilState _depthStencilState = DepthStencilState.Default;
    private RasterizerState _rasterizerState = RasterizerState.CullCounterClockwise;
    private readonly ConstantBufferCollection _vertexConstantBuffers = new ConstantBufferCollection(ShaderStage.Vertex, 16);
    private readonly ConstantBufferCollection _pixelConstantBuffers = new ConstantBufferCollection(ShaderStage.Pixel, 16);
    private readonly ShaderProgramCache _programCache = new ShaderProgramCache();
    private int _shaderProgram = -1;
    internal readonly HashSet<string> _extensions = new HashSet<string>();
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
    internal uint glRenderTargetFrameBuffer;
    internal int MaxVertexAttributes;
    internal int _maxTextureSize;
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
        if (this._currentRenderTargetBindings != null)
          return this._currentRenderTargetBindings.Length > 0;
        else
          return false;
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
        if (!Threading.IsOnUIThread())
          return;
        if (this.IsRenderTargetBound)
          GL.Viewport(value.X, value.Y, value.Width, value.Height);
        else
          GL.Viewport(value.X, this.PresentationParameters.BackBufferHeight - value.Y - value.Height, value.Width, value.Height);
        GL.DepthRange((double) value.MinDepth, (double) value.MaxDepth);
        this._vertexShaderDirty = true;
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
      get
      {
        return this._indexBuffer;
      }
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

    public event EventHandler<ResourceCreatedEventArgs> ResourceCreated;

    public event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;

    public event EventHandler<EventArgs> Disposing;

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
      GL.GetInteger(GetPName.MaxTextureSize, out this._maxTextureSize);
      this.GetGLExtensions();
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

    private void GetGLExtensions()
    {
      string string1 = GL.GetString(StringName.Version);
      int num1 = int.Parse(string1.Substring(0, 1));
      GraphicsExtensions.LogToFile(GraphicsExtensions.LogSeverity.Information, "OpenGL version " + string1);
      GraphicsExtensions.LogToFile(GraphicsExtensions.LogSeverity.Information, "Vendor is " + GL.GetString(StringName.Vendor));
      GraphicsExtensions.LogToFile(GraphicsExtensions.LogSeverity.Information, "GLSL version " + GL.GetString(StringName.ShadingLanguageVersion));
      if (num1 >= 3)
      {
        int @params;
        GL.GetInteger(GetPName.NumExtensions, out @params);
        for (int index = 0; index < @params; ++index)
          this._extensions.Add(GL.GetString(StringName.Extensions, index));
      }
      else
      {
        string string2 = GL.GetString(StringName.Extensions);
        char[] chArray = new char[1]
        {
          ' '
        };
        foreach (string str in string2.Split(chArray))
          this._extensions.Add(str);
      }
      GraphicsExtensions.UseArbFramebuffer = this._extensions.Contains("GL_ARB_framebuffer_object");
      if (!GraphicsExtensions.UseArbFramebuffer)
      {
        if (!this._extensions.Contains("GL_EXT_framebuffer_object"))
        {
          int num2 = (int) MessageBox.Show("An essential rendering feature is unsupported in your current drivers.\nTry updating your drivers to the latest version.\n\nIf you are using the latest available drivers, your video card might not be able to run FEZ.", "FEZ - Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          throw new InvalidOperationException("Framebuffer objects are not supported by the current OpenGL driver, please update your drivers and try again!");
        }
        else
          GraphicsExtensions.LogToFile(GraphicsExtensions.LogSeverity.Warning, "GL_ARB_framebuffer_object not supported : will use GL_EXT_framebuffer_object instead.");
      }
      GraphicsExtensions.FboMultisampleSupported = (this._extensions.Contains("GL_EXT_framebuffer_multisample") || this._extensions.Contains("ARB_framebuffer_multisample")) && this._extensions.Contains("GL_EXT_framebuffer_blit");
      if (!this._extensions.Contains("GL_EXT_packed_depth_stencil"))
        GraphicsExtensions.LogToFile(GraphicsExtensions.LogSeverity.Warning, "Packed depth-stencil is NOT supported");
      GraphicsExtensions.UseDxtCompression = int.Parse(string1.Substring(0, 1)) > 2 && this._extensions.Contains("GL_EXT_texture_compression_s3tc");
      if (GraphicsExtensions.UseDxtCompression)
        return;
      GraphicsExtensions.LogToFile(GraphicsExtensions.LogSeverity.Warning, "No S3TC/DXT support : will decompress DXT textures at load time.");
    }

    internal void Initialize()
    {
      GraphicsCapabilities.Initialize(this);
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
      if ((options & ClearOptions.Target) == ClearOptions.Target)
      {
        GL.ClearColor(color.X, color.Y, color.Z, color.W);
        mask |= ClearBufferMask.ColorBufferBit;
      }
      if ((options & ClearOptions.Stencil) == ClearOptions.Stencil)
      {
        GL.ClearStencil(stencil);
        mask |= ClearBufferMask.StencilBufferBit;
      }
      if ((options & ClearOptions.DepthBuffer) == ClearOptions.DepthBuffer)
      {
        GL.ClearDepth((double) depth);
        mask |= ClearBufferMask.DepthBufferBit;
      }
      GL.Clear(mask);
      this.ScissorRectangle = scissorRectangle;
      this.DepthStencilState = depthStencilState;
      this.BlendState = blendState;
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
        GraphicsDevice.AddDisposeAction((Action) (() =>
        {
          if (this.glRenderTargetFrameBuffer <= 0U)
            return;
          GraphicsExtensions.DeleteFramebuffers(1, ref this.glRenderTargetFrameBuffer);
        }));
      }
      this._isDisposed = true;
    }

    internal static void AddDisposeAction(Action disposeAction)
    {
      if (disposeAction == null)
        throw new ArgumentNullException("disposeAction");
      if (Threading.IsOnUIThread())
      {
        disposeAction();
      }
      else
      {
        lock (GraphicsDevice.disposeActionsLock)
          GraphicsDevice.disposeActions.Add(disposeAction);
      }
    }

    public void Present()
    {
      GL.Flush();
      lock (GraphicsDevice.disposeActionsLock)
      {
        if (GraphicsDevice.disposeActions.Count <= 0)
          return;
        foreach (Action item_0 in GraphicsDevice.disposeActions)
          item_0();
        GraphicsDevice.disposeActions.Clear();
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
      if (this.DeviceResetting != null)
        this.DeviceResetting((object) this, EventArgs.Empty);
      GraphicsResource.DoGraphicsDeviceResetting();
    }

    internal void OnDeviceReset()
    {
      if (this.DeviceReset != null)
        this.DeviceReset((object) this, EventArgs.Empty);
      if (this.glRenderTargetFrameBuffer <= 0U)
        return;
      GraphicsExtensions.DeleteFramebuffers(1, ref this.glRenderTargetFrameBuffer);
      this.glRenderTargetFrameBuffer = 0U;
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

    public void SetRenderTarget(RenderTargetCube renderTarget, CubeMapFace cubeMapFace)
    {
      if (renderTarget == null)
        this.SetRenderTarget((RenderTarget2D) null);
      else
        this.SetRenderTargets(new RenderTargetBinding[1]
        {
          new RenderTargetBinding(renderTarget, cubeMapFace)
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
          if (this._currentRenderTargetBindings[index].RenderTarget != renderTargets[index].RenderTarget || this._currentRenderTargetBindings[index].CubeMapFace != renderTargets[index].CubeMapFace)
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
      bool flag;
      if (this._currentRenderTargetBindings == null || this._currentRenderTargetBindings.Length == 0)
      {
        GraphicsExtensions.BindFramebuffer(FramebufferTarget.Framebuffer, this.glFramebuffer);
        flag = true;
        this.Viewport = new Viewport(0, 0, this.PresentationParameters.BackBufferWidth, this.PresentationParameters.BackBufferHeight);
      }
      else
      {
        if (this._currentRenderTargetBindings[0].RenderTarget is RenderTargetCube)
          throw new NotImplementedException("RenderTargetCube not yet implemented.");
        RenderTarget2D renderTarget = this._currentRenderTargetBindings[0].RenderTarget as RenderTarget2D;
        if ((int) this.glRenderTargetFrameBuffer == 0)
          GraphicsExtensions.GenFramebuffers(1, out this.glRenderTargetFrameBuffer);
        Threading.BlockOnUIThread((Action) (() =>
        {
          GraphicsExtensions.BindFramebuffer(FramebufferTarget.Framebuffer, this.glRenderTargetFrameBuffer);
          GraphicsExtensions.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, renderTarget.glTexture, 0);
          if (renderTarget.DepthStencilFormat != DepthFormat.None)
          {
            GraphicsExtensions.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, renderTarget.glDepthStencilBuffer);
            if (renderTarget.DepthStencilFormat == DepthFormat.Depth24Stencil8)
              GraphicsExtensions.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer, renderTarget.glDepthStencilBuffer);
            else
              GraphicsExtensions.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer, 0U);
          }
          else
            GraphicsExtensions.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, 0U);
          FramebufferErrorCode local_0 = GraphicsExtensions.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
          if (local_0 == FramebufferErrorCode.FramebufferComplete)
            return;
          string local_1 = "Framebuffer Incomplete (" + (object) local_0 + ") : ";
          switch (local_0)
          {
            case FramebufferErrorCode.FramebufferIncompleteAttachment:
              local_1 = local_1 + "Not all framebuffer attachment points are framebuffer attachment complete.";
              break;
            case FramebufferErrorCode.FramebufferIncompleteMissingAttachment:
              local_1 = local_1 + "No images are attached to the framebuffer.";
              break;
            case FramebufferErrorCode.FramebufferIncompleteDimensionsExt:
              local_1 = local_1 + "Not all attached images have the same width and height.";
              break;
            case FramebufferErrorCode.FramebufferUnsupported:
              local_1 = local_1 + "The combination of internal formats of the attached images violates an implementation-dependent set of restrictions.";
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
          Texture renderTarget = renderTargetBindingArray[index].RenderTarget;
          if (renderTarget.LevelCount > 1)
          {
            if (!(renderTarget is RenderTarget2D))
              throw new NotImplementedException();
            (renderTarget as RenderTarget2D).GenerateMipmaps();
          }
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

    public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount)
    {
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
      IntPtr offset = (IntPtr) (gcHandle1.AddrOfPinnedObject().ToInt64() + (long) (vertexDeclaration.VertexStride * vertexOffset));
      vertexDeclaration.GraphicsDevice = this;
      vertexDeclaration.Apply(this._vertexShader, offset);
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
      IntPtr offset = (IntPtr) (gcHandle1.AddrOfPinnedObject().ToInt64() + (long) (vertexDeclaration.VertexStride * vertexOffset));
      vertexDeclaration.GraphicsDevice = this;
      vertexDeclaration.Apply(this._vertexShader, offset);
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
