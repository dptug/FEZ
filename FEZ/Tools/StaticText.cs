// Type: FezGame.Tools.StaticText
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Tools;
using System.Collections.Generic;

namespace FezGame.Tools
{
  public static class StaticText
  {
    private static readonly Dictionary<string, Dictionary<string, string>> AllResources = ServiceHelper.Get<IContentManagerProvider>().Global.Load<Dictionary<string, Dictionary<string, string>>>("Resources/StaticText");
    private static readonly Dictionary<string, string> Fallback = StaticText.AllResources[string.Empty];

    static StaticText()
    {
    }

    public static bool TryGetString(string tag, out string text)
    {
      string letterIsoLanguageName = Culture.TwoLetterISOLanguageName;
      Dictionary<string, string> dictionary;
      if (!StaticText.AllResources.TryGetValue(letterIsoLanguageName, out dictionary))
        dictionary = StaticText.Fallback;
      if (tag != null && dictionary.TryGetValue(tag, out text) || tag != null && StaticText.Fallback.TryGetValue(tag, out text))
        return true;
      text = "[MISSING TEXT]";
      return false;
    }

    public static string GetString(string tag)
    {
      string text;
      if (StaticText.TryGetString(tag, out text))
        return text;
      else
        return "[MISSING TEXT]";
    }
  }
}
