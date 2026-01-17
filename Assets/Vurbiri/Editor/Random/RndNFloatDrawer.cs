using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(RndNFloat))]
    public class RndNFloatDrawer : ARValueDrawer
    {
        private const string NAME_VALUE = "_min";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            var minProperty = property.FindPropertyRelative(NAME_VALUE);
            var min = -Mathf.Abs(minProperty.floatValue);
            var range = fieldInfo.GetCustomAttribute<MaxAttribute>();

            if (min == 0f) min = -MIN;

            label = EditorGUI.BeginProperty(position, label, property);
            {
                if (range != null)
                {
                    min = EditorGUI.Slider(position, label, min, -Mathf.Abs(range.max), -MIN);
                }
                else
                {
                    var (labelSize, minLabelSize, minSize, maxLabelSize, maxSize) = CalkPosition(position);
                    EditorGUI.LabelField(labelSize, label);

                    EditorGUI.PrefixLabel(minLabelSize, s_minLabel);
                    min = EditorGUI.DelayedFloatField(minSize, min);

                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.PrefixLabel(maxLabelSize, s_maxLabel);
                    EditorGUI.FloatField(maxSize, 0f);
                    EditorGUI.EndDisabledGroup();


                }
            }
            EditorGUI.EndProperty();

            minProperty.floatValue = -Mathf.Abs(min);
        }
    }
}
