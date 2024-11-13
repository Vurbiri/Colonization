namespace VurbiriEditor.Colonization
{
    using UnityEditor;
    using UnityEngine;
    using Vurbiri.Colonization;
    using static Vurbiri.Colonization.UI.CONST_UI_LNG_KEYS;

    [CustomPropertyDrawer(typeof(EffectSettings))]
    public class EffectSettingsDrawer : PropertyDrawerUtility
    {
        #region Consts
        private const float RATE_SIZE = 7.7f;
        private const string P_TARGET = "_target", P_TYPE_OP = "_typeOperation", P_VALUE = "_value", P_DUR = "_duration";
        private const string P_KEY_DESC = "_keyDescId";
        private const string P_TARGET_ACTOR = "actor", P_TARGET_AB = "ability";
        private readonly string[] _nameOp = { "Addition", "Percent" };
        private readonly int[] _valueOp = { 0, 2 };
        #endregion


        public override void OnGUI(Rect mainPosition, SerializedProperty mainProperty, GUIContent label)
        {
            base.OnGUI(mainPosition, mainProperty, label);

            SerializedProperty propertyTarget = mainProperty.FindPropertyRelative(P_TARGET);

            EditorGUI.BeginProperty(mainPosition, label, mainProperty);

            DrawId(propertyTarget, P_TARGET_ACTOR, typeof(TargetOfEffectId));
            DrawId(propertyTarget, P_TARGET_AB, typeof(ActorAbilityId));
            
            Space();
            DrawInt(P_VALUE);
            DrawIntPopup(P_TYPE_OP, _nameOp, _valueOp);

            Space();
            DrawIntSlider(P_DUR, 0, 5);

            Space(1.5f);
            DrawPopup(P_KEY_DESC, KEYS_DESK_EFFECTS);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyRateHeight(property) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

        public static float GetPropertyRateHeight(SerializedProperty property)
        {
            return RATE_SIZE;
        }
    }
}
