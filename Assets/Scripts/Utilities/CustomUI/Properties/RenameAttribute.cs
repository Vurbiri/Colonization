using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class RenameAttribute : PropertyAttribute
{
    public string Name { get; private set; }

    public RenameAttribute(string name) => Name = name;
}
