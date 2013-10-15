// Type: Microsoft.Xna.Framework.Content.ContentTypeReaderManager
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microsoft.Xna.Framework.Content
{
  public sealed class ContentTypeReaderManager
  {
    private static bool falseflag = false;
    private static Dictionary<string, Func<ContentTypeReader>> typeCreators = new Dictionary<string, Func<ContentTypeReader>>();
    private static string assemblyName = Assembly.GetExecutingAssembly().FullName;
    private ContentReader _reader;
    private ContentTypeReader[] contentReaders;

    static ContentTypeReaderManager()
    {
    }

    public ContentTypeReaderManager(ContentReader reader)
    {
      this._reader = reader;
    }

    public ContentTypeReader GetTypeReader(Type targetType)
    {
      foreach (ContentTypeReader contentTypeReader in this.contentReaders)
      {
        if (targetType == contentTypeReader.TargetType)
          return contentTypeReader;
      }
      return (ContentTypeReader) null;
    }

    internal ContentTypeReader[] LoadAssetReaders()
    {
      if (ContentTypeReaderManager.falseflag)
      {
        ByteReader byteReader = new ByteReader();
        SByteReader sbyteReader = new SByteReader();
        DateTimeReader dateTimeReader = new DateTimeReader();
        DecimalReader decimalReader = new DecimalReader();
        BoundingSphereReader boundingSphereReader = new BoundingSphereReader();
        BoundingFrustumReader boundingFrustumReader = new BoundingFrustumReader();
        RayReader rayReader = new RayReader();
        ListReader<char> listReader1 = new ListReader<char>();
        ListReader<Rectangle> listReader2 = new ListReader<Rectangle>();
        ArrayReader<Rectangle> arrayReader1 = new ArrayReader<Rectangle>();
        ListReader<Vector3> listReader3 = new ListReader<Vector3>();
        ListReader<StringReader> listReader4 = new ListReader<StringReader>();
        ListReader<int> listReader5 = new ListReader<int>();
        SpriteFontReader spriteFontReader = new SpriteFontReader();
        Texture2DReader texture2Dreader = new Texture2DReader();
        CharReader charReader = new CharReader();
        RectangleReader rectangleReader = new RectangleReader();
        StringReader stringReader = new StringReader();
        Vector2Reader vector2Reader = new Vector2Reader();
        Vector3Reader vector3Reader = new Vector3Reader();
        Vector4Reader vector4Reader = new Vector4Reader();
        CurveReader curveReader = new CurveReader();
        IndexBufferReader indexBufferReader = new IndexBufferReader();
        BoundingBoxReader boundingBoxReader = new BoundingBoxReader();
        MatrixReader matrixReader = new MatrixReader();
        BasicEffectReader basicEffectReader = new BasicEffectReader();
        VertexBufferReader vertexBufferReader = new VertexBufferReader();
        AlphaTestEffectReader testEffectReader = new AlphaTestEffectReader();
        EnumReader<SpriteEffects> enumReader1 = new EnumReader<SpriteEffects>();
        ArrayReader<float> arrayReader2 = new ArrayReader<float>();
        ArrayReader<Vector2> arrayReader3 = new ArrayReader<Vector2>();
        ListReader<Vector2> listReader6 = new ListReader<Vector2>();
        ArrayReader<Matrix> arrayReader4 = new ArrayReader<Matrix>();
        EnumReader<Blend> enumReader2 = new EnumReader<Blend>();
        NullableReader<Rectangle> nullableReader = new NullableReader<Rectangle>();
      }
      int length = this._reader.Read7BitEncodedInt();
      this.contentReaders = new ContentTypeReader[length];
      for (int index = 0; index < length; ++index)
      {
        string str = this._reader.ReadString();
        Func<ContentTypeReader> func;
        if (ContentTypeReaderManager.typeCreators.TryGetValue(str, out func))
        {
          this.contentReaders[index] = func();
        }
        else
        {
          string typeName = ContentTypeReaderManager.PrepareType(str);
          Type type = Type.GetType(typeName);
          if (type != (Type) null)
          {
            try
            {
              this.contentReaders[index] = ContentExtensions.GetDefaultConstructor(type).Invoke((object[]) null) as ContentTypeReader;
            }
            catch (TargetInvocationException ex)
            {
              throw new InvalidOperationException("Failed to get default constructor for ContentTypeReader. To work around, add a creation function to ContentTypeReaderManager.AddTypeCreator() with the following failed type string: " + str);
            }
          }
          else
            throw new ContentLoadException("Could not find matching content reader of type " + str + " (" + typeName + ")");
        }
        this._reader.ReadInt32();
      }
      return this.contentReaders;
    }

    public static string PrepareType(string type)
    {
      int num = type.Split(new string[1]
      {
        "[["
      }, StringSplitOptions.None).Length - 1;
      string input = type;
      for (int index = 0; index < num; ++index)
        input = Regex.Replace(input, "\\[(.+?), Version=.+?\\]", "[$1]");
      if (input.Contains("PublicKeyToken"))
        input = Regex.Replace(input, "(.+?), Version=.+?$", "$1");
      return input.Replace(", Microsoft.Xna.Framework.Graphics", string.Format(", {0}", (object) ContentTypeReaderManager.assemblyName)).Replace(", Microsoft.Xna.Framework", string.Format(", {0}", (object) ContentTypeReaderManager.assemblyName)).Replace(", FezContentPipeline", ", FezEngine");
    }

    public static void AddTypeCreator(string typeString, Func<ContentTypeReader> createFunction)
    {
      if (ContentTypeReaderManager.typeCreators.ContainsKey(typeString))
        return;
      ContentTypeReaderManager.typeCreators.Add(typeString, createFunction);
    }

    public static void ClearTypeCreators()
    {
      ContentTypeReaderManager.typeCreators.Clear();
    }
  }
}
