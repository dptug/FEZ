// Type: Microsoft.Xna.Framework.Content.EnumReader`1
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
      return input.ReadObject<T>(this.elementReader);
    }
  }
}
