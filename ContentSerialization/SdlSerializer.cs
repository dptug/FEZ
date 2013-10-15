// Type: ContentSerialization.SdlSerializer
// Assembly: ContentSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 458E2D00-F22E-495B-9E0A-A944909C9571
// Assembly location: F:\Program Files (x86)\FEZ\ContentSerialization.dll

using Common;
using ContentSerialization.Attributes;
using ContentSerialization.Properties;
using Microsoft.Xna.Framework;
using SDL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace ContentSerialization
{
  public static class SdlSerializer
  {
    private static readonly SerializationAttribute DefaultAttribute = new SerializationAttribute();
    private static readonly System.Type[] EmptyTypes = new System.Type[0];
    public const string AnonymousTag = "content";
    private const string TypeAttribute = "type";
    public static bool Compiling;

    static SdlSerializer()
    {
    }

    public static void Serialize<T>(StreamWriter writer, T instance)
    {
      System.Type type = typeof (T);
      Tag tag = new Tag(SdlSerializer.LowerCamelCase(type.Name));
      tag["type"] = (object) ReflectionHelper.GetShortAssemblyQualifiedName(type);
      SdlSerializer.SerializeInternal((object) instance, type, tag);
      tag.Write((TextWriter) writer, true);
    }

    public static void Serialize<T>(string filePath, T instance)
    {
      System.Type type = typeof (T);
      Tag tag = new Tag(SdlSerializer.LowerCamelCase(type.Name));
      tag["type"] = (object) ReflectionHelper.GetShortAssemblyQualifiedName(type);
      SdlSerializer.SerializeInternal((object) instance, type, tag);
      tag.WriteFile(filePath, true);
    }

    private static void SerializeInternal(object instance, System.Type declaredType, Tag tag)
    {
      System.Type type = declaredType;
      TypeSerializationAttribute serializationAttribute = ReflectionHelper.GetFirstAttribute<TypeSerializationAttribute>(type) ?? new TypeSerializationAttribute();
      foreach (MemberInfo memberInfo in ReflectionHelper.GetSerializableMembers(type))
      {
        SerializationAttribute attr = ReflectionHelper.GetFirstAttribute<SerializationAttribute>(memberInfo) ?? new SerializationAttribute();
        if (!attr.Ignore)
        {
          System.Type memberType = ReflectionHelper.GetMemberType(memberInfo);
          object obj1 = ReflectionHelper.GetValue(memberInfo, instance);
          if (!attr.Optional || serializationAttribute.FlattenToList || obj1 != null && (!attr.DefaultValueOptional || !SdlSerializer.IsDefault(obj1)))
          {
            bool simpleType;
            object obj2 = SdlSerializer.TryCoerce(obj1, out simpleType);
            if ((serializationAttribute.FlattenToList || attr.UseAttribute) && !simpleType)
              throw new SdlSerializationException(Resources.SimpleTypeRequired, type, memberInfo);
            if (serializationAttribute.FlattenToList)
            {
              tag.AddValue(obj2);
            }
            else
            {
              string name = SdlSerializer.LowerCamelCase(attr.Name ?? memberInfo.Name);
              if (attr.UseAttribute)
                tag[name] = obj2;
              else
                SdlSerializer.SerializeChild(name, obj2, memberType, simpleType, attr, tag);
            }
          }
        }
      }
    }

    private static bool IsDefault(object value)
    {
      System.Type type = value.GetType();
      if (type == typeof (string))
        return (string) value == "";
      if (type == typeof (char))
        return (int) (char) value == 0;
      if (type == typeof (Decimal))
        return (Decimal) value == new Decimal(0);
      if (type == typeof (double))
        return (double) value == 0.0;
      if (type == typeof (float))
        return (double) (float) value == 0.0;
      if (type == typeof (long))
        return (long) value == 0L;
      if (type == typeof (ushort))
        return (int) (ushort) value == 0;
      if (type == typeof (uint))
        return (int) (uint) value == 0;
      if (type == typeof (int))
        return (int) value == 0;
      if (type == typeof (short))
        return (int) (short) value == 0;
      if (type == typeof (sbyte))
        return (int) (sbyte) value == 0;
      if (type == typeof (byte))
        return (int) (byte) value == 0;
      if (type == typeof (bool))
        return !(bool) value;
      if (value is ICollection)
        return (value as ICollection).Count == 0;
      if (value is Array)
        return (value as Array).Length == 0;
      if (ReflectionHelper.IsGenericSet(type))
        return (int) ReflectionHelper.GetValue(type.GetProperty("Count", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, typeof (int), SdlSerializer.EmptyTypes, (ParameterModifier[]) null), value) == 0;
      if (type.IsEnum)
        return (int) value == 0;
      else
        return value.Equals(ReflectionHelper.Instantiate(type));
    }

    private static string LowerCamelCase(string identifier)
    {
      return identifier.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + identifier.Substring(1);
    }

    private static object TryCoerce(object value, out bool simpleType)
    {
      value = SDLUtil.TryCoerce(value, out simpleType);
      if (!simpleType)
      {
        System.Type type = value.GetType();
        if (value is Enum)
        {
          value = (object) Util.GetName(type, value);
          simpleType = true;
        }
        else if (ReflectionHelper.IsNullable(type))
          value = SDLUtil.TryCoerce(ReflectionHelper.GetValue(type.GetProperty("Value"), value), out simpleType);
      }
      return value;
    }

    private static void SerializeChild(string name, object value, System.Type type, bool simpleType, SerializationAttribute attr, Tag parent)
    {
      Tag tag = new Tag(name);
      if ((!simpleType || type == typeof (object)) && (value != null && !ReflectionHelper.IsNullable(type)) && value.GetType() != type)
      {
        type = value.GetType();
        tag["type"] = (object) ReflectionHelper.GetShortAssemblyQualifiedName(type);
      }
      if (simpleType)
        tag.AddValue(value);
      else if (type == typeof (Color))
      {
        Color color = (Color) value;
        SdlSerializer.AddValueList(tag, (object) color.R, (object) color.G, (object) color.B);
        if ((int) color.A != (int) byte.MaxValue)
          tag.AddValue((object) color.A);
      }
      else if (type == typeof (Vector2))
      {
        Vector2 vector2 = (Vector2) value;
        SdlSerializer.AddValueList(tag, (object) vector2.X, (object) vector2.Y);
      }
      else if (type == typeof (Vector3))
      {
        Vector3 vector3 = (Vector3) value;
        SdlSerializer.AddValueList(tag, (object) vector3.X, (object) vector3.Y, (object) vector3.Z);
      }
      else if (type == typeof (Vector4))
      {
        Vector4 vector4 = (Vector4) value;
        SdlSerializer.AddValueList(tag, (object) vector4.X, (object) vector4.Y, (object) vector4.Z, (object) vector4.W);
      }
      else if (type == typeof (Quaternion))
      {
        Quaternion quaternion = (Quaternion) value;
        SdlSerializer.AddValueList(tag, (object) quaternion.X, (object) quaternion.Y, (object) quaternion.Z, (object) quaternion.W);
      }
      else if (type == typeof (Matrix))
        SdlSerializer.SerializeMatrix(tag, (Matrix) value);
      else if (ReflectionHelper.IsGenericDictionary(type))
        SdlSerializer.SerializeDictionary(tag, value as IDictionary, attr.CollectionItemName);
      else if (ReflectionHelper.IsGenericCollection(type))
        SdlSerializer.SerializeCollection(tag, value as IEnumerable, attr.CollectionItemName);
      else
        SdlSerializer.SerializeInternal(value, type, tag);
      parent.AddChild(tag);
    }

    private static void SerializeMatrix(Tag tag, Matrix matrix)
    {
      Tag tag1 = new Tag("content");
      tag.AddChild(tag1);
      SdlSerializer.AddValueList(tag1, (object) matrix.M11, (object) matrix.M12, (object) matrix.M13, (object) matrix.M14);
      Tag tag2 = new Tag("content");
      tag.AddChild(tag2);
      SdlSerializer.AddValueList(tag2, (object) matrix.M21, (object) matrix.M22, (object) matrix.M23, (object) matrix.M24);
      Tag tag3 = new Tag("content");
      tag.AddChild(tag3);
      SdlSerializer.AddValueList(tag3, (object) matrix.M31, (object) matrix.M32, (object) matrix.M33, (object) matrix.M34);
      Tag tag4 = new Tag("content");
      tag.AddChild(tag4);
      SdlSerializer.AddValueList(tag4, (object) matrix.M41, (object) matrix.M42, (object) matrix.M43, (object) matrix.M44);
    }

    private static void SerializeDictionary(Tag tag, IDictionary dictionary, string customItemName)
    {
      bool simpleType1 = false;
      foreach (object index in (IEnumerable) dictionary.Keys)
      {
        SdlSerializer.TryCoerce(index, out simpleType1);
        if (simpleType1)
        {
          SdlSerializer.TryCoerce(dictionary[index], out simpleType1);
          if (!simpleType1)
            break;
        }
        else
          break;
      }
      if (simpleType1)
      {
        foreach (object index1 in (IEnumerable) dictionary.Keys)
        {
          bool simpleType2;
          string index2 = SdlSerializer.TryCoerce(index1, out simpleType2).ToString();
          object obj = SdlSerializer.TryCoerce(dictionary[index1], out simpleType2);
          tag[index2] = obj;
        }
      }
      else
      {
        System.Type[] genericArguments = dictionary.GetType().GetGenericArguments();
        System.Type type1 = genericArguments[0];
        System.Type type2 = genericArguments[1];
        foreach (object index in (IEnumerable) dictionary.Keys)
        {
          object instance = dictionary[index];
          bool simpleType2;
          object obj = SdlSerializer.TryCoerce(index, out simpleType2);
          Tag tag1 = new Tag(SdlSerializer.LowerCamelCase(customItemName ?? type2.Name));
          tag.AddChild(tag1);
          if (simpleType2)
          {
            tag1["key"] = obj;
            SdlSerializer.SerializeInternal(instance, type2, tag1);
          }
          else
          {
            SdlSerializer.SerializeChild("key", index, type1, false, SdlSerializer.DefaultAttribute, tag1);
            bool simpleType3;
            SdlSerializer.SerializeChild("value", SdlSerializer.TryCoerce(instance, out simpleType3), type2, simpleType3, SdlSerializer.DefaultAttribute, tag1);
          }
        }
      }
    }

    private static void SerializeCollection(Tag tag, IEnumerable collection, string customItemName)
    {
      List<object> list = new List<object>();
      System.Type type1 = collection.GetType();
      System.Type type2 = type1.IsArray ? type1.GetElementType() : type1.GetGenericArguments()[0];
      bool simpleType1 = false;
      foreach (object obj1 in collection)
      {
        object obj2 = SdlSerializer.TryCoerce(obj1, out simpleType1);
        if (simpleType1)
          list.Add(obj2);
        else
          break;
      }
      if (simpleType1)
      {
        foreach (object obj in list)
          tag.AddValue(obj);
      }
      else
      {
        foreach (object obj in collection)
        {
          bool simpleType2;
          SdlSerializer.SerializeChild(SdlSerializer.LowerCamelCase(customItemName ?? type2.Name), SdlSerializer.TryCoerce(obj, out simpleType2), type2, simpleType2, SdlSerializer.DefaultAttribute, tag);
        }
      }
    }

    private static void AddValueList(Tag tag, params object[] list)
    {
      SdlSerializer.AddValueList(tag, (IList) list);
    }

    private static void AddValueList(Tag tag, IList list)
    {
      foreach (object obj in (IEnumerable) list)
        tag.AddValue(obj);
    }

    public static T Deserialize<T>(StreamReader reader)
    {
      return (T) SdlSerializer.DeserializeInternal((System.Type) null, new Tag("content").Read(reader).Children[0], (object) null);
    }

    public static T Deserialize<T>(string filePath)
    {
      return (T) SdlSerializer.DeserializeInternal((System.Type) null, new Tag("content").ReadFile(filePath).Children[0], (object) null);
    }

    private static object DeserializeInternal(System.Type declaredType, Tag tag, object existingInstance)
    {
      System.Type type = declaredType;
      object instance = existingInstance;
      string typeName = tag["type"] as string;
      if (SdlSerializer.Compiling && typeName == "FezEngine.Structure.TrileSet, FezEngine")
        typeName = "FezContentPipeline.Content.TrileSetContent, FezContentPipeline";
      if (SdlSerializer.Compiling && typeName == "FezEngine.Structure.ArtObject, FezEngine")
        typeName = "FezContentPipeline.Content.ArtObjectContent, FezContentPipeline";
      if (SdlSerializer.Compiling && typeName != null)
        typeName = typeName.Replace(", FezEngine", ", FezContentPipeline");
      if (typeName != null)
        type = System.Type.GetType(typeName);
      if ((instance == null || declaredType != type) && !SdlSerializer.IsCoercible(type))
        instance = ReflectionHelper.Instantiate(type);
      TypeSerializationAttribute serializationAttribute = ReflectionHelper.GetFirstAttribute<TypeSerializationAttribute>(type) ?? new TypeSerializationAttribute();
      int num = 0;
      foreach (MemberInfo memberInfo in ReflectionHelper.GetSerializableMembers(type))
      {
        SerializationAttribute attr = ReflectionHelper.GetFirstAttribute<SerializationAttribute>(memberInfo) ?? new SerializationAttribute();
        if (!attr.Ignore)
        {
          bool valueFound = true;
          object obj = (object) null;
          if (serializationAttribute.FlattenToList)
          {
            obj = tag[num++];
          }
          else
          {
            System.Type memberType = ReflectionHelper.GetMemberType(memberInfo);
            string index = SdlSerializer.LowerCamelCase(attr.Name ?? memberInfo.Name);
            if (attr.UseAttribute)
            {
              if (tag.Attributes.ContainsKey(index))
              {
                obj = SdlSerializer.DeCoerce(tag[index], memberType);
              }
              else
              {
                if (!attr.Optional)
                  throw new SdlSerializationException(Resources.MissingNonOptionalTagOrAttribute, type, memberInfo);
                valueFound = false;
              }
            }
            else
            {
              object existingInstance1 = ReflectionHelper.GetValue(memberInfo, instance);
              bool simpleType = SdlSerializer.IsCoercible(memberType);
              obj = SdlSerializer.DeserializeChild(index, existingInstance1, memberType, simpleType, attr, tag, out valueFound);
            }
          }
          if (valueFound)
            ReflectionHelper.SetValue(memberInfo, instance, obj);
        }
      }
      if (instance is IDeserializationCallback)
        (instance as IDeserializationCallback).OnDeserialization();
      return instance;
    }

    private static bool IsCoercible(System.Type type)
    {
      if (!SDLUtil.IsCoercible(type) && !type.IsEnum)
        return ReflectionHelper.IsNullable(type);
      else
        return true;
    }

    private static object DeserializeChild(string name, object existingInstance, System.Type type, bool simpleType, SerializationAttribute attr, Tag tag, out bool valueFound)
    {
      Tag child = tag.GetChild(name);
      if (child == null)
      {
        if (!attr.Optional)
          throw new SdlSerializationException(Resources.MissingNonOptionalTagOrAttribute, type, name);
        valueFound = false;
        return existingInstance;
      }
      else
      {
        valueFound = true;
        return SdlSerializer.DeserializeChild(child, type, existingInstance, simpleType);
      }
    }

    private static object DeserializeChild(Tag childTag, System.Type type, object existingInstance, bool simpleType)
    {
      string typeName = childTag["type"] as string;
      if (typeName != null)
      {
        if (SdlSerializer.Compiling)
          typeName = typeName.Replace(", FezEngine", ", FezContentPipeline");
        type = System.Type.GetType(typeName);
        simpleType = SdlSerializer.IsCoercible(type);
      }
      object existingInstance1 = existingInstance;
      if (SdlSerializer.IsNull(childTag))
        existingInstance1 = (object) null;
      else if (simpleType)
        existingInstance1 = SdlSerializer.DeCoerce(childTag.Value, type);
      else if (type.Name == "Color")
        existingInstance1 = (object) (childTag.Values.Count == 4 ? new Color((int) (byte) (int) childTag.Values[0], (int) (byte) (int) childTag.Values[1], (int) (byte) (int) childTag.Values[2], (int) (byte) (int) childTag.Values[3]) : new Color((int) (byte) (int) childTag.Values[0], (int) (byte) (int) childTag.Values[1], (int) (byte) (int) childTag.Values[2]));
      else if (type.Name == "Vector2")
        existingInstance1 = (object) new Vector2((float) childTag.Values[0], (float) childTag.Values[1]);
      else if (type.Name == "Vector3")
        existingInstance1 = (object) new Vector3(Convert.ToSingle(childTag.Values[0]), Convert.ToSingle(childTag.Values[1]), Convert.ToSingle(childTag.Values[2]));
      else if (type.Name == "Vector4")
        existingInstance1 = (object) new Vector4((float) childTag.Values[0], (float) childTag.Values[1], (float) childTag.Values[2], (float) childTag.Values[3]);
      else if (type.Name == "Quaternion")
        existingInstance1 = (object) new Quaternion((float) childTag.Values[0], (float) childTag.Values[1], (float) childTag.Values[2], (float) childTag.Values[3]);
      else if (type.Name == "Matrix")
        existingInstance1 = (object) SdlSerializer.DeserializeMatrix(childTag);
      else if (ReflectionHelper.IsGenericDictionary(type))
        SdlSerializer.DeserializeDictionary(childTag, existingInstance1 as IDictionary, type);
      else
        existingInstance1 = !ReflectionHelper.IsGenericCollection(type) ? SdlSerializer.DeserializeInternal(type, childTag, existingInstance1) : (object) SdlSerializer.DeserializeCollection(childTag, existingInstance1 as IEnumerable, type);
      return existingInstance1;
    }

    private static bool IsNull(Tag tag)
    {
      bool flag = false;
      foreach (string str in (IEnumerable<string>) tag.Attributes.Keys)
      {
        if (str != "type")
        {
          flag = true;
          break;
        }
      }
      if (tag.Values.Count == 1 && tag.Children.Count == 0 && tag.Value == null)
        return !flag;
      else
        return false;
    }

    private static object DeCoerce(object coerced, System.Type type)
    {
      if (coerced == null)
        return (object) null;
      if (type.IsInstanceOfType(coerced))
        return coerced;
      if (type.IsEnum && coerced is string)
        return Enum.Parse(type, coerced as string, false);
      if (!ReflectionHelper.IsNullable(type))
        throw new NotImplementedException();
      object instance = Activator.CreateInstance(type);
      ReflectionHelper.SetValue(type.GetProperty("Value"), instance, coerced);
      return instance;
    }

    private static Matrix DeserializeMatrix(Tag tag)
    {
      Matrix matrix = new Matrix();
      Tag tag1 = tag.Children[0];
      matrix.M11 = (float) tag1.Values[0];
      matrix.M12 = (float) tag1.Values[1];
      matrix.M13 = (float) tag1.Values[2];
      matrix.M14 = (float) tag1.Values[3];
      Tag tag2 = tag.Children[1];
      matrix.M21 = (float) tag2.Values[0];
      matrix.M22 = (float) tag2.Values[1];
      matrix.M23 = (float) tag2.Values[2];
      matrix.M24 = (float) tag2.Values[3];
      Tag tag3 = tag.Children[2];
      matrix.M31 = (float) tag3.Values[0];
      matrix.M32 = (float) tag3.Values[1];
      matrix.M33 = (float) tag3.Values[2];
      matrix.M34 = (float) tag3.Values[3];
      Tag tag4 = tag.Children[3];
      matrix.M41 = (float) tag4.Values[0];
      matrix.M42 = (float) tag4.Values[1];
      matrix.M43 = (float) tag4.Values[2];
      matrix.M44 = (float) tag4.Values[3];
      return matrix;
    }

    private static void DeserializeDictionary(Tag tag, IDictionary dictionary, System.Type declaredType)
    {
      bool flag = tag.Children.Count == 0;
      if (dictionary == null)
        dictionary = ReflectionHelper.Instantiate(declaredType) as IDictionary;
      System.Type[] genericArguments = dictionary.GetType().GetGenericArguments();
      System.Type type1 = genericArguments[0];
      System.Type type2 = genericArguments[1];
      if (flag)
      {
        foreach (string index in (IEnumerable<string>) tag.Attributes.Keys)
        {
          object key = SdlSerializer.DeCoerce((object) index, type1);
          object obj = SdlSerializer.DeCoerce(tag[index], type2);
          SdlSerializer.SafeAddToDictionary(dictionary, key, obj);
        }
      }
      else
      {
        foreach (Tag tag1 in (IEnumerable<Tag>) tag.Children)
        {
          object coerced = tag1["key"];
          Tag child = tag1.GetChild("key");
          if (!(coerced != null ^ child != null))
            throw new SdlSerializationException(Resources.IllegalCollectionStructure, dictionary.GetType(), tag.Name);
          if (coerced != null)
          {
            object key = SdlSerializer.DeCoerce(coerced, type1);
            object obj = SdlSerializer.DeserializeInternal(type2, tag1, (object) null);
            SdlSerializer.SafeAddToDictionary(dictionary, key, obj);
          }
          else
          {
            object key = SdlSerializer.DeserializeInternal(type1, child, (object) null);
            bool simpleType = SdlSerializer.IsCoercible(type2);
            bool valueFound;
            object obj = SdlSerializer.DeserializeChild("value", (object) null, type2, simpleType, SdlSerializer.DefaultAttribute, tag1, out valueFound);
            SdlSerializer.SafeAddToDictionary(dictionary, key, obj);
          }
        }
      }
    }

    private static void SafeAddToDictionary(IDictionary dictionary, object key, object value)
    {
      if (dictionary.Contains(key))
        dictionary.Remove(key);
      dictionary.Add(key, value);
    }

    private static IEnumerable DeserializeCollection(Tag tag, IEnumerable collection, System.Type declaredType)
    {
      bool flag1 = tag.Values.Count > 0;
      bool flag2 = tag.Children.Count > 0;
      if (collection == null)
        collection = !declaredType.IsArray ? ReflectionHelper.Instantiate(declaredType) as IEnumerable : (IEnumerable) Array.CreateInstance(declaredType.GetElementType(), 0);
      if (!flag1 && !flag2)
        return collection;
      System.Type type1 = collection.GetType();
      bool flag3 = collection is IList;
      bool isArray = type1.IsArray;
      bool flag4 = ReflectionHelper.IsGenericSet(type1);
      System.Type type2 = isArray ? type1.GetElementType() : type1.GetGenericArguments()[0];
      bool simpleType = SdlSerializer.IsCoercible(type2);
      DynamicMethodDelegate dynamicMethodDelegate = (DynamicMethodDelegate) null;
      if (flag4)
        dynamicMethodDelegate = ReflectionHelper.GetDelegate(type1.GetMethod("Add"));
      if (flag1 && flag2)
        throw new SdlSerializationException(Resources.IllegalCollectionStructure, type1, tag.Name);
      if (isArray)
      {
        int length = flag1 ? tag.Values.Count : tag.Children.Count;
        collection = (IEnumerable) Array.CreateInstance(type2, length);
      }
      int num = 0;
      if (flag1)
      {
        foreach (object coerced in (IEnumerable<object>) tag.Values)
        {
          object obj1 = SdlSerializer.DeCoerce(coerced, type2);
          if (isArray)
            ((IList) collection)[num++] = obj1;
          else if (flag4)
          {
            object obj2 = dynamicMethodDelegate((object) collection, new object[1]
            {
              obj1
            });
          }
          else
          {
            if (!flag3)
              throw new NotImplementedException();
            ((IList) collection).Add(obj1);
          }
        }
      }
      else
      {
        foreach (Tag tag1 in (IEnumerable<Tag>) tag.Children)
        {
          object obj1 = SdlSerializer.IsNull(tag1) ? (object) null : SdlSerializer.DeserializeChild(tag1, type2, (object) null, simpleType);
          if (isArray)
            ((IList) collection)[num++] = obj1;
          else if (flag4)
          {
            object obj2 = dynamicMethodDelegate((object) collection, new object[1]
            {
              obj1
            });
          }
          else
          {
            if (!flag3)
              throw new NotImplementedException();
            ((IList) collection).Add(obj1);
          }
        }
      }
      return collection;
    }
  }
}
