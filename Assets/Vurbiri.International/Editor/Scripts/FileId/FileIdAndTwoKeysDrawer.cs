using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;
using static Vurbiri.International.Editor.FileIdAndKeyDrawer;

namespace Vurbiri.International.Editor
{
	[CustomPropertyDrawer(typeof(FileIdAndTwoKeys))]
	public class FileIdAndTwoKeysDrawer : PropertyDrawer
	{
        #region Consts
        private const string F_ID = "id", F_KEY_A = "keyA", F_KEY_B = "keyB";
        #endregion

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty idProperty = mainProperty.FindPropertyRelative(F_ID);
            SerializedProperty keyPropertyA = mainProperty.FindPropertyRelative(F_KEY_A);
            SerializedProperty keyPropertyB = mainProperty.FindPropertyRelative(F_KEY_B);

            Rect labelRect = GetLabelRect(position);
            Rect idRect = GetIdRect(position);
            Rect keyRect = GetKeyRect(position);

            label = BeginProperty(position, label, mainProperty);
            {
                PrefixLabel(labelRect, label);
                idProperty.intValue = IntPopup(idRect, idProperty.intValue, LanguageData.fileNames, LanguageData.fileValues);
                KeyField(keyRect, keyPropertyA, idProperty.intValue);
                keyRect.y += _height;
                KeyField(keyRect, keyPropertyB, idProperty.intValue);
            }
            EndProperty();
        }
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return _height * 2f;
		}
	}
}