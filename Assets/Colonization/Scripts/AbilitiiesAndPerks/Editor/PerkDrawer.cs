using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.Colonization
{
    //[CustomPropertyDrawer(typeof(Perk))]
    public class PerkDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            var serializedProperty = property.FindPropertyRelative("_name");

            label.text = serializedProperty.stringValue;

            property.Reset();

            EditorGUI.BeginProperty(position, label, property);
            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                do
                {
                    position.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(position, serializedProperty, new GUIContent(serializedProperty.displayName), true);
                }
                while (serializedProperty.NextVisible(false));
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rate = 1.01f;
            var serializedProperty = property.FindPropertyRelative("_name");
            if (property.isExpanded)
            {
                do
                {
                    rate += 1;
                }
                while (serializedProperty.NextVisible(false));
            }

            return EditorGUIUtility.singleLineHeight * rate;
        }
    }
}
