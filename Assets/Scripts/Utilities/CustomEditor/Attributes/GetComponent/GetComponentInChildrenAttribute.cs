using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class GetComponentInChildrenAttribute : PropertyAttribute
{
    public bool IncludeInactive { get; private set; }

    public GetComponentInChildrenAttribute(bool includeInactive = false) => IncludeInactive = includeInactive;
}
