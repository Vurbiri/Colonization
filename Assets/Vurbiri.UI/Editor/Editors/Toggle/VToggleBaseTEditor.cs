using System.Collections.Generic;
using UnityEditor;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggleBase<>), true), CanEditMultipleObjects]
    public class VToggleBaseTEditor : VSelectableEditor
	{
        protected SerializedProperty _isOnProperty;
        protected SerializedProperty _groupProperty;
        protected SerializedProperty _onValueChangedProperty;

        protected int _selectedCount;

        protected override bool IsDerivedEditor => GetType() != typeof(VToggleBaseTEditor);

        protected override void OnEnable()
		{
            _selectedCount = targets.Length;

            _isOnProperty           = serializedObject.FindProperty("_isOn");
            _groupProperty          = serializedObject.FindProperty("_group");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            base.OnEnable();
        }

        protected override HashSet<string> GetExcludePropertyPaths()
        {
            var exclude = base.GetExcludePropertyPaths();
            exclude.Add(_isOnProperty.propertyPath);
            exclude.Add(_groupProperty.propertyPath);
            exclude.Add(_onValueChangedProperty.propertyPath);
            return exclude;
        }

        sealed protected override void CustomMiddlePropertiesGUI()
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            BeginDisabledGroup(_selectedCount > 1 && (_groupProperty.objectReferenceValue != null | _groupProperty.hasMultipleDifferentValues));
            {
                PropertyField(_isOnProperty);
            }
            EndDisabledGroup();
            //============================================================
            TogglePropertiesGUI();
            //============================================================
            PropertyField(_groupProperty);
            Space();
        }

        protected virtual void TogglePropertiesGUI() { }

        sealed protected override void CustomEndPropertiesGUI()
        {
            Space();
            PropertyField(_onValueChangedProperty);
        }
    }
}