using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.International;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.International
{
	[CustomPropertyDrawer(typeof(KeyAttribute))]
	public class KeyAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			if (attribute is not KeyAttribute key)
			{
				PropertyField(position, mainProperty, label, true);
				return;
			}

			if (mainProperty.propertyType != SerializedPropertyType.String)
			{
				HelpBox(position, "The KeyAttribute is used only with a field of type “string”.", UnityEditor.MessageType.Error);
				return;
			}

			label = BeginProperty(position, label, mainProperty);
			{
				var keys = LanguageData.keys[key.fileId];
				int index = Popup(position, label.text, System.Array.IndexOf(keys, mainProperty.stringValue), keys);
				mainProperty.stringValue = keys[MathI.Max(0, index)];
			}
			EndProperty();
		}
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			if (attribute is KeyAttribute)
				return EditorGUIUtility.singleLineHeight;

			return EditorGUI.GetPropertyHeight(mainProperty, label);
		}
	}
}