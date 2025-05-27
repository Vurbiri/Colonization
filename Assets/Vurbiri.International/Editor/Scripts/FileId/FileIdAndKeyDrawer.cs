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

			Rect labelRect = GetLabelRect(position);
			Rect idRect = GetIdRect(position);
            Rect keyRect = GetKeyRect(position);

			label = BeginProperty(position, label, mainProperty);
			{
				PrefixLabel(labelRect, label);
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
            int index = Mathf.Max(0, Popup(position, System.Array.IndexOf(keys, keyProperty.stringValue), keys));
            keyProperty.stringValue = keys[index];
        }

        public static Rect GetLabelRect(Rect rect)
		{
			rect.width = EditorGUIUtility.labelWidth * 0.5f + 15f;
			return rect;
		}
        public static Rect GetIdRect(Rect rect)
        {
            float width = EditorGUIUtility.labelWidth * 0.5f + 15f;

            rect.width = EditorGUIUtility.labelWidth - width - 2f;
            rect.x += width + 1f;
            return rect;
        }
        public static Rect GetKeyRect(Rect rect)
        {
            rect.width -= EditorGUIUtility.labelWidth + 1f;
            rect.x += EditorGUIUtility.labelWidth + 1f;
            return rect;
        }

    }
}
