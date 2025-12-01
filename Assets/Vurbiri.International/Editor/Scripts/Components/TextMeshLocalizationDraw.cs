using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace Vurbiri.International.Editor
{
	internal class TextMeshLocalizationDraw
	{
		private readonly GUIContent _label = new ("<b>Localization</b>");

        private readonly SerializedProperty _getTextProperty;
        private readonly SerializedProperty _extractProperty;

        public TextMeshLocalizationDraw(SerializedProperty getTextProperty, SerializedProperty extractProperty)
		{
			_getTextProperty = getTextProperty;
			_extractProperty = extractProperty;
		}

		public void Draw(SerializedObject serializedObject)
        {
            serializedObject.Update();

            EditorGUILayout.Space(2f);
            GUI.Label(EditorGUILayout.GetControlRect(false, 22), _label, TMP_UIStyleManager.sectionHeader);
            EditorGUILayout.Space(2f);
            EditorGUILayout.PropertyField(_getTextProperty);
            EditorGUILayout.Space(-2f);
            EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_extractProperty);
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
