//Assets\Colonization\Editor\Characteristic\Effects\EffectSettingsDrawer.cs
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.UI.CONST_UI_LNG_KEYS;

namespace VurbiriEditor.Colonization.Characteristics
{
    using static ActorAbilityId;

    [CustomPropertyDrawer(typeof(EffectHitSettings))]
    public class EffectHitSettingsDrawer : PropertyDrawerUtility
    {
        #region Consts
        private const string NAME_POSITIVE = "Positive Effect {0}", NAME_NEGATIVE = "Negative Effect {0}", NAME_VOID ="Void Effect {0}";
        private const string P_IS_SELF = "_isSelf", P_TYPE_OP = "_typeModifier", P_VALUE = "_value", P_DUR = "_duration";
        private const string P_IS_REFLECT = "_isReflect", P_REFLECT = "_reflectValue";
        private const string P_DESC_KEY = "_descKeyId";
        private const string P_TARGET_ABILITY = "_targetAbility", P_USED_ATTACK = "_useAttack", P_USED_DEFENSE = "_useDefense";
        private const string P_PARENT_TARGET = "_parentTarget";
        #endregion

        #region Values
        private static readonly string[] NamesAbilitiesDuration = { ActorAbilityId.Names[MaxHP], ActorAbilityId.Names[HPPerTurn], ActorAbilityId.Names[Attack],
                                                               ActorAbilityId.Names[Defense] };
        private static readonly int[] ValuesAbilitiesDuration = { MaxHP, HPPerTurn, Attack, Defense };

        private static readonly string[] NamesModifiersDuration = { "Flat", "Percent" };
        private static readonly int[] ValuesModifiersDuration = { TypeModifierId.Addition, TypeModifierId.TotalPercent };

        private static readonly string[] NamesAbilitiesInstant = { ActorAbilityId.Names[CurrentHP], ActorAbilityId.Names[CurrentAP], ActorAbilityId.Names[IsMove] };
        private static readonly int[] ValuesAbilitiesInstant = { CurrentHP, CurrentAP, IsMove };

        private static readonly string[] NamesModifiersCurrentHP = { "Percent of CurrentHP", "Flat", "Percent of MaxHP" };

        private static readonly HashSet<int> NonReflect = new() { CurrentAP, IsMove };
        #endregion

        private static readonly Color Positive = new(0.5f, 1f, 0.3f, 1f), Negative = new(1f, 0.5f, 0.3f, 1f);

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            base.OnGUI(position, mainProperty, label);

            int id = IdFromLabel(label);
            var (name, color) = GetSkin();

            label.text = string.Format(name, id);
            EditorGUI.BeginProperty(_position, label, mainProperty);

            Color defaultColor = GUI.color; GUI.color = color;
            Foldout(label); GUI.color = defaultColor;

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
                if ((isNotDuration = DrawInt(P_DUR, 0, 3) <= 0) & id == 0)
                {
                    if(isUsedAttack = DrawBool(P_USED_ATTACK, isTargetEnemy ? "Is Attack" : "Is Heal"))
                        DrawUsedAttack(isTargetEnemy);
                }
                else
                {
                    SetBool(P_USED_ATTACK, isUsedAttack = false);
                    SetBool(P_USED_DEFENSE, false);
                }

                if (!isUsedAttack)
                    targetAbility = DrawDirectEffect(!isNotDuration);

                if (isTarget & isUsedAttack)
                    DrawReflect();
                else
                    SetBool(P_IS_REFLECT, false);

                DrawLine();
                SetAndDrawDesc(isUsedAttack, targetAbility);
                DrawLine();

            }
            EditorGUI.EndProperty();

