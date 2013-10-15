// Type: Newtonsoft.Json.Utilities.WrapperDictionary
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
  internal class WrapperDictionary
  {
    private readonly Dictionary<string, Type> _wrapperTypes = new Dictionary<string, Type>();

    private static string GenerateKey(Type interfaceType, Type realObjectType)
    {
      return interfaceType.Name + "_" + realObjectType.Name;
    }

    public Type GetType(Type interfaceType, Type realObjectType)
    {
      string key = WrapperDictionary.GenerateKey(interfaceType, realObjectType);
      if (this._wrapperTypes.ContainsKey(key))
        return this._wrapperTypes[key];
      else
        return (Type) null;
    }

    public void SetType(Type interfaceType, Type realObjectType, Type wrapperType)
    {
      string key = WrapperDictionary.GenerateKey(interfaceType, realObjectType);
      if (this._wrapperTypes.ContainsKey(key))
        this._wrapperTypes[key] = wrapperType;
      else
        this._wrapperTypes.Add(key, wrapperType);
    }
  }
}
