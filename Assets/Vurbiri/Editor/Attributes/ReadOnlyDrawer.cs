using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, mainProperty, label, true);
            EditorGUI.EndDisabledGroup();
		}

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
	}
}
