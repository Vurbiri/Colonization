using UnityEditor;
using UnityEngine;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    internal class VButtonCreator
    {
        private const string NAME = VUI_CONST_ED.BUTTON, RESOURCE = "VButton";
        private const string MENU = VUI_CONST_ED.CREATE_MENU_NAME + NAME;

        [MenuItem(MENU, false, VUI_CONST_ED.CREATE_MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateObjectFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
