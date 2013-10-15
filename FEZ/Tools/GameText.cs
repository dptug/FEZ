// Type: FezGame.Tools.GameText
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Tools;
using System.Collections.Generic;

namespace FezGame.Tools
{
  public static class GameText
  {
    private static readonly Dictionary<string, Dictionary<string, string>> AllResources = ServiceHelper.Get<IContentManagerProvider>().Global.Load<Dictionary<string, Dictionary<string, string>>>("Resources/GameText");
    private static readonly Dictionary<string, string> Fallback = GameText.AllResources[string.Empty];

    static GameText()
    {
    }

    public static string GetString(string tag)
    {
      string letterIsoLanguageName = Culture.TwoLetterISOLanguageName;
      Dictionary<string, string> dictionary;
      if (!GameText.AllResources.TryGetValue(letterIsoLanguageName, out dictionary))
        dictionary = GameText.Fallback;
      string str;
      if ((tag == null || !dictionary.TryGetValue(tag, out str)) && (tag == null || !GameText.Fallback.TryGetValue(tag, out str)))
        return "[MISSING TEXT]";
      else
        return str;
    }

    public static string GetStringRaw(string tag)
    {
      string str;
      if (tag == null || !GameText.Fallback.TryGetValue(tag, out str))
        return "[MISSING TEXT]";
      else
        return str;
    }
  }
}
