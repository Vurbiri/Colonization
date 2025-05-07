//Assets\Colonization\Editor\Utility\SettingsFileEditor.cs
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    internal class SettingsFileEditor
	{
        private const string PATH = "/Colonization/Settings/Resources/" + SettingsFile.FOLDER;
        private const string JSON_EXT = ".json";

        public static void Load<T>(ref T obj)
        {
            Type type = typeof(T);
            string path = GetPath(type);

            if (File.Exists(path))
                obj = (T)JsonConvert.DeserializeObject(File.ReadAllText(path, CONST_EDITOR.utf8WithoutBom), type);
        }

        public static void Save<T>(T obj)
        {
            Type type = typeof(T);
            FileInfo file = new(GetPath(type));

            if (!file.Directory.Exists)
            {
                file.Directory.Create();
                Debug.Log($"Created <b><color={Color.red.ToHex()}>{file.Directory}</color></b>");
            }

            string json = JsonConvert.SerializeObject(obj, type, Formatting.Indented, null);
            File.WriteAllText(file.FullName, json, CONST_EDITOR.utf8WithoutBom);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static string GetPath(Type type) => Application.dataPath.Concat(PATH, type.Name, JSON_EXT);
    }
}
