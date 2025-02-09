//Assets\Colonization\Scripts\Characteristics\Abilities\AbilitiesSet.cs
namespace Vurbiri.Colonization.Characteristics
{
    using System.Collections;
    using System.Collections.Generic;
    using Vurbiri.Collections;
    using Vurbiri.Reactive;

    public class AbilitiesSet<TId> : IReadOnlyList<IReadOnlyReactive<int>> where TId : AAbilityId<TId>
    {
        private readonly IdArray<TId, Ability<TId>> _abilities = new();

        public IReadOnlyReactive<int> this[int index] => _abilities[index];
        public IReadOnlyReactive<int> this[Id<TId> id] => _abilities[id];

        public int Count => AAbilityId<TId>.Count;

        public AbilitiesSet(IdArray<TId, int> states)
        {
            for (int i = 0; i < AAbilityId<TId>.Count; i++)
                _abilities[i] = new Ability<TId>(i, states[i]);
        }

        public AbilitiesSet(IdArray<TId, int> states, int rate, int maxIndexRate)
        {
            int i;
            for (i = 0; i <= maxIndexRate; i++)
                _abilities[i] = new Ability<TId>(i, states[i] * rate);
            for (; i < AAbilityId<TId>.Count; i++)
                _abilities[i] = new Ability<TId>(i, states[i]);
        }

        public bool IsGreater(Id<TId> stateId, int value) => _abilities[stateId].Value > value;
        public bool IsLess(Id<TId> stateId, int value) => _abilities[stateId].Value < value;

        public bool IsTrue(Id<TId> stateId) => _abilities[stateId].IsValue;

        public Ability<TId> GetAbility(Id<TId> stateId) => _abilities[stateId];

        public int GetValue(Id<TId> stateId) => _abilities[stateId].Value;
        public int ApplyValue(Id<TId> stateId, Id<TypeModifierId> id, int value) => _abilities[stateId].ApplyModifier(id, value);

        public int AddPerk(IPerk perk) => _abilities[perk.TargetAbility].AddModifier(perk);
        public int RemovePerk(IPerk perk) => _abilities[perk.TargetAbility].RemoveModifier(perk);

        public IEnumerator<IReadOnlyReactive<int>> GetEnumerator()
        {
            for (int i = 0; i < AAbilityId<TId>.Count; i++)
                yield return _abilities[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
