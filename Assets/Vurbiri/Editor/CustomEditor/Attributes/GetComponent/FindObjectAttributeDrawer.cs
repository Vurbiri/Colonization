using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(FindObjectAttribute))]
    internal class FindObjectAttributeDrawer : AGetComponentAttributeDrawer
    {
        protected override void SetProperty(SerializedProperty property, GameObject gameObject, System.Type type)
        {
            FindObjectAttribute attr = attribute as FindObjectAttribute;

            property.objectReferenceValue = Object.FindObjectOfType(type, attr.includeInactive);
        }
    }
}
