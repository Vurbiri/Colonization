using System;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(GetComponentInParentAttribute))]
    internal class GetComponentInParentAttributeDrawer : AGetComponentAttributeDrawer
    {
        protected override void SetProperty(SerializedProperty property, GameObject gameObject, Type type)
        {
            GetComponentInParentAttribute attr = attribute as GetComponentInParentAttribute;

            property.objectReferenceValue = gameObject.transform.parent.GetComponent(type);

            Debug.Log("GetComponentInParentAttribute");
        }
    }
}
