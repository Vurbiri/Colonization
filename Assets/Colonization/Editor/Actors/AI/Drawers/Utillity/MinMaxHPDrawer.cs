using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
	[CustomPropertyDrawer(typeof(MinMaxHP))]
	public class MinMaxHPDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            var min = property.FindPropertyRelative("_min");
			var max = property.FindPropertyRelative("_max");
			if(min.intValue == max.intValue)
			{
				min.intValue = 0;
				max.intValue = 100;
            }

            label = BeginProperty(position, label, property);
			{
				VEditorGUI.MinMaxSlider(position, label, min, max, 0, 100);
            }
			EndProperty();
		}
	}
}