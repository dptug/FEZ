// Type: SharpDX.IServiceRegistry
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public interface IServiceRegistry : IServiceProvider
  {
    event EventHandler<ServiceEventArgs> ServiceAdded;

    event EventHandler<ServiceEventArgs> ServiceRemoved;

    void AddService(Type type, object provider);

    void RemoveService(Type type);
  }
}
