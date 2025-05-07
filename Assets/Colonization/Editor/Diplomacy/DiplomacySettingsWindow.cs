//Assets\Colonization\Editor\Diplomacy\DiplomacySettingsWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class DiplomacySettingsWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "Diplomacy", MENU = MENU_PATH + NAME;
        #endregion

        [SerializeField] private DiplomacySettings _settings;

        private SerializedObject _serializedObject;
        private SerializedProperty _serializedProperty;

        [MenuItem(MENU, false, 30)]
		private static void ShowWindow()
		{
			GetWindow<DiplomacySettingsWindow>(true, NAME);
		}
		
		private void OnEnable()
		{
			SettingsFile.Load(ref _settings, SETTINGS_FILE.DIPLOMACY);
            _serializedObject = new(this);
            _serializedProperty = _serializedObject.FindProperty("_settings");
        }
		
		private void OnGUI()
		{
            _serializedObject.Update();
            BeginWindows();
            {
                Space(10);
                LabelField("Diplomacy Settings", STYLES.H1);

                BeginVertical(GUI.skin.box);
                    PropertyField(_serializedProperty);
                EndVertical();
            }
            EndWindows();
            _serializedObject.ApplyModifiedProperties();
        }
		
		private void OnDisable()
		{
            SettingsFile.Save(_settings, SETTINGS_FILE.DIPLOMACY);
        }
    }
}
