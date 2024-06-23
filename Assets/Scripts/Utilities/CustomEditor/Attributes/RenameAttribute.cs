using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class RenameAttribute : PropertyAttribute
{
    public string Name { get; private set; }

    public RenameAttribute(string name) => Name = name;
}
