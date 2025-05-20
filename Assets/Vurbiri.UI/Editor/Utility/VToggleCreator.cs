//Assets\Vurbiri.UI\Editor\Utility\VToggleCreator.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    internal class VToggleCreator
	{
        private const string NAME = VUI_CONST_ED.TOGGLE, RESOURCE = "VToggle";
        private const string MENU = VUI_CONST_ED.NAME_CREATE_MENU + NAME;

        [MenuItem(MENU, false, VUI_CONST_ED.CREATE_MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateObjectFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
