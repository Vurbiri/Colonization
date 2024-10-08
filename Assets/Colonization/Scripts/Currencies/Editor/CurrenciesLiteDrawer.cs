using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(CurrenciesLite))]
    public class CurrenciesLiteDrawer : ADrawerGetConstFieldName
    {
        private const float Y_SPACE = 2f, BUTTON_RATE_POS = 1.1f, BUTTON_CLEAR_SIZE = 60f, BUTTON_PLUSMINUS_SIZE = 20f, LABEL_SIZE = 100f;
        private const string NAME_ARRAY = "_values", NAME_AMOUNT = "_amount", BUTTON_CLEAR = "Clear", BUTTON_PLUS = "+", BUTTON_MINUS = "-";
        private readonly Color[] colors = { new(0.72f, 0.6f, 0f, 1f), Color.yellow, Color.blue, Color.gray, Color.green, Color.red };

        private Rect _position;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            position.height = EditorGUIUtility.singleLineHeight;

            label = EditorGUI.BeginProperty(position, label, property);

            DrawLabelCount(position, property.FindPropertyRelative(NAME_AMOUNT).intValue);

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);
                int count = propertyValues.arraySize;
                List<string> names = GetNames();

                Color prevColor = GUI.color;
                SerializedProperty propertyElement;
                EditorGUI.indentLevel++;
                for (int i = 0; i < count; i++)
                {
                    GUI.color = colors[i];
                    propertyElement = propertyValues.GetArrayElementAtIndex(i);
                    position.y += position.height + Y_SPACE;
                    propertyElement.intValue = EditorGUI.IntSlider(position, names[i], propertyElement.intValue, 0, 25);
                }
                EditorGUI.indentLevel--;
                GUI.color = prevColor;
                
                if (!Application.isPlaying)
                {
                    _position = position;
                    _position.height += Y_SPACE;
                    _position.y += _position.height + Y_SPACE * 2f;

                    if (DrawButton(BUTTON_CLEAR, BUTTON_CLEAR_SIZE, BUTTON_CLEAR_SIZE))
                        Clear(propertyValues, count);
                    if (DrawButton(BUTTON_MINUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE))
                        Add(propertyValues, count, -1);
                    if (DrawButton(BUTTON_PLUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE * 2f))
                        Add(propertyValues, count, 1);
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
            void Clear(SerializedProperty prop, int count)
            {
                for (int index = 0; index < count; index++)
                {
                    prop.GetArrayElementAtIndex(index).intValue = 0;
                }
            }
            //=================================
            void Add(SerializedProperty prop, int count, int add)
            {
                for (int index = 0; index < count - 1; index++)
                {
                    if ((prop.GetArrayElementAtIndex(index).intValue += add) < 0)
                        prop.GetArrayElementAtIndex(index).intValue = 0;
                }
            }
            #endregion
        }

        protected override Type GetTypeId() => typeof(CurrencyId);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rate = 1f;

            if (property.isExpanded)
            {
                rate += CurrencyId.Count * rate;
                if (!Application.isPlaying)
                    rate += 1.35f;
            }

            return (EditorGUIUtility.singleLineHeight + Y_SPACE) * rate;
        }
    }
}