            #region Local: GetSkin(), GetTargetSkill(..), DrawUsedAttack(..), DrawUsedAttack(..), DrawDirectEffect(..), DrawDurationValue(..), DrawInstantValue(..), DrawRateValue(..), DrawMoveValue(..), DrawReflect(), SetDefaultValue()
            //==============================================
            (string name, Color color) GetSkin()
            {
                SerializedProperty property = GetProperty(P_VALUE);

                if (property.intValue > 0)
                    return (NAME_POSITIVE, Positive);

                if (property.intValue < 0)
                    return (NAME_NEGATIVE, Negative);

                return (NAME_VOID, new(0.1f, 0.1f, 0.1f, 1f));
            }
            //==============================================
            void GetTargetSkill(out bool isTargetSkillSelf, out bool isTargetSkillEnemy)
            {
                var parentTarget = GetProperty(P_PARENT_TARGET).GetEnum<TargetOfSkill>();
                isTargetSkillSelf = parentTarget == TargetOfSkill.Self;
                isTargetSkillEnemy = parentTarget == TargetOfSkill.Enemy;
            }
            //==============================================
            void DrawUsedAttack(bool isTargetEnemy)
            {
                Space();
                EditorGUI.indentLevel++;

                SetInt(P_TARGET_ABILITY, CurrentHP);
                SetInt(P_TYPE_OP, TypeModifierId.TotalPercent);

                DrawRateValue("Value (%)", 5, 300, 100, isTargetEnemy ? -1 : 1);

                if (isTargetEnemy)
                    DrawBool(P_USED_DEFENSE);
                else
                    SetBool(P_USED_DEFENSE, false);

                EditorGUI.indentLevel--;
                Space(2f);
            }
            //==============================================
            int DrawDirectEffect(bool isDuration)
            {
                SerializedProperty targetAbility = GetProperty(P_TARGET_ABILITY);
                
                Space();
                if (isDuration)
                {
                    SetDefaultValue(targetAbility, ValuesAbilitiesDuration);
                    return DrawDurationValue(DrawIntPopup(targetAbility, NamesAbilitiesDuration, ValuesAbilitiesDuration));
                }
                else
                {
                    SetDefaultValue(targetAbility, ValuesAbilitiesInstant);
                    return DrawInstantValue(DrawIntPopup(targetAbility, NamesAbilitiesInstant, ValuesAbilitiesInstant));
                }
                
            }
            //==============================================
            int DrawDurationValue(int usedAbility)
            {
                Space();
                EditorGUI.indentLevel++;

                int typeModifierId = DrawIntPopup(P_TYPE_OP, NamesModifiersDuration, ValuesModifiersDuration);

                if (typeModifierId == TypeModifierId.TotalPercent)
                    DrawInt(P_VALUE, "Value (%)", -200, 200, 100);
                else 
                    DrawRateValue("Value", -50, 50);
    
                EditorGUI.indentLevel--;
                Space(2f);

                return usedAbility;
            }
            //==============================================
            int DrawInstantValue(int usedAbility)
            {
                Space();
                EditorGUI.indentLevel++;

                if (usedAbility == CurrentHP)
                {
                    if (DrawIntPopup(P_TYPE_OP, NamesModifiersCurrentHP, ActorAbilityId.Values) == TypeModifierId.Addition)
                        DrawRateValue("Value", -75, 75);
                    else
                        DrawInt(P_VALUE, "Value (%)", -100, 100);
                }
                else
                {
                    SetInt(P_TYPE_OP, TypeModifierId.Addition);

                    if (usedAbility == CurrentAP)
                        DrawInt(P_VALUE, "Value", -5, 5);
                    else
                        DrawMoveValue();
                }

                EditorGUI.indentLevel--;
                Space(2f);

                return usedAbility;
            }
            //==============================================
            void DrawRateValue(string displayName, int min, int max, int defaultValue = 0, int rate = RATE_ABILITY)
            {
                SerializedProperty property = GetProperty(P_VALUE);
                int value = property.intValue / rate;  

                if (value < min | value > max)
                    defaultValue = Mathf.Clamp(defaultValue, min, max);
                else
                    defaultValue = value;

                _position.y += _height;
                property.intValue = EditorGUI.IntSlider(_position, displayName, defaultValue, min, max) * rate;
            }
            //==============================================
            void DrawMoveValue()
            {
                SerializedProperty property = GetProperty(P_VALUE);

                _position.y += _height;
                property.intValue = EditorGUI.Toggle(_position, "Is Move", property.intValue > 0) ? 1 : -1;
            }
            //==============================================
            void DrawReflect()
            {
                if (DrawBool(P_IS_REFLECT))
                {
                    SerializedProperty property = GetProperty(P_REFLECT);
                    int value = -property.intValue;
                    int min = 10, max = 200;
                    if (value < min | value > max)
                        value = 100;

                    _position.y += _height;
                    EditorGUI.indentLevel++;
                    property.intValue = -EditorGUI.IntSlider(_position, "value (%)", value, min, max);
                    EditorGUI.indentLevel--;
                    Space(2f);
                }
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
                    {  key = "Healing"; GUI.contentColor = Positive; }
                    else
                    {  key = GetBool(P_USED_DEFENSE) ? "Damage" : "DamageNotDef"; }

                    DrawLabel(Localization.GetTextFormat(FILE, key, strValue).Delete("<b>", "</b>").Replace("\n", " "));
                }
                else
                {
                    GUI.contentColor = isPositive ? Positive : Negative;

                    if (duration > 0)
                    {
                        key = ActorAbilityId.Names[targetAbility].Concat("Temp");
                        DrawLabel(Localization.GetTextFormat(FILE, key, strValue, duration).Delete("<b>", "</b>"));
                    }
                    else 
                    {
                        key = ActorAbilityId.Names[targetAbility].Concat("Perm");
                        if (mod == TypeModifierId.TotalPercent & targetAbility == CurrentHP) 
                            key = "CurrentHPOfMaxPerm";

                        DrawLabel(Localization.GetTextFormat(FILE, key, strValue).Delete("<b>", "</b>"));
                    }
                }

                

