using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(PlayerColors))]
	public class PlayerColorsDrawer : PropertyDrawer
	{
        private readonly string P_NAME = "_colors";
        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			BeginProperty(position, label, mainProperty);
			{
                SerializedProperty propertyValues = mainProperty.FindPropertyRelative(P_NAME);
				propertyValues.arraySize = PlayerId.Count;

				SerializedProperty property;
                for (int i = 0; i < PlayerId.Count; ++i)
				{
                    property = propertyValues.GetArrayElementAtIndex(i);

					property.colorValue = ColorField(position, PlayerId.Names_Ed[i], property.colorValue);
					position.y += _height;
                }
            }
			EndProperty();
		}
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return _height * PlayerId.Count;
		}
	}
}
