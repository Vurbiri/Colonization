using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggle<>), true), CanEditMultipleObjects]
    public class VToggleTEditor : VSelectableEditor
    {
        private const float MIN_DURATION = 0f, MAX_DURATION = 1f;

        private static readonly string s_durationLabels = "Checkmark Fade Duration";
        private static readonly GUIContent[] s_checkmarkOnNames = { new("Checkmark"), new("Checkmark On"), new("Checkmark") };

        private static readonly Type s_graphicType = typeof(Graphic);

        private SerializedProperty _isOnProperty;
        private SerializedProperty _durationProperty;
        private SerializedProperty _switchingTypeProperty;
        private SerializedProperty _checkmarkOnProperty;
        private SerializedProperty _checkmarkOffProperty;
        private SerializedProperty _colorOnProperty;
        private SerializedProperty _colorOffProperty;
        private SerializedProperty _groupProperty;
        private SerializedProperty _onValueChangedProperty;

        private readonly AnimBool _showSwitchType = new(), _showColorType = new();

        private int _selectedCount;
          private SwitchingType _switchingType;

        protected override bool IsDerivedEditor => GetType() != typeof(VToggleTEditor);

        protected override void OnEnable()
        {
            _selectedCount          = targets.Length;

            _isOnProperty           = serializedObject.FindProperty("_isOn");
            _durationProperty       = serializedObject.FindProperty("_fadeDuration");
            _switchingTypeProperty  = serializedObject.FindProperty("_switchingType");
            _checkmarkOnProperty    = serializedObject.FindProperty("_checkmarkOn");
            _checkmarkOffProperty   = serializedObject.FindProperty("_checkmarkOff");
            _colorOnProperty        = serializedObject.FindProperty("_colorOn");
            _colorOffProperty       = serializedObject.FindProperty("_colorOff");
            _groupProperty          = serializedObject.FindProperty("_group");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            _switchingType          = (SwitchingType)_switchingTypeProperty.enumValueIndex;

            _showSwitchType.value   = _switchingType == SwitchingType.SwitchCheckmark;
            _showColorType.value    = _switchingType == SwitchingType.ColorCheckmark;

            _showSwitchType.valueChanged.AddListener(Repaint);
            _showColorType.valueChanged.AddListener(Repaint);

            base.OnEnable();
        }

        protected override HashSet<string> GetExcludePropertyPaths()
        {
            var exclude = base.GetExcludePropertyPaths();
            exclude.Add(_isOnProperty.propertyPath);
            exclude.Add(_durationProperty.propertyPath);
            exclude.Add(_switchingTypeProperty.propertyPath);
            exclude.Add(_checkmarkOnProperty.propertyPath);
            exclude.Add(_checkmarkOffProperty.propertyPath);
            exclude.Add(_colorOnProperty.propertyPath);
            exclude.Add(_colorOffProperty.propertyPath);
            exclude.Add(_groupProperty.propertyPath);
            exclude.Add(_onValueChangedProperty.propertyPath);
            return exclude;
        }

        protected override void OnDisable()
        {
            _showSwitchType.valueChanged.RemoveListener(Repaint);
            _showColorType.valueChanged.RemoveListener(Repaint);
            base.OnDisable();
        }

        protected override void CustomMiddlePropertiesGUI()
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            BeginDisabledGroup(_selectedCount > 1 && (_groupProperty.objectReferenceValue != null | _groupProperty.hasMultipleDifferentValues));
            {
                PropertyField(_isOnProperty);
            }
            EndDisabledGroup();
            //============================================================
            Slider(_durationProperty, MIN_DURATION, MAX_DURATION, s_durationLabels);
            //============================================================
            PropertyField(_switchingTypeProperty);

            _switchingType = (SwitchingType)_switchingTypeProperty.enumValueIndex;

            _showSwitchType.target = !_switchingTypeProperty.hasMultipleDifferentValues && _switchingType == SwitchingType.SwitchCheckmark;
            _showColorType.target = !_switchingTypeProperty.hasMultipleDifferentValues && _switchingType == SwitchingType.ColorCheckmark;
            //============================================================
            indentLevel++;
            BeginDisabledGroup(_selectedCount > 1);
            {
                ObjectField(_checkmarkOnProperty, s_graphicType, s_checkmarkOnNames[_switchingTypeProperty.enumValueIndex]);

                //============================================================
                if (BeginFadeGroup(_showSwitchType.faded))
                {
                    PropertyField(_checkmarkOffProperty);
                }
                EndFadeGroup();
            }
            EndDisabledGroup();
            //============================================================
            if (BeginFadeGroup(_showColorType.faded))
            {
                PropertyField(_colorOnProperty);
                PropertyField(_colorOffProperty);
            }
            EndFadeGroup();
            Space();
            indentLevel--;
            //============================================================
            PropertyField(_groupProperty);
            Space();
        }

        protected override void CustomEndPropertiesGUI()
        {
            Space();
            PropertyField(_onValueChangedProperty);
        }
    }
}
