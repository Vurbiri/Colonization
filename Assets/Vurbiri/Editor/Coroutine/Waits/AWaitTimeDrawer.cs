using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
	[CustomPropertyDrawer(typeof(AWaitTime), true)]
	public class AWaitTimeDrawer : PropertyDrawer
	{
        private readonly string F_NAME = "_waitTime";

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			SerializedProperty timeProperty = mainProperty.FindPropertyRelative(F_NAME);
            MinMaxAttribute range = fieldInfo.GetCustomAttribute<MinMaxAttribute>();

            position.height = EditorGUIUtility.singleLineHeight;

		
			label = BeginProperty(position, label, mainProperty);
			{
				if (range != null)
					timeProperty.floatValue = Slider(position, label, timeProperty.floatValue, range.min, range.max);
				else
					timeProperty.floatValue = FloatField(position, label, timeProperty.floatValue);
            }
			EndProperty();
		}
	}
}
