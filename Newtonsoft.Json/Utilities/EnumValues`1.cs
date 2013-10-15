// Type: Newtonsoft.Json.Utilities.EnumValues`1
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System.Collections.ObjectModel;

namespace Newtonsoft.Json.Utilities
{
  internal class EnumValues<T> : KeyedCollection<string, EnumValue<T>> where T : struct
  {
    protected override string GetKeyForItem(EnumValue<T> item)
    {
      return item.Name;
    }
  }
}
