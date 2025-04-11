//Assets\Vurbiri.UI\Editor\Editors\Abstract\AVProgressBarEditor.cs
using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(AVProgressBar<>), true), CanEditMultipleObjects]
    public abstract class AVProgressBarEditor<T> : Editor where T : struct, IEquatable<T>, IComparable<T>
    {
        protected delegate T FuncSlider(string label, T value, T min, T max, params GUILayoutOption[] options);
        protected delegate T FuncField(string label, T value, params GUILayoutOption[] options);

        private static readonly string fillRectName = "Fill Rect";
        private static readonly string directionName = "Direction";
        private static readonly string minValueName = "Min Value", maxValueName = "Max Value", valueName = "Value";

        protected static FuncSlider DrawSlider;
        protected static FuncField DrawField;

        private SerializedProperty _gradientProperty;
        private SerializedProperty _onValueChangedProperty;

        protected AVProgressBar<T> _bar;
        protected T _minValue, _maxValue, _value;
        private RectTransform _fillRect, _fillContainerRect;
        private Direction _direction;
        private readonly AnimBool _isCorrectReferences = new();

        protected abstract void CheckMinMaxValues();

        private void OnEnable()
        {
            _bar = (AVProgressBar<T>)target;

            _fillRect = _bar.FillRect;
            if (_fillRect != null && _fillRect.parent != null)
                _fillContainerRect = _fillRect.parent.GetComponent<RectTransform>();

            _gradientProperty = serializedObject.FindProperty("_gradient");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            _isCorrectReferences.valueChanged.AddListener(Repaint);
        }

        private void OnDisable()
        {
            _isCorrectReferences.valueChanged.RemoveListener(Repaint);
        }

        sealed public override void OnInspectorGUI()
        {
            bool isChange = false;

            Space(2f);
            BeginChangeCheck();
            _fillRect = VEditorGUILayout.ObjectField(fillRectName, _bar.FillRect);
            if (isChange |= EndChangeCheck())
            {
                _bar.FillRect = _fillRect;
                if (_fillRect && _fillRect.parent)
                    _fillContainerRect = _fillRect.parent.GetComponent<RectTransform>();
            }

            _isCorrectReferences.target = _fillRect & _fillContainerRect;

            if (BeginFadeGroup(_isCorrectReferences.faded))
            {
                Space(2f);

                BeginChangeCheck();
                _direction = VEditorGUILayout.EnumPopup(directionName, _bar.Direction);
                if (isChange |= EndChangeCheck())
                {
                    if (!Application.isPlaying) Undo.RecordObjects(serializedObject.targetObjects, "Change Progress Bar Direction");

                    _bar.Direction = _direction;
                }

                Space();
                BeginChangeCheck();
                _value = DrawSlider(valueName, _bar.Value, _bar.MinValue, _bar.MaxValue);
                if (isChange |= EndChangeCheck())
                    _bar.Value = _value;

                indentLevel++;
                BeginChangeCheck();
                _minValue = DrawField(minValueName, _bar.MinValue);
                _maxValue = DrawField(maxValueName, _bar.MaxValue);
                if (isChange |= EndChangeCheck())
                {
                    CheckMinMaxValues();
                    _bar.SetMinMax(_minValue, _maxValue);
                }

                serializedObject.Update();
                indentLevel--;

                Space();
                PropertyField(_gradientProperty);

                Space();
                PropertyField(_onValueChangedProperty);

                serializedObject.ApplyModifiedProperties();
            }
            EndFadeGroup();

            if (!_isCorrectReferences.value)
            {
                HelpBox("Specify a RectTransform for the progress bar fill. It must have a parent RectTransform that it can move within.", MessageType.Info);
            }

            if (isChange & !Application.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(_bar.gameObject.scene);
        }
    }
}
