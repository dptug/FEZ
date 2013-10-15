// Type: NVorbis.ACache
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;
using System.Collections.Generic;

namespace NVorbis
{
  internal static class ACache
  {
    private static readonly Dictionary<ACache.BufferDescriptor, Stack<ACache.IBufferStorage>> buffers = new Dictionary<ACache.BufferDescriptor, Stack<ACache.IBufferStorage>>();

    static ACache()
    {
    }

    internal static T[] Get<T>(int elements)
    {
      return ACache.Get<T>(elements, true);
    }

    internal static T[] Get<T>(int elements, bool clearFirst)
    {
      ACache.BufferDescriptor key = new ACache.BufferDescriptor(typeof (T), elements);
      Stack<ACache.IBufferStorage> stack;
      if (!ACache.buffers.TryGetValue(key, out stack))
        ACache.buffers.Add(key, stack = new Stack<ACache.IBufferStorage>());
      T[] objArray = stack.Count != 0 ? ((ACache.BufferStorage<T>) stack.Pop()).Buffer : new T[elements];
      if (clearFirst)
      {
        for (int index = 0; index < elements; ++index)
          objArray[index] = default (T);
      }
      return objArray;
    }

    internal static T[][] Get<T>(int firstRankSize, int secondRankSize)
    {
      T[][] objArray = ACache.Get<T[]>(firstRankSize, false);
      for (int index = 0; index < firstRankSize; ++index)
        objArray[index] = ACache.Get<T>(secondRankSize, true);
      return objArray;
    }

    internal static T[][][] Get<T>(int firstRankSize, int secondRankSize, int thirdRankSize)
    {
      T[][][] objArray = ACache.Get<T[][]>(firstRankSize, false);
      for (int index = 0; index < firstRankSize; ++index)
        objArray[index] = ACache.Get<T>(secondRankSize, thirdRankSize);
      return objArray;
    }

    internal static void Return<T>(ref T[] buffer)
    {
      ACache.BufferDescriptor key = new ACache.BufferDescriptor(typeof (T), buffer.Length);
      Stack<ACache.IBufferStorage> stack;
      if (!ACache.buffers.TryGetValue(key, out stack))
        throw new InvalidOperationException("Returning a buffer that's never been taken!");
      stack.Push((ACache.IBufferStorage) new ACache.BufferStorage<T>(buffer));
    }

    internal static void Return<T>(ref T[][] buffer)
    {
      for (int index = 0; index < buffer.Length; ++index)
      {
        if (buffer[index] != null)
          ACache.Return<T>(ref buffer[index]);
      }
      ACache.Return<T[]>(ref buffer);
    }

    internal static void Return<T>(ref T[][][] buffer)
    {
      for (int index = 0; index < buffer.Length; ++index)
      {
        if (buffer[index] != null)
          ACache.Return<T>(ref buffer[index]);
      }
      ACache.Return<T[][]>(ref buffer);
    }

    private struct BufferDescriptor : IEquatable<ACache.BufferDescriptor>
    {
      private readonly Type Type;
      private readonly int Elements;

      public BufferDescriptor(Type type, int elements)
      {
        this.Type = type;
        this.Elements = elements;
      }

      public override int GetHashCode()
      {
        return Hashing.CombineHashCodes(this.Type.GetHashCode(), this.Elements.GetHashCode());
      }

      public bool Equals(ACache.BufferDescriptor other)
      {
        if (other.Type == this.Type)
          return other.Elements == this.Elements;
        else
          return false;
      }

      public override bool Equals(object other)
      {
        if (other is ACache.BufferDescriptor)
          return ((ACache.BufferDescriptor) other).Equals(this);
        else
          return false;
      }
    }

    private interface IBufferStorage
    {
    }

    private struct BufferStorage<T> : ACache.IBufferStorage
    {
      public readonly T[] Buffer;

      public BufferStorage(T[] buffer)
      {
        this.Buffer = buffer;
      }
    }
  }
}
