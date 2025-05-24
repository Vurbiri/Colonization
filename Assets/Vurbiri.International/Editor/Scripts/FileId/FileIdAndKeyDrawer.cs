using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Vurbiri.International.Editor
{
	[CustomPropertyDrawer(typeof(FileIdAndKey))]
	public class FileIdAndKeyDrawer : PropertyDrawer
	{
        private const string F_ID = "id", F_KEY = "key";

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty idProperty = mainProperty.FindPropertyRelative(F_ID);
            SerializedProperty keyProperty = mainProperty.FindPropertyRelative(F_KEY);

            Rect labelRect = position;
			labelRect.width = EditorGUIUtility.labelWidth * 0.5f + 15f;
            Rect idRect = position;
            idRect.width = EditorGUIUtility.labelWidth - labelRect.width - 2f;
			idRect.x += labelRect.width + 1f;
            
			position.width -= EditorGUIUtility.labelWidth + 1f;
            position.x += EditorGUIUtility.labelWidth + 1f;

            label = BeginProperty(position, label, mainProperty);
			{
				PrefixLabel(labelRect, label);
                idProperty.intValue = IntPopup(idRect, idProperty.intValue, LanguageFiles.names, LanguageFiles.values/*, EditorStyles.popup*/);
                keyProperty.stringValue = TextField(position, keyProperty.stringValue);
            }
			EndProperty();
		}
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
	}
}
