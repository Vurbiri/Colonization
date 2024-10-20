using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "PlayerStates", menuName = "Vurbiri/Colonization/StatesAndPerks/PlayerStates", order = 51)]
    public class PlayerStatesScriptable : ScriptableObject
    {
        [SerializeField] private IdArray<PlayerStateId, int> _states;

        public int Count => _states.Count;

        public StatesSet<PlayerStateId> GetAbilities() => new(_states);
    }
}
