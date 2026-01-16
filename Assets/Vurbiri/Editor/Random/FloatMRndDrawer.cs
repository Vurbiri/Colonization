using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(FloatMRnd))]
    public class FloatMRndDrawer : ARValueDrawer
    {
        private readonly string NAME_VALUE = "_value";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            float max = Mathf.Abs(valueProperty.floatValue), min = -max;

            var range = fieldInfo.GetCustomAttribute<MaxAttribute>();

            label = EditorGUI.BeginProperty(position, label, property);

            if (range != null)
            {
                var (sizeLabel, sizeMin, sizeSlider, sizeMax) = VEditorGUI.CalkPositionSlider(position);

                EditorGUI.LabelField(sizeLabel, label);

                min = EditorGUI.DelayedFloatField(sizeMin, min);
                max = EditorGUI.DelayedFloatField(sizeMax, max);
                EditorGUI.MinMaxSlider(sizeSlider, ref min, ref max, -range.max, range.max);
            }
            else
            {
                var (labelSize, minLabelSize, minSize, maxLabelSize, maxSize) = CalkPosition(position);
                EditorGUI.LabelField(labelSize, label);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PrefixLabel(minLabelSize, s_minLabel);
                EditorGUI.FloatField(minSize, min);
                EditorGUI.EndDisabledGroup();

                EditorGUI.PrefixLabel(maxLabelSize, s_maxLabel);
                max = EditorGUI.DelayedFloatField(maxSize, max);
            }

            EditorGUI.EndProperty();

            valueProperty.floatValue = Mathf.Abs(max);
        }
    }
}
