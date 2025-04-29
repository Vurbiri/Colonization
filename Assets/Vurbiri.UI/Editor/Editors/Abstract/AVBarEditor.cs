//Assets\Vurbiri.UI\Editor\Editors\Abstract\AVBarEditor.cs
using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    public abstract class AVBarEditor<T> : Editor where T : struct, IEquatable<T>, IComparable<T>
    {
        private SerializedProperty _fillRectProperty;
        private SerializedProperty _directionProperty;
        protected SerializedProperty _valueProperty;
        protected SerializedProperty _minValueProperty;
        protected SerializedProperty _maxValueProperty;
        private SerializedProperty _useGradientProperty;
        private SerializedProperty _gradientProperty;

        private AVBar<T> _bar;
        protected AVBar<T>[] _bars;
        private int _selectedCount;
        private Transform _fillRect, _fillContainerRect;
        private Direction _direction;
        private readonly AnimBool _isCorrectReferences = new(), _showGradient = new();

        protected abstract T Value { get; set; }
        protected abstract T MinValue { get; set; }
        protected abstract T MaxValue { get; set; }
        protected abstract T Offset(T value, int rate);

        protected abstract void DrawValue();
        protected abstract void DelayedField(SerializedProperty property);

        private void OnEnable()
        {
            _bar = (AVBar<T>)target;
            _selectedCount = targets.Length;
            _bars = new AVBar<T>[_selectedCount];
            for (int i = 0; i < _selectedCount; i++)
                _bars[i] = (AVBar<T>)targets[i];

            _direction = _bar.Direction;

            _fillRectProperty    = serializedObject.FindProperty("_fillRect");
            _directionProperty   = serializedObject.FindProperty("_direction");
            _valueProperty       = serializedObject.FindProperty("_value");
            _minValueProperty    = serializedObject.FindProperty("_minValue");
            _maxValueProperty    = serializedObject.FindProperty("_maxValue");
            _useGradientProperty = serializedObject.FindProperty("_useGradient");
            _gradientProperty    = serializedObject.FindProperty("_gradient");

            _isCorrectReferences.valueChanged.AddListener(Repaint);

            UpdateFillRectReferences();

            if (!_minValueProperty.hasMultipleDifferentValues & !_maxValueProperty.hasMultipleDifferentValues)
            {
                if (_bar.MaxValue.CompareTo(_bar.MinValue) <= 0)
                    _bar.MaxValue = Offset(_bar.MinValue, 10);
            }
        }

        private void OnDisable()
        {
            _isCorrectReferences.valueChanged.RemoveListener(Repaint);
        }

        private void UpdateFillRectReferences()
        {
            _fillContainerRect = null;
            _fillRect = _fillRectProperty.objectReferenceValue as Transform;
            if (_fillRect != null && _fillRect.parent != null)
                _fillContainerRect = _fillRect.parent;

            _bar.UpdateTracker();
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

        sealed public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Space(2f);
            BeginDisabledGroup(_selectedCount > 1);
            {
                PropertyField(_fillRectProperty);
            }
            EndDisabledGroup();

            UpdateFillRectReferences();
            _isCorrectReferences.target = _fillRect & _fillContainerRect;

            if (BeginFadeGroup(_isCorrectReferences.faded))
            {
                Space(2f);
                BeginChangeCheck();
                    PropertyField(_directionProperty);
                if (EndChangeCheck())
                {
                    _direction = (Direction)_directionProperty.enumValueIndex;
                    if (!Application.isPlaying) Undo.RecordObjects(serializedObject.targetObjects, "Change Slider Direction");

                    foreach (var bar in _bars)
                        bar.Direction = _direction;
                }
                Space();

                DrawValue();

                indentLevel++;
                DelayedField(_minValueProperty);
                SetMinValue();

                DelayedField(_maxValueProperty);
                SetMaxValue();
                indentLevel--;

                Space();
                PropertyField(_useGradientProperty);

                if (_useGradientProperty.boolValue)
                    PropertyField(_gradientProperty);
            }
            EndFadeGroup();

            if (!_isCorrectReferences.value)
            {
                HelpBox("Specify a RectTransform for the progress bar fill. It must have a parent RectTransform that it can move within.", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
