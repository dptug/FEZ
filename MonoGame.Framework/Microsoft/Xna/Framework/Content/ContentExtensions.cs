// Type: Microsoft.Xna.Framework.Content.ContentExtensions
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;
using System.Linq;
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
      return Enumerable.ToList<PropertyInfo>((IEnumerable<PropertyInfo>) type.GetProperties(bindingAttr)).FindAll((Predicate<PropertyInfo>) (p => p.GetGetMethod() == p.GetGetMethod().GetBaseDefinition())).ToArray();
    }

    public static FieldInfo[] GetAllFields(this Type type)
    {
      BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      return type.GetFields(bindingAttr);
    }
  }
}
