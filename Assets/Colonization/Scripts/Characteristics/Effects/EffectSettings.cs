using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class EffectSettings
    {
        [SerializeField] private int _usedAbility;
        [SerializeField] private int _counteredAbility;
        [SerializeField] private int _targetActor;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _typeModifier;
        [SerializeField] private bool _isReflect;
        [SerializeField] private int _value;
        [SerializeField] private int _duration;
        [SerializeField] private int _descKeyId;
        [SerializeField] private bool _isDescKeyBase;
        [SerializeField] private int _parentTarget;

        public Id<TargetOfEffectId> TargetActor => _targetActor;
        public int TargetAbility => _targetAbility;
        public int Value => _value;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Duration => _duration;
        public bool IsNegative => _parentTarget == TargetOfSkillId.Enemy;
        public int KeyDescId => _descKeyId;

        public AEffect CreateEffect()
        {
            bool isSelf = _targetActor == TargetOfEffectId.Self;
            bool isNegative = !isSelf & _parentTarget == TargetOfSkillId.Enemy;

            if (_duration > 0)
            {
                if (isSelf)
                    return new TemporarySelfEffect (_targetAbility, isNegative, _typeModifier, _value, _duration);
                if (_isReflect)
                    return new TemporaryReflectEffect(_targetAbility, isNegative, _typeModifier, _value, _duration);

                return new TemporaryTargetEffect(_targetAbility, isNegative, _typeModifier, _value, _duration);
            }

            if (_usedAbility < 0)
            {
                if (isSelf)
                    return new PermanentSelfEffect(_targetAbility, isNegative, _typeModifier, _value);
                if (_isReflect)
                    return new PermanentReflectEffect(_targetAbility, isNegative, _typeModifier, _value);

                return new PermanentTargetEffect(_targetAbility, isNegative, _typeModifier, _value);
            }

            if (isSelf)
                return new PermanentUsedSelfEffect(_targetAbility, isNegative, _usedAbility, _counteredAbility, _typeModifier, _value);
            if (_isReflect)
                return new PermanentUsedReflectEffect(_targetAbility, isNegative, _usedAbility, _counteredAbility, _typeModifier, _value);

            return new PermanentUsedTargetEffect(_targetAbility, isNegative, _usedAbility, _counteredAbility, _typeModifier, _value);
        }

        public AEffectsUI CreateEffectUI(HintTextColor hintTextColor)
        {
            int value;
            string hexColor;
            string deskKey = CONST_UI_LNG_KEYS.DESK_EFFECTS_KEYS[_descKeyId];

            if (_isDescKeyBase)
            {
                hexColor = hintTextColor.HexColorBase;
                value = _value;
            }
            else
            {
                if (_targetActor == TargetOfEffectId.Target & _parentTarget == TargetOfSkillId.Enemy)
                {
                    hexColor = hintTextColor.HexColorMinus;
                    value = -_value;
                }
                else
                {
                    hexColor = hintTextColor.HexColorPlus;
                    value = _value;
                }
            }

            if (_typeModifier != TypeModifierId.Percent & _targetAbility <= ActorAbilityId.MAX_RATE_ABILITY)
                value /= ActorAbilityId.RATE_ABILITY;


            if (_duration > 0)
                return new TempEffectUI(deskKey, value, _duration, hexColor);

            return new PermEffectUI(deskKey, value, hexColor);
        }
    }
}
