//Assets\Colonization\Editor\Diplomacy\DiplomacySettingsWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class DiplomacySettingsWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "Diplomacy", MENU = MENU_PATH + NAME;
		#endregion
		
		[SerializeField] private DiplomacySettingsScriptable _scriptable;
		
		private Editor _editor;
		
		[MenuItem(MENU, false, 30)]
		private static void ShowWindow()
		{
			GetWindow<DiplomacySettingsWindow>(true, NAME);
		}
		
		private void OnEnable()
		{
            if (_scriptable == null)
                _scriptable = EUtility.FindAnyScriptable<DiplomacySettingsScriptable>();

            _editor = Editor.CreateEditor(_scriptable, typeof(DiplomacySettingsEditor));		
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
