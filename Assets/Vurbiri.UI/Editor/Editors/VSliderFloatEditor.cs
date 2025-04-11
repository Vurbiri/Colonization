//Assets\Vurbiri.UI\Editor\Editors\VSliderEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VSliderFloat), true), CanEditMultipleObjects]
	sealed public class VSliderFloatEditor : AVSliderEditor<float>
    {
        private const string NAME = "Slider Float", RESOURCE = "VSliderFloat";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        static VSliderFloatEditor()
        {
            DrawSlider = EditorGUILayout.Slider;
            DrawField = EditorGUILayout.FloatField;
        }

        protected override void CheckMinMaxValues()
        {
            if (_maxValue <= _minValue)
            {
                if (_minValue <= 0f)
                    _maxValue = _minValue + 1f;
                else
                    _minValue = _maxValue - 1f;
            }
        }

        protected override void DrawStep()
        {
            float delta = _slider.MaxValue - _slider.MinValue;
            EditorGUILayout.Slider(_stepProperty, delta * VSliderFloat.RATE_STEP_MIN, delta * VSliderFloat.RATE_STEP_MAX);
        }

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}