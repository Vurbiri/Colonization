using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public abstract class AValueDrawer : PropertyDrawer
    {
        protected virtual string NameValue => "_value";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative(NameValue), label);
            EditorGUI.EndProperty();
        }
    }
}
