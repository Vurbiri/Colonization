//Assets\Vurbiri.UI\Editor\Editors\VProgressBarIntEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VProgressBarInt)), CanEditMultipleObjects]
    public class VProgressBarIntEditor : AVProgressBarEditor<int>
	{
        private const string NAME = "Progress Bar Int", RESOURCE = "VProgressBarInt";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        protected override int Value { get => _valueProperty.intValue; set => _valueProperty.intValue = value; }
        protected override int MinValue { get => _minValueProperty.intValue; set => _minValueProperty.intValue = value; }
        protected override int MaxValue { get => _maxValueProperty.intValue; set => _maxValueProperty.intValue = value; }

        protected override int Offset(int value, int rate) => value + rate;

        protected override void DrawValue() => IntSlider(_valueProperty, _minValueProperty.intValue, _maxValueProperty.intValue);

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
