using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using Vurbiri.International;
using static UnityEditor.EditorGUI;
using static Vurbiri.Colonization.HitEffectSettings;
using static Vurbiri.Colonization.UI.CONST_UI;

namespace VurbiriEditor.Colonization
{
    using static ActorAbilityId;

    [CustomPropertyDrawer(typeof(HitEffectSettings))]
    public class HitEffectSettingsDrawer : PropertyDrawerUtility
    {
        private const string NAME_POSITIVE = "Positive Effect {0}", NAME_NEGATIVE = "Negative Effect {0}", NAME_VOID = "Void Effect {0}";

        #region Values
        private readonly string[] _namesAbilitiesDuration =
            { ActorAbilityId.Names_Ed[MaxHP], ActorAbilityId.Names_Ed[HPPerTurn], ActorAbilityId.Names_Ed[Attack], ActorAbilityId.Names_Ed[Defense], ActorAbilityId.Names_Ed[Pierce],
              ActorAbilityId.Names_Ed[MaxAP], ActorAbilityId.Names_Ed[APPerTurn]};
        private readonly int[] _valuesAbilitiesDuration = { MaxHP, HPPerTurn, Attack, Defense, Pierce, MaxAP, APPerTurn };

        private readonly string[] _namesModifiersDuration = { "Flat", "Percent" };
        private readonly int[] _valuesModifiersDuration = { TypeModifierId.Addition, TypeModifierId.TotalPercent };

        private readonly string[] _namesAbilitiesInstant = { ActorAbilityId.Names_Ed[CurrentHP], ActorAbilityId.Names_Ed[CurrentAP], ActorAbilityId.Names_Ed[IsMove], "Clear Effects" };
        private readonly int[] _valuesAbilitiesInstant = { CurrentHP, CurrentAP, IsMove, ClearEffectsId.Code };

        private readonly string[] _namesModifiersCurrentHP = { "Percent of CurrentHP", "Flat", "Percent of MaxHP" };
        #endregion

        private readonly Color _positive = new(0.5f, 1f, 0.3f, 1f), _negative = new(1f, 0.5f, 0.3f, 1f), _void = new(0.1f, 0.1f, 0.1f, 1f);

