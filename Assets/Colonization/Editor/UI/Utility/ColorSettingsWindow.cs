using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.UI;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization.UI
{
    public class ColorSettingsWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "Color Settings", MENU = MENU_UI_PATH + NAME;
		#endregion
		
		[SerializeField] private ColorSettingsScriptable _scriptable;
		
		private Editor _editor;
		private Vector2 _scroll;


        [MenuItem(MENU, false, 40)]
		private static void ShowWindow()
		{
			GetWindow<ColorSettingsWindow>(true, NAME);
		}
		
		private void OnEnable()
		{
            Vurbiri.EUtility.SetScriptable(ref _scriptable);
			
			_editor = Editor.CreateEditor(_scriptable);		
		}
		
		private void OnGUI()
		{
			BeginWindows();
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            _editor.OnInspectorGUI();
            EditorGUILayout.EndScrollView();
            EndWindows();
		}
		
		private void OnDisable()
		{
			DestroyImmediate(_editor);
		}
	}
}
