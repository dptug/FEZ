// Type: SharpDX.Multimedia.RiffParser
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SharpDX.Multimedia
{
  public class RiffParser : IEnumerator<RiffChunk>, IDisposable, IEnumerator, IEnumerable<RiffChunk>, IEnumerable
  {
    private readonly Stream input;
    private readonly long startPosition;
    private readonly BinaryReader reader;
    private readonly Stack<RiffChunk> chunckStack;
    private bool descendNext;
    private bool isEndOfRiff;
    private bool isErrorState;
    private RiffChunk current;

    public Stack<RiffChunk> ChunkStack
    {
      get
      {
        return this.chunckStack;
      }
    }

    public RiffChunk Current
    {
      get
      {
        this.CheckState();
        return this.current;
      }
    }

    object IEnumerator.Current
    {
      get
      {
        this.CheckState();
        return (object) this.Current;
      }
    }

    public RiffParser(Stream input)
    {
      this.input = input;
      this.startPosition = input.Position;
      this.reader = new BinaryReader(input);
      this.chunckStack = new Stack<RiffChunk>();
    }

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
      this.CheckState();
      if (this.current != null)
      {
        long num = (long) this.current.DataPosition;
        if (this.descendNext)
        {
          this.descendNext = false;
        }
        else
        {
          num += (long) this.current.Size;
          if ((num & 1L) != 0L)
            ++num;
        }
        this.input.Position = num;
        RiffChunk riffChunk = this.chunckStack.Peek();
        if (this.input.Position >= (long) (riffChunk.DataPosition + riffChunk.Size))
          this.chunckStack.Pop();
        if (this.chunckStack.Count == 0)
        {
          this.isEndOfRiff = true;
          return false;
        }
      }
      FourCC type = (FourCC) this.reader.ReadUInt32();
      bool isList = type == (FourCC) "LIST";
      bool isHeader = type == (FourCC) "RIFF";
      if (this.input.Position == this.startPosition + 4L && !isHeader)
      {
        this.isErrorState = true;
        throw new InvalidOperationException("Invalid RIFF file format");
      }
      else
      {
        uint size = this.reader.ReadUInt32();
        if (isList || isHeader)
        {
          if (isHeader && (long) size > this.input.Length - 8L)
          {
            this.isErrorState = true;
            throw new InvalidOperationException("Invalid RIFF file format");
          }
          else
          {
            size -= 4U;
            type = (FourCC) this.reader.ReadUInt32();
          }
        }
        this.current = new RiffChunk(this.input, type, size, (uint) this.input.Position, isList, isHeader);
        return true;
      }
    }

    private void CheckState()
    {
      if (this.isEndOfRiff)
        throw new InvalidOperationException("End of Riff. Cannot MoveNext");
      if (this.isErrorState)
        throw new InvalidOperationException("The enumerator is in an error state");
    }

    public void Reset()
    {
      this.CheckState();
      this.current = (RiffChunk) null;
      this.input.Position = this.startPosition;
    }

    public void Ascend()
    {
      this.CheckState();
      RiffChunk riffChunk = this.chunckStack.Pop();
      this.input.Position = (long) (riffChunk.DataPosition + riffChunk.Size);
    }

    public void Descend()
    {
      this.CheckState();
      this.chunckStack.Push(this.current);
      this.descendNext = true;
    }

    public IList<RiffChunk> GetAllChunks()
    {
      List<RiffChunk> list = new List<RiffChunk>();
      foreach (RiffChunk riffChunk in this)
        list.Add(riffChunk);
      return (IList<RiffChunk>) list;
    }

    public IEnumerator<RiffChunk> GetEnumerator()
    {
      return (IEnumerator<RiffChunk>) this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
