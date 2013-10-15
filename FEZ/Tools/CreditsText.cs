// Type: FezGame.Tools.CreditsText
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Tools;
using System.Collections.Generic;

namespace FezGame.Tools
{
  public static class CreditsText
  {
    private static readonly Dictionary<string, string> Fallback = ServiceHelper.Get<IContentManagerProvider>().Global.Load<Dictionary<string, Dictionary<string, string>>>("Resources/CreditsText")[string.Empty];

    static CreditsText()
    {
    }

    public static string GetString(string tag)
    {
      string str;
      if (tag == null || !CreditsText.Fallback.TryGetValue(tag, out str))
        return "[MISSING TEXT]";
      else
        return str;
    }
  }
}