                SerializedProperty property = GetProperty(P_DESC_KEY);
                property.intValue = Array.IndexOf(DESK_EFFECTS_KEYS, key);

                if (property.intValue < 0)
                    Debug.LogWarning($"Не найден ключ [{key}]");

                if(GetBool(P_IS_REFLECT))
                {
                    if (isPositive)
                    { key = REFLECT_MINUS; GUI.contentColor = Negative; }
                    else
                    { key = REFLECT_PLUS; GUI.contentColor = Positive; }
                    DrawLabel(Localization.GetTextFormat(FILE, key, -GetInt(P_REFLECT)).Delete("<b>", "</b>"));
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

                if (!isPresent & targetAbility <= MAX_RATE_ABILITY)
                    value /= RATE_ABILITY;

                return isPositive ? $"{PLUS}{value}{present}" : $"{value}{present}";
            }
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyRateHeight(property, IdFromLabel(label)) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

        public static float GetPropertyRateHeight(SerializedProperty property, int id)
        {
            if (!property.isExpanded)
                return 1f;
            
            float size = 11.7f;
            int targetAbility = GetProperty(P_TARGET_ABILITY).intValue;
            bool isTarget = !GetProperty(P_IS_SELF).boolValue;
            bool isTargetEnemy = isTarget && GetProperty(P_PARENT_TARGET).GetEnum<TargetOfSkill>() == TargetOfSkill.Enemy;
            bool isUsedAttack = GetProperty(P_USED_ATTACK).boolValue;


            if (!isTargetEnemy & isUsedAttack)
                size -= 1f;
            else if (id == 0 & !isUsedAttack)
                size += targetAbility != CurrentHP ? 0f : 1f;
            else if (targetAbility != CurrentHP && GetProperty(P_DUR).intValue == 0)
                size -= 1f;

            if (!isTarget | !isUsedAttack)
                size -= 3.1f;
            else if (!GetProperty(P_IS_REFLECT).boolValue)
                size -= 2.1f;

            return size;

            #region Local: GetProperty(..)
            //==============================================
            SerializedProperty GetProperty(string name) => property.FindPropertyRelative(name);
            #endregion
        }
    }
}
