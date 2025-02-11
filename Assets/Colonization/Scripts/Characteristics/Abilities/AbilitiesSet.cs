//Assets\Colonization\Scripts\Characteristics\Abilities\AbilitiesSet.cs
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class AbilitiesSet<TId> : IReadOnlyAbilities<TId> where TId : AbilityId<TId>
    {
        private readonly IdArray<TId, AAbility<TId>> _abilities = new();

        public IReadOnlyReactive<int> this[int index] => _abilities[index];
        public IReadOnlyReactive<int> this[Id<TId> id] => _abilities[id];

        public int Count => AbilityId<TId>.Count;

        public AbilitiesSet(IdArray<TId, int> states)
        {
            for (int i = 0; i < AbilityId<TId>.Count; i++)
                _abilities[i] = new Ability<TId>(i, states[i]);
        }

        public AbilitiesSet(IdArray<TId, int> states, int rate, int maxIndexRate)
        {
            int i;
            for (i = 0; i <= maxIndexRate; i++)
                _abilities[i] = new Ability<TId>(i, states[i] * rate);
            for (; i < AbilityId<TId>.Count; i++)
                _abilities[i] = new Ability<TId>(i, states[i]);
        }

        public BooleanAbility<TId> ReplaceToBoolean(Id<TId> id)
        {
            BooleanAbility<TId> ability = new(_abilities[id]);
            _abilities[id] = ability;
            return ability;
        }

        public SubAbility<TId> ReplaceToSub(Id<TId> id, Id<TId> max, Id<TId> restore)
        {
            SubAbility<TId> ability = new(_abilities[id], _abilities[max], _abilities[restore]);
            _abilities[id] = ability;
            return ability;
        }

        public bool IsGreater(Id<TId> stateId, int value) => _abilities[stateId].Value > value;
        public bool IsLess(Id<TId> stateId, int value) => _abilities[stateId].Value < value;

        public bool IsTrue(Id<TId> stateId) => _abilities[stateId].IsValue;

        public int AddPerk(IPerk perk) => _abilities[perk.TargetAbility].AddModifier(perk);
        public int RemovePerk(IPerk perk) => _abilities[perk.TargetAbility].RemoveModifier(perk);

        public IEnumerator<IReadOnlyReactive<int>> GetEnumerator()
        {
            for (int i = 0; i < AbilityId<TId>.Count; i++)
                yield return _abilities[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