        protected override void OnGUI()
        {
            var propertyTargetAbility = GetProperty(targetAbilityField);

            int id = IdFromLabel();
            var (name, color) = GetSkin(propertyTargetAbility);

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
                    SetBool(isSelfField, true);
                else
                    isTarget = !(isTargetSkillSelf = DrawBool(isSelfField));

                bool isTargetEnemy = isTargetSkillEnemy & isTarget;

                Space();

                isNotDuration = DrawInt(durationField, 0, 3) <= 0;

                if (isNotDuration & id == 0 && (isUsedAttack = DrawBool(useAttackField, isTargetEnemy ? "Is Attack" : "Is Heal")))
                {
                    Space();
                    indentLevel++;

                    propertyTargetAbility.intValue = targetAbility;
                    SetInt(typeModifierField, TypeModifierId.TotalPercent);

                    if (isTargetEnemy)
                        DrawForEnemy();
                    else if(isTargetSkillSelf)
                        DrawForSelf();
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
                    targetAbility = DrawTargetEffect(!isNotDuration, propertyTargetAbility);

                Space(3f);

                DrawLine();
                SetAndDrawDesc(isUsedAttack, targetAbility);
                DrawLine();

            }
            EndProperty();

            #region Local
            //==============================================
            (string name, Color color) GetSkin(SerializedProperty targetAbility)
            {
                (string name, Color color) output;
                if (targetAbility.intValue == ClearEffectsId.Code)
                {
                    int mod = GetInt(typeModifierField);
                    
                    if (mod == ClearEffectsId.Positive)
                        output = (NAME_NEGATIVE, _negative);
                    else if (mod == ClearEffectsId.Negative)
                        output = (NAME_POSITIVE, _positive);
                    else
                        output = (NAME_VOID, _void);
                }
                else
                {
                    int value = GetInt(valueField);

                    if (value > 0)
                        output = (NAME_POSITIVE, _positive);
                    else if (value < 0)
                        output = (NAME_NEGATIVE, _negative);
                    else
                        output = (NAME_VOID, _void);
                }
                return output;
            }
            //==============================================
            void GetTargetSkill(out bool isTargetSkillSelf, out bool isTargetSkillEnemy)
            {
                var parentTarget = GetProperty(parentTargetField).GetEnum<TargetOfSkill>();
                isTargetSkillSelf = parentTarget == TargetOfSkill.Self;
                isTargetSkillEnemy = parentTarget == TargetOfSkill.Enemy;
            }
            //==============================================
            void SetNotAttack()
            {
                SetBool(useAttackField, false);
                SetInt(holyField, 0);
                SetInt(pierceField, 0);
                SetInt(reflectField, 0);
            }
            //==============================================
            void DrawForEnemy()
            {
                DrawAttack(5, 300, 100);
                if(GetProperty(isWarriorField).boolValue)
                    DrawInt(holyField, "Holy (%)", 0, 305);
                else
                    SetInt(holyField, 0);
                DrawInt(pierceField, "Pierce (%)", 0, 100);
                DrawInt(reflectField, "Leech (%)", 0, 200);
            }
            //==============================================
            void DrawForFriend()
            {
                DrawInt(valueField, "Heal (%)", 5, 300, 100);
                SetInt(holyField, 0);
                SetInt(pierceField, 0);
                DrawInt(reflectField, "Loss (%)", 0, 200);
            }
            //==============================================
            void DrawForSelf()
            {
                DrawInt(valueField, "Heal (%)", 5, 300, 100);
                SetInt(holyField, 0);
                SetInt(pierceField, 0);
                SetInt(reflectField, 0);
            }
            //==============================================
            int DrawTargetEffect(bool isDuration, SerializedProperty targetAbility)
            {
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

                if (usedAbility == MaxAP || usedAbility == APPerTurn)
                {
                    SetInt(typeModifierField, TypeModifierId.Addition);
                    DrawFlatValue(-2, 2);
                }
                else
                {
                    if (DrawIntPopup(typeModifierField, _namesModifiersDuration, _valuesModifiersDuration) == TypeModifierId.TotalPercent)
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
                    if (DrawIntPopup(typeModifierField, _namesModifiersCurrentHP, ActorAbilityId.Values_Ed) == TypeModifierId.Addition)
                        DrawShiftValue(-75, 75);
                    else
                        DrawPercentValue(-100, 100);
                }
                else if (usedAbility == ClearEffectsId.Code)
                {
                    DrawId<ClearEffectsId>(typeModifierField, false);
                    DrawFlatValue(1, 3, 1);
                }
                else
                {
                    SetInt(typeModifierField, TypeModifierId.Addition);

                    if (usedAbility == CurrentAP)
                        DrawFlatValue(-3, 3);
                    else
                        DrawMoveValue();
                }

                indentLevel--;

                return usedAbility;
            }
            //==============================================
            void DrawAttack(int min, int max, int defaultValue)
            {
                SerializedProperty property = GetProperty(valueField);
                int value = property.intValue * -1;

                if (value < min | value > max)
                    defaultValue = Mathf.Clamp(value, min, max);
                else
                    defaultValue = value;

                _position.y += _height;
                property.intValue = IntSlider(_position, "Attack (%)", defaultValue, min, max) * -1;
            }
            //==============================================
            void DrawPercentValue(int min, int max, int defaultValue = 0) => DrawInt(valueField, "Value (%)", min, max, defaultValue);
            //==============================================
            void DrawFlatValue(int min, int max, int defaultValue = 0) => DrawInt(valueField, "Value", min, max, defaultValue);
            //==============================================
            void DrawShiftValue(int min, int max)
            {
                SerializedProperty property = GetProperty(valueField);
                int value = property.intValue >> SHIFT_ABILITY;  

                _position.y += _height;
                property.intValue = IntSlider(_position, "Value", Mathf.Clamp(value, min, max), min, max) << SHIFT_ABILITY;
            }
            //==============================================
            void DrawMoveValue()
            {
                SerializedProperty property = GetProperty(valueField);

                _position.y += _height;
                property.intValue = Toggle(_position, "Is Move", property.intValue > 0) ? 1 : -1;
            }
            //==============================================
            void SetDefaultValue(SerializedProperty property, int[] values)
            {
                int value = property.intValue;
                for (int i = 0; i < values.Length; ++i)
                    if (values[i] == value)
                        return;

                property.intValue = values[0];
            }
            //==============================================
            void SetAndDrawDesc(bool isUsedAttack, int ability)
            {
                var localization = Localization.ForEditor(LangFiles.Actors);

                _position.x += 35;

                Color defaultColor = GUI.contentColor;
                int value = GetInt(valueField);
                string key;

                if (isUsedAttack)
                {
                    if (value > 0)
                        key = GetAndDrawHealDesc(localization, value, GetInt(reflectField));
                    else
                        key = GetAndDrawAttackDesc(localization, value, GetInt(reflectField));
                }
                else
                {
                    int duration = GetInt(durationField);

                    GUI.contentColor = value > 0 ? _positive : _negative;

                    if (duration > 0)
                    {
                        key = ActorAbilityId.Names_Ed[ability].Concat("Temp");
                        DrawLabel(localization.GetFormatText(FILE, key, HitEffectSettings.ValueToString(value, ability, GetInt(typeModifierField)), duration).Delete("<b>", "</b>"));
                    }
                    else 
                    {
                        key = GetAndDrawPermDesc(localization, value, ability);
                    }
                }

                // Set Desc
                SetString(descKeyField, key);

                GUI.contentColor = defaultColor;
                _position.x -= 35;
            }
            //==============================================
            string GetAndDrawHealDesc(Localization localization, int value, int reflect)
            {
                string key = "Healing";
                GUI.contentColor = _positive;
                DrawLabel(localization.GetFormatText(FILE, key, value).Delete("<b>", "</b>"));
                if (reflect > 0)
                {
                    GUI.contentColor = _negative;
                    DrawLabel(localization.GetFormatText(FILE, REFLECT_MINUS, GetInt(reflectField)).Delete("<b>", "</b>"));
                }
                return key;
            }
            //==============================================
            string GetAndDrawAttackDesc(Localization localization, int value, int reflect)
            {
                value = -value;

                string key;
                int holy = GetInt(holyField);
                int pierce = GetInt(pierceField);
                if (pierce == 0)
                {
                    if (holy == 0)
                    {
                        key = "Damage";
                        DrawLabel(localization.GetFormatText(FILE, key, value).Delete("<b>", "</b>"));
                    }
                    else
                    {
                        key = "HolyDamage";
                        DrawLabel(localization.GetFormatText(FILE, key, value, holy).Delete("<b>", "</b>"));
                    }
                }
                else
                {
                    string desc;
                    if (holy == 0)
                    {
                        key = "DamagePierce";
                        desc = localization.GetFormatText(FILE, key, value, pierce).Delete("<b>", "</b>");
                        
                    }
                    else
                    {
                        key = "HolyDamagePierce";
                        desc = localization.GetFormatText(FILE, key, value, holy, pierce).Delete("<b>", "</b>");
                    }

                    foreach(var str in desc.Split("\n"))
                        DrawLabel(str);
                }

                if (reflect > 0)
                {
                    GUI.contentColor = _positive;
                    DrawLabel(localization.GetFormatText(FILE, REFLECT_PLUS, GetInt(reflectField)).Delete("<b>", "</b>"));
                }

                return key;
            }
            //==============================================
            string GetAndDrawPermDesc(Localization localization, int value, int ability)
            {
                string key;
                int mod = GetInt(typeModifierField);

                if (ability == CurrentHP)
                {
                    key = mod == TypeModifierId.TotalPercent ? "CurrentHPOfMaxPerm" : "CurrentHPPerm";
                    DrawLabel(localization.GetFormatText(FILE, key, HitEffectSettings.ValueToString(value, ability, GetInt(typeModifierField))).Delete("<b>", "</b>"));
                }
                else if (ability == CurrentAP)
                {
                    key = "CurrentAPPerm";
                    DrawLabel(localization.GetFormatText(FILE, key, value.ToString("+#;-#;0")).Delete("<b>", "</b>"));

                }
                else if (ability == IsMove)
                {
                    key = value > 0 ? "MovePlus" : "MoveMinus";
                    DrawLabel(localization.GetText(FILE, key));
                }
                else
                {
                    key = "ClearEffects".Concat(ClearEffectsId.Names_Ed[mod]);

                    if (mod == ClearEffectsId.Positive) GUI.contentColor = _negative;
                    else if (mod == ClearEffectsId.Negative) GUI.contentColor = _positive;
                    else GUI.contentColor = Color.cyan;

                    DrawLabel(localization.GetFormatText(FILE, key, value).Delete("<b>", "</b>"));
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
            var target = GetProperty(parentTargetField).GetEnum<TargetOfSkill>();

            if (GetProperty(useAttackField).boolValue)
            {
                if (target != TargetOfSkill.Self)
                {
                    size += 1f;
                    if (!GetProperty(isSelfField).boolValue)
                    { 
                        size += 1f;
                        if (target == TargetOfSkill.Enemy)
                        {
                            size += 1f;
                            if (GetProperty(isWarriorField).boolValue)
                                size += 1f;
                            if (GetProperty(pierceField).intValue > 0)
                                size += 1f;
                        }

                        if (GetProperty(reflectField).intValue > 0)
                            size += 1f;
                    }
                }
            }
            else
            {
                int targetAbility = GetProperty(targetAbilityField).intValue;

                if (id == 0 && GetProperty(durationField).intValue == 0)
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
