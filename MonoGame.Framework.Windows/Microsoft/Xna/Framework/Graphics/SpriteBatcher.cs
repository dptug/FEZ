// Type: Microsoft.Xna.Framework.Graphics.SpriteBatcher
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  internal class SpriteBatcher
  {
    private const int InitialBatchSize = 256;
    private const int InitialVertexArraySize = 256;
    private readonly List<SpriteBatchItem> _batchItemList;
    private readonly Queue<SpriteBatchItem> _freeBatchItemQueue;
    private readonly GraphicsDevice _device;
    private short[] _index;
    private VertexPositionColorTexture[] _vertexArray;

    public SpriteBatcher(GraphicsDevice device)
    {
      this._device = device;
      this._batchItemList = new List<SpriteBatchItem>(256);
      this._freeBatchItemQueue = new Queue<SpriteBatchItem>(256);
      this._index = new short[1536];
      for (int index = 0; index < 256; ++index)
      {
        this._index[index * 6] = (short) (index * 4);
        this._index[index * 6 + 1] = (short) (index * 4 + 1);
        this._index[index * 6 + 2] = (short) (index * 4 + 2);
        this._index[index * 6 + 3] = (short) (index * 4 + 1);
        this._index[index * 6 + 4] = (short) (index * 4 + 3);
        this._index[index * 6 + 5] = (short) (index * 4 + 2);
      }
      this._vertexArray = new VertexPositionColorTexture[1024];
    }

    public SpriteBatchItem CreateBatchItem()
    {
      SpriteBatchItem spriteBatchItem = this._freeBatchItemQueue.Count <= 0 ? new SpriteBatchItem() : this._freeBatchItemQueue.Dequeue();
      this._batchItemList.Add(spriteBatchItem);
      return spriteBatchItem;
    }

    private static int CompareTexture(SpriteBatchItem a, SpriteBatchItem b)
    {
      return object.ReferenceEquals((object) a.Texture, (object) b.Texture) ? 0 : 1;
    }

    private static int CompareDepth(SpriteBatchItem a, SpriteBatchItem b)
    {
      return a.Depth.CompareTo(b.Depth);
    }

    private static int CompareReverseDepth(SpriteBatchItem a, SpriteBatchItem b)
    {
      return b.Depth.CompareTo(a.Depth);
    }

    public void DrawBatch(SpriteSortMode sortMode)
    {
      if (this._batchItemList.Count == 0)
        return;
      switch (sortMode)
      {
        case SpriteSortMode.Texture:
          this._batchItemList.Sort(new Comparison<SpriteBatchItem>(SpriteBatcher.CompareTexture));
          break;
        case SpriteSortMode.BackToFront:
          this._batchItemList.Sort(new Comparison<SpriteBatchItem>(SpriteBatcher.CompareReverseDepth));
          break;
        case SpriteSortMode.FrontToBack:
          this._batchItemList.Sort(new Comparison<SpriteBatchItem>(SpriteBatcher.CompareDepth));
          break;
      }
      int start = 0;
      int end = 0;
      Texture2D texture2D = (Texture2D) null;
      if (this._batchItemList.Count * 4 > this._vertexArray.Length)
        this.ExpandVertexArray(this._batchItemList.Count);
      foreach (SpriteBatchItem spriteBatchItem in this._batchItemList)
      {
        if (spriteBatchItem.Texture != texture2D)
        {
          this.FlushVertexArray(start, end);
          texture2D = spriteBatchItem.Texture;
          start = end = 0;
          this._device.Textures[0] = (Texture) texture2D;
        }
        this._vertexArray[end++] = spriteBatchItem.vertexTL;
        this._vertexArray[end++] = spriteBatchItem.vertexTR;
        this._vertexArray[end++] = spriteBatchItem.vertexBL;
        this._vertexArray[end++] = spriteBatchItem.vertexBR;
        spriteBatchItem.Texture = (Texture2D) null;
        this._freeBatchItemQueue.Enqueue(spriteBatchItem);
      }
      this.FlushVertexArray(start, end);
      this._batchItemList.Clear();
    }

    private void ExpandVertexArray(int batchSize)
    {
      int num = this._vertexArray.Length / 4;
      while (batchSize * 4 > num)
        num += 128;
      this._index = new short[6 * num];
      for (int index = 0; index < num; ++index)
      {
        this._index[index * 6] = (short) (index * 4);
        this._index[index * 6 + 1] = (short) (index * 4 + 1);
        this._index[index * 6 + 2] = (short) (index * 4 + 2);
        this._index[index * 6 + 3] = (short) (index * 4 + 1);
        this._index[index * 6 + 4] = (short) (index * 4 + 3);
        this._index[index * 6 + 5] = (short) (index * 4 + 2);
      }
      this._vertexArray = new VertexPositionColorTexture[4 * num];
    }

    private void FlushVertexArray(int start, int end)
    {
      if (start == end)
        return;
      int numVertices = end - start;
      this._device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, this._vertexArray, 0, numVertices, this._index, 0, numVertices / 4 * 2, VertexPositionColorTexture.VertexDeclaration);
    }
  }
}
