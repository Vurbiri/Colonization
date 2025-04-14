//Assets\Vurbiri.UI\Editor\Editors\VBarFloatEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VBarFloat)), CanEditMultipleObjects]
    sealed public class VBarFloatEditor : AVBarEditor<float>
    {
        private const string NAME = "Bar Float", RESOURCE = "VBarFloat";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        protected override float Value { get => _valueProperty.floatValue; set => _valueProperty.floatValue = value; }
        protected override float MinValue { get => _minValueProperty.floatValue; set => _minValueProperty.floatValue = value; }
        protected override float MaxValue { get => _maxValueProperty.floatValue; set => _maxValueProperty.floatValue = value; }

        protected override float Offset(float value, int rate) => value + 0.1f * rate;

        protected override void DrawValue()
        {
            EditorGUI.BeginChangeCheck();
            Slider(_valueProperty, _minValueProperty.floatValue, _maxValueProperty.floatValue);
            if (EditorGUI.EndChangeCheck())
                foreach (var bar in _bars)
                    bar.Value = _valueProperty.floatValue;
        }

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
