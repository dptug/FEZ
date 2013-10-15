// Type: Microsoft.Xna.Framework.GameServiceContainer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
