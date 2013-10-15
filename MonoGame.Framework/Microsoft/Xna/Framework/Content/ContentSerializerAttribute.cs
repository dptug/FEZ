// Type: Microsoft.Xna.Framework.Content.ContentSerializerAttribute
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Content
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class ContentSerializerAttribute : Attribute
  {
    private bool allowNull;
    private string collectionItemName;
    private string elementName;
    private bool flattenContent;
    private bool hasCollectionItemName;
    private bool optional;
    private bool sharedResource;

    public bool AllowNull
    {
      get
      {
        return this.allowNull;
      }
      set
      {
        this.allowNull = value;
      }
    }

    public string CollectionItemName
    {
      get
      {
        return this.collectionItemName;
      }
      set
      {
        this.collectionItemName = value;
      }
    }

    public string ElementName
    {
      get
      {
        return this.elementName;
      }
      set
      {
        this.elementName = value;
      }
    }

    public bool FlattenContent
    {
      get
      {
        return this.flattenContent;
      }
      set
      {
        this.flattenContent = value;
      }
    }

    public bool HasCollectionItemName
    {
      get
      {
        return this.hasCollectionItemName;
      }
    }

    public bool Optional
    {
      get
      {
        return this.optional;
      }
      set
      {
        this.optional = value;
      }
    }

    public bool SharedResource
    {
      get
      {
        return this.sharedResource;
      }
      set
      {
        this.sharedResource = value;
      }
    }

    public ContentSerializerAttribute Clone()
    {
      return new ContentSerializerAttribute()
      {
        allowNull = this.allowNull,
        collectionItemName = this.collectionItemName,
        elementName = this.elementName,
        flattenContent = this.flattenContent,
        hasCollectionItemName = this.hasCollectionItemName,
        optional = this.optional,
        sharedResource = this.sharedResource
      };
    }
  }
}
