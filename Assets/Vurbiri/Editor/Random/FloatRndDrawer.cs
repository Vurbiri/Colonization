using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
	[CustomPropertyDrawer(typeof(FloatRnd))]
	public class FloatRndDrawer : ARValueDrawer
	{
		private const string NAME_MIN = "_min", NAME_MAX = "_max";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty minProperty = property.FindPropertyRelative(NAME_MIN);
            SerializedProperty maxProperty = property.FindPropertyRelative(NAME_MAX);

            float min = minProperty.floatValue, max = maxProperty.floatValue;
            if (min > max) (min, max) = (max, min);

            label = EditorGUI.BeginProperty(position, label, property);
            {
                var (labelSize, minLabelSize, minSize, maxLabelSize, maxSize) = CalkPosition(position);
                EditorGUI.LabelField(labelSize, label);

                EditorGUI.PrefixLabel(minLabelSize, s_minLabel);
                minProperty.floatValue = EditorGUI.DelayedFloatField(minSize, min);

                EditorGUI.PrefixLabel(maxLabelSize, s_maxLabel);
                maxProperty.floatValue = EditorGUI.DelayedFloatField(maxSize, max);
            }
            EditorGUI.EndProperty();
        }
    }
}

