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
			Rect idRect = GetIdRect(position, labelRect);
            Rect keyRect = GetKeyRect(position, labelRect, idRect);

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
            int index = Popup(position, System.Array.IndexOf(keys, keyProperty.stringValue), keys);
            keyProperty.stringValue = keys[Mathf.Max(0, index)];
        }

        public static Rect GetLabelRect(Rect rect)
		{
            rect.width = EditorGUIUtility.labelWidth - indentLevel * 15f - 1f;
            return rect;
		}
        public static Rect GetIdRect(Rect rect, Rect labelRect)
        {
            rect.x += labelRect.width + 1f;
            rect.width = (rect.width - labelRect.width) * 0.315f - 1f;
            return rect;
        }
        public static Rect GetKeyRect(Rect rect, Rect labelRect, Rect idRect)
        {
            rect.x = idRect.x + idRect.width + 1f;
            rect.width = rect.width - labelRect.width - idRect.width - 1f;
            return rect;
        }

    }
}
