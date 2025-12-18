using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.MENU_NAME + VUI_CONST_ED.BUTTON, VUI_CONST_ED.BUTTON_ORDER)]
#endif
    sealed public class VButton : AVButton
    {
        private VButton() { }
    }
}
