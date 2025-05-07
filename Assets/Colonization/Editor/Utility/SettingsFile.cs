//Assets\Colonization\Editor\Utility\SettingsFile.cs
using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor.Colonization
{
    internal class SettingsFile
	{
        private const string FOLDER = "/Colonization/Settings/Resources/";
        private const string JSON_EXT = ".json";

        public static void Load<T>(ref T obj, string fileName)
        {
            string path = GetPath(fileName);
            if (File.Exists(GetPath(fileName)))
                obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(path, CONST_EDITOR.utf8WithoutBom));
        }

        public static void Save<T>(T obj, string fileName)
        {
            FileInfo file = new(GetPath(fileName));
            if (!file.Directory.Exists)
            {
                Debug.Log($"Created <b><color={Color.red.ToHex()}>{file.Directory}</color></b>");
                file.Directory.Create();
            }

            string json = JsonConvert.SerializeObject(obj, typeof(T), Formatting.Indented, null);

            File.WriteAllText(file.FullName, json, CONST_EDITOR.utf8WithoutBom);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            Debug.Log($"Saved <b><color={Color.cyan.ToHex()}>{file.Name}</color></b>");
        }

        private static string GetPath(string fileName) => Application.dataPath.Concat(FOLDER, fileName, JSON_EXT);
    }
}
