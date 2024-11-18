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
        private const float RATE_SIZE_FULL = 12.12f;
        private const string NAME_NEGATIVE_ELEMENT = "Negative Effect {0}", NAME_POSITIVE_ELEMENT = "Positive Effect {0}";
        private const string P_TARGET_ACTOR = "_targetActor", P_TYPE_OP = "_typeModifier", P_VALUE = "_value", P_IS_REFLECT = "_isReflect", P_DUR = "_duration";
        private const string P_DESC_KEY = "_descKeyId", P_IS_DESC_BASE = "_isDescKeyBase";
        private const string P_TARGET_ABILITY = "_targetAbility", P_USED_ABILITY = "_usedAbility", P_CONTR_ABILITY = "_counteredAbility";
        private const string P_PARENT_TARGET = "_parentTarget";
        private readonly (int min, int max) MIN_MAX_A = (0, 7), MIN_MAX_P = (50, 200);
        #endregion

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            base.OnGUI(position, mainProperty, label);

            int parentTarget = mainProperty.FindPropertyRelative(P_PARENT_TARGET).intValue;
            bool isParentSelf = parentTarget == TargetOfSkillId.Self;

            SerializedProperty target = mainProperty.FindPropertyRelative(P_TARGET_ACTOR);

            if (isParentSelf)
                target.intValue = TargetOfEffectId.Self;

            bool isDuration, isNotUse = true, isTarget = false;
            int usedAbility;
                        
            bool isNegative = parentTarget == TargetOfSkillId.Enemy & target.intValue == TargetOfEffectId.Target;

            label.text = string.Format(isNegative ? NAME_NEGATIVE_ELEMENT : NAME_POSITIVE_ELEMENT, IdFromLabel(label));

            EditorGUI.BeginProperty(_position, label, mainProperty);

            if (Foldout(label))
            {
                isDuration = DrawIntSlider(P_DUR, 0, 5) > 0;

                if (!isParentSelf & !isDuration && !(isNotUse = (usedAbility = DrawId(P_USED_ABILITY, typeof(ActorAbilityId), true)) < 0))
                {
                    Space(2f);
                    DrawValue(usedAbility);
                    DrawId(P_CONTR_ABILITY, typeof(ActorAbilityId));
                }

                Space(2f);
                if (isParentSelf)
                    DrawLabel(target.displayName, TargetOfEffectId.GetName(TargetOfEffectId.Self));
                else
                    isTarget = DrawId(P_TARGET_ACTOR, typeof(TargetOfEffectId)) == TargetOfEffectId.Target;

                usedAbility = DrawId(P_TARGET_ABILITY, typeof(ActorAbilityId));
                if (isNotUse)
                    DrawValue(usedAbility);

                if (isTarget)
                    DrawBool(P_IS_REFLECT);
                else
                    DrawLabelAndSetValue(P_IS_REFLECT, isTarget);

                Space(1.5f);
                DrawPopup(P_DESC_KEY, DESK_EFFECTS_KEYS);
                DrawBool(P_IS_DESC_BASE);

            }
            EditorGUI.EndProperty();

            #region Local: DrawValue()
            //==============================================
            void DrawValue(int usedAbility)
            {
                int min, max;
                EditorGUI.indentLevel++;
                if (DrawId(P_TYPE_OP, typeof(TypeModifierId)) == TypeModifierId.Percent)
                {
                    min = MIN_MAX_P.min;
                    max = MIN_MAX_P.max;
                }
                else
                {
                    int ratio = usedAbility <= ActorAbilityId.MAX_RATE_ABILITY ? ActorAbilityId.RATE_ABILITY : 1;
                    min = MIN_MAX_A.min * ratio;
                    max = MIN_MAX_A.max * ratio;
                }
                DrawIntSlider(P_VALUE, min, max);
                EditorGUI.indentLevel--;
                Space(2f);
            }
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyRateHeight(property) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

        public static float GetPropertyRateHeight(SerializedProperty property)
        {
            if (!property.isExpanded)
                return 1f;

            if (property.FindPropertyRelative(P_PARENT_TARGET).intValue == TargetOfSkillId.Self || property.FindPropertyRelative(P_DUR).intValue > 0)
                return RATE_SIZE_FULL - 2f;

            if (property.FindPropertyRelative(P_USED_ABILITY).intValue < 0)
                return RATE_SIZE_FULL - 1f;

            return RATE_SIZE_FULL;
        }
    }
}
