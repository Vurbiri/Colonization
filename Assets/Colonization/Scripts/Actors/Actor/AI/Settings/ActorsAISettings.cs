using Newtonsoft.Json;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract class ActorsAISettings<TId> where TId : ActorId<TId>
    {
        [SerializeField, JsonProperty, HideInInspector] 
        private ActorAISettings[] _values;

        public ActorAISettings this[Id<TId> id] { [Impl(256)] get => _values[id.Value]; }
        public ActorAISettings this[int index] { [Impl(256)] get => _values[index]; }

        public ActorsAISettings() { }

#if UNITY_EDITOR
        public void OnValidate()
        {
            _values ??= new ActorAISettings[ActorId<TId>.Count];
            if(_values.Length != ActorId<TId>.Count)
                System.Array.Resize(ref _values, ActorId<TId>.Count);
            for(int i = 0; i < ActorId<TId>.Count; i++)
                _values[i] ??= new();
        }
#endif
    }
}
