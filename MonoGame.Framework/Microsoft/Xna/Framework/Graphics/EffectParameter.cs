// Type: Microsoft.Xna.Framework.Graphics.EffectParameter
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace Microsoft.Xna.Framework.Graphics
{
  [DebuggerDisplay("{ParameterClass} {ParameterType} {Name} : {Semantic}")]
  public class EffectParameter
  {
    internal static ulong NextStateKey { get; private set; }

    public string Name { get; private set; }

    public string Semantic { get; private set; }

    public EffectParameterClass ParameterClass { get; private set; }

    public EffectParameterType ParameterType { get; private set; }

    public int RowCount { get; private set; }

    public int ColumnCount { get; private set; }

    internal int RegisterCount { get; private set; }

    public EffectParameterCollection Elements { get; private set; }

    public EffectParameterCollection StructureMembers { get; private set; }

    public EffectAnnotationCollection Annotations { get; private set; }

    internal object Data { get; private set; }

    internal ulong StateKey { get; private set; }

    internal EffectParameter(EffectParameterClass class_, EffectParameterType type, string name, int rowCount, int columnCount, int registerCount, string semantic, EffectAnnotationCollection annotations, EffectParameterCollection elements, EffectParameterCollection structMembers, object data)
    {
      this.ParameterClass = class_;
      this.ParameterType = type;
      this.Name = name;
      this.Semantic = semantic;
      this.Annotations = annotations;
      this.RowCount = rowCount;
      this.ColumnCount = columnCount;
      this.RegisterCount = registerCount;
      this.Elements = elements;
      this.StructureMembers = structMembers;
      this.Data = data;
      this.StateKey = EffectParameter.NextStateKey++;
    }

    internal EffectParameter(EffectParameter cloneSource)
    {
      this.ParameterClass = cloneSource.ParameterClass;
      this.ParameterType = cloneSource.ParameterType;
      this.Name = cloneSource.Name;
      this.Semantic = cloneSource.Semantic;
      this.Annotations = cloneSource.Annotations;
      this.RowCount = cloneSource.RowCount;
      this.ColumnCount = cloneSource.ColumnCount;
      this.RegisterCount = cloneSource.RegisterCount;
      this.Elements = new EffectParameterCollection(cloneSource.Elements);
      this.StructureMembers = new EffectParameterCollection(cloneSource.StructureMembers);
      this.Data = cloneSource.Data is Array ? (cloneSource.Data as Array).Clone() : cloneSource.Data;
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(object value)
    {
      throw new NotImplementedException();
    }

    public bool GetValueBoolean()
    {
      if (this.ParameterClass != EffectParameterClass.Scalar || this.ParameterType != EffectParameterType.Bool)
        throw new InvalidCastException();
      else
        return (double) ((float[]) this.Data)[0] != 0.0;
    }

    public bool[] GetValueBooleanArray()
    {
      throw new NotImplementedException();
    }

    public int GetValueInt32()
    {
      if (this.ParameterClass != EffectParameterClass.Scalar || this.ParameterType != EffectParameterType.Int32)
        throw new InvalidCastException();
      else
        return (int) ((float[]) this.Data)[0];
    }

    public int[] GetValueInt32Array()
    {
      throw new NotImplementedException();
    }

    public Matrix GetValueMatrix()
    {
      if (this.RowCount != 4 || this.ColumnCount != 4)
        throw new Exception("Matrix data incorrect or missing!");
      float[] numArray = (float[]) this.Data;
      return new Matrix(numArray[0], numArray[4], numArray[8], numArray[12], numArray[1], numArray[5], numArray[9], numArray[13], numArray[2], numArray[6], numArray[10], numArray[14], numArray[3], numArray[7], numArray[11], numArray[15]);
    }

    public Matrix[] GetValueMatrixArray(int count)
    {
      throw new NotImplementedException();
    }

    public Quaternion GetValueQuaternion()
    {
      throw new NotImplementedException();
    }

    public Quaternion[] GetValueQuaternionArray()
    {
      throw new NotImplementedException();
    }

    public float GetValueSingle()
    {
      if (this.ParameterType == EffectParameterType.Int32)
        return (float) (int) this.Data;
      else
        return (float) this.Data;
    }

    public float[] GetValueSingleArray()
    {
      if (this.Elements != null && this.Elements.Count > 0)
      {
        float[] numArray = new float[this.RowCount * this.ColumnCount * this.Elements.Count];
        for (int index1 = 0; index1 < this.Elements.Count; ++index1)
        {
          float[] valueSingleArray = this.Elements[index1].GetValueSingleArray();
          for (int index2 = 0; index2 < valueSingleArray.Length; ++index2)
            numArray[this.RowCount * this.ColumnCount * index1 + index2] = valueSingleArray[index2];
        }
        return numArray;
      }
      else
      {
        switch (this.ParameterClass)
        {
          case EffectParameterClass.Scalar:
            return new float[1]
            {
              this.GetValueSingle()
            };
          case EffectParameterClass.Vector:
          case EffectParameterClass.Matrix:
            if (this.Data is Matrix)
              return Matrix.ToFloatArray((Matrix) this.Data);
            else
              return (float[]) this.Data;
          default:
            throw new NotImplementedException();
        }
      }
    }

    public string GetValueString()
    {
      if (this.ParameterClass != EffectParameterClass.Object || this.ParameterType != EffectParameterType.String)
        throw new InvalidCastException();
      else
        return ((string[]) this.Data)[0];
    }

    public Texture2D GetValueTexture2D()
    {
      if (this.ParameterClass != EffectParameterClass.Object || this.ParameterType != EffectParameterType.Texture2D)
        throw new InvalidCastException();
      else
        return (Texture2D) this.Data;
    }

    public Texture3D GetValueTexture3D()
    {
      if (this.ParameterClass != EffectParameterClass.Object || this.ParameterType != EffectParameterType.Texture3D)
        throw new InvalidCastException();
      else
        return (Texture3D) this.Data;
    }

    public TextureCube GetValueTextureCube()
    {
      throw new NotImplementedException();
    }

    public Vector2 GetValueVector2()
    {
      if (this.ParameterClass != EffectParameterClass.Vector || this.ParameterType != EffectParameterType.Single)
        throw new InvalidCastException();
      float[] numArray = (float[]) this.Data;
      return new Vector2(numArray[0], numArray[1]);
    }

    public Vector2[] GetValueVector2Array()
    {
      throw new NotImplementedException();
    }

    public Vector3 GetValueVector3()
    {
      if (this.ParameterClass != EffectParameterClass.Vector || this.ParameterType != EffectParameterType.Single)
        throw new InvalidCastException();
      float[] numArray = (float[]) this.Data;
      return new Vector3(numArray[0], numArray[1], numArray[2]);
    }

    public Vector3[] GetValueVector3Array()
    {
      throw new NotImplementedException();
    }

    public Vector4 GetValueVector4()
    {
      if (this.ParameterClass != EffectParameterClass.Vector || this.ParameterType != EffectParameterType.Single)
        throw new InvalidCastException();
      float[] numArray = (float[]) this.Data;
      return new Vector4(numArray[0], numArray[1], numArray[2], numArray[3]);
    }

    public Vector4[] GetValueVector4Array()
    {
      throw new NotImplementedException();
    }

    public void SetValue(bool value)
    {
      if (this.ParameterClass != EffectParameterClass.Scalar || this.ParameterType != EffectParameterType.Bool)
        throw new InvalidCastException();
      ((float[]) this.Data)[0] = value ? 1f : 0.0f;
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(bool[] value)
    {
      throw new NotImplementedException();
    }

    public void SetValue(int value)
    {
      this.Data = (object) value;
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(int[] value)
    {
      throw new NotImplementedException();
    }

    public void SetValue(Matrix value)
    {
      if (this.RowCount == 4 && this.ColumnCount == 4)
      {
        float[] numArray = (float[]) this.Data;
        numArray[0] = value.M11;
        numArray[1] = value.M21;
        numArray[2] = value.M31;
        numArray[3] = value.M41;
        numArray[4] = value.M12;
        numArray[5] = value.M22;
        numArray[6] = value.M32;
        numArray[7] = value.M42;
        numArray[8] = value.M13;
        numArray[9] = value.M23;
        numArray[10] = value.M33;
        numArray[11] = value.M43;
        numArray[12] = value.M14;
        numArray[13] = value.M24;
        numArray[14] = value.M34;
        numArray[15] = value.M44;
      }
      else if (this.RowCount == 4 && this.ColumnCount == 3)
      {
        float[] numArray = (float[]) this.Data;
        numArray[0] = value.M11;
        numArray[1] = value.M21;
        numArray[2] = value.M31;
        numArray[3] = value.M41;
        numArray[4] = value.M12;
        numArray[5] = value.M22;
        numArray[6] = value.M32;
        numArray[7] = value.M42;
        numArray[8] = value.M13;
        numArray[9] = value.M23;
        numArray[10] = value.M33;
        numArray[11] = value.M43;
      }
      else if (this.RowCount == 3 && this.ColumnCount == 4)
      {
        float[] numArray = (float[]) this.Data;
        numArray[0] = value.M11;
        numArray[1] = value.M21;
        numArray[2] = value.M31;
        numArray[3] = value.M12;
        numArray[4] = value.M22;
        numArray[5] = value.M32;
        numArray[6] = value.M13;
        numArray[7] = value.M23;
        numArray[8] = value.M33;
        numArray[9] = value.M14;
        numArray[10] = value.M24;
        numArray[11] = value.M34;
      }
      else if (this.RowCount == 3 && this.ColumnCount == 3)
      {
        float[] numArray = (float[]) this.Data;
        numArray[0] = value.M11;
        numArray[1] = value.M21;
        numArray[2] = value.M31;
        numArray[3] = value.M12;
        numArray[4] = value.M22;
        numArray[5] = value.M32;
        numArray[6] = value.M13;
        numArray[7] = value.M23;
        numArray[8] = value.M33;
      }
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValueTranspose(Matrix value)
    {
      if (this.RowCount == 4 && this.ColumnCount == 4)
      {
        float[] numArray = (float[]) this.Data;
        numArray[0] = value.M11;
        numArray[1] = value.M12;
        numArray[2] = value.M13;
        numArray[3] = value.M14;
        numArray[4] = value.M21;
        numArray[5] = value.M22;
        numArray[6] = value.M23;
        numArray[7] = value.M24;
        numArray[8] = value.M31;
        numArray[9] = value.M32;
        numArray[10] = value.M33;
        numArray[11] = value.M34;
        numArray[12] = value.M41;
        numArray[13] = value.M42;
        numArray[14] = value.M43;
        numArray[15] = value.M44;
      }
      else if (this.RowCount == 4 && this.ColumnCount == 3)
      {
        float[] numArray = (float[]) this.Data;
        numArray[0] = value.M11;
        numArray[1] = value.M12;
        numArray[2] = value.M13;
        numArray[3] = value.M21;
        numArray[4] = value.M22;
        numArray[5] = value.M23;
        numArray[6] = value.M31;
        numArray[7] = value.M32;
        numArray[8] = value.M33;
        numArray[9] = value.M41;
        numArray[10] = value.M42;
        numArray[11] = value.M43;
      }
      else if (this.RowCount == 3 && this.ColumnCount == 4)
      {
        float[] numArray = (float[]) this.Data;
        numArray[0] = value.M11;
        numArray[1] = value.M12;
        numArray[2] = value.M13;
        numArray[3] = value.M14;
        numArray[4] = value.M21;
        numArray[5] = value.M22;
        numArray[6] = value.M23;
        numArray[7] = value.M24;
        numArray[8] = value.M31;
        numArray[9] = value.M32;
        numArray[10] = value.M33;
        numArray[11] = value.M34;
      }
      else if (this.RowCount == 3 && this.ColumnCount == 3)
      {
        float[] numArray = (float[]) this.Data;
        numArray[0] = value.M11;
        numArray[1] = value.M12;
        numArray[2] = value.M13;
        numArray[3] = value.M21;
        numArray[4] = value.M22;
        numArray[5] = value.M23;
        numArray[6] = value.M31;
        numArray[7] = value.M32;
        numArray[8] = value.M33;
      }
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Matrix[] value)
    {
      float[] numArray = this.Data as float[];
      if (numArray == null || numArray.Length < value.Length * 16)
        this.Data = (object) (numArray = new float[value.Length * 16]);
      for (int index = 0; index < value.Length; ++index)
      {
        Matrix matrix = value[index];
        numArray[index * 16] = matrix.M11;
        numArray[index * 16 + 1] = matrix.M21;
        numArray[index * 16 + 2] = matrix.M31;
        numArray[index * 16 + 3] = matrix.M41;
        numArray[index * 16 + 4] = matrix.M12;
        numArray[index * 16 + 5] = matrix.M22;
        numArray[index * 16 + 6] = matrix.M32;
        numArray[index * 16 + 7] = matrix.M42;
        numArray[index * 16 + 8] = matrix.M13;
        numArray[index * 16 + 9] = matrix.M23;
        numArray[index * 16 + 10] = matrix.M33;
        numArray[index * 16 + 11] = matrix.M43;
        numArray[index * 16 + 12] = matrix.M14;
        numArray[index * 16 + 13] = matrix.M24;
        numArray[index * 16 + 14] = matrix.M34;
        numArray[index * 16 + 15] = matrix.M44;
      }
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Quaternion value)
    {
      throw new NotImplementedException();
    }

    public void SetValue(Quaternion[] value)
    {
      throw new NotImplementedException();
    }

    public void SetValue(float value)
    {
      switch (this.ParameterClass)
      {
        case EffectParameterClass.Scalar:
          this.Data = (object) value;
          break;
        case EffectParameterClass.Vector:
        case EffectParameterClass.Matrix:
          ((float[]) this.Data)[0] = value;
          break;
        default:
          throw new NotImplementedException();
      }
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(float[] value)
    {
      float[] numArray = this.Data as float[];
      if (numArray == null || numArray.Length < value.Length)
        this.Data = (object) (numArray = new float[value.Length]);
      Array.Copy((Array) value, (Array) numArray, value.Length);
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(string value)
    {
      throw new NotImplementedException();
    }

    public void SetValue(Texture value)
    {
      if (this.ParameterClass != EffectParameterClass.Object)
        throw new InvalidCastException();
      if (value != null)
      {
        if (value is Texture2D)
        {
          if (this.ParameterType != EffectParameterType.Texture1D && this.ParameterType != EffectParameterType.Texture2D)
            throw new InvalidCastException();
        }
        else if (value is Texture3D)
        {
          if (this.ParameterType != EffectParameterType.Texture3D)
            throw new InvalidCastException();
        }
        else if (!(value is TextureCube) || this.ParameterType != EffectParameterType.TextureCube)
          throw new InvalidCastException();
      }
      this.Data = (object) value;
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Vector2 value)
    {
      if (this.ParameterClass != EffectParameterClass.Vector || this.ParameterType != EffectParameterType.Single)
        throw new InvalidCastException();
      float[] numArray = (float[]) this.Data;
      numArray[0] = value.X;
      numArray[1] = value.Y;
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Vector2[] value)
    {
      for (int index = 0; index < value.Length; ++index)
        this.Elements[index].SetValue(value[index]);
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Vector3 value)
    {
      if (this.ParameterClass != EffectParameterClass.Vector || this.ParameterType != EffectParameterType.Single)
        throw new InvalidCastException();
      float[] numArray = (float[]) this.Data;
      numArray[0] = value.X;
      numArray[1] = value.Y;
      numArray[2] = value.Z;
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Vector3[] value)
    {
      for (int index = 0; index < value.Length; ++index)
        this.Elements[index].SetValue(value[index]);
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Vector4 value)
    {
      if (this.ParameterClass != EffectParameterClass.Vector || this.ParameterType != EffectParameterType.Single)
        throw new InvalidCastException();
      float[] numArray = (float[]) this.Data;
      numArray[0] = value.X;
      numArray[1] = value.Y;
      numArray[2] = value.Z;
      numArray[3] = value.W;
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Vector4[] value)
    {
      float[] numArray = this.Data as float[];
      if (numArray == null || numArray.Length < value.Length * 4)
        this.Data = (object) (numArray = new float[value.Length * 4]);
      for (int index = 0; index < value.Length; ++index)
      {
        numArray[index * 4] = value[index].X;
        numArray[index * 4 + 1] = value[index].Y;
        numArray[index * 4 + 2] = value[index].Z;
        numArray[index * 4 + 3] = value[index].W;
      }
      this.StateKey = EffectParameter.NextStateKey++;
    }
  }
}
