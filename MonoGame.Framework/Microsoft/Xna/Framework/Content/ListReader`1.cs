// Type: Microsoft.Xna.Framework.Content.ListReader`1
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Content
{
  public class ListReader<T> : ContentTypeReader<List<T>>
  {
    private ContentTypeReader elementReader;

    protected internal override void Initialize(ContentTypeReaderManager manager)
    {
      Type targetType = typeof (T);
      this.elementReader = manager.GetTypeReader(targetType);
    }

    protected internal override List<T> Read(ContentReader input, List<T> existingInstance)
    {
      int capacity = input.ReadInt32();
      List<T> list = existingInstance ?? new List<T>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        if (typeof (T).IsValueType)
        {
          list.Add(input.ReadObject<T>(this.elementReader));
        }
        else
        {
          int num = (int) input.ReadByte();
          list.Add(input.ReadObject<T>(input.TypeReaders[num - 1]));
        }
      }
      return list;
    }
  }
}
