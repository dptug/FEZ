// Type: Microsoft.Xna.Framework.Net.CommandReceiveData
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
