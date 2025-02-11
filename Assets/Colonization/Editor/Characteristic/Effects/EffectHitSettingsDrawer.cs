//Assets\Colonization\Editor\Characteristic\Effects\EffectSettingsDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.UI.CONST_UI_LNG_KEYS;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomPropertyDrawer(typeof(EffectHitSettings))]
    public class EffectHitSettingsDrawer : PropertyDrawerUtility
    {
        #region Consts
        private const string NAME_ELEMENT = "EFFECT {0}";
        private const string P_TARGET_ACTOR = "_targetActor", P_TYPE_OP = "_typeModifier", P_VALUE = "_value", P_DUR = "_duration";
        private const string P_IS_REFLECT = "_isReflect", P_REFLECT = "_reflectValue";
        private const string P_DESC_KEY = "_descKeyId", P_IS_DESC_BASE = "_isDescKeyBase";
        private const string P_TARGET_ABILITY = "_targetAbility", P_USED_ATTACK = "_useAttack", P_USED_DEFENSE = "_useDefense";
        private const string P_PARENT_TARGET = "_parentTarget";
        #endregion

        private static readonly string[] NamesAbilitiesDur = { "Max HP", "HP Per Turn", "Attack", "Defense" };
        private static readonly int[] ValuesModifiers = { ActorAbilityId.MaxHP, ActorAbilityId.HPPerTurn, ActorAbilityId.Attack, ActorAbilityId.Defense };

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            base.OnGUI(position, mainProperty, label);

            GetTargetSkill(out bool isTargetSkillSelf, out bool isTargetSkillEnemy);

            bool isUsedAttack = false, isTarget = false;

            
            label.text = string.Format(NAME_ELEMENT, IdFromLabel(label));
            EditorGUI.BeginProperty(_position, label, mainProperty);

            if (Foldout(label))
            {
                if (isTargetSkillSelf)
                    SetLabelEnum(P_TARGET_ACTOR, TargetOfEffect.Self);
                else
                    isTarget = DrawEnum<TargetOfEffect>(P_TARGET_ACTOR) == TargetOfEffect.Target;

                bool isTargetEnemy = isTargetSkillEnemy & isTarget;

                Space();
                if ((DrawInt(P_DUR, 0, 3) <= 0) && DrawBool(P_USED_ATTACK, isTargetEnemy ? "Is Attack?" : "Is Heal?"))
                {
                    isUsedAttack = true;
                    DrawUsedAttack(isTargetEnemy);
                }

                if (!isUsedAttack)
                    DrawValue(DrawId<ActorAbilityId>(P_TARGET_ABILITY));

                if (isTargetEnemy)
                    DrawReflect();
                else
                    SetBool(P_IS_REFLECT, false);

                
                Space(2f);
                DrawIntPopup(P_DESC_KEY, DESK_EFFECTS_KEYS);
                DrawBool(P_IS_DESC_BASE);
                Space(2f);
                DrawLine();

            }
            EditorGUI.EndProperty();

            #region Local: GetTargetSkill(..), DrawValue(..), DrawReflect()
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
            void DrawValue(int usedAbility)
            {
                Space();

                EditorGUI.indentLevel++;

                int typeModifierId = DrawId<TypeModifierId>(P_TYPE_OP);

                if (typeModifierId == TypeModifierId.BasePercent)
                    DrawInt(P_VALUE, "Value (%)", 5, 300, 100);
                else if(typeModifierId == TypeModifierId.TotalPercent)
                    DrawInt(P_VALUE, "Value (%)", 5, 300, 100);
                else if (usedAbility <= ActorAbilityId.MAX_RATE_ABILITY)
                    DrawRateValue("Value (%)", - 50, 50, 0);
                else
                    DrawInt(P_VALUE, -5, 5, 0);

                EditorGUI.indentLevel--;
            }
            //==============================================
            void DrawRateValue(string displayName, int min, int max, int defaultValue, int rate = ActorAbilityId.RATE_ABILITY)
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
            void DrawReflect()
            {
                if (DrawBool(P_IS_REFLECT))
                {
                    EditorGUI.indentLevel++;
                    DrawInt(GetProperty(P_REFLECT), "value (%)", 10, 250, 100);
                    EditorGUI.indentLevel--;
                    Space(2f);
                }
            }
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyRateHeight(property) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

        public static float GetPropertyRateHeight(SerializedProperty property)
        {
            if (!property.isExpanded)
                return 1f;
            
            float size = 12.6f;
            bool isTargetEnemy = property.FindPropertyRelative(P_PARENT_TARGET).GetEnum<TargetOfSkill>() == TargetOfSkill.Enemy &
                                 property.FindPropertyRelative(P_TARGET_ACTOR).GetEnum<TargetOfEffect>() == TargetOfEffect.Target;

            if (property.FindPropertyRelative(P_DUR).intValue > 0)
            {
                size -= 1.2f;
            }
            else if (property.FindPropertyRelative(P_USED_ATTACK).boolValue)
            {
                size -= 1f;
                if (!isTargetEnemy)
                    size -= 1f;
            }

            if(!isTargetEnemy)
                size -= 2.2f;
            else if (!property.FindPropertyRelative(P_IS_REFLECT).boolValue)
                size -= 1.2f;

            return size;
        }
    }
}
