using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class EventBus : ASingleton<EventBus>
{
    public event Action<Crossroad> EventCrossroadSelect;
    public void TriggerCrossroadSelect(Crossroad crossroad) => EventCrossroadSelect?.Invoke(crossroad);

    public event Action<bool> EventCrossroadMarkShow;
    public void TriggerCrossroadMarkShow(bool show) => EventCrossroadMarkShow?.Invoke(show);
}
