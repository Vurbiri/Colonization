//Assets\Colonization\Scripts\Actors\Fractions\Demons\Scriptable\DemonsSettingsScriptable.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    //[CreateAssetMenu(fileName = "DemonsSettings", menuName = "Vurbiri/Colonization/Characteristics/DemonsSettings", order = 51)]
    public class DemonsSettingsScriptable : ScriptableObject
	{
        [SerializeField] private IdArray<DemonId, DemonSettings> _settings;

        public DemonSettings this[Id<DemonId> id] => _settings[id];
        public DemonSettings this[int index] => _settings[index];

        public void Dispose()
        {
            for (int i = 0; i < DemonId.Count; i++)
                _settings[i].Dispose();

            Resources.UnloadAsset(this);
        }

#if UNITY_EDITOR
        public IReadOnlyList<ActorSettings> Settings => _settings;
#endif
    }
}
