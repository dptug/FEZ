// Type: Microsoft.Xna.Framework.Content.ResourceContentManager
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.IO;
using System.Resources;

namespace Microsoft.Xna.Framework.Content
{
  public class ResourceContentManager : ContentManager
  {
    private ResourceManager resource;

    public ResourceContentManager(IServiceProvider servicesProvider, ResourceManager resource)
      : base(servicesProvider)
    {
      if (resource == null)
        throw new ArgumentNullException("resource");
      this.resource = resource;
    }

    protected override Stream OpenStream(string assetName)
    {
      object @object = this.resource.GetObject(assetName);
      if (@object == null)
        throw new ContentLoadException("Resource not found");
      if (!(@object is byte[]))
        throw new ContentLoadException("Resource is not in binary format");
      else
        return (Stream) new MemoryStream(@object as byte[]);
    }
  }
}
