//Assets\Vurbiri.UI\Runtime\Utility\Version.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    public class Version : MonoBehaviour
    {
        void Start() => GetComponent<TMPro.TMP_Text>().text = Application.version;
    }
}
