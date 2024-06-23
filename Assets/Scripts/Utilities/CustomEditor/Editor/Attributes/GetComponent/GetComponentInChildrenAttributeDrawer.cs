using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GetComponentInChildrenAttribute))]
public class GetComponentInChildrenAttributeDrawer : AGetComponentAttributeDrawer
{
    protected override void SetProperty(SerializedProperty property, GameObject gameObject, Type type)
    {
        GetComponentInChildrenAttribute attr = attribute as GetComponentInChildrenAttribute;

        property.objectReferenceValue = gameObject.GetComponentInChildren(type, attr.IncludeInactive);
    }
}
