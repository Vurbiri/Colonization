//Assets\Colonization\Scripts\Characteristics\Effects\EffectSettings.cs
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
        [SerializeField] private TargetOfEffect _targetActor;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _typeModifier;
        [SerializeField] private bool _isReflect;
        [SerializeField] private int _value;
        [SerializeField] private int _duration;
        [SerializeField] private int _descKeyId;
        [SerializeField] private bool _isDescKeyBase;
        [SerializeField] private TargetOfSkill _parentTarget;

        public AEffect CreateEffect(EffectCode _code)
        {
            bool isSelf = _targetActor == TargetOfEffect.Self;
            bool isNegative = !isSelf & _parentTarget == TargetOfSkill.Enemy;

            if (_duration > 0)
            {
                if (isSelf)
                    return new TemporarySelfEffect (_targetAbility, isNegative, _typeModifier, _value, _duration, _code);
                if (_isReflect)
                    return new TemporaryReflectEffect(_targetAbility, isNegative, _typeModifier, _value, _duration, _code);

                return new TemporaryTargetEffect(_targetAbility, isNegative, _typeModifier, _value, _duration, _code);
            }

            if (_usedAbility < 0)
            {
                if (isSelf)
                    return new PermanentSelfEffect(_targetAbility, isNegative, _typeModifier, _value);
                if (_isReflect)
                    return new PermanentReflectEffect(_targetAbility, isNegative, _typeModifier, _value);

                return new PermanentTargetEffect(_targetAbility, isNegative, _typeModifier, _value);
            }

            if (_counteredAbility < 0)
            {
                if (isSelf)
                    return new PermanentUsedNotCounterSelfEffect(_targetAbility, isNegative, _usedAbility, _typeModifier, _value);
                if (_isReflect)
                    return new PermanentUsedNotCounterReflectEffect(_targetAbility, isNegative, _usedAbility, _typeModifier, _value);

                return new PermanentUsedNotCounterTargetEffect(_targetAbility, isNegative, _usedAbility, _typeModifier, _value);
            }

            if (isSelf)
                return new PermanentUsedSelfEffect(_targetAbility, isNegative, _usedAbility, _counteredAbility, _typeModifier, _value);
            if (_isReflect)
                return new PermanentUsedReflectEffect(_targetAbility, isNegative, _usedAbility, _counteredAbility, _typeModifier, _value);

            return new PermanentUsedTargetEffect(_targetAbility, isNegative, _usedAbility, _counteredAbility, _typeModifier, _value);
        }

        public AEffectsUI CreateEffectUI(SettingsTextColor hintTextColor)
        {
            string deskKey = CONST_UI_LNG_KEYS.DESK_EFFECTS_KEYS[_descKeyId];
            (int value, string hexColor) = GetSettingsEffectUI(hintTextColor);

            if (_typeModifier != TypeModifierId.Percent & _targetAbility <= ActorAbilityId.MAX_RATE_ABILITY)
                value /= ActorAbilityId.RATE_ABILITY;


            if (_duration > 0)
                return new TempEffectUI(deskKey, value, _duration, hexColor);

            return new PermEffectUI(deskKey, value, hexColor);
        }

        private (int, string) GetSettingsEffectUI(SettingsTextColor hintTextColor)
        {
            if (_isDescKeyBase)
                return (_value, hintTextColor.HexColorHintBase);

            if (_targetActor == TargetOfEffect.Target & _parentTarget == TargetOfSkill.Enemy)
                return (-_value, hintTextColor.HexColorNegative);

            return (_value, hintTextColor.HexColorPositive);
        }
    }
}
