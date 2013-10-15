// Type: FezEngine.Tools.MissingServiceException
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Tools
{
  public class MissingServiceException : Exception
  {
    private const string messageFormat = "The service dependency for {0} in {1} could not be resolved.";

    public MissingServiceException(Type requiringType, Type requiredType)
      : base(string.Format("The service dependency for {0} in {1} could not be resolved.", (object) requiredType, (object) requiringType))
    {
    }
  }
}
