using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Vurbiri.International.Editor
{
	[CustomPropertyDrawer(typeof(FileIds))]
	public class FileIdsDrawer : PropertyDrawer
	{
		#region Consts
		private const string F_NAME = "_id";
		#endregion
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty valueProperty = mainProperty.FindPropertyRelative(F_NAME);
            label = BeginProperty(position, label, mainProperty);
			{
                valueProperty.intValue = MaskField(position, label, valueProperty.intValue, LanguageData.fileNames) & ~(-1 << LanguageData.fileCount);
            }
			EndProperty();
		}
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
	}
}
