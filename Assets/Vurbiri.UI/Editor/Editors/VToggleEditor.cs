//Assets\Vurbiri.UI\Editor\Editors\VToggleEditor.cs
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggle)), CanEditMultipleObjects]
    sealed public class VToggleEditor : VSelectableEditor
    {
        private const float MIN_DURATION = 0f, MAX_DURATION = 1f;
        
        private static readonly string[] switchingLabels = { "On|Off checkmark", "Switch checkmarks", "Color checkmark" };
        private static readonly GUIContent[] checkmarkLabels = { new("Checkmark"), new("Checkmark On"), new("Checkmark") };
        private static readonly GUIContent isFadeLabels = new("Checkmark Fade Duration");

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
        private VToggle.SwitchingType _fadeType;

        protected override void OnEnable()
        {
            _toggle = target as VToggle;
            _fadeType = _toggle.Switching;
            
            _isOnProperty = serializedObject.FindProperty("_isOn");
            _durationProperty = serializedObject.FindProperty("_fadeDuration");
            _switchingTypeProperty = serializedObject.FindProperty("_switchingType");
            _checkmarkOnProperty = serializedObject.FindProperty("_checkmarkOn");
            _checkmarkOffProperty = serializedObject.FindProperty("_checkmarkOff");
            _colorOnProperty = serializedObject.FindProperty("_colorOn");
            _colorOffProperty = serializedObject.FindProperty("_colorOff");
            _groupProperty = serializedObject.FindProperty("_group");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            _showSwitchType.value = _fadeType == VToggle.SwitchingType.SwitchCheckmark;
            _showColorType.value = _fadeType == VToggle.SwitchingType.ColorCheckmark;

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
            EditorGUI.BeginChangeCheck();
            PropertyField(_isOnProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);

                VToggleGroup group = _toggle.group;

                if (group != null && _toggle.isActiveAndEnabled)
                    _isOnProperty.boolValue = group.CheckValue_Editor(_toggle, _isOnProperty.boolValue);

                _toggle.isOn = _isOnProperty.boolValue;
            }
            //============================================================
            EditorGUI.BeginChangeCheck();
            Slider(_durationProperty, MIN_DURATION, MAX_DURATION, isFadeLabels);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);
                _toggle.CheckmarkFadeDuration = _durationProperty.floatValue;
            }
            //============================================================
            EditorGUI.BeginChangeCheck();
            _switchingTypeProperty.enumValueIndex = Popup(_switchingTypeProperty.displayName, _switchingTypeProperty.enumValueIndex, switchingLabels);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);

                _toggle.Switching = _fadeType = (VToggle.SwitchingType)_switchingTypeProperty.enumValueIndex;
                _showSwitchType.target = !_switchingTypeProperty.hasMultipleDifferentValues && _fadeType == VToggle.SwitchingType.SwitchCheckmark;
                _showColorType.target = !_switchingTypeProperty.hasMultipleDifferentValues && _fadeType == VToggle.SwitchingType.ColorCheckmark;
            }

            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            PropertyField(_checkmarkOnProperty, checkmarkLabels[_switchingTypeProperty.enumValueIndex]);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);
                _toggle.CheckmarkOn = _checkmarkOnProperty.objectReferenceValue as Graphic;
            }

            if (BeginFadeGroup(_showSwitchType.faded))
            {
                EditorGUI.BeginChangeCheck();
                PropertyField(_checkmarkOffProperty);
                if (EditorGUI.EndChangeCheck())
                {
                    if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);
                    _toggle.CheckmarkOff = _checkmarkOffProperty.objectReferenceValue as Graphic;
                }
            }
            EndFadeGroup();
            if (BeginFadeGroup(_showColorType.faded))
            {
                EditorGUI.BeginChangeCheck();
                PropertyField(_colorOnProperty);
                PropertyField(_colorOffProperty);
                if (EditorGUI.EndChangeCheck())
                {
                    if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);
                    _toggle.SetColors(_colorOnProperty.colorValue, _colorOffProperty.colorValue);
                }
            }
            EndFadeGroup();
            Space();
            EditorGUI.indentLevel--;
            //============================================================
            EditorGUI.BeginChangeCheck();
            PropertyField(_groupProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);

                _toggle.group = _groupProperty.objectReferenceValue as VToggleGroup;
            }
            Space();
        }

        protected override void CustomEndPropertiesGUI()
        {
            Space();
            PropertyField(_onValueChangedProperty);
        }
    }
}
