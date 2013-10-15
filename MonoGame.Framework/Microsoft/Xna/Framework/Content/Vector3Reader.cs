// Type: Microsoft.Xna.Framework.Content.Vector3Reader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Content
{
  internal class Vector3Reader : ContentTypeReader<Vector3>
  {
    internal Vector3Reader()
    {
    }

    protected internal override Vector3 Read(ContentReader input, Vector3 existingInstance)
    {
      return input.ReadVector3();
    }
  }
}
