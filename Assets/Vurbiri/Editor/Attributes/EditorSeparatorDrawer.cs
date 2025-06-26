using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(StartEditorAttribute))]
    public class StartEditorDrawer : PropertyDrawer
    {
        private readonly float _ratio = 0.9f;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            var text = label.text;

            if (attribute is StartEditorAttribute startEditor)
            {
                Rect rect = position; rect.x -= 8f;
                EditorGUI.DropShadowLabel(rect, startEditor.separator, EditorStyles.boldLabel);
                position.y += position.height * _ratio;
                label.text = text;
            }

            EditorGUI.PropertyField(position, mainProperty, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) + EditorGUIUtility.singleLineHeight * _ratio;
        }
    }

    [CustomPropertyDrawer(typeof(EndEditorAttribute))]
	public class EndEditorDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            if (attribute is not EndEditorAttribute endEditor)
            {
                EditorGUILayout.PropertyField(mainProperty, label, true);
                return;
            }

            position.height = EditorGUIUtility.singleLineHeight;
            position.x -= 8f; position.y -= EditorGUIUtility.singleLineHeight * 0.45f;

            EditorGUI.DropShadowLabel(position, endEditor.separator, EditorStyles.boldLabel);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight * 1.45f;
    }
}