using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "WarriorsSettings", menuName = "Vurbiri/Colonization/Characteristics/WarriorsSettings", order = 51)]
    public class WarriorsSettingsScriptable : ScriptableObject
    {
        [SerializeField] private IdArray<WarriorId, WarriorSettings> _settings;

        public WarriorSettings this[Id<WarriorId> id] => _settings[id];
        public WarriorSettings this[int index] => _settings[index];
    }
}
