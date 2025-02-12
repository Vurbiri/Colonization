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
        [SerializeField] private bool _isSelf;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _typeModifier;
        [SerializeField] private bool _isReflect;
        [SerializeField] private int _reflectValue;
        [SerializeField] private int _value;
        [SerializeField] private int _duration;
        [SerializeField] private int _descKeyId;
        [SerializeField] private bool _isKeyBase; // _useAttack

#if UNITY_EDITOR
        [SerializeField] private TargetOfSkill _parentTarget;
#endif

        public AEffect CreateEffect(EffectCode _code)
        {
            if (_duration > 0)
            {
                if (_isSelf)
                    return new SelfTemporaryEffect (_targetAbility, _typeModifier, _value, _duration, _code);
                if (_isReflect)
                    return new ReflectTemporaryEffect(_targetAbility, _typeModifier, _value, _reflectValue, _duration, _code);

                return new TargetTemporaryEffect(_targetAbility, _typeModifier, _value, _duration, _code);
            }

            if (!_useAttack)
            {
                if (_isSelf)
                    return new SelfEffect(_targetAbility, _typeModifier, _value);
                if (_isReflect)
                    return new ReflectEffect(_targetAbility, _typeModifier, _value);

                return new TargetEffect(_targetAbility, _typeModifier, _value);
            }

            if (!_useDefense)
            {
                if (_value < 0)
                    return _isReflect ? new ReflectAttackNotDefEffect(_value, _reflectValue) : new AttackNotDefEffect(_value);
                
                return _isSelf ? new SelfHealEffect(_value) : new TargetHealEffect(_value);
            }

            return _isReflect ? new ReflectAttackEffect(_value, _reflectValue) : new AttackEffect(_value);
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
            if (_isKeyBase)
                return (_value, hintTextColor.HexColorHintBase);

            if (!_isSelf & _parentTarget == TargetOfSkill.Enemy)
                return (-_value, hintTextColor.HexColorNegative);

            return (_value, hintTextColor.HexColorPositive);
        }
    }
}
