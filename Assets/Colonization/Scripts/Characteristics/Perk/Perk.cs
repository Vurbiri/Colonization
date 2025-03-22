//Assets\Colonization\Scripts\Characteristics\Perk\Perk.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class Perk : IPerk
    {
        [SerializeField] private int _id;
        [SerializeField] private int _type;
        [SerializeField] private int _level;
        [SerializeField] private int _targetObject;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _value;
        [SerializeField] private int _typeModifier;
        [SerializeField] private int _cost = 1;

        [SerializeField] private int _position;
        [SerializeField] private string _keyDescription;
        [SerializeField] private Sprite _sprite;

        public int Id => _id;
        public int Type => _type;
        public int Level => _level;
        public Id<TargetOfPerkId> TargetObject => _targetObject;
        public int TargetAbility => _targetAbility;
        public int Value => _value;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Cost => _cost;

        public int Position => _position;
        public string KeyDescription => _keyDescription;
        public Sprite Sprite => _sprite;
    }
}
