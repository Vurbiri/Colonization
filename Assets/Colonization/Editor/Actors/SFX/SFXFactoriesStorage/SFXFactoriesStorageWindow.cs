using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization.Actors;
using static UnityEditor.EditorGUILayout;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class SFXFactoriesStorageWindow : EditorWindow
	{
		private const string NAME = "SFX Factories", MENU = MENU_ACTORS_PATH + NAME;
        
		private readonly string _defaultPath = "Assets/Colonization/HitSFX", _defaultName = "FactoriesStorage";

        [SerializeField] private SFXFactoriesStorage _scriptable;

        private Editor _editor;

        [MenuItem(MENU)]
		private static void ShowWindow()
		{
			GetWindow<SFXFactoriesStorageWindow>(true, NAME);
		}
		
		private void OnEnable()
		{
			if(_scriptable == null)
				_scriptable = EUtility.FindAnyScriptable<SFXFactoriesStorage>();
            
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
                        _scriptable = EUtility.CreateScriptable<SFXFactoriesStorage>(_defaultName, _defaultPath);
                        if (_scriptable != null)
                            _editor = Editor.CreateEditor(_scriptable);
                    }
                }
				else
				{
                    _editor.OnInspectorGUI();
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