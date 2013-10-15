// Type: Microsoft.Xna.Framework.Content.ContentExtensions
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Reflection;

namespace Microsoft.Xna.Framework.Content
{
  public static class ContentExtensions
  {
    public static ConstructorInfo GetDefaultConstructor(this Type type)
    {
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      return type.GetConstructor(bindingAttr, (Binder) null, new Type[0], (ParameterModifier[]) null);
    }

    public static PropertyInfo[] GetAllProperties(this Type type)
    {
      BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      return type.GetProperties(bindingAttr);
    }

    public static FieldInfo[] GetAllFields(this Type type)
    {
      BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      return type.GetFields(bindingAttr);
    }
  }
}
