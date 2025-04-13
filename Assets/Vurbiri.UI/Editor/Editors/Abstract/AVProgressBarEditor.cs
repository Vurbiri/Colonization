//Assets\Vurbiri.UI\Editor\Editors\Abstract\AVProgressBarEditor.cs
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
    public abstract class AVProgressBarEditor<T> : Editor where T : struct, IEquatable<T>, IComparable<T>
    {
        private SerializedProperty _fillRectProperty;
        private SerializedProperty _directionProperty;
        protected SerializedProperty _valueProperty;
        protected SerializedProperty _minValueProperty;
        protected SerializedProperty _maxValueProperty;
        private SerializedProperty _useGradientProperty;
        private SerializedProperty _gradientProperty;

        private AVProgressBar<T> _bar;
        protected AVProgressBar<T>[] _bars;
        private int _selectedCount;
        private Transform _fillRect, _fillContainerRect;
        private Graphic _fillGraphic;
        private Direction _direction;
        private readonly AnimBool _isCorrectReferences = new(), _showGradient = new();

        protected abstract T Value { get; set; }
        protected abstract T MinValue { get; set; }
        protected abstract T MaxValue { get; set; }
        protected abstract T Offset(T value, int rate);

        protected abstract void DrawValue();

        private void OnEnable()
        {
            _bar = (AVProgressBar<T>)target;
            _selectedCount = targets.Length;
            _bars = new AVProgressBar<T>[_selectedCount];
            for (int i = 0; i < _selectedCount; i++)
                _bars[i] = (AVProgressBar<T>)targets[i];

            _direction = _bar.Direction;

            _fillRectProperty = serializedObject.FindProperty("_fillRect");
            _directionProperty = serializedObject.FindProperty("_direction");
            _valueProperty = serializedObject.FindProperty("_value");
            _minValueProperty = serializedObject.FindProperty("_minValue");
            _maxValueProperty = serializedObject.FindProperty("_maxValue");
            _useGradientProperty = serializedObject.FindProperty("_useGradient");
            _gradientProperty = serializedObject.FindProperty("_gradient");

            _isCorrectReferences.valueChanged.AddListener(Repaint);
            _showGradient.valueChanged.AddListener(Repaint);

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
            _showGradient.valueChanged.RemoveListener(Repaint);
        }

        private void UpdateFillRectReferences()
        {
            _fillContainerRect = null; _fillGraphic = null;
            _fillRect = _fillRectProperty.objectReferenceValue as Transform;
            if (_fillRect != null && _fillRect.parent != null)
            {
                _fillContainerRect = _fillRect.parent;
                _fillGraphic = _fillRect.GetComponent<Graphic>();
            }
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
                PropertyField(_fillRectProperty);
            EndDisabledGroup();

            UpdateFillRectReferences();
            _isCorrectReferences.target = _fillRect & _fillContainerRect;
            _showGradient.target = _fillGraphic;

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
                PropertyField(_minValueProperty);
                SetMinValue();

                PropertyField(_maxValueProperty);
                SetMaxValue();
                indentLevel--;

                if (BeginFadeGroup(_showGradient.faded))
                {
                    Space();
                    BeginChangeCheck();
                    PropertyField(_useGradientProperty);

                    if (_useGradientProperty.boolValue)
                        PropertyField(_gradientProperty);

                    foreach (var bar in _bars)
                        bar.UseGradient = _useGradientProperty.boolValue;
                }
                EndFadeGroup();
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
