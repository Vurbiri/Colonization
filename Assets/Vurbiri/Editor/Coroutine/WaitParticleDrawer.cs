using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
	[CustomPropertyDrawer(typeof(WaitParticle))]
	public class WaitParticleDrawer : PropertyDrawer
	{
		private readonly string F_NAME = "_particleSystem";
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;
			
			BeginProperty(position, label, mainProperty);
			{
				PropertyField(position, mainProperty.FindPropertyRelative(F_NAME));
			}
			EndProperty();
		}
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}
	}
}