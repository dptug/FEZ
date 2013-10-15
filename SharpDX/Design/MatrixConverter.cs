// Type: SharpDX.Design.MatrixConverter
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace SharpDX.Design
{
  public class MatrixConverter : BaseConverter
  {
    public MatrixConverter()
    {
      Type type = typeof (Matrix);
      this.Properties = new PropertyDescriptorCollection((PropertyDescriptor[]) new FieldPropertyDescriptor[16]
      {
        new FieldPropertyDescriptor(type.GetField("M11")),
        new FieldPropertyDescriptor(type.GetField("M12")),
        new FieldPropertyDescriptor(type.GetField("M13")),
        new FieldPropertyDescriptor(type.GetField("M14")),
        new FieldPropertyDescriptor(type.GetField("M21")),
        new FieldPropertyDescriptor(type.GetField("M22")),
        new FieldPropertyDescriptor(type.GetField("M23")),
        new FieldPropertyDescriptor(type.GetField("M24")),
        new FieldPropertyDescriptor(type.GetField("M31")),
        new FieldPropertyDescriptor(type.GetField("M32")),
        new FieldPropertyDescriptor(type.GetField("M33")),
        new FieldPropertyDescriptor(type.GetField("M34")),
        new FieldPropertyDescriptor(type.GetField("M41")),
        new FieldPropertyDescriptor(type.GetField("M42")),
        new FieldPropertyDescriptor(type.GetField("M43")),
        new FieldPropertyDescriptor(type.GetField("M44"))
      });
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (value is Matrix)
      {
        Matrix matrix = (Matrix) value;
        if (destinationType == typeof (string))
          return (object) BaseConverter.ConvertFromValues<float>(context, culture, matrix.ToArray());
        if (destinationType == typeof (InstanceDescriptor))
        {
          ConstructorInfo constructor = typeof (Matrix).GetConstructor(MathUtil.Array<Type>(typeof (float), 16));
          if (constructor != (ConstructorInfo) null)
            return (object) new InstanceDescriptor((MemberInfo) constructor, (ICollection) matrix.ToArray());
        }
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      float[] values = BaseConverter.ConvertToValues<float>(context, culture, value);
      if (values == null)
        return base.ConvertFrom(context, culture, value);
      else
        return (object) new Matrix(values);
    }

    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
      if (propertyValues == null)
        throw new ArgumentNullException("propertyValues");
      return (object) new Matrix()
      {
        M11 = (float) propertyValues[(object) "M11"],
        M12 = (float) propertyValues[(object) "M12"],
        M13 = (float) propertyValues[(object) "M13"],
        M14 = (float) propertyValues[(object) "M14"],
        M21 = (float) propertyValues[(object) "M21"],
        M22 = (float) propertyValues[(object) "M22"],
        M23 = (float) propertyValues[(object) "M23"],
        M24 = (float) propertyValues[(object) "M24"],
        M31 = (float) propertyValues[(object) "M31"],
        M32 = (float) propertyValues[(object) "M32"],
        M33 = (float) propertyValues[(object) "M33"],
        M34 = (float) propertyValues[(object) "M34"],
        M41 = (float) propertyValues[(object) "M41"],
        M42 = (float) propertyValues[(object) "M42"],
        M43 = (float) propertyValues[(object) "M43"],
        M44 = (float) propertyValues[(object) "M44"]
      };
    }
  }
}
