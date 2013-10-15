// Type: Microsoft.Xna.Framework.Net.CommandGamerStateChange
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Net
{
  internal class CommandGamerStateChange : ICommand
  {
    private GamerStates newState;
    private GamerStates oldState;
    private NetworkGamer gamer;

    public NetworkGamer Gamer
    {
      get
      {
        return this.gamer;
      }
    }

    public GamerStates NewState
    {
      get
      {
        return this.newState;
      }
    }

    public GamerStates OldState
    {
      get
      {
        return this.oldState;
      }
    }

    public CommandEventType Command
    {
      get
      {
        return CommandEventType.GamerStateChange;
      }
    }

    public CommandGamerStateChange(NetworkGamer gamer)
    {
      this.gamer = gamer;
      this.newState = gamer.State;
      this.oldState = gamer.OldState;
    }
  }
}
