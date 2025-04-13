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

        protected override void DrawValue()
        {
            IntSlider(_valueProperty, _minValueProperty.intValue, _maxValueProperty.intValue);
        }

        protected override void SetMinValue()
        {
            int minValue = _minValueProperty.intValue;
            int maxValue = _maxValueProperty.intValue;

            if (minValue >= maxValue)
            {
                minValue = maxValue - 1;
                _minValueProperty.intValue = minValue;
            }

            if (_valueProperty.intValue < minValue)
                _valueProperty.intValue = minValue;
        }

        protected override void SetMaxValue()
        {
            int minValue = _minValueProperty.intValue;
            int maxValue = _maxValueProperty.intValue;

            if (maxValue <= minValue)
            {
                maxValue = minValue + 1;
                _maxValueProperty.intValue = maxValue;
            }

            if (_valueProperty.intValue > maxValue)
                _valueProperty.intValue = maxValue;
        }

        protected override void InitMinMaxValues()
        {
            int minValue = _minValueProperty.intValue;
            int maxValue = _maxValueProperty.intValue;

            if (maxValue <= minValue)
            {
                if (minValue <= 0f)
                    maxValue = minValue + 10;
                else
                    minValue = maxValue - 10;

                _minValueProperty.intValue = minValue;
                _maxValueProperty.intValue = maxValue;
            }
        }

        protected override void DrawStep()
        {
            int delta = _slider.MaxValue - _slider.MinValue;
            int min = Math.Max(delta >> SHIFT_STEP_MIN, STEP_MIN), max = Math.Max(delta >> SHIFT_STEP_MAX, STEP_MIN);
            IntSlider(_stepProperty, min, max);
        }

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
