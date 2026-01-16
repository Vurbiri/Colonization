using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(IntRnd))]
    public class IntRndDrawer : ARValueDrawer
    {
        private const string NAME_MIN = "_min", NAME_MAX = "_max";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            var minProperty = property.FindPropertyRelative(NAME_MIN);
            var maxProperty = property.FindPropertyRelative(NAME_MAX);

            var range = fieldInfo.GetCustomAttribute<MinMaxAttribute>();

            label = EditorGUI.BeginProperty(position, label, property);

            if (range != null)
            {
                var (sizeLabel, sizeMin, sizeSlider, sizeMax) = VEditorGUI.CalkPositionSlider(position);
                float min, max, rMin = Mathf.Round(range.min), rMax = Mathf.Round(range.max);
                min = Mathf.Clamp(minProperty.intValue, rMin, rMax);
                max = Mathf.Clamp(maxProperty.intValue - 1, rMin, rMax);

                EditorGUI.LabelField(sizeLabel, label);

                min = EditorGUI.DelayedFloatField(sizeMin, min);
                max = EditorGUI.DelayedFloatField(sizeMax, max);

                EditorGUI.MinMaxSlider(sizeSlider, ref min, ref max, rMin, rMax);

                if (min > max) (min, max) = (max, min);
                minProperty.intValue = MathI.Round(min); 
                maxProperty.intValue = MathI.Round(max) + 1;
            }
            else
            {
                int min = minProperty.intValue, max = maxProperty.intValue - 1;

                var (labelSize, minLabelSize, minSize, maxLabelSize, maxSize) = CalkPosition(position);
                EditorGUI.LabelField(labelSize, label);

                EditorGUI.PrefixLabel(minLabelSize, s_minLabel);
                min = EditorGUI.DelayedIntField(minSize, min);

                EditorGUI.PrefixLabel(maxLabelSize, s_maxLabel);
                max = EditorGUI.DelayedIntField(maxSize, max);

                if (min > max) (min, max) = (max, min);
                minProperty.intValue = min;
                maxProperty.intValue = max + 1;
            }

            EditorGUI.EndProperty();
        }
    }
}
