using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VBarFloat)), CanEditMultipleObjects]
    sealed public class VBarFloatEditor : AVBarEditor<float>
    {
        private const string NAME = VUI_CONST_ED.BAR_FLOAT, RESOURCE = "VBarFloat";
        private const string MENU = VUI_CONST_ED.CREATE_MENU_NAME + NAME;

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
        protected override void DelayedField(SerializedProperty property) => DelayedFloatField(property);

        [MenuItem(MENU, false, VUI_CONST_ED.CREATE_MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateObjectFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
