// Type: ContentSerialization.Properties.Resources
// Assembly: ContentSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 458E2D00-F22E-495B-9E0A-A944909C9571
// Assembly location: F:\Program Files (x86)\FEZ\ContentSerialization.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ContentSerialization.Properties
{
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
  [DebuggerNonUserCode]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Resources.resourceMan, (object) null))
          Resources.resourceMan = new ResourceManager("ContentSerialization.Properties.Resources", typeof (Resources).Assembly);
        return Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Resources.resourceCulture;
      }
      set
      {
        Resources.resourceCulture = value;
      }
    }

    internal static string IllegalCollectionStructure
    {
      get
      {
        return Resources.ResourceManager.GetString("IllegalCollectionStructure", Resources.resourceCulture);
      }
    }

    internal static string MissingNonOptionalTagOrAttribute
    {
      get
      {
        return Resources.ResourceManager.GetString("MissingNonOptionalTagOrAttribute", Resources.resourceCulture);
      }
    }

    internal static string SimpleTypeRequired
    {
      get
      {
        return Resources.ResourceManager.GetString("SimpleTypeRequired", Resources.resourceCulture);
      }
    }

    internal Resources()
    {
    }
  }
}
