using UnityEngine;

namespace Vurbiri.UI
{
    
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.UTILITY_MENU_NAME + "Version", VUI_CONST_ED.UTILITY_MENU_ORDER)]
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
#endif
    public class Version : MonoBehaviour
    {
        [SerializeField] private string _affix;
        
        void Start()
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = string.IsNullOrEmpty(_affix) ? Application.version : _affix.Concat(Application.version);
            Destroy(this);
        }
    }
}
