using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using static UnityEditor.EditorGUI;
using static Vurbiri.Colonization.UI.CONST_UI_LNG_KEYS;

namespace VurbiriEditor.Colonization.Characteristics
{
    using static ActorAbilityId;

    [CustomPropertyDrawer(typeof(HitEffectSettings))]
    public class HitEffectSettingsDrawer : PropertyDrawerUtility
    {
      
        #region Consts
        private const string NAME_POSITIVE = "Positive Effect {0}", NAME_NEGATIVE = "Negative Effect {0}", NAME_VOID ="Void Effect {0}";
        private const string P_IS_SELF = "_isSelf", P_TARGET_ABILITY = "_targetAbility", P_TYPE_OP = "_typeModifier", P_VALUE = "_value", P_DUR = "_duration";
        private const string P_USED_ATTACK = "_useAttack", P_HOLY = "_holy", P_PIERCE = "_pierce", P_REFLECT = "_reflectValue";
        private const string P_DESC_KEY = "_descKey";
        private const string P_PARENT_TARGET = "_parentTarget_ed", P_PARENT_TYPE = "_isWarrior_ed";
        #endregion

        #region Values
        private readonly string[] _namesAbilitiesDuration = { ActorAbilityId.Names_Ed[MaxHP], ActorAbilityId.Names_Ed[HPPerTurn], ActorAbilityId.Names_Ed[Attack],
                                                               ActorAbilityId.Names_Ed[Defense], ActorAbilityId.Names_Ed[Pierce], ActorAbilityId.Names_Ed[APPerTurn]};
        private readonly int[] _valuesAbilitiesDuration = { MaxHP, HPPerTurn, Attack, Defense, Pierce, APPerTurn };

        private readonly string[] _namesModifiersDuration = { "Flat", "Percent" };
        private readonly int[] _valuesModifiersDuration = { TypeModifierId.Addition, TypeModifierId.TotalPercent };

        private readonly string[] _namesAbilitiesInstant = { ActorAbilityId.Names_Ed[CurrentHP], ActorAbilityId.Names_Ed[CurrentAP], ActorAbilityId.Names_Ed[IsMove] };
        private readonly int[] _valuesAbilitiesInstant = { CurrentHP, CurrentAP, IsMove };

        private readonly string[] _namesModifiersCurrentHP = { "Percent of CurrentHP", "Flat", "Percent of MaxHP" };
        #endregion

        private readonly Color _positive = new(0.5f, 1f, 0.3f, 1f), _negative = new(1f, 0.5f, 0.3f, 1f), _void = new(0.1f, 0.1f, 0.1f, 1f);

