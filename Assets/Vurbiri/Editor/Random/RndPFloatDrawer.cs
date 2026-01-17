using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
	[CustomPropertyDrawer(typeof(RndPFloat))]
	public class RndPFloatDrawer : ARValueDrawer
	{
		private const string NAME_VALUE = "_max";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			var maxProperty = property.FindPropertyRelative(NAME_VALUE);
			var max = Mathf.Abs(maxProperty.floatValue);
			var range = fieldInfo.GetCustomAttribute<MaxAttribute>();

            if (max == 0f) max = MIN;

            label = EditorGUI.BeginProperty(position, label, property);
			{
				if (range != null)
				{
					max = EditorGUI.Slider(position, label, max, MIN, Mathf.Abs(range.max));
				}
				else
				{
					var (labelSize, minLabelSize, minSize, maxLabelSize, maxSize) = CalkPosition(position);
					EditorGUI.LabelField(labelSize, label);

					EditorGUI.BeginDisabledGroup(true);
					EditorGUI.PrefixLabel(minLabelSize, s_minLabel);
					EditorGUI.FloatField(minSize, 0f);
					EditorGUI.EndDisabledGroup();

					EditorGUI.PrefixLabel(maxLabelSize, s_maxLabel);
					max = EditorGUI.DelayedFloatField(maxSize, max);
				}
			}
			EditorGUI.EndProperty();

			maxProperty.floatValue = Mathf.Abs(max);
		}
	}
}
