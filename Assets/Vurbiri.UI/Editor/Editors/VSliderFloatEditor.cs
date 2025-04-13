//Assets\Vurbiri.UI\Editor\Editors\VSliderEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VSliderFloat)), CanEditMultipleObjects]
	sealed public class VSliderFloatEditor : AVSliderEditor<float>
    {
        private const string NAME = "Slider Float", RESOURCE = "VSliderFloat";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        protected override void DrawValue()
        {
            Slider(_valueProperty, _minValueProperty.floatValue, _maxValueProperty.floatValue);
        }
        protected override void SetMinValue()
        {
            float minValue = _minValueProperty.floatValue;
            float maxValue = _maxValueProperty.floatValue;

            if (minValue >= maxValue)
            {
                minValue = maxValue - 0.1f;
                _minValueProperty.floatValue = minValue;
            }

            if (_valueProperty.floatValue < minValue)
                _valueProperty.floatValue = minValue;
        }
        protected override void SetMaxValue()
        {
            float minValue = _minValueProperty.floatValue;
            float maxValue = _maxValueProperty.floatValue;

            if (maxValue <= minValue)
            {
                maxValue = minValue + 0.1f;
                _maxValueProperty.floatValue = maxValue;
            }

            if(_valueProperty.floatValue > maxValue)
                _valueProperty.floatValue = maxValue;
        }

        protected override void InitMinMaxValues()
        {
            float minValue = _minValueProperty.floatValue;
            float maxValue = _maxValueProperty.floatValue;

            if (maxValue <= minValue)
            {
                if (minValue <= 0f)
                    maxValue = minValue + 1f;
                else
                    minValue = maxValue - 1f;

                _minValueProperty.floatValue = minValue;
                _maxValueProperty.floatValue = maxValue;
            }
        }

        protected override void DrawStep()
        {
            float delta = _slider.MaxValue - _slider.MinValue;
            float min = delta * VSliderFloat.RATE_STEP_MIN, max = delta * VSliderFloat.RATE_STEP_MAX;
            Slider(_stepProperty, min, max);
        }

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);

        
    }
}