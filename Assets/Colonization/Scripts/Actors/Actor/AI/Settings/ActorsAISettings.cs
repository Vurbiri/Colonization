using Newtonsoft.Json;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract class ActorsAISettings<TActorId, TStateId> where TActorId : ActorId<TActorId> where TStateId : ActorAIStateId<TStateId>
    {
        [SerializeField, JsonProperty, HideInInspector]
        private ReadOnlyArray<int> _priority;

        [SerializeField, JsonProperty, HideInInspector] 
        private ActorAISettings[] _settings;

        public ActorAISettings this[Id<TActorId> id] { [Impl(256)] get => _settings[id.Value]; }
        public ActorAISettings this[int index] { [Impl(256)] get => _settings[index]; }

        public ReadOnlyArray<int> Priority { [Impl(256)] get => _priority; }

        public ActorsAISettings() { }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if(_priority == null || _priority.Count != ActorAIStateId<TStateId>.Count)
                _priority = ActorAIStateId<TStateId>.Values_Ed.ToArray();

            _settings ??= new ActorAISettings[ActorId<TActorId>.Count];
            if(_settings.Length != ActorId<TActorId>.Count)
                System.Array.Resize(ref _settings, ActorId<TActorId>.Count);
            for (int i = 0; i < ActorId<TActorId>.Count; ++i)
                _settings[i] ??= new();
        }
#endif
    }
}
