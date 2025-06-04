using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
	[CustomPropertyDrawer(typeof(PlayerNames))]
	public class PlayerNamesDrawer : PropertyDrawer
	{
		#region Consts
		private const string F_NAME = "_nameKeys";
		#endregion

		private readonly GUIContent[] labels = { new(PlayerId.PositiveNames[0]), new(PlayerId.PositiveNames[1]), new(PlayerId.PositiveNames[2]),new(PlayerId.PositiveNames[3]) };

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            label = BeginProperty(position, label, mainProperty);
			{
				if (mainProperty.isExpanded = Foldout(position, mainProperty.isExpanded, label))
				{
                    SerializedProperty namesProperty = mainProperty.FindPropertyRelative(F_NAME);
                    namesProperty.arraySize = PlayerId.Count;

                    indentLevel++;
                    for (int i = 0; i < PlayerId.Count; ++i)
					{
						position.y += _height;
						PropertyField(position, namesProperty.GetArrayElementAtIndex(i), labels[i]);
                    }
                    indentLevel--;
                }
			}
			EndProperty();
		}
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			float ratio = 1f;
			if (mainProperty.isExpanded) ratio += PlayerId.Count;
            return _height * ratio;
		}
    }
}