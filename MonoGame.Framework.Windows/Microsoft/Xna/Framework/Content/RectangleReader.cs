// Type: Microsoft.Xna.Framework.Content.RectangleReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Content
{
  internal class RectangleReader : ContentTypeReader<Rectangle>
  {
    internal RectangleReader()
    {
    }

    protected internal override Rectangle Read(ContentReader input, Rectangle existingInstance)
    {
      return new Rectangle(input.ReadInt32(), input.ReadInt32(), input.ReadInt32(), input.ReadInt32());
    }
  }
}
