// Type: FezEngine.Structure.TrileSet
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization;
using ContentSerialization.Attributes;
using FezEngine.Effects.Structures;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class TrileSet : IDeserializationCallback, IDisposable
  {
    public string Name { get; set; }

    public Dictionary<int, Trile> Triles { get; set; }

    [Serialization(Ignore = true)]
    public Texture2D TextureAtlas { get; set; }

    public Trile this[int id]
    {
      get
      {
        return this.Triles[id];
      }
      set
      {
        this.Triles[id] = value;
      }
    }

    public TrileSet()
    {
      this.Triles = new Dictionary<int, Trile>();
    }

    public void OnDeserialization()
    {
      foreach (int index in this.Triles.Keys)
      {
        this.Triles[index].TrileSet = this;
        this.Triles[index].Id = index;
      }
    }

    public void Dispose()
    {
      TextureExtensions.Unhook((Texture) this.TextureAtlas);
      this.TextureAtlas.Dispose();
      foreach (Trile trile in this.Triles.Values)
        trile.Dispose();
    }
  }
}
