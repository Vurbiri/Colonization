//Assets\Vurbiri.UI\Editor\Editors\VProgressBarIntEditor.cs
using UnityEditor;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VProgressBarInt))]
    public class VProgressBarIntEditor : AVProgressBarEditor<int>
	{
        //private const string NAME = "Progress Bar Int", RESOURCE = "VProgressBarInt";
        //private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        static VProgressBarIntEditor()
        {
            DrawSlider = EditorGUILayout.IntSlider;
            DrawField = EditorGUILayout.IntField;
        }

        protected override bool CheckMinMaxValues()
        {
            if (_maxValue <= _minValue)
            {
                if (_minValue <= 0)
                    _maxValue = _minValue + 10;
                else
                    _minValue = _maxValue - 10;

                return true;
            }
            return false;
        }

        //[MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        //public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
