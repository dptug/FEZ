// Type: Microsoft.Xna.Framework.Content.BoundingBoxReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Content
{
  internal class BoundingBoxReader : ContentTypeReader<BoundingBox>
  {
    protected internal override BoundingBox Read(ContentReader input, BoundingBox existingInstance)
    {
      return new BoundingBox(input.ReadVector3(), input.ReadVector3());
    }
  }
}
