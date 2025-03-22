//Assets\Vurbiri\Editor\Utility\ScriptTemplatesKeywords.cs
using System.IO;
using UnityEditor;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    public class ScriptTemplatesKeywords : AssetModificationProcessor
	{
        #region Consts
        private const string MENU_NAME = "Custom keywords/", MENU = MENU_PATH + MENU_NAME;
        private const string MENU_NAME_ENABLE = "Enable", MENU_COMMAND_ENABLE = MENU + MENU_NAME_ENABLE;
        private const string MENU_NAME_DISABLE = "Disable", MENU_COMMAND_DISABLE = MENU + MENU_NAME_DISABLE;
        private const string KEY_SAVE = "CSTK_ENABLE";
        private const string WINDOW = "Window", EDITOR = "Editor", DRAWER = "Drawer";
		#endregion

		private static bool _enabled = true;

        [MenuItem(MENU_COMMAND_ENABLE, false, 35)]
        private static void CommandEnable()
        {
            _enabled = true;
			Save(); Log();
        }
        [MenuItem(MENU_COMMAND_ENABLE, true, 35)]
        private static bool CommandEnableValidate()
        {
            SetChecked();
            return !_enabled;
        }

        [MenuItem(MENU_COMMAND_DISABLE, false, 36)]
        private static void CommandDisable()
        {
            _enabled = false;
            Save(); Log();
        }
        [MenuItem(MENU_COMMAND_DISABLE, true, 36)]
        private static bool CommandDisableValidate() => !CommandEnableValidate();

        public static void OnWillCreateAsset(string assetName)
		{
            if (!_enabled) return;
            
            if (!assetName.EndsWith(META_EXT)) return;
			assetName = assetName.Replace(META_EXT, string.Empty);
			if (!assetName.EndsWith(CS_EXT)) return;

			int index = Application.dataPath.LastIndexOf(ASSETS);
			string path = Path.Combine(Application.dataPath[..index], assetName);
			string file = File.ReadAllText(path, utf8WithoutBom);
			string name = Path.GetFileNameWithoutExtension(path);

			bool isReplace = false;
			isReplace |= Replace(ref file, @"#PROJECTNAME#", PlayerSettings.productName);
			isReplace |= Replace(ref file, @"#COMPANYNAME#", PlayerSettings.companyName);
            isReplace |= Replace(ref file, @"#NAMENOTWINDOW#", name.Replace(WINDOW, string.Empty));
            isReplace |= Replace(ref file, @"#NAMENOTEDITOR#", name.Replace(EDITOR, string.Empty));
			isReplace |= Replace(ref file, @"#NAMENOTDRAWER#", name.Replace(DRAWER, string.Empty));

			if (isReplace)
				File.WriteAllText(path, file, utf8WithoutBom);

			#region Local: Replace(..)
			//=======================================================
			static bool Replace(ref string file, string keyword, string replace)
			{
				if (file.IndexOf(keyword) < 0) 
					return false;

				file = file.Replace(keyword, replace);
				return true;
			}
			//=======================================================
			#endregion
		}

		private static void Save()
		{
            EditorPrefs.SetBool(KEY_SAVE, _enabled);

            SetChecked();
        }

        [InitializeOnLoadMethod]
        private static void Load()
		{
            if (EditorPrefs.HasKey(KEY_SAVE))
                _enabled = EditorPrefs.GetBool(KEY_SAVE);
            
            SetChecked(); 
        }

        private static void SetChecked()
        {
            Menu.SetChecked(MENU_COMMAND_ENABLE, _enabled);
            Menu.SetChecked(MENU_COMMAND_DISABLE, !_enabled);

            Menu.SetChecked(MENU, _enabled);
        }

        private static void Log()
        {
            string state = _enabled ? MENU_NAME_ENABLE : MENU_NAME_DISABLE;
            Debug.Log($"[ScriptTemplatesKeywords] {state}");
        }
    }
}
