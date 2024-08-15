using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    //[DefaultExecutionOrder(-2)]
    public partial class Localization : ASingleton<Localization>
    {
        [SerializeField] private string _folder = "Localization";
        [SerializeField] private string _languagesFile = "Languages";
        [SerializeField] private string _defaultLang = "ru";
        [SerializeField] private EnumArray<TextFiles, bool> _loadFiles;

        public IEnumerable<LanguageType> Languages => _languages;
        public int CurrentId => _currentLanguage == null ? -1 : _currentLanguage.Id;

        //public event Action<Localization> EventSwitchLanguage;
        public event Action EventSwitchLanguage;

        private readonly string[] _nameFiles = Enum<TextFiles>.Names;
        private Dictionary<string, string>[] _text;
        private LanguageType[] _languages;
        private int _countFiles;
        private LanguageType _currentLanguage;

        private const string BANNER = "Banner", SLASH = @"/";

        public bool Initialize()
        {
            if (!_folder.EndsWith(SLASH))
                _folder = _folder.Concat(SLASH);

            if (!LoadFromJson(_folder.Concat(_languagesFile), out LanguageType[] lt))
                return false;

            _countFiles = _nameFiles.Length;
            _text = new Dictionary<string, string>[_countFiles];
            _languages = lt;
            return SwitchLanguage(_defaultLang);
        }

        public bool TryIdFromCode(string codeISO639_1, out int id)
        {
            id = -1;
            if (string.IsNullOrEmpty(codeISO639_1))
                return false;

            foreach (LanguageType language in Languages)
            {
                if (language.CodeISO639_1.ToLowerInvariant() == codeISO639_1.ToLowerInvariant())
                {
                    id = language.Id;
                    return true;
                }
            }
            return false;
        }

        public bool LoadFile(TextFiles file)
        {
            int id = (int)file;
            return _loadFiles[id] || (_loadFiles[id] = LoadingFile(id, _currentLanguage));
        }
        public void UnloadFile(TextFiles file)
        {
            int id = (int)file;
            _text[id] = null;
            _loadFiles[id] = false;
        }

        public bool SwitchLanguage(string codeISO639_1)
        {
            if (TryIdFromCode(codeISO639_1, out int id))
                return SwitchLanguage(id);

            return false;
        }

        public bool SwitchLanguage(int id)
        {
            if (_currentLanguage != null && _currentLanguage.Id == id) return true;

            foreach (LanguageType language in Languages)
                if (language.Id == id)
                    return SetLanguage(language);

            return false;
        }

        public string GetText(TextFiles file, string key) => GetText(idFile: (int)file, key);
        public string GetText(TextFiles file, object obj) => GetText(idFile: (int)file, obj.ToString());
        public string GetText(int idFile, string key)
        {
            if (_text[idFile] != null && _text[idFile].TryGetValue(key, out string str))
                return str;

            string msg = $"ERROR! File:[{(TextFiles)idFile} : {_text[idFile] != null}] Key: [{key} : {_text[idFile]?.ContainsKey(key)}]";
            Message.Log(msg);
            return msg;
        }

        public string GetTextFormat(TextFiles file, string key, params object[] args) => string.Format(GetText(idFile: (int)file, key), args);
        public string GetTextFormat(TextFiles file, string key, object arg0, object arg1, object arg2) => string.Format(GetText(idFile: (int)file, key), arg0, arg1, arg2);
        public string GetTextFormat(TextFiles file, string key, object arg0, object arg1) => string.Format(GetText(idFile: (int)file, key), arg0, arg1);
        public string GetTextFormat(TextFiles file, string key, object arg0) => string.Format(GetText(idFile: (int)file, key), arg0);

        private bool SetLanguage(LanguageType type)
        {
            for (int i = 0; i < _countFiles; i++)
            {
                if (!_loadFiles[i])
                    continue;

                if (!LoadingFile(i, type))
                    return false;
            }

            _currentLanguage = type;
            EventSwitchLanguage?.Invoke();
            return true;
        }

        private bool LoadingFile(int idFile, LanguageType type)
        {
            string path = _folder.Concat(type.Folder, _nameFiles[idFile]);
            if (!LoadFromJson(path, out Dictionary<string, string> load))
                return false;

            var current = _text[idFile];
            if (current == null)
            {
                _text[idFile] = new(load, new StringComparer());
                return true;
            }

            foreach (var item in load)
                current[item.Key] = item.Value;

            return true;
        }

        private bool LoadFromJson<T>(string path, out T resource) where T : class
        {
            try
            {
                var textAsset = Resources.Load<TextAsset>(path);
                resource = JsonConvert.DeserializeObject<T>(textAsset.text);
                Resources.UnloadAsset(textAsset);
                return true;
            }
            catch (Exception ex)
            {
                Message.Error($"--- Ошибка загрузки файла локализации: {path} ---\n".Concat(ex.Message));
                resource = null;
                return false;
            }
        }

        #region Nested: LanguageType, StringComparer
        //***********************************************************
        public class LanguageType
        {
            public int Id { get; }
            public string CodeISO639_1 { get; }
            public string Name { get; }
            public string Folder { get; }
            [JsonIgnore]
            public Sprite Sprite { get; }

            [JsonConstructor]
            public LanguageType(int id, string codeISO639_1, string name, string folder)
            {
                Id = id;
                CodeISO639_1 = codeISO639_1;
                Name = name;
                if (!folder.EndsWith(SLASH))
                    folder = folder.Concat(SLASH);
                Folder = folder;
                Sprite = Resources.Load<Sprite>(folder.Concat(BANNER));
            }
        }
        //***********************************************************
        public class StringComparer : IEqualityComparer<string>
        {
            public bool Equals(string str1, string str2)
            {
                return str1.ToLowerInvariant() == str2.ToLowerInvariant();
            }
            public int GetHashCode(string str)
            {
                return str.ToLowerInvariant().GetHashCode();
            }

        }
        #endregion
    }
}
