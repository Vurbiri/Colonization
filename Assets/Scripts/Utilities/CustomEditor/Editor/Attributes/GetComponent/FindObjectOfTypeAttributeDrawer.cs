using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FindObjectOfTypeAttribute))]
public class FindObjectOfTypeAttributeDrawer : AGetComponentAttributeDrawer
{
    protected override void SetProperty(SerializedProperty property, GameObject gameObject, Type type)
    {
        FindObjectOfTypeAttribute attr = attribute as FindObjectOfTypeAttribute;

        property.objectReferenceValue = UnityEngine.Object.FindObjectOfType(type, attr.IncludeInactive);

        Debug.Log(property.objectReferenceValue);
    }
}
