// Type: Microsoft.Xna.Framework.Net.CommandEvent
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
