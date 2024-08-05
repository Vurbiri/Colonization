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
            label.text = ((RenameAttribute)attribute).Name;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndProperty();
        }
    }
}
