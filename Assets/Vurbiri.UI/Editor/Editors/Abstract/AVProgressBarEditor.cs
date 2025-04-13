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
        protected delegate T FuncSlider(string label, T value, T min, T max, params GUILayoutOption[] options);
        protected delegate T FuncField(string label, T value, params GUILayoutOption[] options);

        private static readonly string fillRectName = "Fill Rect";
        private static readonly string directionName = "Direction";
        private static readonly string minValueName = "Min", maxValueName = "Max", valueName = "Value";
        private static readonly string useGradientName = "Use Gradient";

        protected static FuncSlider DrawSlider;
        protected static FuncField DrawField;

        private SerializedProperty _gradientProperty;

        protected AVProgressBar<T> _bar;
        protected T _minValue, _maxValue, _value;
        private RectTransform _fillRect, _fillContainerRect;
        private Graphic _fillGraphic;
        private Direction _direction;
        private readonly AnimBool _isCorrectReferences = new(), _showGradient = new();

        protected abstract bool CheckMinMaxValues();

        private void OnEnable()
        {
            _bar = (AVProgressBar<T>)target;

            _fillRect = _bar.FillRect;
            UpdateFillRectReferences();

            _minValue = _bar.MinValue; _maxValue = _bar.MaxValue;
            if (CheckMinMaxValues())
                _bar.SetMinMax(_minValue, _maxValue);

            _gradientProperty = serializedObject.FindProperty("_gradient");

            _isCorrectReferences.valueChanged.AddListener(Repaint);
            _showGradient.valueChanged.AddListener(Repaint);
        }

        private void OnDisable()
        {
            _isCorrectReferences.valueChanged.RemoveListener(Repaint);
            _showGradient.valueChanged.RemoveListener(Repaint);
        }

        private void UpdateFillRectReferences()
        {
            _fillContainerRect = null; _fillGraphic = null;
            if (_fillRect != null && _fillRect.parent != null)
            {
                _fillContainerRect = (RectTransform)_fillRect.parent;
                _fillGraphic = _fillRect.GetComponent<Graphic>();
            }
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
                UpdateFillRectReferences();
            }

            _isCorrectReferences.target = _fillRect & _fillContainerRect;
            _showGradient.target = _fillGraphic;

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
                indentLevel--;
                serializedObject.Update();

                if (BeginFadeGroup(_showGradient.faded))
                {
                    Space();
                    BeginChangeCheck();
                    bool useGradient = Toggle(useGradientName, _bar.UseGradient);
                    if (isChange |= EndChangeCheck())
                    {
                        _bar.UseGradient = useGradient;
                        serializedObject.Update();
                    }

                    if (useGradient)
                    {
                        BeginChangeCheck();
                        PropertyField(_gradientProperty);
                        if (EndChangeCheck())
                        {
                            serializedObject.ApplyModifiedProperties();
                            _bar.UpdateColor();
                        }
                    }
                }
                EndFadeGroup();
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
