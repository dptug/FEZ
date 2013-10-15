// Type: FezGame.Tools.SpaceDivider
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezGame.Tools
{
  internal class SpaceDivider
  {
    public static List<SpaceDivider.DividedCell> Split(int count)
    {
      List<SpaceDivider.DividedCell> list = new List<SpaceDivider.DividedCell>()
      {
        new SpaceDivider.DividedCell(0, 0, 16, 16)
      };
      for (int index = 0; index < count - 1; ++index)
      {
        int num1 = 4;
        int num2 = 0;
        SpaceDivider.DividedCell dividedCell;
        do
        {
          dividedCell = RandomHelper.InList<SpaceDivider.DividedCell>(list);
          if (num2++ <= 100)
            num1 = (double) ((float) dividedCell.Bottom + (float) dividedCell.Height / 2f) < 6.0 ? 2 : 4;
          else
            break;
        }
        while (dividedCell.Width <= num1 && dividedCell.Height <= num1);
        if (num2 <= 100)
        {
          list.Remove(dividedCell);
          if (RandomHelper.Probability(0.5) && dividedCell.Height != num1 || dividedCell.Width == num1)
          {
            int height = (int) Math.Round((double) dividedCell.Height / 2.0);
            list.Add(new SpaceDivider.DividedCell(dividedCell.Left, dividedCell.Bottom + height, dividedCell.Width, dividedCell.Height - height));
            list.Add(new SpaceDivider.DividedCell(dividedCell.Left, dividedCell.Bottom, dividedCell.Width, height));
          }
          else
          {
            int width = (int) Math.Round((double) dividedCell.Width / 2.0);
            list.Add(new SpaceDivider.DividedCell(dividedCell.Left, dividedCell.Bottom, width, dividedCell.Height));
            list.Add(new SpaceDivider.DividedCell(dividedCell.Left + width, dividedCell.Bottom, dividedCell.Width - width, dividedCell.Height));
          }
        }
        else
          break;
      }
      return list;
    }

    public struct DividedCell
    {
      public int Left;
      public int Bottom;
      public int Width;
      public int Height;

      public int Top
      {
        get
        {
          return this.Bottom + this.Height;
        }
      }

      public int Right
      {
        get
        {
          return this.Left + this.Width;
        }
      }

      public Vector2 Center
      {
        get
        {
          return new Vector2((float) this.Left + (float) this.Width / 2f, (float) this.Bottom + (float) this.Height / 2f);
        }
      }

      public DividedCell(int left, int bottom, int width, int height)
      {
        this.Left = left;
        this.Bottom = bottom;
        this.Width = width;
        this.Height = height;
      }
    }
  }
}
