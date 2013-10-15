// Type: SharpDX.Serialization.BinarySerializer
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using SharpDX.IO;
using SharpDX.Multimedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SharpDX.Serialization
{
  public class BinarySerializer
  {
    private static readonly List<BinarySerializer.Dynamic> DefaultDynamics = new List<BinarySerializer.Dynamic>()
    {
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 0,
        Type = typeof (int),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderInt),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterInt)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 1,
        Type = typeof (uint),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderUInt),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterUInt)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 2,
        Type = typeof (short),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderShort),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterShort)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 3,
        Type = typeof (ushort),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderUShort),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterUShort)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 4,
        Type = typeof (long),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderLong),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterLong)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 5,
        Type = typeof (ulong),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderULong),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterULong)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 6,
        Type = typeof (byte),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderByte),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterByte)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 7,
        Type = typeof (sbyte),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderSByte),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterSByte)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 8,
        Type = typeof (bool),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderBool),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterBool)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 9,
        Type = typeof (float),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderFloat),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterFloat)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 10,
        Type = typeof (double),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderDouble),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterDouble)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 11,
        Type = typeof (string),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderString),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterString)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 12,
        Type = typeof (char),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderChar),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterChar)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 13,
        Type = typeof (DateTime),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderDateTime),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterDateTime)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 14,
        Type = typeof (Guid),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderGuid),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterGuid)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 30,
        Type = typeof (int[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderIntArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterIntArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 31,
        Type = typeof (uint[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderUIntArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterUIntArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 32,
        Type = typeof (short[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderShortArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterShortArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 33,
        Type = typeof (ushort[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderUShortArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterUShortArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 34,
        Type = typeof (long[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderLongArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterLongArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 35,
        Type = typeof (ulong[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderULongArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterULongArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 36,
        Type = typeof (byte[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderByteArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterByteArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 37,
        Type = typeof (sbyte[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderSByteArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterSByteArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 38,
        Type = typeof (bool[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderBoolArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterBoolArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 39,
        Type = typeof (float[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderFloatArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterFloatArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 40,
        Type = typeof (double[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderDoubleArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterDoubleArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 41,
        Type = typeof (string[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderStringArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterStringArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 42,
        Type = typeof (char[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderCharArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterCharArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 43,
        Type = typeof (DateTime[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderDateTimeArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterDateTimeArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 44,
        Type = typeof (Guid[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderGuidArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterGuidArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 45,
        Type = typeof (object[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderObjectArray),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterObjectArray)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 60,
        Type = typeof (List<int>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderIntList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterIntList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 61,
        Type = typeof (List<uint>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderUIntList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterUIntList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 62,
        Type = typeof (List<short>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderShortList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterShortList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 63,
        Type = typeof (List<ushort>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderUShortList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterUShortList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 64,
        Type = typeof (List<long>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderLongList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterLongList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 65,
        Type = typeof (List<ulong>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderULongList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterULongList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 66,
        Type = typeof (List<byte>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderByteList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterByteList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 67,
        Type = typeof (List<sbyte>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderSByteList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterSByteList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 68,
        Type = typeof (List<bool>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderBoolList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterBoolList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 69,
        Type = typeof (List<float>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderFloatList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterFloatList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 70,
        Type = typeof (List<double>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderDoubleList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterDoubleList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 71,
        Type = typeof (List<string>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderStringList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterStringList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 72,
        Type = typeof (List<char>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderCharList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterCharList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 73,
        Type = typeof (List<DateTime>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderDateTimeList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterDateTimeList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 74,
        Type = typeof (List<Guid>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderGuidList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterGuidList)
      },
      new BinarySerializer.Dynamic()
      {
        Id = (FourCC) 75,
        Type = typeof (List<object>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderObjectList),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterObjectList)
      }
    };
    private const int LargeByteBufferSize = 1024;
    private int chunkCount;
    private BinarySerializer.Chunk[] chunks;
    private BinarySerializer.Chunk currentChunk;
    private readonly Dictionary<FourCC, BinarySerializer.Dynamic> dynamicMapToType;
    private readonly Dictionary<Type, BinarySerializer.Dynamic> dynamicMapToFourCC;
    private readonly Dictionary<object, int> objectToPosition;
    private readonly Dictionary<int, object> positionToObject;
    private Dictionary<object, object> mapTag;
    private int allowIdentityReferenceCount;
    private Encoding encoding;
    private Encoder encoder;
    private Decoder decoder;
    private byte[] largeByteBuffer;
    private int maxChars;
    private char[] largeCharBuffer;
    private int maxCharSize;
    private SerializerMode mode;

    public Stream Stream { get; private set; }

    public BinaryReader Reader { get; private set; }

    public BinaryWriter Writer { get; private set; }

    public ArrayLengthType ArrayLengthType { get; set; }

    public SerializerMode Mode
    {
      get
      {
        return this.mode;
      }
      set
      {
        this.mode = value;
        if (this.Mode == SerializerMode.Read)
          this.Reader = this.Reader ?? new BinaryReader(this.Stream);
        else
          this.Writer = this.Writer ?? new BinaryWriter(this.Stream);
      }
    }

    public Encoding Encoding
    {
      get
      {
        return this.encoding;
      }
      set
      {
        if (object.ReferenceEquals((object) value, (object) null))
          throw new ArgumentNullException("value");
        if (object.ReferenceEquals((object) this.encoding, (object) value))
          return;
        this.encoding = value;
        this.encoder = this.encoding.GetEncoder();
        this.decoder = this.encoding.GetDecoder();
        this.maxCharSize = this.encoding.GetMaxCharCount(1024);
      }
    }

    public bool AllowIdentity
    {
      get
      {
        return this.allowIdentityReferenceCount > 0;
      }
      set
      {
        this.allowIdentityReferenceCount += value ? 1 : -1;
        if (this.allowIdentityReferenceCount < 0)
          throw new InvalidOperationException("Invalid call to AllowIdentity. Must match true/false in pair.");
      }
    }

    private BinarySerializer.Chunk CurrentChunk
    {
      get
      {
        return this.currentChunk;
      }
      set
      {
        this.currentChunk = value;
      }
    }

    static BinarySerializer()
    {
    }

    public BinarySerializer(Stream stream, SerializerMode mode)
      : this(stream, mode, Encoding.ASCII)
    {
    }

    public BinarySerializer(Stream stream, SerializerMode mode, Encoding encoding)
    {
      this.Encoding = encoding;
      this.dynamicMapToType = new Dictionary<FourCC, BinarySerializer.Dynamic>();
      this.dynamicMapToFourCC = new Dictionary<Type, BinarySerializer.Dynamic>();
      this.objectToPosition = new Dictionary<object, int>((IEqualityComparer<object>) new IdentityEqualityComparer<object>());
      this.positionToObject = new Dictionary<int, object>();
      this.chunks = new BinarySerializer.Chunk[8];
      this.Stream = stream;
      this.Mode = mode;
      this.CurrentChunk = new BinarySerializer.Chunk()
      {
        ChunkIndexStart = 0L
      };
      this.chunks[this.chunkCount] = this.CurrentChunk;
      ++this.chunkCount;
      foreach (BinarySerializer.Dynamic dynamic in BinarySerializer.DefaultDynamics)
        this.RegisterDynamic(dynamic);
    }

    public object GetTag(object key)
    {
      if (this.mapTag == null)
        return (object) null;
      object obj;
      this.mapTag.TryGetValue(key, out obj);
      return obj;
    }

    public bool HasTag(object key)
    {
      if (this.mapTag == null)
        return false;
      else
        return this.mapTag.ContainsKey(key);
    }

    public void RemoveTag(object key)
    {
      if (this.mapTag == null)
        return;
      this.mapTag.Remove(key);
    }

    public void SetTag(object key, object value)
    {
      if (this.mapTag == null)
        this.mapTag = new Dictionary<object, object>();
      this.mapTag.Remove(key);
      this.mapTag.Add(key, value);
    }

    public void RegisterDynamic<T>() where T : IDataSerializable, new()
    {
      DynamicSerializerAttribute customAttribute = Utilities.GetCustomAttribute<DynamicSerializerAttribute>((MemberInfo) typeof (T), false);
      if (customAttribute == null)
        throw new ArgumentException("Type T doesn't have DynamicSerializerAttribute", "T");
      this.RegisterDynamic<T>(customAttribute.Id);
    }

    public void RegisterDynamic<T>(FourCC id) where T : IDataSerializable, new()
    {
      this.RegisterDynamic(BinarySerializer.GetDynamic<T>(id));
    }

    public void RegisterDynamicArray<T>(FourCC id) where T : IDataSerializable, new()
    {
      this.RegisterDynamic(BinarySerializer.GetDynamicArray<T>(id));
    }

    public void RegisterDynamicList<T>(FourCC id) where T : IDataSerializable, new()
    {
      this.RegisterDynamic(BinarySerializer.GetDynamicList<T>(id));
    }

    public void RegisterDynamic<T>(FourCC id, SerializerAction serializer) where T : new()
    {
      BinarySerializer.Dynamic dynamic = new BinarySerializer.Dynamic()
      {
        Id = id,
        Type = typeof (T),
        DynamicSerializer = serializer
      };
      dynamic.Reader = new BinarySerializer.ReadRef(dynamic.DynamicReader<T>);
      dynamic.Writer = new BinarySerializer.WriteRef(dynamic.DynamicWriter);
      this.RegisterDynamic(dynamic);
    }

    public void BeginChunk(FourCC chunkId)
    {
      if (this.chunks[this.chunkCount] == null)
      {
        this.CurrentChunk = new BinarySerializer.Chunk();
        this.chunks[this.chunkCount] = this.CurrentChunk;
      }
      else
        this.CurrentChunk = this.chunks[this.chunkCount];
      ++this.chunkCount;
      this.CurrentChunk.Id = chunkId;
      if (this.Mode == SerializerMode.Write)
        this.CurrentChunk.ChunkIndexStart = this.Stream.Position;
      if (this.chunkCount >= this.chunks.Length)
      {
        BinarySerializer.Chunk[] chunkArray = new BinarySerializer.Chunk[this.chunks.Length * 2];
        Array.Copy((Array) this.chunks, (Array) chunkArray, this.chunks.Length);
        this.chunks = chunkArray;
      }
      if (this.Mode == SerializerMode.Write)
      {
        this.Writer.Write((int) chunkId);
        this.Writer.Write(0);
      }
      else
      {
        int num = this.Reader.ReadInt32();
        if ((FourCC) num != chunkId)
          throw new InvalidChunkException((FourCC) num, chunkId);
        this.CurrentChunk.ChunkIndexEnd = this.Stream.Position + (long) this.Reader.ReadUInt32();
      }
    }

    public void EndChunk()
    {
      if (this.chunkCount <= 1)
        throw new InvalidOperationException("EndChunk() called without BeginChunk()");
      BinarySerializer.Chunk currentChunk = this.CurrentChunk;
      --this.chunkCount;
      this.CurrentChunk = this.chunks[this.chunkCount - 1];
      if (this.Mode == SerializerMode.Write)
      {
        long position = this.Stream.Position;
        this.Stream.Position = currentChunk.ChunkIndexStart + 4L;
        this.Writer.Write((uint) ((ulong) (position - this.Stream.Position) - 4UL));
        this.Stream.Position = position;
      }
      else if (currentChunk.ChunkIndexEnd != this.Stream.Position)
        throw new IOException(string.Format("Unexpected size when reading chunk [{0}]", (object) this.CurrentChunk.Id));
    }

    public T Load<T>() where T : IDataSerializable, new()
    {
      this.ResetStoredReference();
      this.Mode = SerializerMode.Read;
      T obj = default (T);
      this.Serialize<T>(ref obj, SerializeFlags.Normal);
      return obj;
    }

    public void Save<T>(T value) where T : IDataSerializable, new()
    {
      this.ResetStoredReference();
      this.Mode = SerializerMode.Write;
      this.Serialize<T>(ref value, SerializeFlags.Normal);
      this.Flush();
    }

    public void Flush()
    {
      this.Writer.Flush();
    }

    public void SerializeDynamic<T>(ref T value)
    {
      this.SerializeDynamic<T>(ref value, SerializeFlags.Dynamic | SerializeFlags.Nullable);
    }

    public void SerializeDynamic<T>(ref T value, SerializeFlags serializeFlags)
    {
      int storeObjectReference;
      if (this.SerializeIsNull<T>(ref value, out storeObjectReference, serializeFlags | SerializeFlags.Dynamic))
        return;
      this.SerializeRawDynamic<T>(ref value, false);
    }

    public void Serialize<T>(ref T value, SerializeFlags serializeFlags = SerializeFlags.Normal) where T : IDataSerializable, new()
    {
      int storeObjectReference;
      if (this.SerializeIsNull<T>(ref value, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Read)
        value = new T();
      value.Serialize(this);
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) value, storeObjectReference);
    }

    public void SerializeWithNoInstance<T>(ref T value, SerializeFlags serializeFlags = SerializeFlags.Normal) where T : IDataSerializable
    {
      int storeObjectReference;
      if (this.SerializeIsNull<T>(ref value, out storeObjectReference, serializeFlags))
        return;
      value.Serialize(this);
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) value, storeObjectReference);
    }

    public unsafe void SerializeEnum<T>(ref T value) where T : struct, IComparable, IFormattable
    {
      if (!Utilities.IsEnum(typeof (T)))
        throw new ArgumentException("T generic parameter must be a valid enum", "value");
      fixed (T* objPtr = &value)
      {
        switch (Utilities.SizeOf<T>())
        {
          case 1:
            this.Serialize(ref *(byte*) objPtr);
            break;
          case 2:
            this.Serialize(ref *(short*) objPtr);
            break;
          case 4:
            this.Serialize(ref *(int*) objPtr);
            break;
          case 8:
            this.Serialize(ref *(long*) objPtr);
            break;
        }
      }
    }

    public void Serialize<T>(ref T[] valueArray, BinarySerializer.SerializerPrimitiveAction<T> serializer, SerializeFlags serializeFlags = SerializeFlags.Normal)
    {
      int storeObjectReference;
      if (this.SerializeIsNull<T[]>(ref valueArray, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(valueArray.Length);
        for (int index = 0; index < valueArray.Length; ++index)
          serializer(ref valueArray[index]);
      }
      else
      {
        int length = this.ReadArrayLength();
        valueArray = new T[length];
        for (int index = 0; index < length; ++index)
          serializer(ref valueArray[index]);
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueArray, storeObjectReference);
    }

    public void Serialize<T>(ref T[] valueArray, int count, BinarySerializer.SerializerPrimitiveAction<T> serializer, SerializeFlags serializeFlags = SerializeFlags.Normal)
    {
      int storeObjectReference;
      if (this.SerializeIsNull<T[]>(ref valueArray, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        for (int index = 0; index < count; ++index)
          serializer(ref valueArray[index]);
      }
      else
      {
        valueArray = new T[count];
        for (int index = 0; index < count; ++index)
          serializer(ref valueArray[index]);
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueArray, storeObjectReference);
    }

    public void Serialize<T>(ref T[] valueArray, SerializeFlags serializeFlags = SerializeFlags.Normal) where T : IDataSerializable, new()
    {
      int storeObjectReference;
      if (this.SerializeIsNull<T[]>(ref valueArray, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(valueArray.Length);
        for (int index = 0; index < valueArray.Length; ++index)
          this.Serialize<T>(ref valueArray[index], serializeFlags);
      }
      else
      {
        int length = this.ReadArrayLength();
        valueArray = new T[length];
        for (int index = 0; index < length; ++index)
          this.Serialize<T>(ref valueArray[index], serializeFlags);
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueArray, storeObjectReference);
    }

    public void SerializeWithNoInstance<T>(ref T[] valueArray, SerializeFlags serializeFlags = SerializeFlags.Normal) where T : IDataSerializable
    {
      int storeObjectReference;
      if (this.SerializeIsNull<T[]>(ref valueArray, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(valueArray.Length);
        for (int index = 0; index < valueArray.Length; ++index)
          this.SerializeWithNoInstance<T>(ref valueArray[index], serializeFlags);
      }
      else
      {
        int length = this.ReadArrayLength();
        valueArray = new T[length];
        for (int index = 0; index < length; ++index)
          this.SerializeWithNoInstance<T>(ref valueArray[index], serializeFlags);
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueArray, storeObjectReference);
    }

    public void Serialize<T>(ref T[] valueArray, int count, SerializeFlags serializeFlags = SerializeFlags.Normal) where T : IDataSerializable, new()
    {
      int storeObjectReference;
      if (this.SerializeIsNull<T[]>(ref valueArray, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        for (int index = 0; index < count; ++index)
          this.Serialize<T>(ref valueArray[index], serializeFlags);
      }
      else
      {
        valueArray = new T[count];
        for (int index = 0; index < count; ++index)
          this.Serialize<T>(ref valueArray[index], serializeFlags);
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueArray, storeObjectReference);
    }

    public void Serialize(ref byte[] valueArray, SerializeFlags serializeFlags = SerializeFlags.Normal)
    {
      int storeObjectReference;
      if (this.SerializeIsNull<byte[]>(ref valueArray, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(valueArray.Length);
        this.Writer.Write(valueArray);
      }
      else
      {
        int count = this.ReadArrayLength();
        valueArray = new byte[count];
        this.Reader.Read(valueArray, 0, count);
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueArray, storeObjectReference);
    }

    public void Serialize(ref byte[] valueArray, int count, SerializeFlags serializeFlags = SerializeFlags.Normal)
    {
      int storeObjectReference;
      if (this.SerializeIsNull<byte[]>(ref valueArray, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.Writer.Write(valueArray, 0, count);
      }
      else
      {
        valueArray = new byte[count];
        this.Reader.Read(valueArray, 0, count);
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueArray, storeObjectReference);
    }

    public void Serialize<T>(ref List<T> valueList, SerializeFlags serializeFlags = SerializeFlags.Normal) where T : IDataSerializable, new()
    {
      int storeObjectReference;
      if (this.SerializeIsNull<List<T>>(ref valueList, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(valueList.Count);
        foreach (T obj in valueList)
          this.Serialize<T>(ref obj, SerializeFlags.Normal);
      }
      else
      {
        int capacity = this.ReadArrayLength();
        valueList = new List<T>(capacity);
        for (int index = 0; index < capacity; ++index)
        {
          T obj = default (T);
          this.Serialize<T>(ref obj, SerializeFlags.Normal);
          valueList.Add(obj);
        }
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueList, storeObjectReference);
    }

    public void Serialize<T>(ref List<T> valueList, BinarySerializer.SerializerPrimitiveAction<T> serializerMethod, SerializeFlags serializeFlags = SerializeFlags.Normal)
    {
      int storeObjectReference;
      if (this.SerializeIsNull<List<T>>(ref valueList, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(valueList.Count);
        foreach (T obj in valueList)
          serializerMethod(ref obj);
      }
      else
      {
        int capacity = this.ReadArrayLength();
        valueList = new List<T>(capacity);
        for (int index = 0; index < capacity; ++index)
        {
          T obj = default (T);
          serializerMethod(ref obj);
          valueList.Add(obj);
        }
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueList, storeObjectReference);
    }

    public void Serialize<T>(ref List<T> valueList, int count, SerializeFlags serializeFlags = SerializeFlags.Normal) where T : IDataSerializable, new()
    {
      int storeObjectReference;
      if (this.SerializeIsNull<List<T>>(ref valueList, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        for (int index = 0; index < count; ++index)
        {
          T obj = valueList[index];
          this.Serialize<T>(ref obj, serializeFlags);
        }
      }
      else
      {
        valueList = new List<T>(count);
        for (int index = 0; index < count; ++index)
        {
          T obj = default (T);
          this.Serialize<T>(ref obj, serializeFlags);
          valueList.Add(obj);
        }
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueList, storeObjectReference);
    }

    public void Serialize<T>(ref List<T> valueList, int count, BinarySerializer.SerializerPrimitiveAction<T> serializerMethod, SerializeFlags serializeFlags = SerializeFlags.Normal)
    {
      int storeObjectReference;
      if (this.SerializeIsNull<List<T>>(ref valueList, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        for (int index = 0; index < count; ++index)
        {
          T obj = valueList[index];
          serializerMethod(ref obj);
        }
      }
      else
      {
        valueList = new List<T>(count);
        for (int index = 0; index < count; ++index)
        {
          T obj = default (T);
          serializerMethod(ref obj);
          valueList.Add(obj);
        }
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) valueList, storeObjectReference);
    }

    public void Serialize<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary, SerializeFlags serializeFlags = SerializeFlags.Normal) where TKey : IDataSerializable, new() where TValue : IDataSerializable, new()
    {
      int storeObjectReference;
      if (this.SerializeIsNull<Dictionary<TKey, TValue>>(ref dictionary, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(dictionary.Count);
        foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
        {
          TKey key = keyValuePair.Key;
          TValue obj = keyValuePair.Value;
          key.Serialize(this);
          obj.Serialize(this);
        }
      }
      else
      {
        int capacity = this.ReadArrayLength();
        dictionary = new Dictionary<TKey, TValue>(capacity);
        for (int index = 0; index < capacity; ++index)
        {
          TKey key = default (TKey);
          TValue obj = default (TValue);
          key.Serialize(this);
          obj.Serialize(this);
        }
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) dictionary, storeObjectReference);
    }

    public void Serialize<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary, BinarySerializer.SerializerPrimitiveAction<TValue> valueSerializer, SerializeFlags serializeFlags = SerializeFlags.Normal) where TKey : IDataSerializable, new()
    {
      int storeObjectReference;
      if (this.SerializeIsNull<Dictionary<TKey, TValue>>(ref dictionary, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(dictionary.Count);
        foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
        {
          TKey key = keyValuePair.Key;
          TValue obj = keyValuePair.Value;
          key.Serialize(this);
          valueSerializer(ref obj);
        }
      }
      else
      {
        int capacity = this.ReadArrayLength();
        dictionary = new Dictionary<TKey, TValue>(capacity);
        for (int index = 0; index < capacity; ++index)
        {
          TKey key = default (TKey);
          TValue obj = default (TValue);
          key.Serialize(this);
          valueSerializer(ref obj);
        }
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) dictionary, storeObjectReference);
    }

    public void Serialize<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary, BinarySerializer.SerializerPrimitiveAction<TKey> keySerializer, SerializeFlags serializeFlags = SerializeFlags.Normal) where TValue : IDataSerializable, new()
    {
      int storeObjectReference;
      if (this.SerializeIsNull<Dictionary<TKey, TValue>>(ref dictionary, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(dictionary.Count);
        foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
        {
          TKey key = keyValuePair.Key;
          TValue obj = keyValuePair.Value;
          keySerializer(ref key);
          obj.Serialize(this);
        }
      }
      else
      {
        int capacity = this.ReadArrayLength();
        dictionary = new Dictionary<TKey, TValue>(capacity);
        for (int index = 0; index < capacity; ++index)
        {
          TKey key = default (TKey);
          TValue obj = default (TValue);
          keySerializer(ref key);
          obj.Serialize(this);
        }
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) dictionary, storeObjectReference);
    }

    public void Serialize<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary, BinarySerializer.SerializerPrimitiveAction<TKey> keySerializer, BinarySerializer.SerializerPrimitiveAction<TValue> valueSerializer, SerializeFlags serializeFlags = SerializeFlags.Normal)
    {
      int storeObjectReference;
      if (this.SerializeIsNull<Dictionary<TKey, TValue>>(ref dictionary, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
      {
        this.WriteArrayLength(dictionary.Count);
        foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
        {
          TKey key = keyValuePair.Key;
          TValue obj = keyValuePair.Value;
          keySerializer(ref key);
          valueSerializer(ref obj);
        }
      }
      else
      {
        int capacity = this.ReadArrayLength();
        dictionary = new Dictionary<TKey, TValue>(capacity);
        for (int index = 0; index < capacity; ++index)
        {
          TKey key = default (TKey);
          TValue obj = default (TValue);
          keySerializer(ref key);
          valueSerializer(ref obj);
        }
      }
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) dictionary, storeObjectReference);
    }

    public void Serialize(ref string value)
    {
      this.Serialize(ref value, false, SerializeFlags.Normal);
    }

    public void Serialize(ref string value, SerializeFlags serializeFlags)
    {
      this.Serialize(ref value, false, serializeFlags);
    }

    public void Serialize(ref string value, bool writeNullTerminatedString, SerializeFlags serializeFlags = SerializeFlags.Normal)
    {
      int storeObjectReference = -1;
      if (this.SerializeIsNull<string>(ref value, out storeObjectReference, serializeFlags))
        return;
      if (this.Mode == SerializerMode.Write)
        this.WriteString(value, writeNullTerminatedString, -1);
      else
        value = this.ReadString(writeNullTerminatedString, -1);
      if (storeObjectReference < 0)
        return;
      this.StoreObjectRef((object) value, storeObjectReference);
    }

    public void Serialize(ref string value, int len)
    {
      if (this.Mode == SerializerMode.Write)
        this.WriteString(value, false, len);
      else
        value = this.ReadString(false, len);
    }

    public void Serialize(ref bool value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadBoolean();
    }

    public void Serialize(ref byte value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadByte();
    }

    public void Serialize(ref sbyte value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadSByte();
    }

    public void Serialize(ref short value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadInt16();
    }

    public void Serialize(ref ushort value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadUInt16();
    }

    public void Serialize(ref int value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadInt32();
    }

    public void SerializePackedInt(ref int value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Write7BitEncodedInt(value);
      else
        value = this.Read7BitEncodedInt();
    }

    public void SerializeMemoryRegion(DataPointer dataRegion)
    {
      this.SerializeMemoryRegion(dataRegion.Pointer, dataRegion.Size);
    }

    public unsafe void SerializeMemoryRegion(IntPtr dataPointer, int sizeInBytes)
    {
      if (this.largeByteBuffer == null)
        this.largeByteBuffer = new byte[32768];
      if (this.largeByteBuffer.Length < 32768)
        this.largeByteBuffer = new byte[32768];
      NativeFileStream nativeFileStream = this.Stream as NativeFileStream;
      if (nativeFileStream != null)
      {
        if (this.Mode == SerializerMode.Write)
          nativeFileStream.Write(dataPointer, 0, sizeInBytes);
        else
          nativeFileStream.Read(dataPointer, 0, sizeInBytes);
      }
      else if (this.Stream is DataStream)
      {
        if (this.Mode == SerializerMode.Write)
          ((DataStream) this.Stream).Write(dataPointer, 0, sizeInBytes);
        else
          ((DataStream) this.Stream).Read(dataPointer, 0, sizeInBytes);
      }
      else if (this.Mode == SerializerMode.Write)
      {
        int num = sizeInBytes;
        while (num > 0)
        {
          int count = num < this.largeByteBuffer.Length ? num : this.largeByteBuffer.Length;
          Utilities.Read<byte>(dataPointer, this.largeByteBuffer, 0, count);
          this.Stream.Write(this.largeByteBuffer, 0, count);
          dataPointer = (IntPtr) ((void*) ((IntPtr) (void*) dataPointer + count));
          num -= count;
        }
      }
      else
      {
        int num = sizeInBytes;
        while (num > 0)
        {
          int count = num < this.largeByteBuffer.Length ? num : this.largeByteBuffer.Length;
          if (this.Stream.Read(this.largeByteBuffer, 0, count) != count)
            throw new EndOfStreamException();
          Utilities.Write<byte>(dataPointer, this.largeByteBuffer, 0, count);
          dataPointer = (IntPtr) ((void*) ((IntPtr) (void*) dataPointer + count));
          num -= count;
        }
      }
    }

    public void Serialize(ref uint value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadUInt32();
    }

    public void Serialize(ref long value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadInt64();
    }

    public void Serialize(ref ulong value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadUInt64();
    }

    public void Serialize(ref char value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadChar();
    }

    public void Serialize(ref float value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadSingle();
    }

    public void Serialize(ref double value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value);
      else
        value = this.Reader.ReadDouble();
    }

    public void Serialize(ref DateTime value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value.ToBinary());
      else
        value = new DateTime(this.Reader.ReadInt64());
    }

    public void Serialize(ref Guid value)
    {
      if (this.Mode == SerializerMode.Write)
        this.Writer.Write(value.ToByteArray());
      else
        value = new Guid(this.Reader.ReadBytes(16));
    }

    private bool SerializeIsNull<T>(ref T value, out int storeObjectReference, SerializeFlags flags)
    {
      storeObjectReference = -1;
      if (Utilities.IsValueType(typeof (T)))
        return false;
      bool flag = object.ReferenceEquals((object) value, (object) null);
      if ((flags & SerializeFlags.Nullable) != SerializeFlags.Normal || this.allowIdentityReferenceCount > 0)
      {
        if (this.Mode == SerializerMode.Write)
        {
          if (!flag && this.allowIdentityReferenceCount > 0 && (flags & SerializeFlags.Dynamic) == SerializeFlags.Normal)
          {
            int num;
            if (this.objectToPosition.TryGetValue((object) value, out num))
            {
              this.Writer.Write((byte) 2);
              this.Write7BitEncodedInt(num);
              return true;
            }
            else
              this.objectToPosition.Add((object) value, (int) this.Stream.Position);
          }
          this.Writer.Write(flag ? (byte) 0 : (byte) 1);
          return flag;
        }
        else
        {
          value = default (T);
          int num1 = (int) this.Stream.Position;
          int num2 = (int) this.Reader.ReadByte();
          switch (num2)
          {
            case 1:
              if (this.allowIdentityReferenceCount > 0 && (flags & SerializeFlags.Dynamic) == SerializeFlags.Normal)
              {
                storeObjectReference = num1;
                break;
              }
              else
                break;
            case 2:
              if (this.allowIdentityReferenceCount == 0)
                throw new InvalidOperationException("Can't read serialized reference when SerializeReference is off");
              int key = this.Read7BitEncodedInt();
              object obj;
              if (!this.positionToObject.TryGetValue(key, out obj))
                throw new InvalidOperationException(string.Format("Can't find serialized reference at position [{0}]", (object) key));
              value = (T) obj;
              num2 = 0;
              break;
          }
          return num2 == 0;
        }
      }
      else if (flag && this.Mode == SerializerMode.Write)
        throw new ArgumentNullException("value");
      else
        return false;
    }

    private void SerializeRawDynamic<T>(ref T value, bool noDynamic = false)
    {
      if (this.Mode == SerializerMode.Write)
      {
        Type key = noDynamic ? typeof (T) : value.GetType();
        BinarySerializer.Dynamic dynamic;
        if (!this.dynamicMapToFourCC.TryGetValue(key, out dynamic))
          throw new IOException(string.Format("Type [{0}] is not registered as dynamic", (object) key));
        if (!noDynamic)
          this.Writer.Write((int) dynamic.Id);
        dynamic.Writer((object) value, this);
      }
      else
      {
        BinarySerializer.Dynamic dynamic;
        if (noDynamic)
        {
          Type key = typeof (T);
          if (!this.dynamicMapToFourCC.TryGetValue(key, out dynamic))
            throw new IOException(string.Format("Type [{0}] is not registered as dynamic", (object) key));
        }
        else
        {
          FourCC key = (FourCC) this.Reader.ReadInt32();
          if (!this.dynamicMapToType.TryGetValue(key, out dynamic))
            throw new IOException(string.Format("Type [{0}] is not registered as dynamic", (object) key));
        }
        value = (T) dynamic.Reader(this);
      }
    }

    private void RegisterDynamic(BinarySerializer.Dynamic dynamic)
    {
      this.dynamicMapToFourCC.Add(dynamic.Type, dynamic);
      this.dynamicMapToType.Add(dynamic.Id, dynamic);
    }

    private static object ReaderIntArray(BinarySerializer serializer)
    {
      int[] valueArray = (int[]) null;
      serializer.Serialize<int>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<int>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderUIntArray(BinarySerializer serializer)
    {
      uint[] valueArray = (uint[]) null;
      serializer.Serialize<uint>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<uint>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderShortArray(BinarySerializer serializer)
    {
      short[] valueArray = (short[]) null;
      serializer.Serialize<short>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<short>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderUShortArray(BinarySerializer serializer)
    {
      ushort[] valueArray = (ushort[]) null;
      serializer.Serialize<ushort>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<ushort>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderLongArray(BinarySerializer serializer)
    {
      long[] valueArray = (long[]) null;
      serializer.Serialize<long>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<long>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderULongArray(BinarySerializer serializer)
    {
      ulong[] valueArray = (ulong[]) null;
      serializer.Serialize<ulong>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<ulong>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderBoolArray(BinarySerializer serializer)
    {
      bool[] valueArray = (bool[]) null;
      serializer.Serialize<bool>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<bool>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderFloatArray(BinarySerializer serializer)
    {
      float[] valueArray = (float[]) null;
      serializer.Serialize<float>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<float>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderDoubleArray(BinarySerializer serializer)
    {
      double[] valueArray = (double[]) null;
      serializer.Serialize<double>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<double>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderDateTimeArray(BinarySerializer serializer)
    {
      DateTime[] valueArray = (DateTime[]) null;
      serializer.Serialize<DateTime>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<DateTime>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderGuidArray(BinarySerializer serializer)
    {
      Guid[] valueArray = (Guid[]) null;
      serializer.Serialize<Guid>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<Guid>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderObjectArray(BinarySerializer serializer)
    {
      object[] valueArray = (object[]) null;
      serializer.Serialize<object>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<object>(serializer.SerializeDynamic<object>), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderCharArray(BinarySerializer serializer)
    {
      char[] valueArray = (char[]) null;
      serializer.Serialize<char>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<char>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderStringArray(BinarySerializer serializer)
    {
      string[] valueArray = (string[]) null;
      serializer.Serialize<string>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<string>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderByteArray(BinarySerializer serializer)
    {
      byte[] valueArray = (byte[]) null;
      serializer.Serialize(ref valueArray, SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static object ReaderSByteArray(BinarySerializer serializer)
    {
      sbyte[] valueArray = (sbyte[]) null;
      serializer.Serialize<sbyte>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<sbyte>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static void WriterIntArray(object value, BinarySerializer serializer)
    {
      int[] valueArray = (int[]) value;
      serializer.Serialize<int>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<int>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterUIntArray(object value, BinarySerializer serializer)
    {
      uint[] valueArray = (uint[]) value;
      serializer.Serialize<uint>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<uint>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterShortArray(object value, BinarySerializer serializer)
    {
      short[] valueArray = (short[]) value;
      serializer.Serialize<short>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<short>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterUShortArray(object value, BinarySerializer serializer)
    {
      ushort[] valueArray = (ushort[]) value;
      serializer.Serialize<ushort>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<ushort>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterLongArray(object value, BinarySerializer serializer)
    {
      long[] valueArray = (long[]) value;
      serializer.Serialize<long>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<long>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterULongArray(object value, BinarySerializer serializer)
    {
      ulong[] valueArray = (ulong[]) value;
      serializer.Serialize<ulong>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<ulong>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterSByteArray(object value, BinarySerializer serializer)
    {
      sbyte[] valueArray = (sbyte[]) value;
      serializer.Serialize<sbyte>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<sbyte>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterStringArray(object value, BinarySerializer serializer)
    {
      string[] valueArray = (string[]) value;
      serializer.Serialize<string>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<string>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterCharArray(object value, BinarySerializer serializer)
    {
      char[] valueArray = (char[]) value;
      serializer.Serialize<char>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<char>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterBoolArray(object value, BinarySerializer serializer)
    {
      bool[] valueArray = (bool[]) value;
      serializer.Serialize<bool>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<bool>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterFloatArray(object value, BinarySerializer serializer)
    {
      float[] valueArray = (float[]) value;
      serializer.Serialize<float>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<float>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterDoubleArray(object value, BinarySerializer serializer)
    {
      double[] valueArray = (double[]) value;
      serializer.Serialize<double>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<double>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterDateTimeArray(object value, BinarySerializer serializer)
    {
      DateTime[] valueArray = (DateTime[]) value;
      serializer.Serialize<DateTime>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<DateTime>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterGuidArray(object value, BinarySerializer serializer)
    {
      Guid[] valueArray = (Guid[]) value;
      serializer.Serialize<Guid>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<Guid>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterObjectArray(object value, BinarySerializer serializer)
    {
      object[] valueArray = (object[]) value;
      serializer.Serialize<object>(ref valueArray, new BinarySerializer.SerializerPrimitiveAction<object>(serializer.SerializeDynamic<object>), SerializeFlags.Normal);
    }

    private static void WriterByteArray(object value, BinarySerializer serializer)
    {
      byte[] valueArray = (byte[]) value;
      serializer.Serialize(ref valueArray, SerializeFlags.Normal);
    }

    private static object ReaderIntList(BinarySerializer serializer)
    {
      List<int> valueList = (List<int>) null;
      serializer.Serialize<int>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<int>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderUIntList(BinarySerializer serializer)
    {
      List<uint> valueList = (List<uint>) null;
      serializer.Serialize<uint>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<uint>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderShortList(BinarySerializer serializer)
    {
      List<short> valueList = (List<short>) null;
      serializer.Serialize<short>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<short>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderUShortList(BinarySerializer serializer)
    {
      List<ushort> valueList = (List<ushort>) null;
      serializer.Serialize<ushort>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<ushort>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderLongList(BinarySerializer serializer)
    {
      List<long> valueList = (List<long>) null;
      serializer.Serialize<long>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<long>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderULongList(BinarySerializer serializer)
    {
      List<ulong> valueList = (List<ulong>) null;
      serializer.Serialize<ulong>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<ulong>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderBoolList(BinarySerializer serializer)
    {
      List<bool> valueList = (List<bool>) null;
      serializer.Serialize<bool>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<bool>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderFloatList(BinarySerializer serializer)
    {
      List<float> valueList = (List<float>) null;
      serializer.Serialize<float>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<float>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderDoubleList(BinarySerializer serializer)
    {
      List<double> valueList = (List<double>) null;
      serializer.Serialize<double>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<double>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderDateTimeList(BinarySerializer serializer)
    {
      List<DateTime> valueList = (List<DateTime>) null;
      serializer.Serialize<DateTime>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<DateTime>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderGuidList(BinarySerializer serializer)
    {
      List<Guid> valueList = (List<Guid>) null;
      serializer.Serialize<Guid>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<Guid>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderObjectList(BinarySerializer serializer)
    {
      List<object> valueList = (List<object>) null;
      serializer.Serialize<object>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<object>(serializer.SerializeDynamic<object>), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderCharList(BinarySerializer serializer)
    {
      List<char> valueList = (List<char>) null;
      serializer.Serialize<char>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<char>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderStringList(BinarySerializer serializer)
    {
      List<string> valueList = (List<string>) null;
      serializer.Serialize<string>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<string>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderByteList(BinarySerializer serializer)
    {
      List<byte> valueList = (List<byte>) null;
      serializer.Serialize<byte>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<byte>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static object ReaderSByteList(BinarySerializer serializer)
    {
      List<sbyte> valueList = (List<sbyte>) null;
      serializer.Serialize<sbyte>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<sbyte>(serializer.Serialize), SerializeFlags.Normal);
      return (object) valueList;
    }

    private static void WriterIntList(object value, BinarySerializer serializer)
    {
      List<int> valueList = (List<int>) value;
      serializer.Serialize<int>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<int>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterUIntList(object value, BinarySerializer serializer)
    {
      List<uint> valueList = (List<uint>) value;
      serializer.Serialize<uint>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<uint>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterShortList(object value, BinarySerializer serializer)
    {
      List<short> valueList = (List<short>) value;
      serializer.Serialize<short>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<short>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterUShortList(object value, BinarySerializer serializer)
    {
      List<ushort> valueList = (List<ushort>) value;
      serializer.Serialize<ushort>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<ushort>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterLongList(object value, BinarySerializer serializer)
    {
      List<long> valueList = (List<long>) value;
      serializer.Serialize<long>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<long>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterULongList(object value, BinarySerializer serializer)
    {
      List<ulong> valueList = (List<ulong>) value;
      serializer.Serialize<ulong>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<ulong>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterSByteList(object value, BinarySerializer serializer)
    {
      List<sbyte> valueList = (List<sbyte>) value;
      serializer.Serialize<sbyte>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<sbyte>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterStringList(object value, BinarySerializer serializer)
    {
      List<string> valueList = (List<string>) value;
      serializer.Serialize<string>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<string>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterCharList(object value, BinarySerializer serializer)
    {
      List<char> valueList = (List<char>) value;
      serializer.Serialize<char>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<char>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterBoolList(object value, BinarySerializer serializer)
    {
      List<bool> valueList = (List<bool>) value;
      serializer.Serialize<bool>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<bool>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterFloatList(object value, BinarySerializer serializer)
    {
      List<float> valueList = (List<float>) value;
      serializer.Serialize<float>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<float>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterDoubleList(object value, BinarySerializer serializer)
    {
      List<double> valueList = (List<double>) value;
      serializer.Serialize<double>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<double>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterDateTimeList(object value, BinarySerializer serializer)
    {
      List<DateTime> valueList = (List<DateTime>) value;
      serializer.Serialize<DateTime>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<DateTime>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterGuidList(object value, BinarySerializer serializer)
    {
      List<Guid> valueList = (List<Guid>) value;
      serializer.Serialize<Guid>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<Guid>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static void WriterObjectList(object value, BinarySerializer serializer)
    {
      List<object> valueList = (List<object>) value;
      serializer.Serialize<object>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<object>(serializer.SerializeDynamic<object>), SerializeFlags.Normal);
    }

    private static void WriterByteList(object value, BinarySerializer serializer)
    {
      List<byte> valueList = (List<byte>) value;
      serializer.Serialize<byte>(ref valueList, new BinarySerializer.SerializerPrimitiveAction<byte>(serializer.Serialize), SerializeFlags.Normal);
    }

    private static object ReaderInt(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadInt32();
    }

    private static object ReaderUInt(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadUInt32();
    }

    private static object ReaderShort(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadInt16();
    }

    private static object ReaderUShort(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadUInt16();
    }

    private static object ReaderLong(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadInt64();
    }

    private static object ReaderULong(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadUInt64();
    }

    private static object ReaderBool(BinarySerializer serializer)
    {
      return (object) (bool) (serializer.Reader.ReadBoolean() ? 1 : 0);
    }

    private static object ReaderByte(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadByte();
    }

    private static object ReaderSByte(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadSByte();
    }

    private static object ReaderString(BinarySerializer serializer)
    {
      string str = (string) null;
      serializer.Serialize(ref str);
      return (object) str;
    }

    private static object ReaderFloat(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadSingle();
    }

    private static object ReaderDouble(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadDouble();
    }

    private static object ReaderChar(BinarySerializer serializer)
    {
      return (object) serializer.Reader.ReadChar();
    }

    private static object ReaderDateTime(BinarySerializer serializer)
    {
      return (object) new DateTime(serializer.Reader.ReadInt64());
    }

    private static object ReaderGuid(BinarySerializer serializer)
    {
      return (object) new Guid(serializer.Reader.ReadBytes(16));
    }

    private static void WriterInt(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((int) value);
    }

    private static void WriterUInt(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((uint) value);
    }

    private static void WriterShort(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((short) value);
    }

    private static void WriterUShort(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((ushort) value);
    }

    private static void WriterLong(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((long) value);
    }

    private static void WriterULong(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((ulong) value);
    }

    private static void WriterByte(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((byte) value);
    }

    private static void WriterSByte(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((sbyte) value);
    }

    private static void WriterString(object value, BinarySerializer serializer)
    {
      string str = (string) value;
      serializer.Serialize(ref str);
    }

    private static void WriterChar(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((char) value);
    }

    private static void WriterBool(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((bool) value);
    }

    private static void WriterFloat(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((float) value);
    }

    private static void WriterDouble(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write((double) value);
    }

    private static void WriterDateTime(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write(((DateTime) value).ToBinary());
    }

    private static void WriterGuid(object value, BinarySerializer serializer)
    {
      serializer.Writer.Write(((Guid) value).ToByteArray());
    }

    private static object ReaderDataSerializer<T>(BinarySerializer serializer) where T : IDataSerializable, new()
    {
      T obj = default (T);
      serializer.Serialize<T>(ref obj, SerializeFlags.Normal);
      return (object) obj;
    }

    private static void WriterDataSerializer<T>(object value, BinarySerializer serializer) where T : IDataSerializable, new()
    {
      T obj = (T) value;
      serializer.Serialize<T>(ref obj, SerializeFlags.Normal);
    }

    private static object ReaderDataSerializerArray<T>(BinarySerializer serializer) where T : IDataSerializable, new()
    {
      T[] valueArray = (T[]) null;
      serializer.Serialize<T>(ref valueArray, SerializeFlags.Normal);
      return (object) valueArray;
    }

    private static void WriterDataSerializerArray<T>(object value, BinarySerializer serializer) where T : IDataSerializable, new()
    {
      T[] valueArray = (T[]) value;
      serializer.Serialize<T>(ref valueArray, SerializeFlags.Normal);
    }

    private static object ReaderDataSerializerList<T>(BinarySerializer serializer) where T : IDataSerializable, new()
    {
      List<T> valueList = (List<T>) null;
      serializer.Serialize<T>(ref valueList, SerializeFlags.Normal);
      return (object) valueList;
    }

    private static void WriterDataSerializerList<T>(object value, BinarySerializer serializer) where T : IDataSerializable, new()
    {
      List<T> valueList = (List<T>) value;
      serializer.Serialize<T>(ref valueList, SerializeFlags.Normal);
    }

    private void StoreObjectRef(object value, int position)
    {
      this.objectToPosition.Add(value, position);
      this.positionToObject.Add(position, value);
    }

    private void ResetStoredReference()
    {
      this.positionToObject.Clear();
      this.objectToPosition.Clear();
    }

    private string ReadString(bool readNullTerminatedString, int stringLength)
    {
      int byteCount1 = 0;
      if (this.largeByteBuffer == null)
        this.largeByteBuffer = new byte[1024];
      if (this.largeCharBuffer == null)
        this.largeCharBuffer = new char[this.maxCharSize];
      string str = string.Empty;
      if (readNullTerminatedString)
      {
        byte num;
        while ((int) (num = this.Reader.ReadByte()) != 0)
        {
          if (byteCount1 == this.largeByteBuffer.Length)
          {
            byte[] numArray = new byte[this.largeByteBuffer.Length * 2];
            Array.Copy((Array) this.largeByteBuffer, 0, (Array) numArray, 0, this.largeByteBuffer.Length);
            this.largeByteBuffer = numArray;
          }
          this.largeByteBuffer[byteCount1++] = num;
        }
        int maxCharCount = this.encoding.GetMaxCharCount(byteCount1);
        if (maxCharCount > this.largeCharBuffer.Length)
          this.largeCharBuffer = new char[maxCharCount];
        str = new string(this.largeCharBuffer, 0, this.decoder.GetChars(this.largeByteBuffer, 0, byteCount1, this.largeCharBuffer, 0));
      }
      else
      {
        if (stringLength < 0)
        {
          stringLength = this.ReadArrayLength();
          if (stringLength < 0)
            throw new IOException(string.Format("Invalid string length ({0})", (object) stringLength));
        }
        if (stringLength > 0)
        {
          StringBuilder stringBuilder = (StringBuilder) null;
          do
          {
            int byteCount2 = this.Stream.Read(this.largeByteBuffer, 0, stringLength - byteCount1 > 1024 ? 1024 : stringLength - byteCount1);
            if (byteCount2 == 0)
              throw new EndOfStreamException();
            int chars = this.decoder.GetChars(this.largeByteBuffer, 0, byteCount2, this.largeCharBuffer, 0);
            if (byteCount1 == 0 && byteCount2 == stringLength)
              return new string(this.largeCharBuffer, 0, chars);
            if (stringBuilder == null)
              stringBuilder = new StringBuilder(stringLength);
            stringBuilder.Append(this.largeCharBuffer, 0, chars);
            byteCount1 += byteCount2;
          }
          while (byteCount1 < stringLength);
          str = ((object) stringBuilder).ToString();
        }
      }
      return str;
    }

    private unsafe void WriteString(string value, bool writeNullTerminated, int len)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (len < 0)
      {
        len = this.encoding.GetByteCount(value);
        if (!writeNullTerminated)
          this.WriteArrayLength(len);
      }
      else
      {
        if (value.Length != len)
          throw new ArgumentException(string.Format("length of string to serialized ({0}) != fixed length ({1})", (object) value.Length, (object) len));
        if (writeNullTerminated)
          throw new ArgumentException("Cannot use null terminated string and fixed length");
      }
      if (this.largeByteBuffer == null)
      {
        this.largeByteBuffer = new byte[1024];
        this.maxChars = 1024 / this.encoding.GetMaxByteCount(1);
      }
      if (len <= 1024)
      {
        this.encoding.GetBytes(value, 0, value.Length, this.largeByteBuffer, 0);
        this.Stream.Write(this.largeByteBuffer, 0, len);
      }
      else
      {
        int num = 0;
        int length = value.Length;
        while (length > 0)
        {
          int charCount = length > this.maxChars ? this.maxChars : length;
          int bytes1;
          fixed (char* chPtr = value)
            fixed (byte* bytes2 = this.largeByteBuffer)
              bytes1 = this.encoder.GetBytes(chPtr + num, charCount, bytes2, 1024, charCount == length);
          this.Stream.Write(this.largeByteBuffer, 0, bytes1);
          num += charCount;
          length -= charCount;
        }
      }
      if (!writeNullTerminated)
        return;
      this.Stream.WriteByte((byte) 0);
    }

    protected int ReadArrayLength()
    {
      switch (this.ArrayLengthType)
      {
        case ArrayLengthType.Dynamic:
          return this.Read7BitEncodedInt();
        case ArrayLengthType.Byte:
          return (int) this.Reader.ReadByte();
        case ArrayLengthType.UShort:
          return (int) this.Reader.ReadUInt16();
        default:
          return this.Reader.ReadInt32();
      }
    }

    protected void WriteArrayLength(int value)
    {
      switch (this.ArrayLengthType)
      {
        case ArrayLengthType.UShort:
          if (value > (int) ushort.MaxValue)
            throw new NotSupportedException(string.Format("Cannot serialize array length [{0}], larger then ArrayLengthType [{1}]", (object) value, (object) (int) ushort.MaxValue));
          this.Writer.Write((ushort) value);
          break;
        case ArrayLengthType.Int:
          if (value < 0)
            throw new NotSupportedException(string.Format("Cannot serialize array length [{0}], larger then ArrayLengthType [{1}]", (object) value, (object) 134217727));
          this.Writer.Write(value);
          break;
        case ArrayLengthType.Dynamic:
          this.Write7BitEncodedInt(value);
          break;
        case ArrayLengthType.Byte:
          if (value > (int) byte.MaxValue)
            throw new NotSupportedException(string.Format("Cannot serialize array length [{0}], larger then ArrayLengthType [{1}]", (object) value, (object) (int) byte.MaxValue));
          this.Writer.Write((byte) value);
          break;
      }
    }

    protected int Read7BitEncodedInt()
    {
      int num1 = 0;
      int num2 = 0;
      while (num2 != 35)
      {
        byte num3 = this.Reader.ReadByte();
        num1 |= ((int) num3 & (int) sbyte.MaxValue) << num2;
        num2 += 7;
        if (((int) num3 & 128) == 0)
          return num1;
      }
      throw new FormatException("Bad string length. 7bit Int32 format");
    }

    protected void Write7BitEncodedInt(int value)
    {
      uint num = (uint) value;
      while (num >= 128U)
      {
        this.Writer.Write((byte) (num | 128U));
        num >>= 7;
      }
      this.Writer.Write((byte) num);
    }

    private static BinarySerializer.Dynamic GetDynamic<T>(FourCC id) where T : IDataSerializable, new()
    {
      return new BinarySerializer.Dynamic()
      {
        Id = id,
        Type = typeof (T),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderDataSerializer<T>),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterDataSerializer<T>)
      };
    }

    private static BinarySerializer.Dynamic GetDynamicArray<T>(FourCC id) where T : IDataSerializable, new()
    {
      return new BinarySerializer.Dynamic()
      {
        Id = id,
        Type = typeof (T[]),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderDataSerializerArray<T>),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterDataSerializerArray<T>)
      };
    }

    private static BinarySerializer.Dynamic GetDynamicList<T>(FourCC id) where T : IDataSerializable, new()
    {
      return new BinarySerializer.Dynamic()
      {
        Id = id,
        Type = typeof (List<T>),
        Reader = new BinarySerializer.ReadRef(BinarySerializer.ReaderDataSerializerList<T>),
        Writer = new BinarySerializer.WriteRef(BinarySerializer.WriterDataSerializerList<T>)
      };
    }

    public delegate void SerializerPrimitiveAction<T>(ref T value);

    public delegate void SerializerTypeAction<T>(ref T value, BinarySerializer serializer);

    public delegate object ReadRef(BinarySerializer serializer);

    public delegate void WriteRef(object value, BinarySerializer serializer);

    private class Chunk
    {
      public FourCC Id;
      public long ChunkIndexStart;
      public long ChunkIndexEnd;
    }

    private class Dynamic
    {
      public FourCC Id;
      public Type Type;
      public BinarySerializer.ReadRef Reader;
      public BinarySerializer.WriteRef Writer;
      public SerializerAction DynamicSerializer;

      public object DynamicReader<T>(BinarySerializer serializer) where T : new()
      {
        object obj = (object) new T();
        this.DynamicSerializer(ref obj, serializer);
        return obj;
      }

      public void DynamicWriter(object value, BinarySerializer serializer)
      {
        this.DynamicSerializer(ref value, serializer);
      }
    }
  }
}
