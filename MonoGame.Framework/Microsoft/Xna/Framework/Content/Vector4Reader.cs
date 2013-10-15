// Type: Microsoft.Xna.Framework.Content.Vector4Reader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
