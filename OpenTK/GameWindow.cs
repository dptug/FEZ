// Type: OpenTK.GameWindow
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace OpenTK
{
  public class GameWindow : NativeWindow, IGameWindow, INativeWindow, IDisposable
  {
    private object exit_lock = new object();
    private Stopwatch update_watch = new Stopwatch();
    private Stopwatch render_watch = new Stopwatch();
    private FrameEventArgs update_args = new FrameEventArgs();
    private FrameEventArgs render_args = new FrameEventArgs();
    private IGraphicsContext glContext;
    private bool isExiting;
    private double update_period;
    private double render_period;
    private double target_update_period;
    private double target_render_period;
    private double update_time;
    private double render_time;
    private VSyncMode vsync;
    private double next_render;
    private double next_update;

    public IGraphicsContext Context
    {
      get
      {
        this.EnsureUndisposed();
        return this.glContext;
      }
    }

    public bool IsExiting
    {
      get
      {
        this.EnsureUndisposed();
        return this.isExiting;
      }
    }

    public IList<JoystickDevice> Joysticks
    {
      get
      {
        return this.InputDriver.Joysticks;
      }
    }

    public KeyboardDevice Keyboard
    {
      get
      {
        if (this.InputDriver.Keyboard.Count <= 0)
          return (KeyboardDevice) null;
        else
          return this.InputDriver.Keyboard[0];
      }
    }

    public MouseDevice Mouse
    {
      get
      {
        if (this.InputDriver.Mouse.Count <= 0)
          return (MouseDevice) null;
        else
          return this.InputDriver.Mouse[0];
      }
    }

    public double RenderFrequency
    {
      get
      {
        this.EnsureUndisposed();
        if (this.render_period == 0.0)
          return 1.0;
        else
          return 1.0 / this.render_period;
      }
    }

    public double RenderPeriod
    {
      get
      {
        this.EnsureUndisposed();
        return this.render_period;
      }
    }

    public double RenderTime
    {
      get
      {
        this.EnsureUndisposed();
        return this.render_time;
      }
      protected set
      {
        this.EnsureUndisposed();
        this.render_time = value;
      }
    }

    public double TargetRenderFrequency
    {
      get
      {
        this.EnsureUndisposed();
        if (this.TargetRenderPeriod == 0.0)
          return 0.0;
        else
          return 1.0 / this.TargetRenderPeriod;
      }
      set
      {
        this.EnsureUndisposed();
        if (value < 1.0)
        {
          this.TargetRenderPeriod = 0.0;
        }
        else
        {
          if (value > 200.0)
            return;
          this.TargetRenderPeriod = 1.0 / value;
        }
      }
    }

    public double TargetRenderPeriod
    {
      get
      {
        this.EnsureUndisposed();
        return this.target_render_period;
      }
      set
      {
        this.EnsureUndisposed();
        if (value <= 0.005)
        {
          this.target_render_period = 0.0;
        }
        else
        {
          if (value > 1.0)
            return;
          this.target_render_period = value;
        }
      }
    }

    public double TargetUpdateFrequency
    {
      get
      {
        this.EnsureUndisposed();
        if (this.TargetUpdatePeriod == 0.0)
          return 0.0;
        else
          return 1.0 / this.TargetUpdatePeriod;
      }
      set
      {
        this.EnsureUndisposed();
        if (value < 1.0)
        {
          this.TargetUpdatePeriod = 0.0;
        }
        else
        {
          if (value > 200.0)
            return;
          this.TargetUpdatePeriod = 1.0 / value;
        }
      }
    }

    public double TargetUpdatePeriod
    {
      get
      {
        this.EnsureUndisposed();
        return this.target_update_period;
      }
      set
      {
        this.EnsureUndisposed();
        if (value <= 0.005)
        {
          this.target_update_period = 0.0;
        }
        else
        {
          if (value > 1.0)
            return;
          this.target_update_period = value;
        }
      }
    }

    public double UpdateFrequency
    {
      get
      {
        this.EnsureUndisposed();
        if (this.update_period == 0.0)
          return 1.0;
        else
          return 1.0 / this.update_period;
      }
    }

    public double UpdatePeriod
    {
      get
      {
        this.EnsureUndisposed();
        return this.update_period;
      }
    }

    public double UpdateTime
    {
      get
      {
        this.EnsureUndisposed();
        return this.update_time;
      }
    }

    public VSyncMode VSync
    {
      get
      {
        this.EnsureUndisposed();
        GraphicsContext.Assert();
        return this.vsync;
      }
      set
      {
        this.EnsureUndisposed();
        GraphicsContext.Assert();
        this.Context.VSync = (this.vsync = value) != VSyncMode.Off;
      }
    }

    public override WindowState WindowState
    {
      get
      {
        return base.WindowState;
      }
      set
      {
        base.WindowState = value;
        if (this.Context == null)
          return;
        this.Context.Update(this.WindowInfo);
      }
    }

    public event EventHandler<EventArgs> Load = delegate {};

    public event EventHandler<FrameEventArgs> RenderFrame = delegate {};

    public event EventHandler<EventArgs> Unload = delegate {};

    public event EventHandler<FrameEventArgs> UpdateFrame = delegate {};

    public GameWindow()
      : this(640, 480, GraphicsMode.Default, "OpenTK Game Window", GameWindowFlags.Default, DisplayDevice.Default)
    {
    }

    public GameWindow(int width, int height)
      : this(width, height, GraphicsMode.Default, "OpenTK Game Window", GameWindowFlags.Default, DisplayDevice.Default)
    {
    }

    public GameWindow(int width, int height, GraphicsMode mode)
      : this(width, height, mode, "OpenTK Game Window", GameWindowFlags.Default, DisplayDevice.Default)
    {
    }

    public GameWindow(int width, int height, GraphicsMode mode, string title)
      : this(width, height, mode, title, GameWindowFlags.Default, DisplayDevice.Default)
    {
    }

    public GameWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options)
      : this(width, height, mode, title, options, DisplayDevice.Default)
    {
    }

    public GameWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device)
      : this(width, height, mode, title, options, device, 1, 0, GraphicsContextFlags.Default)
    {
    }

    public GameWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device, int major, int minor, GraphicsContextFlags flags)
      : this(width, height, mode, title, options, device, major, minor, flags, (IGraphicsContext) null)
    {
    }

    public GameWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options, DisplayDevice device, int major, int minor, GraphicsContextFlags flags, IGraphicsContext sharedContext)
      : base(width, height, title, options, mode == null ? GraphicsMode.Default : mode, device == null ? DisplayDevice.Default : device)
    {
      try
      {
        this.glContext = (IGraphicsContext) new GraphicsContext(mode == null ? GraphicsMode.Default : mode, this.WindowInfo, major, minor, flags);
        this.glContext.MakeCurrent(this.WindowInfo);
        (this.glContext as IGraphicsContextInternal).LoadAll();
        this.VSync = VSyncMode.On;
      }
      catch (Exception ex)
      {
        base.Dispose();
        throw;
      }
    }

    public override void Dispose()
    {
      try
      {
        this.Dispose(true);
      }
      finally
      {
        try
        {
          if (this.glContext != null)
          {
            this.glContext.Dispose();
            this.glContext = (IGraphicsContext) null;
          }
        }
        finally
        {
          base.Dispose();
        }
      }
      GC.SuppressFinalize((object) this);
    }

    public virtual void Exit()
    {
      this.Close();
    }

    public void MakeCurrent()
    {
      this.EnsureUndisposed();
      this.Context.MakeCurrent(this.WindowInfo);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);
      if (e.Cancel)
        return;
      this.isExiting = true;
      this.OnUnloadInternal(EventArgs.Empty);
    }

    protected virtual void OnLoad(EventArgs e)
    {
      this.Load((object) this, e);
    }

    protected virtual void OnUnload(EventArgs e)
    {
      this.Unload((object) this, e);
    }

    public void Run()
    {
      this.Run(0.0, 0.0);
    }

    public void Run(double updateRate)
    {
      this.Run(updateRate, 0.0);
    }

    public void Run(double updates_per_second, double frames_per_second)
    {
      this.EnsureUndisposed();
      try
      {
        if (updates_per_second < 0.0 || updates_per_second > 200.0)
          throw new ArgumentOutOfRangeException("updates_per_second", (object) updates_per_second, "Parameter should be inside the range [0.0, 200.0]");
        if (frames_per_second < 0.0 || frames_per_second > 200.0)
          throw new ArgumentOutOfRangeException("frames_per_second", (object) frames_per_second, "Parameter should be inside the range [0.0, 200.0]");
        this.TargetUpdateFrequency = updates_per_second;
        this.TargetRenderFrequency = frames_per_second;
        this.Visible = true;
        this.OnLoadInternal(EventArgs.Empty);
        this.OnResize(EventArgs.Empty);
        this.update_watch.Start();
        this.render_watch.Start();
        while (true)
        {
          this.ProcessEvents();
          if (this.Exists && !this.IsExiting)
            this.DispatchUpdateAndRenderFrame((object) this, EventArgs.Empty);
          else
            break;
        }
      }
      finally
      {
        this.Move -= new EventHandler<EventArgs>(this.DispatchUpdateAndRenderFrame);
        this.Resize -= new EventHandler<EventArgs>(this.DispatchUpdateAndRenderFrame);
        int num = this.Exists ? 1 : 0;
      }
    }

    private void DispatchUpdateAndRenderFrame(object sender, EventArgs e)
    {
      this.RaiseUpdateFrame(this.update_watch, ref this.next_update, this.update_args);
      this.RaiseRenderFrame(this.render_watch, ref this.next_render, this.render_args);
    }

    private void RaiseUpdateFrame(Stopwatch update_watch, ref double next_update, FrameEventArgs update_args)
    {
      int num1 = 0;
      double num2 = 0.0;
      double num3 = update_watch.Elapsed.TotalSeconds;
      if (num3 <= 0.0)
      {
        update_watch.Reset();
        update_watch.Start();
      }
      else
      {
        if (num3 > 1.0)
          num3 = 1.0;
        while (next_update - num3 <= 0.0 && num3 > 0.0)
        {
          next_update -= num3;
          update_args.Time = num3;
          this.OnUpdateFrameInternal(update_args);
          GameWindow gameWindow = this;
          TimeSpan elapsed = update_watch.Elapsed;
          double num4;
          double num5 = num4 = Math.Max(elapsed.TotalSeconds, 0.0) - num3;
          gameWindow.update_time = num4;
          num3 = num5;
          update_watch.Reset();
          update_watch.Start();
          next_update += this.TargetUpdatePeriod;
          next_update = Math.Max(next_update, -1.0);
          num2 += this.update_time;
          if (++num1 >= 10 || this.TargetUpdateFrequency == 0.0)
            break;
        }
        if (num1 <= 0)
          return;
        this.update_period = num2 / (double) num1;
      }
    }

    private void RaiseRenderFrame(Stopwatch render_watch, ref double next_render, FrameEventArgs render_args)
    {
      double num1 = render_watch.Elapsed.TotalSeconds;
      if (num1 <= 0.0)
      {
        render_watch.Reset();
        render_watch.Start();
      }
      else
      {
        if (num1 > 1.0)
          num1 = 1.0;
        double num2 = next_render - num1;
        if (num2 > 0.0 || num1 <= 0.0)
          return;
        next_render = num2 + this.TargetRenderPeriod;
        if (next_render < -1.0)
          next_render = -1.0;
        render_watch.Reset();
        render_watch.Start();
        if (num1 <= 0.0)
          return;
        if (this.Context.IsCurrent && this.VSync == VSyncMode.Adaptive && this.TargetRenderPeriod != 0.0)
          this.Context.SwapInterval = this.RenderTime <= 2.0 * this.TargetRenderPeriod ? 1 : 0;
        this.render_period = render_args.Time = num1;
        this.OnRenderFrameInternal(render_args);
        this.render_time = render_watch.Elapsed.TotalSeconds;
      }
    }

    public void SwapBuffers()
    {
      this.EnsureUndisposed();
      this.Context.SwapBuffers();
    }

    protected virtual void Dispose(bool manual)
    {
    }

    protected virtual void OnRenderFrame(FrameEventArgs e)
    {
      this.RenderFrame((object) this, e);
    }

    protected virtual void OnUpdateFrame(FrameEventArgs e)
    {
      this.UpdateFrame((object) this, e);
    }

    protected virtual void OnWindowInfoChanged(EventArgs e)
    {
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      this.glContext.Update(this.WindowInfo);
    }

    private void OnLoadInternal(EventArgs e)
    {
      this.OnLoad(e);
    }

    private void OnRenderFrameInternal(FrameEventArgs e)
    {
      if (!this.Exists || this.isExiting)
        return;
      this.OnRenderFrame(e);
    }

    private void OnUnloadInternal(EventArgs e)
    {
      this.OnUnload(e);
    }

    private void OnUpdateFrameInternal(FrameEventArgs e)
    {
      if (!this.Exists || this.isExiting)
        return;
      this.OnUpdateFrame(e);
    }

    private void OnWindowInfoChangedInternal(EventArgs e)
    {
      this.glContext.MakeCurrent(this.WindowInfo);
      this.OnWindowInfoChanged(e);
    }
  }
}
