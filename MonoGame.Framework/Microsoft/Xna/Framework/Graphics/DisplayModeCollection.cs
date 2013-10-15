// Type: Microsoft.Xna.Framework.Graphics.DisplayModeCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
