// Type: FezEngine.Tools.Culture
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;
using System.Globalization;

namespace FezEngine.Tools
{
  public static class Culture
  {
    public static Language Language = Culture.LanguageFromCurrentCulture();

    public static bool IsCJK
    {
      get
      {
        return Culture.IsCjk(Culture.Language);
      }
    }

    public static string TwoLetterISOLanguageName
    {
      get
      {
        switch (Culture.Language)
        {
          case Language.English:
            return "en";
          case Language.French:
            return "fr";
          case Language.Italian:
            return "it";
          case Language.German:
            return "de";
          case Language.Spanish:
            return "es";
          case Language.Portuguese:
            return "pt";
          case Language.Chinese:
            return "zh";
          case Language.Japanese:
            return "ja";
          case Language.Korean:
            return "ko";
          default:
            throw new InvalidOperationException("Unknown culture");
        }
      }
    }

    static Culture()
    {
    }

    public static Language LanguageFromCurrentCulture()
    {
      switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
      {
        case "fr":
          return Language.French;
        case "it":
          return Language.Italian;
        case "de":
          return Language.German;
        case "es":
          return Language.Spanish;
        case "pt":
          return Language.Portuguese;
        case "zh":
          return Language.Chinese;
        case "ja":
          return Language.Japanese;
        case "ko":
          return Language.Korean;
        default:
          return Language.English;
      }
    }

    public static bool IsCjk(this Language language)
    {
      if (language != Language.Japanese && language != Language.Korean)
        return language == Language.Chinese;
      else
        return true;
    }
  }
}
