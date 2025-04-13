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

        protected override float Value { get => _valueProperty.floatValue; set => _valueProperty.floatValue = value; }
        protected override float MinValue { get => _minValueProperty.floatValue; set => _minValueProperty.floatValue = value; }
        protected override float MaxValue { get => _maxValueProperty.floatValue; set => _maxValueProperty.floatValue = value; }

        protected override float Offset(float value, int rate) => value + 0.1f * rate;

        protected override void DrawValue() => Slider(_valueProperty, _minValueProperty.floatValue, _maxValueProperty.floatValue);
        protected override void DrawStep()
        {
            float delta = _maxValueProperty.floatValue - _minValueProperty.floatValue;
            Slider(_stepProperty, delta * VSliderFloat.RATE_STEP_MIN, delta * VSliderFloat.RATE_STEP_MAX);
        }

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);

        
    }
}