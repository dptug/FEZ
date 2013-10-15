// Type: FezGame.Structure.MenuItem
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Microsoft.Xna.Framework;
using System;

namespace FezGame.Structure
{
  internal interface MenuItem
  {
    string Text { get; set; }

    Func<string> SuffixText { get; set; }

    MenuLevel Parent { get; set; }

    bool Hovered { get; set; }

    Action Selected { get; set; }

    Vector2 Size { get; set; }

    bool Selectable { get; set; }

    bool IsSlider { get; set; }

    bool UpperCase { get; set; }

    bool Centered { get; set; }

    bool Disabled { get; set; }

    bool IsGamerCard { get; set; }

    bool Hidden { get; set; }

    TimeSpan SinceHovered { get; set; }

    Rectangle HoverArea { get; set; }

    bool LocalizeSliderValue { get; set; }

    string LocalizationTagFormat { get; set; }

    float ActivityRatio { get; }

    void OnSelected();

    void ClampTimer();

    void Slide(int direction);
  }
}
