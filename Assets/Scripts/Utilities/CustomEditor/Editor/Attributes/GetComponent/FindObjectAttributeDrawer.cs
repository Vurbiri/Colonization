using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FindObjectAttribute))]
public class FindObjectAttributeDrawer : AGetComponentAttributeDrawer
{
    protected override void SetProperty(SerializedProperty property, GameObject gameObject, System.Type type)
    {
        FindObjectAttribute attr = attribute as FindObjectAttribute;

        property.objectReferenceValue = Object.FindObjectOfType(type, attr.includeInactive);
    }
}
