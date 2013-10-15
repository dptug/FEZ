// Type: Microsoft.Xna.Framework.Content.EnumReader`1
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Content
{
  public class EnumReader<T> : ContentTypeReader<T>
  {
    private ContentTypeReader elementReader;

    protected internal override void Initialize(ContentTypeReaderManager manager)
    {
      Type underlyingType = Enum.GetUnderlyingType(typeof (T));
      this.elementReader = manager.GetTypeReader(underlyingType);
    }

    protected internal override T Read(ContentReader input, T existingInstance)
    {
      return input.ReadRawObject<T>(this.elementReader);
    }
  }
}
