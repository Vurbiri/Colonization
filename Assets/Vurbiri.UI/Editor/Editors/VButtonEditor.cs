//Assets\Vurbiri.UI\Editor\Editors\CmButtonEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VButton)), CanEditMultipleObjects]
    sealed public class VButtonEditor : AVButtonEditor
    {
        private const string NAME = "Button", RESOURCE = "VButton";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        protected override bool IsDerivedEditor => GetType() != typeof(VButtonEditor);

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateObjectFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
