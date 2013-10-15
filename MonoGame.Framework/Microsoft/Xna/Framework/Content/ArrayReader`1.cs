// Type: Microsoft.Xna.Framework.Content.ArrayReader`1
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Content
{
  public class ArrayReader<T> : ContentTypeReader<T[]>
  {
    private ContentTypeReader elementReader;

    protected internal override void Initialize(ContentTypeReaderManager manager)
    {
      Type targetType = typeof (T);
      this.elementReader = manager.GetTypeReader(targetType);
    }

    protected internal override T[] Read(ContentReader input, T[] existingInstance)
    {
      uint num1 = input.ReadUInt32();
      T[] objArray = existingInstance ?? new T[(IntPtr) num1];
      if (typeof (T).IsValueType)
      {
        for (uint index = 0U; index < num1; ++index)
          objArray[(IntPtr) index] = input.ReadObject<T>(this.elementReader);
      }
      else
      {
        for (uint index = 0U; index < num1; ++index)
        {
          int num2 = input.Read7BitEncodedInt();
          objArray[(IntPtr) index] = num2 > 0 ? input.ReadObject<T>(input.TypeReaders[num2 - 1]) : default (T);
        }
      }
      return objArray;
    }
  }
}
