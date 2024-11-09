using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Ability<TId> : AReactive<int>, IValueId<TId> where TId : AAbilityId<TId>
    {
        private readonly Id<TId> _id;
        private readonly int _baseValue;

        private int _currentValue;
        private readonly IdArray<TypeOperationId, IAbilityModifier> _perks = new();

        public Id<TId> Id => _id;
        public override int Value { get => _currentValue; protected set { } }

        private Ability()
        {
            _perks[TypeOperationId.Addition] = new AbilityModAdd();
            _perks[TypeOperationId.Percent] = new AbilityModPercent();
            _perks[TypeOperationId.RandomAdd] = new AbilityModRandom();
        }

        public Ability(Id<TId> id, int baseValue) : this()
        {
            _id = id;
            _baseValue = _currentValue = baseValue;
        }

        public Ability(Ability<TId> state) : this()
        {
            _id = state._id;
            _baseValue = _currentValue = state._baseValue;
        }

        public void AddPerk(IAbilityModifierSettings settings)
        {
            _perks[settings.TypeOperation].Add(settings);
            NextValue();
        }
        public void RemovePerk(IAbilityModifierSettings settings)
        {
            _perks[settings.TypeOperation].Remove(settings);
            NextValue();
        }

        public int NextValue()
        {
            _currentValue = _baseValue;
            for (int i = 0; i < TypeOperationId.Count; i++)
                _currentValue = _perks[i].Apply(_currentValue);

            actionValueChange?.Invoke(_currentValue);

            return _currentValue;
        }
    }
}
