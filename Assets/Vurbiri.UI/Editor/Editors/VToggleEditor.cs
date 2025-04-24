//Assets\Vurbiri.UI\Editor\Editors\VToggleEditor.cs
using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;
using static Vurbiri.UI.VToggle;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggle)), CanEditMultipleObjects]
    sealed public class VToggleEditor : VSelectableEditor
    {
        private const string NAME = "Toggle", RESOURCE = "VToggle";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        private const float MIN_DURATION = 0f, MAX_DURATION = 1f;

        private static readonly string durationLabels = "Checkmark Fade Duration";
        private static readonly GUIContent[] checkmarkOnNames = { new("Checkmark"), new("Checkmark On"), new("Checkmark") };

        private static readonly Type graphicType = typeof(Graphic);

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

        private VToggle _toggle;
        private int _selectedCount;
        private VToggle[] _toggles;
        private SwitchingType _switchingType;

        protected override void OnEnable()
        {
            _toggle = (VToggle)target;
            _switchingType = _toggle.Switching;

            _selectedCount = targets.Length;
            _toggles = new VToggle[_selectedCount];
            for (int i = 0; i < _selectedCount; i++)
                _toggles[i] = (VToggle)targets[i];

            _isOnProperty           = serializedObject.FindProperty("_isOn");
            _durationProperty       = serializedObject.FindProperty("_fadeDuration");
            _switchingTypeProperty  = serializedObject.FindProperty("_switchingType");
            _checkmarkOnProperty    = serializedObject.FindProperty("_checkmarkOn");
            _checkmarkOffProperty   = serializedObject.FindProperty("_checkmarkOff");
            _colorOnProperty        = serializedObject.FindProperty("_colorOn");
            _colorOffProperty       = serializedObject.FindProperty("_colorOff");
            _groupProperty          = serializedObject.FindProperty("_group");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            _showSwitchType.value   = _switchingType == SwitchingType.SwitchCheckmark;
            _showColorType.value    = _switchingType == SwitchingType.ColorCheckmark;

            _showSwitchType.valueChanged.AddListener(Repaint);
            _showColorType.valueChanged.AddListener(Repaint);

            base.OnEnable();
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
                BeginChangeCheck();
                PropertyField(_isOnProperty);
                if (EndChangeCheck())
                {
                    foreach (var toggle in _toggles)
                        toggle.IsOn = _isOnProperty.boolValue;

                    _isOnProperty.boolValue = _toggle.IsOn;
                }
            }
            EndDisabledGroup();
            //============================================================
            Slider(_durationProperty, MIN_DURATION, MAX_DURATION, durationLabels);
            //============================================================
            PropertyField(_switchingTypeProperty);

            _switchingType = (SwitchingType)_switchingTypeProperty.enumValueIndex;

            _showSwitchType.target = !_switchingTypeProperty.hasMultipleDifferentValues && _switchingType == SwitchingType.SwitchCheckmark;
            _showColorType.target = !_switchingTypeProperty.hasMultipleDifferentValues && _switchingType == SwitchingType.ColorCheckmark;
            //============================================================
            indentLevel++;
            BeginDisabledGroup(_selectedCount > 1);
            {
                ObjectField(_checkmarkOnProperty, graphicType, checkmarkOnNames[_switchingTypeProperty.enumValueIndex]);

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
            BeginChangeCheck();
            PropertyField(_groupProperty);
            if (EndChangeCheck())
            {
                VToggleGroup group = _groupProperty.objectReferenceValue as VToggleGroup;
                
                foreach (var toggle in _toggles)
                    toggle.Group = group;

                if (!Application.isPlaying && group != null)
                {
                    serializedObject.ApplyModifiedProperties();
                    foreach (var toggle in _toggles)
                    {
                        if (group.IsActiveToggle)
                            toggle.IsOn = group.ActiveToggle == toggle;
                        else if (!group.AllowSwitchOff)
                            toggle.IsOn = true;
                    }
                    serializedObject.Update();
                }
            }
            Space();
        }

        protected override void CustomEndPropertiesGUI()
        {
            Space();
            PropertyField(_onValueChangedProperty);
        }

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateObjectFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
