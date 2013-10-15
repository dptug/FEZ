// Type: Microsoft.Xna.Framework.Graphics.DisplayModeCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  public class DisplayModeCollection : IEnumerable<DisplayMode>, IEnumerable
  {
    private List<DisplayMode> modes;

    public IEnumerable<DisplayMode> this[SurfaceFormat format]
    {
      get
      {
        List<DisplayMode> list = new List<DisplayMode>();
        foreach (DisplayMode displayMode in this.modes)
        {
          if (displayMode.Format == format)
            list.Add(displayMode);
        }
        return (IEnumerable<DisplayMode>) list;
      }
    }

    public DisplayModeCollection(List<DisplayMode> setmodes)
    {
      this.modes = setmodes;
    }

    public IEnumerator<DisplayMode> GetEnumerator()
    {
      return (IEnumerator<DisplayMode>) this.modes.GetEnumerator();
    }

    public override int GetHashCode()
    {
      throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }
  }
}
