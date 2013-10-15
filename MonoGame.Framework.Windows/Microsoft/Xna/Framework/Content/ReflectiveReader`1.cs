// Type: Microsoft.Xna.Framework.Content.ReflectiveReader`1
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Reflection;

namespace Microsoft.Xna.Framework.Content
{
  internal class ReflectiveReader<T> : ContentTypeReader
  {
    private ConstructorInfo constructor;
    private PropertyInfo[] properties;
    private FieldInfo[] fields;
    private ContentTypeReaderManager manager;
    private Type targetType;
    private Type baseType;
    private ContentTypeReader baseTypeReader;

    internal ReflectiveReader()
      : base(typeof (T))
    {
      this.targetType = typeof (T);
    }

    protected internal override void Initialize(ContentTypeReaderManager manager)
    {
      base.Initialize(manager);
      this.manager = manager;
      Type baseType = this.targetType.BaseType;
      if (baseType != (Type) null && baseType != typeof (object))
      {
        this.baseType = baseType;
        this.baseTypeReader = manager.GetTypeReader(this.baseType);
      }
      this.constructor = ContentExtensions.GetDefaultConstructor(this.targetType);
      this.properties = ContentExtensions.GetAllProperties(this.targetType);
      this.fields = ContentExtensions.GetAllFields(this.targetType);
    }

    private static object CreateChildObject(PropertyInfo property, FieldInfo field)
    {
      object obj = (object) null;
      Type type = !(property != (PropertyInfo) null) ? field.FieldType : property.PropertyType;
      if (type.IsClass && !type.IsAbstract)
      {
        ConstructorInfo defaultConstructor = ContentExtensions.GetDefaultConstructor(type);
        if (defaultConstructor != (ConstructorInfo) null)
          obj = defaultConstructor.Invoke((object[]) null);
      }
      return obj;
    }

    private void Read(object parent, ContentReader input, MemberInfo member)
    {
      PropertyInfo property = member as PropertyInfo;
      FieldInfo field = member as FieldInfo;
      if (property != (PropertyInfo) null && (!property.CanWrite || !property.CanRead))
        return;
      if (property != (PropertyInfo) null && property.Name == "Item")
      {
        MethodInfo getMethod = property.GetGetMethod();
        MethodInfo setMethod = property.GetSetMethod();
        if (getMethod != (MethodInfo) null && getMethod.GetParameters().Length > 0 || setMethod != (MethodInfo) null && setMethod.GetParameters().Length > 0)
          return;
      }
      if (Attribute.GetCustomAttribute(member, typeof (ContentSerializerIgnoreAttribute)) != null)
        return;
      Attribute customAttribute = Attribute.GetCustomAttribute(member, typeof (ContentSerializerAttribute));
      bool flag = false;
      if (customAttribute != null)
        flag = (customAttribute as ContentSerializerAttribute).SharedResource;
      else if (property != (PropertyInfo) null)
      {
        foreach (MethodBase methodBase in property.GetAccessors(true))
        {
          if (!methodBase.IsPublic)
            return;
        }
      }
      else if (!field.IsPublic)
        return;
      Type type;
      ContentTypeReader typeReader = !(property != (PropertyInfo) null) ? this.manager.GetTypeReader(type = field.FieldType) : this.manager.GetTypeReader(type = property.PropertyType);
      if (!flag)
      {
        object childObject = ReflectiveReader<T>.CreateChildObject(property, field);
        object obj = typeReader != null || !(type == typeof (object)) ? input.ReadObject<object>(typeReader, childObject) : input.ReadObject<object>();
        if (property != (PropertyInfo) null)
          property.SetValue(parent, obj, (object[]) null);
        else if (!field.IsPrivate || customAttribute != null)
          field.SetValue(parent, obj);
      }
      else
      {
        Action<object> fixup = (Action<object>) (value =>
        {
          if (property != (PropertyInfo) null)
            property.SetValue(parent, value, (object[]) null);
          else
            field.SetValue(parent, value);
        });
        input.ReadSharedResource<object>(fixup);
      }
    }

    protected internal override object Read(ContentReader input, object existingInstance)
    {
      T obj = existingInstance == null ? (this.constructor == (ConstructorInfo) null ? (T) Activator.CreateInstance(typeof (T), false) : (T) this.constructor.Invoke((object[]) null)) : (T) existingInstance;
      if (this.baseTypeReader != null)
        this.baseTypeReader.Read(input, (object) obj);
      object parent = (object) obj;
      foreach (PropertyInfo propertyInfo in this.properties)
        this.Read(parent, input, (MemberInfo) propertyInfo);
      foreach (FieldInfo fieldInfo in this.fields)
        this.Read(parent, input, (MemberInfo) fieldInfo);
      return (object) (T) parent;
    }
  }
}
