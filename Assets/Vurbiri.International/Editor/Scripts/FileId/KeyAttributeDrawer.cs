using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Vurbiri.International.Editor
{
	[CustomPropertyDrawer(typeof(KeyAttribute))]
	public class KeyAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
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
			
			position.height = EditorGUIUtility.singleLineHeight;
			
			label = BeginProperty(position, label, mainProperty);
			{
                string[] keys = LanguageData.keys[key.fileId];
				int index = Popup(position, label.text, System.Array.IndexOf(keys, mainProperty.stringValue), keys);
                mainProperty.stringValue = keys[Mathf.Max(0, index)];
            }
			EndProperty();
		}
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
            if (attribute is KeyAttribute)
	            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			return EditorGUI.GetPropertyHeight(mainProperty, label);

        }
	}
}