using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    public class Version : MonoBehaviour
    {
        void Start() => GetComponent<TMPro.TextMeshProUGUI>().text = Application.version;
    }
}
