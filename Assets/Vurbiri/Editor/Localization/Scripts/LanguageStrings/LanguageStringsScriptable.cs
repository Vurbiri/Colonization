//Assets\Vurbiri\Editor\Localization\Scripts\LanguageStrings\LanguageStringsScriptable.cs
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Vurbiri.Storage;

namespace Vurbiri.Localization.Editors
{
    using static CONST;

    //[CreateAssetMenu(fileName = LANG_STRINGS_NAME, menuName = "Vurbiri/Localization/LanguageStrings", order = 51)]
    internal class LanguageStringsScriptable : AGetOrCreateScriptableObject<LanguageStringsScriptable>
    {
        [SerializeField] private Files _file = Files.Main;
        [SerializeField] private string _loadFile = string.Empty;
        [SerializeField] private List<LanguageRecord> _strings;

        private LanguageType[] _languages;
        private string[] _names;
        private int _count;
        private string _folderPath, _folder, _languageFile;

        public string LoadFile => _loadFile;

        public void Init()
        {
            using (SettingsScriptable settings = ProjectSettingsScriptable.GetCurrentSettings())
            {
                _folderPath = settings.FolderPath;
                _folder = settings.Folder;
                _languageFile = settings.LanguageFile;
            }

            LoadObjectFromResourceJson(Path.Combine(_folder, _languageFile), out _languages);
            _count = _languages.Length;
            _names = new string[_count];
            for (int i = 0; i < _count; i++)
                _names[i] = _languages[i].Folder.Concat(" (", _languages[i].Name, ")");
        }

        public void UnInit()
        {
            _languages = null;
            _count = -1;
            _names = null;
        }

        public void Reset()
        {
            _loadFile = string.Empty;
            _strings = null;
        }

        public void OnAdded(IEnumerable<int> indexes)
        {
            if (string.IsNullOrEmpty(_loadFile))
            {
                _strings.RemoveRange(indexes.First(), indexes.Count());
                return;
            }

            LanguageRecord record;
            foreach (int index in indexes)
            {
                record = new(string.Empty);
                for (int i = 0; i < _count; i++)
                    record.Add(_names[i], string.Empty);

                _strings[index] = record;
            }
        }

        public string Load()
        {
            Dictionary<string, string>[] strings = new Dictionary<string, string>[_count];

            string path;
            int idMaxLength = -1, maxLength = -1;
            for (int i = 0; i < _count; i++)
            {
                path = Application.dataPath.Concat(Path.Combine(_folderPath, _languages[i].Folder, _file.ToString()), JSON_EXP);
                if (File.Exists(path))
                    LoadObjectFromResourceJson(Path.Combine(_folder, _languages[i].Folder, _file.ToString()), out strings[i]);
                else
                    strings[i] = new();

                if (strings[i].Count > maxLength)
                {
                    maxLength = strings[i].Count;
                    idMaxLength = i;
                }
            }

            _strings = new();

            LanguageRecord record;
            foreach (var key in strings[idMaxLength].Keys)
            {
                record = new(key);

                for (int i = 0; i < _count; i++)
                {
                    if (!strings[i].TryGetValue(key, out string text))
                        text = string.Empty;

                    record.Add(_names[i], text);
                }

                _strings.Add(record);
            }

            return _loadFile = _file.ToString();
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_loadFile))
                return;

            Dictionary<string, string>[] strings = new Dictionary<string, string>[_count];

            for (int i = 0; i < _count; i++)
                strings[i] = new(_strings.Count);

            string key;
            foreach (var str in _strings)
            {
                key = str.Key;
                if (string.IsNullOrEmpty(key))
                    continue;

                for (int i = 0; i < _count; i++)
                    strings[i].Add(key, str.GetText(i));
            }

            string path;
            for (int i = 0; i < _count; i++)
            {
                path = Application.dataPath.Concat(Path.Combine(_folderPath, _languages[i].Folder, _file.ToString()), JSON_EXP);
                if (!File.Exists(path))
                    new FileInfo(path).Directory.Create();
                
                File.WriteAllText(path, JsonConvert.SerializeObject(strings[i]));
            }

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        public static LanguageStringsScriptable GetOrCreateSelf() => GetOrCreateSelf(STR_LANG_NAME, STR_LANG_PATH);
        public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(STR_LANG_NAME, STR_LANG_PATH));
    }
}
