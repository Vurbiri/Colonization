//Assets\Vurbiri\Editor\CustomEditor\Attributes\GetComponent\GetComponentAttributeDrawer.cs
using System;
using UnityEditor;
using UnityEngine;
using Vurbiri;


namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(GetComponentAttribute))]
    internal class GetComponentAttributeDrawer : AGetComponentAttributeDrawer
    {
        protected override void SetProperty(SerializedProperty property, GameObject gameObject, Type type) => property.objectReferenceValue = gameObject.GetComponent(type);
    }
}
