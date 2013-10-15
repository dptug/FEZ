// Type: Microsoft.Xna.Framework.Content.ContentReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Xna.Framework.Content
{
  public sealed class ContentReader : BinaryReader
  {
    private ContentManager contentManager;
    private Action<IDisposable> recordDisposableObject;
    private ContentTypeReaderManager typeReaderManager;
    private GraphicsDevice graphicsDevice;
    private string assetName;
    private List<KeyValuePair<int, Action<object>>> sharedResourceFixups;
    private ContentTypeReader[] typeReaders;
    internal int version;
    internal int sharedResourceCount;

    internal ContentTypeReader[] TypeReaders
    {
      get
      {
        return this.typeReaders;
      }
    }

    internal GraphicsDevice GraphicsDevice
    {
      get
      {
        return this.graphicsDevice;
      }
    }

    public ContentManager ContentManager
    {
      get
      {
        return this.contentManager;
      }
    }

    public string AssetName
    {
      get
      {
        return this.assetName;
      }
    }

    internal ContentReader(ContentManager manager, Stream stream, GraphicsDevice graphicsDevice, string assetName, int version, Action<IDisposable> recordDisposableObject)
      : base(stream)
    {
      this.graphicsDevice = graphicsDevice;
      this.recordDisposableObject = recordDisposableObject;
      this.contentManager = manager;
      this.assetName = assetName;
      this.version = version;
    }

    internal object ReadAsset<T>()
    {
      this.InitializeTypeReaders();
      object obj = (object) this.ReadObject<T>();
      this.ReadSharedResources();
      return obj;
    }

    internal void InitializeTypeReaders()
    {
      this.typeReaderManager = new ContentTypeReaderManager(this);
      this.typeReaders = this.typeReaderManager.LoadAssetReaders();
      foreach (ContentTypeReader contentTypeReader in this.typeReaders)
        contentTypeReader.Initialize(this.typeReaderManager);
      this.sharedResourceCount = this.Read7BitEncodedInt();
      this.sharedResourceFixups = new List<KeyValuePair<int, Action<object>>>();
    }

    internal void ReadSharedResources()
    {
      if (this.sharedResourceCount <= 0)
        return;
      object[] objArray = new object[this.sharedResourceCount];
      for (int index = 0; index < this.sharedResourceCount; ++index)
      {
        int num = this.Read7BitEncodedInt();
        if (num > 0)
        {
          ContentTypeReader typeReader = this.typeReaders[num - 1];
          objArray[index] = this.ReadObject<object>(typeReader);
        }
        else
          objArray[index] = (object) null;
      }
      foreach (KeyValuePair<int, Action<object>> keyValuePair in this.sharedResourceFixups)
        keyValuePair.Value(objArray[keyValuePair.Key]);
    }

    public T ReadExternalReference<T>()
    {
      string str = this.ReadString();
      if (string.IsNullOrEmpty(str))
        return default (T);
      char newChar = Path.DirectorySeparatorChar;
      string relativeUri = str.Replace('\\', newChar);
      return this.contentManager.Load<T>(new Uri(new Uri("file:///" + this.assetName.Replace('\\', newChar)), relativeUri).LocalPath.Substring(1));
    }

    public Matrix ReadMatrix()
    {
      return new Matrix()
      {
        M11 = this.ReadSingle(),
        M12 = this.ReadSingle(),
        M13 = this.ReadSingle(),
        M14 = this.ReadSingle(),
        M21 = this.ReadSingle(),
        M22 = this.ReadSingle(),
        M23 = this.ReadSingle(),
        M24 = this.ReadSingle(),
        M31 = this.ReadSingle(),
        M32 = this.ReadSingle(),
        M33 = this.ReadSingle(),
        M34 = this.ReadSingle(),
        M41 = this.ReadSingle(),
        M42 = this.ReadSingle(),
        M43 = this.ReadSingle(),
        M44 = this.ReadSingle()
      };
    }

    private void RecordDisposable<T>(T result)
    {
      IDisposable disposable = (object) result as IDisposable;
      if (disposable == null)
        return;
      if (this.recordDisposableObject != null)
        this.recordDisposableObject(disposable);
      else
        this.contentManager.RecordDisposable(disposable);
    }

    public T ReadObject<T>()
    {
      int num = this.Read7BitEncodedInt();
      if (num == 0)
        return default (T);
      T result = (T) this.typeReaders[num - 1].Read(this, (object) default (T));
      this.RecordDisposable<T>(result);
      return result;
    }

    public T ReadObject<T>(ContentTypeReader typeReader)
    {
      T result = (T) typeReader.Read(this, (object) default (T));
      this.RecordDisposable<T>(result);
      return result;
    }

    public T ReadObject<T>(T existingInstance)
    {
      int num = this.Read7BitEncodedInt();
      if (num == 0)
        return default (T);
      T result = (T) this.typeReaders[num - 1].Read(this, (object) existingInstance);
      this.RecordDisposable<T>(result);
      return result;
    }

    public T ReadObject<T>(ContentTypeReader typeReader, T existingInstance)
    {
      if (!typeReader.TargetType.IsValueType)
        return (T) this.ReadObject<object>();
      T result = (T) typeReader.Read(this, (object) existingInstance);
      this.RecordDisposable<T>(result);
      return result;
    }

    public Quaternion ReadQuaternion()
    {
      return new Quaternion()
      {
        X = this.ReadSingle(),
        Y = this.ReadSingle(),
        Z = this.ReadSingle(),
        W = this.ReadSingle()
      };
    }

    public T ReadRawObject<T>()
    {
      return this.ReadRawObject<T>(default (T));
    }

    public T ReadRawObject<T>(ContentTypeReader typeReader)
    {
      return this.ReadRawObject<T>(typeReader, default (T));
    }

    public T ReadRawObject<T>(T existingInstance)
    {
      Type type = typeof (T);
      foreach (ContentTypeReader typeReader in this.typeReaders)
      {
        if (typeReader.TargetType == type)
          return this.ReadRawObject<T>(typeReader, existingInstance);
      }
      throw new NotSupportedException();
    }

    public T ReadRawObject<T>(ContentTypeReader typeReader, T existingInstance)
    {
      return (T) typeReader.Read(this, (object) existingInstance);
    }

    public void ReadSharedResource<T>(Action<T> fixup)
    {
      int num = this.Read7BitEncodedInt();
      if (num <= 0)
        return;
      this.sharedResourceFixups.Add(new KeyValuePair<int, Action<object>>(num - 1, (Action<object>) (v =>
      {
        if (!(v is T))
          throw new ContentLoadException(string.Format("Error loading shared resource. Expected type {0}, received type {1}", (object) typeof (T).Name, (object) v.GetType().Name));
        fixup((T) v);
      })));
    }

    public Vector2 ReadVector2()
    {
      return new Vector2()
      {
        X = this.ReadSingle(),
        Y = this.ReadSingle()
      };
    }

    public Vector3 ReadVector3()
    {
      return new Vector3()
      {
        X = this.ReadSingle(),
        Y = this.ReadSingle(),
        Z = this.ReadSingle()
      };
    }

    public Vector4 ReadVector4()
    {
      return new Vector4()
      {
        X = this.ReadSingle(),
        Y = this.ReadSingle(),
        Z = this.ReadSingle(),
        W = this.ReadSingle()
      };
    }

    public Color ReadColor()
    {
      return new Color()
      {
        R = this.ReadByte(),
        G = this.ReadByte(),
        B = this.ReadByte(),
        A = this.ReadByte()
      };
    }

    internal new int Read7BitEncodedInt()
    {
      return base.Read7BitEncodedInt();
    }

    internal BoundingSphere ReadBoundingSphere()
    {
      return new BoundingSphere(this.ReadVector3(), this.ReadSingle());
    }
  }
}
