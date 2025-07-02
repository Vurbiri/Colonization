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

            label = BeginProperty(position, label, mainProperty);
			{
                ObjectField(position, mainProperty.FindPropertyRelative(F_NAME), typeof(ParticleSystem), label);
            }
			EndProperty();
		}
	}
}