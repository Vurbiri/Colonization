using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization.UI
{
	[CustomEditor(typeof(ColorSlider))]
	public class ColorSliderEditor : Editor
	{
		private readonly GUIContent[] _componentNames = { new("Red"), new("Green"), new("Blue") };
		private readonly GUIContent _componentName = new("Component");
		private readonly GUIContent[] _axisNames = { new("Horizontal"), new("Vertical") };
		private readonly GUIContent _axisName = new("Axis");

		private SerializedProperty _componentProperty;
		private SerializedProperty _axisProperty;
		private SerializedProperty _handleProperty;

		private void OnEnable()
		{
			_componentProperty = serializedObject.FindProperty(ColorSlider.componentField);
			_axisProperty      = serializedObject.FindProperty(ColorSlider.axisField);
			_handleProperty    = serializedObject.FindProperty(ColorSlider.handleField);
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			_componentProperty.intValue = Popup(_componentName, _componentProperty.intValue, _componentNames);
			_axisProperty.intValue = Popup(_axisName, _axisProperty.intValue, _axisNames);
			Space();
			PropertyField(_handleProperty);

			serializedObject.ApplyModifiedProperties();
		}
	}
}