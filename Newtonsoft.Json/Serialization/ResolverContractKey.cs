// Type: Newtonsoft.Json.Serialization.ResolverContractKey
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Serialization
{
  internal struct ResolverContractKey : IEquatable<ResolverContractKey>
  {
    private readonly Type _resolverType;
    private readonly Type _contractType;

    public ResolverContractKey(Type resolverType, Type contractType)
    {
      this._resolverType = resolverType;
      this._contractType = contractType;
    }

    public override int GetHashCode()
    {
      return this._resolverType.GetHashCode() ^ this._contractType.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (!(obj is ResolverContractKey))
        return false;
      else
        return this.Equals((ResolverContractKey) obj);
    }

    public bool Equals(ResolverContractKey other)
    {
      if (this._resolverType == other._resolverType)
        return this._contractType == other._contractType;
      else
        return false;
    }
  }
}
