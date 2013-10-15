// Type: Microsoft.Xna.Framework.Net.CommandGamerStateChange
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
