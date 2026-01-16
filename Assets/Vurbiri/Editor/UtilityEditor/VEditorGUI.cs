using System;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    public static class VEditorGUI
    {
        private const float VALUE_SIZE = 50f, SPACE_SIZE = 6f;
        private const float MIN = 1E-5f;

        private static readonly float s_height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        private static readonly float s_ySpace = EditorGUIUtility.standardVerticalSpacing;

        #region DrawLine
        public static Rect DrawLine(Rect position) => DrawLine(position, Color.gray, 0f);
        public static Rect DrawLine(Rect position, float leftOffset) => DrawLine(position, Color.gray, leftOffset);
        public static Rect DrawLine(Rect position, Color color, float leftOffset = 0f)
        {
            Rect size = position;
            size.y += s_height + s_ySpace * 2f; size.x += leftOffset;
            size.width -= leftOffset; size.height = s_ySpace;
            EditorGUI.DrawRect(size, color);
            position.y += s_ySpace * SPACE_SIZE;
            return position;
        }
        #endregion

        #region MinMaxSlider
        public static bool MinMaxSlider(Rect position, GUIContent label, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit)
        {
            bool isValid = minLimit < maxLimit && minProperty.propertyType == maxProperty.propertyType && maxProperty.propertyType == SerializedPropertyType.Float;
            if (isValid)
            {
                float minValue = Mathf.Clamp(minProperty.floatValue, minLimit, maxLimit - MIN);
                float maxValue = Mathf.Clamp(maxProperty.floatValue, minValue + MIN, maxLimit);

                var (labelSize, minSize, sliderSize, maxSize) = CalkPositionSlider(position);

                EditorGUI.LabelField(labelSize, label);
                minValue = EditorGUI.DelayedFloatField(minSize, minValue);
                maxValue = EditorGUI.DelayedFloatField(maxSize, maxValue);
                EditorGUI.MinMaxSlider(sliderSize, ref minValue, ref maxValue, minLimit, maxLimit);

                minValue = Math.Max(minValue, minLimit);
                maxValue = Math.Min(maxValue, maxLimit);

                minProperty.floatValue = Math.Clamp(minValue, minLimit, maxValue - MIN);
                maxProperty.floatValue = Math.Clamp(maxValue, minValue + MIN, maxLimit);
            }
            return isValid;
        }

        public static bool MinMaxSlider(Rect position, GUIContent label, SerializedProperty minProperty, SerializedProperty maxProperty, int minLimit, int maxLimit)
        {
            bool isValid = minLimit < maxLimit && minProperty.propertyType == SerializedPropertyType.Integer && maxProperty.propertyType == SerializedPropertyType.Integer;

            if (isValid)
            {
                int minI = minProperty.intValue;
                int maxI = maxProperty.intValue;

                float minF = Mathf.Clamp(minI, minLimit, maxLimit - 1);
                float maxF = Mathf.Clamp(maxI, minI + 1, maxLimit);

                var (labelSize, minSize, sliderSize, maxSize) = CalkPositionSlider(position);

                EditorGUI.LabelField(labelSize, label);
                minF = EditorGUI.DelayedFloatField(minSize, minF);
                maxF = EditorGUI.DelayedFloatField(maxSize, maxF);
                EditorGUI.MinMaxSlider(sliderSize, ref minF, ref maxF, minLimit, maxLimit);

                minI = Math.Max(MathI.Round(minF), minLimit);
                maxI = Math.Min(MathI.Round(maxF), maxLimit);

                minProperty.intValue = Math.Clamp(minI, minLimit, maxI - 1);
                maxProperty.intValue = Math.Clamp(maxI, minI + 1, maxLimit);
            }

            return isValid;
        }
        public static void MinMaxSlider(Rect position, ref int minValue, ref int maxValue, int minLimit, int maxLimit)
        {
            var (minSize, sliderSize, maxSize) = CalkPositionSliderNotLabel(position);

            minValue = Math.Clamp(minValue, minLimit, maxLimit - 1);
            maxValue = Math.Clamp(maxValue, minValue + 1, maxLimit);

            float min = EditorGUI.DelayedIntField(minSize, minValue);
            float max = EditorGUI.DelayedIntField(maxSize, maxValue);
            EditorGUI.MinMaxSlider(sliderSize, ref min, ref max, minLimit, maxLimit);

            minValue = Math.Max(MathI.Round(min), minLimit);
            maxValue = Math.Min(MathI.Round(max), maxLimit);

            minValue = Math.Clamp(minValue, minLimit, maxValue - 1);
            maxValue = Math.Clamp(maxValue, minValue + 1, maxLimit);
        }

        public static (Rect, Rect, Rect, Rect) CalkPositionSlider(Rect position)
        {
            float offset = EditorGUI.indentLevel * 15f;
            float sliderWidth = position.width - EditorGUIUtility.labelWidth - (VALUE_SIZE + SPACE_SIZE) * 2f;
            
            Rect labelSize = position, minSize = position, maxSize = position, sliderSize = position;

            labelSize.width = EditorGUIUtility.labelWidth - offset;
            minSize.width = maxSize.width = VALUE_SIZE + offset;
            sliderSize.width = sliderWidth + offset;

            minSize.x += labelSize.width;
            sliderSize.x = minSize.x + (VALUE_SIZE + SPACE_SIZE);
            maxSize.x = sliderSize.x + sliderWidth + SPACE_SIZE;
            
            return (labelSize, minSize, sliderSize, maxSize);
        }

        private static (Rect, Rect, Rect) CalkPositionSliderNotLabel(Rect position)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            position.x = SPACE_SIZE; position.width = EditorGUIUtility.currentViewWidth - SPACE_SIZE * 2f;

            Rect minSize = position, maxSize = position, sliderSize = position;

            sliderSize.x += VALUE_SIZE + SPACE_SIZE;
            maxSize.x += position.width - VALUE_SIZE;

            minSize.width = maxSize.width = VALUE_SIZE;
            sliderSize.width = position.width - (VALUE_SIZE + SPACE_SIZE) * 2f;

            return (minSize, sliderSize, maxSize);
        }
        #endregion

        public static Rect CustomPropertyField(Rect position, SerializedProperty property, string name)
        {
            EditorGUI.PropertyField(position, property, new GUIContent(name));
            position.y += EditorGUI.GetPropertyHeight(property) + s_ySpace;
            return position;
        }
        public static Rect DefaultPropertyField(Rect position, SerializedProperty property, string name)
        {
            EditorGUI.PropertyField(position, property, new GUIContent(name));

            if (property.hasVisibleChildren)
            {
                position.y += s_height;
                int count = property.Copy().CountInProperty() - 1;

                EditorGUI.indentLevel++;
                while (property.NextVisible(true) && count > 0)
                {
                    count--;
                    EditorGUI.PropertyField(position, property, new GUIContent(property.displayName));

                    if (property.hasVisibleChildren)
                        position.y += s_height;
                    else
                        position.y += EditorGUI.GetPropertyHeight(property) + s_ySpace;

                }
                EditorGUI.indentLevel--;
            }
            else
            {
                position.y += EditorGUI.GetPropertyHeight(property) + s_ySpace;
            }
            return position;

        }
    }
}
