using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
	[CustomPropertyDrawer(typeof(RefValue<>), true)]
	public class RefValueDrawer : PropertyDrawer
	{
		private const string P_NAME = "_value";
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			label = BeginProperty(position, label, mainProperty);
			{
				PropertyField(position, mainProperty.FindPropertyRelative(P_NAME), label);
			}
			EndProperty();
		}
	}
}
