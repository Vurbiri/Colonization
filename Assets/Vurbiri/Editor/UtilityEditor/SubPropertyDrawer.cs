//Assets\Vurbiri\Editor\UtilityEditor\SubPropertyDrawer.cs
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public abstract class SubPropertyDrawer : PropertyDrawer
    {
        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            property.isExpanded = true;

            int count = property.Copy().CountInProperty();
            EditorGUI.BeginProperty(position, label, property);
            while (--count > 0)
            {
                property.Next(true);
                EditorGUI.PropertyField(position, property, new GUIContent(property.displayName));
                position.y += _height;
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _height * (property.CountInProperty() - 1);
        }
    }
}
