// Type: Microsoft.Xna.Framework.Content.NullableReader`1
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Content
{
  internal class NullableReader<T> : ContentTypeReader<T?> where T : struct
  {
    private ContentTypeReader elementReader;

    internal NullableReader()
    {
    }

    protected internal override void Initialize(ContentTypeReaderManager manager)
    {
      Type targetType = typeof (T);
      this.elementReader = manager.GetTypeReader(targetType);
    }

    protected internal override T? Read(ContentReader input, T? existingInstance)
    {
      if (input.ReadBoolean())
        return new T?(input.ReadObject<T>(this.elementReader));
      else
        return new T?();
    }
  }
}
