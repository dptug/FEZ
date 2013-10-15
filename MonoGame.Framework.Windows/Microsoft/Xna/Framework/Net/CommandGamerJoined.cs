// Type: Microsoft.Xna.Framework.Net.CommandGamerJoined
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Net
{
  internal class CommandGamerJoined : ICommand
  {
    private int gamerInternalIndex = -1;
    internal long remoteUniqueIdentifier = -1L;
    private string gamerTag = string.Empty;
    private string displayName = string.Empty;
    private GamerStates states;

    public string DisplayName
    {
      get
      {
        return this.displayName;
      }
      set
      {
        this.displayName = value;
      }
    }

    public string GamerTag
    {
      get
      {
        return this.gamerTag;
      }
      set
      {
        this.gamerTag = value;
      }
    }

    public GamerStates State
    {
      get
      {
        return this.states;
      }
      set
      {
        this.states = value;
      }
    }

    public int InternalIndex
    {
      get
      {
        return this.gamerInternalIndex;
      }
    }

    public CommandEventType Command
    {
      get
      {
        return CommandEventType.GamerJoined;
      }
    }

    public CommandGamerJoined(int internalIndex, bool isHost, bool isLocal)
    {
      this.gamerInternalIndex = internalIndex;
      if (isHost)
        this.states = this.states | GamerStates.Host;
      if (!isLocal)
        return;
      this.states = this.states | GamerStates.Local;
    }

    public CommandGamerJoined(long uniqueIndentifier)
    {
      this.remoteUniqueIdentifier = uniqueIndentifier;
    }
  }
}
