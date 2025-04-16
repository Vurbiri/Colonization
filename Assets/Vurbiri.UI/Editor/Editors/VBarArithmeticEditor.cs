//Assets\Vurbiri.UI\Editor\Editors\VBarArithmeticEditor.cs
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VBarArithmetic)), CanEditMultipleObjects]
    public class VBarArithmeticEditor : Editor
    {
        private const string NAME = "Bar Arithmetic", RESOURCE = "VBarArithmetic";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        private SerializedProperty _fillRectProperty;
        private SerializedProperty _directionProperty;
        private SerializedProperty _valueProperty;
        private SerializedProperty _differenceProperty;
        private SerializedProperty _maxStepsProperty;
        private SerializedProperty _useGradientProperty;
        private SerializedProperty _gradientProperty;

        private VBarArithmetic _bar;
        private VBarArithmetic[] _bars;
        private int _selectedCount;
        private Transform _fillRect, _fillContainerRect;
        private Direction _direction;
        private readonly AnimBool _isCorrectReferences = new(), _showGradient = new();

        private void OnEnable()
        {
            _bar = (VBarArithmetic)target;
            _selectedCount = targets.Length;
            _bars = new VBarArithmetic[_selectedCount];
            for (int i = 0; i < _selectedCount; i++)
                _bars[i] = (VBarArithmetic)targets[i];

            _direction = _bar.Direction;

            _fillRectProperty = serializedObject.FindProperty("_fillRect");
            _directionProperty = serializedObject.FindProperty("_direction");
            _valueProperty = serializedObject.FindProperty("_value");
            _differenceProperty = serializedObject.FindProperty("_difference");
            _maxStepsProperty = serializedObject.FindProperty("_maxSteps");
            _useGradientProperty = serializedObject.FindProperty("_useGradient");
            _gradientProperty = serializedObject.FindProperty("_gradient");

            _isCorrectReferences.valueChanged.AddListener(Repaint);

            UpdateFillRectReferences();
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

                DelayedIntField(_differenceProperty);
                if(_differenceProperty.intValue < 1)
                    _differenceProperty.intValue = 1;

                DelayedIntField(_maxStepsProperty);
                if (_maxStepsProperty.intValue < 1)
                    _maxStepsProperty.intValue = 1;

                if(!_differenceProperty.hasMultipleDifferentValues && !_maxStepsProperty.hasMultipleDifferentValues)
                    LabelField("Max Value", _bar.MaxValue.ToString());

                BeginChangeCheck();
                IntSlider(_valueProperty, 0, _bar.MaxValue);
                if (EndChangeCheck())
                {
                    foreach (var bar in _bars)
                        bar.Value = _valueProperty.intValue;
                }

                if (!_differenceProperty.hasMultipleDifferentValues && !_maxStepsProperty.hasMultipleDifferentValues && !_valueProperty.hasMultipleDifferentValues)
                {
                    BeginDisabledGroup(true);
                        IntSlider("Step", _bar.Step, 0, _maxStepsProperty.intValue);
                    EndDisabledGroup();
                }

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

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateObjectFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
