using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public class TestIdHexToggle : MonoBehaviour
    {
        private void Start()
        {
            Toggle toggle = GetComponent<Toggle>();
            GameplayEventBus eventBus = SceneServices.Get<GameplayEventBus>();

            eventBus.TriggerHexagonIdShow(toggle.isOn);
            toggle.onValueChanged.AddListener(eventBus.TriggerHexagonIdShow);
        }
    }
}
