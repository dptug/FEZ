// Type: Newtonsoft.Json.Utilities.DynamicWrapper
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;

namespace Newtonsoft.Json.Utilities
{
  internal static class DynamicWrapper
  {
    private static readonly object _lock = new object();
    private static readonly WrapperDictionary _wrapperDictionary = new WrapperDictionary();
    private static ModuleBuilder _moduleBuilder;

    private static ModuleBuilder ModuleBuilder
    {
      get
      {
        DynamicWrapper.Init();
        return DynamicWrapper._moduleBuilder;
      }
    }

    static DynamicWrapper()
    {
    }

    private static void Init()
    {
      if (DynamicWrapper._moduleBuilder != null)
        return;
      lock (DynamicWrapper._lock)
      {
        if (DynamicWrapper._moduleBuilder != null)
          return;
        DynamicWrapper._moduleBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Newtonsoft.Json.Dynamic")
        {
          KeyPair = new StrongNameKeyPair(DynamicWrapper.GetStrongKey())
        }, AssemblyBuilderAccess.Run).DefineDynamicModule("Newtonsoft.Json.DynamicModule", false);
      }
    }

    private static byte[] GetStrongKey()
    {
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Newtonsoft.Json.Dynamic.snk"))
      {
        if (manifestResourceStream == null)
          throw new MissingManifestResourceException("Should have Newtonsoft.Json.Dynamic.snk as an embedded resource.");
        int count = (int) manifestResourceStream.Length;
        byte[] buffer = new byte[count];
        manifestResourceStream.Read(buffer, 0, count);
        return buffer;
      }
    }

    public static Type GetWrapper(Type interfaceType, Type realObjectType)
    {
      Type wrapperType = DynamicWrapper._wrapperDictionary.GetType(interfaceType, realObjectType);
      if (wrapperType == null)
      {
        lock (DynamicWrapper._lock)
        {
          wrapperType = DynamicWrapper._wrapperDictionary.GetType(interfaceType, realObjectType);
          if (wrapperType == null)
          {
            wrapperType = DynamicWrapper.GenerateWrapperType(interfaceType, realObjectType);
            DynamicWrapper._wrapperDictionary.SetType(interfaceType, realObjectType, wrapperType);
          }
        }
      }
      return wrapperType;
    }

    public static object GetUnderlyingObject(object wrapper)
    {
      DynamicWrapperBase dynamicWrapperBase = wrapper as DynamicWrapperBase;
      if (dynamicWrapperBase == null)
        throw new ArgumentException("Object is not a wrapper.", "wrapper");
      else
        return dynamicWrapperBase.UnderlyingObject;
    }

    private static Type GenerateWrapperType(Type interfaceType, Type underlyingType)
    {
      TypeBuilder proxyBuilder = DynamicWrapper.ModuleBuilder.DefineType(StringUtils.FormatWith("{0}_{1}_Wrapper", (IFormatProvider) CultureInfo.InvariantCulture, (object) interfaceType.Name, (object) underlyingType.Name), TypeAttributes.Sealed, typeof (DynamicWrapperBase), new Type[1]
      {
        interfaceType
      });
      WrapperMethodBuilder wrapperMethodBuilder = new WrapperMethodBuilder(underlyingType, proxyBuilder);
      foreach (MethodInfo newMethod in TypeExtensions.GetAllMethods(interfaceType))
        wrapperMethodBuilder.Generate(newMethod);
      return proxyBuilder.CreateType();
    }

    public static T CreateWrapper<T>(object realObject) where T : class
    {
      DynamicWrapperBase dynamicWrapperBase = (DynamicWrapperBase) Activator.CreateInstance(DynamicWrapper.GetWrapper(typeof (T), realObject.GetType()));
      dynamicWrapperBase.UnderlyingObject = realObject;
      return dynamicWrapperBase as T;
    }
  }
}
