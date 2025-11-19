using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public abstract partial class ActorSettingsScriptable<TId, TSettings> : ScriptableObjectDisposable where TId : ActorId<TId> where TSettings : ActorSettings
	{
        [SerializeField] private IdArray<TId, TSettings> _settings;

        public ReadOnlyArray<TSettings> Settings => _settings;

        public TSettings[] Init()
        {
            for (int i = 0; i < ActorId<TId>.Count; ++i)
                _settings[i].Init();

            return _settings.Values;
        }
    }
}
