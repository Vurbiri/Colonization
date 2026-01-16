using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(FloatZRnd))]
    public class FloatZRndDrawer : ARValueDrawer
    {
        private const string NAME_VALUE = "_value";
        private const float ZERO = 1E-5f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            float max, min;
            float value = valueProperty.floatValue;

            var range = fieldInfo.GetCustomAttribute<MaxAttribute>();

            label = EditorGUI.BeginProperty(position, label, property);

            if (range != null)
            {
                if(range.max >= 0) { max = range.max; min = ZERO; }
                else { min = range.max; max = -ZERO; }

                value = EditorGUI.Slider(position, label, value, min, max);
            }
            else
            {
                var (labelSize, minLabelSize, minSize, maxLabelSize, maxSize) = CalkPosition(position);
                EditorGUI.LabelField(labelSize, label);

                if (value >= 0f)
                {
                    min = 0f; max = value;
                }
                else
                {
                    min = value; max = 0f;
                }

                EditorGUI.PrefixLabel(minLabelSize, s_minLabel);
                EditorGUI.BeginChangeCheck();
                min = EditorGUI.DelayedFloatField(minSize, min);
                if (EditorGUI.EndChangeCheck())
                    value = min;

                EditorGUI.PrefixLabel(maxLabelSize, s_maxLabel);
                EditorGUI.BeginChangeCheck();
                max = EditorGUI.DelayedFloatField(maxSize, max);
                if (EditorGUI.EndChangeCheck())
                    value = max;
            }

            EditorGUI.EndProperty();

            valueProperty.floatValue = value;
        }
    }
}
