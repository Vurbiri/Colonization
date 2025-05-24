using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public static class VEditorGUI
    {
        private const float OFFSET_SIZE_LABEL = 20f, SIZE_VALUE = 50f, SIZE_SPACE = 5f;

        public static bool MinMaxSlider(Rect position, GUIContent label, SerializedProperty minProperty, SerializedProperty maxProperty, float min, float max)
        {
            if (minProperty.propertyType != SerializedPropertyType.Float) return false;
            if (Mathf.Approximately(min, max)) return false;

            if (min > max) (min, max) = (max, min);

            float minValue = Mathf.Clamp(minProperty.floatValue, min, max);
            float maxValue = Mathf.Clamp(maxProperty.floatValue, min, max);

            var (sizeLabel, sizeMin, sizeSlider, sizeMax) = CalkPositionSlider(position);

            EditorGUI.LabelField(sizeLabel, label);
            minValue = EditorGUI.FloatField(sizeMin, minValue);
            maxValue = EditorGUI.FloatField(sizeMax, maxValue);
            EditorGUI.MinMaxSlider(sizeSlider, ref minValue, ref maxValue, min, max);

            if (Mathf.Approximately(minValue, maxValue)) maxValue *= 0.1f;

            if (minValue > maxValue) (minValue, maxValue) = (maxValue, minValue);

            minProperty.floatValue = minValue;
            maxProperty.floatValue = maxValue;

            return true;
        }

        public static bool MinMaxSlider(Rect position, GUIContent label, SerializedProperty minProperty, SerializedProperty maxProperty, int min, int max)
        {
            if (minProperty.propertyType != SerializedPropertyType.Integer) return false;
            if (min == max) return false;

            if (min > max) (min, max) = (max, min);

            float minValue = Mathf.Clamp(minProperty.intValue, min, max);
            float maxValue = Mathf.Clamp(maxProperty.intValue, min, max);

            var (sizeLabel, sizeMin, sizeSlider, sizeMax) = CalkPositionSlider(position);

            EditorGUI.LabelField(sizeLabel, label);
            minValue = EditorGUI.FloatField(sizeMin, minValue);
            maxValue = EditorGUI.FloatField(sizeMax, maxValue);
            EditorGUI.MinMaxSlider(sizeSlider, ref minValue, ref maxValue, min, max);

            if (Mathf.Approximately(minValue, maxValue)) maxValue += 1f;

            if (minValue > maxValue) (minValue, maxValue) = (maxValue, minValue);

            minProperty.intValue = Mathf.RoundToInt(minValue);
            maxProperty.intValue = Mathf.RoundToInt(maxValue);

            return true;
        }


        private static (Rect, Rect, Rect, Rect) CalkPositionSlider(Rect position)
        {
            Rect sizeLabel = position, sizeMin = position, sizeMax = position, sizeSlider = position;
            sizeLabel.width = EditorGUIUtility.labelWidth + OFFSET_SIZE_LABEL;
            sizeMin.x = sizeLabel.width;
            sizeSlider.x = sizeLabel.width + SIZE_VALUE + SIZE_SPACE;
            sizeMax.x = EditorGUIUtility.currentViewWidth - SIZE_VALUE;

            sizeMin.width = sizeMax.width = SIZE_VALUE;
            sizeSlider.width = EditorGUIUtility.currentViewWidth - SIZE_VALUE * 2f - sizeLabel.width - SIZE_SPACE * 2f;

            return (sizeLabel, sizeMin, sizeSlider, sizeMax);
        }
    }
}
