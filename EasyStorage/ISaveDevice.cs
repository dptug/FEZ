// Type: EasyStorage.ISaveDevice
// Assembly: EasyStorage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F34DFF18-A3CB-48F6-8800-AFF45A305BF1
// Assembly location: F:\Program Files (x86)\FEZ\EasyStorage.dll

namespace EasyStorage
{
  public interface ISaveDevice
  {
    bool Save(string fileName, SaveAction saveAction);

    bool Load(string fileName, LoadAction loadAction);

    bool Delete(string fileName);

    bool FileExists(string fileName);
  }
}
