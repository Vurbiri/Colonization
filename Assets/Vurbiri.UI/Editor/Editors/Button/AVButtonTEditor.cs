using System.Collections.Generic;
using UnityEditor;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(AVButton<>), true), CanEditMultipleObjects]
    public class AVButtonEditorT : VSelectableEditor
    {
        private SerializedProperty _valueProperty;
        private SerializedProperty _onClickProperty;

        protected override bool IsDerivedEditor => GetType() != typeof(AVButtonEditorT);
        protected override HashSet<string> GetExcludePropertyPaths()
        {
            var exclude = base.GetExcludePropertyPaths();
            exclude.Add(_valueProperty.propertyPath);
            exclude.Add(_onClickProperty.propertyPath);
            return exclude;
        }

        protected override void OnEnable()
        {
            _valueProperty   = serializedObject.FindProperty("_value");
            _onClickProperty = serializedObject.FindProperty("_onClick");
            base.OnEnable();
        }

        protected override void CustomStartPropertiesGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_valueProperty);
            EditorGUILayout.Space();
        }

        protected override void CustomEndPropertiesGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_onClickProperty);
            EditorGUILayout.Space();
        }
    }
}
