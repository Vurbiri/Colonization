using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class GetComponentsInChildrenAttribute : PropertyAttribute
{
    public int IndexGeneric { get; private set; }
    public bool IncludeInactive { get; private set; }

    public GetComponentsInChildrenAttribute(int indexGeneric = 0, bool includeInactive = false)
    {
        IncludeInactive = includeInactive;
        IndexGeneric = indexGeneric;
    }
}
