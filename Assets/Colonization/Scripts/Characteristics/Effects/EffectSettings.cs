namespace Vurbiri.Colonization.Characteristics
{
    using UnityEngine;

    [System.Serializable]
    public class EffectSettings
    {
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _usedAbility;
        [SerializeField] private int _counteredAbility;
        [SerializeField] private int _targetActor;
        [SerializeField] private int _typeModifier;
        [SerializeField] private int _value;
        [SerializeField] private bool _isNegative;
        [SerializeField] private int _duration;
        [SerializeField] private int _keyDescId;

        public Id<TargetOfEffectId> TargetActor => _targetActor;
        public int TargetAbility => _targetAbility;
        public int Value => _value;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Duration => _duration;
        public bool IsNegative => _isNegative;
        public int KeyDescId => _keyDescId;

        public AEffect CreateEffect()
        {
            bool isSelf = _targetActor == TargetOfEffectId.Self;

            if (_duration > 0)
            {
                if (isSelf)
                    return new TemporarySelfEffect (_targetAbility, _isNegative, _typeModifier, _value, _duration);

                return new TemporaryTargetEffect(_targetAbility, _isNegative, _typeModifier, _value, _duration);
            }

            if (_usedAbility < 0)
            {
                if (isSelf)
                    return new PermanentSelfEffect(_targetAbility, _isNegative, _typeModifier, _value);

                return new PermanentTargetEffect(_targetAbility, _isNegative, _typeModifier, _value);
            }

            if (isSelf)
                return new PermanentUsedSelfEffect(_targetAbility, _isNegative, _usedAbility, _counteredAbility, _typeModifier, _value);

            return new PermanentUsedTargetEffect(_targetAbility, _isNegative, _usedAbility, _counteredAbility, _typeModifier, _value);
        }
    }
}
