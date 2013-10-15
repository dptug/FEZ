// Type: FezEngine.Tools.MissingSetterException
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Tools
{
  public class MissingSetterException : Exception
  {
    private const string messageFormat = "The service dependency for {0} in {1} could not be injected because a setter could not be found.";

    public MissingSetterException(Type requiringType, Type requiredType)
      : base(string.Format("The service dependency for {0} in {1} could not be injected because a setter could not be found.", (object) requiredType, (object) requiringType))
    {
    }
  }
}
