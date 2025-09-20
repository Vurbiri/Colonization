using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class Perk : IPerk
    {
        [SerializeField] private int _type;
        [SerializeField] private int _id;
        [SerializeField] private int _level = PerkTree.MIN_LEVEL;
        [SerializeField] private int _targetObject;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _value;
        [SerializeField] private int _typeModifier;
        [SerializeField] private int _cost = PerkTree.MIN_LEVEL + 1;

        public int Type => _type;
        public int Id => _id;
        public int Level => _level;
        public int Points => _level * (_level + 1);
        public Id<TargetOfPerkId> TargetObject => _targetObject;
        public int TargetAbility => _targetAbility;
        public int Value => _value;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Cost => _cost;

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
