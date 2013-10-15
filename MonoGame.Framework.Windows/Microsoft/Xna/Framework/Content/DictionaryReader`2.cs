// Type: Microsoft.Xna.Framework.Content.DictionaryReader`2
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
      int num1 = input.ReadInt32();
      Dictionary<TKey, TValue> dictionary = existingInstance ?? new Dictionary<TKey, TValue>();
      for (int index = 0; index < num1; ++index)
      {
        TKey key;
        if (this.keyType.IsValueType)
        {
          key = input.ReadObject<TKey>(this.keyReader);
        }
        else
        {
          int num2 = (int) input.ReadByte();
          key = input.ReadObject<TKey>(input.TypeReaders[num2 - 1]);
        }
        TValue obj;
        if (this.valueType.IsValueType)
        {
          obj = input.ReadObject<TValue>(this.valueReader);
        }
        else
        {
          int num2 = (int) input.ReadByte();
          obj = input.ReadObject<TValue>(input.TypeReaders[num2 - 1]);
        }
        dictionary.Add(key, obj);
      }
      return dictionary;
    }
  }
}
