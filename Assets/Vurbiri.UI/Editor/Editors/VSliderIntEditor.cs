//Assets\Vurbiri.UI\Editor\Editors\VSliderIntEditor.cs
using System;
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static Vurbiri.UI.VSliderInt;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VSliderInt), true), CanEditMultipleObjects]
    public class VSliderIntEditor : AVSliderEditor<int>
    {
        private const string NAME = "Slider Int", RESOURCE = "VSliderInt";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        static VSliderIntEditor()
        {
            DrawSlider = EditorGUILayout.IntSlider;
            DrawField = EditorGUILayout.IntField;
        }

        protected override void CheckMinMaxValues()
        {
            if (_maxValue <= _minValue)
            {
                if (_minValue <= 0)
                    _maxValue = _minValue + 10;
                else
                    _minValue = _maxValue - 10;
            }
        }

        protected override void DrawStep()
        {
            int delta = _slider.MaxValue - _slider.MinValue;
            EditorGUILayout.IntSlider(_stepProperty, Math.Max(delta >> SHIFT_STEP_MIN, STEP_MIN), Math.Max(delta >> SHIFT_STEP_MAX, STEP_MIN));
        }

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromPrefab(RESOURCE, NAME, command.context as GameObject);
    }
}
