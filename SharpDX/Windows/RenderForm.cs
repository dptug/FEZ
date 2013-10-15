// Type: SharpDX.Windows.RenderForm
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using SharpDX.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SharpDX.Windows
{
  public class RenderForm : Form
  {
    private const int WM_SIZE = 5;
    private const int SIZE_RESTORED = 0;
    private const int SIZE_MINIMIZED = 1;
    private const int SIZE_MAXIMIZED = 2;
    private const int SIZE_MAXSHOW = 3;
    private const int SIZE_MAXHIDE = 4;
    private const int WM_ACTIVATEAPP = 28;
    private const int WM_POWERBROADCAST = 536;
    private const int WM_MENUCHAR = 288;
    private const int WM_SYSCOMMAND = 274;
    private const uint PBT_APMRESUMESUSPEND = 7U;
    private const uint PBT_APMQUERYSUSPEND = 0U;
    private const int SC_MONITORPOWER = 61808;
    private const int SC_SCREENSAVE = 61760;
    private const int MNC_CLOSE = 1;
    private Size cachedSize;
    private bool minimized;
    private bool sizeMove;
    private bool isBackgroundFirstDraw;

    public event EventHandler<EventArgs> AppActivated;

    public event EventHandler<EventArgs> AppDeactivated;

    public event EventHandler<EventArgs> MonitorChanged;

    public event EventHandler<EventArgs> PauseRendering;

    public event EventHandler<EventArgs> ResumeRendering;

    public event EventHandler<CancelEventArgs> Screensaver;

    public event EventHandler<EventArgs> SystemResume;

    public event EventHandler<EventArgs> SystemSuspend;

    public event EventHandler<EventArgs> UserResized;

    public RenderForm()
      : this("SharpDX")
    {
    }

    public RenderForm(string text)
    {
      this.Text = text;
      this.ClientSize = new Size(800, 600);
      this.MinimumSize = new Size(200, 200);
      this.ResizeRedraw = true;
      this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
      this.Icon = Resources.logo;
    }

    protected override void OnResizeBegin(EventArgs e)
    {
      base.OnResizeBegin(e);
      this.sizeMove = true;
      this.cachedSize = this.Size;
      this.OnPauseRendering(e);
    }

    protected override void OnResizeEnd(EventArgs e)
    {
      base.OnResizeEnd(e);
      if (this.sizeMove && this.cachedSize != this.Size)
      {
        this.OnUserResized(e);
        this.UpdateScreen();
      }
      this.sizeMove = false;
      this.OnResumeRendering(e);
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      this.cachedSize = this.Size;
      this.UpdateScreen();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this.isBackgroundFirstDraw)
        return;
      base.OnPaintBackground(e);
      this.isBackgroundFirstDraw = true;
    }

    private void OnPauseRendering(EventArgs e)
    {
      if (this.PauseRendering == null)
        return;
      this.PauseRendering((object) this, e);
    }

    private void OnResumeRendering(EventArgs e)
    {
      if (this.ResumeRendering == null)
        return;
      this.ResumeRendering((object) this, e);
    }

    private void OnUserResized(EventArgs e)
    {
      if (this.UserResized == null)
        return;
      this.UserResized((object) this, e);
    }

    private void OnMonitorChanged(EventArgs e)
    {
      if (this.MonitorChanged == null)
        return;
      this.MonitorChanged((object) this, e);
    }

    private void OnAppActivated(EventArgs e)
    {
      if (this.AppActivated == null)
        return;
      this.AppActivated((object) this, e);
    }

    private void OnAppDeactivated(EventArgs e)
    {
      if (this.AppDeactivated == null)
        return;
      this.AppDeactivated((object) this, e);
    }

    private void OnSystemSuspend(EventArgs e)
    {
      if (this.SystemSuspend == null)
        return;
      this.SystemSuspend((object) this, e);
    }

    private void OnSystemResume(EventArgs e)
    {
      if (this.SystemResume == null)
        return;
      this.SystemResume((object) this, e);
    }

    private void OnScreensaver(CancelEventArgs e)
    {
      if (this.Screensaver == null)
        return;
      this.Screensaver((object) this, e);
    }

    protected override void WndProc(ref Message m)
    {
      long num = m.WParam.ToInt64();
      switch (m.Msg)
      {
        case 274:
          switch (num & 65520L)
          {
            case 61808L:
            case 61760L:
              CancelEventArgs e = new CancelEventArgs();
              this.OnScreensaver(e);
              if (e.Cancel)
              {
                m.Result = IntPtr.Zero;
                return;
              }
              else
                break;
          }
        case 288:
          m.Result = new IntPtr(65536);
          return;
        case 536:
          if (num == 0L)
          {
            this.OnSystemSuspend(EventArgs.Empty);
            m.Result = new IntPtr(1);
            return;
          }
          else if (num == 7L)
          {
            this.OnSystemResume(EventArgs.Empty);
            m.Result = new IntPtr(1);
            return;
          }
          else
            break;
        case 5:
          if (num == 1L)
          {
            this.minimized = true;
            this.OnPauseRendering(EventArgs.Empty);
            break;
          }
          else
          {
            SharpDX.Rectangle lpRect;
            Win32Native.GetClientRect(m.HWnd, out lpRect);
            if (lpRect.Bottom - lpRect.Top != 0)
            {
              if (num == 2L)
              {
                if (this.minimized)
                  this.OnResumeRendering(EventArgs.Empty);
                this.minimized = false;
                this.OnUserResized(EventArgs.Empty);
                this.UpdateScreen();
                break;
              }
              else if (num == 0L)
              {
                if (this.minimized)
                  this.OnResumeRendering(EventArgs.Empty);
                this.minimized = false;
                if (!this.sizeMove && this.Size != this.cachedSize)
                {
                  this.OnUserResized(EventArgs.Empty);
                  this.UpdateScreen();
                  this.cachedSize = this.Size;
                  break;
                }
                else
                  break;
              }
              else
                break;
            }
            else
              break;
          }
        case 28:
          if (num != 0L)
          {
            this.OnAppActivated(EventArgs.Empty);
            break;
          }
          else
          {
            this.OnAppDeactivated(EventArgs.Empty);
            break;
          }
      }
      base.WndProc(ref m);
    }

    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == (Keys.Menu | Keys.Alt))
        return true;
      else
        return base.ProcessDialogKey(keyData);
    }

    private void UpdateScreen()
    {
    }
  }
}
