using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    public abstract class ActorSettingsScriptable<TId, TSettings> : ScriptableObjectDisposable where TId : ActorId<TId> where TSettings : ActorSettings
	{
        [SerializeField] private IdArray<TId, TSettings> _settings;

        public abstract Id<ActorTypeId> TypeId { get; }

        public ReadOnlyIdArray<TId, TSettings> Settings => _settings;

        public TSettings this[Id<TId> id] => _settings[id];
        public TSettings this[int index] => _settings[index];

        public TSettings[] Init()
        {
            for (int i = 0; i < ActorId<TId>.Count; i++)
                _settings[i].Init();

            return _settings.Values;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < ActorId<TId>.Count; i++)
                _settings[i].Init_Ed();
        }
#endif
    }
}
