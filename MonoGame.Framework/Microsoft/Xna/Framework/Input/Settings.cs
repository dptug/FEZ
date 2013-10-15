// Type: Microsoft.Xna.Framework.Input.Settings
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Input
{
  public class Settings
  {
    private List<PadConfig> players;

    public PadConfig this[int index]
    {
      get
      {
        return this.players[index];
      }
      set
      {
        this.players[index] = value;
      }
    }

    public PadConfig Player1 { get; set; }

    public PadConfig Player2 { get; set; }

    public PadConfig Player3 { get; set; }

    public PadConfig Player4 { get; set; }

    public Settings()
    {
      this.players = new List<PadConfig>();
      this.players.Add((PadConfig) null);
      this.players.Add((PadConfig) null);
      this.players.Add((PadConfig) null);
      this.players.Add((PadConfig) null);
    }
  }
}
