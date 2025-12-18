using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.MENU_NAME + VUI_CONST_ED.TOGGLE, VUI_CONST_ED.TOGGLE_ORDER)]
#endif
    sealed public class VToggle : VToggleGraphic<VToggle>
    {
        
    }
}
