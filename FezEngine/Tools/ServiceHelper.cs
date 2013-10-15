// Type: FezEngine.Tools.ServiceHelper
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FezEngine.Tools
{
  public static class ServiceHelper
  {
    private static readonly List<object> services = new List<object>();
    public static object Mutex = new object();
    public static bool IsFull;
    public static bool FirstLoadDone;

    public static Game Game { get; set; }

    static ServiceHelper()
    {
    }

    public static void Clear()
    {
      foreach (object obj in ServiceHelper.services)
      {
        foreach (Type type in obj.GetType().GetInterfaces())
          ServiceHelper.Game.Services.RemoveService(type);
        if (obj is IDisposable)
          (obj as IDisposable).Dispose();
      }
      ServiceHelper.services.Clear();
    }

    public static void AddComponent(IGameComponent component)
    {
      ServiceHelper.AddComponent(component, false);
    }

    public static void AddComponent(IGameComponent component, bool addServices)
    {
      lock (ServiceHelper.Mutex)
      {
        if (!addServices)
          ServiceHelper.InjectServices((object) component);
        ServiceHelper.Game.Components.Add(component);
        if (addServices)
          ServiceHelper.AddService((object) component);
        if (!TraceFlags.TraceContentLoad)
          return;
        Logger.Log("ServiceHelper", LogSeverity.Information, component.GetType().Name + " loaded");
      }
    }

    public static T Get<T>() where T : class
    {
      return ServiceHelper.Game.Services.GetService(typeof (T)) as T;
    }

    public static object Get(Type type)
    {
      return ServiceHelper.Game.Services.GetService(type);
    }

    public static void AddService(object service)
    {
      foreach (Type type in service.GetType().GetInterfaces())
      {
        if (type != typeof (IDisposable) && type != typeof (IUpdateable) && (type != typeof (IDrawable) && type != typeof (IGameComponent)) && (!type.Name.StartsWith("IComparable") && type.GetCustomAttributes(typeof (DisabledServiceAttribute), false).Length == 0))
          ServiceHelper.Game.Services.AddService(type, service);
      }
      ServiceHelper.services.Add(service);
    }

    public static void InitializeServices()
    {
      foreach (object componentOrService in ServiceHelper.services)
        ServiceHelper.InjectServices(componentOrService);
    }

    public static void InjectServices(object componentOrService)
    {
      Type type = componentOrService.GetType();
      do
      {
        foreach (PropertyInfo propInfo in ReflectionHelper.GetSettableProperties(type))
        {
          ServiceDependencyAttribute firstAttribute = ReflectionHelper.GetFirstAttribute<ServiceDependencyAttribute>(propInfo);
          if (firstAttribute != null)
          {
            Type propertyType = propInfo.PropertyType;
            object obj = ServiceHelper.Game == null ? (object) null : ServiceHelper.Game.Services.GetService(propertyType);
            if (obj == null)
            {
              if (!firstAttribute.Optional)
                throw new MissingServiceException(type, propertyType);
            }
            else
              propInfo.GetSetMethod(true).Invoke(componentOrService, new object[1]
              {
                obj
              });
          }
        }
        type = type.BaseType;
      }
      while (type != typeof (object));
    }

    public static void RemoveComponent<T>(T component) where T : IGameComponent
    {
      lock (ServiceHelper.Game)
      {
        lock (ServiceHelper.Mutex)
        {
          if ((object) component is IDisposable)
            ((object) component as IDisposable).Dispose();
          ServiceHelper.Game.Components.Remove((IGameComponent) component);
        }
      }
    }
  }
}
