// Type: FezEngine.Structure.Scripting.EntityTypeDescriptor
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using System;
using System.Collections.Generic;

namespace FezEngine.Structure.Scripting
{
  public struct EntityTypeDescriptor
  {
    public readonly string Name;
    public readonly bool Static;
    public readonly Type Model;
    public readonly ActorType[] RestrictTo;
    public readonly Type Interface;
    public readonly IDictionary<string, OperationDescriptor> Operations;
    public readonly IDictionary<string, PropertyDescriptor> Properties;
    public readonly IDictionary<string, EventDescriptor> Events;

    public EntityTypeDescriptor(string name, bool isStatic, Type modelType, ActorType[] restrictTo, Type interfaceType, IEnumerable<OperationDescriptor> operations, IEnumerable<PropertyDescriptor> properties, IEnumerable<EventDescriptor> events)
    {
      this.Name = name;
      this.Static = isStatic;
      this.Model = modelType;
      this.RestrictTo = restrictTo;
      this.Interface = interfaceType;
      this.Operations = (IDictionary<string, OperationDescriptor>) new Dictionary<string, OperationDescriptor>();
      foreach (OperationDescriptor operationDescriptor in operations)
        this.Operations.Add(operationDescriptor.Name, operationDescriptor);
      this.Properties = (IDictionary<string, PropertyDescriptor>) new Dictionary<string, PropertyDescriptor>();
      foreach (PropertyDescriptor propertyDescriptor in properties)
        this.Properties.Add(propertyDescriptor.Name, propertyDescriptor);
      this.Events = (IDictionary<string, EventDescriptor>) new Dictionary<string, EventDescriptor>();
      foreach (EventDescriptor eventDescriptor in events)
        this.Events.Add(eventDescriptor.Name, eventDescriptor);
    }
  }
}
