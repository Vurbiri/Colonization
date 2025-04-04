//Assets\Vurbiri\Editor\UtilityEditor\SubPropertyDrawer.cs
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public abstract class SubPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUIUtility.singleLineHeight;

            property.isExpanded = true;

            int count = property.Copy().CountInProperty();
            EditorGUI.BeginProperty(position, label, property);
            while (--count > 0)
            {
                property.Next(true);
                EditorGUI.PropertyField(position, property, new GUIContent(property.displayName));
                position.y += height;
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * (property.CountInProperty() - 1);
        }
    }
}
