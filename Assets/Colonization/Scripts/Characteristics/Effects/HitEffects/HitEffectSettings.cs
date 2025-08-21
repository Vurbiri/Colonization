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
        [SerializeField] private int _holy;
        [SerializeField] private int _reflectValue;
        [SerializeField] private string _descKey;

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
            {
                if(_holy > 0)
                    return isReflect ? new ReflectHolyAttackEffect(_value, _holy, _pierce, _reflectValue) : new HolyAttackEffect(_value, _holy, _pierce);

                return isReflect ? new ReflectAttackEffect(_value, _pierce, _reflectValue) : new AttackEffect(_value, _pierce);
            }

            if (_isSelf)
                return new SelfHealEffect(_value);
            if (isReflect)
                return new ReflectHealEffect(_value, _reflectValue);

            return new TargetHealEffect(_value);
        }
               
        public AEffectsUI CreateEffectUI(ProjectColors colors)
        {
            AEffectsUI output;
            bool isPositive = _value > 0;
            string hexColor, value;

            if (_useAttack)
            {
                AEffectsUI reflect;

                if (_reflectValue <= 0)
                    reflect = new EmptyEffectUI();
                else
                    reflect = isPositive ? new MainEffectUI(REFLECT_MINUS, _reflectValue, colors.TextNegativeTag) : new MainEffectUI(REFLECT_PLUS, _reflectValue, colors.TextPositiveTag);

                hexColor = colors.TextDefaultTag;
                value = _value.ToString("#;#;0");

                if (_holy > 0)
                    output = _pierce > 0 ? new MainAddTwoEffectUI(_descKey, value, _holy, _pierce, hexColor, reflect) : new MainAddOneEffectUI(_descKey, value, _holy, hexColor, reflect);
                else
                    output = _pierce > 0 ? new MainAddOneEffectUI(_descKey, value, _pierce, hexColor, reflect)        : new MainEffectUI(_descKey, value, hexColor, reflect);
            }
            else
            {
                hexColor = isPositive ? colors.TextPositiveTag : colors.TextNegativeTag;
                value = ValueToString(isPositive);

                if (_duration > 0)
                    output = new MainAddOneEffectUI(_descKey, value, _duration, hexColor);
                else
                    output = new MainEffectUI(_descKey, value, hexColor);
            }

            return output;

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
        [SerializeField] private bool _isWarrior_ed;
#endif
    }
}
