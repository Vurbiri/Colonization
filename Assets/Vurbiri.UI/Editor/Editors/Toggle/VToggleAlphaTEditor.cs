using System.Collections.Generic;
using UnityEditor;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggleAlpha<>), true), CanEditMultipleObjects]
    sealed public class VToggleAlphaTEditor : VToggleBaseTEditor
    {
        private SerializedProperty _switcherProperty;
        private SerializedProperty _canvasGroupProperty;
        private SerializedProperty _speedProperty;

        protected override bool IsDerivedEditor => GetType() != typeof(VToggleAlphaTEditor);

        protected override void OnEnable()
        {
            _switcherProperty = serializedObject.FindProperty("_switcher");

            _canvasGroupProperty = _switcherProperty.FindPropertyRelative("canvasGroup");
            _speedProperty       = _switcherProperty.FindPropertyRelative("speed");

            base.OnEnable();
        }

        protected override HashSet<string> GetExcludePropertyPaths()
        {
            var exclude = base.GetExcludePropertyPaths();
            exclude.Add(_switcherProperty.propertyPath);
            return exclude;
        }

        protected override void TogglePropertiesGUI()
        {
            PropertyField(_canvasGroupProperty);
            
            EditorGUI.indentLevel++;
            PropertyField(_speedProperty);
            EditorGUI.indentLevel--;
        }
    }
}