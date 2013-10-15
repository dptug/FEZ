// Type: Microsoft.Xna.Framework.Content.Int64Reader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Content
{
  internal class Int64Reader : ContentTypeReader<long>
  {
    internal Int64Reader()
    {
    }

    protected internal override long Read(ContentReader input, long existingInstance)
    {
      return input.ReadInt64();
    }
  }
}
