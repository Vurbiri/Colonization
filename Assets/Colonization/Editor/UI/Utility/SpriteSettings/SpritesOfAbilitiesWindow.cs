using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization.UI;
using static UnityEditor.EditorGUILayout;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class SpritesOfAbilitiesWindow : EditorWindow
	{
		private const string NAME = "Sprites Of Abilities", MENU = MENU_UI_PATH + NAME;
		
		[SerializeField] private SpritesOfAbilitiesScriptable _scriptable;
		
		private Editor _editor;
		private Vector2 _scroll;
		
		[MenuItem(MENU, false, 40)]
		private static void ShowWindow()
		{
			GetWindow<SpritesOfAbilitiesWindow>(true, NAME);
		}

		private void OnEnable()
		{
			EUtility.SetScriptable(ref _scriptable);
			
			if (_scriptable != null)
				_editor = Editor.CreateEditor(_scriptable);		
		}
		
		private void OnGUI()
		{
			BeginWindows();
			{
				Space(10f);
				LabelField(NAME, STYLES.H1);
				if (_editor == null)
				{
					if (GUILayout.Button("Create"))
					{
						_scriptable = EUtility.CreateScriptable<SpritesOfAbilitiesScriptable>(NAME, SETTINGS_UI_PATH);
						if (_scriptable != null)
							_editor = Editor.CreateEditor(_scriptable);
					}
				}
				else
				{
					_scroll = BeginScrollView(_scroll);
					{
						_editor.OnInspectorGUI();
					}
					EndScrollView();
				}
				
			}
			EndWindows();
		}
		
		private void OnDisable()
		{
			DestroyImmediate(_editor);
		}
	}
}