// Type: Microsoft.Xna.Framework.Graphics.EffectParameter
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
      throw new NotImplementedException();
    }

    public bool[] GetValueBooleanArray()
    {
      throw new NotImplementedException();
    }

    public int GetValueInt32()
    {
      return (int) this.Data;
    }

    public int[] GetValueInt32Array()
    {
      throw new NotImplementedException();
    }

    public Matrix GetValueMatrix()
    {
      throw new NotImplementedException();
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
      throw new NotImplementedException();
    }

    public Texture2D GetValueTexture2D()
    {
      return (Texture2D) this.Data;
    }

    public TextureCube GetValueTextureCube()
    {
      throw new NotImplementedException();
    }

    public Vector2 GetValueVector2()
    {
      float[] numArray = (float[]) this.Data;
      return new Vector2(numArray[0], numArray[1]);
    }

    public Vector2[] GetValueVector2Array()
    {
      throw new NotImplementedException();
    }

    public Vector3 GetValueVector3()
    {
      float[] numArray = (float[]) this.Data;
      return new Vector3(numArray[0], numArray[1], numArray[2]);
    }

    public Vector3[] GetValueVector3Array()
    {
      throw new NotImplementedException();
    }

    public Vector4 GetValueVector4()
    {
      float[] numArray = (float[]) this.Data;
      return new Vector4(numArray[0], numArray[1], numArray[2], numArray[3]);
    }

    public Vector4[] GetValueVector4Array()
    {
      throw new NotImplementedException();
    }

    public void SetValue(bool value)
    {
      throw new NotImplementedException();
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
      float[] numArray = Matrix.ToFloatArray(Matrix.Transpose(value));
      for (int index1 = 0; index1 < this.RowCount; ++index1)
      {
        for (int index2 = 0; index2 < this.ColumnCount; ++index2)
          ((float[]) this.Data)[index1 * this.ColumnCount + index2] = numArray[index1 * 4 + index2];
      }
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Matrix[] value)
    {
      float[] numArray = this.Data as float[];
      if (this.Data == null || numArray.Length < value.Length * 16)
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
      for (int index = 0; index < value.Length; ++index)
        this.Elements[index].SetValue(value[index]);
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(string value)
    {
      throw new NotImplementedException();
    }

    public void SetValue(Texture value)
    {
      this.Data = (object) value;
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Vector2 value)
    {
      this.Data = (object) new float[2]
      {
        value.X,
        value.Y
      };
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
      this.Data = (object) new float[3]
      {
        value.X,
        value.Y,
        value.Z
      };
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
      this.Data = (object) new float[4]
      {
        value.X,
        value.Y,
        value.Z,
        value.W
      };
      this.StateKey = EffectParameter.NextStateKey++;
    }

    public void SetValue(Vector4[] value)
    {
      float[] numArray = this.Data as float[];
      if (this.Data == null || numArray.Length < value.Length * 4)
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
