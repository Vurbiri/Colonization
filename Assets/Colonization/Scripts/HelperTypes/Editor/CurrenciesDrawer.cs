using System;
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(Currencies))]
    public class CurrenciesDrawer : EnumArrayDrawer
    {
        private const float BUTTON_RATE_POS = 1.1f, BUTTON_CLEAR_SIZE = 60f, BUTTON_PLUSMINUS_SIZE = 20f, LABEL_SIZE = 100f;
        private const string NAME_AMOUNT = "_amount", BUTTON_CLEAR = "Clear", BUTTON_PLUS = "+", BUTTON_MINUS = "-";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            base.OnGUI(position, property, label);

            DrawLabelCount(position, property.FindPropertyRelative(NAME_AMOUNT).intValue);

            _position.height += Y_SPACE;
            _position.y += _position.height + Y_SPACE;


            if (property.isExpanded)
            {
                if (!Application.isPlaying)
                {
                    if (DrawButton(BUTTON_CLEAR, BUTTON_CLEAR_SIZE, BUTTON_CLEAR_SIZE))
                        Clear();
                    if (DrawButton(BUTTON_MINUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE))
                        Add(-1);
                    if (DrawButton(BUTTON_PLUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE * 2f))
                        Add(1);
                }
            }

            EditorGUI.EndProperty();

            #region Local: DrawLabelCount(), DrawButton(...), Clear(...), Add(...)
            //=================================
            void DrawLabelCount(Rect pos, int value)
            {
                GUIStyle style = new(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleRight
                };

                pos.height = EditorGUIUtility.singleLineHeight;
                pos.x = EditorGUIUtility.currentViewWidth - LABEL_SIZE - 10f;
                pos.width = LABEL_SIZE;

                EditorGUI.LabelField(pos, $"Amount: {value}", style);

            }
            //=================================
            bool DrawButton(string text, float size, float offset)
            {
                Rect positionButton = _position;
                positionButton.x = EditorGUIUtility.currentViewWidth - offset * BUTTON_RATE_POS;
                positionButton.width = size;

                return GUI.Button(positionButton, text.ToUpper());
            }
            //=================================
            void Clear()
            {
                for (int index = 0; index < _count; index++)
                    _propertyValues.GetArrayElementAtIndex(index).intValue = 0;
            }
            //=================================
            void Add(int add)
            {
                for (int index = 0; index < _count; index++)
                {
                    if ((_propertyValues.GetArrayElementAtIndex(index).intValue += add) < 0)
                        _propertyValues.GetArrayElementAtIndex(index).intValue = 0;
                }
            }
            #endregion
        }

        protected override Type GetTypeEnum() => typeof(CurrencyType);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rate = 0;

            if (property.isExpanded)
                if (!Application.isPlaying)
                    rate += 1.35f;

            return base.GetPropertyHeight(property, label) + (EditorGUIUtility.singleLineHeight + Y_SPACE) * rate;
        }
    }
}
