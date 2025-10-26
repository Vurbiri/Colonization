using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    sealed public class Perk : IPerk
    {
        [SerializeField] private int _type;
        [SerializeField] private int _id;
        [SerializeField] private int _level;
        [SerializeField] private int _targetObject;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _value;
        [SerializeField] private int _typeModifier;
        [SerializeField] private int _cost;

        public int Type { [Impl(256)] get => _type; }
        public int Id { [Impl(256)] get => _id; }
        public int Level { [Impl(256)] get => _level; }
        public int Points { [Impl(256)] get => _level * (_level + 1); }
        public Id<TargetOfPerkId> TargetObject { [Impl(256)] get => _targetObject; }
        public int TargetAbility { [Impl(256)] get => _targetAbility; }
        public int Value { [Impl(256)] get => _value; }
        public Id<TypeModifierId> TypeModifier { [Impl(256)] get => _typeModifier; }
        public int Cost { [Impl(256)] get => _cost; }

#if UNITY_EDITOR
        [Impl(256)] public override string ToString() => $"[{AbilityTypeId.Names_Ed[_type]}.{(_type == 0 ? EconomicPerksId.Names_Ed : MilitaryPerksId.Names_Ed)[_id]}]";
#else
        [Impl(256)] public override string ToString() => $"[{_type}.{_id}]";
#endif

#if UNITY_EDITOR

        public int perkModifier;
        public int position;
        public string keyDescription;
        public Sprite sprite;

        public Perk Clone_Ed()
        {
            Perk perk = new()
            {
                _type = _type,
                _id = _id,
                _level = _level,
                _targetObject = _targetObject,
                _targetAbility = _targetAbility,
                _value = _value,
                _typeModifier = _typeModifier,
                _cost = _cost,

                perkModifier = perkModifier,
                position = position,
                keyDescription = keyDescription,
                sprite = sprite
            };

            return perk;
        }
#endif
    }
}
