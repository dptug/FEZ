// Type: Microsoft.Xna.Framework.Net.CommandSessionStateChange
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Net
{
  internal class CommandSessionStateChange : ICommand
  {
    private NetworkSessionState newState;
    private NetworkSessionState oldState;

    public NetworkSessionState NewState
    {
      get
      {
        return this.newState;
      }
    }

    public NetworkSessionState OldState
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
        return CommandEventType.SessionStateChange;
      }
    }

    public CommandSessionStateChange(NetworkSessionState newState, NetworkSessionState oldState)
    {
      this.newState = newState;
      this.oldState = oldState;
    }
  }
}
