using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VBarInt)), CanEditMultipleObjects]
    public class VBarIntEditor : AVBarEditor<int>
	{
        private const string NAME = VUI_CONST_ED.BAR_INT, RESOURCE = "VBarInt";
        private const string MENU = VUI_CONST_ED.CREATE_MENU_NAME + NAME;

        protected override int Value { get => _valueProperty.intValue; set => _valueProperty.intValue = value; }
        protected override int MinValue { get => _minValueProperty.intValue; set => _minValueProperty.intValue = value; }
        protected override int MaxValue { get => _maxValueProperty.intValue; set => _maxValueProperty.intValue = value; }

        protected override int Offset(int value, int rate) => value + rate;

        protected override void DrawValue()
        {
            EditorGUI.BeginChangeCheck();
            IntSlider(_valueProperty, _minValueProperty.intValue, _maxValueProperty.intValue);
            if (EditorGUI.EndChangeCheck())
                foreach (var bar in _bars)
                    bar.Value = _valueProperty.intValue;
        }
        protected override void DelayedField(SerializedProperty property) => DelayedIntField(property);

        [MenuItem(MENU, false, VUI_CONST_ED.CREATE_MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateObjectFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
