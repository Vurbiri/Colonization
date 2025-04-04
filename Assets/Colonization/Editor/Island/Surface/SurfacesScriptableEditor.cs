//Assets\Colonization\Editor\Island\Surface\SurfacesScriptableEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(SurfacesScriptable), true), CanEditMultipleObjects]
	public class SurfacesScriptableEditor : Editor
	{
		#region Consts
		private const string P_ARRAY = "_surfaces", P_SUB_ARRAY = "_values";
        #endregion

        private SurfacesScriptable _surfaces;
        private SerializedProperty _serializedProperty;
        private Vector2 _scrollPos;

        private void OnEnable()
		{
			_serializedProperty = serializedObject.FindProperty(P_ARRAY).FindPropertyRelative(P_SUB_ARRAY);
            _surfaces = EUtility.FindAnyScriptable<SurfacesScriptable>();

        }
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

            if(_surfaces != null) 
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Set"))
                {
                    _surfaces.Set();
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(_surfaces);
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.Space();
            }

            EditorGUILayout.BeginVertical(GUI.skin.window);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            for (int i = 0; i < _serializedProperty.arraySize; i++)
			{
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(GUI.skin.window);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_serializedProperty.GetArrayElementAtIndex(i), new GUIContent(IdType<SurfaceId>.Names[i]));
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
		}
	}
}