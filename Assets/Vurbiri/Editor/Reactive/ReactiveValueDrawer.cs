using UnityEditor;
using UnityEngine;
using Vurbiri.Reactive;

namespace VurbiriEditor.Reactive
{
    [CustomPropertyDrawer(typeof(ReactiveValue<>), true)]
	public class ReactiveValueDrawer : PropertyDrawer
	{
		private readonly string P_NAME = "_value";
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            EditorGUI.PropertyField(position, mainProperty.FindPropertyRelative(P_NAME), label);
        }
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property.FindPropertyRelative(P_NAME), label);
        }
	}
}
