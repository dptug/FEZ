// Type: Microsoft.Xna.Framework.Net.CommandSessionStateChange
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
