//Assets\Colonization\Editor\UI\Utility\SettingsTextColorWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.UI;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization.UI
{
    public class TextColorSettingsWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "Text Color Settings", MENU = MENU_UI_PATH + NAME;
		#endregion
		
		[SerializeField] private TextColorSettingsScriptable _scriptable;
		
		private Editor _editor;
		
		[MenuItem(MENU, false, 40)]
		private static void ShowWindow()
		{
			GetWindow<TextColorSettingsWindow>(true, NAME);
		}
		
		private void OnEnable()
		{
			if(_scriptable == null)
				_scriptable = Vurbiri.EUtility.FindAnyScriptable<TextColorSettingsScriptable>();
			
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