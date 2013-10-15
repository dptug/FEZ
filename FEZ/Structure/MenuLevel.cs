// Type: FezGame.Structure.MenuLevel
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Components;
using FezEngine.Services;
using FezGame.Components;
using FezGame.Tools;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Structure
{
  internal class MenuLevel
  {
    public readonly List<MenuItem> Items = new List<MenuItem>();
    private string titleString;
    public MenuLevel Parent;
    public bool IsDynamic;
    public bool Oversized;
    public Action OnScrollUp;
    public Action OnScrollDown;
    public Action OnClose;
    public Action OnReset;
    private bool initialized;
    public Action<SpriteBatch, SpriteFont, GlyphTextRenderer, float> OnPostDraw;
    private string xButtonString;
    private string aButtonString;
    private string bButtonString;
    public Action XButtonAction;
    public Action AButtonAction;
    public bool AButtonStarts;

    public string Title
    {
      get
      {
        if (this.titleString != null)
          return StaticText.GetString(this.titleString);
        else
          return (string) null;
      }
      set
      {
        this.titleString = value;
      }
    }

    public MenuItem SelectedItem
    {
      get
      {
        if (this.SelectedIndex != -1 && this.Items.Count > this.SelectedIndex)
          return this.Items[this.SelectedIndex];
        else
          return (MenuItem) null;
      }
    }

    public int SelectedIndex { get; set; }

    public bool ForceCancel { get; set; }

    public bool TrapInput { get; set; }

    public string XButtonString
    {
      get
      {
        return this.xButtonString;
      }
      set
      {
        this.xButtonString = value;
      }
    }

    public virtual string AButtonString
    {
      get
      {
        if (this.aButtonString != null)
          return StaticText.GetString(this.aButtonString);
        else
          return (string) null;
      }
      set
      {
        this.aButtonString = value;
      }
    }

    public string BButtonString
    {
      get
      {
        if (this.bButtonString != null)
          return StaticText.GetString(this.bButtonString);
        else
          return (string) null;
      }
      set
      {
        this.bButtonString = value;
      }
    }

    public IContentManagerProvider CMProvider { protected get; set; }

    public virtual void Initialize()
    {
      this.initialized = true;
    }

    public virtual void Dispose()
    {
    }

    public bool MoveDown()
    {
      MenuItem selectedItem = this.SelectedItem;
      if (this.SelectedItem != null)
        this.SelectedItem.Hovered = false;
      if (this.Items.Count == 0 || Enumerable.All<MenuItem>((IEnumerable<MenuItem>) this.Items, (Func<MenuItem, bool>) (x =>
      {
        if (x.Selectable)
          return this.SelectedItem.Hidden;
        else
          return true;
      })))
      {
        this.SelectedIndex = -1;
      }
      else
      {
        do
        {
          ++this.SelectedIndex;
          if (this.SelectedIndex == this.Items.Count)
          {
            if (this.OnScrollDown != null)
            {
              this.OnScrollDown();
              if (this.SelectedIndex == this.Items.Count)
              {
                do
                {
                  --this.SelectedIndex;
                }
                while (!this.SelectedItem.Selectable || this.SelectedItem.Hidden);
                break;
              }
            }
            else
              this.SelectedIndex = 0;
          }
        }
        while (!this.SelectedItem.Selectable || this.SelectedItem.Hidden);
        if (selectedItem != this.SelectedItem)
          this.SelectedItem.SinceHovered = TimeSpan.Zero;
        this.SelectedItem.Hovered = true;
      }
      return selectedItem != this.SelectedItem;
    }

    public bool MoveUp()
    {
      MenuItem selectedItem = this.SelectedItem;
      if (this.SelectedItem != null)
        this.SelectedItem.Hovered = false;
      if (this.Items.Count == 0 || Enumerable.All<MenuItem>((IEnumerable<MenuItem>) this.Items, (Func<MenuItem, bool>) (x =>
      {
        if (x.Selectable)
          return this.SelectedItem.Hidden;
        else
          return true;
      })))
      {
        this.SelectedIndex = -1;
      }
      else
      {
        do
        {
          --this.SelectedIndex;
          if (this.SelectedIndex == -1)
          {
            if (this.OnScrollUp != null)
            {
              this.OnScrollUp();
              if (this.SelectedIndex == -1)
                ++this.SelectedIndex;
            }
            else
              this.SelectedIndex = this.Items.Count - 1;
          }
        }
        while (!this.SelectedItem.Selectable || this.SelectedItem.Hidden);
        if (selectedItem != this.SelectedItem)
          this.SelectedItem.SinceHovered = TimeSpan.Zero;
        this.SelectedItem.Hovered = true;
      }
      return selectedItem != this.SelectedItem;
    }

    public virtual void Update(TimeSpan elapsed)
    {
      foreach (MenuItem menuItem in this.Items)
      {
        if (menuItem.Hovered)
          menuItem.SinceHovered += elapsed;
        else
          menuItem.SinceHovered -= elapsed;
        menuItem.ClampTimer();
      }
    }

    public virtual void PostDraw(SpriteBatch batch, SpriteFont font, GlyphTextRenderer tr, float alpha)
    {
      if (this.OnPostDraw == null)
        return;
      this.OnPostDraw(batch, font, tr, alpha);
    }

    public void Select()
    {
      if (this.SelectedItem == null || !this.SelectedItem.Selectable)
        return;
      this.SelectedItem.OnSelected();
    }

    public MenuItem AddItem(string text, int at = -1)
    {
      return this.AddItem(text, MenuBase.SliderAction, false, at);
    }

    public MenuItem AddItem(string text, Action onSelect, int at = -1)
    {
      return this.AddItem(text, onSelect, false, at);
    }

    public MenuItem AddItem(string text, Action onSelect, bool defaultItem, int at = -1)
    {
      return (MenuItem) this.AddItem<float>(text, onSelect, defaultItem, new Func<float>(Util.NullFunc<float>), new Action<float, int>(Util.NullAction<float, int>), at);
    }

    public MenuItem<T> AddItem<T>(string text, Action onSelect, bool defaultItem, Func<T> sliderValueGetter, Action<T, int> sliderValueSetter, int at = -1)
    {
      MenuItem<T> menuItem1 = new MenuItem<T>()
      {
        Parent = this,
        Text = text,
        Selected = onSelect,
        IsSlider = sliderValueGetter != new Func<T>(Util.NullFunc<T>),
        SliderValueGetter = sliderValueGetter,
        SliderValueSetter = sliderValueSetter
      };
      if (!this.initialized && (this.Items.Count == 0 || defaultItem))
      {
        foreach (MenuItem menuItem2 in this.Items)
          menuItem2.Hovered = false;
        menuItem1.Hovered = true;
        this.SelectedIndex = this.Items.Count;
      }
      if (onSelect == new Action(Util.NullAction))
        menuItem1.Hovered = menuItem1.Selectable = false;
      if (at == -1)
        this.Items.Add((MenuItem) menuItem1);
      else
        this.Items.Insert(at, (MenuItem) menuItem1);
      return menuItem1;
    }

    public virtual void Reset()
    {
      if (this.OnReset == null)
        return;
      this.OnReset();
    }
  }
}
