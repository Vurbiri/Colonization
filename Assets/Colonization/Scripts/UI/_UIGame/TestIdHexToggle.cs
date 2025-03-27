//Assets\Colonization\Scripts\UI\_UIGame\TestIdHexToggle.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public class TestIdHexToggle : MonoBehaviour
    {
        private void Start()
        {
            Toggle toggle = GetComponent<Toggle>();
            GameplayTriggerBus eventBus = SceneContainer.Get<GameplayTriggerBus>();

            eventBus.TriggerHexagonIdShow(toggle.isOn);
            toggle.onValueChanged.AddListener(eventBus.TriggerHexagonIdShow);
        }
    }
}
