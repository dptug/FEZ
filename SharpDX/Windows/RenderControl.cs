// Type: SharpDX.Windows.RenderControl
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System.Drawing;
using System.Windows.Forms;

namespace SharpDX.Windows
{
  public class RenderControl : UserControl
  {
    private Font fontForDesignMode;

    public RenderControl()
    {
      this.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint, true);
      this.UpdateStyles();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (!this.DesignMode)
        return;
      base.OnPaintBackground(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      if (!this.DesignMode)
        return;
      if (this.fontForDesignMode == null)
        this.fontForDesignMode = new Font("Calibri", 24f, FontStyle.Regular);
      e.Graphics.Clear(Color.WhiteSmoke);
      string str = "SharpDX RenderControl";
      SizeF sizeF = e.Graphics.MeasureString(str, this.fontForDesignMode);
      e.Graphics.DrawString(str, this.fontForDesignMode, (Brush) new SolidBrush(Color.Black), (float) (((double) this.Width - (double) sizeF.Width) / 2.0), (float) (((double) this.Height - (double) sizeF.Height) / 2.0));
    }
  }
}
