// Type: Newtonsoft.Json.Serialization.IReferenceResolver
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Serialization
{
  public interface IReferenceResolver
  {
    object ResolveReference(object context, string reference);

    string GetReference(object context, object value);

    bool IsReferenced(object context, object value);

    void AddReference(object context, string reference, object value);
  }
}
