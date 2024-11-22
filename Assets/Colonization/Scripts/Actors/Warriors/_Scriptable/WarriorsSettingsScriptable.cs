//Assets\Colonization\Scripts\Actors\Warriors\_Scriptable\WarriorsSettingsScriptable.cs
using System;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "WarriorsSettings", menuName = "Vurbiri/Colonization/Characteristics/WarriorsSettings", order = 51)]
    public class WarriorsSettingsScriptable : ScriptableObject , IDisposable
    {
        [SerializeField] private IdArray<WarriorId, WarriorSettings> _settings;

        public WarriorSettings this[Id<WarriorId> id] => _settings[id];
        public WarriorSettings this[int index] => _settings[index];

        public void Dispose()
        {
            for(int i = 0; i < WarriorId.Count; i++)
                _settings[i].Dispose();

            Resources.UnloadAsset(this);
        }
    }
}