        protected override void OnGUI()
        {
            int id = IdFromLabel();
            var (name, color) = GetSkin();

            _label.text = string.Format(name, id);
            BeginProperty();

            Color defaultColor = GUI.color; GUI.color = color;
            Foldout(); GUI.color = defaultColor;

            if (_mainProperty.isExpanded)
            {
                GetTargetSkill(out bool isTargetSkillSelf, out bool isTargetSkillEnemy);
                bool isNotDuration, isUsedAttack, isTarget = false;
                int targetAbility = CurrentHP;

                if (isTargetSkillSelf)
                    SetBool(P_IS_SELF, true);
                else
                    isTarget = !DrawBool(P_IS_SELF);

                bool isTargetEnemy = isTargetSkillEnemy & isTarget;

                Space();

                isNotDuration = DrawInt(P_DUR, 0, 3) <= 0;

                if (isNotDuration & id == 0 && (isUsedAttack = DrawBool(P_USED_ATTACK, isTargetEnemy ? "Is Attack" : "Is Heal")))
                {
                    Space();
                    indentLevel++;

                    SetInt(P_TARGET_ABILITY, CurrentHP);
                    SetInt(P_TYPE_OP, TypeModifierId.TotalPercent);

                    if (isTargetEnemy)
                        DrawForEnemy(GetProperty(P_PARENT_TYPE).boolValue);
                    else
                        DrawForFriend();

                    indentLevel--;
                }
                else
                {
                    isUsedAttack = false;
                    SetNotAttack();
                }

                if (!isUsedAttack)
                    targetAbility = DrawDirectEffect(!isNotDuration);

                Space(3f);

                DrawLine();
                SetAndDrawDesc(isUsedAttack, targetAbility);
                DrawLine();

            }
            EndProperty();

            #region Local
            //==============================================
            (string name, Color color) GetSkin()
            {
                SerializedProperty property = GetProperty(P_VALUE);

                if (property.intValue > 0)
                    return (NAME_POSITIVE, _positive);

                if (property.intValue < 0)
                    return (NAME_NEGATIVE, _negative);

                return (NAME_VOID, _void);
            }
            //==============================================
            void GetTargetSkill(out bool isTargetSkillSelf, out bool isTargetSkillEnemy)
            {
                var parentTarget = GetProperty(P_PARENT_TARGET).GetEnum<TargetOfSkill>();
                isTargetSkillSelf = parentTarget == TargetOfSkill.Self;
                isTargetSkillEnemy = parentTarget == TargetOfSkill.Enemy;
            }
            //==============================================
            void SetNotAttack()
            {
                SetBool(P_USED_ATTACK, false);
                SetInt(P_HOLY, 0);
                SetInt(P_PIERCE, 0);
                SetInt(P_REFLECT, 0);
            }
            //==============================================
            void DrawForEnemy(bool isWarrior)
            {
                DrawAttack(5, 300, 100);
                if(isWarrior)
                    DrawInt(P_HOLY, "Holy (%)", 0, 305);
                else
                    SetInt(P_HOLY, 0);
                DrawInt(P_PIERCE, "Pierce (%)", 0, 100);
                DrawInt(P_REFLECT, "Leech (%)", 0, 200);
            }
            //==============================================
            void DrawForFriend()
            {
                DrawInt(P_VALUE, "Heal (%)", 5, 300, 100);
                SetInt(P_HOLY, 0);
                SetInt(P_PIERCE, 0);
                DrawInt(P_REFLECT, "Loss (%)", 0, 200);
            }
            //==============================================
            int DrawDirectEffect(bool isDuration)
            {
                SerializedProperty targetAbility = GetProperty(P_TARGET_ABILITY);
                
                Space();
                if (isDuration)
                {
                    SetDefaultValue(targetAbility, _valuesAbilitiesDuration);
                    return DrawDurationValue(DrawIntPopup(targetAbility, _namesAbilitiesDuration, _valuesAbilitiesDuration));
                }
                else
                {
                    SetDefaultValue(targetAbility, _valuesAbilitiesInstant);
                    return DrawInstantValue(DrawIntPopup(targetAbility, _namesAbilitiesInstant, _valuesAbilitiesInstant));
                }
                
            }
            //==============================================
            int DrawDurationValue(int usedAbility)
            {
                Space();
                indentLevel++;

                if (usedAbility == APPerTurn)
                {
                    SetInt(P_TYPE_OP, TypeModifierId.Addition);
                    DrawFlatValue(-2, 2);
                }
                else
                {
                    if (DrawIntPopup(P_TYPE_OP, _namesModifiersDuration, _valuesModifiersDuration) == TypeModifierId.TotalPercent)
                        DrawPercentValue(-200, 200, 100);
                    else if (usedAbility == Pierce)
                        DrawFlatValue(-50, 50);
                    else
                        DrawShiftValue(-50, 50);
                }

                indentLevel--;

                return usedAbility;
            }
            //==============================================
            int DrawInstantValue(int usedAbility)
            {
                Space();
                indentLevel++;

                if (usedAbility == CurrentHP)
                {
                    if (DrawIntPopup(P_TYPE_OP, _namesModifiersCurrentHP, ActorAbilityId.Values_Ed) == TypeModifierId.Addition)
                        DrawShiftValue(-75, 75);
                    else
                        DrawPercentValue(-100, 100);
                }
                else
                {
                    SetInt(P_TYPE_OP, TypeModifierId.Addition);

                    if (usedAbility == CurrentAP)
                        DrawFlatValue(-5, 5);
                    else
                        DrawMoveValue();
                }

                indentLevel--;

                return usedAbility;
            }
            //==============================================
            void DrawAttack(int min, int max, int defaultValue)
            {
                SerializedProperty property = GetProperty(P_VALUE);
                int value = property.intValue * -1;

                if (value < min | value > max)
                    defaultValue = Mathf.Clamp(defaultValue, min, max);
                else
                    defaultValue = value;

                _position.y += _height;
                property.intValue = IntSlider(_position, "Attack (%)", defaultValue, min, max) * -1;
            }
            //==============================================
            void DrawPercentValue(int min, int max, int defaultValue = 0) => DrawInt(P_VALUE, "Value (%)", min, max, defaultValue);
            //==============================================
            void DrawFlatValue(int min, int max, int defaultValue = 0) => DrawInt(P_VALUE, "Value", min, max, defaultValue);
            //==============================================
            void DrawShiftValue(int min, int max)
            {
                SerializedProperty property = GetProperty(P_VALUE);
                int value = property.intValue >> SHIFT_ABILITY;  

                _position.y += _height;
                property.intValue = IntSlider(_position, "Value", Mathf.Clamp(value, min, max), min, max) << SHIFT_ABILITY;
            }
            //==============================================
            void DrawMoveValue()
            {
                SerializedProperty property = GetProperty(P_VALUE);

                _position.y += _height;
                property.intValue = Toggle(_position, "Is Move", property.intValue > 0) ? 1 : -1;
            }
            //==============================================
            void SetDefaultValue(SerializedProperty property, int[] values)
            {
                int value = property.intValue;
                for (int i = 0; i < values.Length; i++)
                    if (values[i] == value)
                        return;

                property.intValue = values[0];
            }
            //==============================================
            void SetAndDrawDesc(bool isUsedAttack, int targetAbility)
            {
                var localization = Localization.ForEditor(LangFiles.Actors);

                _position.x += 35;

                Color defaultColor = GUI.contentColor;
                int value = GetInt(P_VALUE);
                bool isPositive = value > 0;
                int duration = GetInt(P_DUR);
                int mod = GetInt(P_TYPE_OP);

                string key;
                string strValue = ValueToString(value, targetAbility, mod, isUsedAttack);

                if (isUsedAttack)
                {
                    if(isPositive)
                    {  
                        key = "Healing"; 
                        GUI.contentColor = _positive;
                        DrawLabel(localization.GetFormatText(FILE, key, strValue).Delete("<b>", "</b>"));
                    }
                    else
                    {
                        key = DrawAttackDesc(localization, strValue);
                    }
                }
                else
                {
                    GUI.contentColor = isPositive ? _positive : _negative;

                    if (duration > 0)
                    {
                        key = ActorAbilityId.Names_Ed[targetAbility].Concat("Temp");
                        DrawLabel(localization.GetFormatText(FILE, key, strValue, duration).Delete("<b>", "</b>"));
                    }
                    else 
                    {
                        key = ActorAbilityId.Names_Ed[targetAbility].Concat("Perm");
                        if (mod == TypeModifierId.TotalPercent & targetAbility == CurrentHP) 
                            key = "CurrentHPOfMaxPerm";

                        DrawLabel(localization.GetFormatText(FILE, key, strValue).Delete("<b>", "</b>"));
                    }
                }

                // Set Desc
                SetString(P_DESC_KEY, key);

                if(GetInt(P_REFLECT) > 0)
                {
                    if (isPositive)
                    { key = REFLECT_MINUS; GUI.contentColor = _negative; }
                    else
                    { key = REFLECT_PLUS; GUI.contentColor = _positive; }
                    DrawLabel(localization.GetFormatText(FILE, key, GetInt(P_REFLECT)).Delete("<b>", "</b>"));
                }

                GUI.contentColor = defaultColor;
                _position.x -= 35;
            }
            //==============================================
            string ValueToString(int value, int targetAbility, int typeModifier, bool isUsedAttack)
            {
                bool isPositive = value > 0;

                if (targetAbility == IsMove)
                    return isPositive ? PLUS : MINUS;
                if (isUsedAttack)
                    return isPositive ? value.ToString() : (-value).ToString();

                bool isPresent = !(typeModifier == TypeModifierId.Addition);
                string present = isPresent ? PRESENT : string.Empty;

                if (!isPresent & targetAbility <= MAX_ID_SHIFT_ABILITY)
                    value >>= SHIFT_ABILITY;

                return isPositive ? $"{PLUS}{value}{present}" : $"{value}{present}";
            }
            //==============================================
            string DrawAttackDesc(Localization localization, string strValue)
            {
                string key;
                int holy = GetInt(P_HOLY);
                int pierce = GetInt(P_PIERCE);
                if (pierce == 0)
                {
                    if (holy == 0)
                    {
                        key = "Damage";
                        DrawLabel(localization.GetFormatText(FILE, key, strValue).Delete("<b>", "</b>"));
                    }
                    else
                    {
                        key = "HolyDamage";
                        DrawLabel(localization.GetFormatText(FILE, key, strValue, holy).Delete("<b>", "</b>"));
                    }
                }
                else
                {
                    string desc;
                    if (holy == 0)
                    {
                        key = "DamagePierce";
                        desc = localization.GetFormatText(FILE, key, strValue, pierce).Delete("<b>", "</b>");
                        
                    }
                    else
                    {
                        key = "HolyDamagePierce";
                        desc = localization.GetFormatText(FILE, key, strValue, holy, pierce).Delete("<b>", "</b>");
                    }

                    foreach(var str in desc.Split("\n"))
                        DrawLabel(str);
                }
                return key;
            }
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyRateHeight(property, IdFromLabel(label)) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

        public static float GetPropertyRateHeight(SerializedProperty property, int id)
        {
            if (!property.isExpanded)
                return 1.1f;
            
            float size = 7f;
            var target = GetProperty(P_PARENT_TARGET).GetEnum<TargetOfSkill>();

            if (GetProperty(P_USED_ATTACK).boolValue)
            {
                size += 1f;
                if (target != TargetOfSkill.Self)
                {
                    size += 1f;
                    if (target == TargetOfSkill.Enemy && !GetProperty(P_IS_SELF).boolValue)
                    {
                        size += 1f;
                        if (GetProperty(P_PARENT_TYPE).boolValue)
                            size += 1f;
                        if (GetProperty(P_PIERCE).intValue > 0)
                            size += 1f;
                    }
                }
                if (GetProperty(P_REFLECT).intValue > 0)
                    size += 1f;
            }
            else
            {
                int targetAbility = GetProperty(P_TARGET_ABILITY).intValue;

                if (id == 0 && GetProperty(P_DUR).intValue == 0)
                    size += 1f;
                if (target != TargetOfSkill.Self)
                    size += 1f;
                if (targetAbility != CurrentAP & targetAbility != IsMove & targetAbility != APPerTurn)
                    size += 1f;
            }

            return size;

            #region Local: GetProperty(..)
            //==============================================
            SerializedProperty GetProperty(string name) => property.FindPropertyRelative(name);
            #endregion
        }
    }
}
