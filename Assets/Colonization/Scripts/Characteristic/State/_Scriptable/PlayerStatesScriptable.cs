using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "PlayerStates", menuName = "Vurbiri/Colonization/Characteristics/PlayerStates", order = 51)]
    public class PlayerStatesScriptable : ScriptableObject
    {
        [SerializeField] private IdArray<PlayerStateId, int> _states;

        public StatesSet<PlayerStateId> GetStates() => new(_states);
    }
}
