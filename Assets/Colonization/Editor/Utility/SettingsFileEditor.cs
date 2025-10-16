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

        public static void Load<T>(ref T obj) => Load(ref obj, PATH);
        public static void Load<T>(ref T obj, string path)
        {
            Type type = typeof(T);
            try
            {
                obj = (T)JsonConvert.DeserializeObject(File.ReadAllText(GetPath(path, type), CONST_EDITOR.utf8WithoutBom), type);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }

        public static void Save<T>(T obj) => Save(obj, PATH);
        public static void Save<T>(T obj, string path)
        {
            Type type = typeof(T);
            FileInfo file = new(GetPath(path, type));

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
        private static string GetPath(string path, Type type) => Application.dataPath.Concat(path, type.Name, JSON_EXT);
    }
}
