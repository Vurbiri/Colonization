//Assets\Colonization\Editor\Players\PlayerColorsDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(PlayerColors))]
	public class PlayerColorsDrawer : PropertyDrawer
	{
		#region Consts
		private const string P_NAME = "_colors";
        #endregion

        private readonly string[] _names = PlayerId.PositiveNames;
        private readonly int _count = PlayerId.Count;
        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			BeginProperty(position, label, mainProperty);
			{
                SerializedProperty propertyValues = mainProperty.FindPropertyRelative(P_NAME);
				propertyValues.arraySize = _count;

				SerializedProperty property;
                for (int i = 0; i < _count; i++)
				{
                    property = propertyValues.GetArrayElementAtIndex(i);

					property.colorValue = ColorField(position, _names[i], property.colorValue);
					position.y += _height;
                }
            }
			EndProperty();
		}
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return _height * _count;
		}
	}
}