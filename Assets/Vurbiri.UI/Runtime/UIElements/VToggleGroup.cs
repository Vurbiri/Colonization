using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.TOGGLE_GROUP, VUI_CONST_ED.TOGGLE_ORDER), DisallowMultipleComponent]
#endif
    public class VToggleGroup : VToggleGroup<VToggle>
    {
        
    }
}
