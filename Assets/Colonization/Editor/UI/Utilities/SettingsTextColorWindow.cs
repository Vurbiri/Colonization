//Assets\Colonization\Editor\UI\Utilities\SettingsTextColorWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.UI;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization.UI
{
    public class SettingsTextColorWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "Settings Text Color", MENU = MENU_UI_PATH + NAME;
		#endregion
		
		[SerializeField] private SettingsTextColorScriptable _scriptable;
		
		private Editor _editor;
		
		[MenuItem(MENU)]
		private static void ShowWindow()
		{
			GetWindow<SettingsTextColorWindow>(true, NAME);
		}
		
		private void OnEnable()
		{
			if(_scriptable == null)
				_scriptable = VurbiriEditor.Utility.FindAnyScriptable<SettingsTextColorScriptable>();
			
			_editor = Editor.CreateEditor(_scriptable);		
		}
		
		private void OnGUI()
		{
			BeginWindows();
			_editor.OnInspectorGUI();
			EndWindows();
		}
		
		private void OnDisable()
		{
			DestroyImmediate(_editor);
		}
	}
}