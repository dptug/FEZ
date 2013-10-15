// Type: Microsoft.Xna.Framework.Net.CommandReceiveData
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Net
{
  internal class CommandReceiveData : ICommand
  {
    internal long remoteUniqueIdentifier = -1L;
    internal byte[] data;
    internal NetworkGamer gamer;

    public CommandEventType Command
    {
      get
      {
        return CommandEventType.ReceiveData;
      }
    }

    public CommandReceiveData(long remoteUniqueIdentifier, byte[] data)
    {
      this.remoteUniqueIdentifier = remoteUniqueIdentifier;
      this.data = data;
    }
  }
}
