using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public class AbilitySet<TId> : IEnumerable<Ability<TId>> where TId : AAbilityId<TId>
    {
        private readonly Ability<TId>[] _abilities;
        private readonly int _count;

        public Ability<TId> this[Id<TId> id] => _abilities[id.ToInt];

        public AbilitySet(IdArray<TId, int> abilities)
        {
            _count = AAbilityId<TId>.Count;
            _abilities = new Ability<TId>[_count];

            for (int i = 0; i < _count; i++)
                _abilities[i] = new Ability<TId>(i, abilities[i]);
        }

        public bool IsMore(Id<TId> abilityId, int value = 0) => _abilities[abilityId.ToInt].NextValue > value;

        public int GetValue(Id<TId> abilityId) => _abilities[abilityId.ToInt].NextValue;

        public bool TryAddPerk(IPerk<TId> perk) => _abilities[perk.TargetAbility.ToInt].TryAddPerk(perk);

        public IEnumerator<Ability<TId>> GetEnumerator()
        {
            foreach(var ability in _abilities)
                yield return ability;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
