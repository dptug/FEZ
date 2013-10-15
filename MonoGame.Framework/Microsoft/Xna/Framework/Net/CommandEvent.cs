// Type: Microsoft.Xna.Framework.Net.CommandEvent
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Net
{
  internal class CommandEvent
  {
    private CommandEventType command;
    private object commandObject;

    public CommandEventType Command
    {
      get
      {
        return this.command;
      }
    }

    public object CommandObject
    {
      get
      {
        return this.commandObject;
      }
    }

    public CommandEvent(CommandEventType command, object commandObject)
    {
      this.command = command;
      this.commandObject = commandObject;
    }

    public CommandEvent(ICommand command)
    {
      this.command = command.Command;
      this.commandObject = (object) command;
    }
  }
}
