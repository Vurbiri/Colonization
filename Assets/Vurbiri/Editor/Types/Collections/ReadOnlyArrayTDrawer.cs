using UnityEditor;
using UnityEngine;
using Vurbiri.Collections;

namespace VurbiriEditor.Collections
{
	[CustomPropertyDrawer(typeof(ReadOnlyArray<>))]
	public class ReadOnlyArrayTDrawer : PropertyDrawer
	{
		private readonly string F_VALUES = "_values";
	
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(position, mainProperty.FindPropertyRelative(F_VALUES), label, true);
        }
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(mainProperty.FindPropertyRelative(F_VALUES));
        }
	}
}