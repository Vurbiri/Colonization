//Assets\Colonization\Editor\EntryPoint\GameplayEntryPointWindow.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Vurbiri.Colonization;
using Vurbiri.EntryPoint;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class SceneEntryPointWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "EntryPoint", MENU = MENU_PATH + NAME;
		#endregion
		
		private Editor _editor;
        private readonly GUIContent _titleScene = new("Scene EntryPoint"), _titleProject = new("Project EntryPoint"), _titleNone = new("No EntryPoint");
        private readonly Vector2 _wndMinSizeScene = new(450f, 625f), _wndMinSizeProject = new(425f, 405f);
        private Vector2 _scrollPos;

        [MenuItem(MENU)]
		private static void ShowWindow()
		{
			GetWindow<SceneEntryPointWindow>(true);
		}
		
		private void OnEnable()
		{

			ASceneEntryPoint gameplayEntryPoint = FindAnyObjectByType<ASceneEntryPoint>();

			if (gameplayEntryPoint != null)
			{
				_editor = Editor.CreateEditor(gameplayEntryPoint);
				minSize = _wndMinSizeScene;
				titleContent = _titleScene;
				return;
			}

			ProjectInitializationData projectInitialization = FindAnyObjectByType<ProjectInitializationData>();

			if (projectInitialization != null)
			{
				_editor = Editor.CreateEditor(projectInitialization);
				minSize = _wndMinSizeProject;
				titleContent = _titleProject;
				return;
			}

			maxSize = new(350f, 25f);
			titleContent = _titleNone;
		}
		
		private void OnGUI()
		{
            if (_editor == null)
                return;

            BeginWindows();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            _editor.OnInspectorGUI();
            EditorGUILayout.EndScrollView();
            EndWindows();
		}
		
		private void OnDisable()
		{
            if (_editor == null)
                return;

            DestroyImmediate(_editor);

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
	}
}