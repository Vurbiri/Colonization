//Assets\Colonization\Editor\Characteristic\Effects\EffectSettingsDrawer.cs
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
        private const string P_DESC_KEY = "_descKeyId", P_IS_DESC_BASE = "_isKeyBase";
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

                if (isTargetEnemy && !NonReflect.Contains(targetAbility))
                    DrawReflect();
                else
                    SetBool(P_IS_REFLECT, false);

                Space(2f);
                DrawLabel("UI:");
                EditorGUI.indentLevel++;
                DrawIntPopup(P_DESC_KEY, DESK_EFFECTS_KEYS);
                DrawBool(P_IS_DESC_BASE);
                EditorGUI.indentLevel--;
                Space(2f);
                DrawLine();

            }
            EditorGUI.EndProperty();

            #region Local: GetSkin(), GetTargetSkill(..), DrawUsedAttack(..), DrawUsedAttack(..), DrawDirectEffect(..), DrawDurationValue(..), DrawInstantValue(..), DrawRateValue(..), DrawMoveValue(..), DrawReflect(), SetDefaultValue()
            //==============================================
            (string name, Color color) GetSkin()
            {
                SerializedProperty property = GetProperty(P_VALUE);

                if (property.intValue > 0)
                    return (NAME_POSITIVE, new(0.5f, 1f, 0.3f, 1f));

                if (property.intValue < 0)
                    return (NAME_NEGATIVE, new(1f, 0.5f, 0.3f, 1f));

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
                    DrawInt(P_VALUE, "Value (%)", -300, 300, 100);
                else 
                    DrawRateValue("Value (%)", -50, 50);
    
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
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyRateHeight(property) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

        public static float GetPropertyRateHeight(SerializedProperty property)
        {
            if (!property.isExpanded)
                return 1f;
            
            float size = 12.6f;
            int targetAbility = GetProperty(P_TARGET_ABILITY).intValue;
            bool isTargetEnemy = !GetProperty(P_IS_SELF).boolValue && GetProperty(P_PARENT_TARGET).GetEnum<TargetOfSkill>() == TargetOfSkill.Enemy;
            
            if (!isTargetEnemy && GetProperty(P_USED_ATTACK).boolValue)
                size -= 1f;
            else if(targetAbility != CurrentHP && GetProperty(P_DUR).intValue == 0)
                size -= 1f;

            if (!isTargetEnemy || NonReflect.Contains(targetAbility))
                size -= 2.1f;
            else if (!GetProperty(P_IS_REFLECT).boolValue)
                size -= 1.1f;

            return size;

            #region Local: GetProperty(..)
            //==============================================
            SerializedProperty GetProperty(string name) => property.FindPropertyRelative(name);
            #endregion
        }
    }
}
