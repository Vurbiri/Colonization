using UnityEditor;
using UnityEngine;
using Vurbiri.International;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.International
{
	[CustomPropertyDrawer(typeof(FileId))]
	internal class FileIdDrawer : PropertyDrawer
	{
		private const string F_NAME = "_id";

		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			SerializedProperty idProperty = mainProperty.FindPropertyRelative(F_NAME);
			label = BeginProperty(position, label, mainProperty);
			{
				idProperty.intValue = IntPopup(position, label.text, idProperty.intValue, LanguageData.fileNames, LanguageData.fileValues, EditorStyles.popup);
			}
			EndProperty();
		}
	}
}
