//Assets\Vurbiri\Editor\CustomEditor\Attributes\RenameAttributeDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(RenameAttribute))]
    internal class RenameAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            label.text = ((RenameAttribute)attribute).Name;
            EditorGUI.PropertyField(position, property, label);

            EditorGUI.EndProperty();
        }
    }
}
