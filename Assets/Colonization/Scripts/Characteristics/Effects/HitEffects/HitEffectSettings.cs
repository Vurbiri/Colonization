using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Characteristics
{
    using static CONST_UI_LNG_KEYS;

    [System.Serializable]
    public class HitEffectSettings
    {
        [SerializeField] private bool _isSelf;
        [SerializeField] private bool _useAttack = true;
        [SerializeField] private int _duration;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _typeModifier;
        [SerializeField] private int _value;
        [SerializeField] private int _pierce;
        [SerializeField] private int _reflectValue;
        [SerializeField] private int _descKeyId;

        public bool IsSelf => _isSelf;

        public AHitEffect CreateEffect(EffectCode code)
        {
            if (_duration > 0)
                return _isSelf ? new SelfTemporaryEffect(code, _targetAbility, _typeModifier, _value, _duration) :
                                 new TargetTemporaryEffect(code, _targetAbility, _typeModifier, _value, _duration);

            if (!_useAttack)
                return _isSelf ? new SelfEffect(_targetAbility, _typeModifier, _value) : new TargetEffect(_targetAbility, _typeModifier, _value);

            bool isReflect = _reflectValue > 0;

            if (_value < 0)
                    return isReflect ? new ReflectAttackEffect(_value, _pierce, _reflectValue) : new AttackEffect(_value, _pierce);

            if (_isSelf)
                return new SelfHealEffect(_value);
            if (isReflect)
                return new ReflectHealEffect(_value, _reflectValue);

            return new TargetHealEffect(_value);
        }
               
        public AEffectsUI CreateEffectUI(ProjectColors colors)
        {
            string deskKey = DESK_EFFECTS_KEYS[_descKeyId];
            
            bool isPositive = _value > 0;
            string hexColor, value;

            if (_useAttack)
            {
                bool isNotPiercing = _pierce == 0;

                hexColor = colors.TextDefaultTag;
                value = _value.ToString("#;#;0");

                if (_reflectValue <= 0)
                {
                    if (isNotPiercing) return new PermEffectUI(deskKey, value, hexColor);
                    return new PenetrationEffectUI(deskKey, value, _pierce, hexColor);
                }

                string descKeyReflect, hexColorReflect;

                if (isPositive)
                {
                    descKeyReflect = REFLECT_MINUS;
                    hexColorReflect = colors.TextNegativeTag;
                }
                else
                {
                    descKeyReflect = REFLECT_PLUS;
                    hexColorReflect = colors.TextPositiveTag;
                }

                if (isNotPiercing) return new ReflectEffectUI(deskKey, value, hexColor, descKeyReflect, _reflectValue, hexColorReflect);
                return new ReflectPenetrationEffectUI(deskKey, value, _pierce, hexColor, descKeyReflect, _reflectValue, hexColorReflect);
            }

            hexColor = isPositive ? colors.TextPositiveTag : colors.TextNegativeTag;
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

                if (!isPresent & _targetAbility <= ActorAbilityId.MAX_ID_SHIFT_ABILITY)
                    value >>= ActorAbilityId.SHIFT_ABILITY;

                return isPositive ? $"{PLUS}{value}{present}" : $"{value}{present}";
            }
            #endregion

        }

#if UNITY_EDITOR
        [SerializeField] private TargetOfSkill _parentTarget_ed;
#endif
    }
}
