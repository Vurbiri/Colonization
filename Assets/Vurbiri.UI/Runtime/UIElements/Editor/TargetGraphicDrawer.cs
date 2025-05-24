using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Vurbiri.UI
{
    public partial class VSelectable
    {
		[CustomPropertyDrawer(typeof(TargetGraphic))]
		public class TargetGraphicDrawer : PropertyDrawer
		{
			#region Consts
			private const float OFFSET = 5f, SCALE = 0.46f;
			private const string P_GRAPHIC = "_graphic", P_STATE = "_stateFilter";
			#endregion

			private readonly GUIContent _empty = new();

			public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
			{
				float width = position.width;

				position.height = EditorGUIUtility.singleLineHeight;
				position.width *= SCALE;

				BeginProperty(position, label, mainProperty);
				{
					PropertyField(position, mainProperty.FindPropertyRelative(P_GRAPHIC), _empty);
					position.x += position.width + OFFSET; position.width = width - position.width - OFFSET;
					PropertyField(position, mainProperty.FindPropertyRelative(P_STATE), _empty);
				}
				EndProperty();
			}

			public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
			{
				return EditorGUIUtility.singleLineHeight;
			}
		}
	}
}
