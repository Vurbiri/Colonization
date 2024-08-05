using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(Chance))]
    public class ChanceDrawer : PropertyDrawer
    {
        private const string NAME_VALUE = "_value";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);

            EditorGUI.BeginProperty(position, label, property);
            valueProperty.intValue = EditorGUI.IntSlider(position, label, valueProperty.intValue, 0, 100);
            EditorGUI.EndProperty();
        }
    }
}
