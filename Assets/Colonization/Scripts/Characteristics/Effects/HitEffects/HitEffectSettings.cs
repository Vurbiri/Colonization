using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    using static CONST_UI;

    [System.Serializable]
    public class HitEffectSettings
    {
        [SerializeField] private bool _isSelf;
        [SerializeField] private bool _useAttack;
        [SerializeField] private int _duration;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _typeModifier;
        [SerializeField] private int _value;
        [SerializeField] private int _pierce;
        [SerializeField] private int _holy;
        [SerializeField] private int _reflect;
        [SerializeField] private string _descKey;

        public bool IsSelf => _isSelf;

        public AHitEffect CreateEffect(EffectCode code)
        {
            AHitEffect output;

            if (_useAttack)
            {
                bool isReflect = _reflect > 0;

                if (_value < 0)
                {
                    if (_holy > 0)
                        output = isReflect ? new ReflectHolyAttackEffect(_value, _holy, _pierce, _reflect) : new HolyAttackEffect(_value, _holy, _pierce);
                    else
                        output = isReflect ? new ReflectAttackEffect(_value, _pierce, _reflect) : new AttackEffect(_value, _pierce);
                }
                else
                {
                    if (_isSelf)
                        output = new SelfHealEffect(_value);
                    else
                        output = isReflect ? new ReflectHealEffect(_value, _reflect) : new TargetHealEffect(_value);
                }
            }
            else if (_duration > 0)
            {
                output = _isSelf ? new AddSelfEffect(code, _targetAbility, _typeModifier, _value, _duration): new AddTargetEffect(code, _targetAbility, _typeModifier, _value, _duration);
            }
            else if (_targetAbility == ClearEffectsId.Code)
            {
                output = _isSelf ? new SelfClearEffect(_typeModifier, _value) : new TargetClearEffect(_typeModifier, _value);
            }
            else
            {
                output = _isSelf ? new ApplySelfEffect(_targetAbility, _typeModifier, _value) : new ApplyTargetEffect(_targetAbility, _typeModifier, _value);
            }

            return output;
        }
               
        public AEffectUI CreateEffectUI(ProjectColors colors)
        {
            AEffectUI output;
            string hexColor, value;

            if (_useAttack)
            {
                AEffectUI reflect;

                if (_reflect <= 0)
                    reflect = EffectUI.Empty;
                else
                    reflect = _value > 0 ? new ValueEffectUI(REFLECT_MINUS, _reflect, colors.TextNegativeTag) : new ValueEffectUI(REFLECT_PLUS, _reflect, colors.TextPositiveTag);

                hexColor = colors.TextDefaultTag;
                value = _value.ToString("#;#;0");

                if (_holy > 0)
                    output = _pierce > 0 ? new ValuesEffectUI(_descKey, value, _holy, _pierce, hexColor, reflect) : new ValuesEffectUI(_descKey, value, _holy, hexColor, reflect);
                else
                    output = _pierce > 0 ? new ValuesEffectUI(_descKey, value, _pierce, hexColor, reflect) : new ValueEffectUI(_descKey, value, hexColor, reflect);
            }
            else if (_targetAbility == ClearEffectsId.Code)
            {
                output = new ValueEffectUI(_descKey, _value, ClearEffectsId.ColorSelection(colors, _typeModifier));
            }
            else
            {
                hexColor = _value > 0 ? colors.TextPositiveTag : colors.TextNegativeTag;

                if (_targetAbility == ActorAbilityId.IsMove)
                {
                    output = new EffectUI(_descKey, hexColor);
                }
                else
                {
                    value = ValueToString(_value, _targetAbility, _typeModifier);

                    if (_duration > 0)
                        output = new ValuesEffectUI(_descKey, value, _duration, hexColor);
                    else
                        output = new ValueEffectUI(_descKey, value, hexColor);
                }
            }

            return output;
        }

        public static string ValueToString(int value, int ability, int modifier)
        {
            string output;

            if (modifier == TypeModifierId.Addition)
            {
                if (ability <= ActorAbilityId.MAX_ID_SHIFT_ABILITY)
                    value >>= ActorAbilityId.SHIFT_ABILITY;

                output = value.ToString("+#;-#;0");
            }
            else
            {
                output = value.ToString("+#;-#;0").Concat("%");
            }

            return output;
        }

#if UNITY_EDITOR
#pragma warning disable 414
        [SerializeField] private TargetOfSkill _parentTarget_ed;
        [SerializeField] private bool _isWarrior_ed;
#pragma warning restore

        public bool UseAttack_Ed => _useAttack;
#endif
    }
}
