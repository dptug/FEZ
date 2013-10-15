// Type: Microsoft.Xna.Framework.Content.ListReader`1
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
      int num1 = input.ReadInt32();
      List<T> list = existingInstance ?? new List<T>();
      for (int index = 0; index < num1; ++index)
      {
        if (typeof (T).IsValueType)
        {
          list.Add(input.ReadObject<T>(this.elementReader));
        }
        else
        {
          int num2 = (int) input.ReadByte();
          list.Add(input.ReadObject<T>(input.TypeReaders[num2 - 1]));
        }
      }
      return list;
    }
  }
}
