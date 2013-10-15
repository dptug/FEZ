// Type: Newtonsoft.Json.Serialization.DefaultSerializationBinder
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  public class DefaultSerializationBinder : SerializationBinder
  {
    internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();
    private readonly ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type> _typeCache = new ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type>(new Func<DefaultSerializationBinder.TypeNameKey, Type>(DefaultSerializationBinder.GetTypeFromTypeNameKey));

    static DefaultSerializationBinder()
    {
    }

    private static Type GetTypeFromTypeNameKey(DefaultSerializationBinder.TypeNameKey typeNameKey)
    {
      string partialName = typeNameKey.AssemblyName;
      string str = typeNameKey.TypeName;
      if (partialName == null)
        return Type.GetType(str);
      Assembly assembly = Assembly.LoadWithPartialName(partialName);
      if (assembly == null)
        throw new JsonSerializationException(StringUtils.FormatWith("Could not load assembly '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) partialName));
      Type type = assembly.GetType(str);
      if (type == null)
        throw new JsonSerializationException(StringUtils.FormatWith("Could not find type '{0}' in assembly '{1}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) str, (object) assembly.FullName));
      else
        return type;
    }

    public override Type BindToType(string assemblyName, string typeName)
    {
      return this._typeCache.Get(new DefaultSerializationBinder.TypeNameKey(assemblyName, typeName));
    }

    internal struct TypeNameKey : IEquatable<DefaultSerializationBinder.TypeNameKey>
    {
      internal readonly string AssemblyName;
      internal readonly string TypeName;

      public TypeNameKey(string assemblyName, string typeName)
      {
        this.AssemblyName = assemblyName;
        this.TypeName = typeName;
      }

      public override int GetHashCode()
      {
        return (this.AssemblyName != null ? this.AssemblyName.GetHashCode() : 0) ^ (this.TypeName != null ? this.TypeName.GetHashCode() : 0);
      }

      public override bool Equals(object obj)
      {
        if (!(obj is DefaultSerializationBinder.TypeNameKey))
          return false;
        else
          return this.Equals((DefaultSerializationBinder.TypeNameKey) obj);
      }

      public bool Equals(DefaultSerializationBinder.TypeNameKey other)
      {
        if (this.AssemblyName == other.AssemblyName)
          return this.TypeName == other.TypeName;
        else
          return false;
      }
    }
  }
}
