//Assets\Colonization\Scripts\Characteristics\Abilities\AbilitiesSet.cs
using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class AbilitiesSet<TId> : IReadOnlyAbilities<TId>, IDisposable where TId : AbilityId<TId>
    {
        private readonly IdArray<TId, AAbility<TId>> _abilities = new();

        public AAbility<TId> this[int index] => _abilities[index];
        public AAbility<TId> this[Id<TId> id] => _abilities[id];

        public int Count => AbilityId<TId>.Count;

        public AbilitiesSet(IdArray<TId, int> states, IReactive<Perk> perks)
        {
            for (int i = 0; i < AbilityId<TId>.Count; i++)
                _abilities[i] = new Ability<TId>(i, states[i]);

            perks.Subscribe(OnPerks);
        }

        public AbilitiesSet(IdArray<TId, int> states, int rate, int maxIndexRate)
        {
            int i;
            for (i = 0; i <= maxIndexRate; i++)
                _abilities[i] = new Ability<TId>(i, states[i] * rate);
            for (; i < AbilityId<TId>.Count; i++)
                _abilities[i] = new Ability<TId>(i, states[i]);
        }

        public BooleanAbility<TId> ReplaceToBoolean(Id<TId> id) => ReplaceTo(id, new BooleanAbility<TId>(_abilities[id]));
        public ChanceAbility<TId> ReplaceToChance(Id<TId> id, IAbility ratioA, IAbility ratioB)
        {
            return ReplaceTo(id, new ChanceAbility<TId>(_abilities[id], ratioA, ratioB));
        }
        public SubAbility<TId> ReplaceToSub(Id<TId> id, Id<TId> max, Id<TId> restore)
        {
            return ReplaceTo(id, new SubAbility<TId>(_abilities[id], _abilities[max], _abilities[restore]));
        }

        public bool IsGreater(Id<TId> stateId, int value) => _abilities[stateId].Value > value;
        public bool IsLess(Id<TId> stateId, int value) => _abilities[stateId].Value < value;

        public bool IsTrue(Id<TId> stateId) => _abilities[stateId].IsValue;

        public int AddPerk(IPerk perk) => _abilities[perk.TargetAbility].AddModifier(perk);
        public int RemovePerk(IPerk perk) => _abilities[perk.TargetAbility].RemoveModifier(perk);

        public IEnumerator<AAbility<TId>> GetEnumerator()
        {
            for (int i = 0; i < AbilityId<TId>.Count; i++)
                yield return _abilities[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            for(int i = 0; i < AbilityId<TId>.Count; i++)
                _abilities[i].Dispose();
        }


        private T ReplaceTo<T>(Id<TId> id, T newAbility) where T : AAbility<TId>
        {
            _abilities[id] = newAbility;
            return newAbility;
        }

        private void OnPerks(Perk perk)
        {
            if (perk.TargetObject == TargetOfPerkId.Player)
                _abilities[perk.TargetAbility].AddModifier(perk);
        }
    }
}
