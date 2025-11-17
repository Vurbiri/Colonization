using System;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    public static class VEditorGUI
    {
        private const float SIZE_VALUE = 50f, SIZE_SPACE = 6f;

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
            position.y += s_ySpace * SIZE_SPACE;
            return position;
        }
        #endregion

        #region MinMaxSlider
        public static bool MinMaxSlider(Rect position, GUIContent label, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit)
        {
            bool isValid = !Mathf.Approximately(minLimit, maxLimit) && minProperty.propertyType == maxProperty.propertyType && maxProperty.propertyType == SerializedPropertyType.Float;
            if (isValid)
            {
                if (minLimit > maxLimit) (minLimit, maxLimit) = (maxLimit, minLimit);
 
                float minValue = Mathf.Clamp(minProperty.floatValue, minLimit, maxLimit - 1E-06f);
                float maxValue = Mathf.Clamp(maxProperty.floatValue, minValue + 1E-06f, maxLimit);

                var (sizeLabel, sizeMin, sizeSlider, sizeMax) = CalkPositionSlider(position);

                EditorGUI.LabelField(sizeLabel, label);
                minValue = EditorGUI.DelayedFloatField(sizeMin, minValue);
                maxValue = EditorGUI.DelayedFloatField(sizeMax, maxValue);
                EditorGUI.MinMaxSlider(sizeSlider, ref minValue, ref maxValue, minLimit, maxLimit);

                minValue = Math.Max(minValue, minLimit);
                maxValue = Math.Min(maxValue, maxLimit);

                minProperty.floatValue = Math.Clamp(minValue, minLimit, maxValue - 1E-06f);
                maxProperty.floatValue = Math.Clamp(maxValue, minValue + 1E-06f, maxLimit);
            }
            return isValid;
        }

        public static bool MinMaxSlider(Rect position, GUIContent label, SerializedProperty minProperty, SerializedProperty maxProperty, int minLimit, int maxLimit)
        {
            bool isValid = minLimit != maxLimit && minProperty.propertyType == SerializedPropertyType.Integer && maxProperty.propertyType == SerializedPropertyType.Integer;

            if (isValid)
            {
                if (minLimit > maxLimit) (minLimit, maxLimit) = (maxLimit, minLimit);

                int minI = minProperty.intValue;
                int maxI = maxProperty.intValue;

                float minF = Mathf.Clamp(minI, minLimit, maxLimit - 1);
                float maxF = Mathf.Clamp(maxI, minI + 1, maxLimit);

                var (sizeLabel, sizeMin, sizeSlider, sizeMax) = CalkPositionSlider(position);

                EditorGUI.LabelField(sizeLabel, label);
                minF = EditorGUI.DelayedFloatField(sizeMin, minF);
                maxF = EditorGUI.DelayedFloatField(sizeMax, maxF);
                EditorGUI.MinMaxSlider(sizeSlider, ref minF, ref maxF, minLimit, maxLimit);

                minI = Math.Max(MathI.Round(minF), minLimit);
                maxI = Math.Min(MathI.Round(maxF), maxLimit);

                minProperty.intValue = Math.Clamp(minI, minLimit, maxI - 1);
                maxProperty.intValue = Math.Clamp(maxI, minI + 1, maxLimit);
            }

            return isValid;
        }
        public static void MinMaxSlider(Rect position, ref int minValue, ref int maxValue, int minLimit, int maxLimit)
        {
            var (sizeMin, sizeSlider, sizeMax) = CalkPositionSliderNotLabel(position);

            minValue = Math.Clamp(minValue, minLimit, maxLimit - 1);
            maxValue = Math.Clamp(maxValue, minValue + 1, maxLimit);

            float min = EditorGUI.IntField(sizeMin, minValue);
            float max = EditorGUI.IntField(sizeMax, maxValue);
            EditorGUI.MinMaxSlider(sizeSlider, ref min, ref max, minLimit, maxLimit);

            minValue = Math.Max(MathI.Round(min), minLimit);
            maxValue = Math.Min(MathI.Round(max), maxLimit);

            minValue = Math.Clamp(minValue, minLimit, maxValue - 1);
            maxValue = Math.Clamp(maxValue, minValue + 1, maxLimit);
        }

        public static (Rect, Rect, Rect, Rect) CalkPositionSlider(Rect position)
        {
            float indentLevel = EditorGUI.indentLevel;
            float offset = indentLevel * 15f;
            float size  = SIZE_VALUE * (1f + indentLevel * 0.3f);
            float sliderWidth = position.width - EditorGUIUtility.labelWidth - (size + SIZE_SPACE - offset) * 2f;
            
            Rect sizeLabel = position, sizeMin = position, sizeMax = position, sizeSlider = position;

            sizeLabel.width = EditorGUIUtility.labelWidth - offset;
            sizeMin.width = sizeMax.width = size;
            sizeSlider.width = sliderWidth * (1f + indentLevel * 0.068f);

            sizeMin.x += sizeLabel.width;
            sizeSlider.x = sizeMin.x + (size + SIZE_SPACE) - offset;
            sizeMax.x = sizeSlider.x + sliderWidth + SIZE_SPACE;
            
            return (sizeLabel, sizeMin, sizeSlider, sizeMax);
        }

        private static (Rect, Rect, Rect) CalkPositionSliderNotLabel(Rect position)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            position.x = SIZE_SPACE; position.width = EditorGUIUtility.currentViewWidth - SIZE_SPACE * 2f;

            Rect sizeMin = position, sizeMax = position, sizeSlider = position;

            sizeSlider.x += SIZE_VALUE + SIZE_SPACE;
            sizeMax.x += position.width - SIZE_VALUE;

            sizeMin.width = sizeMax.width = SIZE_VALUE;
            sizeSlider.width = position.width - (SIZE_VALUE + SIZE_SPACE) * 2f;

            return (sizeMin, sizeSlider, sizeMax);
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
