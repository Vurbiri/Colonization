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
    public abstract class AVSliderEditor<T> : VSelectableEditor where T : struct, IEquatable<T>, IComparable<T>
    {
        private SerializedProperty _fillRectProperty;
        private SerializedProperty _handleRectProperty;
        private SerializedProperty _directionProperty;
        protected SerializedProperty _valueProperty;
        protected SerializedProperty _minValueProperty;
        protected SerializedProperty _maxValueProperty;
        protected SerializedProperty _stepProperty;
        private SerializedProperty _onValueChangedProperty;

        protected AVSlider<T> _slider;
        protected AVSlider<T>[] _sliders;
        protected int _selectedCount;
        private Direction _direction;
        private readonly AnimBool _isCorrectReferences = new();

        protected abstract void DrawValue();
        protected abstract void InitMinMaxValues();
        protected abstract void SetMinValue();
        protected abstract void SetMaxValue();
        protected abstract void DrawStep();

        sealed protected override void OnEnable()
        {
            _slider = (AVSlider<T>)target;

            _selectedCount = targets.Length;
            _sliders = new AVSlider<T>[_selectedCount];
            for (int i = 0; i < _sliders.Length; i++)
                _sliders[i] = (AVSlider<T>)targets[i];

            _fillRectProperty = serializedObject.FindProperty("_fillRect");
            _handleRectProperty = serializedObject.FindProperty("_handleRect");
            _directionProperty = serializedObject.FindProperty("_direction");
            _valueProperty = serializedObject.FindProperty("_value");
            _minValueProperty = serializedObject.FindProperty("_minValue");
            _maxValueProperty = serializedObject.FindProperty("_maxValue");
            _stepProperty = serializedObject.FindProperty("_step");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            _isCorrectReferences.value = CheckFillRectReferences() || CheckHandleRectReferences();
            _isCorrectReferences.valueChanged.AddListener(Repaint);

            if(!_minValueProperty.hasMultipleDifferentValues & !_maxValueProperty.hasMultipleDifferentValues)
                InitMinMaxValues();

            base.OnEnable();
        }

        sealed protected override void OnDisable()
        {
            _isCorrectReferences.valueChanged.RemoveListener(Repaint);
            base.OnDisable();
        }

        private bool CheckFillRectReferences()
        {
            foreach(var slider in _sliders)
            {
                if (slider.FillRect == null || slider.FillRect.parent == null)
                    return false;
            }
            return true;
        }
        private bool CheckHandleRectReferences()
        {
            foreach (var slider in _sliders)
            {
                if (slider.HandleRect == null || slider.HandleRect.parent == null)
                    return false;
            }
            return true;
        }

        sealed protected override void CustomMiddlePropertiesGUI()
        {

            Space();

            BeginDisabledGroup(_selectedCount > 1);
            {
                BeginChangeCheck();
                PropertyField(_fillRectProperty);
                PropertyField(_handleRectProperty);
                if (EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    _isCorrectReferences.target = CheckFillRectReferences() || CheckHandleRectReferences();
                }
            }
            EndDisabledGroup();

            if (BeginFadeGroup(_isCorrectReferences.faded))
            {
                Space(2f);
                BeginChangeCheck();
                PropertyField(_directionProperty);
                if (EndChangeCheck())
                {
                    _direction = (Direction)_directionProperty.enumValueIndex;
                    if (!Application.isPlaying) Undo.RecordObjects(serializedObject.targetObjects, "Change Slider Direction");

                    foreach (var slider in _sliders)
                        slider.Direction = _direction;
                }
                Space();

                BeginDisabledGroup(_minValueProperty.hasMultipleDifferentValues | _maxValueProperty.hasMultipleDifferentValues);
                     DrawValue();
                EndDisabledGroup();

                indentLevel++;
                BeginChangeCheck();
                PropertyField(_minValueProperty);
                if (EndChangeCheck()) SetMinValue();
                BeginChangeCheck();
                PropertyField(_maxValueProperty);
                if (EndChangeCheck()) SetMaxValue();

                BeginDisabledGroup(_minValueProperty.hasMultipleDifferentValues | _maxValueProperty.hasMultipleDifferentValues);
                    DrawStep();
                EndDisabledGroup();
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
        }

        sealed protected override void CustomEndPropertiesGUI()
        {
            if (_isCorrectReferences.value)
            {
                Space();
                PropertyField(_onValueChangedProperty);
            }
        }
    }
}
