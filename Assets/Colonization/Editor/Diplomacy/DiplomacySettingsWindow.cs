//Assets\Colonization\Editor\Diplomacy\DiplomacySettingsWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class DiplomacySettingsWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "Diplomacy Settings", MENU = MENU_PATH + NAME;
		#endregion
		
		[SerializeField] private DiplomacySettingsScriptable _scriptable;
		
		private Editor _editor;
		
		[MenuItem(MENU)]
		private static void ShowWindow()
		{
			GetWindow<DiplomacySettingsWindow>(true, NAME);
		}
		
		private void OnEnable()
		{
            if (_scriptable == null)
                _scriptable = VurbiriEditor.Utility.FindAnyScriptable<DiplomacySettingsScriptable>();

            _editor = Editor.CreateEditor(_scriptable, typeof(DiplomacySettingsEditor));		
		}
		
		private void OnGUI()
		{
            BeginWindows();
            EditorGUILayout.BeginVertical(GUI.skin.window);
            _editor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            EndWindows();
        }
		
		private void OnDisable()
		{
			DestroyImmediate(_editor);
		}
	}
}