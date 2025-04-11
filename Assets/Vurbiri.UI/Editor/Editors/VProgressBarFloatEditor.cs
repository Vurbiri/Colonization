//Assets\Vurbiri.UI\Editor\Editors\VProgressBarFloatEditor.cs
using UnityEditor;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VProgressBarFloat), true), CanEditMultipleObjects]
    public class VProgressBarFloatEditor : AVProgressBarEditor<float>
    {
        //private const string NAME = "Progress Bar Float", RESOURCE = "VProgressBarFloat";
        //private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        static VProgressBarFloatEditor()
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

        //[MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        //public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
