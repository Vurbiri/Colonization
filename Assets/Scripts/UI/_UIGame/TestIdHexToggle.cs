using UnityEngine;
using UnityEngine.UI;

public class TestIdHexToggle : MonoBehaviour
{
    private void Start()
    {
        Toggle toggle = GetComponent<Toggle>();
        EventBus eventBus = EventBus.Instance;

        eventBus.TriggerHexagonIdShow(toggle.isOn);
        toggle.onValueChanged.AddListener(eventBus.TriggerHexagonIdShow);
    }
}
