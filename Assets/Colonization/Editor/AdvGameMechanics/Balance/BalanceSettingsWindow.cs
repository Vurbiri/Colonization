//Assets\Colonization\Editor\AdvGameMechanics\Balance\BalanceSettingsWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class BalanceSettingsWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "Balance", MENU = MENU_MECH_PATH + NAME;
		#endregion
		
		[SerializeField] private BalanceSettingsScriptable _scriptable;
		
		private Editor _editor;
		
		[MenuItem(MENU)]
		private static void ShowWindow()
		{
			GetWindow<BalanceSettingsWindow>(true, NAME);
		}
		
		private void OnEnable()
		{
			if(_scriptable == null)
				_scriptable = EUtility.FindAnyScriptable<BalanceSettingsScriptable>();
			
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