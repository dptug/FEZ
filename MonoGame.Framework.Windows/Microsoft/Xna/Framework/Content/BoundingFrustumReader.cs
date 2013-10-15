// Type: Microsoft.Xna.Framework.Content.BoundingFrustumReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Content
{
  internal class BoundingFrustumReader : ContentTypeReader<BoundingFrustum>
  {
    internal BoundingFrustumReader()
    {
    }

    protected internal override BoundingFrustum Read(ContentReader input, BoundingFrustum existingInstance)
    {
      return new BoundingFrustum(input.ReadMatrix());
    }
  }
}
