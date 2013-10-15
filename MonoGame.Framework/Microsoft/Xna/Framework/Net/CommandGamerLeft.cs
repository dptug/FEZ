// Type: Microsoft.Xna.Framework.Net.CommandGamerLeft
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
