// Type: FezEngine.Services.TargetRenderingManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Effects;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Services
{
  public class TargetRenderingManager : GameComponent, ITargetRenderingManager
  {
    private readonly List<RenderTargetHandle> fullscreenRTs = new List<RenderTargetHandle>();
    private readonly List<TargetRenderingManager.RtHook> renderTargetsToHook = new List<TargetRenderingManager.RtHook>();
    private int currentlyHookedRtIndex = -1;
    private readonly Mesh fullscreenPlane;
    private BasicPostEffect basicPostEffect;
    private GraphicsDevice graphicsDevice;
    private Texture2D fullWhite;

    public bool HasRtInQueue
    {
      get
      {
        return this.renderTargetsToHook.Count > 0;
      }
    }

    [ServiceDependency]
    public IGraphicsDeviceService GraphicsDeviceService { protected get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { protected get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { protected get; set; }

    private event Action nextFrameHooks;

    public event Action<GameTime> PreDraw = new Action<GameTime>(Util.NullAction<GameTime>);

    public TargetRenderingManager(Game game)
      : base(game)
    {
      this.fullscreenPlane = new Mesh()
      {
        DepthWrites = false,
        AlwaysOnTop = true
      };
      this.fullscreenPlane.AddFace(Vector3.One * 2f, Vector3.Zero, FaceOrientation.Front, true);
    }

    public void ScheduleHook(int drawOrder, RenderTarget2D rt)
    {
      if (Enumerable.Any<TargetRenderingManager.RtHook>((IEnumerable<TargetRenderingManager.RtHook>) this.renderTargetsToHook, (Func<TargetRenderingManager.RtHook, bool>) (x => x.Target == rt)))
      {
        if (!Enumerable.Any<TargetRenderingManager.RtHook>((IEnumerable<TargetRenderingManager.RtHook>) this.renderTargetsToHook, (Func<TargetRenderingManager.RtHook, bool>) (x =>
        {
          if (x.Target == rt)
            return x.DrawOrder == drawOrder;
          else
            return false;
        })))
          throw new InvalidOperationException("Tried to hook already-hooked RT, but with different draw order");
      }
      else if (this.currentlyHookedRtIndex != -1)
      {
        this.nextFrameHooks += (Action) (() =>
        {
          this.renderTargetsToHook.Add(new TargetRenderingManager.RtHook()
          {
            DrawOrder = drawOrder,
            Target = rt
          });
          this.renderTargetsToHook.Sort((Comparison<TargetRenderingManager.RtHook>) ((a, b) => a.DrawOrder.CompareTo(b.DrawOrder)));
        });
      }
      else
      {
        this.renderTargetsToHook.Add(new TargetRenderingManager.RtHook()
        {
          DrawOrder = drawOrder,
          Target = rt
        });
        this.renderTargetsToHook.Sort((Comparison<TargetRenderingManager.RtHook>) ((a, b) => a.DrawOrder.CompareTo(b.DrawOrder)));
      }
    }

    public void UnscheduleHook(RenderTarget2D rt)
    {
      this.renderTargetsToHook.RemoveAll((Predicate<TargetRenderingManager.RtHook>) (x => x.Target == rt));
    }

    public void Resolve(RenderTarget2D rt, bool reschedule)
    {
      if (this.currentlyHookedRtIndex == -1)
        throw new InvalidOperationException("No render target hooked right now!");
      if (this.renderTargetsToHook[this.currentlyHookedRtIndex].Target != rt)
        throw new InvalidOperationException("Not the right render target hooked, can't resolve!");
      if (!reschedule)
        this.UnscheduleHook(rt);
      else
        ++this.currentlyHookedRtIndex;
      if (this.currentlyHookedRtIndex == this.renderTargetsToHook.Count)
      {
        this.graphicsDevice.SetRenderTarget((RenderTarget2D) null);
        this.currentlyHookedRtIndex = -1;
      }
      else
        this.graphicsDevice.SetRenderTarget(this.renderTargetsToHook[this.currentlyHookedRtIndex].Target);
    }

    public bool IsHooked(RenderTarget2D rt)
    {
      if (this.currentlyHookedRtIndex == -1)
        return false;
      else
        return this.renderTargetsToHook[this.currentlyHookedRtIndex].Target == rt;
    }

    public void OnPreDraw(GameTime gameTime)
    {
      this.PreDraw(gameTime);
    }

    public void OnRtPrepare()
    {
      if (this.nextFrameHooks != null)
      {
        this.nextFrameHooks();
        this.nextFrameHooks = (Action) null;
      }
      if (this.renderTargetsToHook.Count == 0)
        return;
      this.graphicsDevice.SetRenderTarget(this.renderTargetsToHook[this.currentlyHookedRtIndex = 0].Target);
    }

    public override void Initialize()
    {
      this.graphicsDevice = this.GraphicsDeviceService.GraphicsDevice;
      this.graphicsDevice.DeviceReset += (EventHandler<EventArgs>) ((_, __) => this.RecreateTargets());
      this.basicPostEffect = new BasicPostEffect();
      this.fullWhite = this.CMProvider.Global.Load<Texture2D>("Other Textures/FullWhite");
    }

    private void RecreateTargets()
    {
      List<Action<RenderTarget2D>> list = new List<Action<RenderTarget2D>>();
      foreach (RenderTargetHandle renderTargetHandle in this.fullscreenRTs)
      {
        list.Clear();
        foreach (TargetRenderingManager.RtHook rtHook in this.renderTargetsToHook)
        {
          if (renderTargetHandle.Target == rtHook.Target)
          {
            TargetRenderingManager.RtHook _ = rtHook;
            list.Add((Action<RenderTarget2D>) (t => _.Target = t));
          }
        }
        renderTargetHandle.Target.Dispose();
        renderTargetHandle.Target = this.CreateFullscreenTarget();
        foreach (Action<RenderTarget2D> action in list)
          action(renderTargetHandle.Target);
      }
    }

    public RenderTargetHandle TakeTarget()
    {
      RenderTargetHandle renderTargetHandle1 = (RenderTargetHandle) null;
      foreach (RenderTargetHandle renderTargetHandle2 in this.fullscreenRTs)
      {
        if (!renderTargetHandle2.Locked)
        {
          renderTargetHandle1 = renderTargetHandle2;
          break;
        }
      }
      if (renderTargetHandle1 == null)
        this.fullscreenRTs.Add(renderTargetHandle1 = new RenderTargetHandle()
        {
          Target = this.CreateFullscreenTarget()
        });
      renderTargetHandle1.Locked = true;
      return renderTargetHandle1;
    }

    private RenderTarget2D CreateFullscreenTarget()
    {
      SettingsManager.SetupViewport(this.Game.GraphicsDevice, false);
      return new RenderTarget2D(this.graphicsDevice, this.graphicsDevice.Viewport.Width, this.graphicsDevice.Viewport.Height, false, this.graphicsDevice.PresentationParameters.BackBufferFormat, this.graphicsDevice.PresentationParameters.DepthStencilFormat, this.graphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PlatformContents);
    }

    public void ReturnTarget(RenderTargetHandle handle)
    {
      if (handle == null)
        return;
      if (this.IsHooked(handle.Target))
        this.Resolve(handle.Target, false);
      else
        this.UnscheduleHook(handle.Target);
      handle.Locked = false;
    }

    public void DrawFullscreen(Color color)
    {
      this.DrawFullscreen((BaseEffect) this.basicPostEffect, (Texture) this.fullWhite, new Matrix?(), color);
    }

    public void DrawFullscreen(Texture texture)
    {
      this.DrawFullscreen((BaseEffect) this.basicPostEffect, texture, new Matrix?(), Color.White);
    }

    public void DrawFullscreen(Texture texture, Color color)
    {
      this.DrawFullscreen((BaseEffect) this.basicPostEffect, texture, new Matrix?(), color);
    }

    public void DrawFullscreen(Texture texture, Matrix textureMatrix)
    {
      this.DrawFullscreen((BaseEffect) this.basicPostEffect, texture, new Matrix?(textureMatrix), Color.White);
    }

    public void DrawFullscreen(Texture texture, Matrix textureMatrix, Color color)
    {
      this.DrawFullscreen((BaseEffect) this.basicPostEffect, texture, new Matrix?(textureMatrix), color);
    }

    public void DrawFullscreen(BaseEffect effect)
    {
      this.DrawFullscreen(effect, (Texture) null, new Matrix?(), Color.White);
    }

    public void DrawFullscreen(BaseEffect effect, Color color)
    {
      this.DrawFullscreen(effect, (Texture) this.fullWhite, new Matrix?(), color);
    }

    public void DrawFullscreen(BaseEffect effect, Texture texture)
    {
      this.DrawFullscreen(effect, texture, new Matrix?(), Color.White);
    }

    public void DrawFullscreen(BaseEffect effect, Texture texture, Matrix? textureMatrix)
    {
      this.DrawFullscreen(effect, texture, textureMatrix, Color.White);
    }

    public void DrawFullscreen(BaseEffect effect, Texture texture, Matrix? textureMatrix, Color color)
    {
      bool flag = effect.IgnoreCache;
      effect.IgnoreCache = true;
      if (texture != null)
        this.fullscreenPlane.Texture.Set(texture);
      if (textureMatrix.HasValue)
        this.fullscreenPlane.TextureMatrix.Set(textureMatrix.Value);
      if (color != Color.White)
      {
        this.fullscreenPlane.Material.Diffuse = color.ToVector3();
        this.fullscreenPlane.Material.Opacity = (float) color.A / (float) byte.MaxValue;
      }
      this.fullscreenPlane.Effect = effect;
      this.fullscreenPlane.Draw();
      if (color != Color.White)
      {
        this.fullscreenPlane.Material.Diffuse = Vector3.One;
        this.fullscreenPlane.Material.Opacity = 1f;
      }
      if (texture != null)
        this.fullscreenPlane.Texture.Set((Texture) null);
      if (textureMatrix.HasValue)
        this.fullscreenPlane.TextureMatrix.Set(Matrix.Identity);
      effect.IgnoreCache = flag;
    }

    private class RtHook
    {
      public int DrawOrder;
      public RenderTarget2D Target;
    }
  }
}
