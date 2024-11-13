using System;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Ability<TId> : AReactive<int>, IValueId<TId> where TId : AAbilityId<TId>
    {
        private readonly Id<TId> _id;
        private int _baseValue;
        private int _currentValue;
        private readonly IdArray<TypeOperationId, IAbilityModifier> _perks = new();
        private Func<int, int> funcClamp;

        public Id<TId> Id => _id;
        public override int Value { get => _currentValue; protected set {} } 
        public int BaseValue
        {
            get => _baseValue;
            set
            {
                if (_baseValue != value)
                {
                    _baseValue = value;
                    NextValue();
                }
            }
        }
        public bool IsBaseValue
        {
            get => _baseValue > 0;
            set
            {
                if ((_baseValue > 0) != value)
                {
                    _baseValue = value ? 1 : 0;
                    NextValue();
                }
            }
        }
        public Func<int, int> Clamp { set => funcClamp = value; }

        public Ability(Id<TId> id, int baseValue)
        {
            _perks[TypeOperationId.Addition] = new AbilityModAdd();
            _perks[TypeOperationId.RandomAdd] = new AbilityModRandom();
            _perks[TypeOperationId.Percent] = new AbilityModPercent();

            _id = id;
            _baseValue = _currentValue = baseValue;
        }

        public void Add(IAbilityModifierSettings settings)
        {
            _perks[settings.TypeOperation].Add(settings);
            NextValue();
        }
        public void Remove(IAbilityModifierSettings settings)
        {
            _perks[settings.TypeOperation].Remove(settings);
            NextValue();
        }

        public int NextValue()
        {
            int old = _currentValue;
            _currentValue = _baseValue;
            for (int i = 0; i < TypeOperationId.Count; i++)
                _currentValue = _perks[i].Apply(_currentValue);

            if(funcClamp != null)
                _currentValue = funcClamp(_currentValue);

            if (old != _currentValue & actionValueChange != null)
                actionValueChange(_currentValue);

            return _currentValue;
        }
    }
}
