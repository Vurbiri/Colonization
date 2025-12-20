using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
	[CustomPropertyDrawer(typeof(RefFloat))]
	public class RefFloatDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			var property = mainProperty.FindPropertyRelative(nameof(RefFloat.value));
            var range = fieldInfo.GetCustomAttribute<MinMaxAttribute>();

            position.height = EditorGUIUtility.singleLineHeight;
            label = BeginProperty(position, label, mainProperty);
			{
                if (range != null)
                    property.floatValue = Slider(position, label, property.floatValue, range.min, range.max);
                else
                    property.floatValue = FloatField(position, label, property.floatValue);
            }
			EndProperty();
		}
	}
}