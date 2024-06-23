using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GetComponentAttribute))]
public class GetComponentAttributeDrawer : AGetComponentAttributeDrawer
{
    protected override void SetProperty(SerializedProperty property, GameObject gameObject, Type type) => property.objectReferenceValue = gameObject.GetComponent(type);
}
