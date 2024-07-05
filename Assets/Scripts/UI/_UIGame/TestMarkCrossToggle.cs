using UnityEngine;
using UnityEngine.UI;

public class TestMarkCrossToggle : MonoBehaviour
{
    private void Start()
    {
        Toggle toggle = GetComponent<Toggle>();
        EventBus eventBus = EventBus.Instance;

        eventBus.TriggerCrossroadMarkShow(toggle.isOn);
        toggle.onValueChanged.AddListener(eventBus.TriggerCrossroadMarkShow);
    }
}
