// Type: SharpDX.ResultDescriptor
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [Serializable]
  public sealed class ResultDescriptor
  {
    private static readonly object LockDescriptor = new object();
    private static readonly List<Type> RegisteredDescriptorProvider = new List<Type>();
    private static readonly Dictionary<Result, ResultDescriptor> Descriptors = new Dictionary<Result, ResultDescriptor>();
    private const string UnknownText = "Unknown";

    public Result Result { get; private set; }

    public string Module { get; private set; }

    public string NativeApiCode { get; private set; }

    public string ApiCode { get; private set; }

    public string Description { get; set; }

    static ResultDescriptor()
    {
    }

    public ResultDescriptor(Result code, string module, string nativeApiCode, string apiCode, string description = null)
    {
      this.Result = code;
      this.Module = module;
      this.NativeApiCode = nativeApiCode;
      this.ApiCode = apiCode;
      this.Description = description;
    }

    public static implicit operator Result(ResultDescriptor result)
    {
      return result.Result;
    }

    public static bool operator ==(ResultDescriptor left, Result right)
    {
      if (left == null)
        return false;
      else
        return left.Result.Code == right.Code;
    }

    public static bool operator !=(ResultDescriptor left, Result right)
    {
      if (left == null)
        return false;
      else
        return left.Result.Code != right.Code;
    }

    public bool Equals(ResultDescriptor other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      else
        return other.Result.Equals(this.Result);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      if (obj.GetType() != typeof (ResultDescriptor))
        return false;
      else
        return this.Equals((ResultDescriptor) obj);
    }

    public override int GetHashCode()
    {
      return this.Result.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("HRESULT: [0x{0:X}], Module: [{1}], ApiCode: [{2}/{3}], Message: {4}", (object) this.Result.Code, (object) this.Module, (object) this.NativeApiCode, (object) this.ApiCode, (object) this.Description);
    }

    public static void RegisterProvider(Type descriptorsProviderType)
    {
      lock (ResultDescriptor.LockDescriptor)
      {
        if (ResultDescriptor.RegisteredDescriptorProvider.Contains(descriptorsProviderType))
          return;
        ResultDescriptor.RegisteredDescriptorProvider.Add(descriptorsProviderType);
      }
    }

    public static ResultDescriptor Find(Result result)
    {
      ResultDescriptor resultDescriptor;
      lock (ResultDescriptor.LockDescriptor)
      {
        if (ResultDescriptor.RegisteredDescriptorProvider.Count > 0)
        {
          foreach (Type item_0 in ResultDescriptor.RegisteredDescriptorProvider)
            ResultDescriptor.AddDescriptorsFromType(item_0);
          ResultDescriptor.RegisteredDescriptorProvider.Clear();
        }
        if (!ResultDescriptor.Descriptors.TryGetValue(result, out resultDescriptor))
          resultDescriptor = new ResultDescriptor(result, "Unknown", "Unknown", "Unknown", (string) null);
        if (resultDescriptor.Description == null)
        {
          string local_2 = ResultDescriptor.GetDescriptionFromResultCode(result.Code);
          resultDescriptor.Description = local_2 ?? "Unknown";
        }
      }
      return resultDescriptor;
    }

    private static void AddDescriptorsFromType(Type type)
    {
      foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
      {
        if (fieldInfo.FieldType == typeof (ResultDescriptor))
        {
          ResultDescriptor resultDescriptor = (ResultDescriptor) fieldInfo.GetValue((object) null);
          if (!ResultDescriptor.Descriptors.ContainsKey(resultDescriptor.Result))
            ResultDescriptor.Descriptors.Add(resultDescriptor.Result, resultDescriptor);
        }
      }
    }

    private static string GetDescriptionFromResultCode(int resultCode)
    {
      IntPtr lpBuffer = IntPtr.Zero;
      int num = (int) ResultDescriptor.FormatMessageW(4864, IntPtr.Zero, resultCode, 0, ref lpBuffer, 0, IntPtr.Zero);
      string str = Marshal.PtrToStringUni(lpBuffer);
      Marshal.FreeHGlobal(lpBuffer);
      return str;
    }

    [DllImport("kernel32.dll")]
    private static uint FormatMessageW(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, ref IntPtr lpBuffer, int nSize, IntPtr Arguments);
  }
}
