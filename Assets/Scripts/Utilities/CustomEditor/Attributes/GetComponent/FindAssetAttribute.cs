using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class FindAssetAttribute : PropertyAttribute
{
    public string fileName;
    
    public FindAssetAttribute(string fileName = null)
    {
        this.fileName = fileName;
    }
}
