// Type: Newtonsoft.Json.Utilities.ConvertUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class ConvertUtils
  {
    private static readonly ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>> CastConverters = new ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>>(new Func<ConvertUtils.TypeConvertKey, Func<object, object>>(ConvertUtils.CreateCastConverter));

    static ConvertUtils()
    {
    }

    public static TypeCode GetTypeCode(this IConvertible convertible)
    {
      return convertible.GetTypeCode();
    }

    public static TypeCode GetTypeCode(object o)
    {
      return Convert.GetTypeCode(o);
    }

    public static TypeCode GetTypeCode(Type t)
    {
      return Type.GetTypeCode(t);
    }

    public static IConvertible ToConvertible(object o)
    {
      return o as IConvertible;
    }

    public static bool IsConvertible(object o)
    {
      return o is IConvertible;
    }

    public static bool IsConvertible(Type t)
    {
      return typeof (IConvertible).IsAssignableFrom(t);
    }

    private static Func<object, object> CreateCastConverter(ConvertUtils.TypeConvertKey t)
    {
      MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[1]
      {
        t.InitialType
      });
      if (method == null)
        method = t.TargetType.GetMethod("op_Explicit", new Type[1]
        {
          t.InitialType
        });
      if (method == null)
        return (Func<object, object>) null;
      MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
      return (Func<object, object>) (o => call((object) null, new object[1]
      {
        o
      }));
    }

    public static object Convert(object initialValue, CultureInfo culture, Type targetType)
    {
      if (initialValue == null)
        throw new ArgumentNullException("initialValue");
      if (ReflectionUtils.IsNullableType(targetType))
        targetType = Nullable.GetUnderlyingType(targetType);
      Type type = initialValue.GetType();
      if (targetType == type)
        return initialValue;
      if (ConvertUtils.IsConvertible(initialValue) && ConvertUtils.IsConvertible(targetType))
      {
        if (TypeExtensions.IsEnum(targetType))
        {
          if (initialValue is string)
            return Enum.Parse(targetType, initialValue.ToString(), true);
          if (ConvertUtils.IsInteger(initialValue))
            return Enum.ToObject(targetType, initialValue);
        }
        return Convert.ChangeType(initialValue, targetType, (IFormatProvider) culture);
      }
      else
      {
        if (initialValue is string && typeof (Type).IsAssignableFrom(targetType))
          return (object) Type.GetType((string) initialValue, true);
        if (TypeExtensions.IsInterface(targetType) || TypeExtensions.IsGenericTypeDefinition(targetType) || TypeExtensions.IsAbstract(targetType))
          throw new ArgumentException(StringUtils.FormatWith("Target type {0} is not a value type or a non-abstract class.", (IFormatProvider) CultureInfo.InvariantCulture, (object) targetType), "targetType");
        if (initialValue is string)
        {
          if (targetType == typeof (Guid))
            return (object) new Guid((string) initialValue);
          if (targetType == typeof (Uri))
            return (object) new Uri((string) initialValue, UriKind.RelativeOrAbsolute);
          if (targetType == typeof (TimeSpan))
            return (object) TimeSpan.Parse((string) initialValue);
        }
        TypeConverter converter1 = ConvertUtils.GetConverter(type);
        if (converter1 != null && converter1.CanConvertTo(targetType))
          return converter1.ConvertTo((ITypeDescriptorContext) null, culture, initialValue, targetType);
        TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
        if (converter2 != null && converter2.CanConvertFrom(type))
          return converter2.ConvertFrom((ITypeDescriptorContext) null, culture, initialValue);
        if (initialValue == DBNull.Value)
        {
          if (ReflectionUtils.IsNullable(targetType))
            return ConvertUtils.EnsureTypeAssignable((object) null, type, targetType);
          else
            throw new Exception(StringUtils.FormatWith("Can not convert null {0} into non-nullable {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type, (object) targetType));
        }
        else if (initialValue is INullable)
          return ConvertUtils.EnsureTypeAssignable(ConvertUtils.ToValue((INullable) initialValue), type, targetType);
        else
          throw new InvalidOperationException(StringUtils.FormatWith("Can not convert from {0} to {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) type, (object) targetType));
      }
    }

    public static bool TryConvert(object initialValue, CultureInfo culture, Type targetType, out object convertedValue)
    {
      return MiscellaneousUtils.TryAction<object>((Creator<object>) (() => ConvertUtils.Convert(initialValue, culture, targetType)), out convertedValue);
    }

    public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
    {
      if (targetType == typeof (object))
        return initialValue;
      if (initialValue == null && ReflectionUtils.IsNullable(targetType))
        return (object) null;
      object convertedValue;
      if (ConvertUtils.TryConvert(initialValue, culture, targetType, out convertedValue))
        return convertedValue;
      else
        return ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
    }

    private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
    {
      Type type = value != null ? value.GetType() : (Type) null;
      if (value != null)
      {
        if (targetType.IsAssignableFrom(type))
          return value;
        Func<object, object> func = ConvertUtils.CastConverters.Get(new ConvertUtils.TypeConvertKey(type, targetType));
        if (func != null)
          return func(value);
      }
      else if (ReflectionUtils.IsNullable(targetType))
        return (object) null;
      throw new ArgumentException(StringUtils.FormatWith("Could not cast or convert from {0} to {1}.", (IFormatProvider) CultureInfo.InvariantCulture, initialType != null ? (object) initialType.ToString() : (object) "{null}", (object) targetType));
    }

    public static object ToValue(INullable nullableValue)
    {
      if (nullableValue == null)
        return (object) null;
      if (nullableValue is SqlInt32)
        return ConvertUtils.ToValue((INullable) (ValueType) nullableValue);
      if (nullableValue is SqlInt64)
        return ConvertUtils.ToValue((INullable) (ValueType) nullableValue);
      if (nullableValue is SqlBoolean)
        return ConvertUtils.ToValue((INullable) (ValueType) nullableValue);
      if (nullableValue is SqlString)
        return ConvertUtils.ToValue((INullable) (ValueType) nullableValue);
      if (nullableValue is SqlDateTime)
        return ConvertUtils.ToValue((INullable) (ValueType) nullableValue);
      else
        throw new ArgumentException(StringUtils.FormatWith("Unsupported INullable type: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) nullableValue.GetType()));
    }

    internal static TypeConverter GetConverter(Type t)
    {
      return JsonTypeReflector.GetTypeConverter(t);
    }

    public static bool IsInteger(object value)
    {
      switch (ConvertUtils.GetTypeCode(value))
      {
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return true;
        default:
          return false;
      }
    }

    internal struct TypeConvertKey : IEquatable<ConvertUtils.TypeConvertKey>
    {
      private readonly Type _initialType;
      private readonly Type _targetType;

      public Type InitialType
      {
        get
        {
          return this._initialType;
        }
      }

      public Type TargetType
      {
        get
        {
          return this._targetType;
        }
      }

      public TypeConvertKey(Type initialType, Type targetType)
      {
        this._initialType = initialType;
        this._targetType = targetType;
      }

      public override int GetHashCode()
      {
        return this._initialType.GetHashCode() ^ this._targetType.GetHashCode();
      }

      public override bool Equals(object obj)
      {
        if (!(obj is ConvertUtils.TypeConvertKey))
          return false;
        else
          return this.Equals((ConvertUtils.TypeConvertKey) obj);
      }

      public bool Equals(ConvertUtils.TypeConvertKey other)
      {
        if (this._initialType == other._initialType)
          return this._targetType == other._targetType;
        else
          return false;
      }
    }
  }
}
