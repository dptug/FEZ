// Type: Microsoft.Xna.Framework.GameServiceContainer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
  public class GameServiceContainer : IServiceProvider
  {
    private Dictionary<Type, object> services;

    public GameServiceContainer()
    {
      this.services = new Dictionary<Type, object>();
    }

    public void AddService(Type type, object provider)
    {
      if (type == (Type) null)
        throw new ArgumentNullException("type");
      if (provider == null)
        throw new ArgumentNullException("provider");
      if (!type.IsAssignableFrom(provider.GetType()))
        throw new ArgumentException("The provider does not match the specified service type!");
      this.services.Add(type, provider);
    }

    public object GetService(Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException("type");
      object obj;
      if (this.services.TryGetValue(type, out obj))
        return obj;
      else
        return (object) null;
    }

    public void RemoveService(Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException("type");
      this.services.Remove(type);
    }
  }
}
