// Type: Microsoft.Xna.Framework.Graphics.VertexDeclarationCache`1
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
