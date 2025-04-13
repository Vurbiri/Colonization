//Assets\Vurbiri.UI\Editor\Editors\VSliderIntEditor.cs
using System;
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;
using static Vurbiri.UI.VSliderInt;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VSliderInt))]
    sealed public class VSliderIntEditor : AVSliderEditor<int>
    {
        private const string NAME = "Slider Int", RESOURCE = "VSliderInt";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        protected override int Value { get => _valueProperty.intValue; set => _valueProperty.intValue = value; }
        protected override int MinValue { get => _minValueProperty.intValue; set => _minValueProperty.intValue = value; }
        protected override int MaxValue { get => _maxValueProperty.intValue; set => _maxValueProperty.intValue = value; }

        protected override int Offset(int value, int rate) => value + rate;

        protected override void DrawValue()
        {
            BeginChangeCheck();
            IntSlider(_valueProperty, _minValueProperty.intValue, _maxValueProperty.intValue);
            if (EndChangeCheck())
                foreach (var slider in _sliders)
                    slider.Value = _valueProperty.intValue;
        }
        protected override void DrawStep()
        {
            int delta = _maxValueProperty.intValue - _minValueProperty.intValue;
            IntSlider(_stepProperty, Math.Max(delta >> SHIFT_STEP_MIN, STEP_MIN), Math.Max(delta >> SHIFT_STEP_MAX, STEP_MIN));
        }

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
