//Assets\Colonization\Scripts\Characteristics\Effects\EffectsHit\EffectHitSettings.cs
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class EffectHitSettings
    {
        [SerializeField] private bool _useAttack;
        [SerializeField] private bool _useDefense;
        [SerializeField] private TargetOfEffect _targetActor;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _typeModifier;
        [SerializeField] private bool _isReflect;
        [SerializeField] private int _reflectValue;
        [SerializeField] private int _value;
        [SerializeField] private int _duration;
        [SerializeField] private int _descKeyId;
        [SerializeField] private bool _isDescKeyBase;
        [SerializeField] private TargetOfSkill _parentTarget;

        public AEffect CreateEffect(EffectCode _code)
        {
            bool isSelf = _targetActor == TargetOfEffect.Self;
            //bool isNegative = !isSelf & _parentTarget == TargetOfSkill.Enemy;
            bool isNegative = _value < 0;

            if (_duration > 0)
            {
                if (isSelf)
                    return new TemporarySelfEffect (_targetAbility, isNegative, _typeModifier, _value, _duration, _code);
                if (_isReflect)
                    return new TemporaryReflectEffect(_targetAbility, isNegative, _typeModifier, _value, _duration, _code);

                return new TemporaryTargetEffect(_targetAbility, isNegative, _typeModifier, _value, _duration, _code);
            }

            if (!_useAttack)
            {
                if (isSelf)
                    return new PermanentSelfEffect(_targetAbility, isNegative, _typeModifier, _value);
                if (_isReflect)
                    return new PermanentReflectEffect(_targetAbility, isNegative, _typeModifier, _value);

                return new PermanentTargetEffect(_targetAbility, isNegative, _typeModifier, _value);
            }

            if (!_useDefense)
            {
                if (isNegative)
                    return _isReflect ? new AttackNotDefReflectEffect(_value) : new AttackNotDefEffect(_value);
                
                return isSelf? new SelfHealEffect(_value) : new TargetHealEffect(_value);
            }

            return _isReflect ? new AttackReflectEffect(_value) : new AttackEffect(_value);
        }

        public AEffectsUI CreateEffectUI(SettingsTextColor hintTextColor)
        {
            string deskKey = CONST_UI_LNG_KEYS.DESK_EFFECTS_KEYS[_descKeyId];
            (int value, string hexColor) = GetSettingsEffectUI(hintTextColor);

            if (_typeModifier == TypeModifierId.Addition & _targetAbility <= ActorAbilityId.MAX_RATE_ABILITY)
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
