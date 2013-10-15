// Type: Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
  public interface IPackedVector
  {
    void PackFromVector4(Vector4 vector);

    Vector4 ToVector4();
  }
}
