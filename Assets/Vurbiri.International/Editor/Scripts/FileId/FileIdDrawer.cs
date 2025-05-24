using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Vurbiri.International.Editor
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
                idProperty.intValue = IntPopup(position, label.text, idProperty.intValue, LanguageFiles.names, LanguageFiles.values, EditorStyles.popup);
            }
            EndProperty();
        }

		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}

    }
}
