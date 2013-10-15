// Type: SharpDX.Win32.Variant
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
  public struct Variant
  {
    private ushort vt;
    private ushort reserved1;
    private ushort reserved2;
    private ushort reserved3;
    private Variant.VariantValue variantValue;

    public VariantElementType ElementType
    {
      get
      {
        return (VariantElementType) ((uint) this.vt & 4095U);
      }
      set
      {
        this.vt = (ushort) ((VariantElementType) ((int) this.vt & 61440) | value);
      }
    }

    public VariantType Type
    {
      get
      {
        return (VariantType) ((uint) this.vt & 61440U);
      }
      set
      {
        this.vt = (ushort) ((VariantType) ((int) this.vt & 4095) | value);
      }
    }

    public unsafe object Value
    {
      get
      {
        switch (this.Type)
        {
          case VariantType.Default:
            switch (this.ElementType)
            {
              case VariantElementType.Empty:
              case VariantElementType.Null:
                return (object) null;
              case VariantElementType.Short:
                return (object) this.variantValue.shortValue;
              case VariantElementType.Int:
              case VariantElementType.Int1:
                return (object) this.variantValue.intValue;
              case VariantElementType.Float:
                return (object) this.variantValue.floatValue;
              case VariantElementType.Double:
                return (object) this.variantValue.doubleValue;
              case VariantElementType.BinaryString:
                throw new NotSupportedException();
              case VariantElementType.Dispatch:
              case VariantElementType.ComUnknown:
                return (object) new ComObject(this.variantValue.pointerValue);
              case VariantElementType.Bool:
                return (object) (bool) (this.variantValue.intValue != 0 ? 1 : 0);
              case VariantElementType.Byte:
                return (object) this.variantValue.signedByteValue;
              case VariantElementType.UByte:
                return (object) this.variantValue.byteValue;
              case VariantElementType.UShort:
                return (object) this.variantValue.ushortValue;
              case VariantElementType.UInt:
              case VariantElementType.UInt1:
                return (object) this.variantValue.uintValue;
              case VariantElementType.Long:
                return (object) this.variantValue.longValue;
              case VariantElementType.ULong:
                return (object) this.variantValue.ulongValue;
              case VariantElementType.Pointer:
              case VariantElementType.IntPointer:
                return (object) this.variantValue.pointerValue;
              case VariantElementType.StringPointer:
                return (object) Marshal.PtrToStringAnsi(this.variantValue.pointerValue);
              case VariantElementType.WStringPointer:
                return (object) Marshal.PtrToStringUni(this.variantValue.pointerValue);
              case VariantElementType.Blob:
                byte[] data1 = new byte[(int) this.variantValue.recordValue.RecordInfo];
                Utilities.Read<byte>(this.variantValue.recordValue.RecordPointer, data1, 0, data1.Length);
                return (object) data1;
              default:
                return (object) null;
            }
          case VariantType.Vector:
            int count = (int) this.variantValue.recordValue.RecordInfo;
            switch (this.ElementType)
            {
              case VariantElementType.Short:
                short[] data2 = new short[count];
                Utilities.Read<short>(this.variantValue.recordValue.RecordPointer, data2, 0, count);
                return (object) data2;
              case VariantElementType.Int:
              case VariantElementType.Int1:
                int[] data3 = new int[count];
                Utilities.Read<int>(this.variantValue.recordValue.RecordPointer, data3, 0, count);
                return (object) data3;
              case VariantElementType.Float:
                float[] data4 = new float[count];
                Utilities.Read<float>(this.variantValue.recordValue.RecordPointer, data4, 0, count);
                return (object) data4;
              case VariantElementType.Double:
                double[] data5 = new double[count];
                Utilities.Read<double>(this.variantValue.recordValue.RecordPointer, data5, 0, count);
                return (object) data5;
              case VariantElementType.BinaryString:
                throw new NotSupportedException();
              case VariantElementType.Dispatch:
              case VariantElementType.ComUnknown:
                ComObject[] comObjectArray = new ComObject[count];
                for (int index = 0; index < count; ++index)
                  comObjectArray[index] = new ComObject(((IntPtr*) (void*) this.variantValue.recordValue.RecordPointer)[index]);
                return (object) comObjectArray;
              case VariantElementType.Bool:
                Bool[] boolArray = new Bool[count];
                Utilities.Read<Bool>(this.variantValue.recordValue.RecordPointer, boolArray, 0, count);
                return (object) Utilities.ConvertToBoolArray(boolArray);
              case VariantElementType.Byte:
                sbyte[] data6 = new sbyte[count];
                Utilities.Read<sbyte>(this.variantValue.recordValue.RecordPointer, data6, 0, count);
                return (object) data6;
              case VariantElementType.UByte:
                byte[] data7 = new byte[count];
                Utilities.Read<byte>(this.variantValue.recordValue.RecordPointer, data7, 0, count);
                return (object) data7;
              case VariantElementType.UShort:
                ushort[] data8 = new ushort[count];
                Utilities.Read<ushort>(this.variantValue.recordValue.RecordPointer, data8, 0, count);
                return (object) data8;
              case VariantElementType.UInt:
              case VariantElementType.UInt1:
                uint[] data9 = new uint[count];
                Utilities.Read<uint>(this.variantValue.recordValue.RecordPointer, data9, 0, count);
                return (object) data9;
              case VariantElementType.Long:
                long[] data10 = new long[count];
                Utilities.Read<long>(this.variantValue.recordValue.RecordPointer, data10, 0, count);
                return (object) data10;
              case VariantElementType.ULong:
                ulong[] data11 = new ulong[count];
                Utilities.Read<ulong>(this.variantValue.recordValue.RecordPointer, data11, 0, count);
                return (object) data11;
              case VariantElementType.Pointer:
              case VariantElementType.IntPointer:
                IntPtr[] data12 = new IntPtr[count];
                Utilities.Read<IntPtr>(this.variantValue.recordValue.RecordPointer, data12, 0, count);
                return (object) data12;
              case VariantElementType.StringPointer:
                string[] strArray1 = new string[count];
                for (int index = 0; index < count; ++index)
                  strArray1[index] = Marshal.PtrToStringAnsi(((IntPtr*) (void*) this.variantValue.recordValue.RecordPointer)[index]);
                return (object) strArray1;
              case VariantElementType.WStringPointer:
                string[] strArray2 = new string[count];
                for (int index = 0; index < count; ++index)
                  strArray2[index] = Marshal.PtrToStringUni(((IntPtr*) (void*) this.variantValue.recordValue.RecordPointer)[index]);
                return (object) strArray2;
              default:
                return (object) null;
            }
          default:
            return (object) null;
        }
      }
      set
      {
        if (value == null)
        {
          this.Type = VariantType.Default;
          this.ElementType = VariantElementType.Null;
        }
        else
        {
          Type type = value.GetType();
          this.Type = VariantType.Default;
          if (type.IsPrimitive)
          {
            if (type == typeof (int))
            {
              this.ElementType = VariantElementType.Int;
              this.variantValue.intValue = (int) value;
              return;
            }
            else if (type == typeof (uint))
            {
              this.ElementType = VariantElementType.UInt;
              this.variantValue.uintValue = (uint) value;
              return;
            }
            else if (type == typeof (long))
            {
              this.ElementType = VariantElementType.Long;
              this.variantValue.longValue = (long) value;
              return;
            }
            else if (type == typeof (ulong))
            {
              this.ElementType = VariantElementType.ULong;
              this.variantValue.ulongValue = (ulong) value;
              return;
            }
            else if (type == typeof (short))
            {
              this.ElementType = VariantElementType.Short;
              this.variantValue.shortValue = (short) value;
              return;
            }
            else if (type == typeof (ushort))
            {
              this.ElementType = VariantElementType.UShort;
              this.variantValue.ushortValue = (ushort) value;
              return;
            }
            else if (type == typeof (float))
            {
              this.ElementType = VariantElementType.Float;
              this.variantValue.floatValue = (float) value;
              return;
            }
            else if (type == typeof (double))
            {
              this.ElementType = VariantElementType.Double;
              this.variantValue.doubleValue = (double) value;
              return;
            }
          }
          else if (value is ComObject)
          {
            this.ElementType = VariantElementType.ComUnknown;
            this.variantValue.pointerValue = ((CppObject) value).NativePointer;
            return;
          }
          throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Type [{0}] is not handled", new object[1]
          {
            (object) type.Name
          }));
        }
      }
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct VariantValue
    {
      [FieldOffset(0)]
      public byte byteValue;
      [FieldOffset(0)]
      public sbyte signedByteValue;
      [FieldOffset(0)]
      public ushort ushortValue;
      [FieldOffset(0)]
      public short shortValue;
      [FieldOffset(0)]
      public uint uintValue;
      [FieldOffset(0)]
      public int intValue;
      [FieldOffset(0)]
      public ulong ulongValue;
      [FieldOffset(0)]
      public long longValue;
      [FieldOffset(0)]
      public float floatValue;
      [FieldOffset(0)]
      public double doubleValue;
      [FieldOffset(0)]
      public IntPtr pointerValue;
      [FieldOffset(0)]
      public Variant.VariantValue.CurrencyValue currencyValue;
      [FieldOffset(0)]
      public Variant.VariantValue.RecordValue recordValue;

      public struct CurrencyLowHigh
      {
        public uint LowValue;
        public int HighValue;
      }

      [StructLayout(LayoutKind.Explicit)]
      public struct CurrencyValue
      {
        [FieldOffset(0)]
        public Variant.VariantValue.CurrencyLowHigh LowHigh;
        [FieldOffset(0)]
        public long longValue;
      }

      public struct RecordValue
      {
        public IntPtr RecordInfo;
        public IntPtr RecordPointer;
      }
    }
  }
}
