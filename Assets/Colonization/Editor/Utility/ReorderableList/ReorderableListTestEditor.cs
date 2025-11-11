using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VurbiriEditor.Colonization
{
	[CustomEditor(typeof(ReorderableListTest), true), CanEditMultipleObjects]
	public class ReorderableListTestEditor : Editor
	{
        private float _height, _itemHeight;
        private SerializedProperty _arrayProperty;
        private ReorderableList _list;

        private void OnEnable()
		{
            _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            _itemHeight = 2f * EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            _arrayProperty = serializedObject.FindProperty("values");
            _list = new(serializedObject, _arrayProperty, true, false, true, true)
            {
                multiSelect = true,
                //drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawListItems,
                drawElementBackgroundCallback = ReorderableListUtility.DrawBackground,
                elementHeightCallback = GetHeight
            };
        }
		
		public override void OnInspectorGUI()
		{
            serializedObject.Update();

            if(_arrayProperty.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(_arrayProperty.isExpanded, "Test"))
            _list.DoLayoutList();
            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
		}

        private void DrawListItems(Rect position, int index, bool isActive, bool isFocused)
        {
            var currentProperty = _list.serializedProperty.GetArrayElementAtIndex(index);
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginProperty(position, null, currentProperty);
            {
                EditorGUI.PropertyField(position, currentProperty.FindPropertyRelative("index"));

                position.y += _height;
                EditorGUI.PropertyField(position, currentProperty.FindPropertyRelative("name"));
            }
            EditorGUI.EndProperty();
        }

        private void DrawHeader(Rect position)
        {
            EditorGUI.LabelField(position, "Test");
        }

        private float GetHeight(int index) => _itemHeight;

    }
}