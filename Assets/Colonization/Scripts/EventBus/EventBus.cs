using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [DefaultExecutionOrder(-1)]
    public class EventBus : ASingleton<EventBus>
    {
        public event Action<Crossroad> EventCrossroadSelect;
        public void TriggerCrossroadSelect(Crossroad crossroad) => EventCrossroadSelect?.Invoke(crossroad);

        public event Action<bool> EventCrossroadMarkShow;
        public void TriggerCrossroadMarkShow(bool show) => EventCrossroadMarkShow?.Invoke(show);

        public event Action<bool> EventHexagonIdShow;
        public void TriggerHexagonIdShow(bool show) => EventHexagonIdShow?.Invoke(show);

        public event Action EventEndSceneCreate;
        public void TriggerEndSceneCreate() => EventEndSceneCreate?.Invoke();
    }
}
