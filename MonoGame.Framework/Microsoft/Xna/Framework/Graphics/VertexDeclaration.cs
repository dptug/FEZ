// Type: Microsoft.Xna.Framework.Graphics.VertexDeclaration
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using MonoGame.Utilities;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
  public class VertexDeclaration : GraphicsResource
  {
    private Dictionary<int, VertexDeclaration.VertexDeclarationAttributeInfo> shaderAttributeInfo = new Dictionary<int, VertexDeclaration.VertexDeclarationAttributeInfo>();
    private VertexElement[] _elements;
    private int _vertexStride;

    internal int HashKey { get; private set; }

    public int VertexStride
    {
      get
      {
        return this._vertexStride;
      }
    }

    public VertexDeclaration(params VertexElement[] elements)
      : this(VertexDeclaration.GetVertexStride(elements), elements)
    {
    }

    public VertexDeclaration(int vertexStride, params VertexElement[] elements)
    {
      if (elements == null || elements.Length == 0)
        throw new ArgumentNullException("elements", "Elements cannot be empty");
      this._elements = (VertexElement[]) elements.Clone();
      this._vertexStride = vertexStride;
      string s = string.Empty;
      foreach (VertexElement vertexElement in this._elements)
        s = s + (object) vertexElement;
      this.HashKey = Hash.ComputeHash(Encoding.UTF8.GetBytes(s));
    }

    private static int GetVertexStride(VertexElement[] elements)
    {
      int num1 = 0;
      for (int index = 0; index < elements.Length; ++index)
      {
        int num2 = elements[index].Offset + GraphicsExtensions.GetTypeSize(elements[index].VertexElementFormat);
        if (num1 < num2)
          num1 = num2;
      }
      return num1;
    }

    internal static VertexDeclaration FromType(Type vertexType)
    {
      if (vertexType == (Type) null)
        throw new ArgumentNullException("vertexType", "Cannot be null");
      if (!vertexType.IsValueType)
        throw new ArgumentException("vertexType", "Must be value type");
      IVertexType vertexType1 = Activator.CreateInstance(vertexType) as IVertexType;
      if (vertexType1 == null)
        throw new ArgumentException("vertexData does not inherit IVertexType");
      VertexDeclaration vertexDeclaration = vertexType1.VertexDeclaration;
      if (vertexDeclaration == null)
        throw new Exception("VertexDeclaration cannot be null");
      else
        return vertexDeclaration;
    }

    public VertexElement[] GetVertexElements()
    {
      return (VertexElement[]) this._elements.Clone();
    }

    internal void Apply(Shader shader, IntPtr offset)
    {
      int hashCode = shader.GetHashCode();
      VertexDeclaration.VertexDeclarationAttributeInfo declarationAttributeInfo;
      if (!this.shaderAttributeInfo.TryGetValue(hashCode, out declarationAttributeInfo))
      {
        declarationAttributeInfo = new VertexDeclaration.VertexDeclarationAttributeInfo(this.GraphicsDevice.MaxVertexAttributes);
        foreach (VertexElement element in this._elements)
        {
          int attribLocation = shader.GetAttribLocation(element.VertexElementUsage, element.UsageIndex);
          if (attribLocation >= 0)
          {
            declarationAttributeInfo.Elements.Add(new VertexDeclaration.VertexDeclarationAttributeInfo.Element()
            {
              Offset = element.Offset,
              AttributeLocation = attribLocation,
              NumberOfElements = GraphicsExtensions.OpenGLNumberOfElements(element.VertexElementFormat),
              VertexAttribPointerType = GraphicsExtensions.OpenGLVertexAttribPointerType(element.VertexElementFormat),
              Normalized = GraphicsExtensions.OpenGLVertexAttribNormalized(element)
            });
            declarationAttributeInfo.EnabledAttributes[attribLocation] = true;
          }
        }
        this.shaderAttributeInfo.Add(hashCode, declarationAttributeInfo);
      }
      foreach (VertexDeclaration.VertexDeclarationAttributeInfo.Element element in declarationAttributeInfo.Elements)
        GL.VertexAttribPointer(element.AttributeLocation, element.NumberOfElements, element.VertexAttribPointerType, element.Normalized, this.VertexStride, (IntPtr) (offset.ToInt64() + (long) element.Offset));
      this.GraphicsDevice.SetVertexAttributeArray(declarationAttributeInfo.EnabledAttributes);
    }

    private class VertexDeclarationAttributeInfo
    {
      internal bool[] EnabledAttributes;
      internal List<VertexDeclaration.VertexDeclarationAttributeInfo.Element> Elements;

      internal VertexDeclarationAttributeInfo(int maxVertexAttributes)
      {
        this.EnabledAttributes = new bool[maxVertexAttributes];
        this.Elements = new List<VertexDeclaration.VertexDeclarationAttributeInfo.Element>();
      }

      internal class Element
      {
        public int Offset;
        public int AttributeLocation;
        public int NumberOfElements;
        public VertexAttribPointerType VertexAttribPointerType;
        public bool Normalized;
      }
    }
  }
}
