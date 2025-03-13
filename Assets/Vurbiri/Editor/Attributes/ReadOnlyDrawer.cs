//Assets\Vurbiri\Editor\Attributes\ReadOnlyDrawer.cs
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
			if (attribute is not ReadOnlyAttribute)
            {
                EditorGUILayout.PropertyField(mainProperty, label, true);
                return;
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(mainProperty, label, true);
            EditorGUI.EndDisabledGroup();
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float rate = 1f;


			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * rate;
		}
	}
}