using UnityEditor;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
	[CustomEditor(typeof(DiamondEffect)), CanEditMultipleObjects]
	public class DiamondEffectEditor : Editor
	{
		private const float SIZE_X = 58f, SIZE_Y = 40f, SPACE = 10f;
        private const float OFFSET_X = SIZE_X * 0.5f + SPACE, OFFSET_Y = SIZE_Y + SPACE * 2f;
        private const float HEIGHT = (SIZE_Y + SPACE) * 2f;

        private SerializedProperty _leftTopProperty;
        private SerializedProperty _rightTopProperty;
        private SerializedProperty _leftBottomProperty;
        private SerializedProperty _rightBottomProperty;

		private readonly GUIContent empty = new(string.Empty);

        private void OnEnable()
		{
			_leftTopProperty     = serializedObject.FindProperty(DiamondEffect.leftTopField);
			_rightTopProperty    = serializedObject.FindProperty(DiamondEffect.rightTopField);
			_leftBottomProperty  = serializedObject.FindProperty(DiamondEffect.leftBottomField);
			_rightBottomProperty = serializedObject.FindProperty(DiamondEffect.rightBottomField);
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			Space(2f);
			Rect position = BeginVertical();
			{
				float center = (position.width + position.x) * 0.5f;
				float left = center - OFFSET_X, right = center + OFFSET_X;
                
				position.width = SIZE_X;  position.height = SIZE_Y;

				position.x = left;
				EditorGUI.PropertyField(position, _leftTopProperty, empty);
                position.x = right;
                EditorGUI.PropertyField(position, _rightTopProperty, empty);

				position.y += OFFSET_Y;

                position.x = left;
                EditorGUI.PropertyField(position, _leftBottomProperty, empty);
                position.x = right;
                EditorGUI.PropertyField(position, _rightBottomProperty, empty);

                Space(HEIGHT);
            }
			EndVertical();
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}