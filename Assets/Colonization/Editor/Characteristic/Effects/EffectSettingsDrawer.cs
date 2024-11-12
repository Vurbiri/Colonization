namespace VurbiriEditor.Colonization
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using Vurbiri.Colonization;

    [CustomPropertyDrawer(typeof(EffectSettings))]
    public class EffectSettingsDrawer : ADrawerGetConstFieldName
    {
        #region Consts
        private const string P_TARGET_ACTOR = "_targetActor", P_TARGET_AB = "_targetAbility", P_TYPE_OP = "_typeOperation", P_VALUE = "_value", P_DUR = "_duration";
        private readonly string[] _nameOp = { "Addition", "Percent" };
        private readonly int[] _valueOp = { 0, 2 };
        #endregion

        public override void OnGUI(Rect position, SerializedProperty propertyMain, GUIContent label)
        {
            SerializedProperty property;
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginProperty(position, label, propertyMain);

            DrawId(P_TARGET_ACTOR, typeof(TargetOfEffectId));
            DrawId(P_TARGET_AB, typeof(ActorAbilityId));
            
            Space();
            DrawInt(P_VALUE);
            position.y += height;
            property = propertyMain.FindPropertyRelative(P_TYPE_OP);
            property.intValue = EditorGUI.IntPopup(position, property.displayName, property.intValue, _nameOp, _valueOp);

            Space();
            DrawIntSlider(P_DUR, 0, 5);

            EditorGUI.EndProperty();

            #region Local: Space(..), DrawIntSlider(..), DrawId(..), DrawInt(..), DrawString(..)
            //================================================================
            void Space(float ratio = 1f) => position.y += EditorGUIUtility.standardVerticalSpacing * ratio;
            //================================================================
            int DrawIntSlider(string nameProperty, int min, int max)
            {
                position.y += height;
                property = propertyMain.FindPropertyRelative(nameProperty);
                int value = Mathf.Clamp(property.intValue, min, max);
                property.intValue = value = EditorGUI.IntSlider(position, property.displayName, value, min, max);
                return value;
            }
            //================================================================
            int DrawId(string nameProperty, Type t_field)
            {
                position.y += height;
                var (names, values) = GetNamesAndValues(t_field);
                property = propertyMain.FindPropertyRelative(nameProperty);
                property.intValue = EditorGUI.IntPopup(position, property.displayName, property.intValue, names, values);
                return property.intValue;
            }
            //================================================================
            void DrawInt(string nameProperty)
            {
                position.y += height;
                property = propertyMain.FindPropertyRelative(nameProperty);
                property.intValue = EditorGUI.IntField(position, property.displayName, property.intValue);
            }
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 7.2f * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
    }
}
