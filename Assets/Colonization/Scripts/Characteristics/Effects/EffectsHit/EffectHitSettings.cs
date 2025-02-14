//Assets\Colonization\Scripts\Characteristics\Effects\EffectsHit\EffectHitSettings.cs
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Characteristics
{
    using static CONST_UI_LNG_KEYS;

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

#if UNITY_EDITOR
        [SerializeField] private TargetOfSkill _parentTarget;
#endif

        public bool IsSelf => _isSelf;

        public AEffect CreateEffect(EffectCode _code)
        {
            if (_duration > 0)
                return _isSelf ? new SelfTemporaryEffect(_targetAbility, _typeModifier, _value, _duration, _code) :
                                 new TargetTemporaryEffect(_targetAbility, _typeModifier, _value, _duration, _code);

            if (!_useAttack)
                return _isSelf ? new SelfEffect(_targetAbility, _typeModifier, _value) : new TargetEffect(_targetAbility, _typeModifier, _value);

            if (_useDefense)
                return _isReflect ? new ReflectAttackEffect(_value, _reflectValue) : new AttackEffect(_value);

            if (_value < 0)
                return _isReflect ? new ReflectAttackNotDefEffect(_value, _reflectValue) : new AttackNotDefEffect(_value);

            if (_isSelf)
                return new SelfHealEffect(_value);
            if (_isReflect)
                return new ReflectHealEffect(_value, _reflectValue);

            return new TargetHealEffect(_value);
        }

       
        public AEffectsUI CreateEffectUI(SettingsTextColor hintTextColor)
        {
            string deskKey = DESK_EFFECTS_KEYS[_descKeyId];
            
            bool isPositive = _value > 0;
            string hexColor, value;

            if (_useAttack)
            {
                hexColor = hintTextColor.HexColorTextBase;
                value = isPositive ? _value.ToString() : (-_value).ToString();

                if (!_isReflect)
                    return new PermEffectUI(deskKey, value, hexColor);

                string descKeyReflect, hexColorReflect;

                if (isPositive)
                {
                    descKeyReflect = REFLECT_MINUS;
                    hexColorReflect = hintTextColor.HexColorNegative;
                }
                else
                {
                    descKeyReflect = REFLECT_PLUS;
                    hexColorReflect = hintTextColor.HexColorPositive;
                }

                return new ReflectEffectUI(deskKey, value, hexColor, descKeyReflect, -_reflectValue, hexColorReflect);
            }

            hexColor = isPositive ? hintTextColor.HexColorPositive : hintTextColor.HexColorNegative;
            value = ValueToString(isPositive);

            if (_duration > 0)
                return new TempEffectUI(deskKey, value, _duration, hexColor);

            return new PermEffectUI(deskKey, value, hexColor);

            #region Local: ValueToString(..)
            //==============================================
            string ValueToString(bool isPositive)
            {
                if (_targetAbility == ActorAbilityId.IsMove)
                    return isPositive ? PLUS : MINUS;

                int value = _value;
                bool isPresent = !(_typeModifier == TypeModifierId.Addition);
                string present = isPresent ? PRESENT : string.Empty;

                if (!isPresent & _targetAbility <= ActorAbilityId.MAX_RATE_ABILITY)
                    value /= ActorAbilityId.RATE_ABILITY;

                return isPositive ? $"{PLUS}{value}{present}" : $"{value}{present}";
            }
            #endregion
        }
    }
}
