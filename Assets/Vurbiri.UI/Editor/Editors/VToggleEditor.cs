//Assets\Vurbiri.UI\Editor\Editors\VToggleEditor.cs
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggle)), CanEditMultipleObjects]
    sealed public class VToggleEditor : VSelectableEditor
    {
        private const float MIN_DURATION = 0.01f, MAX_DURATION = 1f;
        private static readonly EnumArray<VToggle.FadeType, GUIContent> markLabels = new(new GUIContent[] 
                                                                                         { new("Checkmark"), new("Checkmark On"), new("Checkmark") });
        private SerializedProperty _isOnProperty;
        private SerializedProperty _isFadeProperty;
        private SerializedProperty _durationProperty;
        private SerializedProperty _fadeTypeProperty;
        private SerializedProperty _checkmarkOnProperty;
        private SerializedProperty _checkmarkOffProperty;
        private SerializedProperty _colorOnProperty;
        private SerializedProperty _colorOffProperty;
        private SerializedProperty _groupProperty;
        private SerializedProperty _onValueChangedProperty;

        private readonly AnimBool _showDuration = new();
        private readonly AnimBool _showSwitchType = new(), _showColorType = new();
       
        private VToggle _toggle;
        private bool _isFade;
        private VToggle.FadeType _fadeType;

        protected override void OnEnable()
        {
            _toggle = target as VToggle;
            _isFade = _toggle.IsCheckmarkFade;
            _fadeType = _toggle.CheckmarkFadeType;
            
            _isOnProperty = serializedObject.FindProperty("_isOn");
            _isFadeProperty = serializedObject.FindProperty("_isFade");
            _durationProperty = serializedObject.FindProperty("_fadeDuration");
            _fadeTypeProperty = serializedObject.FindProperty("_fadeType");
            _checkmarkOnProperty = serializedObject.FindProperty("_checkmarkOn");
            _checkmarkOffProperty = serializedObject.FindProperty("_checkmarkOff");
            _colorOnProperty = serializedObject.FindProperty("_colorOn");
            _colorOffProperty = serializedObject.FindProperty("_colorOff");
            _groupProperty = serializedObject.FindProperty("_group");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            _showDuration.value = _isFade;
            _showSwitchType.value = _fadeType == VToggle.FadeType.SwitchCheckmark;
            _showColorType.value = _fadeType == VToggle.FadeType.ColorCheckmark;

            _showDuration.valueChanged.AddListener(Repaint);
            _showSwitchType.valueChanged.AddListener(Repaint);
            _showColorType.valueChanged.AddListener(Repaint);

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            _showDuration.valueChanged.RemoveListener(Repaint);
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
            PropertyField(_isFadeProperty, new GUIContent("Checkmark Fade?"));
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);

                _toggle.IsCheckmarkFade = _isFade = _isFadeProperty.boolValue;
                _showDuration.target = !_fadeTypeProperty.hasMultipleDifferentValues && _isFade;
            }
            
            if (BeginFadeGroup(_showDuration.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                Slider(_durationProperty, MIN_DURATION, MAX_DURATION);
                if (EditorGUI.EndChangeCheck())
                {
                    if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);
                    _toggle.CheckmarkFadeDuration = _durationProperty.floatValue;
                }
                EditorGUI.indentLevel--;
            }
            EndFadeGroup();
            //============================================================
            EditorGUI.BeginChangeCheck();
            PropertyField(_fadeTypeProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);

                _toggle.CheckmarkFadeType = _fadeType = (VToggle.FadeType)_fadeTypeProperty.enumValueIndex;
                _showSwitchType.target = !_fadeTypeProperty.hasMultipleDifferentValues && _fadeType == VToggle.FadeType.SwitchCheckmark;
                _showColorType.target = !_fadeTypeProperty.hasMultipleDifferentValues && _fadeType == VToggle.FadeType.ColorCheckmark;
            }

            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            PropertyField(_checkmarkOnProperty, markLabels[_fadeTypeProperty.enumValueIndex]);
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
