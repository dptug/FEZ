// Type: Microsoft.Xna.Framework.Graphics.Effect
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using MonoGame.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Xna.Framework.Graphics
{
  public class Effect : GraphicsResource
  {
    private static readonly Dictionary<int, Effect> EffectCache = new Dictionary<int, Effect>();
    private List<Shader> _shaderList = new List<Shader>();
    private const string MGFXHeader = "MGFX";
    private const int MGFXVersion = 3;
    private const int MinSupportedMGFXVersion = 2;
    private readonly bool _isClone;
    private int version;

    public EffectParameterCollection Parameters { get; private set; }

    public EffectTechniqueCollection Techniques { get; private set; }

    public EffectTechnique CurrentTechnique { get; set; }

    internal ConstantBuffer[] ConstantBuffers { get; private set; }

    static Effect()
    {
    }

    internal Effect(GraphicsDevice graphicsDevice)
    {
      if (graphicsDevice == null)
        throw new ArgumentNullException("Graphics Device Cannot Be Null");
      this.GraphicsDevice = graphicsDevice;
    }

    protected Effect(Effect cloneSource)
      : this(cloneSource.GraphicsDevice)
    {
      this._isClone = true;
      this.Clone(cloneSource);
    }

    public Effect(GraphicsDevice graphicsDevice, byte[] effectCode)
      : this(graphicsDevice)
    {
      int hash = Hash.ComputeHash(effectCode);
      Effect cloneSource;
      if (!Effect.EffectCache.TryGetValue(hash, out cloneSource))
      {
        cloneSource = new Effect(graphicsDevice);
        using (MemoryStream memoryStream = new MemoryStream(effectCode))
        {
          using (BinaryReader reader = new BinaryReader((Stream) memoryStream))
            cloneSource.ReadEffect(reader);
        }
        Effect.EffectCache.Add(hash, cloneSource);
      }
      this._isClone = true;
      this.Clone(cloneSource);
    }

    private void Clone(Effect cloneSource)
    {
      this.Parameters = new EffectParameterCollection(cloneSource.Parameters);
      this.Techniques = new EffectTechniqueCollection(this, cloneSource.Techniques);
      this.ConstantBuffers = new ConstantBuffer[cloneSource.ConstantBuffers.Length];
      for (int index = 0; index < cloneSource.ConstantBuffers.Length; ++index)
        this.ConstantBuffers[index] = new ConstantBuffer(cloneSource.ConstantBuffers[index]);
      for (int index = 0; index < cloneSource.Techniques.Count; ++index)
      {
        if (cloneSource.Techniques[index] == cloneSource.CurrentTechnique)
        {
          this.CurrentTechnique = this.Techniques[index];
          break;
        }
      }
      this._shaderList = cloneSource._shaderList;
    }

    public virtual Effect Clone()
    {
      return new Effect(this);
    }

    public void End()
    {
    }

    protected internal virtual bool OnApply()
    {
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed && !this._isClone)
      {
        foreach (GraphicsResource graphicsResource in this._shaderList)
          graphicsResource.Dispose();
      }
      base.Dispose(disposing);
    }

    protected internal new virtual void GraphicsDeviceResetting()
    {
      for (int index = 0; index < this.ConstantBuffers.Length; ++index)
        this.ConstantBuffers[index].Clear();
    }

    internal static byte[] LoadEffectResource(string name)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        typeof (Effect).Assembly.GetManifestResourceStream(name).CopyTo((Stream) memoryStream);
        return memoryStream.ToArray();
      }
    }

    private void ReadEffect(BinaryReader reader)
    {
      string str = new string(reader.ReadChars("MGFX".Length));
      this.version = (int) reader.ReadByte();
      if (str != "MGFX" || this.version < 2)
        throw new Exception("Unsupported MGFX format!");
      if ((int) reader.ReadByte() != 0)
        throw new Exception("The MGFX effect is the wrong profile for this platform!");
      int length = (int) reader.ReadByte();
      this.ConstantBuffers = new ConstantBuffer[length];
      for (int index1 = 0; index1 < length; ++index1)
      {
        string name = reader.ReadString();
        int sizeInBytes = (int) reader.ReadInt16();
        int[] parameterIndexes = new int[(int) reader.ReadByte()];
        int[] parameterOffsets = new int[parameterIndexes.Length];
        for (int index2 = 0; index2 < parameterIndexes.Length; ++index2)
        {
          parameterIndexes[index2] = (int) reader.ReadByte();
          parameterOffsets[index2] = (int) reader.ReadUInt16();
        }
        ConstantBuffer constantBuffer = new ConstantBuffer(this.GraphicsDevice, sizeInBytes, parameterIndexes, parameterOffsets, name);
        this.ConstantBuffers[index1] = constantBuffer;
      }
      this._shaderList = new List<Shader>();
      int num1 = (int) reader.ReadByte();
      for (int index = 0; index < num1; ++index)
        this._shaderList.Add(new Shader(this.GraphicsDevice, reader));
      this.Parameters = this.ReadParameters(reader);
      this.Techniques = new EffectTechniqueCollection();
      int num2 = (int) reader.ReadByte();
      for (int index = 0; index < num2; ++index)
      {
        string name = reader.ReadString();
        EffectAnnotationCollection annotations = Effect.ReadAnnotations(reader);
        EffectPassCollection passes = Effect.ReadPasses(reader, this, this._shaderList);
        this.Techniques.Add(new EffectTechnique(this, name, passes, annotations));
      }
      this.CurrentTechnique = this.Techniques[0];
    }

    private static EffectAnnotationCollection ReadAnnotations(BinaryReader reader)
    {
      EffectAnnotationCollection annotationCollection = new EffectAnnotationCollection();
      if ((int) reader.ReadByte() == 0)
        return annotationCollection;
      else
        return annotationCollection;
    }

    private static EffectPassCollection ReadPasses(BinaryReader reader, Effect effect, List<Shader> shaders)
    {
      Shader vertexShader = (Shader) null;
      Shader pixelShader = (Shader) null;
      EffectPassCollection effectPassCollection = new EffectPassCollection();
      int num = (int) reader.ReadByte();
      for (int index1 = 0; index1 < num; ++index1)
      {
        string name = reader.ReadString();
        EffectAnnotationCollection annotations = Effect.ReadAnnotations(reader);
        int index2 = (int) reader.ReadByte();
        if (index2 != (int) byte.MaxValue)
          vertexShader = shaders[index2];
        int index3 = (int) reader.ReadByte();
        if (index3 != (int) byte.MaxValue)
          pixelShader = shaders[index3];
        EffectPass pass = new EffectPass(effect, name, vertexShader, pixelShader, (BlendState) null, (DepthStencilState) null, (RasterizerState) null, annotations);
        effectPassCollection.Add(pass);
      }
      return effectPassCollection;
    }

    private EffectParameterCollection ReadParameters(BinaryReader reader)
    {
      EffectParameterCollection parameterCollection = new EffectParameterCollection();
      int num = (int) reader.ReadByte();
      if (num == 0)
        return parameterCollection;
      for (int index1 = 0; index1 < num; ++index1)
      {
        EffectParameterClass class_ = (EffectParameterClass) reader.ReadByte();
        EffectParameterType type = (EffectParameterType) reader.ReadByte();
        string name = reader.ReadString();
        string semantic = reader.ReadString();
        EffectAnnotationCollection annotations = Effect.ReadAnnotations(reader);
        int rowCount = (int) reader.ReadByte();
        int columnCount = (int) reader.ReadByte();
        int registerCount = this.version >= 3 ? (int) reader.ReadByte() : rowCount;
        EffectParameterCollection elements = this.ReadParameters(reader);
        EffectParameterCollection structMembers = this.ReadParameters(reader);
        object data = (object) null;
        if (elements.Count == 0 && structMembers.Count == 0)
        {
          switch (type)
          {
            case EffectParameterType.Bool:
            case EffectParameterType.Int32:
              int[] numArray1 = new int[rowCount * columnCount];
              for (int index2 = 0; index2 < numArray1.Length; ++index2)
                numArray1[index2] = reader.ReadInt32();
              data = (object) numArray1;
              break;
            case EffectParameterType.Single:
              float[] numArray2 = new float[rowCount * columnCount];
              for (int index2 = 0; index2 < numArray2.Length; ++index2)
                numArray2[index2] = reader.ReadSingle();
              data = (object) numArray2;
              break;
            case EffectParameterType.String:
              throw new NotImplementedException();
          }
        }
        EffectParameter effectParameter = new EffectParameter(class_, type, name, rowCount, columnCount, registerCount, semantic, annotations, elements, structMembers, data);
        parameterCollection.Add(effectParameter);
      }
      return parameterCollection;
    }

    internal static void FlushCache()
    {
      foreach (KeyValuePair<int, Effect> keyValuePair in Effect.EffectCache)
        keyValuePair.Value.Dispose();
      Effect.EffectCache.Clear();
    }
  }
}
