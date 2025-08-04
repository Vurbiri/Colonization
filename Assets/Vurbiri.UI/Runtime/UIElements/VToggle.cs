using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.TOGGLE, VUI_CONST_ED.TOGGLE_ORDER)]
    [RequireComponent(typeof(RectTransform))]
#endif
    sealed public class VToggle : VToggleGraphic<VToggle>
    {
        
    }
}
