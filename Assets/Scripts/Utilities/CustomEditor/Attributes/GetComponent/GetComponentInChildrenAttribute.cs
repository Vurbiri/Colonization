using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class GetComponentInChildrenAttribute : PropertyAttribute
{
    public bool IncludeInactive { get; private set; }

    public GetComponentInChildrenAttribute(bool includeInactive = false) => IncludeInactive = includeInactive;
}
