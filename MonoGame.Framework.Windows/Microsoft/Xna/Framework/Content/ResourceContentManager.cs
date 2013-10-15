// Type: Microsoft.Xna.Framework.Content.ResourceContentManager
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
