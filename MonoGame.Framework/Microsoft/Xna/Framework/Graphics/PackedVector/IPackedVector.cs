// Type: Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
  public interface IPackedVector
  {
    void PackFromVector4(Vector4 vector);

    Vector4 ToVector4();
  }
}
