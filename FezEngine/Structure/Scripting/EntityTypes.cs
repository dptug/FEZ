// Type: FezEngine.Structure.Scripting.EntityTypes
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Services.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FezEngine.Structure.Scripting
{
  public static class EntityTypes
  {
    private const string Namespace = "FezEngine.Services.Scripting";

    public static IDictionary<string, EntityTypeDescriptor> Types { get; private set; }

    static EntityTypes()
    {
      EntityTypes.Types = (IDictionary<string, EntityTypeDescriptor>) new Dictionary<string, EntityTypeDescriptor>();
      foreach (Type interfaceType in Enumerable.Where<Type>((IEnumerable<Type>) Assembly.GetExecutingAssembly().GetTypes(), (Func<Type, bool>) (x =>
      {
        if (x.Namespace == "FezEngine.Services.Scripting" && x.IsInterface)
          return Util.Implements(x, typeof (IScriptingBase));
        else
          return false;
      })))
      {
        string str = interfaceType.Name.Substring(1, interfaceType.Name.IndexOf("Service") - 1);
        EntityAttribute attribute = Enumerable.FirstOrDefault<object>((IEnumerable<object>) interfaceType.GetCustomAttributes(typeof (EntityAttribute), false)) as EntityAttribute;
        if (attribute == null)
          Logger.Log("Engine.Scripting.EntityTypes", LogSeverity.Warning, "Entity type '" + str + "' did not contain any metadata, thus was not loaded.");
        else
          EntityTypes.Types.Add(str, new EntityTypeDescriptor(str, attribute.Static, attribute.Model, attribute.RestrictTo, interfaceType, Enumerable.Select<MethodInfo, OperationDescriptor>(Enumerable.Where<MethodInfo>((IEnumerable<MethodInfo>) interfaceType.GetMethods(), (Func<MethodInfo, bool>) (m =>
          {
            if (!m.IsSpecialName && !m.Name.StartsWith("On"))
              return !m.Name.StartsWith("get_");
            else
              return false;
          })), (Func<MethodInfo, OperationDescriptor>) (m => new OperationDescriptor(m.Name, EntityTypes.GetDescription((MemberInfo) m), ReflectionHelper.CreateDelegate((MethodBase) m), Enumerable.Select<ParameterInfo, ParameterDescriptor>(Enumerable.Skip<ParameterInfo>((IEnumerable<ParameterInfo>) m.GetParameters(), attribute.Static ? 0 : 1), (Func<ParameterInfo, ParameterDescriptor>) (p => new ParameterDescriptor(p.Name, p.ParameterType)))))), Enumerable.Union<PropertyDescriptor>(Enumerable.Select<PropertyInfo, PropertyDescriptor>((IEnumerable<PropertyInfo>) interfaceType.GetProperties(), (Func<PropertyInfo, PropertyDescriptor>) (p => new PropertyDescriptor(p.Name, EntityTypes.GetDescription((MemberInfo) p), p.PropertyType, ReflectionHelper.CreateDelegate((MethodBase) p.GetGetMethod())))), Enumerable.Select<MethodInfo, PropertyDescriptor>(Enumerable.Where<MethodInfo>((IEnumerable<MethodInfo>) interfaceType.GetMethods(), (Func<MethodInfo, bool>) (m =>
          {
            if (!m.IsSpecialName)
              return m.Name.StartsWith("get_");
            else
              return false;
          })), (Func<MethodInfo, PropertyDescriptor>) (m => new PropertyDescriptor(m.Name.Substring(4), EntityTypes.GetDescription((MemberInfo) m), m.ReturnType, ReflectionHelper.CreateDelegate((MethodBase) m))))), Enumerable.Select<EventInfo, EventDescriptor>((IEnumerable<EventInfo>) interfaceType.GetEvents(), (Func<EventInfo, EventDescriptor>) (e => new EventDescriptor(e.Name, EntityTypes.GetDescription((MemberInfo) e), ReflectionHelper.CreateDelegate((MethodBase) e.GetAddMethod()), EntityTypes.GetEndTrigger(e))))));
      }
    }

    private static string GetDescription(MemberInfo info)
    {
      DescriptionAttribute descriptionAttribute = Enumerable.FirstOrDefault<object>((IEnumerable<object>) info.GetCustomAttributes(typeof (DescriptionAttribute), false)) as DescriptionAttribute;
      if (descriptionAttribute != null)
        return descriptionAttribute.Description;
      else
        return (string) null;
    }

    private static DynamicMethodDelegate GetEndTrigger(EventInfo info)
    {
      EndTriggerAttribute triggerAttribute = Enumerable.FirstOrDefault<object>((IEnumerable<object>) info.GetCustomAttributes(typeof (EndTriggerAttribute), false)) as EndTriggerAttribute;
      if (triggerAttribute == null)
        return (DynamicMethodDelegate) null;
      else
        return ReflectionHelper.CreateDelegate((MethodBase) info.DeclaringType.GetEvent(triggerAttribute.Trigger).GetAddMethod());
    }
  }
}
