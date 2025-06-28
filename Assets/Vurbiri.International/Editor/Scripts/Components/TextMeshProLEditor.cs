using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using Vurbiri.International.UI;


namespace Vurbiri.International.Editor
{
	[CustomEditor(typeof(TextMeshProL), true), CanEditMultipleObjects]
	public class TextMeshProLEditor : TMP_EditorPanel
    {
        protected SerializedProperty _keyProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _keyProperty = serializedObject.FindProperty("_getText");
        }

        public override void OnInspectorGUI()
        {
            if (IsMixSelectionTypes()) return;

            serializedObject.Update();

            EditorGUILayout.Space(2f);
            GUI.Label(EditorGUILayout.GetControlRect(false, 22), new GUIContent("<b>Localization</b>"), TMP_UIStyleManager.sectionHeader);
            EditorGUILayout.Space(2f);
            EditorGUILayout.PropertyField(_keyProperty);
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}