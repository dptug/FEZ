// Type: Microsoft.Xna.Framework.Graphics.VertexDeclarationCache`1
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Graphics
{
  internal class VertexDeclarationCache<T> where T : struct, IVertexType
  {
    private static VertexDeclaration _cached;

    public static VertexDeclaration VertexDeclaration
    {
      get
      {
        if (VertexDeclarationCache<T>._cached == null)
          VertexDeclarationCache<T>._cached = VertexDeclaration.FromType(typeof (T));
        return VertexDeclarationCache<T>._cached;
      }
    }
  }
}
