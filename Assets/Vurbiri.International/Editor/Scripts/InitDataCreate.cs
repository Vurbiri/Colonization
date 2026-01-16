using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor.International
{
	internal class InitDataCreate
	{
        private const string RESOURCES_FOLDER = CONST.RESOURCES_FOLDER;
        private const string BANNER = "Banner.png.meta";
		private const string BANNER_ENG = RESOURCES_FOLDER + "English/" + BANNER;
		private const string BANNER_RUS = RESOURCES_FOLDER + "Russian/" + BANNER;
		private const string IMPORT_SETTINGS = "BannerImportSettings";
        private const string ZIP_FILE = "ImportData";

        internal static T LoadOrCreateScriptable<T>(string name, string path) where T : ScriptableObject
        {
            var scriptable = AssetDatabase.LoadAssetAtPath<T>(path);
			if (scriptable == null)
			{
				scriptable = ScriptableObject.CreateInstance<T>();
                scriptable.name = name;

                var directory = new DirectoryInfo(FileUtil.GetPhysicalPath(path));
                if (!directory.Exists)
                    directory.Create();

                AssetDatabase.CreateAsset(scriptable, path);
                Debug.Log($"<b>[Localization]</b> <color=yellow>ScriptableObject <i>{path}</i> created.</color>");
            }
            return scriptable;
        }

        [InitializeOnLoadMethod]
		private static void OnProjectLoadedInEditor()
		{
            DirectoryInfo directory = new(FileUtil.GetPhysicalPath(RESOURCES_FOLDER));
			if (!directory.Exists)
			{
                directory.Create();
                Debug.Log($"<b>[Localization]</b> <color=yellow>Directory <i>{directory.FullName}</i> created.</color>");

                var zip = Resources.Load(ZIP_FILE) as TextAsset;
                using (MemoryStream stream = new(zip.bytes))
                {
                    using ZipArchive archive = new(stream);
                    archive.ExtractToDirectory(directory.FullName);
                }
                zip.Unload();

                var settings = Resources.Load<TextAsset>(IMPORT_SETTINGS);
                SettingsImportImage(FileUtil.GetPhysicalPath(BANNER_ENG), settings.text);
                SettingsImportImage(FileUtil.GetPhysicalPath(BANNER_RUS), settings.text);
                settings.Unload();
            }

            directory = new(FileUtil.GetPhysicalPath(CONST.EDITOR_FOLDER));
            if (!directory.Exists)
            {
                directory.Create();
                CreateScriptable<LanguageFilesScriptable>(CONST.LANG_FILES_NAME, CONST.LANG_FILES_PATH);
                CreateScriptable<LanguageTypesScriptable>(CONST.LANG_TYPES_NAME, CONST.LANG_TYPES_PATH);
                CreateScriptable<LanguageStringsScriptable>(CONST.LANG_STRING_NAME, CONST.LANG_STRING_PATH);
            }
        }

        private static async void CreateScriptable<T>(string name, string path) where T : ScriptableObject
        {
            await Task.Delay(8);

            var scriptable = ScriptableObject.CreateInstance<T>();
            scriptable.name = name;
            AssetDatabase.CreateAsset(scriptable, path);
            Debug.Log($"<b>[Localization]</b> <color=yellow>ScriptableObject <i>{path}</i> created.</color>");
        }

        private static async void SettingsImportImage(string path, string settings)
		{
			int count = 66;
			while (!File.Exists(path) && count --> 0)
				await Task.Delay(111);

            if (!File.Exists(path)) return;
			
            var guid = Regex.Match(File.ReadAllText(path), @"guid: \w+").Value;
			File.WriteAllText(path, settings.Replace("guid:", guid));

			AssetDatabase.Refresh();
		}
    }
}
