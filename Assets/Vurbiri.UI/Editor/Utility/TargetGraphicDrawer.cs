//Assets\Vurbiri.UI\Editor\Utility\TargetGraphicDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.UI
{
    [CustomPropertyDrawer(typeof(VSelectable.TargetGraphic))]
	public class TargetGraphicDrawer : PropertyDrawer
	{
		#region Consts
		private const float OFFSET = 5f, SCALE = 0.47f;
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
				position.x += position.width + OFFSET; position.width = width - position.width - OFFSET * 1.1f;
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