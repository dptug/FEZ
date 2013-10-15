// Type: FezGame.Structure.MenuItem`1
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Tools;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Globalization;

namespace FezGame.Structure
{
  internal class MenuItem<T> : MenuItem
  {
    private static readonly TimeSpan HoverGrowDuration = TimeSpan.FromSeconds(0.1);
    private string text;
    public Func<T> SliderValueGetter;
    public Action<T, int> SliderValueSetter;

    public string Text
    {
      get
      {
        string str = (this.text == null ? "" : StaticText.GetString(this.text)) + (this.SuffixText == null ? "" : this.SuffixText());
        if (!this.UpperCase)
          return str;
        else
          return str.ToUpper(CultureInfo.InvariantCulture);
      }
      set
      {
        this.text = value;
      }
    }

    public Func<string> SuffixText { get; set; }

    public MenuLevel Parent { get; set; }

    public bool Hovered { get; set; }

    public bool UpperCase { get; set; }

    public Action Selected { get; set; }

    public Vector2 Size { get; set; }

    public bool Selectable { get; set; }

    public bool IsSlider { get; set; }

    public bool LocalizeSliderValue { get; set; }

    public string LocalizationTagFormat { get; set; }

    public bool Centered { get; set; }

    public bool Disabled { get; set; }

    public bool IsGamerCard { get; set; }

    public TimeSpan SinceHovered { get; set; }

    public bool Hidden { get; set; }

    public Rectangle HoverArea { get; set; }

    public float ActivityRatio
    {
      get
      {
        return FezMath.Saturate((float) this.SinceHovered.Ticks / (float) MenuItem<T>.HoverGrowDuration.Ticks);
      }
    }

    static MenuItem()
    {
    }

    public MenuItem()
    {
      this.Selectable = true;
      this.Centered = true;
    }

    public void Slide(int direction)
    {
      if (!this.IsSlider)
        return;
      this.SliderValueSetter(this.SliderValueGetter(), Math.Sign(direction));
    }

    public void ClampTimer()
    {
      if (this.SinceHovered.Ticks < 0L)
        this.SinceHovered = TimeSpan.Zero;
      if (!(this.SinceHovered > MenuItem<T>.HoverGrowDuration))
        return;
      this.SinceHovered = MenuItem<T>.HoverGrowDuration;
    }

    public void OnSelected()
    {
      this.Selected();
    }

    public override string ToString()
    {
      // ISSUE: unable to decompile the method.
    }
  }
}
