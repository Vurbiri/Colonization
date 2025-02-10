//Assets\Colonization\Editor\Characteristic\Effects\EffectSettingsDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.UI.CONST_UI_LNG_KEYS;

namespace VurbiriEditor.Colonization.Characteristics
{
    [CustomPropertyDrawer(typeof(EffectSettings))]
    public class EffectSettingsDrawer : PropertyDrawerUtility
    {
        #region Consts
        private const string NAME_NEGATIVE_ELEMENT = "Negative Effect {0}", NAME_POSITIVE_ELEMENT = "Positive Effect {0}";
        private const string P_TARGET_ACTOR = "_targetActor", P_TYPE_OP = "_typeModifier", P_VALUE = "_value", P_DUR = "_duration";
        private const string P_IS_REFLECT = "_isReflect", P_REFLECT = "_reflectValue";
        private const string P_DESC_KEY = "_descKeyId", P_IS_DESC_BASE = "_isDescKeyBase";
        private const string P_TARGET_ABILITY = "_targetAbility", P_USED_ABILITY = "_usedAbility", P_CONTR_ABILITY = "_counteredAbility";
        private const string P_PARENT_TARGET = "_parentTarget";
        #endregion

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            base.OnGUI(position, mainProperty, label);

            int id = IdFromLabel(label);

            var parentTarget = GetProperty(P_PARENT_TARGET).GetEnumValue<TargetOfSkill>();
            bool isParentSelf = parentTarget == TargetOfSkill.Self;

            SerializedProperty targetActorProperty = GetProperty(P_TARGET_ACTOR);

            if (isParentSelf)
                targetActorProperty.SetEnumValue(TargetOfEffect.Self);

            bool isDuration, isNotUsedAbility = true, isTarget = false;
            int usedAbility;
                        
            bool isNegative = parentTarget == TargetOfSkill.Enemy & targetActorProperty.GetEnumValue<TargetOfEffect>() == TargetOfEffect.Target;
            
            label.text = string.Format(isNegative ? NAME_NEGATIVE_ELEMENT : NAME_POSITIVE_ELEMENT, id);

            EditorGUI.BeginProperty(_position, label, mainProperty);

            if (Foldout(label))
            {
                isDuration = DrawInt(P_DUR, 0, 3) > 0;

                if (!isParentSelf & !isDuration && !(isNotUsedAbility = (usedAbility = DrawId<ActorAbilityId>(P_USED_ABILITY, true)) < 0))
                {
                    Space(2f);
                    DrawValue(usedAbility);
                    DrawId<ActorAbilityId>(P_CONTR_ABILITY, true);
                }

                Space(2f);
                if (isParentSelf)
                    DrawLabel(targetActorProperty.displayName, TargetOfEffect.Self.ToString());
                else
                    isTarget = DrawEnumPopup<TargetOfEffect>(P_TARGET_ACTOR) == TargetOfEffect.Target;

                usedAbility = DrawId<ActorAbilityId>(P_TARGET_ABILITY);
                if (isNotUsedAbility)
                    DrawValue(usedAbility);

                if (isTarget)
                    DrawReflect();
                else
                    DrawLabelAndSetValue(P_IS_REFLECT, isTarget);

                Space(1.5f);
                DrawIntPopup(P_DESC_KEY, DESK_EFFECTS_KEYS);
                DrawBool(P_IS_DESC_BASE);

            }
            EditorGUI.EndProperty();

            #region Local: DrawValue(..), DrawReflect()
            //==============================================
            void DrawValue(int usedAbility)
            {
                EditorGUI.indentLevel++;

                int typeModifierId = DrawId<TypeModifierId>(P_TYPE_OP);

                if (typeModifierId == TypeModifierId.BasePercent)
                    DrawInt(P_VALUE, "Value (%)", 5, 300, 100);
                else if(typeModifierId == TypeModifierId.TotalPercent)
                    DrawInt(P_VALUE, "Value (%)", 5, 300, 100);
                else if (usedAbility <= ActorAbilityId.MAX_RATE_ABILITY)
                    DrawRateValue(-50, 50);
                else
                    DrawInt(P_VALUE, -5, 5, 0);

                EditorGUI.indentLevel--;
                Space(2f);
            }
            //==============================================
            void DrawRateValue(int min, int max)
            {
                SerializedProperty valueProperty = GetProperty(P_VALUE);
                int rate = ActorAbilityId.RATE_ABILITY;
                _position.y += _height;
                valueProperty.intValue = EditorGUI.IntSlider(_position, "Value", valueProperty.intValue / rate, min, max) * rate;
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

            float size = 13.14f;

            if (!property.FindPropertyRelative(P_IS_REFLECT).boolValue)
                size -= 1.2f;

            if (property.FindPropertyRelative(P_PARENT_TARGET).GetEnumValue<TargetOfSkill>() == TargetOfSkill.Self || property.FindPropertyRelative(P_DUR).intValue > 0)
                return size - 2f;

            if (property.FindPropertyRelative(P_USED_ABILITY).intValue < 0)
                return size - 1f;

            return size;
        }
    }
}
