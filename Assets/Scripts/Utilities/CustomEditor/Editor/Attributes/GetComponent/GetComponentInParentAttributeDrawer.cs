using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GetComponentInParentAttribute))]
public class GetComponentInParentAttributeDrawer : AGetComponentAttributeDrawer
{
    protected override void SetProperty(SerializedProperty property, GameObject gameObject, Type type)
    {
        GetComponentInParentAttribute attr = attribute as GetComponentInParentAttribute;

        property.objectReferenceValue = gameObject.GetComponentInParent(type, attr.IncludeInactive);

        Debug.Log("GetComponentInParentAttribute");
    }
}
