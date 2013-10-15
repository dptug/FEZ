// Type: Microsoft.Xna.Framework.Content.RayReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Content
{
  internal class RayReader : ContentTypeReader<Ray>
  {
    internal RayReader()
    {
    }

    protected internal override Ray Read(ContentReader input, Ray existingInstance)
    {
      return new Ray(input.ReadVector3(), input.ReadVector3());
    }
  }
}
