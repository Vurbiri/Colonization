//Assets\Vurbiri.UI\Editor\Editors\Abstract\AVSliderEditor.cs
using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(AVSlider<>), true), CanEditMultipleObjects]
    public abstract class AVSliderEditor<T> : VSelectableEditor where T : struct, IEquatable<T>, IComparable<T>
    {
        protected delegate T FuncSlider(string label, T value, T min, T max, params GUILayoutOption[] options);
        protected delegate T FuncField(string label, T value, params GUILayoutOption[] options);

        private static readonly string fillRectName = "Fill Rect", handleRectName = "Handle Rect";
        private static readonly string directionName = "Direction";
        private static readonly string minValueName = "Min Value", maxValueName = "Max Value", valueName = "Value";

        protected static FuncSlider DrawSlider;
        protected static FuncField DrawField;

        protected SerializedProperty _stepProperty;
        private SerializedProperty _onValueChangedProperty;

        protected AVSlider<T> _slider;
        protected T _minValue, _maxValue, _value;
        private RectTransform _fillRect, _fillContainerRect, _handleRect, _handleContainerRect;
        private Direction _direction;
        private readonly AnimBool _isCorrectReferences = new();

        protected abstract void CheckMinMaxValues();
        protected abstract void DrawStep();

        sealed protected override void OnEnable()
        {
            _slider = (AVSlider<T>)target;

            _fillRect = _slider.FillRect;
            if (_fillRect != null && _fillRect.parent != null)
                _fillContainerRect = _fillRect.parent.GetComponent<RectTransform>();

            _handleRect = _slider.HandleRect;
            if (_handleRect != null && _handleRect.parent != null)
                _handleContainerRect = _handleRect.parent.GetComponent<RectTransform>();

            _stepProperty = serializedObject.FindProperty("_step");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            _isCorrectReferences.valueChanged.AddListener(Repaint);

            base.OnEnable();
        }

        sealed protected override void OnDisable()
        {
            _isCorrectReferences.valueChanged.RemoveListener(Repaint);
            base.OnDisable();
        }

        sealed protected override void CustomMiddlePropertiesGUI()
        {
            serializedObject.ApplyModifiedProperties();

            bool isChange = false;

            Space();

            BeginChangeCheck();
            _fillRect = VEditorGUILayout.ObjectField(fillRectName, _slider.FillRect);
            if (isChange |= EndChangeCheck())
            {
                _slider.FillRect = _fillRect;
                if(_fillRect != null && _fillRect.parent != null)
                    _fillContainerRect = _fillRect.parent.GetComponent<RectTransform>();
            }

            BeginChangeCheck();
            _handleRect = VEditorGUILayout.ObjectField(handleRectName, _slider.HandleRect);
            if (isChange |= EndChangeCheck())
            {
                _slider.HandleRect = _handleRect;
                if (_handleRect != null && _handleRect.parent != null)
                    _handleContainerRect = _handleRect.parent.GetComponent<RectTransform>();
            }

            _isCorrectReferences.target = (_fillRect & _fillContainerRect) | (_handleRect & _handleContainerRect);

            if (BeginFadeGroup(_isCorrectReferences.faded))
            {
                Space(2f);
                BeginChangeCheck();
                _direction = VEditorGUILayout.EnumPopup(directionName, _slider.Direction);
                if (isChange |= EndChangeCheck())
                {
                    if (!Application.isPlaying) Undo.RecordObjects(serializedObject.targetObjects, "Change Slider Direction");

                    _slider.Direction = _direction;
                }

                Space();

                BeginChangeCheck();
                _value = DrawSlider(valueName, _slider.Value, _slider.MinValue, _slider.MaxValue);
                if (isChange |= EndChangeCheck())
                    _slider.Value = _value;

                indentLevel++;
                BeginChangeCheck();
                _minValue = DrawField(minValueName, _slider.MinValue);
                _maxValue = DrawField(maxValueName, _slider.MaxValue);
                if (isChange |= EndChangeCheck())
                {
                    CheckMinMaxValues();
                    _slider.SetMinMax(_minValue, _maxValue);
                }

                serializedObject.Update();

                DrawStep();
                indentLevel--;

                bool warning;
                if (_direction == Direction.LeftToRight || _direction == Direction.RightToLeft)
                    warning = (_slider.navigation.mode != Navigation.Mode.Automatic && (_slider.FindSelectableOnLeft() != null || _slider.FindSelectableOnRight() != null));
                else
                    warning = (_slider.navigation.mode != Navigation.Mode.Automatic && (_slider.FindSelectableOnDown() != null || _slider.FindSelectableOnUp() != null));

                if (warning)
                    HelpBox("The selected slider direction conflicts with navigation. Not all navigation options may work.", MessageType.Warning);
            }
            EndFadeGroup();

            if (!_isCorrectReferences.value)
            {
                HelpBox("Specify a RectTransform for the slider fill or the slider handle or both. Each must have a parent RectTransform that it can slide within.", MessageType.Info);
            }

            Space();

            if (isChange & !Application.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(_slider.gameObject.scene);
        }

        sealed protected override void CustomEndPropertiesGUI()
        {
            if (_isCorrectReferences.value)
            {
                Space();
                PropertyField(_onValueChangedProperty);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
