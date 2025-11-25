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

            var (labelRect, idRect, keyRect) = CalkPosition(position);

            label = BeginProperty(position, label, mainProperty);
			{
				LabelField(labelRect, label);
				idProperty.intValue = IntPopup(idRect, idProperty.intValue, LanguageData.fileNames, LanguageData.fileValues);
				KeyField(keyRect, keyProperty, idProperty.intValue);
			}
			EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}

        public static void KeyField(Rect position, SerializedProperty keyProperty, int fileId)
        {
            string[] keys = LanguageData.keys[fileId];
            int index = Popup(position, System.Array.IndexOf(keys, keyProperty.stringValue), keys);
            keyProperty.stringValue = keys[Mathf.Max(0, index)];
        }

        public static (Rect, Rect, Rect) CalkPosition(Rect position)
        {
            float offset = indentLevel * 15f;
            float fieldWidth = position.width - EditorGUIUtility.labelWidth;
            float idSize = fieldWidth * 0.31f;
            float keySize = fieldWidth - idSize - 2f;

            Rect labelRect = position, idRect = position, keyRect = position;

            labelRect.width = EditorGUIUtility.labelWidth - offset + 2f;
            idRect.width = idSize + offset;
            keyRect.width = keySize + offset;

            idRect.x += labelRect.width;
            keyRect.x = idRect.x + idSize + 2f;

            return (labelRect, idRect, keyRect);
        }

    }
}
