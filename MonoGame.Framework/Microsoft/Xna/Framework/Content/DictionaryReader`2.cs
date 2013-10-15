// Type: Microsoft.Xna.Framework.Content.DictionaryReader`2
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Content
{
  public class DictionaryReader<TKey, TValue> : ContentTypeReader<Dictionary<TKey, TValue>>
  {
    private ContentTypeReader keyReader;
    private ContentTypeReader valueReader;
    private Type keyType;
    private Type valueType;

    protected internal override void Initialize(ContentTypeReaderManager manager)
    {
      this.keyType = typeof (TKey);
      this.valueType = typeof (TValue);
      this.keyReader = manager.GetTypeReader(this.keyType);
      this.valueReader = manager.GetTypeReader(this.valueType);
    }

    protected internal override Dictionary<TKey, TValue> Read(ContentReader input, Dictionary<TKey, TValue> existingInstance)
    {
      int capacity = input.ReadInt32();
      Dictionary<TKey, TValue> dictionary = existingInstance ?? new Dictionary<TKey, TValue>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        TKey key;
        if (this.keyType.IsValueType)
        {
          key = input.ReadObject<TKey>(this.keyReader);
        }
        else
        {
          int num = (int) input.ReadByte();
          key = input.ReadObject<TKey>(input.TypeReaders[num - 1]);
        }
        TValue obj;
        if (this.valueType.IsValueType)
        {
          obj = input.ReadObject<TValue>(this.valueReader);
        }
        else
        {
          int num = (int) input.ReadByte();
          obj = input.ReadObject<TValue>(input.TypeReaders[num - 1]);
        }
        dictionary.Add(key, obj);
      }
      return dictionary;
    }
  }
}
