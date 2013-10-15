// Type: FezEngine.Readers.ExtensibleReader`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FezEngine.Readers
{
  public class ExtensibleReader<T> : IDisposable
  {
    public readonly Dictionary<string, ContentTypeReader> ReaderReplacements = new Dictionary<string, ContentTypeReader>();
    private readonly Func<int> mReadHeader;
    private readonly Action<int> mReadSharedResources;
    private readonly FieldInfo fTypeReaders;
    private readonly ContentReader BaseReader;
    private readonly Stream Stream;

    public ExtensibleReader(ContentManager manager, Stream stream, string assetName)
    {
      this.Stream = stream;
      Type type = typeof (ContentReader);
      this.BaseReader = (ContentReader) type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[5]
      {
        typeof (ContentManager),
        typeof (Stream),
        typeof (string),
        typeof (Action<IDisposable>),
        typeof (int)
      }, (ParameterModifier[]) null).Invoke(new object[5]
      {
        (object) manager,
        (object) this.Stream,
        (object) assetName,
        (object) (Action<IDisposable>) (id => {}),
        (object) 0
      });
      this.mReadHeader = (Func<int>) Delegate.CreateDelegate(typeof (Func<int>), (object) this.BaseReader, type.GetMethod("ReadHeader", BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[0], (ParameterModifier[]) null));
      this.mReadSharedResources = (Action<int>) Delegate.CreateDelegate(typeof (Action<int>), (object) this.BaseReader, type.GetMethod("ReadSharedResources", BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[1]
      {
        typeof (int)
      }, (ParameterModifier[]) null));
      this.fTypeReaders = type.GetField("typeReaders", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    private void ReplaceReaders()
    {
      ContentTypeReader[] contentTypeReaderArray = (ContentTypeReader[]) this.fTypeReaders.GetValue((object) this.BaseReader);
      for (int index = 0; index < contentTypeReaderArray.Length; ++index)
      {
        ContentTypeReader contentTypeReader;
        if (this.ReaderReplacements.TryGetValue(contentTypeReaderArray[index].GetType().Name, out contentTypeReader))
          contentTypeReaderArray[index] = contentTypeReader;
      }
    }

    public T Read()
    {
      int num = this.mReadHeader();
      this.ReplaceReaders();
      T obj = this.BaseReader.ReadObject<T>();
      this.mReadSharedResources(num);
      return obj;
    }

    public void Dispose()
    {
      this.BaseReader.Close();
      this.Stream.Dispose();
    }
  }
}
