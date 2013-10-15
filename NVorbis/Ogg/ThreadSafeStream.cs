// Type: NVorbis.Ogg.ThreadSafeStream
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace NVorbis.Ogg
{
  internal class ThreadSafeStream : Stream
  {
    private Stream _baseStream;
    private LinkedList<ThreadSafeStream.Node> _positions;
    private long _length;
    private object _streamLock;
    private ReaderWriterLockSlim _nodeLock;
    private int _totalHitCounter;

    public override bool CanRead
    {
      get
      {
        return this._baseStream.CanRead;
      }
    }

    public override bool CanSeek
    {
      get
      {
        return this._baseStream.CanSeek;
      }
    }

    public override bool CanWrite
    {
      get
      {
        return this._baseStream.CanWrite;
      }
    }

    public override long Length
    {
      get
      {
        return this._length;
      }
    }

    public override long Position
    {
      get
      {
        return this.GetNode().Position;
      }
      set
      {
        this.Seek(value, SeekOrigin.Begin);
      }
    }

    internal ThreadSafeStream(Stream baseStream)
    {
      if (!baseStream.CanSeek)
        throw new ArgumentException("The stream must be seekable.", "baseStream");
      this._baseStream = baseStream;
      this._positions = new LinkedList<ThreadSafeStream.Node>();
      this._streamLock = new object();
      this._nodeLock = new ReaderWriterLockSlim();
      this._length = baseStream.Length;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        this._positions.Clear();
        if (this._nodeLock != null)
        {
          this._nodeLock.Dispose();
          this._nodeLock = (ReaderWriterLockSlim) null;
        }
      }
      base.Dispose(disposing);
    }

    public override void Flush()
    {
      lock (this._streamLock)
        this._baseStream.Flush();
    }

    private ThreadSafeStream.Node GetNode()
    {
      if (Interlocked.Increment(ref this._totalHitCounter) % 50 == 0)
      {
        bool flag = false;
        this._nodeLock.EnterUpgradeableReadLock();
        try
        {
          LinkedListNode<ThreadSafeStream.Node> linkedListNode = this._positions.First;
          while (linkedListNode != null)
          {
            LinkedListNode<ThreadSafeStream.Node> node = linkedListNode;
            while (node.Previous != null && node.Previous.Value.HitCount < linkedListNode.Value.HitCount)
              node = node.Previous;
            if (node != linkedListNode)
            {
              LinkedListNode<ThreadSafeStream.Node> next = linkedListNode.Next;
              if (!flag)
              {
                this._nodeLock.EnterWriteLock();
                flag = true;
              }
              this._positions.Remove(linkedListNode);
              this._positions.AddBefore(node, linkedListNode);
              linkedListNode = next;
            }
            else
              linkedListNode = linkedListNode.Next;
          }
        }
        finally
        {
          if (flag)
            this._nodeLock.ExitWriteLock();
          this._nodeLock.ExitUpgradeableReadLock();
        }
      }
      Thread currentThread = Thread.CurrentThread;
      this._nodeLock.EnterReadLock();
      try
      {
        for (LinkedListNode<ThreadSafeStream.Node> linkedListNode = this._positions.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
        {
          if (linkedListNode.Value.Thread == currentThread)
          {
            ++linkedListNode.Value.HitCount;
            return linkedListNode.Value;
          }
        }
      }
      finally
      {
        this._nodeLock.ExitReadLock();
      }
      LinkedListNode<ThreadSafeStream.Node> node1 = new LinkedListNode<ThreadSafeStream.Node>(new ThreadSafeStream.Node()
      {
        Thread = currentThread,
        Position = 0L,
        HitCount = 1
      });
      this._nodeLock.EnterWriteLock();
      try
      {
        this._positions.AddLast(node1);
      }
      finally
      {
        this._nodeLock.ExitWriteLock();
      }
      return node1.Value;
    }

    public override int ReadByte()
    {
      byte[] buffer = ACache.Get<byte>(1);
      try
      {
        if (this.Read(buffer, 0, 1) == 0)
          return -1;
        else
          return (int) buffer[0];
      }
      finally
      {
        ACache.Return<byte>(ref buffer);
      }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      ThreadSafeStream.Node node = this.GetNode();
      int num;
      lock (this._streamLock)
      {
        if (this._baseStream.Position != node.Position)
          this._baseStream.Position = node.Position;
        num = this._baseStream.Read(buffer, offset, count);
      }
      node.Position += (long) num;
      return num;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      if (!this.CanSeek)
        throw new InvalidOperationException();
      ThreadSafeStream.Node node = this.GetNode();
      long num = 0L;
      switch (origin)
      {
        case SeekOrigin.Begin:
          num = offset;
          break;
        case SeekOrigin.Current:
          num = node.Position + offset;
          break;
        case SeekOrigin.End:
          num = this.Length + offset;
          break;
      }
      if (num < 0L || num > this.Length)
        throw new ArgumentOutOfRangeException("offset");
      else
        return node.Position = num;
    }

    public override void SetLength(long value)
    {
      lock (this._streamLock)
      {
        this._baseStream.SetLength(value);
        this._length = value;
      }
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      ThreadSafeStream.Node node = this.GetNode();
      lock (this._streamLock)
      {
        if (this._baseStream.Position != node.Position)
          this._baseStream.Position = node.Position;
        this._baseStream.Write(buffer, offset, count);
        this._length = this._baseStream.Length;
      }
      node.Position += (long) count;
    }

    private class Node
    {
      public Thread Thread;
      public long Position;
      public int HitCount;
    }
  }
}
