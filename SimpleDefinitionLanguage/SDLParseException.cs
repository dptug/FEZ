// Type: SDL.SDLParseException
// Assembly: SimpleDefinitionLanguage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8A9B110C-63DC-4C6F-B639-88CD09E9B5B5
// Assembly location: F:\Program Files (x86)\FEZ\SimpleDefinitionLanguage.dll

using System;

namespace SDL
{
  public class SDLParseException : FormatException
  {
    private int line;
    private int position;

    public int Line
    {
      get
      {
        return this.line;
      }
    }

    public int Position
    {
      get
      {
        return this.position;
      }
    }

    public SDLParseException(string description, int line, int position)
      : base(description + " Line " + (line == -1 ? "unknown" : string.Concat((object) line)) + ", Position " + (position == -1 ? "unknown" : string.Concat((object) position)))
    {
      this.line = line;
      this.position = position;
    }

    public override string ToString()
    {
      return this.Message;
    }
  }
}
