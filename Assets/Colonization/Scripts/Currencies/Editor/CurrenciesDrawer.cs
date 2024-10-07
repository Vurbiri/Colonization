using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using Vurbiri.Colonization.UI;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(Currencies))]
    public class CurrenciesDrawer : ADrawerGetConstFieldName
    {
        private const float Y_SPACE = 2f, BUTTON_RATE_POS = 1.1f, BUTTON_CLEAR_SIZE = 60f, BUTTON_PLUSMINUS_SIZE = 20f, LABEL_SIZE = 100f;
        private const string NAME_VALUE = "_value", NAME_ARRAY = "_values", NAME_AMOUNT = "_amount", BUTTON_CLEAR = "Clear", BUTTON_PLUS = "+", BUTTON_MINUS = "-";
        private const string PATH = "Assets/Colonization/Settings/UI/CurrenciesIcons.asset";

        private Rect _position;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            position.height = EditorGUIUtility.singleLineHeight;

            label = EditorGUI.BeginProperty(position, label, property);

            DrawLabelCount(position, property.FindPropertyRelative(NAME_AMOUNT).intValue);

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                CurrenciesIconsScriptable icons = AssetDatabase.LoadAssetAtPath<CurrenciesIconsScriptable>(PATH);
                SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);
                int _count = propertyValues.arraySize;
                List<SerializedProperty> _values = new List<SerializedProperty>(_count);
                Color prevColor = GUI.color;
                List<string> names = GetNames();

                EditorGUI.indentLevel++;
                for (int i = 0; i < _count; i++)
                {
                    GUI.color = i!= 3 ? icons[i].Color : Color.gray;
                    position = DrawField(position, propertyValues.GetArrayElementAtIndex(i), names[i]);
                }
                EditorGUI.indentLevel--;
                GUI.color = prevColor;

                if (!Application.isPlaying)
                {
                    _position = position;
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
            Rect DrawField(Rect position, SerializedProperty prop, string name)
            {
                position.y += position.height + Y_SPACE;
                SerializedProperty valueProperty = prop.FindPropertyRelative(NAME_VALUE);
                valueProperty.intValue = EditorGUI.IntSlider(position, name, valueProperty.intValue, 0, 20);
                return position;
            }
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
                {
                    propertyValues.GetArrayElementAtIndex(index).intValue = 0;
                }
            }
            //=================================
            void Add(int add)
            {
                for (int index = 0; index < _count; index++)
                {
                    if ((propertyValues.GetArrayElementAtIndex(index).intValue += add) < 0)
                        propertyValues.GetArrayElementAtIndex(index).intValue = 0;
                }
            }
            #endregion
        }

        protected override Type GetTypeId() => typeof(CurrencyId);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rate = 1.01f;

            if (property.isExpanded)
            {
                rate += CurrencyId.Count * rate;
                rate += 1.35f;
            }

            return (EditorGUIUtility.singleLineHeight + Y_SPACE) * rate;
        }
    }
}
