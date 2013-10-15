// Type: Microsoft.Xna.Framework.Net.CommandGamerLeft
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Net
{
  internal class CommandGamerLeft : ICommand
  {
    private int gamerInternalIndex = -1;
    internal long remoteUniqueIdentifier = -1L;

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
        return CommandEventType.GamerLeft;
      }
    }

    public CommandGamerLeft(int internalIndex)
    {
      this.gamerInternalIndex = internalIndex;
    }

    public CommandGamerLeft(long uniqueIndentifier)
    {
      this.remoteUniqueIdentifier = uniqueIndentifier;
    }
  }
}
