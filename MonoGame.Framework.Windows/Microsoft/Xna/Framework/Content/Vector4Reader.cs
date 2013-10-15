// Type: Microsoft.Xna.Framework.Content.Vector4Reader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Content
{
  internal class Vector4Reader : ContentTypeReader<Vector4>
  {
    internal Vector4Reader()
    {
    }

    protected internal override Vector4 Read(ContentReader input, Vector4 existingInstance)
    {
      return input.ReadVector4();
    }
  }
}
