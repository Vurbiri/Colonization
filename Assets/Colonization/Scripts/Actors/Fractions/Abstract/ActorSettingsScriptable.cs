using System;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
	public abstract class ActorSettingsScriptable<TId, TValue> : ScriptableObject, IDisposable where TId : ActorId<TId> where TValue : ActorSettings
	{
        [SerializeField] private IdArray<TId, TValue> _settings;

        public TValue this[Id<TId> id] => _settings[id];
        public TValue this[int index] => _settings[index];

        public void Dispose()
        {
            for (int i = 0; i < ActorId<TId>.Count; i++)
                _settings[i].Dispose();

            Resources.UnloadAsset(this);
        }

#if UNITY_EDITOR
        public ReadOnlyIdArray<TId, TValue> Settings => _settings;
#endif
    }
}
