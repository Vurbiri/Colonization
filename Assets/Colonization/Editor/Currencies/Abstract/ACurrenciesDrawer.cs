using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
	public abstract class ACurrenciesDrawer : PropertyDrawer
    {
        private const string NAME_ARRAY = "_values", NAME_AMOUNT = "_amount";

        private const int MAX_VALUE = 15;
        private const float BUTTON_RATE_POS = 1.1f, BUTTON_OFFSET = 10f, BUTTON_CLEAR_SIZE = 60f, BUTTON_PLUSMINUS_SIZE = 20f;
        private const float LABEL_SIZE = 100f, LABEL_OFFSET = 15f;
        private const string BUTTON_CLEAR = "Clear", BUTTON_PLUS = "+", BUTTON_MINUS = "-";

        private static readonly Color[] colors = { new(0.77f, 0.6f, 0f, 1f), Color.yellow, new(0f, 0.8f, 0f, 1f), Color.gray, new(0.33f, 0.48f, 1f, 1f), Color.red };

        private readonly float _ySpace = EditorGUIUtility.standardVerticalSpacing, _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        protected void OnGUI(Rect position, SerializedProperty property, GUIContent label, int count)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            label = EditorGUI.BeginProperty(position, label, property);

            SerializedProperty propertyAmount = property.FindPropertyRelative(NAME_AMOUNT);
            DrawLabelCount(position, propertyAmount.intValue);

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY), propertyElement;
                
                propertyValues.arraySize = count;
                propertyAmount.intValue = 0;

                Color prevColor = GUI.color;
                
                EditorGUI.indentLevel++;
                for (int index = 0; index < count; index++)
                {
                    GUI.color = colors[index];
                    propertyElement = propertyValues.GetArrayElementAtIndex(index);
                    position.y += _height;
                    propertyElement.intValue = EditorGUI.IntSlider(position, CurrencyId.Names_Ed[index], propertyElement.intValue, 0, MAX_VALUE);

                    if (index != CurrencyId.Blood)
                        propertyAmount.intValue += propertyElement.intValue;
                }
                EditorGUI.indentLevel--;
                GUI.color = prevColor;

                if (!Application.isPlaying)
                {
                    position.height += _ySpace;
                    position.y += position.height + _ySpace * 2f;

                    if (DrawButton(position, BUTTON_CLEAR, BUTTON_CLEAR_SIZE, BUTTON_CLEAR_SIZE + BUTTON_OFFSET))
                        Clear(propertyValues, propertyAmount, count);
                    if (DrawButton(position, BUTTON_MINUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE + BUTTON_OFFSET))
                        Add(propertyValues, propertyAmount, count, -1);
                    if (DrawButton(position, BUTTON_PLUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE * 2f + BUTTON_OFFSET))
                        Add(propertyValues, propertyAmount, count, 1);
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
                pos.x = EditorGUIUtility.currentViewWidth - LABEL_SIZE - LABEL_OFFSET;
                pos.width = LABEL_SIZE;

                EditorGUI.LabelField(pos, $"Amount: {value}", style);

            }
            //=================================
            bool DrawButton(Rect positionButton, string text, float size, float offset)
            {
                positionButton.x = EditorGUIUtility.currentViewWidth - offset * BUTTON_RATE_POS;
                positionButton.width = size;

                return GUI.Button(positionButton, text.ToUpper());
            }
            //=================================
            void Clear(SerializedProperty values, SerializedProperty amount, int count)
            {
                for (int index = 0; index < count; index++)
                    values.GetArrayElementAtIndex(index).intValue = 0;

                amount.intValue = 0;
            }
            //=================================
            void Add(SerializedProperty values, SerializedProperty amount, int count, int add)
            {
                SerializedProperty propertyElement;

                amount.intValue = 0;
                for (int index = 0; index < count; index++)
                {
                    propertyElement = values.GetArrayElementAtIndex(index);
                    propertyElement.intValue = Mathf.Clamp(propertyElement.intValue + add, 0, MAX_VALUE);

                    if (index != CurrencyId.Blood)
                        amount.intValue += propertyElement.intValue;
                }
            }
            #endregion
        }

        protected float GetPropertyHeight(bool isExpanded, int count)
        {
            float rate = 1f;

            if (isExpanded)
            {
                rate += count;
                if (!Application.isPlaying)
                    rate += 1.35f;
            }

            return _height * rate;
        }
    }
}
