// Type: Microsoft.Xna.Framework.Content.SByteReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Content
{
  internal class SByteReader : ContentTypeReader<sbyte>
  {
    internal SByteReader()
    {
    }

    protected internal override sbyte Read(ContentReader input, sbyte existingInstance)
    {
      return input.ReadSByte();
    }
  }
}
