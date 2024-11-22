//Assets\Colonization\Scripts\Characteristics\Perk\PerkSettings.cs
namespace Vurbiri.Colonization.Characteristics
{
    using UI;
    using UnityEngine;

    [System.Serializable]
    public class PerkSettings : IPerkSettings, IPerkSettingsUI
    {
        [SerializeField] private int _id;
        [SerializeField] private int _type;
        [SerializeField] private int _level = 1;
        [SerializeField] private int _position = 1;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _keyDescription;
        [SerializeField] private int _targetObject;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _typeModifier;
        [SerializeField] private int _value;
        [SerializeField] private Chance _chance = 100;
        [SerializeField] private CurrenciesLite _cost;
        [SerializeField] private int _prevPerk = -1;

        public int Id => _id;
        public int Type => _type;
        public int Level => _level;
        public int Position => _position;
        public Sprite Sprite => _sprite;
        public string KeyDescription => _keyDescription;
        public Id<TargetOfPerkId> TargetObject => _targetObject;
        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;
        public Chance Chance => _chance;
        public CurrenciesLite Cost => _cost;
        public int PrevPerk => _prevPerk;
    }
}
