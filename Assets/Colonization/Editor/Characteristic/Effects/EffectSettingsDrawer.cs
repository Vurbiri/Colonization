namespace VurbiriEditor.Colonization.Characteristics
{
    using UnityEditor;
    using UnityEngine;
    using Vurbiri.Colonization.Characteristics;
    using static Vurbiri.Colonization.UI.CONST_UI_LNG_KEYS;

    [CustomPropertyDrawer(typeof(EffectSettings))]
    public class EffectSettingsDrawer : PropertyDrawerUtility
    {
        #region Consts
        private const float RATE_SIZE_FULL = 11f, RATE_SIZE_MIDL = 10f, RATE_SIZE_MINI = 9f;
        private const string P_TARGET_ACTOR = "_targetActor", P_TYPE_OP = "_typeModifier", P_VALUE = "_value", P_IS_N = "_isNegative", P_DUR = "_duration";
        private const string P_KEY_DESC = "_keyDescId";
        private const string P_TARGET_ABILITY = "_targetAbility", P_USED_ABILITY = "_usedAbility", P_CONTR_ABILITY = "_counteredAbility";
        private readonly (int min, int max) MIN_MAX_A = (0, 7), MIN_MAX_P = (50, 200);
        #endregion


        public override void OnGUI(Rect mainPosition, SerializedProperty mainProperty, GUIContent label)
        {
            base.OnGUI(mainPosition, mainProperty, label);

            bool isDuration, isNotUse = true;
            int target, used;

            EditorGUI.BeginProperty(mainPosition, label, mainProperty);

            isDuration = DrawIntSlider(P_DUR, 0, 5) > 0;

            if (!isDuration && !(isNotUse = (used = DrawId(P_USED_ABILITY, typeof(ActorAbilityId), true)) < 0))
            {
                Space(2f);
                DrawValue(used);
                DrawId(P_CONTR_ABILITY, typeof(ActorAbilityId));
            }

            Space(2f);
            target = DrawId(P_TARGET_ACTOR, typeof(TargetOfEffectId));
            used = DrawId(P_TARGET_ABILITY, typeof(ActorAbilityId));
            if (isNotUse)
                DrawValue(used);

            DrawLabelAndSetValue(P_IS_N, target == TargetOfEffectId.Enemy);

            Space(1.5f);
            DrawPopup(P_KEY_DESC, KEYS_DESK_EFFECTS);

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
            if (property.FindPropertyRelative(P_DUR).intValue > 0)
                return RATE_SIZE_MINI;

            if (property.FindPropertyRelative(P_USED_ABILITY).intValue < 0)
                return RATE_SIZE_MIDL;
            
            return RATE_SIZE_FULL;
        }
    }
}
