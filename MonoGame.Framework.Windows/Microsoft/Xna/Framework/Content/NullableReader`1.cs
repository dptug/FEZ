// Type: Microsoft.Xna.Framework.Content.NullableReader`1
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
