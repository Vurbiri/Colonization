//Assets\Colonization\Editor\EntryPoint\GameplayEntryPointWindow.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.EntryPoint;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization.EntryPoint
{
    public class SceneEntryPointWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "EntryPoint", MENU = MENU_PATH + NAME;
		#endregion
		
		private Editor _editor, _editorProject;
        private readonly GUIContent _titleScene = new("Scene EntryPoint"), _titleProject = new("Project EntryPoint"), _titleNone = new("No EntryPoint");
        private readonly Vector2 _wndMinSizeScene = new(450f, 625f), _wndMinSizeProject = new(425f, 405f);
        private Vector2 _scrollPos;

        [MenuItem(MENU, false, 52)]
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

            ProjectInitialization projectInitialization = FindAnyObjectByType<ProjectInitialization>();
			if (projectInitialization != null)
			{
				_editor = Editor.CreateEditor(projectInitialization);
                _editorProject = Editor.CreateEditor(FindAnyObjectByType<ProjectEntryPoint>());
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
			if (_editorProject != null)
			{
                _editorProject.OnInspectorGUI();
				EditorGUILayout.Space(8);
            }
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