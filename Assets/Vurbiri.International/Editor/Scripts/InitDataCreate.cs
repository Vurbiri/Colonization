using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.International
{
	using static CONST;

	internal class InitDataCreate
	{
		private const string ZIP_FILE = IN_RESOURCE_FOLDER + "Init/Data.zip";
		private const string BANNER = "Banner.png.meta";
		private const string BANNER_ENG = OUT_RESOURCE_FOLDER + "English/" + BANNER;
		private const string BANNER_RUS = OUT_RESOURCE_FOLDER + "Russian/" + BANNER;
		private const string IMPORT_SETTINGS = "Init/BannerImportSettings";

		[InitializeOnLoadMethod]
		private static void OnProjectLoadedInEditor()
		{
			DirectoryInfo directory = new(FileUtil.GetPhysicalPath(OUT_RESOURCE_FOLDER));
			if (!directory.Exists)
			{
				directory.Create();
				ZipFile.ExtractToDirectory(FileUtil.GetPhysicalPath(ZIP_FILE), directory.FullName);
				Debug.Log($"<b>[Localization]</b> <color=green>Created <i>{directory.FullName}</i></color>");
				SettingsImportImage(FileUtil.GetPhysicalPath(BANNER_ENG));
				SettingsImportImage(FileUtil.GetPhysicalPath(BANNER_RUS));
			}
		}

		private static async void SettingsImportImage(string path)
		{
			int count = 99;
			while (!File.Exists(path) && count --> 0)
				await Task.Delay(250);

			TextAsset textAsset = Resources.Load<TextAsset>(IMPORT_SETTINGS);

			string guid = Regex.Match(File.ReadAllText(path), @"guid: \w+").Value;
			File.WriteAllText(path, textAsset.text.Replace("guid:", guid));

			AssetDatabase.Refresh();
			Resources.UnloadAsset(textAsset);
		}
	}
}
