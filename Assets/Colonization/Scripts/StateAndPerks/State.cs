using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class State<TId> : AReactive<int>, IValueId<TId> where TId : AStateId<TId>
    {
        private readonly Id<TId> _id;
        private readonly int _baseValue;

        private int _currentValue;
        private readonly HashSet<IPerk<TId>> _permanentPerks;
        private readonly HashSet<IPerk<TId>> _randomPerks;

        public Id<TId> Id => _id;
        public override int Value { get => _currentValue; protected set { } }
        public int NextValue
        {
            get
            {
                if (_randomPerks.Count == 0)
                    return _currentValue;

                int newValue = _currentValue;
                foreach (IPerk<TId> perk in _randomPerks)
                    newValue = perk.Apply(newValue);

                return newValue;
            }
        }
        
        public State(Id<TId> id, int baseValue)
        {
            _id = id;
            _baseValue = _currentValue = baseValue;
            _permanentPerks = new();
            _randomPerks = new();
        }

        public State(State<TId> ability)
        {
            _id = ability._id;
            _baseValue = _currentValue = ability._baseValue;
            _permanentPerks = new();
            _randomPerks = new();
        }

        public bool TryAddPerk(IPerk<TId> perk)
        {
            if (perk.IsPermanent)
            {
                if (_permanentPerks.Add(perk))
                {
                    _currentValue = perk.Apply(_currentValue);
                    ActionValueChange?.Invoke(_currentValue);
                    return true;
                }

                return false;
            }

            return _randomPerks.Add(perk);
        }

    }
}
