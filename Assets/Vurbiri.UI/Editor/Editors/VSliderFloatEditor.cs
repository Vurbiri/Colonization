//Assets\Vurbiri.UI\Editor\Editors\VSliderEditor.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VSliderFloat), true), CanEditMultipleObjects]
	public class VSliderFloatEditor : VSelectableEditor
    {
        private SerializedProperty _directionProperty;
        private SerializedProperty _fillRectProperty;
        private SerializedProperty _handleRectProperty;
        private SerializedProperty _minValueProperty;
        private SerializedProperty _maxValueProperty;
        private SerializedProperty _stepProperty;
        private SerializedProperty _valueProperty;
        private SerializedProperty _onValueChangedProperty;

        private VSliderFloat _slider;

        protected override void OnEnable()
        {
            _slider = (VSliderFloat)target;

            _fillRectProperty = serializedObject.FindProperty("_fillRect");
            _handleRectProperty = serializedObject.FindProperty("_handleRect");
            _directionProperty = serializedObject.FindProperty("_direction");
            _minValueProperty = serializedObject.FindProperty("_minValue");
            _maxValueProperty = serializedObject.FindProperty("_maxValue");
            _stepProperty = serializedObject.FindProperty("_step");
            _valueProperty = serializedObject.FindProperty("_value");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            base.OnEnable();
        }

        protected override void CustomMiddlePropertiesGUI()
        {
            Space();

            BeginChangeCheck();
            PropertyField(_fillRectProperty);
            if (EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_slider.gameObject.scene);
                _slider.FillRect = _fillRectProperty.objectReferenceValue as RectTransform;

            }
            BeginChangeCheck();
            PropertyField(_handleRectProperty);
            if (EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_slider.gameObject.scene);
                _slider.HandleRect = _handleRectProperty.objectReferenceValue as RectTransform;
            }

            if (_fillRectProperty.objectReferenceValue != null | _handleRectProperty.objectReferenceValue != null)
            {
                BeginChangeCheck();
                PropertyField(_directionProperty);
                if (EndChangeCheck())
                {
                    
                    if (!Application.isPlaying)
                    {
                        Undo.RecordObjects(serializedObject.targetObjects, "Change Slider Direction");
                        EditorSceneManager.MarkSceneDirty(_slider.gameObject.scene);
                    }

                    _slider.Direction = (Direction)_directionProperty.enumValueIndex;
                }

                Space();

                BeginChangeCheck();
                float newValue = Slider(_valueProperty.displayName, _valueProperty.floatValue, _minValueProperty.floatValue, _maxValueProperty.floatValue);
                if (EndChangeCheck())
                {
                    if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_slider.gameObject.scene);
                    _slider.Value = newValue;
                }

                indentLevel++;
                BeginChangeCheck();
                float newMin = FloatField("Min Value", _minValueProperty.floatValue);
                if (EndChangeCheck())
                {
                    if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_slider.gameObject.scene);

                    _slider.MinValue = newMin;
                }

                BeginChangeCheck();
                float newMax = FloatField("Max Value", _maxValueProperty.floatValue);
                if (EndChangeCheck())
                {
                    if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_slider.gameObject.scene);

                    _slider.MaxValue = newMax;
                }

                float delta = _slider.MaxValue - _slider.MinValue;
                Slider(_stepProperty, delta * VSliderFloat.RATE_STEP_MIN, delta * VSliderFloat.RATE_STEP_MAX);
                indentLevel--;

                bool warning;
                Direction dir = _slider.Direction;
                if (dir == Direction.LeftToRight || dir == Direction.RightToLeft)
                    warning = (_slider.navigation.mode != Navigation.Mode.Automatic && (_slider.FindSelectableOnLeft() != null || _slider.FindSelectableOnRight() != null));
                else
                    warning = (_slider.navigation.mode != Navigation.Mode.Automatic && (_slider.FindSelectableOnDown() != null || _slider.FindSelectableOnUp() != null));

                if (warning)
                    HelpBox("The selected slider direction conflicts with navigation. Not all navigation options may work.", MessageType.Warning);
            }
            else
            {
                HelpBox("Specify a RectTransform for the slider fill or the slider handle or both. Each must have a parent RectTransform that it can slide within.", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
            Space();
        }

        protected override void CustomEndPropertiesGUI()
        {
            if (_fillRectProperty.objectReferenceValue != null || _handleRectProperty.objectReferenceValue != null)
            {
                Space();
                PropertyField(_onValueChangedProperty);
            }
        }
    }
}