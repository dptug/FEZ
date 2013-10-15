// Type: Newtonsoft.Json.Serialization.JsonSerializerInternalBase
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
  internal abstract class JsonSerializerInternalBase
  {
    private ErrorContext _currentErrorContext;
    private BidirectionalDictionary<string, object> _mappings;
    internal readonly JsonSerializer Serializer;

    internal BidirectionalDictionary<string, object> DefaultReferenceMappings
    {
      get
      {
        if (this._mappings == null)
          this._mappings = new BidirectionalDictionary<string, object>((IEqualityComparer<string>) EqualityComparer<string>.Default, (IEqualityComparer<object>) new JsonSerializerInternalBase.ReferenceEqualsEqualityComparer(), "A different value already has the Id '{0}'.", "A different Id has already been assigned for value '{0}'.");
        return this._mappings;
      }
    }

    protected JsonSerializerInternalBase(JsonSerializer serializer)
    {
      ValidationUtils.ArgumentNotNull((object) serializer, "serializer");
      this.Serializer = serializer;
    }

    protected ErrorContext GetErrorContext(object currentObject, object member, string path, Exception error)
    {
      if (this._currentErrorContext == null)
        this._currentErrorContext = new ErrorContext(currentObject, member, path, error);
      if (this._currentErrorContext.Error != error)
        throw new InvalidOperationException("Current error context error is different to requested error.");
      else
        return this._currentErrorContext;
    }

    protected void ClearErrorContext()
    {
      if (this._currentErrorContext == null)
        throw new InvalidOperationException("Could not clear error context. Error context is already null.");
      this._currentErrorContext = (ErrorContext) null;
    }

    protected bool IsErrorHandled(object currentObject, JsonContract contract, object keyValue, string path, Exception ex)
    {
      ErrorContext errorContext = this.GetErrorContext(currentObject, keyValue, path, ex);
      if (contract != null)
        contract.InvokeOnError(currentObject, this.Serializer.Context, errorContext);
      if (!errorContext.Handled)
        this.Serializer.OnError(new ErrorEventArgs(currentObject, errorContext));
      return errorContext.Handled;
    }

    private class ReferenceEqualsEqualityComparer : IEqualityComparer<object>
    {
      bool IEqualityComparer<object>.Equals(object x, object y)
      {
        return object.ReferenceEquals(x, y);
      }

      int IEqualityComparer<object>.GetHashCode(object obj)
      {
        return RuntimeHelpers.GetHashCode(obj);
      }
    }
  }
}
