// Type: Microsoft.Xna.Framework.Content.Vector3Reader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
