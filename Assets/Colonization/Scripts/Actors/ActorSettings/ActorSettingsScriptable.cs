using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public abstract partial class ActorSettingsScriptable<TId, TSettings> : ScriptableObjectDisposable where TId : ActorId<TId> where TSettings : ActorSettings
	{
        [SerializeField] private IdArray<TId, TSettings> _settings;

        public ReadOnlyArray<TSettings> Settings => _settings;

        public TSettings[] Init(out int force)
        {
            TSettings settings;
            int minForce = int.MaxValue, maxForce = int.MinValue;
            for (int i = 0; i < ActorId<TId>.Count; ++i)
            {
                settings = _settings[i];
                settings.Init();
                minForce = Mathf.Min(minForce, settings.Force);
                maxForce = Mathf.Max(maxForce, settings.Force);
            }
            force = minForce + maxForce;

            return _settings.Values;
        }
    }
}
