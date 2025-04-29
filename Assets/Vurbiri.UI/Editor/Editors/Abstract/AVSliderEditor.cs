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

        private AVSlider<T> _slider;
        protected AVSlider<T>[] _sliders;
        private int _selectedCount;
        private Direction _direction;
        private readonly AnimBool _isCorrectReferences = new();

        protected abstract T Value { get; set; }
        protected abstract T MinValue { get; set; }
        protected abstract T MaxValue { get; set; }
        protected abstract T Offset(T value, int rate);

        protected abstract void DrawValue();
        protected abstract void DelayedField(SerializedProperty property);
        protected abstract void DrawStep();

        sealed protected override void OnEnable()
        {
            _slider = (AVSlider<T>)target;

            _selectedCount = targets.Length;
            _sliders = new AVSlider<T>[_selectedCount];
            for (int i = 0; i < _selectedCount; i++)
                _sliders[i] = (AVSlider<T>)targets[i];

            _direction = _slider.Direction;

            _fillRectProperty       = serializedObject.FindProperty("_fillRect");
            _handleRectProperty     = serializedObject.FindProperty("_handleRect");
            _directionProperty      = serializedObject.FindProperty("_direction");
            _valueProperty          = serializedObject.FindProperty("_value");
            _minValueProperty       = serializedObject.FindProperty("_minValue");
            _maxValueProperty       = serializedObject.FindProperty("_maxValue");
            _stepProperty           = serializedObject.FindProperty("_step");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            _isCorrectReferences.value = CheckReferences();
            _isCorrectReferences.valueChanged.AddListener(Repaint);

            if (!_minValueProperty.hasMultipleDifferentValues & !_maxValueProperty.hasMultipleDifferentValues)
            {
                if (_slider.MaxValue.CompareTo(_slider.MinValue) <= 0)
                    _slider.MaxValue = Offset(_slider.MinValue, 10);
            }

            base.OnEnable();
        }

        sealed protected override void OnDisable()
        {
            _isCorrectReferences.valueChanged.RemoveListener(Repaint);
            base.OnDisable();
        }

        private bool CheckReferences()
        {
            RectTransform fillRect = _fillRectProperty.objectReferenceValue as RectTransform;
            RectTransform handleRect = _handleRectProperty.objectReferenceValue as RectTransform;

            _slider.UpdateTracker();

            return (fillRect != null && fillRect.parent != null) || (handleRect != null && handleRect.parent != null);
        }

        protected void SetMinValue()
        {
            if (MinValue.CompareTo(MaxValue) >= 0)
                MinValue = Offset(MaxValue, -1);

            if (Value.CompareTo(MinValue) < 0)
                Value = MinValue;
        }
        protected void SetMaxValue()
        {
            if (MaxValue.CompareTo(MinValue) <= 0)
                MaxValue = Offset(MinValue, 1);

            if (Value.CompareTo(MaxValue) > 0)
                Value = MaxValue;
        }

        sealed protected override void CustomMiddlePropertiesGUI()
        {

            Space();

            BeginDisabledGroup(_selectedCount > 1);
            {
                PropertyField(_fillRectProperty);
                PropertyField(_handleRectProperty);
            }
            EndDisabledGroup();

            _isCorrectReferences.target = CheckReferences();

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

                DrawValue();

                indentLevel++;
                DelayedField(_minValueProperty); 
                SetMinValue();

                DelayedField(_maxValueProperty);
                SetMaxValue();

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
