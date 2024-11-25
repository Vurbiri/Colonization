//Assets\Colonization\Editor\Diplomacy\DiplomacySettingsDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(DiplomacySettings))]
	public class DiplomacySettingsDrawer : PropertyDrawer
	{
	
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginProperty(position, label, property);

            while (property.Next(true))
            {
                EditorGUI.PropertyField(position, property, new GUIContent(property.displayName));
                position.y += height;
            }

            EditorGUI.EndProperty();
		}

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * property.CountInProperty();
        }
    }
}