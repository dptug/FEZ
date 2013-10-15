// Type: FezEngine.Readers.ShaderInstancedIndexedPrimitivesReader`2
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Readers
{
  public class ShaderInstancedIndexedPrimitivesReader<TemplateType, InstanceType> : ContentTypeReader<ShaderInstancedIndexedPrimitives<TemplateType, InstanceType>> where TemplateType : struct, IShaderInstantiatableVertex where InstanceType : struct
  {
    protected override ShaderInstancedIndexedPrimitives<TemplateType, InstanceType> Read(ContentReader input, ShaderInstancedIndexedPrimitives<TemplateType, InstanceType> existingInstance)
    {
      PrimitiveType type = input.ReadObject<PrimitiveType>();
      if (existingInstance == null)
        existingInstance = new ShaderInstancedIndexedPrimitives<TemplateType, InstanceType>(type, typeof (InstanceType) == typeof (Matrix) ? 60 : 220);
      else if (existingInstance.PrimitiveType != type)
        existingInstance.PrimitiveType = type;
      existingInstance.NeedsEffectCommit = true;
      existingInstance.Vertices = input.ReadObject<TemplateType[]>(existingInstance.Vertices);
      ushort[] numArray1 = input.ReadObject<ushort[]>();
      int[] numArray2 = existingInstance.Indices = new int[numArray1.Length];
      for (int index = 0; index < numArray1.Length; ++index)
        numArray2[index] = (int) numArray1[index];
      return existingInstance;
    }
  }
}
