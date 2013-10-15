// Type: Microsoft.Xna.Framework.Content.ContentSerializerAttribute
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
