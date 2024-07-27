using UnityEditor;
using UnityEngine;

namespace Vurbiri
{
    [CustomPropertyDrawer(typeof(UnityDictionary<,>))]
    public class UnityDictionaryDrawer : PropertyDrawer
    {
        private const string NAME_PROPERTY = "_values";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty propertyValues = property.FindPropertyRelative(NAME_PROPERTY);

            EditorGUI.BeginProperty(position, label, property);
            EditorGUILayout.PropertyField(propertyValues, label);
            EditorGUI.EndProperty();
        }
    }
}
