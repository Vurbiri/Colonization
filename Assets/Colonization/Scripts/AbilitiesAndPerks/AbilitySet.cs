using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public class AbilitySet<T> : IEnumerable<PlayerAbility> where T : AAbilityId<T>
    {
        private readonly PlayerAbility[] _abilities;
        private readonly int _count;

        public PlayerAbility this[Id<T> id] => _abilities[id.ToInt];

        public AbilitySet(IdArray<T, int> abilities)
        {
            _count = AAbilityId<T>.Count;
            _abilities = new PlayerAbility[_count];

            for (int i = 0; i < _count; i++)
                _abilities[i] = (new(i, abilities[i]));
        }

        public bool IsMore(Id<T> abilityId, int value = 0) => _abilities[abilityId.ToInt].NextValue > value;

        public int GetValue(Id<T> abilityId) => _abilities[abilityId.ToInt].NextValue;

        public bool TryAddPerk(IPerk<PlayerAbilityId> perk) => _abilities[perk.TargetAbility.ToInt].TryAddPerk(perk);

        public IEnumerator<PlayerAbility> GetEnumerator()
        {
            foreach(var ability in _abilities)
                yield return ability;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
